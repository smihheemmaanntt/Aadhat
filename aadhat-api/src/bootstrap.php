<?php

declare(strict_types=1);

function config(): array
{
    static $config = null;
    if ($config !== null) {
        return $config;
    }

    $configFile = __DIR__ . '/../config/config.php';
    if (!is_file($configFile)) {
        $configFile = __DIR__ . '/../config/config.sample.php';
    }

    $config = require $configFile;
    return $config;
}

function using_sample_config(): bool
{
    return !is_file(__DIR__ . '/../config/config.php');
}

function db(): PDO
{
    static $pdo = null;
    if ($pdo instanceof PDO) {
        return $pdo;
    }

    $db = config()['database'];
    $driver = $db['driver'] ?? 'mysql';

    if ($driver !== 'mysql') {
        throw new RuntimeException('Only mysql driver is enabled for online hybrid mode.');
    }

    $dsn = sprintf(
        'mysql:host=%s;port=%d;dbname=%s;charset=%s',
        $db['host'],
        (int)$db['port'],
        $db['database'],
        $db['charset'] ?? 'utf8mb4'
    );

    $pdo = new PDO($dsn, $db['username'], $db['password'], [
        PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::ATTR_EMULATE_PREPARES => false,
    ]);

    return $pdo;
}

function ensure_schema(PDO $pdo): void
{
    $pdo->exec(
        "CREATE TABLE IF NOT EXISTS hybrid_companies (
            id INT AUTO_INCREMENT PRIMARY KEY,
            company_code VARCHAR(64) NOT NULL UNIQUE,
            company_name VARCHAR(255) NOT NULL,
            mode ENUM('LocalOnly','OnlineOnly','SyncingToOnline','SyncingToLocal','ReadOnlyArchive') NOT NULL DEFAULT 'LocalOnly',
            local_db_path VARCHAR(500) NULL,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"
    );

    $pdo->exec(
        "CREATE TABLE IF NOT EXISTS hybrid_audit_log (
            id BIGINT AUTO_INCREMENT PRIMARY KEY,
            company_code VARCHAR(64) NOT NULL,
            user_name VARCHAR(100) NULL,
            pc_name VARCHAR(100) NULL,
            action VARCHAR(100) NOT NULL,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            INDEX idx_company_created (company_code, created_at)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"
    );

    $pdo->exec(
        "CREATE TABLE IF NOT EXISTS hybrid_transfer_jobs (
            id BIGINT AUTO_INCREMENT PRIMARY KEY,
            company_code VARCHAR(64) NOT NULL,
            direction ENUM('LocalToOnline','OnlineToLocal') NOT NULL,
            status ENUM('Pending','Running','Completed','Failed','Cancelled') NOT NULL DEFAULT 'Pending',
            requested_by VARCHAR(100) NULL,
            requested_pc VARCHAR(100) NULL,
            message TEXT NULL,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            INDEX idx_company_status (company_code, status)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"
    );

    $pdo->exec(
        "CREATE TABLE IF NOT EXISTS hybrid_authorization_keys (
            id BIGINT AUTO_INCREMENT PRIMARY KEY,
            auth_key_hash CHAR(64) NOT NULL UNIQUE,
            customer_code VARCHAR(64) NOT NULL,
            max_devices INT NOT NULL DEFAULT 1,
            status ENUM('Active','Paused','Revoked') NOT NULL DEFAULT 'Active',
            expires_at DATETIME NULL,
            note VARCHAR(255) NULL,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            INDEX idx_customer_status (customer_code, status)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"
    );

    $pdo->exec(
        "CREATE TABLE IF NOT EXISTS hybrid_devices (
            id BIGINT AUTO_INCREMENT PRIMARY KEY,
            auth_key_id BIGINT NOT NULL,
            customer_code VARCHAR(64) NOT NULL,
            device_id_hash CHAR(64) NOT NULL,
            pc_name VARCHAR(100) NULL,
            device_token_hash CHAR(64) NOT NULL,
            status ENUM('Active','Blocked','Released') NOT NULL DEFAULT 'Active',
            activated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            last_seen_at DATETIME NULL,
            UNIQUE KEY uq_auth_device (auth_key_id, device_id_hash),
            INDEX idx_customer_status (customer_code, status),
            CONSTRAINT fk_hybrid_devices_auth_key FOREIGN KEY (auth_key_id)
                REFERENCES hybrid_authorization_keys(id)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"
    );

    $pdo->exec(
        "CREATE TABLE IF NOT EXISTS hybrid_company_access (
            id BIGINT AUTO_INCREMENT PRIMARY KEY,
            customer_code VARCHAR(64) NOT NULL,
            company_code VARCHAR(64) NOT NULL,
            can_access TINYINT(1) NOT NULL DEFAULT 1,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            UNIQUE KEY uq_customer_company (customer_code, company_code)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4"
    );
}

function require_token(): void
{
    $expected = (string)(config()['app_key'] ?? '');
    $header = $_SERVER['HTTP_AUTHORIZATION'] ?? '';
    $token = '';

    if (stripos($header, 'Bearer ') === 0) {
        $token = trim(substr($header, 7));
    }

    if ($expected === '' || !hash_equals($expected, $token)) {
        json_response(['success' => false, 'message' => 'Unauthorized'], 401);
    }
}

function json_input(): array
{
    $body = file_get_contents('php://input');
    if ($body === false || trim($body) === '') {
        return [];
    }

    $data = json_decode($body, true);
    if (!is_array($data)) {
        json_response(['success' => false, 'message' => 'Invalid JSON body'], 400);
    }

    return $data;
}

function audit_event(PDO $pdo, string $companyCode, string $userName, string $pcName, string $action): void
{
    $stmt = $pdo->prepare(
        'INSERT INTO hybrid_audit_log (company_code, user_name, pc_name, action)
         VALUES (:company_code, :user_name, :pc_name, :action)'
    );
    $stmt->execute([
        ':company_code' => $companyCode,
        ':user_name' => $userName,
        ':pc_name' => $pcName,
        ':action' => $action,
    ]);
}

function sha_value(string $value): string
{
    return hash('sha256', trim($value));
}

function random_token(): string
{
    return bin2hex(random_bytes(32));
}

function authorize_device(PDO $pdo, array $input, bool $requireDeviceToken): array
{
    $authorizationKey = trim((string)($input['authorization_key'] ?? ''));
    $deviceId = trim((string)($input['device_id'] ?? ''));
    $deviceToken = trim((string)($input['device_token'] ?? ''));

    if ($authorizationKey === '' || $deviceId === '') {
        json_response(['success' => false, 'message' => 'authorization_key and device_id are required'], 401);
    }

    $stmt = $pdo->prepare(
        "SELECT *
         FROM hybrid_authorization_keys
         WHERE auth_key_hash = :hash
           AND status = 'Active'
           AND (expires_at IS NULL OR expires_at >= NOW())
         LIMIT 1"
    );
    $stmt->execute([':hash' => sha_value($authorizationKey)]);
    $key = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$key) {
        json_response(['success' => false, 'message' => 'Invalid or inactive authorization key'], 401);
    }

    $deviceStmt = $pdo->prepare(
        "SELECT *
         FROM hybrid_devices
         WHERE auth_key_id = :auth_key_id
           AND device_id_hash = :device_id_hash
         LIMIT 1"
    );
    $deviceStmt->execute([
        ':auth_key_id' => $key['id'],
        ':device_id_hash' => sha_value($deviceId),
    ]);
    $device = $deviceStmt->fetch(PDO::FETCH_ASSOC);

    if (!$device) {
        if ($requireDeviceToken) {
            json_response(['success' => false, 'message' => 'Device is not activated'], 401);
        }
        return ['key' => $key, 'device' => null];
    }

    if ($device['status'] !== 'Active') {
        json_response(['success' => false, 'message' => 'Device is not active'], 403);
    }

    if ($requireDeviceToken && ($deviceToken === '' || !hash_equals($device['device_token_hash'], sha_value($deviceToken)))) {
        json_response(['success' => false, 'message' => 'Invalid device token'], 401);
    }

    $pdo->prepare('UPDATE hybrid_devices SET last_seen_at = NOW(), pc_name = :pc_name WHERE id = :id')
        ->execute([
            ':pc_name' => (string)($input['pc_name'] ?? ''),
            ':id' => $device['id'],
        ]);

    return ['key' => $key, 'device' => $device];
}

function json_response(array $data, int $status = 200): void
{
    http_response_code($status);
    header('Content-Type: application/json; charset=utf-8');
    echo json_encode($data, JSON_UNESCAPED_SLASHES);
    exit;
}

function redirect_to(string $path): void
{
    header('Location: ' . $path);
    exit;
}

function h(?string $value): string
{
    return htmlspecialchars((string)$value, ENT_QUOTES, 'UTF-8');
}

function current_base_path(): string
{
    $scriptName = str_replace('\\', '/', dirname($_SERVER['SCRIPT_NAME'] ?? ''));
    return rtrim($scriptName === '/' || $scriptName === '.' ? '' : $scriptName, '/');
}
