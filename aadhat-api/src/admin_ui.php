<?php

declare(strict_types=1);

function admin_handle(string $path, string $method): void
{
    if (session_status() !== PHP_SESSION_ACTIVE) {
        session_start();
    }

    $base = current_base_path();
    $adminBase = $base . '/admin';
    $subPath = trim(substr($path, strlen('admin')), '/');

    if ($subPath === 'logout') {
        $_SESSION = [];
        session_destroy();
        redirect_to($adminBase);
    }

    if (!admin_is_logged_in()) {
        admin_login($adminBase, $method);
        return;
    }

    try {
        if ($method === 'POST') {
            admin_post($subPath, $adminBase);
            return;
        }

        admin_dashboard($adminBase, admin_normalize_page($subPath));
    } catch (Throwable $e) {
        admin_render('Error', $adminBase, '<div class="alert error">' . h($e->getMessage()) . '</div>');
    }
}

function admin_is_logged_in(): bool
{
    return !empty($_SESSION['aadhat_admin']);
}

function admin_login(string $adminBase, string $method): void
{
    $message = '';
    if ($method === 'POST') {
        $token = trim((string)($_POST['api_token'] ?? ''));
        $expected = (string)(config()['app_key'] ?? '');
        if ($expected !== '' && hash_equals($expected, $token)) {
            $_SESSION['aadhat_admin'] = true;
            redirect_to($adminBase);
        }
        $message = '<div class="alert error">Invalid API token.</div>';
    }

    $setup = using_sample_config()
        ? '<div class="alert warn">config.php is missing. Run tools\\setup-config.cmd before opening the dashboard.</div>'
        : '';

    $html = $setup . $message . '
        <form method="post" class="panel login-panel">
            <div class="login-title"><span>🔐</span><div><h2>Owner Login</h2><p class="hint">Use the developer token from server config. Customers do not need this.</p></div></div>
            <label>Owner Password / Developer Token</label>
            <div class="secret-field"><input id="login-api-token" type="password" name="api_token" autocomplete="current-password" autofocus><button class="icon-button" type="button" data-toggle-secret="login-api-token" aria-label="Show or hide API token">👁</button></div>
            <button type="submit"><span class="button-icon">🔓</span> Open Admin</button>
        </form>';

    admin_render('Aadhat Hybrid Admin', $adminBase, $html, false);
}

function admin_post(string $action, string $adminBase): void
{
    $pdo = db();
    ensure_schema($pdo);

    switch ($action) {
        case 'company-save':
            $companyCode = trim((string)($_POST['company_code'] ?? ''));
            $companyName = trim((string)($_POST['company_name'] ?? ''));
            $mode = admin_mode((string)($_POST['mode'] ?? 'LocalOnly'));
            if ($companyCode === '' || $companyName === '') {
                throw new RuntimeException('Company code and name required.');
            }
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_companies (company_code, company_name, mode)
                 VALUES (:company_code, :company_name, :mode)
                 ON DUPLICATE KEY UPDATE company_name = VALUES(company_name), mode = VALUES(mode)"
            );
            $stmt->execute([':company_code' => $companyCode, ':company_name' => $companyName, ':mode' => $mode]);
            $_SESSION['flash'] = 'Company saved.';
            redirect_to($adminBase . '/companies');

        case 'grant-access':
            $customerCode = trim((string)($_POST['customer_code'] ?? ''));
            $companyCode = trim((string)($_POST['company_code'] ?? ''));
            if ($customerCode === '' || $companyCode === '') {
                throw new RuntimeException('Customer code and company code required.');
            }
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_company_access (customer_code, company_code, can_access)
                 VALUES (:customer_code, :company_code, 1)
                 ON DUPLICATE KEY UPDATE can_access = 1"
            );
            $stmt->execute([':customer_code' => $customerCode, ':company_code' => $companyCode]);
            $_SESSION['flash'] = 'Company access granted.';
            redirect_to($adminBase . '/setup');

        case 'revoke-access':
            $customerCode = trim((string)($_POST['customer_code'] ?? ''));
            $companyCode = trim((string)($_POST['company_code'] ?? ''));
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_company_access (customer_code, company_code, can_access)
                 VALUES (:customer_code, :company_code, 0)
                 ON DUPLICATE KEY UPDATE can_access = 0"
            );
            $stmt->execute([':customer_code' => $customerCode, ':company_code' => $companyCode]);
            $_SESSION['flash'] = 'Company access revoked.';
            redirect_to($adminBase . '/setup');

        case 'mode-save':
            $companyCode = trim((string)($_POST['company_code'] ?? ''));
            $mode = admin_mode((string)($_POST['mode'] ?? 'LocalOnly'));
            $stmt = $pdo->prepare('UPDATE hybrid_companies SET mode = :mode WHERE company_code = :company_code');
            $stmt->execute([':mode' => $mode, ':company_code' => $companyCode]);
            $_SESSION['flash'] = 'Company mode changed.';
            redirect_to($adminBase . '/companies');

        case 'key-generate':
            $customerCode = trim((string)($_POST['customer_code'] ?? ''));
            $maxDevices = max(1, (int)($_POST['max_devices'] ?? 1));
            $note = trim((string)($_POST['note'] ?? ''));
            $companyCode = trim((string)($_POST['company_code'] ?? ''));
            $apiBaseUrl = trim((string)($_POST['api_base_url'] ?? ''));
            if ($customerCode === '') {
                throw new RuntimeException('Customer code required.');
            }
            if ($apiBaseUrl === '') {
                $apiBaseUrl = admin_public_base_url();
            }
            $plainKey = 'ADH-' . strtoupper(bin2hex(random_bytes(4))) . '-' . strtoupper(bin2hex(random_bytes(4))) . '-' . strtoupper(bin2hex(random_bytes(4)));
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
            if ($companyCode !== '') {
                $accessStmt = $pdo->prepare(
                    "INSERT INTO hybrid_company_access (customer_code, company_code, can_access)
                     VALUES (:customer_code, :company_code, 1)
                     ON DUPLICATE KEY UPDATE can_access = 1"
                );
                $accessStmt->execute([':customer_code' => $customerCode, ':company_code' => $companyCode]);
            }
            $_SESSION['generated_key'] = $plainKey;
            $_SESSION['setup_package'] = json_encode([
                'ApiBaseUrl' => $apiBaseUrl,
                'ApiToken' => (string)(config()['app_key'] ?? ''),
                'AuthorizationKey' => $plainKey,
                'CompanyCode' => $companyCode,
                'CustomerCode' => $customerCode,
            ], JSON_UNESCAPED_SLASHES | JSON_PRETTY_PRINT);
            $_SESSION['flash'] = $companyCode === ''
                ? 'Authorization key generated. Copy the desktop setup code now.'
                : 'Authorization key generated and company access granted. Copy the desktop setup code now.';
            redirect_to($adminBase . '/setup');

        case 'device-status':
            $deviceId = (int)($_POST['device_id'] ?? 0);
            $status = (string)($_POST['status'] ?? 'Blocked');
            if (!in_array($status, ['Active', 'Blocked', 'Released'], true)) {
                throw new RuntimeException('Invalid device status.');
            }
            $stmt = $pdo->prepare('UPDATE hybrid_devices SET status = :status WHERE id = :id');
            $stmt->execute([':status' => $status, ':id' => $deviceId]);
            $_SESSION['flash'] = 'Device status updated.';
            redirect_to($adminBase . '/devices');

        case 'transfer-create':
            $companyCode = trim((string)($_POST['company_code'] ?? ''));
            $direction = (string)($_POST['direction'] ?? '');
            if (!in_array($direction, ['LocalToOnline', 'OnlineToLocal'], true)) {
                throw new RuntimeException('Invalid transfer direction.');
            }
            $stmt = $pdo->prepare(
                "INSERT INTO hybrid_transfer_jobs (company_code, direction, status, requested_by, requested_pc, message)
                 VALUES (:company_code, :direction, 'Pending', 'Admin UI', :requested_pc, 'Transfer requested from admin UI.')"
            );
            $stmt->execute([
                ':company_code' => $companyCode,
                ':direction' => $direction,
                ':requested_pc' => php_uname('n'),
            ]);
            $_SESSION['flash'] = 'Transfer job created.';
            redirect_to($adminBase . '/transfers');

        default:
            throw new RuntimeException('Unknown action.');
    }

    redirect_to($adminBase);
}

function admin_dashboard(string $adminBase, string $page = 'dashboard'): void
{
    if (using_sample_config()) {
        admin_render('Setup Required', $adminBase, '<div class="alert warn">config.php is missing. Run the setup-config command first.</div>', true, 'dashboard');
        return;
    }

    $pdo = db();
    ensure_schema($pdo);

    $companies = admin_companies($pdo);
    $devices = admin_devices($pdo);
    $transfers = admin_transfers($pdo);
    $nextCompanyCode = next_company_code($pdo);
    $nextCustomerCode = next_customer_code($pdo);
    $defaultCompanyCode = $companies[0]['company_code'] ?? $nextCompanyCode;

    $flash = admin_flash_html();
    $generatedKey = admin_generated_key_html();
    $setupPackage = admin_setup_package_html();

    $onlineCount = 0;
    foreach ($companies as $company) {
        if (($company['mode'] ?? '') === 'OnlineOnly') {
            $onlineCount++;
        }
    }

    $stats = admin_stats_html(count($companies), $onlineCount, count($devices), count($transfers));
    $apiPanel = admin_api_access_html();
    $setupPanel = admin_setup_html($adminBase, $nextCompanyCode, $nextCustomerCode, $defaultCompanyCode);
    $companyPanel = '<section class="panel wide"><h2>Company List</h2>' . admin_companies_table($companies, $adminBase) . '</section>';
    $devicePanel = '<section class="panel wide"><h2>Activated Devices</h2>' . admin_devices_table($devices, $adminBase) . '</section>';
    $transferPanel = admin_transfer_html($adminBase, $defaultCompanyCode) . '<section class="panel wide"><h2>Transfer Jobs</h2>' . admin_table($transfers) . '</section>';

    switch ($page) {
        case 'api-access':
            $title = 'API Access';
            $content = $flash . $apiPanel;
            break;
        case 'setup':
            $title = 'Setup';
            $content = $flash . $generatedKey . $setupPackage . $setupPanel;
            break;
        case 'companies':
            $title = 'Companies';
            $content = $flash . $companyPanel;
            break;
        case 'devices':
            $title = 'Devices';
            $content = $flash . $devicePanel;
            break;
        case 'transfers':
            $title = 'Transfers';
            $content = $flash . $transferPanel;
            break;
        case 'dashboard':
        default:
            $title = 'Dashboard';
            $content = $flash . $stats . '<section class="dashboard-grid">' . $apiPanel . $setupPanel . '</section>';
            $page = 'dashboard';
            break;
    }

    admin_render($title, $adminBase, $content, true, $page);
}

function admin_companies(PDO $pdo): array
{
    return $pdo->query(
        "SELECT c.company_code, c.company_name, c.mode,
            GROUP_CONCAT(CONCAT(a.customer_code, ':', IF(a.can_access = 1, 'Yes', 'No')) ORDER BY a.customer_code SEPARATOR ', ') AS access_list
         FROM hybrid_companies c
         LEFT JOIN hybrid_company_access a ON a.company_code = c.company_code
         GROUP BY c.company_code, c.company_name, c.mode
         ORDER BY c.company_code"
    )->fetchAll(PDO::FETCH_ASSOC);
}

function admin_devices(PDO $pdo): array
{
    return $pdo->query(
        "SELECT d.id, d.customer_code, d.pc_name, d.status, d.activated_at, d.last_seen_at, k.max_devices, k.status AS key_status
         FROM hybrid_devices d
         INNER JOIN hybrid_authorization_keys k ON k.id = d.auth_key_id
         ORDER BY d.activated_at DESC
         LIMIT 25"
    )->fetchAll(PDO::FETCH_ASSOC);
}

function admin_transfers(PDO $pdo): array
{
    return $pdo->query(
        "SELECT id, company_code, direction, status, requested_by, created_at
         FROM hybrid_transfer_jobs
         ORDER BY id DESC
         LIMIT 20"
    )->fetchAll(PDO::FETCH_ASSOC);
}

function admin_flash_html(): string
{
    if (empty($_SESSION['flash'])) {
        return '';
    }
    $flash = '<div class="alert ok">' . h((string)$_SESSION['flash']) . '</div>';
    unset($_SESSION['flash']);
    return $flash;
}

function admin_generated_key_html(): string
{
    if (empty($_SESSION['generated_key'])) {
        return '';
    }
    $generatedKey = '<div class="copy-box"><span>Generated Key</span><strong>' . h((string)$_SESSION['generated_key']) . '</strong></div>';
    unset($_SESSION['generated_key']);
    return $generatedKey;
}

function admin_setup_package_html(): string
{
    if (empty($_SESSION['setup_package'])) {
        return '';
    }
    $id = 'desktop-setup-code';
    $setupPackage = '<section class="panel wide setup-panel">
        <div class="panel-heading"><div><span class="eyebrow">Ready for Desktop</span><h2>Desktop Setup Code</h2><p class="hint">Copy this code into Aadhat desktop and click Start Hybrid on this PC.</p></div><button class="button small-button" type="button" data-copy="' . h($id) . '">📋 Copy</button></div>
        <textarea id="' . h($id) . '" class="setup-code" readonly onclick="this.select()">' . h((string)$_SESSION['setup_package']) . '</textarea>
    </section>';
    unset($_SESSION['setup_package']);
    return $setupPackage;
}

function admin_stats_html(int $companies, int $online, int $devices, int $transfers): string
{
    return '<section class="stats-grid">
        <div class="stat-card"><span class="stat-label">Companies</span><strong>' . h((string)$companies) . '</strong><span class="stat-note">registered</span></div>
        <div class="stat-card"><span class="stat-label">Online Mode</span><strong>' . h((string)$online) . '</strong><span class="stat-note">active companies</span></div>
        <div class="stat-card"><span class="stat-label">Devices</span><strong>' . h((string)$devices) . '</strong><span class="stat-note">latest shown</span></div>
        <div class="stat-card"><span class="stat-label">Transfers</span><strong>' . h((string)$transfers) . '</strong><span class="stat-note">latest jobs</span></div>
    </section>';
}

function admin_api_access_html(): string
{
    $apiToken = (string)(config()['app_key'] ?? '');
    $baseUrl = admin_public_base_url();
    return '<section class="panel api-panel">
        <div><span class="eyebrow">API Access</span><h2>Desktop connection details</h2><p class="hint">Use Generate Setup Code for normal setup. These details are for developer verification.</p></div>
        <div class="api-grid">
            <label>API Base URL<div class="copy-field"><input id="api-base-url" readonly onclick="this.select()" value="' . h($baseUrl) . '"><button class="icon-button" type="button" data-copy="api-base-url" aria-label="Copy API Base URL">📋</button></div></label>
            <label>Health Check<div class="copy-field"><input id="api-health-url" readonly onclick="this.select()" value="' . h($baseUrl . '/api/health') . '"><button class="icon-button" type="button" data-copy="api-health-url" aria-label="Copy health check URL">📋</button></div></label>
            <label>API Token<div class="secret-field"><input id="api-token-field" type="password" readonly onclick="this.select()" value="' . h($apiToken) . '"><button class="icon-button" type="button" data-toggle-secret="api-token-field" aria-label="Show or hide API token">👁</button></div></label>
        </div>
        <div class="endpoint-list">
            <div class="endpoint-card"><span class="method get">GET</span><div><strong>' . h($baseUrl . '/api/health') . '</strong><p>Check that the hybrid API server is running.</p></div></div>
            <div class="endpoint-card"><span class="method post">POST</span><div><strong>' . h($baseUrl . '/api/device/activate') . '</strong><p>Activate a desktop PC using API token and authorization key. It returns the device token.</p></div></div>
            <div class="endpoint-card"><span class="method post">POST</span><div><strong>' . h($baseUrl . '/api/company/status') . '</strong><p>Check selected company access and load its current hybrid mode for the desktop app.</p></div></div>
            <div class="endpoint-card"><span class="method ui">UI</span><div><strong>' . h($baseUrl . '/admin/setup') . '</strong><p>Generate the setup code for Aadhat desktop without manually copying each token.</p></div></div>
        </div>
    </section>';
}

function admin_setup_html(string $adminBase, string $nextCompanyCode, string $nextCustomerCode, string $defaultCompanyCode): string
{
    return '<section class="quick-guide panel">
        <div class="guide-step"><span>1</span><div><strong>Save Company</strong><p>Create one company entry. Unique ID is generated automatically.</p></div></div>
        <div class="guide-step"><span>2</span><div><strong>Generate Desktop Setup</strong><p>Select allowed PCs and generate one setup code for the customer.</p></div></div>
        <div class="guide-step"><span>3</span><div><strong>Start from VB App</strong><p>Paste setup code in Aadhat desktop. Activation and tokens are handled automatically.</p></div></div>
    </section>
    <section class="grid">
        <form method="post" action="' . h($adminBase) . '/company-save" class="panel action-card">
            <div class="card-title"><span>🏢</span><h2>Company</h2></div>
            <label>Company Unique ID</label><input name="company_code" value="' . h($nextCompanyCode) . '" readonly>
            <label>Company Name</label><input name="company_name" placeholder="Demo Company">
            <label>Mode</label>' . admin_mode_select('mode', 'LocalOnly') . '
            <button type="submit"><span class="button-icon">💾</span> Save Company</button>
        </form>
        <form method="post" action="' . h($adminBase) . '/key-generate" class="panel action-card featured-card">
            <div class="card-title"><span>🔐</span><h2>Authorization Key</h2></div>
            <label>Customer Unique ID</label><input name="customer_code" value="' . h($nextCustomerCode) . '" readonly>
            <label>Company Unique ID</label><input name="company_code" value="' . h($defaultCompanyCode) . '">
            <label>Allowed PCs</label><input name="max_devices" type="number" min="1" value="1">
            <label>API URL</label><input name="api_base_url" value="' . h(admin_public_base_url()) . '">
            <label>Note</label><input name="note" placeholder="Customer office">
            <button type="submit"><span class="button-icon">⚡</span> Generate Setup Code</button>
        </form>
        <form method="post" action="' . h($adminBase) . '/grant-access" class="panel action-card">
            <div class="card-title"><span>✅</span><h2>Company Access</h2></div>
            <label>Customer Unique ID</label><input name="customer_code" value="' . h($nextCustomerCode) . '">
            <label>Company Unique ID</label><input name="company_code" value="' . h($defaultCompanyCode) . '">
            <button type="submit"><span class="button-icon">✅</span> Grant Access</button>
        </form>
    </section>';
}

function admin_transfer_html(string $adminBase, string $defaultCompanyCode): string
{
    return '<section class="grid compact-grid">
        <form method="post" action="' . h($adminBase) . '/transfer-create" class="panel action-card">
            <div class="card-title"><span>🔄</span><h2>Create Transfer Job</h2></div>
            <label>Company Unique ID</label><input name="company_code" value="' . h($defaultCompanyCode) . '">
            <label>Direction</label>
            <select name="direction"><option value="LocalToOnline">Local to Web</option><option value="OnlineToLocal">Web to Local</option></select>
            <button type="submit"><span class="button-icon">🔄</span> Create Job</button>
        </form>
    </section>';
}
function admin_mode(string $mode): string
{
    foreach (['LocalOnly', 'OnlineOnly', 'SyncingToOnline', 'SyncingToLocal', 'ReadOnlyArchive'] as $valid) {
        if (strcasecmp($mode, $valid) === 0) {
            return $valid;
        }
    }
    throw new RuntimeException('Invalid company mode.');
}

function admin_mode_select(string $name, string $selected): string
{
    $html = '<select name="' . h($name) . '">';
    foreach (['LocalOnly', 'OnlineOnly', 'SyncingToOnline', 'SyncingToLocal', 'ReadOnlyArchive'] as $mode) {
        $html .= '<option value="' . h($mode) . '"' . ($mode === $selected ? ' selected' : '') . '>' . h($mode) . '</option>';
    }
    return $html . '</select>';
}

function admin_companies_table(array $rows, string $adminBase): string
{
    if (!$rows) return '<p class="empty">No companies registered.</p>';
    $html = '<div class="table-wrap"><table><thead><tr><th>Code</th><th>Name</th><th>Mode</th><th>Access</th><th>Change Mode</th></tr></thead><tbody>';
    foreach ($rows as $row) {
        $html .= '<tr><td data-label="Code">' . h($row['company_code']) . '</td><td data-label="Name">' . h($row['company_name']) . '</td><td data-label="Mode"><span class="pill">' . h($row['mode']) . '</span></td><td data-label="Access">' . h($row['access_list'] ?? '') . '</td><td data-label="Change Mode">
            <form method="post" action="' . h($adminBase) . '/mode-save" class="inline-form">
                <input type="hidden" name="company_code" value="' . h($row['company_code']) . '">' . admin_mode_select('mode', (string)$row['mode']) . '<button>Update</button>
            </form>
        </td></tr>';
    }
    return $html . '</tbody></table></div>';
}

function admin_devices_table(array $rows, string $adminBase): string
{
    if (!$rows) return '<p class="empty">No activated devices.</p>';
    $html = '<div class="table-wrap"><table><thead><tr><th>ID</th><th>Customer</th><th>PC</th><th>Status</th><th>Seen</th><th>Action</th></tr></thead><tbody>';
    foreach ($rows as $row) {
        $html .= '<tr><td data-label="ID">' . h((string)$row['id']) . '</td><td data-label="Customer">' . h($row['customer_code']) . '</td><td data-label="PC">' . h($row['pc_name']) . '</td><td data-label="Status"><span class="pill">' . h($row['status']) . '</span></td><td data-label="Seen">' . h($row['last_seen_at']) . '</td><td data-label="Action">
            <form method="post" action="' . h($adminBase) . '/device-status" class="inline-form">
                <input type="hidden" name="device_id" value="' . h((string)$row['id']) . '">
                <select name="status"><option>Active</option><option>Blocked</option><option>Released</option></select>
                <button>Save</button>
            </form>
        </td></tr>';
    }
    return $html . '</tbody></table></div>';
}

function admin_table(array $rows): string
{
    if (!$rows) return '<p class="empty">No records.</p>';
    $headers = array_keys($rows[0]);
    $html = '<div class="table-wrap"><table><thead><tr>';
    foreach ($headers as $header) $html .= '<th>' . h($header) . '</th>';
    $html .= '</tr></thead><tbody>';
    foreach ($rows as $row) {
        $html .= '<tr>';
        foreach ($headers as $header) $html .= '<td data-label="' . h($header) . '">' . h((string)($row[$header] ?? '')) . '</td>';
        $html .= '</tr>';
    }
    return $html . '</tbody></table></div>';
}

function admin_normalize_page(string $subPath): string
{
    $page = trim($subPath, '/');
    if ($page === '') {
        return 'dashboard';
    }

    $validPages = ['dashboard', 'api-access', 'setup', 'companies', 'devices', 'transfers'];
    return in_array($page, $validPages, true) ? $page : 'dashboard';
}

function admin_nav_class(string $activePage, string $page): string
{
    return $activePage === $page ? 'active' : '';
}

function admin_render(string $title, string $adminBase, string $content, bool $showNav = true, string $activePage = 'dashboard'): void
{
    header('Content-Type: text/html; charset=utf-8');
    echo '<!doctype html><html><head><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1">';
    echo '<title>' . h($title) . '</title><script>(function(){try{if(localStorage.getItem("aadhatSidebarCollapsed")==="1"){document.documentElement.classList.add("sidebar-collapsed");}}catch(e){}})();</script><link rel="stylesheet" href="' . h(current_base_path()) . '/assets/admin.css"></head><body>';
    if ($showNav) {
        echo '<aside class="sidebar">
            <div class="sidebar-top">
                <a class="brand" href="' . h($adminBase) . '/dashboard"><span class="brand-mark">AH</span><strong class="brand-text">Aadhat Hybrid</strong></a>
                <button class="sidebar-toggle" type="button" data-sidebar-toggle aria-label="Hide or show sidebar">☰</button>
            </div>
            <nav class="side-nav">
                <a class="' . admin_nav_class($activePage, 'dashboard') . '" href="' . h($adminBase) . '/dashboard"><span class="nav-icon">📊</span><span class="nav-label">Dashboard</span></a>
                <a class="' . admin_nav_class($activePage, 'api-access') . '" href="' . h($adminBase) . '/api-access"><span class="nav-icon">🔌</span><span class="nav-label">API Access</span></a>
                <a class="' . admin_nav_class($activePage, 'setup') . '" href="' . h($adminBase) . '/setup"><span class="nav-icon">⚙️</span><span class="nav-label">Setup</span></a>
                <a class="' . admin_nav_class($activePage, 'companies') . '" href="' . h($adminBase) . '/companies"><span class="nav-icon">🏢</span><span class="nav-label">Companies</span></a>
                <a class="' . admin_nav_class($activePage, 'devices') . '" href="' . h($adminBase) . '/devices"><span class="nav-icon">💻</span><span class="nav-label">Devices</span></a>
                <a class="' . admin_nav_class($activePage, 'transfers') . '" href="' . h($adminBase) . '/transfers"><span class="nav-icon">🔄</span><span class="nav-label">Transfers</span></a>
            </nav>
        </aside>';
    }
    echo '<section class="' . ($showNav ? 'app-shell' : 'login-shell') . '">';
    echo '<header><div class="header-shell"><div><h1>' . h($title) . '</h1><p>Secure multi-PC access management</p></div>';
    if ($showNav) echo '<details class="profile-menu"><summary><span class="avatar">AD</span><span>Admin</span></summary><div><a href="' . h($adminBase) . '/logout">🚪 Logout</a></div></details>';
    echo '</div></header><main>' . $content . '</main></section><script>
(function(){
    document.querySelectorAll("[data-toggle-secret]").forEach(function(button){
        button.addEventListener("click", function(){
            var field = document.getElementById(button.getAttribute("data-toggle-secret"));
            if (!field) return;
            field.type = field.type === "password" ? "text" : "password";
            button.textContent = field.type === "password" ? "👁" : "🙈";
        });
    });
    var toggle = document.querySelector("[data-sidebar-toggle]");
    if (toggle) {
        toggle.addEventListener("click", function(){
            var collapsed = document.documentElement.classList.toggle("sidebar-collapsed");
            try { localStorage.setItem("aadhatSidebarCollapsed", collapsed ? "1" : "0"); } catch(e) {}
        });
    }
    document.querySelectorAll("[data-copy]").forEach(function(button){
        button.addEventListener("click", function(){
            var field = document.getElementById(button.getAttribute("data-copy"));
            if (!field) return;
            field.focus();
            field.select();
            try {
                navigator.clipboard && navigator.clipboard.writeText(field.value || field.textContent || "");
                button.textContent = "✅";
                setTimeout(function(){ button.textContent = button.classList.contains("small-button") ? "📋 Copy" : "📋"; }, 1200);
            } catch(e) {
                document.execCommand("copy");
            }
        });
    });
})();
</script></body></html>';
    exit;
}

function admin_public_base_url(): string
{
    $https = (!empty($_SERVER['HTTPS']) && $_SERVER['HTTPS'] !== 'off') || ((int)($_SERVER['SERVER_PORT'] ?? 0) === 443);
    $scheme = $https ? 'https' : 'http';
    $host = $_SERVER['HTTP_HOST'] ?? '127.0.0.1:8088';
    return $scheme . '://' . $host . current_base_path();
}

function next_company_code(PDO $pdo): string
{
    for ($i = 0; $i < 20; $i++) {
        $id = (string)random_int(10000000, 99999999);
        $stmt = $pdo->prepare('SELECT 1 FROM hybrid_companies WHERE company_code = :id LIMIT 1');
        $stmt->execute([':id' => $id]);
        if (!$stmt->fetchColumn()) {
            return $id;
        }
    }

    throw new RuntimeException('Unable to generate unique company ID. Please try again.');
}

function next_customer_code(PDO $pdo): string
{
    for ($i = 0; $i < 20; $i++) {
        $id = (string)random_int(10000000, 99999999);
        $stmt = $pdo->prepare('SELECT 1 FROM hybrid_authorization_keys WHERE customer_code = :id LIMIT 1');
        $stmt->execute([':id' => $id]);
        if (!$stmt->fetchColumn()) {
            return $id;
        }
    }

    throw new RuntimeException('Unable to generate unique customer ID. Please try again.');
}

function mask_token(string $token): string
{
    if (strlen($token) <= 12) {
        return str_repeat('•', strlen($token));
    }
    return substr($token, 0, 6) . str_repeat('•', 18) . substr($token, -6);
}


