<?php

declare(strict_types=1);

if (PHP_SAPI !== 'cli') {
    fwrite(STDERR, "This tool can run only from command line.\n");
    exit(1);
}

$host = trim((string)($argv[1] ?? '127.0.0.1'));
$database = trim((string)($argv[2] ?? 'aadhat_hybrid'));
$username = trim((string)($argv[3] ?? 'root'));
$password = (string)($argv[4] ?? '');
$port = (int)($argv[5] ?? 3306);

if ($port <= 0) {
    $port = 3306;
}

$appKey = bin2hex(random_bytes(32));
$config = <<<PHP
<?php

return [
    'app_key' => '{$appKey}',
    'database' => [
        'driver' => 'mysql',
        'host' => '{$host}',
        'port' => {$port},
        'database' => '{$database}',
        'username' => '{$username}',
        'password' => '{$password}',
        'charset' => 'utf8mb4',
    ],
];
PHP;

$path = __DIR__ . '/../config/config.php';
if (is_file($path)) {
    fwrite(STDERR, "config.php already exists. Rename/delete it first if you want to regenerate.\n");
    exit(1);
}

file_put_contents($path, $config);

echo "config.php created.\n";
echo "API Token/app_key:\n";
echo $appKey . "\n";
echo "Use this value in hybrid.json as ApiToken.\n";
