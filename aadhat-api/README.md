# Aadhat Hybrid API

This is the first server-side foundation for hybrid mode.

Single-user desktop mode is unchanged. The VB.NET app should continue using local SQLite unless `hybrid.json` is created and `Enabled` is set to `true`.

## Setup

1. Create `config/config.php` with `tools\setup-config.cmd 127.0.0.1 aadhat_hybrid root ""`.
2. Keep the printed `app_key`; it is the desktop `ApiToken`.
3. Point Apache/XAMPP to `public/index.php` or use this folder under htdocs.
4. Create the MySQL database named in config.
5. Import `database/schema.sql` or let the API create the first tables automatically.
6. Open `/api/health`.

## Initial Endpoints

- `GET /admin`
- `GET /api/health`
- `POST /api/device/activate`
- `POST /api/company/status`

## Admin UI

Open:

```text
http://127.0.0.1:8088/admin
```

Login with the `app_key` / desktop `ApiToken`. From the dashboard you can register companies, generate desktop setup codes, grant company access, change company mode, view/block/release devices, and create Local to Web / Web to Local transfer jobs.

`device/activate` requires:

```json
{
  "authorization_key": "ADH-XXXX-XXXX-XXXX",
  "device_id": "BOARD-SERIAL",
  "pc_name": "OFFICE-PC"
}
```

It returns `device_token`. Save that token in desktop `hybrid.json`.

`company/status` requires an activated device:

```json
{
  "company_code": "1",
  "user_name": "admin",
  "pc_name": "OFFICE-PC",
  "device_id": "BOARD-SERIAL",
  "authorization_key": "ADH-XXXX-XXXX-XXXX",
  "device_token": "returned-device-token"
}
```

Use header:

```text
Authorization: Bearer change-this-secret-token
```

## Company Modes

- `LocalOnly`
- `OnlineOnly`
- `SyncingToOnline`
- `SyncingToLocal`
- `ReadOnlyArchive`

Only one master should accept writes for one company at a time. Online mode writes to server SQL through API. Local mode writes to SQLite.

## Desktop Opt-In

In the desktop app, log in as an Admin user and open `Tools > Hybrid Settings` to import the desktop setup code, activate the PC, and create or edit `hybrid.json` beside `Aadhat.exe`.

Keep `"Enabled": false`, click `Disable Hybrid`, or remove `hybrid.json` for current single-user behavior.

## Developer Authorization

Generate a customer authorization key from web admin or command line. Web admin is easier because it creates one copyable desktop setup code.

```text
php tools/generate_authorization_key.php CUST001 3 "Customer office"
```

This creates one key for `CUST001` with a 3-PC limit. The database stores only the key hash. Share the printed key with the customer once.

Company access is controlled separately in `hybrid_company_access`, so a customer can access only allowed companies inside the single shared server database.

## Easy Admin Commands

```text
tools\aadhat-admin.cmd install-schema
tools\aadhat-admin.cmd register-company 1 "Demo Company" LocalOnly
tools\generate-key.cmd CUST001 3 "Customer office"
tools\aadhat-admin.cmd grant-company CUST001 1
tools\aadhat-admin.cmd set-mode 1 OnlineOnly
tools\aadhat-admin.cmd list-companies
tools\aadhat-admin.cmd list-devices CUST001
```

Full step-by-step process is in `docs/HYBRID_EASY_PROCESS.md`.
