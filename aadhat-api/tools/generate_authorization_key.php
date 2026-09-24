<?php

declare(strict_types=1);

if (PHP_SAPI !== 'cli') {
    fwrite(STDERR, "This tool can run only from command line.\n");
    exit(1);
}

require_once __DIR__ . '/../src/bootstrap.php';

$customerCode = trim((string)($argv[1] ?? ''));
$maxDevices = (int)($argv[2] ?? 1);
$note = trim((string)($argv[3] ?? ''));

if ($customerCode === '') {
    fwrite(STDERR, "Usage: php tools/generate_authorization_key.php CUSTOMER_CODE MAX_DEVICES \"optional note\"\n");
    exit(1);
}

if ($maxDevices < 1) {
    $maxDevices = 1;
}

$plainKey = 'ADH-' . strtoupper(bin2hex(random_bytes(4))) . '-' . strtoupper(bin2hex(random_bytes(4))) . '-' . strtoupper(bin2hex(random_bytes(4)));

$pdo = db();
ensure_schema($pdo);

$stmt = $pdo->prepare(
    "INSERT INTO hybrid_authorization_keys (auth_key_hash, customer_code, max_devices, status, note)
     VALUES (:auth_key_hash, :customer_code, :max_devices, 'Active', :note)"
);
$stmt->execute([
    ':auth_key_hash' => sha_value($plainKey),
    ':customer_code' => $customerCode,
    ':max_devices' => $maxDevices,
    ':note' => $note,
]);

echo "Authorization key generated.\n";
echo "Customer Code: {$customerCode}\n";
echo "Max Devices: {$maxDevices}\n";
echo "Key: {$plainKey}\n";
echo "Store this key now. Only its hash is saved in database.\n";
