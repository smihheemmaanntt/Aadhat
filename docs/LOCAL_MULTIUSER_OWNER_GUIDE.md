# Local Multiuser Owner Guide

Local multiuser setup is handled from the Aadhat desktop app. The developer web panel is not required for local-to-local use.

## What permission is used

The app reads LAN permission from `coreaccess.smx`.

- `LAN: Yes` means local multiuser is allowed.
- `LAN: Yes (1/2)` means 2 PCs are allowed and 1 PC is already registered in the license.
- If the license allows only 1 PC, the app stays single-user.

## Make one PC the Admin PC

1. Open Aadhat on the main PC.
2. Login as Admin.
3. Open `Tools > Local Multiuser Setup`.
4. Click `Use Current Company Folder`.
5. Click `Make This PC Admin`.
6. Share the selected data folder in Windows.

This PC must stay on while client PCs are using the shared company database.

## Connect another PC as Client PC

1. Install/open Aadhat on the client PC.
2. Apply/retrieve the same license so `coreaccess.smx` is available.
3. Open `Tools > Local Multiuser Setup`.
4. Click `Select Data.db`.
5. Select `Data.db` from the Admin PC shared folder.
6. Click `Connect as Client`.
7. Restart Aadhat and open the company.

## Result

Both PCs open the same shared SQLite `Data.db` from the Admin PC folder. Single-user data is not changed unless the owner enables this setup.

