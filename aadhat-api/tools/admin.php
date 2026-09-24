<?php

declare(strict_types=1);

if (PHP_SAPI !== 'cli') {
    fwrite(STDERR, "This tool can run only from command line.\n");
    exit(1);
}

require_once __DIR__ . '/../src/bootstrap.php';

$command = strtolower(trim((string)($argv[1] ?? '')));
$validModes = ['LocalOnly', 'OnlineOnly', 'SyncingToOnline', 'SyncingToLocal', 'ReadOnlyArchive'];

if ($command === '' || in_array($command, ['help', '-h', '--help'], true)) {
    show_help();
    exit(0);
}

$pdo = db();
ensure_schema($pdo);

try {
    switch ($command) {
        case 'install-schema':
            echo "Schema ready.\n";
            break;

        case 'register-company':
            $companyCode = require_arg($argv, 2, 'COMPANY_CODE');
            $companyName = require_arg($argv, 3, 'COMPANY_NAME');
            $mode = normalize_mode($argv[4] ?? 'LocalOnly', $validModes);
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_companies (company_code, company_name, mode)
                 VALUES (:company_code, :company_name, :mode)
                 ON DUPLICATE KEY UPDATE company_name = VALUES(company_name), mode = VALUES(mode)"
            );
            $stmt->execute([
                ':company_code' => $companyCode,
                ':company_name' => $companyName,
                ':mode' => $mode,
            ]);
            echo "Company saved: {$companyCode} / {$companyName} / {$mode}\n";
            break;

        case 'grant-company':
            $customerCode = require_arg($argv, 2, 'CUSTOMER_CODE');
            $companyCode = require_arg($argv, 3, 'COMPANY_CODE');
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_company_access (customer_code, company_code, can_access)
                 VALUES (:customer_code, :company_code, 1)
                 ON DUPLICATE KEY UPDATE can_access = 1"
            );
            $stmt->execute([
                ':customer_code' => $customerCode,
                ':company_code' => $companyCode,
            ]);
            echo "Access granted: {$customerCode} -> {$companyCode}\n";
            break;

        case 'revoke-company':
            $customerCode = require_arg($argv, 2, 'CUSTOMER_CODE');
            $companyCode = require_arg($argv, 3, 'COMPANY_CODE');
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_company_access (customer_code, company_code, can_access)
                 VALUES (:customer_code, :company_code, 0)
                 ON DUPLICATE KEY UPDATE can_access = 0"
            );
            $stmt->execute([
                ':customer_code' => $customerCode,
                ':company_code' => $companyCode,
            ]);
            echo "Access revoked: {$customerCode} -> {$companyCode}\n";
            break;

        case 'set-mode':
            $companyCode = require_arg($argv, 2, 'COMPANY_CODE');
            $mode = normalize_mode(require_arg($argv, 3, 'MODE'), $validModes);
            $stmt = $pdo->prepare('UPDATE hybrid_companies SET mode = :mode WHERE company_code = :company_code');
            $stmt->execute([
                ':mode' => $mode,
                ':company_code' => $companyCode,
            ]);
            echo "Company mode updated: {$companyCode} -> {$mode}\n";
            break;

        case 'list-companies':
            $rows = $pdo->query(
                "SELECT c.company_code, c.company_name, c.mode,
                    GROUP_CONCAT(CONCAT(a.customer_code, ':', IF(a.can_access = 1, 'Y', 'N')) ORDER BY a.customer_code SEPARATOR ', ') AS access_list
                 FROM hybrid_companies c
                 LEFT JOIN hybrid_company_access a ON a.company_code = c.company_code
                 GROUP BY c.company_code, c.company_name, c.mode
                 ORDER BY c.company_code"
            )->fetchAll(PDO::FETCH_ASSOC);
            print_rows($rows);
            break;

        case 'list-devices':
            $customerCode = trim((string)($argv[2] ?? ''));
            $sql = "SELECT d.id, d.customer_code, d.pc_name, d.status, d.activated_at, d.last_seen_at, k.max_devices, k.status AS key_status
                    FROM hybrid_devices d
                    INNER JOIN hybrid_authorization_keys k ON k.id = d.auth_key_id";
            $params = [];
            if ($customerCode !== '') {
                $sql .= " WHERE d.customer_code = :customer_code";
                $params[':customer_code'] = $customerCode;
            }
            $sql .= " ORDER BY d.customer_code, d.activated_at";
            $stmt = $pdo->prepare($sql);
            $stmt->execute($params);
            print_rows($stmt->fetchAll(PDO::FETCH_ASSOC));
            break;

        case 'block-device':
        case 'release-device':
            $deviceRowId = (int)require_arg($argv, 2, 'DEVICE_ROW_ID');
            $status = $command === 'block-device' ? 'Blocked' : 'Released';
            $stmt = $pdo->prepare('UPDATE hybrid_devices SET status = :status WHERE id = :id');
            $stmt->execute([':status' => $status, ':id' => $deviceRowId]);
            echo "Device {$deviceRowId} marked {$status}.\n";
            break;

        case 'request-transfer':
            $companyCode = require_arg($argv, 2, 'COMPANY_CODE');
            $direction = normalize_direction(require_arg($argv, 3, 'DIRECTION'));
            $requestedBy = trim((string)($argv[4] ?? 'Developer'));
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_transfer_jobs (company_code, direction, status, requested_by, requested_pc, message)
                 VALUES (:company_code, :direction, 'Pending', :requested_by, :requested_pc, :message)"
            );
            $stmt->execute([
                ':company_code' => $companyCode,
                ':direction' => $direction,
                ':requested_by' => $requestedBy,
                ':requested_pc' => php_uname('n'),
                ':message' => 'Transfer requested from admin tool.',
            ]);
            echo "Transfer job created: " . $pdo->lastInsertId() . " / {$companyCode} / {$direction}\n";
            break;

        case 'list-transfers':
            $rows = $pdo->query(
                "SELECT id, company_code, direction, status, requested_by, requested_pc, message, created_at, updated_at
                 FROM hybrid_transfer_jobs
                 ORDER BY id DESC"
            )->fetchAll(PDO::FETCH_ASSOC);
            print_rows($rows);
            break;

        default:
            fwrite(STDERR, "Unknown command: {$command}\n\n");
            show_help();
            exit(1);
    }
} catch (Throwable $e) {
    fwrite(STDERR, "Error: " . $e->getMessage() . "\n");
    exit(1);
}

function require_arg(array $argv, int $index, string $name): string
{
    $value = trim((string)($argv[$index] ?? ''));
    if ($value === '') {
        throw new InvalidArgumentException("Missing {$name}.");
    }
    return $value;
}

function normalize_mode(string $mode, array $validModes): string
{
    foreach ($validModes as $validMode) {
        if (strcasecmp($mode, $validMode) === 0) {
            return $validMode;
        }
    }
    throw new InvalidArgumentException('Invalid mode. Use: ' . implode(', ', $validModes));
}

function normalize_direction(string $direction): string
{
    if (strcasecmp($direction, 'LocalToOnline') === 0) {
        return 'LocalToOnline';
    }
    if (strcasecmp($direction, 'OnlineToLocal') === 0) {
        return 'OnlineToLocal';
    }
    throw new InvalidArgumentException('Invalid direction. Use: LocalToOnline or OnlineToLocal');
}

function print_rows(array $rows): void
{
    if (!$rows) {
        echo "No records found.\n";
        return;
    }

    foreach ($rows as $row) {
        echo json_encode($row, JSON_UNESCAPED_SLASHES) . "\n";
    }
}

function show_help(): void
{
    echo "Aadhat Hybrid Admin\n";
    echo "\n";
    echo "Commands:\n";
    echo "  install-schema\n";
    echo "  register-company COMPANY_CODE COMPANY_NAME [MODE]\n";
    echo "  grant-company CUSTOMER_CODE COMPANY_CODE\n";
    echo "  revoke-company CUSTOMER_CODE COMPANY_CODE\n";
    echo "  set-mode COMPANY_CODE MODE\n";
    echo "  list-companies\n";
    echo "  list-devices [CUSTOMER_CODE]\n";
    echo "  block-device DEVICE_ROW_ID\n";
    echo "  release-device DEVICE_ROW_ID\n";
    echo "  request-transfer COMPANY_CODE LocalToOnline|OnlineToLocal [REQUESTED_BY]\n";
    echo "  list-transfers\n";
}
