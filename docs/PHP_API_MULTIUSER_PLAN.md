# Aadhat PHP API Multiuser Plan

Goal: make Aadhat usable from multiple PCs without forcing the existing single-user SQLite setup to migrate immediately. In local-server mode, the database file stays on the server PC local disk. Client PCs do not open the database file directly; they send requests to the PHP API.

Important: current single-user mode must remain unchanged. If `hybrid.json` is missing or `"Enabled": false`, VB.NET app should continue using the existing local SQLite flow exactly as before.

## Architecture

```
Client PC 1 / Client PC 2 / Laptop
        |
   LAN / Wi-Fi / VPN / HTTPS
        |
Server PC with XAMPP/PHP API
        |
Local SQLite Data.db
```

This keeps SQLite safe because only one server process controls database access. Clients never use `\\server\share\Data.db` directly.

## Why This Avoids Conflict

- The SQLite file stays on the server PC local SSD/HDD.
- All writes go through the PHP API and are controlled by transactions.
- Sale, purchase, receipt, and payment saves complete inside one request and one transaction.
- If a save fails, the transaction rolls back.
- Reads and reports return data through API JSON/DataTable responses.
- If Wi-Fi disconnects, the client request can fail, but a remote PC cannot corrupt the database file.

## SQLite Settings On Server

Set these once when API starts or before DB use:

```sql
PRAGMA journal_mode=WAL;
PRAGMA synchronous=NORMAL;
PRAGMA busy_timeout=5000;
PRAGMA temp_store=MEMORY;
PRAGMA wal_autocheckpoint=2000;
```

WAL helps only when:

- The database stays on the server PC local disk.
- Each voucher save is completed in one transaction.
- Antivirus does not repeatedly scan `.db`, `.db-wal`, and `.db-shm` files.
- Long-running write transactions are avoided.

## PHP API Modules

Start with a small API surface:

- `POST /api/login`
- `GET /api/company/current`
- `GET /api/accounts`
- `POST /api/accounts`
- `GET /api/items`
- `POST /api/vouchers/next-number`
- `POST /api/purchase/save`
- `POST /api/sale/save`
- `POST /api/receipt/save`
- `POST /api/payment/save`
- `GET /api/reports/ledger`
- `GET /api/reports/day-book`
- `GET /api/health`

The transaction save APIs must accept full voucher data in one JSON request: header, items, charges, ledger rows, crate rows, stock rows.

## Required PHP API Rules

- Use PDO SQLite.
- Use prepared statements for all user input.
- Use `BEGIN IMMEDIATE` for write APIs.
- Generate bill/voucher number inside the same transaction.
- Commit only after all related rows are saved.
- Rollback on any exception.
- Return clear JSON response:
  - `success`
  - `message`
  - `voucher_id`
  - `bill_no`
  - `server_time`
- Add API token/session authentication.
- Add user ID and PC name in every save log.

## VB.NET Desktop Changes

Do not convert every form at once. Start module-wise.

First layer to add in VB.NET:

- `ApiClient.vb` for HTTP GET/POST.
- DTO classes for Account, Voucher, Transaction rows, Ledger rows.
- Config values:
  - API base URL, for example `http://192.168.1.10/aadhat-api`
  - API token/session
  - local mode/API mode switch if needed.

Then convert one module:

1. Account master
2. Item master
3. Receipt/Payment
4. Purchase/Sale
5. Reports

## Important Conflict Points In Current Code

Current app uses many direct SQLite calls through:

- `Aadhat/Class/clsFun.vb`
- `Aadhat/Class/ClsFunPrimary.vb`
- `Aadhat/Class/ClsFunserver.vb`
- Direct `SQLiteCommand` inside forms

These areas need special care:

- `Select Max(ID)` for account/voucher IDs.
- `Select Max(...) + 1` or count-based bill numbers.
- Delete old voucher rows and reinsert new rows while editing.
- Separate `ExecNonQuery` calls for voucher, ledger, stock, charges.

For multiuser, these must move into a single API transaction.

## LAN Setup

- Server PC: fixed IP, for example `192.168.1.10`.
- Install XAMPP/PHP on server PC.
- Put API under Apache, for example `D:\xampp82\htdocs\aadhat-api`.
- Keep data at local path, for example `D:\AadhatData\Company1\Data.db`.
- Allow Apache/PHP user read-write access to DB folder.
- Open firewall only for Apache port on LAN.
- Clients use API URL: `http://192.168.1.10/aadhat-api`.

## Internet / 10-20 KM Setup

Use the same PHP API, but expose it safely:

- Best simple option: Tailscale/WireGuard VPN.
- Public option: HTTPS domain + login/token + rate limit + firewall.
- Avoid direct public access to SQLite file.
- Avoid exposing DB folder over SMB/shared drive.

## Hybrid Local + Cloud Mode

This mode allows one company to work either from local SQLite or from an online server database, but only one active master should be allowed at a time.

### Mode A: Local Office Server

- Server PC stays on in the office.
- PHP API uses server PC local SQLite `Data.db`.
- LAN/Wi-Fi clients connect to office server API.
- Remote users can connect through VPN/HTTPS to the same office API.

### Mode B: Online Server Master

- Online server has the active company database.
- Local PCs login and sync company access/config.
- While internet is connected, VB.NET app reads/writes through online API.
- Local SQLite can be used as a cache or backup copy, but should not accept independent edits unless offline mode is explicitly enabled.

### Mode C: Shift Company Back To Single PC

When company is moved from online server back to a single PC:

- Stop new writes on online server for that company.
- Download/sync the final server database state to local SQLite.
- Mark company mode as `Local`.
- VB.NET app starts using local SQLite again.
- Online server keeps company disabled/read-only until it is explicitly enabled again.

### Important Rule

Avoid allowing both server database and local SQLite to accept writes for the same company at the same time. If both sides write independently, conflict resolution becomes hard for accounting data because voucher numbers, ledger rows, stock rows, and balances must remain exact.

Recommended company modes:

- `LocalOnly`: one PC or local office server SQLite is active.
- `OnlineOnly`: online SQL database is active.
- `SyncingToOnline`: upload local company to online server, local writes locked during final sync.
- `SyncingToLocal`: download online company to local SQLite, online writes locked during final sync.
- `ReadOnlyArchive`: old copy available only for reports/backup.

### Login-Time Sync

At login, VB.NET app should call API:

- Check license and user access.
- Check which companies this user can open.
- Check company mode: `LocalOnly` or `OnlineOnly`.
- If company is online, use API/server database.
- If company is local, use SQLite path.
- If mode changed, show clear message and switch connection mode.

### Online Database Choice

For online mode, use a server database:

- MySQL/MariaDB is practical with PHP hosting.
- SQL Server is practical on Windows/VPS.

The online database can still sync back to SQLite when shifting to single PC, but all sync must happen through controlled import/export logic.

### One Server Database, Many Companies

Online server should keep one shared SQL database for all companies. Company separation must be done by IDs and access rules:

- `hybrid_companies`: registered companies and current mode.
- `hybrid_company_access`: which customer/user/license can open which company.
- Transaction tables should include `company_code` or `company_id` before online transaction writes are enabled.

Every API query must filter by the authorized company. Never trust company code only from the client; validate it against the activated customer/device.

### Developer Authorization Keys

Direct database access should never be given to client PCs. API access should also require authorization:

- A global API token protects the API installation.
- A developer-generated authorization key binds a customer/license to a max PC count.
- Each PC activates once with motherboard/device ID.
- Server returns a device token after activation.
- Later API calls require authorization key + device token + device ID.
- If max PC limit is reached, activation is refused.
- Developer can pause/revoke key or block/release a device from server database/admin panel.

This allows selling/allowing exactly as many PCs as needed for a customer.

### Sync Conflict Policy

For accounting safety, recommended policy is not auto-merge for the same active company. Use ownership/lock:

- Online master active: all writes go online.
- Local master active: all writes go local.
- During transfer: writes locked until transfer completes.

This keeps voucher numbers, ledgers, stock, crates, receipts, and payments consistent.

## Testing Before Production

- Run two clients and save receipt at the same time.
- Run two clients and save purchase/sale bill at the same time.
- Confirm no duplicate bill/voucher number.
- Kill one client during save and confirm no partial rows.
- Open reports while another client is saving.
- Test LAN cable and Wi-Fi clients together.
- Test API health after server restart.
- Test backup while users are logged in.

## First Build Step

Build a small PHP API proof of concept:

- `/api/health`
- `/api/accounts`
- `/api/receipt/save`

Then modify only one VB.NET form to use API mode. Once that works reliably from two PCs, continue module by module.

## Started Foundation

- Added opt-in VB.NET hybrid runtime class under `Aadhat/Class/Hybrid/HybridRuntime.vb`.
- Added `hybrid.example.json`. This is only an example; active app behavior changes only after a real `hybrid.json` is created beside `Aadhat.exe` and enabled.
- Added PHP API skeleton under `aadhat-api`.
- Added MySQL setup schema under `aadhat-api/database/schema.sql`.
- Added guarded login-time company status hook. It runs only when `HybridRuntime.IsEnabled` is true.
- Added Web Admin UI under `/admin`.
- Added VB.NET `HybridSettingsForm` base. It is not wired to the menu yet, so it does not affect normal users.

Next coding step:

- Add a small Hybrid Settings screen or admin-only menu to create/update `hybrid.json`.
- Add online transaction module implementation after API save endpoints are ready.
- Keep the existing local login path as the default fallback.
