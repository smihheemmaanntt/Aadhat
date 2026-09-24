<?php

require_once __DIR__ . '/../src/bootstrap.php';
require_once __DIR__ . '/../src/admin_ui.php';

$method = $_SERVER['REQUEST_METHOD'] ?? 'GET';
$path = parse_url($_SERVER['REQUEST_URI'] ?? '/', PHP_URL_PATH);
$scriptName = str_replace('\\', '/', dirname($_SERVER['SCRIPT_NAME'] ?? ''));

if ($scriptName !== '/' && $scriptName !== '.') {
    $path = preg_replace('#^' . preg_quote($scriptName, '#') . '#', '', $path);
}

$path = trim($path, '/');
if (strpos($path, 'api/') === 0) {
    $path = substr($path, 4);
}

try {
    if ($path === 'admin' || strpos($path, 'admin/') === 0) {
        admin_handle($path, $method);
    }

    if ($method === 'GET' && $path === 'health') {
        json_response([
            'success' => true,
            'message' => 'Aadhat hybrid API is running',
            'server_time' => date('Y-m-d H:i:s'),
        ]);
    }

    require_token();

    if ($method === 'POST' && $path === 'device/activate') {
        $input = json_input();
        $pdo = db();
        ensure_schema($pdo);

        $auth = authorize_device($pdo, $input, false);
        $key = $auth['key'];
        $device = $auth['device'];

        if ($device) {
            json_response([
                'success' => true,
                'message' => 'Device already activated',
                'device_token' => '',
                'max_devices' => (int)$key['max_devices'],
                'active_devices' => active_device_count($pdo, (int)$key['id']),
                'server_time' => date('Y-m-d H:i:s'),
            ]);
        }

        $activeDevices = active_device_count($pdo, (int)$key['id']);
        if ($activeDevices >= (int)$key['max_devices']) {
            json_response([
                'success' => false,
                'message' => 'Device limit reached for this authorization key',
                'max_devices' => (int)$key['max_devices'],
                'active_devices' => $activeDevices,
                'server_time' => date('Y-m-d H:i:s'),
            ], 403);
        }

        $deviceToken = random_token();
        $stmt = $pdo->prepare(
            "INSERT INTO hybrid_devices
                (auth_key_id, customer_code, device_id_hash, pc_name, device_token_hash, status, last_seen_at)
             VALUES
                (:auth_key_id, :customer_code, :device_id_hash, :pc_name, :device_token_hash, 'Active', NOW())"
        );
        $stmt->execute([
            ':auth_key_id' => $key['id'],
            ':customer_code' => $key['customer_code'],
            ':device_id_hash' => sha_value((string)$input['device_id']),
            ':pc_name' => (string)($input['pc_name'] ?? ''),
            ':device_token_hash' => sha_value($deviceToken),
        ]);

        json_response([
            'success' => true,
            'message' => 'Device activated',
            'device_token' => $deviceToken,
            'max_devices' => (int)$key['max_devices'],
            'active_devices' => $activeDevices + 1,
            'server_time' => date('Y-m-d H:i:s'),
        ]);
    }

    if ($method === 'POST' && $path === 'company/status') {
        $input = json_input();
        $companyCode = trim((string)($input['company_code'] ?? ''));
        $userName = trim((string)($input['user_name'] ?? ''));

        if ($companyCode === '') {
            json_response(['success' => false, 'message' => 'company_code is required'], 422);
        }

        $pdo = db();
        ensure_schema($pdo);
        $auth = authorize_device($pdo, $input, true);
        $customerCode = (string)$auth['key']['customer_code'];

        if (!can_access_company($pdo, $customerCode, $companyCode)) {
            json_response([
                'success' => false,
                'message' => 'This customer is not authorized for selected company',
                'company_code' => $companyCode,
                'mode' => 'LocalOnly',
                'server_time' => date('Y-m-d H:i:s'),
            ], 403);
        }

        $stmt = $pdo->prepare(
            'SELECT company_code, company_name, mode, local_db_path
             FROM hybrid_companies
             WHERE company_code = :company_code
             LIMIT 1'
        );
        $stmt->execute([':company_code' => $companyCode]);
        $company = $stmt->fetch(PDO::FETCH_ASSOC);

        if (!$company) {
            json_response([
                'success' => false,
                'message' => 'Company not registered on hybrid server',
                'company_code' => $companyCode,
                'mode' => 'LocalOnly',
                'server_time' => date('Y-m-d H:i:s'),
            ], 404);
        }

        audit_event($pdo, $companyCode, $userName, (string)($input['pc_name'] ?? ''), 'company.status');

        json_response([
            'success' => true,
            'message' => 'Company status loaded',
            'company_code' => $company['company_code'],
            'company_name' => $company['company_name'],
            'mode' => $company['mode'],
            'local_db_path' => $company['local_db_path'],
            'server_time' => date('Y-m-d H:i:s'),
        ]);
    }

    json_response(['success' => false, 'message' => 'Route not found'], 404);
} catch (Throwable $e) {
    json_response([
        'success' => false,
        'message' => 'Server error',
        'error' => $e->getMessage(),
    ], 500);
}

function active_device_count(PDO $pdo, int $authKeyId): int
{
    $stmt = $pdo->prepare(
        "SELECT COUNT(*)
         FROM hybrid_devices
         WHERE auth_key_id = :auth_key_id
           AND status = 'Active'"
    );
    $stmt->execute([':auth_key_id' => $authKeyId]);
    return (int)$stmt->fetchColumn();
}

function can_access_company(PDO $pdo, string $customerCode, string $companyCode): bool
{
    $stmt = $pdo->prepare(
        "SELECT can_access
         FROM hybrid_company_access
         WHERE customer_code = :customer_code
           AND company_code = :company_code
         LIMIT 1"
    );
    $stmt->execute([
        ':customer_code' => $customerCode,
        ':company_code' => $companyCode,
    ]);
    $value = $stmt->fetchColumn();
    return $value !== false && (int)$value === 1;
}
