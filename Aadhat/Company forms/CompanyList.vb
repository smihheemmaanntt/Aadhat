Imports System.Data.SQLite
Imports System.IO

Public Class CompanyList
    Dim rs As New Resizer
    Dim root As String = Application.StartupPath
    ' Public Shared filepath As String = String.Empty
    Public newconnection As String = ""

    Private Sub CompanyList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtMainPath.Text = ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path")
        rowColums() : getCompanies()
        rs.FindAllControls(Me)
    End Sub

    Public Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Company Name" : dg1.Columns(1).Width = 300
        dg1.Columns(2).Name = "Address" : dg1.Columns(2).Width = 270
        dg1.Columns(3).Name = "City" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Finacial Year Start" : dg1.Columns(4).Width = 90
        dg1.Columns(5).Name = "Finacial Year End" : dg1.Columns(5).Width = 90
        dg1.Columns(6).Name = "FYID" : dg1.Columns(6).Visible = False
        dg1.Columns(7).Name = "DbPath" : dg1.Columns(7).Visible = False
        dg1.Columns(8).Name = "PrvYrPath" : dg1.Columns(8).Visible = False
        ' retrive()
    End Sub
    Public Sub getCompanies()
        Dim Connectionstring As String = String.Empty
        dg1.Rows.Clear()
        Dim i As Integer = 1 ' NewCompanies
        If txtMainPath.Text.ToUpper = ("Data").ToUpper Or txtMainPath.Text.ToUpper = "" Then
            root = root
        Else
            root = txtMainPath.Text
        End If

        For Each sDir In Directory.GetDirectories(root, "Data", SearchOption.AllDirectories)
            For Each FilePath In Directory.GetFiles(sDir, "*data*.db", SearchOption.AllDirectories)
                Application.DoEvents()
                Dim detailedfile As New IO.FileInfo(FilePath)
                Dim dt As New DataTable
                Dim cmdText As String = "Select * from Company"
                If txtMainPath.Text.ToUpper = ("Data").ToUpper Then
                    Connectionstring = "Data Source=|DataDirectory|" & FilePath.ToString & ";Version=3;New=True;Compress=True;synchronous=ON;"
                Else
                    Connectionstring = "Data Source=" & FilePath.ToString & ";Version=3;New=True;Compress=True;synchronous=ON;"
                End If

                Dim con As New SQLite.SQLiteConnection(Connectionstring)
                Dim ad As New SQLiteDataAdapter(cmdText, con)
                ad.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dg1.Rows.Add()
                    With dg1.Rows(i - 1)
                        .Cells(0).Value = dt.Rows(0)("id").ToString()
                        .Cells(1).Value = dt.Rows(0)("CompanyName").ToString()
                        .Cells(2).Value = dt.Rows(0)("Address").ToString()
                        .Cells(3).Value = dt.Rows(0)("City").ToString()
                        .Cells(6).Value = dt.Rows(0)("id").ToString()
                        .Cells(4).Value = CDate(dt.Rows(0)("YearStart")).ToString("dd-MM-yyyy")
                        .Cells(5).Value = CDate(dt.Rows(0)("YearEnd")).ToString("dd-MM-yyyy")
                        .Cells(7).Value = FilePath.ToString
                    End With
                    i = i + 1
                End If
                '  dg1.Rows.Add("", detailedfile.Name, FilePath)
            Next
        Next
    End Sub
    Private Sub UpdateLoose()
        'LoosePurchase Table
        Dim sql As String = "CREATE TABLE if not exists PurchaseLoose (ID INTEGER PRIMARY KEY AUTOINCREMENT,EntryDate DATE,TransType TEXT, VoucherID INTEGER," &
            "BillNo TEXT,VehicleNo TEXT,AccountID INTEGER,AccountName TEXT,ItemID INTEGER,ItemName TEXT,Nug  DECIMAL,Weight DECIMAL,Rate DECIMAL," &
            "Per TEXT,Amount DECIMAL,MaintainCrate TEXT,CrateID INTEGER,CrateName TEXT,CrateQty DECIMAL,Remark TEXT,Roundoff DECIMAL);"
        clsFun.ExecNonQuery(sql)
        ''LooseSale Table
        sql = ""
        sql = "CREATE TABLE IF NOT exists Transaction4(ID INTEGER PRIMARY KEY AUTOINCREMENT,EntryDate DATE,VoucherID INTEGER,TransType TEXT,BillNo TEXT,AccountID INTEGER,AccountName TEXT,ItemID INTEGER," &
            " ItemName TEXT,Nug DECIMAL,Weight DECIMAL,Rate DECIMAL,Per TEXT,Amount DECIMAL,Charges DECIMAL,TotalAmount DECIMAL,CommPer DECIMAL,CommAmt DECIMAL,MPer DECIMAL,MAmt DECIMAL," &
            " RdfPer DECIMAL,RdfAmt DECIMAL,Tare DECIMAL,TareAmt DECIMAL,labour  DECIMAL,LabourAmt DECIMAL,MaintainCrate TEXT,CrateID DECIMAL,Cratemarka TEXT,CrateQty DECIMAL,CrateRate DECIMAL, " &
            " CrateAmount DECIMAL,PurchaseID INTEGER,RoundOff DECIMAL,CrateAccountID INTEGER,CrateAccountName TEXT,OnWeight TEXT);"
        clsFun.ExecNonQuery(sql)
    End Sub
    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        UpdateLedgerTable() : ReceiptUpdate() : UpdateVoucher()
        If clsFun.CheckIfColumnExists("Vouchers", "InvoiceID") = False Then
            MsgBox("Update Database.... Contact to Service Partner...") : Exit Sub
        End If
        If clsFun.CheckIfColumnExists("CrateVoucher", "Amount") = False Then UpdateCrateTable()
        If clsFun.CheckIfColumnExists("Company", "LinkedDB") = False Then updateCompanytbl()
        If clsFun.CheckIfColumnExists("Transaction2", "CrateRate") = False Then
            Dim sql As String = "ALTER TABLE Transaction2 ADD COLUMN CrateRate DECIMAL;"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfColumnExists("Transaction2", "GrossWeight") = False Then
            Dim sql As String = "ALTER TABLE Transaction2 ADD COLUMN GrossWeight DECIMAL;"
            clsFun.ExecNonQuery(sql)
        End If
        UpdateVoucherTbl() : CreateOptions() : Transaction1() : UpdateAccountGroups()
        UserRights() : UpdateView() : DbpFile() : ServerDb() : UpdateLoose()
        CompanyInfo() : OptionsControl()
        Login.MdiParent = ShowCompanies
        Login.Show()
        If Not Login Is Nothing Then
            Login.BringToFront()
        End If
    End Sub
    Private Sub DbpFile()
        If ClsFunPrimary.CheckIfColumnExists("Printing", "M10") = False Then
            Dim sql As String = String.Empty
            sql = "Drop table Printing;CREATE TABLE Printing (D1 TEXT,D2 TEXT,M1 TEXT,M2  TEXT,M3 TEXT,M4  TEXT,M5 TEXT,M6  TEXT,P1 TEXT,P2  TEXT,P3 TEXT,P4  TEXT,P5 TEXT,P6 TEXT," _
                & "P7 TEXT,P8  TEXT,P9 TEXT,P10 TEXT,P11 TEXT,P12 TEXT,P13 TEXT,P14 TEXT,P15 TEXT,P16 TEXT,P17 TEXT,P18 TEXT,P19 TEXT,P20 TEXT,T1 TEXT," _
                & "T2  TEXT,T3  TEXT,T4  TEXT,T5  TEXT,T6  TEXT,T7  TEXT,T8  TEXT,T9  TEXT,T10 TEXT,P21 TEXT,P22 TEXT,P23 TEXT,P24 TEXT,P25 TEXT,P26 TEXT," _
                & "P27 TEXT,P28 TEXT,P29 TEXT,P30 TEXT,P31 TEXT,P32 TEXT,P33 TEXT,P34 TEXT,P35 TEXT,P36 TEXT,P37 TEXT,P38 TEXT,P39 TEXT,P40 TEXT,P41 TEXT," _
                & "P42 TEXT,P43 TEXT,P44 TEXT,P45 TEXT,P46 TEXT,P47 TEXT,P48 TEXT,P49 TEXT,P50 TEXT,P51 TEXT,P52 TEXT,P53 TEXT,P54 TEXT,P55 TEXT,P56 TEXT," _
                & "P57 TEXT,P58 TEXT,P59 TEXT,P60 TEXT,P61 TEXT,P62 TEXT,P63 TEXT,P64 TEXT,P65 TEXT,P66 TEXT,P67 TEXT,P68 TEXT,P69 TEXT,P70 TEXT,P71 TEXT," _
                & "P72 TEXT,P73 TEXT,P74 TEXT,P75 TEXT,P76 TEXT,P77 TEXT,P78 TEXT,P79 TEXT,P80 TEXT,P81 TEXT,P82 TEXT,M7  TEXT,M8  TEXT,M9  TEXT,M10 TEXT);"
            ClsFunPrimary.ExecNonQuery(sql)
        End If
        If ClsFunPrimary.CheckIfColumnExists("Printing", "P83") = False Then
            Dim sql As String = String.Empty
            sql = "Drop table Printing;CREATE TABLE Printing (D1 TEXT,D2 TEXT,P1 TEXT,P2  TEXT,P3 TEXT,P4  TEXT,P5 TEXT,P6 TEXT," _
                & "P7 TEXT,P8  TEXT,P9 TEXT,P10 TEXT,P11 TEXT,P12 TEXT,P13 TEXT,P14 TEXT,P15 TEXT,P16 TEXT,P17 TEXT,P18 TEXT,P19 TEXT,P20 TEXT,T1 TEXT," _
                & "T2  TEXT,T3  TEXT,T4  TEXT,T5  TEXT,T6  TEXT,T7  TEXT,T8  TEXT,T9  TEXT,T10 TEXT,P21 TEXT,P22 TEXT,P23 TEXT,P24 TEXT,P25 TEXT,P26 TEXT," _
                & "P27 TEXT,P28 TEXT,P29 TEXT,P30 TEXT,P31 TEXT,P32 TEXT,P33 TEXT,P34 TEXT,P35 TEXT,P36 TEXT,P37 TEXT,P38 TEXT,P39 TEXT,P40 TEXT,P41 TEXT," _
                & "P42 TEXT,P43 TEXT,P44 TEXT,P45 TEXT,P46 TEXT,P47 TEXT,P48 TEXT,P49 TEXT,P50 TEXT,P51 TEXT,P52 TEXT,P53 TEXT,P54 TEXT,P55 TEXT,P56 TEXT," _
                & "P57 TEXT,P58 TEXT,P59 TEXT,P60 TEXT,P61 TEXT,P62 TEXT,P63 TEXT,P64 TEXT,P65 TEXT,P66 TEXT,P67 TEXT,P68 TEXT,P69 TEXT,P70 TEXT,P71 TEXT," _
                & "P72 TEXT,P73 TEXT,P74 TEXT,P75 TEXT,P76 TEXT,P77 TEXT,P78 TEXT,P79 TEXT,P80 TEXT,P81 TEXT,P82 TEXT,P83 TEXT,P84 TEXT,P85 TEXT,P86 TEXT, " _
                & "M1 TEXT,M2  TEXT,M3 TEXT,M4  TEXT,M5 TEXT,M6  TEXT,M7  TEXT,M8  TEXT,M9  TEXT,M10 TEXT);"
            ClsFunPrimary.ExecNonQuery(sql)
        End If

        If ClsFunPrimary.CheckIfColumnExists("Printing", "M11") = False Then
            Dim sql As String = String.Empty
            sql = "Drop table Printing;CREATE TABLE Printing (D1 TEXT,D2 TEXT,P1 TEXT,P2  TEXT,P3 TEXT,P4  TEXT,P5 TEXT,P6 TEXT," _
                & "P7 TEXT,P8  TEXT,P9 TEXT,P10 TEXT,P11 TEXT,P12 TEXT,P13 TEXT,P14 TEXT,P15 TEXT,P16 TEXT,P17 TEXT,P18 TEXT,P19 TEXT,P20 TEXT,T1 TEXT," _
                & "T2  TEXT,T3  TEXT,T4  TEXT,T5  TEXT,T6  TEXT,T7  TEXT,T8  TEXT,T9  TEXT,T10 TEXT,P21 TEXT,P22 TEXT,P23 TEXT,P24 TEXT,P25 TEXT,P26 TEXT," _
                & "P27 TEXT,P28 TEXT,P29 TEXT,P30 TEXT,P31 TEXT,P32 TEXT,P33 TEXT,P34 TEXT,P35 TEXT,P36 TEXT,P37 TEXT,P38 TEXT,P39 TEXT,P40 TEXT,P41 TEXT," _
                & "P42 TEXT,P43 TEXT,P44 TEXT,P45 TEXT,P46 TEXT,P47 TEXT,P48 TEXT,P49 TEXT,P50 TEXT,P51 TEXT,P52 TEXT,P53 TEXT,P54 TEXT,P55 TEXT,P56 TEXT," _
                & "P57 TEXT,P58 TEXT,P59 TEXT,P60 TEXT,P61 TEXT,P62 TEXT,P63 TEXT,P64 TEXT,P65 TEXT,P66 TEXT,P67 TEXT,P68 TEXT,P69 TEXT,P70 TEXT,P71 TEXT," _
                & "P72 TEXT,P73 TEXT,P74 TEXT,P75 TEXT,P76 TEXT,P77 TEXT,P78 TEXT,P79 TEXT,P80 TEXT,P81 TEXT,P82 TEXT,P83 TEXT,P84 TEXT,P85 TEXT,P86 TEXT, " _
                & "M1 TEXT,M2  TEXT,M3 TEXT,M4  TEXT,M5 TEXT,M6  TEXT,M7  TEXT,M8  TEXT,M9  TEXT,M10 TEXT,M11 TEXT,M12  TEXT,M13 TEXT,M14  TEXT,M15 TEXT,M16  TEXT,M17  TEXT,M18  TEXT,M19  TEXT,M20 TEXT);"
            ClsFunPrimary.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfViewColumnExists("BillPrints", "MobileNo2") = False Then
            Dim sql As String = String.Empty
            sql = "Drop View if  exists BillPrints;CREATE VIEW BillPrints AS SELECT Transaction2.EntryDate, Items.ItemName, Accounts.AccountName, Transaction2.Nug, Transaction2.Weight, Transaction2.Rate," & _
         "Transaction2.Per, Transaction2.Amount, Transaction2.Charges, Transaction2.TotalAmount, Transaction2.CommPer, Transaction2.CommAmt," & _
         "Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, Transaction2.Tare, Transaction2.Tareamt," & _
         "Transaction2.Labour, Transaction2.LabourAmt, Transaction2.MaintainCrate, Transaction2.Cratemarka, Transaction2.CrateQty," & _
         "Items.OtherName, Accounts.Othername as AccountNameOther,Accounts.Mobile1 AS MobileNo1,Accounts.Mobile2 AS MobileNo2,Accounts.LFNo as LFNo, Accounts.Area as Area,Accounts.City, Transaction2.TransType, Transaction2.AccountID,ifnull(Transaction2.OnWeight,'') as OnWeight,ifnull(Transaction2.Lot,'') as Lot,ifnull(Transaction2.Cut,'') as Cut " & _
         "FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID " & _
         "WHERE (((Transaction2.TransType) Not In ('Standard Sale','On Sale','Store Out','Store Transfer'))) ORDER BY Transaction2.AccountName"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfViewColumnExists("BillPrints", "GrossWeight") = False Then
            Dim sql As String = String.Empty
            sql = "Drop View if  exists BillPrints;CREATE VIEW BillPrints AS SELECT Transaction2.EntryDate, Items.ItemName, Accounts.AccountName, Transaction2.Nug, Transaction2.Weight, Transaction2.Rate," & _
         "Transaction2.Per, Transaction2.Amount, Transaction2.Charges, Transaction2.TotalAmount, Transaction2.CommPer, Transaction2.CommAmt," & _
         "Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, Transaction2.Tare, Transaction2.Tareamt," & _
         "Transaction2.Labour, Transaction2.LabourAmt, Transaction2.MaintainCrate, Transaction2.Cratemarka, Transaction2.CrateQty," & _
         "Items.OtherName, Accounts.Othername as AccountNameOther,Accounts.Mobile1 AS MobileNo1,Accounts.Mobile2 AS MobileNo2,Accounts.LFNo as LFNo, Accounts.Area as Area,Accounts.City, Transaction2.TransType, Transaction2.AccountID,ifnull(Transaction2.OnWeight,'') as OnWeight,ifnull(Transaction2.Lot,'') as Lot,ifnull(Transaction2.Cut,'') as Cut,Transaction2.GrossWeight as GrossWeight " & _
         "FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID " & _
         "WHERE (((Transaction2.TransType) Not In ('Standard Sale','On Sale','Store Out','Store Transfer'))) ORDER BY Transaction2.AccountName"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfViewColumnExists("Stock_Sale_Report", "PBill") = False Then
            Dim sql As String = String.Empty
            sql = "Drop View Stock_Sale_Report;CREATE VIEW Stock_Sale_Report AS  SELECT v.VehicleNo AS VehicleNo, V.id AS VoucherID,V.entryDate,v.billNo AS BillNo, v.SallerID AS SallerID,v.sallerName AS SallerName," & _
                  " t.ItemName AS ItemName,t.Lot AS lot, t.accountName AS AccountName, t.nug AS nug, t.Weight AS weight, t.rate AS rate, t.per AS per, t.Amount AS amount, t.Charges AS charges,t.TotalAmount AS totalAmount," & _
                  " t.PurchaseID as PurchaseID,(Select BillNo From Vouchers Where ID=t.PurchaseID) as PBill, v.TransType AS transtype FROM Vouchers v INNER JOIN Transaction2 t ON v.id = t.VoucherID"
            clsFun.ExecNonQuery(sql)
        End If
        ClsFunPrimary.ExecScalarStr("Create  table if Not Exists API (InstanceID TEXT,SendingMethod TEXT,LanguageType TEXT,SendingType TEXT)")
        clsFun.ExecScalarStr("Create  table if Not Exists WaReport(EntryDate DATE,AccountName TEXT,WhatsAppNo TEXT,Type TEXT,Status TEXT)")
        If ClsFunPrimary.CheckIfColumnExists("API", "GAP1") = False Then
            Dim sql As String = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempAPI AS SELECT * FROM API;DROP TABLE API;CREATE TABLE API (InstanceID TEXT,SendingMethod TEXT,LanguageType TEXT,SendingType TEXT," & _
                "GAP1 INTEGER,GAP2 INTEGER);INSERT INTO API(InstanceID,SendingMethod,LanguageType,SendingType)SELECT InstanceID,SendingMethod,LanguageType,SendingType FROM TempAPI; DROP TABLE TempAPI;PRAGMA foreign_keys = 1;"
            ClsFunPrimary.ExecNonQuery(sql)
        End If
    End Sub
    Private Sub UpdateLedgerTable()

        If clsFun.CheckIfColumnExists("Ledger", "RemarkHindi") = False Then
            Dim sql As String = String.Empty
            sql = "drop table if  exists sqlitestudio_temp_table;PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS SELECT * FROM Ledger;" & _
                "DROP TABLE Ledger;CREATE TABLE Ledger (VourchersID INTEGER,EntryDate DATE,TransType TEXT,AccountID INTEGER,AccountName TEXT,Amount DECIMAL, " & _
                "DC TEXT,Remark TEXT,Narration TEXT,RemarkHindi TEXT);" & _
                "INSERT INTO Ledger (VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration) " & _
                "SELECT VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration FROM sqlitestudio_temp_table;" & _
                "DROP TABLE sqlitestudio_temp_table;CREATE INDEX AccountIndex ON Ledger (AccountID);PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfColumnExists("Ledger", "PartyID") = False Then
            Dim sql As String = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempLedger AS SELECT * FROM Ledger;DROP TABLE Ledger;" & _
                "CREATE TABLE Ledger (VourchersID INTEGER,EntryDate DATE,TransType TEXT,AccountID INTEGER,AccountName TEXT," & _
                "Amount DECIMAL,DC TEXT,Remark TEXT,Narration TEXT,RemarkHindi TEXT,PartyID INTEGER,PartyName TEXT);" & _
                "INSERT INTO Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount," & _
                "DC,Remark,Narration,RemarkHindi)SELECT VourchersID,EntryDate,TransType,AccountID,AccountName,Amount," & _
                "DC,Remark,Narration,RemarkHindi FROM TempLedger;DROP TABLE TempLedger;" & _
                "CREATE INDEX AccountIndex ON Ledger(AccountID,AccountName);PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If

        If clsFun.CheckIfColumnExists("Ledger", "PostingID") = False Then
            Dim sql As String = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempLedger AS SELECT * FROM Ledger;DROP TABLE Ledger;" & _
                "CREATE TABLE Ledger (VourchersID INTEGER,EntryDate DATE,TransType TEXT,AccountID INTEGER,AccountName TEXT," & _
                "Amount DECIMAL,DC TEXT,Remark TEXT,Narration TEXT,RemarkHindi TEXT,PartyID INTEGER,PartyName TEXT,PostingID INTEGER,PostingAccount TEXT);" & _
                "INSERT INTO Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount," & _
                "DC,Remark,Narration,RemarkHindi,PartyID,PartyName)SELECT VourchersID,EntryDate,TransType,AccountID,AccountName,Amount," & _
                "DC,Remark,Narration,RemarkHindi,PartyID,PartyName FROM TempLedger;DROP TABLE TempLedger;" & _
                "CREATE INDEX AccountIndex ON Ledger(AccountID,AccountName);PRAGMA foreign_keys = 1;"
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.ExecNonQuery("Update Ledger SET PostingID=0,PostingAccount=''")
            End If
        End If
    End Sub

    Private Sub UpdateAccountGroups()
        If clsFun.CheckIfColumnExists("AccountGroup", "ParentID") = False Then
            Dim sql As String = String.Empty
            sql = "drop table if  exists TempGroups;PRAGMA foreign_keys = 0;CREATE TABLE TempGroups AS SELECT * FROM AccountGroup;" & _
                "DROP TABLE AccountGroup;CREATE TABLE AccountGroup (ID INTEGER PRIMARY KEY AUTOINCREMENT,GroupName TEXT, " & _
                "UnderGroupID INTEGER,UnderGroupName TEXT,DC TEXT,Primary2 TEXT,Tag TEXT,ParentID INTEGER);" & _
                "INSERT INTO AccountGroup (ID,GroupName,UnderGroupID,UnderGroupName,DC,Primary2,Tag) SELECT ID,GroupName,UnderGroupID,UnderGroupName," & _
                "DC,Primary2,Tag FROM TempGroups;DROP TABLE TempGroups;PRAGMA foreign_keys = 1;Update AccountGroup set ParentID=ID Where Primary2='Y'; " & _
                "Update AccountGroup set ParentID=UnderGroupID Where UnderGroupID in(1,2,3,4,5,6,7,8,9,10);Update AccountGroup Set ParentID=3 Where ID in(30,31,33);" & _
                "Update AccountGroup Set ParentID=2 Where ID in(32)"
            clsFun.ExecNonQuery(sql)
        End If
    End Sub

    Private Sub Transaction1()
        If clsFun.CheckIfColumnExists("Transaction1", "CrateAccountName") = False Then
            Dim sql As String = String.Empty
            sql = "drop table if  exists UpdateTrans1;PRAGMA foreign_keys = 0;CREATE TABLE UpdateTrans1 AS SELECT * FROM Transaction1;" & _
                "DROP TABLE Transaction1;CREATE TABLE Transaction1 (ID INTEGER PRIMARY KEY AUTOINCREMENT," & _
                "VoucherID INTEGER,AccountID INTEGER,AccountName TEXT,ItemID INTEGER,ItemName TEXT," & _
                "Cut TEXT,Lot TEXT,Nug DECIMAL,Weight DECIMAL,Rate DECIMAL,SRate DECIMAL,Per TEXT," & _
                "Amount DECIMAL,Charges DECIMAL,TotalAmount DECIMAL,MaintainCrate TEXT,CrateID INTEGER," & _
                "Cratemarka TEXT,CrateQty DECIMAL,PurchaseID INTEGER,Roundoff DECIMAL,CrateTransType TEXT,CrateAccountID INTEGER,CrateAccountName TEXT);" & _
                "INSERT INTO Transaction1(ID, VoucherID, AccountID, AccountName,ItemID,ItemName,Cut,Lot,Nug," & _
                "Weight,Rate,SRate,Per,Amount,Charges,TotalAmount,MaintainCrate,CrateID,Cratemarka,CrateQty,PurchaseID,Roundoff)" & _
                "SELECT ID,VoucherID, AccountID, AccountName, ItemID, ItemName, Cut, Lot, Nug, Weight, Rate," & _
                "SRate, Per, Amount,Charges, TotalAmount, MaintainCrate, CrateID, Cratemarka, CrateQty, PurchaseID," & _
                "Roundoff FROM UpdateTrans1 ;DROP TABLE UpdateTrans1 ;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If

        If clsFun.CheckIfColumnExists("Transaction1", "AddWeight") = False Then
            Dim sql As String = String.Empty
            sql = "drop table if  exists UpdateTrans1;PRAGMA foreign_keys = 0;CREATE TABLE UpdateTrans1 AS SELECT * FROM Transaction1;" & _
                "DROP TABLE Transaction1;CREATE TABLE Transaction1 (ID INTEGER PRIMARY KEY AUTOINCREMENT," & _
                "VoucherID INTEGER,AccountID INTEGER,AccountName TEXT,ItemID INTEGER,ItemName TEXT," & _
                "Cut TEXT,Lot TEXT,Nug DECIMAL,Weight DECIMAL,Rate DECIMAL,SRate DECIMAL,Per TEXT," & _
                "Amount DECIMAL,Charges DECIMAL,TotalAmount DECIMAL,MaintainCrate TEXT,CrateID INTEGER," & _
                "Cratemarka TEXT,CrateQty DECIMAL,PurchaseID INTEGER,Roundoff DECIMAL,CrateTransType TEXT,CrateAccountID INTEGER,CrateAccountName TEXT,AddWeight TEXT);" & _
                "INSERT INTO Transaction1(ID, VoucherID, AccountID, AccountName,ItemID,ItemName,Cut,Lot,Nug," & _
                "Weight,Rate,SRate,Per,Amount,Charges,TotalAmount,MaintainCrate,CrateID,Cratemarka,CrateQty,PurchaseID,Roundoff,CrateTransType,CrateAccountID,CrateAccountName)" & _
                "SELECT ID,VoucherID, AccountID, AccountName, ItemID, ItemName, Cut, Lot, Nug, Weight, Rate," & _
                "SRate, Per, Amount,Charges, TotalAmount, MaintainCrate, CrateID, Cratemarka, CrateQty, PurchaseID," & _
                "Roundoff,CrateTransType,CrateAccountID,CrateAccountName FROM UpdateTrans1 ;DROP TABLE UpdateTrans1 ;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If

        If clsFun.CheckIfColumnExists("Transaction1", "OnSaleID") = False Then
            Dim sql As String = String.Empty
            sql = "drop table if  exists UpdateTrans1;PRAGMA foreign_keys = 0;CREATE TABLE UpdateTrans1 AS SELECT * FROM Transaction1;" & _
                "DROP TABLE Transaction1;CREATE TABLE Transaction1 (ID INTEGER PRIMARY KEY AUTOINCREMENT," & _
                "VoucherID INTEGER,AccountID INTEGER,AccountName TEXT,ItemID INTEGER,ItemName TEXT," & _
                "Cut TEXT,Lot TEXT,Nug DECIMAL,Weight DECIMAL,Rate DECIMAL,SRate DECIMAL,Per TEXT," & _
                "Amount DECIMAL,Charges DECIMAL,TotalAmount DECIMAL,MaintainCrate TEXT,CrateID INTEGER," & _
                "Cratemarka TEXT,CrateQty DECIMAL,PurchaseID INTEGER,Roundoff DECIMAL,CrateTransType TEXT,CrateAccountID INTEGER,CrateAccountName TEXT,AddWeight TEXT,OnSaleID INTEGER);" & _
                "INSERT INTO Transaction1(ID, VoucherID, AccountID, AccountName,ItemID,ItemName,Cut,Lot,Nug," & _
                "Weight,Rate,SRate,Per,Amount,Charges,TotalAmount,MaintainCrate,CrateID,Cratemarka,CrateQty,PurchaseID,Roundoff,CrateTransType,CrateAccountID,CrateAccountName,AddWeight)" & _
                "SELECT ID,VoucherID, AccountID, AccountName, ItemID, ItemName, Cut, Lot, Nug, Weight, Rate," & _
                "SRate, Per, Amount,Charges, TotalAmount, MaintainCrate, CrateID, Cratemarka, CrateQty, PurchaseID," & _
                "Roundoff,CrateTransType,CrateAccountID,CrateAccountName,AddWeight FROM UpdateTrans1 ;DROP TABLE UpdateTrans1 ;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfColumnExists("Charges", "PrintName") = False Then
            Dim sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE TempCharges AS SELECT * FROM Charges;DROP TABLE Charges;CREATE TABLE Charges (ID INTEGER PRIMARY KEY AUTOINCREMENT," & _
                " ChargeName TEXT,Calculate TEXT,AccountID INTEGER,AccountName TEXT,ApplyType TEXT,ChargesType TEXT,ApplyOn TEXT,CostOn  TEXT,RoundOff DECIMAL,PrintName   TEXT);" & _
                " INSERT INTO Charges (ID,ChargeName,Calculate,AccountID,AccountName,ApplyType,ChargesType,ApplyOn,CostOn,RoundOff) SELECT ID,ChargeName,Calculate,AccountID, " & _
                " AccountName,ApplyType,ChargesType,ApplyOn,CostOn,RoundOff FROM TempCharges; DROP TABLE TempCharges;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            clsFun.ExecScalarStr("Update Charges SET PrintName=ChargeName")
        End If
    End Sub
    Private Sub UpdateView()
        Dim sql As String = ""
        If clsFun.CheckIfColumnExists("BillPrints", "OnWeight") = False Then
            sql = "Drop View if  exists BillPrints;CREATE VIEW BillPrints AS SELECT Transaction2.EntryDate, Transaction2.ItemName, Transaction2.AccountName, Transaction2.Nug, Transaction2.Weight, Transaction2.Rate," & _
                "Transaction2.Per, Transaction2.Amount, Transaction2.Charges, Transaction2.TotalAmount, Transaction2.CommPer, Transaction2.CommAmt," & _
                "Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, Transaction2.Tare, Transaction2.Tareamt," & _
                "Transaction2.Labour, Transaction2.LabourAmt, Transaction2.MaintainCrate, Transaction2.Cratemarka, Transaction2.CrateQty," & _
                "Items.OtherName, Accounts.Othername as AccountNameOther, Transaction2.TransType, Transaction2.AccountID,ifnull(Transaction2.OnWeight,'') as OnWeight " & _
                "FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID " & _
                "WHERE (((Transaction2.TransType) Not In ('Standard Sale'))) ORDER BY Transaction2.AccountName"
            clsFun.ExecNonQuery(sql)
        End If
        sql = ""
        'sql = "Drop View if  exists BillPrints2;CREATE VIEW BillPrints2 AS SELECT Transaction2.EntryDate, Transaction2.ItemName, Transaction2.AccountName, Transaction2.Nug, Transaction2.Weight, Transaction2.Rate," & _
        '    "Transaction2.Per, Transaction2.Amount, Transaction2.Charges, Transaction2.TotalAmount, Transaction2.CommPer, Transaction2.CommAmt," & _
        '    "Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, Transaction2.Tare, Transaction2.Tareamt," & _
        '    "Transaction2.Labour, Transaction2.LabourAmt, Transaction2.MaintainCrate, Transaction2.Cratemarka, Transaction2.CrateQty," & _
        '    "Items.OtherName, Accounts.Othername as AccountNameOther, Transaction2.TransType, Transaction2.AccountID,ifnull(Transaction2.OnWeight,'') as OnWeight " & _
        '    "FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID " & _
        '    "WHERE (((Transaction2.TransType) Not In ('Standard Sale')))  GROUP BY Transaction2.EntryDate,Transaction2.AccountID,Transaction2.ItemID, " & _
        ' " Transaction2.Rate,Transaction2.Per ORDER BY Transaction2.AccountName, Transaction2.ItemName;"
        sql = "Drop View if exists BillPrints2;CREATE VIEW if not exists BillPrints2 AS SELECT Transaction2.EntryDate, Transaction2.ItemName, Transaction2.AccountName, sum(Transaction2.Nug) AS nug," & _
         " round(sum(Transaction2.Weight), 2) AS weight,     Transaction2.Rate,  Transaction2.Per, round(sum(Transaction2.Amount), 2) AS amount," & _
         " sum(Transaction2.Charges) AS Charges,sum(Transaction2.TotalAmount) AS Totalamount, Transaction2.CommPer, sum(Transaction2.CommAmt) AS CommAmt," & _
         " Transaction2.MPer,sum(Transaction2.MAmt) AS MAmt, Transaction2.RdfPer, sum(Transaction2.RdfAmt) AS RdfAmt,Transaction2.Tare, " & _
         " sum(Transaction2.Tareamt) AS Tareamt,  Transaction2.Labour,sum(Transaction2.LabourAmt) AS LabourAmt, Transaction2.MaintainCrate," & _
         " Transaction2.Cratemarka, Transaction2.CrateQty, Items.OtherName, Accounts.Othername as AccountNameOther,Transaction2.TransType, Transaction2.AccountID,transaction2.OnWeight" & _
         " FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID" & _
         "  WHERE (((Transaction2.TransType) Not In ('Standard Sale','On Sale')))  GROUP BY Transaction2.EntryDate,Transaction2.AccountID, Transaction2.ItemID," & _
         " Transaction2.Rate,Transaction2.Per ORDER BY Transaction2.AccountName,Transaction2.ItemName"
        clsFun.ExecNonQuery(sql)

        If clsFun.CheckIfColumnExists("BillPrints2", "OnWeight") = False Then
            sql = ""
            'sql = "Drop View if  exists BillPrints2;CREATE VIEW BillPrints2 AS SELECT Transaction2.EntryDate, Transaction2.ItemName, Transaction2.AccountName, Transaction2.Nug, Transaction2.Weight, Transaction2.Rate," & _
            '    "Transaction2.Per, Transaction2.Amount, Transaction2.Charges, Transaction2.TotalAmount, Transaction2.CommPer, Transaction2.CommAmt," & _
            '    "Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, Transaction2.Tare, Transaction2.Tareamt," & _
            '    "Transaction2.Labour, Transaction2.LabourAmt, Transaction2.MaintainCrate, Transaction2.Cratemarka, Transaction2.CrateQty," & _
            '    "Items.OtherName, Accounts.Othername as AccountNameOther, Transaction2.TransType, Transaction2.AccountID,ifnull(Transaction2.OnWeight,'') as OnWeight " & _
            '    "FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID " & _
            '    "WHERE (((Transaction2.TransType) Not In ('Standard Sale')))  GROUP BY Transaction2.EntryDate,Transaction2.AccountID,Transaction2.ItemID, " & _
            ' " Transaction2.Rate,Transaction2.Per ORDER BY Transaction2.AccountName, Transaction2.ItemName;"
            sql = "Drop View if exists BillPrints2;CREATE VIEW if not exists BillPrints2 AS SELECT Transaction2.EntryDate, Transaction2.ItemName, Transaction2.AccountName, sum(Transaction2.Nug) AS nug," & _
         " round(sum(Transaction2.Weight), 2) AS weight,     Transaction2.Rate,  Transaction2.Per, round(sum(Transaction2.Amount), 2) AS amount," & _
         " sum(Transaction2.Charges) AS Charges,sum(Transaction2.TotalAmount) AS Totalamount, Transaction2.CommPer, sum(Transaction2.CommAmt) AS CommAmt," & _
         " Transaction2.MPer,sum(Transaction2.MAmt) AS MAmt, Transaction2.RdfPer, sum(Transaction2.RdfAmt) AS RdfAmt,Transaction2.Tare, " & _
         " sum(Transaction2.Tareamt) AS Tareamt,  Transaction2.Labour,sum(Transaction2.LabourAmt) AS LabourAmt, Transaction2.MaintainCrate," & _
         " Transaction2.Cratemarka, Transaction2.CrateQty, Items.OtherName, Accounts.Othername as AccountNameOther,Transaction2.TransType, Transaction2.AccountID,transaction2.OnWeight" & _
         " FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID" & _
         "  WHERE (((Transaction2.TransType) Not In ('Standard Sale','On Sale')))  GROUP BY Transaction2.EntryDate,Transaction2.AccountID, Transaction2.ItemID," & _
         " Transaction2.Rate,Transaction2.Per ORDER BY Transaction2.AccountName,Transaction2.ItemName"
            clsFun.ExecNonQuery(sql)
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Transaction1() : UpdateAccountGroups() : ReceiptUpdate()
            UpdateLedgerTable() : UpdateVoucher()
            If clsFun.CheckIfColumnExists("CrateVoucher", "CashPaid") = False Then UpdateCrateTable()
            UpdateVoucherTbl()
            If clsFun.CheckIfColumnExists("Company", "LinkedDB") = False Then updateCompanytbl()
            If clsFun.CheckIfColumnExists("Transaction2", "CrateRate") = False Then
                Dim sql As String = "ALTER TABLE Transaction2 ADD COLUMN CrateRate DECIMAL;"
                clsFun.ExecNonQuery(sql)
            End If
            If clsFun.CheckIfColumnExists("Transaction2", "GrossWeight") = False Then
                Dim sql As String = "ALTER TABLE Transaction2 ADD COLUMN GrossWeight DECIMAL;"
                clsFun.ExecNonQuery(sql)
            End If
            CreateOptions() : DbpFile() : UserRights() : UpdateView() : ServerDb()
            CompanyInfo() : OptionsControl() : UpdateLoose()
            Login.MdiParent = ShowCompanies
            Login.Show()
            If Not Login Is Nothing Then
                Login.BringToFront()
            End If
            e.SuppressKeyPress = True
            ' clsFun.GetConnection()
        End If
    End Sub
    Public Sub ServerDb()

        If Directory.Exists(Application.StartupPath & "\ServerDb") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\ServerDb")
            Dim FileName As String = "ServerDb.db"
            SQLiteConnection.CreateFile(Application.StartupPath & "\ServerDb\ServerDB.db")
            ClsFunserver.ExecScalarStr("CREATE TABLE if not exists AccountGroup (ID INTEGER,GroupName TEXT,UnderGroupID INTEGER,UnderGroupName TEXT, " & _
                                       "DC TEXT,Primary2 TEXT,Tag TEXT,ParentID INTEGER,ServerTag INTEGER, ORGID INTEGER);")
            ClsFunserver.ExecScalarStr("CREATE TABLE if not exists Accounts (ID INTEGER," & _
                                       "AccountName TEXT,GroupID  INTEGER,DC TEXT,Tag  TEXT,OpBal DECIMAL," & _
                                       "OtherName TEXT,address TEXT,LFNo TEXT,Area TEXT,City TEXT,State TEXT," & _
                                       "Phone TEXT,Contact TEXT,Mobile1 TEXT,Mobile2 TEXT,MailID TEXT," & _
                                       "BankName TEXT,AccNo TEXT,IFSC TEXT,GName TEXT,Gmobile1 TEXT," & _
                                       "Gmobile2 TEXT,Gaddress TEXT,GCity TEXT,Gstate TEXT,[Limit]  TEXT," & _
                                       "AccountPhoto BLOB,Gphoto BLOB,CommPer  DECIMAL,Mper  DECIMAL,RdfPer  DECIMAL, " & _
                                       "TarePer  DECIMAL,LabourPer  DECIMAL ,ServerTag INTEGER, ORGID INTEGER);")
            ClsFunserver.ExecScalarStr("CREATE TABLE if not exists Company (ID INTEGER,CompanyName TEXT, PrintOtherName TEXT, Address TEXT,PrintOtheraddress TEXT, " & _
                                       "City TEXT,PrintOtherCity TEXT,State TEXT,PrintOtherState   TEXT,MobileNo1 TEXT,MobileNo2 TEXT, PhoneNo TEXT, " & _
                                       "FaxNo TEXT,EmailID TEXT, Website TEXT, GSTN TEXT, DealsIN TEXT,RegistrationNo TEXT,PanNo TEXT,Marka TEXT, " & _
                                       "Other TEXT, Logo BLOB, YearStart DATE,Yearend DATE, CompData TEXT, tag TEXT,ServerTag TEXT,OrganizationID INTEGER, " & _
                                       "Password TEXT,Autosync TEXT,FullSync TEXT);")
            ClsFunserver.ExecScalarStr("CREATE TABLE if not Exists CrateMarka(ID INTEGER,MarkaName TEXT,OpQty DECIMAL,Rate DECIMAL,ServerTag INTEGER, ORGID INTEGER);")
            ClsFunserver.ExecScalarStr("CREATE TABLE if not exists CrateVoucher (ID INTEGER PRIMARY KEY AUTOINCREMENT,VoucherID INTEGER,SlipNo TEXT,EntryDate DATE,TransType TEXT," & _
                                       "AccountID   INTEGER,AccountName TEXT,CrateType TEXT,CrateID INTEGER,CrateName TEXT,Qty INTEGER,Remark TEXT,Rate DECIMAL,Amount DECIMAL,CashPaid TEXT,ServerTag INTEGER, ORGID INTEGER);")
            ClsFunserver.ExecScalarStr("CREATE TABLE if Not Exists Ledger (VourchersID INTEGER,EntryDate DATE,TransType TEXT,AccountID INTEGER,AccountName TEXT,Amount DECIMAL, " & _
                                        "DC TEXT,Remark TEXT,Narration TEXT,RemarkHindi TEXT,ServerTag INTEGER, ORGID INTEGER);")
            ClsFunserver.ExecScalarStr("CREATE TABLE if Not Exists Vouchers(ID INTEGER,TransType TEXT,billNo TEXT,VehicleNo TEXT,Entrydate DATE, PurchaseType TEXT, " & _
                                       "sallerID INTEGER,sallerName TEXT,ItemID INTEGER,ItemName TEXT,AccountID INTEGER,AccountName TEXT,Nug DECIMAL,Kg DECIMAL,Rate DECIMAL,Per TEXT,BasicAmount DECIMAL " & _
                                       ",DiscountAmount DECIMAL,SubTotal DECIMAL,TotalAmount DECIMAL,CommissionPer DECIMAL,CommissionAmount DECIMAL,MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount DECIMAL, " & _
                                       "Tare DECIMAL,TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT,CrateID INTEGER,CrateName TEXT," & _
                                       "CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT,StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL, " & _
                                       "N2 DECIMAL,N3 DECIMAL,N4 DECIMAL,N5 DECIMAL,T1 TEXT,T2 TEXT,T3 TEXT,T4 TEXT,T5 TEXT,RoundOff DECIMAL,InvoiceID INTEGER,ServerTag INTEGER, ORGID INTEGER);")

            ClsFunserver.ExecScalarStr("CREATE TABLE Transaction2 (ID INTEGER PRIMARY KEY AUTOINCREMENT, EntryDate DATE, VoucherID INTEGER, TransType TEXT, BillNo   TEXT, SallerID  INTEGER, SallerName  TEXT, " & _
                                       "AccountID INTEGER, AccountName  TEXT, OtherAccountName TEXT, ItemID   INTEGER, ItemName  TEXT, OtherItemName TEXT, Cut   DECIMAL, Lot   TEXT, Nug   DECIMAL, Weight   DECIMAL," & _
                                       "Rate  DECIMAL, SRate  DECIMAL,ActualRate  DECIMAL, Per   TEXT, Amount   DECIMAL, Charges  DECIMAL, TotalAmount  DECIMAL, SallerAmt DECIMAL, CommPer  DECIMAL, CommAmt  DECIMAL, " & _
                                       "MPer  DECIMAL, MAmt  DECIMAL, RdfPer   DECIMAL, RdfAmt   DECIMAL, Tare  DECIMAL, TareAmt  DECIMAL, labour   DECIMAL, LabourAmt DECIMAL, MaintainCrate TEXT, CrateID  DECIMAL, " & _
                                       "Cratemarka  TEXT, CrateQty  DECIMAL, CrateRate  DECIMAL, CrateAmount  DECIMAL, PurchaseTypename TEXT, PurchaseID  INTEGER, RoundOff  DECIMAL, CrateAccountID  INTEGER, " & _
                                       "CrateAccountName TEXT,StorageID INTEGER,StorageName TEXT,OnWeight TEXT,ServerTag INTEGER, ORGID INTEGER)")
        End If


    End Sub
    Public Sub AccountsFields()
        Dim sql As String = String.Empty
        If clsFun.CheckIfColumnExists("Accounts", "CommPer") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempAccounts AS SELECT * FROM Accounts;DROP TABLE Accounts;" & _
                "CREATE TABLE Accounts (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," & _
                "AccountName TEXT,GroupID  INTEGER,DC TEXT,Tag  TEXT,OpBal DECIMAL," & _
                "OtherName TEXT,address TEXT,LFNo TEXT,Area TEXT,City TEXT,State TEXT," & _
                "Phone TEXT,Contact TEXT,Mobile1 TEXT,Mobile2 TEXT,MailID TEXT," & _
                "BankName TEXT,AccNo TEXT,IFSC TEXT,GName TEXT,Gmobile1 TEXT," & _
                "Gmobile2 TEXT,Gaddress TEXT,GCity TEXT,Gstate TEXT,[Limit]  TEXT," & _
                "AccountPhoto BLOB,Gphoto BLOB,CommPer  DECIMAL,Mper  DECIMAL,RdfPer  DECIMAL,TarePer  DECIMAL,LabourPer  DECIMAL);" & _
                "INSERT INTO Accounts (ID,AccountName,GroupID,DC,Tag,OpBal,OtherName," & _
                "address,LFNo,Area,City,State,Phone,Contact,Mobile1,Mobile2,MailID,BankName," & _
                "AccNo,IFSC,GName,Gmobile1,Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,CommPer,Mper,RdfPer,TarePer,LabourPer)" & _
                "SELECT ID,AccountName,GroupID,DC,Tag,OpBal,OtherName,address,LFNo,Area,City," & _
                "State,Phone,Contact,Mobile1,Mobile2,MailID,BankName,AccNo,IFSC,GName,Gmobile1," & _
                "Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,0,0,0,0,0 FROM TempAccounts;" & _
                "DROP TABLE TempAccounts;" & _
                "CREATE INDEX AccountIDindex ON Accounts(ID,AccountName,GroupID);" & _
                "DROP VIEW Vw_BalanceSheet;CREATE VIEW Vw_BalanceSheet AS  SELECT grp1.id," & _
                "grp1.GroupName,grp1.DC,grp1.UnderGroupName, " & _
                "grp1.UnderGroupID,ac.id AS acid,ac.AccountName,grp1.UnderGroupID,ac.id AS acid,ac.AccountName, ac.OpBal FROM AccountGroup grp1 " & _
                "LEFT JOIN AccountGroup grp2 ON grp1.id = grp2.UnderGroupID LEFT JOIN Accounts ac ON ac.groupid = grp1.id " & _
                "WHERE grp1.id NOT IN (9, 22, 23, 24, 25, 26, 27, 29);PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfColumnExists("Accounts", "Deactivate") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempAccounts AS SELECT * FROM Accounts;DROP TABLE Accounts;" & _
                "CREATE TABLE Accounts (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," & _
                "AccountName TEXT,GroupID  INTEGER,DC TEXT,Tag  TEXT,OpBal DECIMAL," & _
                "OtherName TEXT,address TEXT,LFNo TEXT,Area TEXT,City TEXT,State TEXT," & _
                "Phone TEXT,Contact TEXT,Mobile1 TEXT,Mobile2 TEXT,MailID TEXT," & _
                "BankName TEXT,AccNo TEXT,IFSC TEXT,GName TEXT,Gmobile1 TEXT," & _
                "Gmobile2 TEXT,Gaddress TEXT,GCity TEXT,Gstate TEXT,[Limit]  TEXT," & _
                "AccountPhoto BLOB,Gphoto BLOB,CommPer  DECIMAL,Mper  DECIMAL,RdfPer  DECIMAL,TarePer  DECIMAL,LabourPer  DECIMAL,Deactivate TEXT);" & _
                "INSERT INTO Accounts (ID,AccountName,GroupID,DC,Tag,OpBal,OtherName," & _
                "address,LFNo,Area,City,State,Phone,Contact,Mobile1,Mobile2,MailID,BankName," & _
                "AccNo,IFSC,GName,Gmobile1,Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,CommPer,Mper,RdfPer,TarePer,LabourPer)" & _
                "SELECT ID,AccountName,GroupID,DC,Tag,OpBal,OtherName,address,LFNo,Area,City," & _
                "State,Phone,Contact,Mobile1,Mobile2,MailID,BankName,AccNo,IFSC,GName,Gmobile1," & _
                "Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,CommPer,Mper,RdfPer,TarePer,LabourPer FROM TempAccounts;" & _
                "DROP TABLE TempAccounts;" & _
                "CREATE INDEX AccountIDindex ON Accounts(ID,AccountName,GroupID);" & _
                "DROP VIEW Vw_BalanceSheet;CREATE VIEW Vw_BalanceSheet AS  SELECT grp1.id," & _
                "grp1.GroupName,grp1.DC,grp1.UnderGroupName, " & _
                "grp1.UnderGroupID,ac.id AS acid,ac.AccountName,grp1.UnderGroupID,ac.id AS acid,ac.AccountName, ac.OpBal FROM AccountGroup grp1 " & _
                "LEFT JOIN AccountGroup grp2 ON grp1.id = grp2.UnderGroupID LEFT JOIN Accounts ac ON ac.groupid = grp1.id " & _
                "WHERE grp1.id NOT IN (9, 22, 23, 24, 25, 26, 27, 29);PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If

        If clsFun.CheckIfColumnExists("Accounts", "POSTINGID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempAccounts AS SELECT * FROM Accounts;DROP TABLE Accounts;" & _
                "CREATE TABLE Accounts (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," & _
                "AccountName TEXT,GroupID  INTEGER,DC TEXT,Tag  TEXT,OpBal DECIMAL," & _
                "OtherName TEXT,address TEXT,LFNo TEXT,Area TEXT,City TEXT,State TEXT," & _
                "Phone TEXT,Contact TEXT,Mobile1 TEXT,Mobile2 TEXT,MailID TEXT," & _
                "BankName TEXT,AccNo TEXT,IFSC TEXT,GName TEXT,Gmobile1 TEXT," & _
                "Gmobile2 TEXT,Gaddress TEXT,GCity TEXT,Gstate TEXT,[Limit]  TEXT," & _
                "AccountPhoto BLOB,Gphoto BLOB,CommPer  DECIMAL,Mper  DECIMAL,RdfPer  DECIMAL,TarePer  DECIMAL,LabourPer  DECIMAL,Deactivate TEXT,POSTINGID INTEGER, POSTINGACNAME TEXT);" & _
                "INSERT INTO Accounts (ID,AccountName,GroupID,DC,Tag,OpBal,OtherName," & _
                "address,LFNo,Area,City,State,Phone,Contact,Mobile1,Mobile2,MailID,BankName," & _
                "AccNo,IFSC,GName,Gmobile1,Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,CommPer,Mper,RdfPer,TarePer,LabourPer,Deactivate)" & _
                "SELECT ID,AccountName,GroupID,DC,Tag,OpBal,OtherName,address,LFNo,Area,City," & _
                "State,Phone,Contact,Mobile1,Mobile2,MailID,BankName,AccNo,IFSC,GName,Gmobile1," & _
                "Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,CommPer,Mper,RdfPer,TarePer,LabourPer,Deactivate FROM TempAccounts;" & _
                "DROP TABLE TempAccounts;" & _
                "CREATE INDEX AccountIDindex ON Accounts(ID,AccountName,GroupID);" & _
                "DROP VIEW Vw_BalanceSheet;CREATE VIEW Vw_BalanceSheet AS  SELECT grp1.id," & _
                "grp1.GroupName,grp1.DC,grp1.UnderGroupName, " & _
                "grp1.UnderGroupID,ac.id AS acid,ac.AccountName,grp1.UnderGroupID,ac.id AS acid,ac.AccountName, ac.OpBal FROM AccountGroup grp1 " & _
                "LEFT JOIN AccountGroup grp2 ON grp1.id = grp2.UnderGroupID LEFT JOIN Accounts ac ON ac.groupid = grp1.id " & _
                "WHERE grp1.id NOT IN (9, 22, 23, 24, 25, 26, 27, 29);PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If

        If clsFun.CheckIfColumnExists("AccountGroup", "GUID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0; CREATE TABLE TempAcGroup AS SELECT *FROM AccountGroup; " & _
                " DROP TABLE AccountGroup; CREATE TABLE AccountGroup (ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                " GroupName TEXT, UnderGroupID INTEGER,UnderGroupName TEXT, DC TEXT,Primary2 TEXT,Tag TEXT," & _
                " ParentID INTEGER,GUID   TEXT); INSERT INTO AccountGroup(ID,GroupName,UnderGroupID," & _
                " UnderGroupName,DC,Primary2,Tag,ParentID) SELECT ID,GroupName,UnderGroupID,UnderGroupName," & _
                " DC,Primary2,Tag,ParentID FROM TempAcGroup;DROP TABLE TempAcGroup; PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            UpdateGUID("AccountGroup")
        End If

        If clsFun.CheckIfColumnExists("Charges", "GUID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempCharges AS SELECT * FROM Charges; " & _
                " DROP TABLE Charges;CREATE TABLE Charges (ID   INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                " ChargeName  TEXT,Calculate TEXT, AccountID   INTEGER, AccountName TEXT, " & _
                " ApplyType TEXT,ChargesType TEXT,ApplyOn TEXT,CostOn TEXT, RoundOff  DECIMAL,PrintName TEXT,GUID TEXT);" & _
                " INSERT INTO Charges (ID,ChargeName,Calculate,AccountID,AccountName,ApplyType,ChargesType,ApplyOn,CostOn,RoundOff,PrintName) " & _
                " SELECT ID,ChargeName,Calculate,AccountID,AccountName,ApplyType,ChargesType,ApplyOn,CostOn,RoundOff,PrintName FROM TempCharges; DROP TABLE TempCharges;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            UpdateGUID("Charges")
        End If


        If clsFun.CheckIfColumnExists("Items", "GUID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempItems AS SELECT * FROM Items; " & _
                " DROP TABLE Items; CREATE TABLE Items (ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                " ItemName TEXT,OtherName TEXT,CommisionPer DECIMAL,UserChargesPer DECIMAL, " & _
                " Tare DECIMAL,Labour DECIMAL,RDFPer DECIMAL,WeightPerNug   DECIMAL,CutPerNug DECIMAL," & _
                " MaintainCrate TEXT,GUID TEXT);INSERT INTO Items (ID,ItemName,OtherName,CommisionPer,UserChargesPer,Tare,Labour,RDFPer," & _
                " WeightPerNug,CutPerNug,MaintainCrate) SELECT ID,ItemName,OtherName,CommisionPer," & _
                " UserChargesPer,Tare,Labour,RDFPer,WeightPerNug,CutPerNug,MaintainCrate FROM TempItems; DROP TABLE TempItems;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            UpdateGUID("Items")
        End If


        If clsFun.CheckIfColumnExists("Items", "RateAs") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempItems AS SELECT * FROM Items; " & _
                " DROP TABLE Items; CREATE TABLE Items (ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                " ItemName TEXT,OtherName TEXT,CommisionPer DECIMAL,UserChargesPer DECIMAL, " & _
                " Tare DECIMAL,Labour DECIMAL,RDFPer DECIMAL,WeightPerNug   DECIMAL,CutPerNug DECIMAL," & _
                " MaintainCrate TEXT,RateAs TEXT,TrackStock TEXT,GUID TEXT);INSERT INTO Items (ID,ItemName,OtherName,CommisionPer,UserChargesPer,Tare,Labour,RDFPer," & _
                " WeightPerNug,CutPerNug,MaintainCrate,GUID) SELECT ID,ItemName,OtherName,CommisionPer," & _
                " UserChargesPer,Tare,Labour,RDFPer,WeightPerNug,CutPerNug,MaintainCrate,GUID FROM TempItems; DROP TABLE TempItems;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            clsFun.ExecNonQuery("Update items SET RateAs='Kg',TrackStock='Nug'")
        End If


        If clsFun.CheckIfColumnExists("CrateMarka", "GUID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempMarka AS SELECT * FROM CrateMarka;" & _
                " DROP TABLE CrateMarka;CREATE TABLE CrateMarka ( " & _
                " ID INTEGER PRIMARY KEY AUTOINCREMENT,MarkaName TEXT,OpQty DECIMAL, " & _
                " Rate DECIMAL,GUID TEXT);INSERT INTO CrateMarka (ID,MarkaName,OpQty,Rate) " & _
                " SELECT ID,MarkaName,OpQty,Rate FROM TempMarka; " & _
                " DROP TABLE TempMarka; PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            UpdateGUID("CrateMarka")
        End If

        If clsFun.CheckIfColumnExists("Storage", "GUID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempStore AS SELECT * FROM Storage;" & _
                " DROP TABLE Storage;CREATE TABLE Storage (ID INTEGER PRIMARY KEY AUTOINCREMENT," & _
                " StorageName TEXT,GUID TEXT);INSERT INTO Storage (ID,StorageName) " & _
                " SELECT ID,StorageName FROM TempStore;DROP TABLE TempStore; " & _
                " PRAGMA foreign_keys = 1; "
            clsFun.ExecNonQuery(sql)
            UpdateGUID("Storage")
        End If



        If clsFun.CheckIfColumnExists("Accounts", "GUID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempAccounts AS SELECT * FROM Accounts;DROP TABLE Accounts;" & _
                "CREATE TABLE Accounts (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," & _
                "AccountName TEXT,GroupID  INTEGER,DC TEXT,Tag  TEXT,OpBal DECIMAL," & _
                "OtherName TEXT,address TEXT,LFNo TEXT,Area TEXT,City TEXT,State TEXT," & _
                "Phone TEXT,Contact TEXT,Mobile1 TEXT,Mobile2 TEXT,MailID TEXT," & _
                "BankName TEXT,AccNo TEXT,IFSC TEXT,GName TEXT,Gmobile1 TEXT," & _
                "Gmobile2 TEXT,Gaddress TEXT,GCity TEXT,Gstate TEXT,[Limit]  TEXT," & _
                "AccountPhoto BLOB,Gphoto BLOB,CommPer  DECIMAL,Mper  DECIMAL,RdfPer  DECIMAL,TarePer  DECIMAL,LabourPer  DECIMAL,Deactivate TEXT,POSTINGID INTEGER, POSTINGACNAME TEXT,GUID TEXT);" & _
                "INSERT INTO Accounts (ID,AccountName,GroupID,DC,Tag,OpBal,OtherName," & _
                "address,LFNo,Area,City,State,Phone,Contact,Mobile1,Mobile2,MailID,BankName," & _
                "AccNo,IFSC,GName,Gmobile1,Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,CommPer,Mper,RdfPer,TarePer,LabourPer,Deactivate,POSTINGID,POSTINGACNAME)" & _
                "SELECT ID,AccountName,GroupID,DC,Tag,OpBal,OtherName,address,LFNo,Area,City," & _
                "State,Phone,Contact,Mobile1,Mobile2,MailID,BankName,AccNo,IFSC,GName,Gmobile1," & _
                "Gmobile2,Gaddress,GCity,Gstate,[Limit],AccountPhoto,Gphoto,CommPer,Mper,RdfPer,TarePer,LabourPer,Deactivate,POSTINGID,POSTINGACNAME FROM TempAccounts;" & _
                "DROP TABLE TempAccounts;" & _
                "CREATE INDEX AccountIDindex ON Accounts(ID,AccountName,GroupID);" & _
                "DROP VIEW Vw_BalanceSheet;CREATE VIEW Vw_BalanceSheet AS  SELECT grp1.id," & _
                "grp1.GroupName,grp1.DC,grp1.UnderGroupName, " & _
                "grp1.UnderGroupID,ac.id AS acid,ac.AccountName,grp1.UnderGroupID,ac.id AS acid,ac.AccountName, ac.OpBal FROM AccountGroup grp1 " & _
                "LEFT JOIN AccountGroup grp2 ON grp1.id = grp2.UnderGroupID LEFT JOIN Accounts ac ON ac.groupid = grp1.id " & _
                "WHERE grp1.id NOT IN (9, 22, 23, 24, 25, 26, 27, 29);PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            UpdateGUID("Accounts")
        End If
        If clsFun.ExecScalarInt("Select Count(Tag) From Accounts Where TAG=1") = 0 Then
            clsFun.ExecNonQuery("DELETE FROM sqlite_sequence WHERE name = 'Accounts';INSERT INTO sqlite_sequence (name, seq) VALUES ('Accounts', 1000);")
        End If
    End Sub

    Public Sub UpdateGUID(tableName As String)
        Dim dt As DataTable
        Dim sql As String = "SELECT ID FROM " & tableName
        dt = clsFun.ExecDataTable(sql)
        For i = 0 To dt.Rows.Count - 1
            Dim id As Integer = dt.Rows(i)("id").ToString()
            Dim guid As Guid = guid.NewGuid()
            clsFun.ExecScalarStr("UPDATE " & tableName & " SET GUID = '" & guid.ToString & "' WHERE ID = " & id & "")
        Next
    End Sub

    Public Sub UserRights()
        Dim sql As String = String.Empty
        sql = "CREATE TABLE if  not exists PartyRates (ID INTEGER PRIMARY KEY AUTOINCREMENT,AccountID INTEGER,AccountName TEXT,ItemID INTEGER,ItemName TEXT,Rate DECIMAL);"
        clsFun.ExecNonQuery(sql)
        sql = String.Empty
        If clsFun.ExecScalarStr("SELECT Name FROM sqlite_master WHERE name='UserRights'") <> "UserRights" Then
            sql = "create table if not exists UserRights (UserTypeID INTEGER,UserTypeName TEXT," &
          "EntryType TEXT,Save TEXT,Modify TEXT,Remove TEXT,See TEXT," &
          "Print TEXT,Mobileapp TEXT,Server TEXT,Reports TEXT,BillPrints TEXT," &
          "AppPassword TEXT,DontAllowBack TEXT,Tag TEXT);"
            clsFun.ExecNonQuery(sql)
            sql = String.Empty
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
                    " ('1','Admin','Sale','Y','Y','Y','Y','Y',0);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
        " ('1','Admin','Purchase','Y','Y','Y','Y','Y',0);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
        " ('1','Admin','Voucher','Y','Y','Y','Y','Y',0);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
        " ('1','Admin','Sellout','Y','Y','Y','Y','Y',0);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
        " ('1','Admin','Crate','Y','Y','Y','Y','Y',0);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Mobileapp,Server,Reports,BillPrints,AppPassword,DontAllowBack,Tag) values " &
                         " ('1','Admin','Other','Y','Y','Y','Y', 'Y','N',0);"
            ''''Oprators Rights
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
         " ('2','Operator','Sale','Y','N','N','Y','Y',1);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
         " ('2','Operator','Purchase','Y','N','N','Y','Y',1);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
         " ('2','Operator','Voucher','Y','N','N','Y','Y',1);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
         " ('2','Operator','Sellout','Y','N','N','Y','Y',1);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " &
         " ('2','Operator','Crate','Y','N','N','Y','Y',1);"
            sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Mobileapp,Server,Reports,BillPrints,AppPassword,DontAllowBack,Tag) values " &
                         " ('2','Operator','Other','Y','N','Y','Y', 'N','N',1);"
            clsFun.ExecNonQuery(sql)
        End If

        If clsFun.CheckIfColumnExists("Users", "UserTypeID") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempUser AS SELECT *  FROM Users;" & _
                "DROP TABLE Users; CREATE TABLE Users (ID INTEGER PRIMARY KEY,UserTypeID INTEGER,UserType TEXT,FullName TEXT,UserName TEXT,Password TEXT,InActive TEXT,tag INTEGER);" & _
                "INSERT INTO Users (ID,UserName,Password,tag,UserTypeID,UserType,FullName,InActive)SELECT ID,UserName,Password,tag,1,'Admin',UserName,'N' FROM TempUser; " & _
                "DROP TABLE TempUser;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfColumnExists("Vouchers", "T10") = False Then
            sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempVoucher AS SELECT * FROM Vouchers;DROP TABLE Vouchers;" & _
                "CREATE TABLE Vouchers (ID INTEGER PRIMARY KEY AUTOINCREMENT,TransType TEXT,billNo TEXT," & _
                "VehicleNo  TEXT,Entrydate DATE,PurchaseType TEXT, sallerID   INTEGER, sallerName TEXT,ItemID INTEGER," & _
                "ItemName TEXT,AccountID  INTEGER,AccountName TEXT,Nug  DECIMAL,Kg   DECIMAL,Rate DECIMAL,Per  TEXT," & _
                "BasicAmount DECIMAL,DiscountAmount   DECIMAL,SubTotal DECIMAL,TotalAmount DECIMAL,CommissionPer DECIMAL," & _
                "CommissionAmount DECIMAL, MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount  DECIMAL,Tare DECIMAL," & _
                "TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT," & _
                "CrateID INTEGER,CrateName  TEXT,CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT," & _
                "StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL,N2 DECIMAL,N3 DECIMAL,N4 DECIMAL," & _
                "N5 DECIMAL,T1 TEXT,T2 TEXT,T3 TEXT,T4 TEXT,T5 TEXT,T6 TEXT,T7 TEXT,T8 TEXT,T9 TEXT,T10 TEXT,T11 TEXT,T12 TEXT,T13 TEXT,T14 TEXT,T15 TEXT,RoundOff DECIMAL, " & _
                "InvoiceID INTEGER,UserID INTEGER,EntryTime TEXT,ModifiedByID INTEGER,ModifiedTime TEXT);" & _
                "INSERT INTO Vouchers(ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName," & _
                "AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount," & _
                "MPer,MAmount,RdfPer,RdfAmount,Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty," & _
                "Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff,InvoiceID,UserID,ModifiedByID)" & _
                "SELECT ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName," & _
                "Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount," & _
                "Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo," & _
                "BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff,InvoiceID,1,1 FROM TempVoucher;" & _
                "DROP TABLE TempVoucher;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfColumnExists("Purchase", "StockID") = False Then
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempPurchase AS SELECT * FROM Purchase;DROP TABLE Purchase;CREATE TABLE Purchase(ID INTEGER PRIMARY KEY AUTOINCREMENT," & _
                  "EntryDate DATE,TransType TEXT,VoucherID INTEGER,BillNo TEXT,VehicleNo TEXT,PurchaseTypeName TEXT,AccountID INTEGER,AccountName TEXT,StorageID INTEGER," & _
                  "StorageName TEXT,ItemID INTEGER,ItemName TEXT,LotNo TEXT,Nug DECIMAL,Weight DECIMAL,Rate DECIMAL,Per TEXT,Amount DECIMAL,MaintainCrate TEXT,CrateID INTEGER," & _
                  "CrateName TEXT,CrateQty DECIMAL,StockID INTEGER,Remark TEXT,StockHolderID INTEGER,StockHoldername  TEXT,Roundoff DECIMAL);" & _
                  "INSERT INTO Purchase(ID,EntryDate,TransType, VoucherID, BillNo, VehicleNo, PurchaseTypeName, AccountID, AccountName, StorageID, StorageName, ItemID," & _
                  "ItemName, LotNo, Nug, Weight, Rate, Per, Amount, MaintainCrate, CrateID, CrateName, CrateQty, Remark, StockHolderID, StockHoldername, Roundoff)" & _
                  "SELECT ID,EntryDate,TransType,VoucherID,BillNo,VehicleNo,PurchaseTypeName,AccountID,AccountName,StorageID,StorageName,ItemID,ItemName,LotNo,Nug," & _
                  "Weight,Rate,Per,Amount,MaintainCrate,CrateID,CrateName,CrateQty,Remark,StockHolderID,StockHoldername,Roundoff  FROM TempPurchase;" & _
                  "DROP TABLE TempPurchase;CREATE INDEX StockHolderIndex ON Purchase(StockHolderID);PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If
        AccountsFields()
    End Sub
    Public Sub CompanyInfo()
        
        Dim Sql As String = "Select * From Company Where ID=" & Val(dg1.SelectedRows(0).Cells(0).Value) & ""
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable(Sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    sCompCode = dg1.SelectedRows(0).Cells(0).Value
                    compname = dg1.SelectedRows(0).Cells(1).Value
                    compnameHindi = dt.Rows(i)("PrintOtherName").ToString()
                    Mob1 = dt.Rows(i)("MobileNo1").ToString()
                    Mob2 = dt.Rows(i)("MobileNo2").ToString()
                    Address = dt.Rows(i)("Address").ToString()
                    AddressHindi = dt.Rows(i)("PrintOtheraddress").ToString()
                    City = dt.Rows(i)("City").ToString()
                    CityHindi = dt.Rows(i)("PrintOtherCity").ToString()
                    State = dt.Rows(i)("State").ToString()
                    StateHindi = dt.Rows(i)("PrintOtherState").ToString()
                    Fax = dt.Rows(i)("FaxNo").ToString()
                    Email = dt.Rows(i)("EmailID").ToString()
                    Phone = dt.Rows(i)("PhoneNo").ToString()
                    Website = dt.Rows(i)("Website").ToString()
                    Gstn = dt.Rows(i)("Gstn").ToString()
                    DealsIN = dt.Rows(i)("DealsIN").ToString()
                    Registration = dt.Rows(i)("RegistrationNo").ToString()
                    Pan = dt.Rows(i)("PanNo").ToString()
                    Marka = dt.Rows(i)("Marka").ToString()
                    other = dt.Rows(i)("Other").ToString()
                    OrgID = Val(dt.Rows(i)("OrganizationID").ToString())
                    GlobalData.PrvPath = dt.Rows(i)("LinkedDb").ToString()
                Next

            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        'clsFun.CloseConnection()
    End Sub

    Private Sub ReceiptUpdate()
        If clsFun.ExecScalarInt("Select Count(*) From Vouchers WHere TransType in ('Reciept','Group Reciept')") <> 0 Then
            Dim sql As String = String.Empty
            sql = "Update Vouchers Set TransType='Receipt' Where TransType='Reciept'; " & _
        "Update Ledger Set TransType='Receipt' Where TransType='Reciept';" & _
        "Update Vouchers Set TransType='Group Receipt' Where TransType='Group Reciept';" & _
        "Update Ledger Set TransType='Group Receipt' Where TransType='Group Receipt';"
            clsFun.ExecNonQuery(sql)
        End If
    End Sub
    Private Sub updateCompanytbl()
            Dim sql As String = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempCompany AS SELECT *  FROM Company;DROP TABLE Company;CREATE TABLE Company(ID INTEGER PRIMARY KEY AUTOINCREMENT," & _
                    "CompanyName TEXT,PrintOtherName TEXT,Address TEXT,PrintOtheraddress TEXT,City TEXT,PrintOtherCity TEXT,State TEXT,PrintOtherState TEXT," & _
                    "MobileNo1 TEXT,MobileNo2 TEXT,PhoneNo TEXT,FaxNo TEXT,EmailID TEXT,Website TEXT,GSTN TEXT,DealsIN TEXT,RegistrationNo TEXT,PanNo TEXT,Marka TEXT,Other TEXT,Logo  BLOB,YearStart DATE,Yearend   DATE,CompData  TEXT,LinkedDB TEXT," & _
                    "tag   TEXT,OrganizationID INTEGER,Password  TEXT,AutoSync  TEXT,SyncDate  TEXT);INSERT INTO Company (ID,CompanyName,PrintOtherName,Address,PrintOtheraddress,City,PrintOtherCity,State,PrintOtherState,MobileNo1,MobileNo2," & _
                    "PhoneNo,FaxNo,EmailID,Website,GSTN,DealsIN,RegistrationNo,PanNo,Marka,Other,Logo,YearStart,Yearend,CompData,tag,OrganizationID,Password,AutoSync,SyncDate)SELECT ID,CompanyName,PrintOtherName,Address,PrintOtheraddress,City,PrintOtherCity,State,PrintOtherState,MobileNo1," & _
                    "MobileNo2,PhoneNo,FaxNo,EmailID,Website,GSTN,DealsIN,RegistrationNo,PanNo,Marka,Other,Logo,YearStart,Yearend,CompData,tag,OrganizationID,Password,AutoSync,SyncDate FROM TempCompany;DROP TABLE TempCompany;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)

    End Sub



    Private Sub UpdateVoucherTbl()
        Dim sql As String
        If clsFun.CheckIfColumnExists("Vouchers", "RoundOff") = False Then
            '  UpdateVoucherTbl()
            If clsFun.CheckIfColumnExists("Vouchers", "N") = False Then
                sql = "drop table if  exists sqlitestudio_temp_table; PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS Select * FROM Vouchers;DROP TABLE Vouchers;" & _
                "CREATE TABLE Vouchers (ID INTEGER PRIMARY KEY AUTOINCREMENT,TransType TEXT,billNo TEXT,VehicleNo TEXT,Entrydate DATE, PurchaseType TEXT, " & _
                "sallerID INTEGER,sallerName TEXT,ItemID INTEGER,ItemName TEXT,AccountID INTEGER,AccountName TEXT,Nug DECIMAL,Kg DECIMAL,Rate DECIMAL,Per TEXT,BasicAmount DECIMAL " & _
                ",DiscountAmount DECIMAL,SubTotal DECIMAL,TotalAmount DECIMAL,CommissionPer DECIMAL,CommissionAmount DECIMAL,MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount DECIMAL, " & _
                "Tare DECIMAL,TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT,CrateID INTEGER,CrateName TEXT," & _
                "CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT,StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL,N2 DECIMAL,N3 DECIMAL,N4 DECIMAL,N5 DECIMAL,T1 TEXT,T2 TEXT,T3 TEXT,T4 TEXT,T5 TEXT,RoundOff DECIMAL);" & _
                "INSERT INTO Vouchers (ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount, " & _
                "Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff)" & _
                "Select ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount,Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff FROM sqlitestudio_temp_table;" & _
                "DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"
                clsFun.ExecNonQuery(sql)
            ElseIf clsFun.CheckIfColumnExists("Vouchers", "N5") = False Then
                sql = "drop table if  exists sqlitestudio_temp_table; PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS Select * FROM Vouchers;DROP TABLE Vouchers;" & _
              "CREATE TABLE Vouchers (ID INTEGER PRIMARY KEY AUTOINCREMENT,TransType TEXT,billNo TEXT,VehicleNo TEXT,Entrydate DATE, PurchaseType TEXT, " & _
              "sallerID INTEGER,sallerName TEXT,ItemID INTEGER,ItemName TEXT,AccountID INTEGER,AccountName TEXT,Nug DECIMAL,Kg DECIMAL,Rate DECIMAL,Per TEXT,BasicAmount DECIMAL " & _
              ",DiscountAmount DECIMAL,SubTotal DECIMAL,TotalAmount DECIMAL,CommissionPer DECIMAL,CommissionAmount DECIMAL,MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount DECIMAL, " & _
              "Tare DECIMAL,TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT,CrateID INTEGER,CrateName TEXT," & _
              "CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT,StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL,N2 DECIMAL,N3 DECIMAL,N4 DECIMAL,N5 DECIMAL,T1 TEXT,T2 TEXT,T3 TEXT,T4 TEXT,T5 TEXT,RoundOff DECIMAL);" & _
              "INSERT INTO Vouchers (ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount, " & _
              "Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5)" & _
              "Select ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount,Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N,T1,T2,T3,T4,T5 FROM sqlitestudio_temp_table;" & _
              "DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"
                clsFun.ExecNonQuery(sql)
            End If
        ElseIf clsFun.CheckIfColumnExists("Vouchers", "N") = True Then
            sql = "drop table if  exists sqlitestudio_temp_table; PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS Select * FROM Vouchers;DROP TABLE Vouchers;" & _
          "CREATE TABLE Vouchers (ID INTEGER PRIMARY KEY AUTOINCREMENT,TransType TEXT,billNo TEXT,VehicleNo TEXT,Entrydate DATE, PurchaseType TEXT, " & _
          "sallerID INTEGER,sallerName TEXT,ItemID INTEGER,ItemName TEXT,AccountID INTEGER,AccountName TEXT,Nug DECIMAL,Kg DECIMAL,Rate DECIMAL,Per TEXT,BasicAmount DECIMAL " & _
          ",DiscountAmount DECIMAL,SubTotal DECIMAL,TotalAmount DECIMAL,CommissionPer DECIMAL,CommissionAmount DECIMAL,MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount DECIMAL, " & _
          "Tare DECIMAL,TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT,CrateID INTEGER,CrateName TEXT," & _
          "CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT,StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL,N2 DECIMAL,N3 DECIMAL,N4 DECIMAL,N5 DECIMAL,T1 TEXT,T2 TEXT,T3 TEXT,T4 TEXT,T5 TEXT,RoundOff DECIMAL);" & _
          "INSERT INTO Vouchers (ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount, " & _
          "Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5)" & _
          "Select ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount,Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N,T1,T2,T3,T4,T5 FROM sqlitestudio_temp_table;" & _
          "DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If

        If clsFun.CheckIfColumnExists("Vouchers", "InvoiceID") = False Then
            sql = "drop table if  exists sqlitestudio_temp_table; PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS Select * FROM Vouchers;DROP TABLE Vouchers;" & _
             "CREATE TABLE Vouchers (ID INTEGER PRIMARY KEY AUTOINCREMENT,TransType TEXT,billNo TEXT,VehicleNo TEXT,Entrydate DATE, PurchaseType TEXT, " & _
             "sallerID INTEGER,sallerName TEXT,ItemID INTEGER,ItemName TEXT,AccountID INTEGER,AccountName TEXT,Nug DECIMAL,Kg DECIMAL,Rate DECIMAL,Per TEXT,BasicAmount DECIMAL " & _
             ",DiscountAmount DECIMAL,SubTotal DECIMAL,TotalAmount DECIMAL,CommissionPer DECIMAL,CommissionAmount DECIMAL,MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount DECIMAL, " & _
             "Tare DECIMAL,TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT,CrateID INTEGER,CrateName TEXT," & _
             "CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT,StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL,N2 DECIMAL,N3 DECIMAL,N4 DECIMAL,N5 DECIMAL,T1 TEXT,T2 TEXT,T3 TEXT,T4 TEXT,T5 TEXT,RoundOff DECIMAL,InvoiceID INTEGER);" & _
             "INSERT INTO Vouchers (ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount, " & _
             "Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff)" & _
             "Select ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount,Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff FROM sqlitestudio_temp_table;" & _
             "DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
        End If

        'If clsFun.CheckIfColumnExists("Vouchers", "IsCanceled") = False Then
        '    sql = "drop table if  exists sqlitestudio_temp_table; PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS Select * FROM Vouchers;DROP TABLE Vouchers;" & _
        ' "CREATE TABLE Vouchers (ID INTEGER PRIMARY KEY AUTOINCREMENT,TransType TEXT,billNo TEXT,VehicleNo TEXT,Entrydate DATE, PurchaseType TEXT, " & _
        ' "sallerID INTEGER,sallerName TEXT,ItemID INTEGER,ItemName TEXT,AccountID INTEGER,AccountName TEXT,Nug DECIMAL,Kg DECIMAL,Rate DECIMAL,Per TEXT,BasicAmount DECIMAL " & _
        ' ",DiscountAmount DECIMAL,SubTotal DECIMAL,TotalAmount DECIMAL,CommissionPer DECIMAL,CommissionAmount DECIMAL,MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount DECIMAL, " & _
        ' "Tare DECIMAL,TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT,CrateID INTEGER,CrateName TEXT," & _
        ' "CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT,StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL,N2 DECIMAL,N3 DECIMAL,N4 DECIMAL,N5 DECIMAL,T1 TEXT,T2 TEXT,T3 TEXT,T4 TEXT,T5 TEXT,RoundOff DECIMAL,InvoiceID INTEGER,IsCanceled TEXT);" & _
        ' "INSERT INTO Vouchers (ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount, " & _
        ' "Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff,InvoiceID)" & _
        ' "Select ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount,Tare,TareAmount,Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,RoundOff,InvoiceID FROM sqlitestudio_temp_table;" & _
        ' "DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"
        '    clsFun.ExecNonQuery(sql)
        'End If
    End Sub

    Private Sub UpdateVoucher()
        'Dim sql As String = "drop table if  exists sqlitestudio_temp_table; PRAGMA foreign_keys = 0; CREATE TABLE sqlitestudio_temp_table  AS  Select  *  FROM Company; DROP TABLE Company;CREATE TABLE Company ( ID                NUMERIC,    CompanyName       TEXT,    PrintOtherName    TEXT,    Address           TEXT,    PrintOtheraddress TEXT,    City              TEXT,    PrintOtherCity    TEXT,    State             TEXT,    PrintOtherState   TEXT,    MobileNo1         TEXT,    MobileNo2         TEXT,    PhoneNo           TEXT,    FaxNo             TEXT,    EmailID           TEXT,    Website           TEXT,    GSTN              TEXT,    DealsIN           TEXT,    RegistrationNo    TEXT,    PanNo             TEXT,    Marka             TEXT,    Other             TEXT,    Logo              BLOB,    YearStart         DATE,    Yearend           DATE,    CompData          TEXT,    tag               TEXT,    OrganizationID    INTEGER,    Password          TEXT,    Autosync          TEXT);" & _
        '                     "INSERT INTO Company ( ID, CompanyName, PrintOtherName, Address, PrintOtheraddress, City, PrintOtherCity,  State,  PrintOtherState,   MobileNo1,  MobileNo2,  PhoneNo, FaxNo,                     EmailID,  Website,   GSTN, DealsIN, RegistrationNo, PanNo, Marka, Other,  Logo, YearStart,                        Yearend, CompData, tag )  Select ID,  CompanyName,  PrintOtherName,   Address,                     PrintOtheraddress,   City, PrintOtherCity,  State, PrintOtherState, MobileNo1, MobileNo2,                    PhoneNo, FaxNo,  EmailID, Website,  GSTN,  DealsIN,RegistrationNo,PanNo,  Marka, Other,  Logo, YearStart,  Yearend,  CompData, tag  FROM sqlitestudio_temp_table;DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"
        Dim sql As String = String.Empty
        clsFun.ExecNonQuery(Sql)
        If clsFun.CheckIfColumnExists("Vouchers", "PaymentAmount") = False Then
            '  sql = String.Empty
            sql = "PRAGMA foreign_keys = 0;CREATE TABLE TempVoucher AS SELECT *  FROM Vouchers;DROP TABLE Vouchers; CREATE TABLE Vouchers (ID   INTEGER PRIMARY KEY AUTOINCREMENT,TransType TEXT,billNo TEXT,VehicleNo TEXT,Entrydate DATE," & _
           "PurchaseType TEXT,sallerID INTEGER,sallerName TEXT,ItemID INTEGER,ItemName TEXT,AccountID INTEGER,AccountName TEXT, Nug  DECIMAL,Kg DECIMAL,Rate DECIMAL,Per TEXT,BasicAmount DECIMAL,DiscountAmount DECIMAL,SubTotal DECIMAL,TotalAmount  DECIMAL," & _
           "CommissionPer DECIMAL,CommissionAmount DECIMAL,MPer DECIMAL,MAmount DECIMAL,RdfPer DECIMAL,RdfAmount DECIMAL,Tare DECIMAL,TareAmount DECIMAL,Labour DECIMAL,LabourAmount DECIMAL,TotalCharges DECIMAL,MaintainCrate TEXT,CrateID  INTEGER,CrateName TEXT," & _
           "CrateQty DECIMAL,Remark TEXT,ChequeDate TEXT,ChequeNo TEXT,BankEntry TEXT,StorageID INTEGER,StorageName TEXT,BillingType TEXT,N1 DECIMAL,N2 DECIMAL,N3 DECIMAL,N4 DECIMAL,N5 DECIMAL,T1  TEXT,T2 TEXT,T3   TEXT,T4 TEXT,T5 TEXT,T6 TEXT,T7 TEXT,T8 TEXT," & _
           "T9 TEXT,T10 TEXT,T11  TEXT,T12 TEXT,T13  TEXT,T14 TEXT,T15  TEXT,RoundOff DECIMAL,PaymentID INTEGER,PaymentAmount DECIMAL,InvoiceID INTEGER,UserID  INTEGER,EntryTime TEXT,ModifiedByID INTEGER,ModifiedTime TEXT);" & _
           "INSERT INTO Vouchers ( ID, TransType, billNo, VehicleNo, Entrydate, PurchaseType, sallerID,sallerName, ItemID, ItemName,AccountID, AccountName, Nug, Kg, Rate, Per, BasicAmount, DiscountAmount, SubTotal, TotalAmount, CommissionPer, " & _
           "CommissionAmount, MPer, MAmount, RdfPer, RdfAmount, Tare, TareAmount, Labour, LabourAmount, TotalCharges, MaintainCrate,CrateID, CrateName, CrateQty, Remark, ChequeDate, ChequeNo, BankEntry, StorageID, StorageName, BillingType, N1, N2, N3," & _
           "N4, N5, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, RoundOff, InvoiceID, UserID, EntryTime,ModifiedByID, ModifiedTime ) " & _
           "SELECT ID,TransType,billNo,VehicleNo,Entrydate,PurchaseType,sallerID,sallerName,ItemID,ItemName,AccountID,AccountName,Nug,Kg,Rate,Per,BasicAmount,DiscountAmount,SubTotal,TotalAmount,CommissionPer,CommissionAmount,MPer,MAmount,RdfPer,RdfAmount,Tare,TareAmount," & _
           "Labour,LabourAmount,TotalCharges,MaintainCrate,CrateID,CrateName,CrateQty,Remark,ChequeDate,ChequeNo,BankEntry,StorageID,StorageName,BillingType,N1,N2,N3,N4,N5,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,RoundOff,InvoiceID,UserID,EntryTime," & _
           "ModifiedByID,ModifiedTime FROM TempVoucher;DROP TABLE TempVoucher;PRAGMA foreign_keys = 1;"
            clsFun.ExecNonQuery(sql)
            clsFun.ExecScalarStr("Update Vouchers Set PaymentID=ID Where TransType in('Receipt','Payment')")
        End If

    End Sub


    Private Sub UpdateCrateTable()
        Dim sql As String = " drop table if  exists sqlitestudio_temp_table; PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS Select * FROM CrateVoucher; " & _
       " DROP TABLE CrateVoucher;CREATE TABLE CrateVoucher (    ID          INTEGER PRIMARY KEY AUTOINCREMENT,    VoucherID   INTEGER,    SlipNo      TEXT,    EntryDate   DATE,    TransType   TEXT,    AccountID   INTEGER,    AccountName TEXT,    CrateType   TEXT,    CrateID     INTEGER,    CrateName   TEXT,    Qty         INTEGER,    Remark      TEXT,    Rate        DECIMAL,    Amount      DECIMAL, CashPaid    TEXT);" & _
       " INSERT INTO CrateVoucher (        ID,        VoucherID,        SlipNo,        EntryDate,        TransType,        AccountID,        AccountName,        CrateType,        CrateID,        CrateName,        Qty,        Remark )  Select ID,        VoucherID,        SlipNo,        EntryDate,        TransType,        AccountID,        AccountName,        CrateType,        CrateID,        CrateName,        Qty,        Remark  FROM sqlitestudio_temp_table;" & _
      "  DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"

        clsFun.ExecNonQuery(sql)
    End Sub
    Private Sub OptionsControl()
        If clsFun.CheckIfColumnExists("Controls", "RcptDate") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE TempControls AS SELECT * FROM Controls;DROP TABLE Controls;" & _
                "CREATE TABLE Controls (Decimals TEXT,AskReciept TEXT,AskPayment TEXT,AskMannual TEXT,AskAuto TEXT,sendDiff TEXT,CrateBardana TEXT,ChargeEffect TEXT,TareSameAc TEXT,DefaultDate TEXT,PurcahseRO TEXT,SaleRO TEXT,ROEachItem TEXT,AskCreditLimit TEXT,ApplyCommWeight TEXT, " & _
                "Octroi TEXT,SpeedCommission TEXT,SpeedMandiTax TEXT,SpeedRDF TEXT,SpeedTare TEXT,SpeedLabour TEXT,SpeedTaxPaid  TEXT, SpeedKaat TEXT,SuperCommission TEXT,SuperMandiTax TEXT,SuperRDF TEXT,SuperTare TEXT,SuperLabour TEXT,SuperTaxPaid TEXT,SuperKaat TEXT,SuperVehicleNo  TEXT," & _
                "STDCommission TEXT,STDMandiTax TEXT,STDRDF TEXT,STDTare TEXT,STDLabour TEXT,StdTaxPaid TEXT,StdKaat TEXT,StockCommission TEXT,StockMandiTax TEXT,StockRDF TEXT,StockTare TEXT,StockLabour TEXT,StockTaxPaid TEXT,StockKaat TEXT,RcptDate TEXT,RcptSlip TEXT, " & _
                "RcptDisc TEXT,RcptTotal TEXT,RcptRemark TEXT,PymtDate TEXT,PymtSlip TEXT,PymtDisc TEXT,PymtTotal TEXT,PymtRemark TEXT);INSERT INTO Controls (Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, DefaultDate, PurcahseRO, SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission," & _
                " SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, STDCommission, STDMandiTax, STDRDF,STDTare, STDLabour, StdTaxPaid, StdKaat, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, StockTaxPaid, StockKaat) SELECT Decimals,    AskReciept,    AskPayment," & _
                "AskMannual,    AskAuto,    sendDiff,    CrateBardana,    ChargeEffect,    TareSameAc,    DefaultDate,    PurcahseRO,    SaleRO,    ROEachItem,    AskCreditLimit,    ApplyCommWeight,Octroi,    SpeedCommission,    SpeedMandiTax,    SpeedRDF,    SpeedTare,    SpeedLabour,    SpeedTaxPaid,    SpeedKaat,    SuperCommission,    SuperMandiTax,    SuperRDF,    SuperTare,    SuperLabour,    SuperTaxPaid,    SuperKaat,    STDCommission," & _
                "    STDMandiTax,    STDRDF,    STDTare,    STDLabour,    StdTaxPaid,    StdKaat,    StockCommission,    StockMandiTax,    StockRDF,    StockTare,    StockLabour,    StockTaxPaid,    StockKaat  FROM TempControls;DROP TABLE TempControls;PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set RcptDate='Yes',RcptSlip='Yes',RcptDisc='Yes',RcptTotal='Yes',RcptRemark='Yes',PymtDate='Yes',PymtSlip='Yes',PymtDisc='Yes',PymtTotal='Yes',PymtRemark='Yes',SuperVehicleNo='Yes'"
            clsFun.ExecScalarStr(Sql)
        End If
        If clsFun.CheckIfColumnExists("Controls", "StopBasic") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE TempControls AS SELECT * FROM Controls;DROP TABLE Controls;" & _
                "CREATE TABLE Controls(Decimals TEXT,AskReciept  TEXT,AskPayment  TEXT,AskMannual  TEXT,AskAuto TEXT,sendDiff TEXT,CrateBardana TEXT,ChargeEffect TEXT,TareSameAc TEXT," & _
                "DefaultDate TEXT,PurcahseRO TEXT,SaleRO  TEXT,ROEachItem  TEXT,AskCreditLimit TEXT,ApplyCommWeight TEXT,Octroi  TEXT,SpeedCommission TEXT,SpeedMandiTax TEXT,SpeedRDF TEXT," & _
                "SpeedTare   TEXT,SpeedLabour TEXT,SpeedTaxPaid TEXT,SpeedKaat TEXT,SuperCommission TEXT,SuperMandiTax   TEXT,SuperRDF TEXT,SuperTare TEXT,SuperLabour TEXT,SuperTaxPaid TEXT,SuperKaat TEXT,SuperVehicleNo  TEXT,STDCommission TEXT,STDMandiTax TEXT,STDRDF TEXT,STDTare TEXT,STDLabour TEXT,StdTaxPaid  TEXT,StdKaat TEXT," & _
                "StockCommission TEXT,StockMandiTax TEXT,StockRDF TEXT,StockTare TEXT,StockLabour TEXT,StockTaxPaid TEXT,StockKaat TEXT,RcptDate TEXT,RcptSlip TEXT,RcptDisc TEXT,RcptTotal   TEXT,RcptRemark  TEXT,PymtDate TEXT,PymtSlip TEXT,PymtDisc TEXT,PymtTotal TEXT,PymtRemark  TEXT,StopBasic TEXT);" & _
                "INSERT INTO Controls( Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, DefaultDate, PurcahseRO," & _
                "SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, STDCommission, STDMandiTax, STDRDF, STDTare, STDLabour," & _
                "StdTaxPaid, StdKaat, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark) SELECT Decimals,AskReciept,AskPayment," & _
                "AskMannual,AskAuto,sendDiff,CrateBardana,ChargeEffect,TareSameAc,DefaultDate,PurcahseRO,SaleRO,ROEachItem,AskCreditLimit,ApplyCommWeight,Octroi,SpeedCommission,SpeedMandiTax,SpeedRDF,SpeedTare," & _
                "SpeedLabour,SpeedTaxPaid,SpeedKaat,SuperCommission,SuperMandiTax,SuperRDF,SuperTare,SuperLabour,SuperTaxPaid,SuperKaat,SuperVehicleNo,STDCommission,STDMandiTax,STDRDF,STDTare,STDLabour," & _
                "StdTaxPaid,StdKaat,StockCommission,StockMandiTax,StockRDF,StockTare,StockLabour,StockTaxPaid,StockKaat,RcptDate,RcptSlip,RcptDisc,RcptTotal,RcptRemark,PymtDate,PymtSlip," & _
                "PymtDisc,PymtTotal,PymtRemark FROM TempControls;DROP TABLE TempControls;PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set StopBasic='No'"
            clsFun.ExecScalarStr(Sql)
        End If
        If clsFun.CheckIfColumnExists("Controls", "AutoSwitch") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE TempControls AS SELECT * FROM Controls;DROP TABLE Controls;" & _
                "CREATE TABLE Controls(Decimals TEXT,AskReciept  TEXT,AskPayment  TEXT,AskMannual  TEXT,AskAuto TEXT,sendDiff TEXT,CrateBardana TEXT,ChargeEffect TEXT,TareSameAc TEXT," & _
                "DefaultDate TEXT,PurcahseRO TEXT,SaleRO  TEXT,ROEachItem  TEXT,AskCreditLimit TEXT,ApplyCommWeight TEXT,Octroi  TEXT,SpeedCommission TEXT,SpeedMandiTax TEXT,SpeedRDF TEXT," & _
                "SpeedTare   TEXT,SpeedLabour TEXT,SpeedTaxPaid TEXT,SpeedKaat TEXT,SuperCommission TEXT,SuperMandiTax   TEXT,SuperRDF TEXT,SuperTare TEXT,SuperLabour TEXT,SuperTaxPaid TEXT,SuperKaat TEXT,SuperVehicleNo  TEXT,STDCommission TEXT,STDMandiTax TEXT,STDRDF TEXT,STDTare TEXT,STDLabour TEXT,StdTaxPaid  TEXT,StdKaat TEXT," & _
                "StockCommission TEXT,StockMandiTax TEXT,StockRDF TEXT,StockTare TEXT,StockLabour TEXT,StockTaxPaid TEXT,StockKaat TEXT,RcptDate TEXT,RcptSlip TEXT,RcptDisc TEXT,RcptTotal   TEXT,RcptRemark  TEXT,PymtDate TEXT,PymtSlip TEXT,PymtDisc TEXT,PymtTotal TEXT,PymtRemark  TEXT,StopBasic TEXT,AutoSwitch TEXT,OnSaleNet TEXT);" & _
                "INSERT INTO Controls( Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, DefaultDate, PurcahseRO," & _
                "SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, STDCommission, STDMandiTax, STDRDF, STDTare, STDLabour," & _
                "StdTaxPaid, StdKaat, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark,StopBasic) SELECT Decimals,AskReciept,AskPayment," & _
                "AskMannual,AskAuto,sendDiff,CrateBardana,ChargeEffect,TareSameAc,DefaultDate,PurcahseRO,SaleRO,ROEachItem,AskCreditLimit,ApplyCommWeight,Octroi,SpeedCommission,SpeedMandiTax,SpeedRDF,SpeedTare," & _
                "SpeedLabour,SpeedTaxPaid,SpeedKaat,SuperCommission,SuperMandiTax,SuperRDF,SuperTare,SuperLabour,SuperTaxPaid,SuperKaat,SuperVehicleNo,STDCommission,STDMandiTax,STDRDF,STDTare,STDLabour," & _
                "StdTaxPaid,StdKaat,StockCommission,StockMandiTax,StockRDF,StockTare,StockLabour,StockTaxPaid,StockKaat,RcptDate,RcptSlip,RcptDisc,RcptTotal,RcptRemark,PymtDate,PymtSlip," & _
                "PymtDisc,PymtTotal,PymtRemark,StopBasic FROM TempControls;DROP TABLE TempControls;PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set AutoSwitch='No',OnSaleNet='No'"
            clsFun.ExecScalarStr(Sql)
        End If
        If clsFun.CheckIfColumnExists("Controls", "Margin") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE TempControls AS SELECT * FROM Controls;DROP TABLE Controls;" & _
                "CREATE TABLE Controls(Decimals TEXT,AskReciept  TEXT,AskPayment  TEXT,AskMannual  TEXT,AskAuto TEXT,sendDiff TEXT,CrateBardana TEXT,ChargeEffect TEXT,TareSameAc TEXT," & _
                "DefaultDate TEXT,PurcahseRO TEXT,SaleRO  TEXT,ROEachItem  TEXT,AskCreditLimit TEXT,ApplyCommWeight TEXT,Octroi  TEXT,SpeedCommission TEXT,SpeedMandiTax TEXT,SpeedRDF TEXT," & _
                "SpeedTare   TEXT,SpeedLabour TEXT,SpeedTaxPaid TEXT,SpeedKaat TEXT,SuperCommission TEXT,SuperMandiTax   TEXT,SuperRDF TEXT,SuperTare TEXT,SuperLabour TEXT,SuperTaxPaid TEXT,SuperKaat TEXT,SuperVehicleNo  TEXT,STDCommission TEXT,STDMandiTax TEXT,STDRDF TEXT,STDTare TEXT,STDLabour TEXT,StdTaxPaid  TEXT,StdKaat TEXT," & _
                "StockCommission TEXT,StockMandiTax TEXT,StockRDF TEXT,StockTare TEXT,StockLabour TEXT,StockTaxPaid TEXT,StockKaat TEXT,RcptDate TEXT,RcptSlip TEXT,RcptDisc TEXT,RcptTotal   TEXT,RcptRemark  TEXT,PymtDate TEXT,PymtSlip TEXT,PymtDisc TEXT,PymtTotal TEXT,PymtRemark  TEXT,StopBasic TEXT,AutoSwitch TEXT,OnSaleNet TEXT,Margin Integer,Language TEXT,Per TEXT);" & _
                "INSERT INTO Controls( Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, DefaultDate, PurcahseRO," & _
                "SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, STDCommission, STDMandiTax, STDRDF, STDTare, STDLabour," & _
                "StdTaxPaid, StdKaat, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark,StopBasic,AutoSwitch,OnSaleNet) SELECT Decimals,AskReciept,AskPayment," & _
                "AskMannual,AskAuto,sendDiff,CrateBardana,ChargeEffect,TareSameAc,DefaultDate,PurcahseRO,SaleRO,ROEachItem,AskCreditLimit,ApplyCommWeight,Octroi,SpeedCommission,SpeedMandiTax,SpeedRDF,SpeedTare," & _
                "SpeedLabour,SpeedTaxPaid,SpeedKaat,SuperCommission,SuperMandiTax,SuperRDF,SuperTare,SuperLabour,SuperTaxPaid,SuperKaat,SuperVehicleNo,STDCommission,STDMandiTax,STDRDF,STDTare,STDLabour," & _
                "StdTaxPaid,StdKaat,StockCommission,StockMandiTax,StockRDF,StockTare,StockLabour,StockTaxPaid,StockKaat,RcptDate,RcptSlip,RcptDisc,RcptTotal,RcptRemark,PymtDate,PymtSlip," & _
                "PymtDisc,PymtTotal,PymtRemark,StopBasic,AutoSwitch,OnSaleNet FROM TempControls;DROP TABLE TempControls;PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set AutoSwitch='No',OnSaleNet='No',Margin=1,Language='Hindi',Per='Nug'"
            clsFun.ExecScalarStr(Sql)
        End If
        If clsFun.CheckIfColumnExists("Controls", "StdMark") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE TempControls AS SELECT * FROM Controls;DROP TABLE Controls;" & _
                "CREATE TABLE Controls(Decimals TEXT,AskReciept  TEXT,AskPayment  TEXT,AskMannual  TEXT,AskAuto TEXT,sendDiff TEXT,CrateBardana TEXT,ChargeEffect TEXT,TareSameAc TEXT," & _
                "DefaultDate TEXT,PurcahseRO TEXT,SaleRO  TEXT,ROEachItem  TEXT,AskCreditLimit TEXT,ApplyCommWeight TEXT,Octroi  TEXT,SpeedCommission TEXT,SpeedMandiTax TEXT,SpeedRDF TEXT," & _
                "SpeedTare   TEXT,SpeedLabour TEXT,SpeedTaxPaid TEXT,SpeedKaat TEXT,SuperCommission TEXT,SuperMandiTax   TEXT,SuperRDF TEXT,SuperTare TEXT,SuperLabour TEXT,SuperTaxPaid TEXT,SuperKaat TEXT,SuperVehicleNo  TEXT,STDCommission TEXT,STDMandiTax TEXT,STDRDF TEXT,STDTare TEXT,STDLabour TEXT,StdTaxPaid  TEXT,StdKaat TEXT,StdMark TEXT,STDNoLot TEXT," & _
                "StockCommission TEXT,StockMandiTax TEXT,StockRDF TEXT,StockTare TEXT,StockLabour TEXT,StockTaxPaid TEXT,StockKaat TEXT,RcptDate TEXT,RcptSlip TEXT,RcptDisc TEXT,RcptTotal   TEXT,RcptRemark  TEXT,PymtDate TEXT,PymtSlip TEXT,PymtDisc TEXT,PymtTotal TEXT,PymtRemark  TEXT,StopBasic TEXT,AutoSwitch TEXT,OnSaleNet TEXT,Margin Integer,Language TEXT,Per TEXT);" & _
                "INSERT INTO Controls( Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, DefaultDate, PurcahseRO," & _
                "SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, STDCommission, STDMandiTax, STDRDF, STDTare, STDLabour," & _
                "StdTaxPaid, StdKaat, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark,StopBasic,AutoSwitch,OnSaleNet,Language,Per) SELECT Decimals,AskReciept,AskPayment," & _
                "AskMannual,AskAuto,sendDiff,CrateBardana,ChargeEffect,TareSameAc,DefaultDate,PurcahseRO,SaleRO,ROEachItem,AskCreditLimit,ApplyCommWeight,Octroi,SpeedCommission,SpeedMandiTax,SpeedRDF,SpeedTare," & _
                "SpeedLabour,SpeedTaxPaid,SpeedKaat,SuperCommission,SuperMandiTax,SuperRDF,SuperTare,SuperLabour,SuperTaxPaid,SuperKaat,SuperVehicleNo,STDCommission,STDMandiTax,STDRDF,STDTare,STDLabour," & _
                "StdTaxPaid,StdKaat,StockCommission,StockMandiTax,StockRDF,StockTare,StockLabour,StockTaxPaid,StockKaat,RcptDate,RcptSlip,RcptDisc,RcptTotal,RcptRemark,PymtDate,PymtSlip," & _
                "PymtDisc,PymtTotal,PymtRemark,StopBasic,AutoSwitch,OnSaleNet,Language,Per FROM TempControls;DROP TABLE TempControls;PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set STDMark='No',STDNoLot='No'"
            clsFun.ExecScalarStr(Sql)
        End If
        If clsFun.CheckIfColumnExists("Controls", "SuperBasic") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE TempControls AS SELECT * FROM Controls;DROP TABLE Controls;" & _
                "CREATE TABLE Controls(Decimals TEXT,AskReciept  TEXT,AskPayment  TEXT,AskMannual  TEXT,AskAuto TEXT,sendDiff TEXT,CrateBardana TEXT,ChargeEffect TEXT,TareSameAc TEXT," & _
                "DefaultDate TEXT,PurcahseRO TEXT,SaleRO  TEXT,ROEachItem  TEXT,AskCreditLimit TEXT,ApplyCommWeight TEXT,Octroi  TEXT,SpeedCommission TEXT,SpeedMandiTax TEXT,SpeedRDF TEXT," & _
                "SpeedTare   TEXT,SpeedLabour TEXT,SpeedTaxPaid TEXT,SpeedKaat TEXT,SuperCommission TEXT,SuperMandiTax   TEXT,SuperRDF TEXT,SuperTare TEXT,SuperLabour TEXT,SuperTaxPaid TEXT,SuperKaat TEXT,SuperVehicleNo  TEXT,SuperBasic  TEXT,STDCommission TEXT,STDMandiTax TEXT,STDRDF TEXT,STDTare TEXT,STDLabour TEXT,StdTaxPaid  TEXT,StdKaat TEXT,StdMark TEXT,STDNoLot TEXT," & _
                "StockCommission TEXT,StockMandiTax TEXT,StockRDF TEXT,StockTare TEXT,StockLabour TEXT,StockTaxPaid TEXT,StockKaat TEXT,RcptDate TEXT,RcptSlip TEXT,RcptDisc TEXT,RcptTotal   TEXT,RcptRemark  TEXT,PymtDate TEXT,PymtSlip TEXT,PymtDisc TEXT,PymtTotal TEXT,PymtRemark  TEXT,StopBasic TEXT,AutoSwitch TEXT,OnSaleNet TEXT,Margin Integer,Language TEXT,Per TEXT);" & _
                "INSERT INTO Controls( Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, DefaultDate, PurcahseRO," & _
                "SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, STDCommission, STDMandiTax, STDRDF, STDTare, STDLabour," & _
                "StdTaxPaid, StdKaat, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark,StopBasic,AutoSwitch,OnSaleNet,Language,Per,STDMark,STDNoLot) SELECT Decimals,AskReciept,AskPayment," & _
                "AskMannual,AskAuto,sendDiff,CrateBardana,ChargeEffect,TareSameAc,DefaultDate,PurcahseRO,SaleRO,ROEachItem,AskCreditLimit,ApplyCommWeight,Octroi,SpeedCommission,SpeedMandiTax,SpeedRDF,SpeedTare," & _
                "SpeedLabour,SpeedTaxPaid,SpeedKaat,SuperCommission,SuperMandiTax,SuperRDF,SuperTare,SuperLabour,SuperTaxPaid,SuperKaat,SuperVehicleNo,STDCommission,STDMandiTax,STDRDF,STDTare,STDLabour," & _
                "StdTaxPaid,StdKaat,StockCommission,StockMandiTax,StockRDF,StockTare,StockLabour,StockTaxPaid,StockKaat,RcptDate,RcptSlip,RcptDisc,RcptTotal,RcptRemark,PymtDate,PymtSlip," & _
                "PymtDisc,PymtTotal,PymtRemark,StopBasic,AutoSwitch,OnSaleNet,Language,Per,STDMark,STDNoLot FROM TempControls;DROP TABLE TempControls;PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set SuperBasic='No'"
            clsFun.ExecScalarStr(Sql)
        End If

        If clsFun.CheckIfColumnExists("Controls", "AskFarmer") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0; CREATE TABLE tempControls AS SELECT * FROM Controls; DROP TABLE Controls; CREATE TABLE Controls (" & _
        "Decimals TEXT, AskReciept TEXT, AskPayment TEXT, AskMannual TEXT, AskAuto TEXT, sendDiff TEXT, CrateBardana TEXT, ChargeEffect TEXT, " & _
        "TareSameAc TEXT, DefaultDate TEXT, PurcahseRO TEXT, SaleRO TEXT, ROEachItem TEXT, AskCreditLimit TEXT, ApplyCommWeight TEXT, Octroi TEXT, " & _
        "SpeedCommission TEXT, SpeedMandiTax TEXT, SpeedRDF TEXT, SpeedTare TEXT, SpeedLabour TEXT, SpeedTaxPaid TEXT, SpeedKaat TEXT, SuperCommission TEXT, " & _
        "SuperMandiTax TEXT, SuperRDF TEXT, SuperTare TEXT, SuperLabour TEXT, SuperTaxPaid TEXT, SuperKaat TEXT, SuperVehicleNo TEXT, SuperBasic TEXT, " & _
        "STDCommission TEXT, STDMandiTax TEXT, STDRDF TEXT, STDTare TEXT, STDLabour TEXT, StdTaxPaid TEXT, StdKaat TEXT, StdMark TEXT, STDNoLot TEXT, " & _
        "StockCommission TEXT, StockMandiTax TEXT, StockRDF TEXT, StockTare TEXT, StockLabour TEXT, StockTaxPaid TEXT, StockKaat TEXT, RcptDate TEXT, RcptSlip TEXT, " & _
        "RcptDisc TEXT, RcptTotal TEXT, RcptRemark TEXT, PymtDate TEXT, PymtSlip TEXT, PymtDisc TEXT, PymtTotal TEXT, PymtRemark TEXT, StopBasic TEXT, " & _
        "AutoSwitch TEXT, OnSaleNet TEXT, Margin INTEGER, Language TEXT, Per TEXT,AskFarmer TEXT); INSERT INTO Controls (Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, " & _
        "TareSameAc, DefaultDate, PurcahseRO, SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, " & _
        "SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, SuperBasic, STDCommission, " & _
        "STDMandiTax, STDRDF, STDTare, STDLabour, StdTaxPaid, StdKaat, StdMark, STDNoLot, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, " & _
        "StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark, StopBasic, " & _
        "AutoSwitch, OnSaleNet, Margin, Language, Per) SELECT Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, " & _
        "DefaultDate, PurcahseRO, SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, " & _
        "SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, SuperBasic, STDCommission, " & _
        "STDMandiTax, STDRDF, STDTare, STDLabour, StdTaxPaid, StdKaat, StdMark, STDNoLot, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, " & _
        "StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark, StopBasic, " & _
        "AutoSwitch, OnSaleNet, Margin, Language, Per FROM tempControls; DROP TABLE tempControls; PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set AskFarmer='No'"
            clsFun.ExecScalarStr(Sql)
        End If

        If clsFun.CheckIfColumnExists("Controls", "SelloutRemark") = False Then
            Dim Sql As String = "PRAGMA foreign_keys = 0; CREATE TABLE tempControls AS SELECT * FROM Controls; DROP TABLE Controls; CREATE TABLE Controls (" & _
        "Decimals TEXT, AskReciept TEXT, AskPayment TEXT, AskMannual TEXT, AskAuto TEXT, sendDiff TEXT, CrateBardana TEXT, ChargeEffect TEXT, " & _
        "TareSameAc TEXT, DefaultDate TEXT, PurcahseRO TEXT, SaleRO TEXT, ROEachItem TEXT, AskCreditLimit TEXT, ApplyCommWeight TEXT, Octroi TEXT, " & _
        "SpeedCommission TEXT, SpeedMandiTax TEXT, SpeedRDF TEXT, SpeedTare TEXT, SpeedLabour TEXT, SpeedTaxPaid TEXT, SpeedKaat TEXT, SuperCommission TEXT, " & _
        "SuperMandiTax TEXT, SuperRDF TEXT, SuperTare TEXT, SuperLabour TEXT, SuperTaxPaid TEXT, SuperKaat TEXT, SuperVehicleNo TEXT, SuperBasic TEXT, " & _
        "STDCommission TEXT, STDMandiTax TEXT, STDRDF TEXT, STDTare TEXT, STDLabour TEXT, StdTaxPaid TEXT, StdKaat TEXT, StdMark TEXT, STDNoLot TEXT, " & _
        "StockCommission TEXT, StockMandiTax TEXT, StockRDF TEXT, StockTare TEXT, StockLabour TEXT, StockTaxPaid TEXT, StockKaat TEXT, RcptDate TEXT, RcptSlip TEXT, " & _
        "RcptDisc TEXT, RcptTotal TEXT, RcptRemark TEXT, PymtDate TEXT, PymtSlip TEXT, PymtDisc TEXT, PymtTotal TEXT, PymtRemark TEXT, StopBasic TEXT, " & _
        "AutoSwitch TEXT, OnSaleNet TEXT, Margin INTEGER, Language TEXT, Per TEXT,AskFarmer TEXT,SelloutRemark); INSERT INTO Controls (Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, " & _
        "TareSameAc, DefaultDate, PurcahseRO, SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, " & _
        "SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, SuperBasic, STDCommission, " & _
        "STDMandiTax, STDRDF, STDTare, STDLabour, StdTaxPaid, StdKaat, StdMark, STDNoLot, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, " & _
        "StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark, StopBasic, " & _
        "AutoSwitch, OnSaleNet, Margin, Language, Per,AskFarmer) SELECT Decimals, AskReciept, AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect, TareSameAc, " & _
        "DefaultDate, PurcahseRO, SaleRO, ROEachItem, AskCreditLimit, ApplyCommWeight, Octroi, SpeedCommission, SpeedMandiTax, SpeedRDF, SpeedTare, SpeedLabour, " & _
        "SpeedTaxPaid, SpeedKaat, SuperCommission, SuperMandiTax, SuperRDF, SuperTare, SuperLabour, SuperTaxPaid, SuperKaat, SuperVehicleNo, SuperBasic, STDCommission, " & _
        "STDMandiTax, STDRDF, STDTare, STDLabour, StdTaxPaid, StdKaat, StdMark, STDNoLot, StockCommission, StockMandiTax, StockRDF, StockTare, StockLabour, " & _
        "StockTaxPaid, StockKaat, RcptDate, RcptSlip, RcptDisc, RcptTotal, RcptRemark, PymtDate, PymtSlip, PymtDisc, PymtTotal, PymtRemark, StopBasic, " & _
        "AutoSwitch, OnSaleNet, Margin, Language, Per,AskFarmer FROM tempControls; DROP TABLE tempControls; PRAGMA foreign_keys = 1;"
            clsFun.ExecScalarStr(Sql)
            Sql = "Update Controls Set SelloutRemark='Full'"
            clsFun.ExecScalarStr(Sql)
        End If
    End Sub

    Private Sub CreateOptions()
        Dim sql As String = String.Empty
        If clsFun.ExecScalarInt("Select Count(*) From Accounts Where ID=56") = 0 Then
            sql = "Insert Into Accounts (ID,AccountName,GroupID,DC,Tag,Opbal,OtherName) values(56,'Difference A/c',10,'Dr',0,0,'Difference A/c')"
            clsFun.ExecNonQuery(Sql)
        End If
        If clsFun.ExecScalarInt("Select Count(*) From Accounts Where ID=57") = 0 Then
            sql = "Insert Into Accounts (ID,AccountName,GroupID,DC,Tag,Opbal,OtherName) values(57,'Crate A/c',25,'Cr',0,0,'Crate A/c')"
            clsFun.ExecNonQuery(sql)
        End If

        sql = "CREATE TABLE if not exists Transaction3 (VoucherID INTEGER,TransType TEXT,AccountID INTEGER,AccountName TEXT,Amount DECIMAL,Remark TEXT);"
        clsFun.ExecNonQuery(sql)
        sql = "CREATE TABLE if not Exists ExpVouchers ( ID   INTEGER PRIMARY KEY AUTOINCREMENT, EntryDate TEXT,ApplyID INTEGER, ApplyOn   TEXT,OnItem    TEXT); " &
            "CREATE TABLE if not Exists ExpControl (VoucherID INTEGER ,SRNo TEXT,ApplyOn TEXT,OnItem TEXT,ChargesID   INTEGER,ChargesName TEXT,FixAs   DECIMAL);"
        clsFun.ExecNonQuery(sql)
        sql = String.Empty
        sql = "CREATE TABLE if not exists Controls (Decimals TEXT,AskReciept TEXT,AskPayment TEXT,AskMannual TEXT,AskAuto TEXT,sendDiff " & _
            " TEXT,CrateBardana TEXT,ChargeEffect TEXT,TareSameAc TEXT,DefaultDate TEXT,PurcahseRO TEXT,SaleRO TEXT,ROEachItem TEXT," & _
            " AskCreditLimit TEXT,ApplyCommWeight TEXT,Octroi TEXT,SpeedCommission TEXT,SpeedMandiTax TEXT,SpeedRDF TEXT, " & _
            " SpeedTare TEXT,SpeedLabour TEXT, SpeedTaxPaid TEXT,SpeedKaat TEXT,SuperCommission TEXT,SuperMandiTax TEXT,SuperRDF TEXT,SuperTare " & _
            " TEXT,SuperLabour TEXT,SuperTaxPaid TEXT,SuperKaat TEXT,STDCommission TEXT,STDMandiTax TEXT,STDRDF TEXT,STDTare TEXT,STDLabour " & _
            " TEXT,StdTaxPaid TEXT,StdKaat TEXT,StockCommission TEXT,StockMandiTax TEXT,StockRDF TEXT,StockTare TEXT,StockLabour TEXT," & _
            " StockTaxPaid TEXT,StockKaat TEXT);"
        clsFun.ExecNonQuery(sql, True)
        If clsFun.ExecScalarInt("Select Count(*) From Controls") = 0 Then
            Dim Octroi As String : Dim ISDiff As String
            Dim BardanaInSameAccount As String : Dim SellOutCharges As String
            Dim CrateRate As String
            If clsFun.ExecScalarInt("SELECT Count(*) FROM sqlite_master WHERE type='table' AND name='{Options}'") > 0 Then
                Octroi = clsFun.ExecScalarStr("Select Octroi From Options")
                If Octroi = "Y" Then Octroi = "Yes" Else Octroi = "No"
                ISDiff = clsFun.ExecScalarStr("Select ISDiff From Options")
                If ISDiff = "Y" Then ISDiff = "Yes" Else ISDiff = "No"
                BardanaInSameAccount = clsFun.ExecScalarStr("Select BardanaInSameAccount From Options")
                If BardanaInSameAccount = "Y" Then BardanaInSameAccount = "Yes" Else BardanaInSameAccount = "No"
                SellOutCharges = clsFun.ExecScalarStr("Select SellOutCharges From Options")
                If SellOutCharges = "Y" Then SellOutCharges = "Yes" Else SellOutCharges = "No"
                CrateRate = clsFun.ExecScalarStr("Select CrateRate From Options")
                If CrateRate = "Y" Then CrateRate = "Yes" Else CrateRate = "No"
            Else
                Octroi = "No" : ISDiff = "No" : BardanaInSameAccount = "No" : SellOutCharges = "No" : CrateRate = "No"
            End If
            sql = " Insert Into Controls (Decimals,AskReciept,AskPayment,AskMannual,AskAuto,sendDiff,CrateBardana,ChargeEffect,TareSameAc,DefaultDate,PurcahseRO,SaleRO,ROEachItem, " & _
                  "AskCreditLimit,ApplyCommWeight,Octroi,SpeedCommission,SpeedMandiTax,SpeedRDF,SpeedTare,SpeedLabour,SpeedTaxPaid,SpeedKaat,SuperCommission,SuperMandiTax,SuperRDF,SuperTare," & _
                  "SuperLabour,SuperTaxPaid,SuperKaat,STDCommission,STDMandiTax,STDRDF,STDTare,STDLabour,StdTaxPaid,StdKaat,StockCommission,StockMandiTax,StockRDF,StockTare,StockLabour,StockTaxPaid,StockKaat) values" & _
                  "('0.00','No','No','Yes','Yes', '" & ISDiff & "','" & CrateRate & "','" & SellOutCharges & "','" & BardanaInSameAccount & "', " & _
                  "'Default','No','No','No','Allow','No','" & Octroi & "','None','None','None','None','None','No','No','None','None','None', " & _
                 " 'None','None','No','No','None','None','None','None','None','No','No','None','None','None','None','None', 'No','No')"
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    clsFun.ExecScalarStr("Drop table If exists Options;Drop table If exists Option1;Drop table If exists Option2; " & _
                                         "Drop table If exists Option3;Drop table If exists Option4;Drop table If Exists Option5;")
                    clsFun.ExecScalarStr("vacuum;")
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
        sql = String.Empty
        sql = "CREATE TABLE if not Exists Stock (ID INTEGER, ENTRYDATE TEXT NOT NULL, TRANSTYPE TEXT NOT NULL, PURCHASETYPENAME TEXT NOT NULL, PurchaseID INTEGER NOT NULL," & _
                    "SELLERID INTEGER NOT NULL, SELLERNAME TEXT NOT NULL, STORAGEID INTEGER NOT NULL, StorageName TEXT NOT NULL, ITEMID INTEGER NOT NULL, ITEMNAME TEXT NOT NULL, " & _
                    "CUT TEXT, LOT TEXT, NUG INTEGER, WEIGHT REAL NOT NULL, PER REAL)"
        clsFun.ExecNonQuery(sql)
        If clsFun.CheckIfColumnExists("Stock", "TransID") = False Then
            sql = "ALTER TABLE Stock ADD TransID Integer;"
            clsFun.ExecNonQuery(sql)
        End If
        If clsFun.CheckIfColumnExists("Stock", "GrossWeight") = False Then
            sql = "ALTER TABLE Stock ADD COLUMN GrossWeight DECIMAL;"
            clsFun.ExecNonQuery(sql)
        End If
    End Sub

    Private Sub dg1_SelectionChanged(sender As Object, e As EventArgs) Handles dg1.SelectionChanged, dg1.GotFocus
        If dg1.RowCount = 0 Or dg1.SelectedRows.Count = 0 Then Exit Sub
        txtPath.Text = dg1.SelectedRows(0).Cells(7).Value
        GlobalData.ConnectionPath = txtPath.Text
        GlobalData.PrvPath = dg1.SelectedRows(0).Cells(8).Value

    End Sub

    Private Sub BtnRetrive_Click(sender As Object, e As EventArgs) Handles BtnRetrive.Click
        txtMainPath.Text = ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path")
        rowColums() : getCompanies()
    End Sub

    Private Sub CompanyList_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnPath.Click
        txtMainPath.Text = ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path")
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            txtMainPath.Text = FolderBrowserDialog1.SelectedPath
            ClsFunPrimary.ExecScalarStr("Update Path Set DefaultPath='" & txtMainPath.Text & "'")
            getCompanies() : dg1.Focus()
        End If
    End Sub

    Private Sub txtMainPath_TextChanged(sender As Object, e As EventArgs) Handles txtMainPath.TextChanged

    End Sub
End Class


'    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
'        If dg1.RowCount = 0 Then Exit Sub
'    End Sub

'    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
'        clsFun.changeCompany()
'        If clsFun.CheckIfColumnExists("Company", "OrganizationID") = False Then
'            updateCompanytbl()
'        End If
'        Data = dg1.SelectedRows(0).Cells(7).Value
'        sCompData = dg1.SelectedRows(0).Cells(7).Value
'        YEARSTART = dg1.SelectedRows(0).Cells(4).Value
'        YEAREND = dg1.SelectedRows(0).Cells(5).Value
'        Dim tmpsql As String = String.Empty
'        Dim tmpSql1 As String = String.Empty
'        tmpSql1 = "INSERT INTO Company (ID,CompanyName,PrintOtherName,Address,PrintOtheraddress,City,PrintOtherCity,State,PrintOtherState,MobileNo1,MobileNo2,PhoneNo,FaxNo,EmailID,Website,GSTN,DealsIN,RegistrationNo,PanNo,Marka,Other,Logo,YearStart,Yearend,CompData,tag,OrganizationID,Password,AutoSync) "
'        If Data <> "" Then
'            isCompanyOpen = True
'            tmpsql = tempSqlQuery(dg1.SelectedRows(0).Cells(0).Value)
'            tmpSql1 = tmpSql1 & tmpsql
'            changeCompany("Data\" & Data)
'            If clsFun.CheckIfColumnExists("Company", "OrganizationID") = False Then
'                updateCompanytbl()
'            End If
'            If clsFun.CheckIfColumnExists("Transaction2", "CrateAccountID") = False Then
'                updateTransaction2()
'            End If
'            clsFun.ExecNonQuery("delete from Company")
'        End If
'        clsFun.ExecNonQuery("delete from Company")
'        If clsFun.ExecScalarInt("Select count(*) from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "") = 0 And tmpsql <> "" Then
'            clsFun.ExecNonQuery("delete from Company")
'            clsFun.ExecNonQuery(tmpSql1)
'        End If
'        CompanyInfo()
'        Login.MdiParent = ShowCompanies
'        Login.Show()
'        If Not Login Is Nothing Then
'            Login.BringToFront()
'        End If
'    End Sub
'    Private Sub updateCompanytbl()
'        Dim sql As String = "PRAGMA foreign_keys = 0; CREATE TABLE sqlitestudio_temp_table  AS  Select  *  FROM Company; DROP TABLE Company;CREATE TABLE Company ( ID                NUMERIC,    CompanyName       TEXT,    PrintOtherName    TEXT,    Address           TEXT,    PrintOtheraddress TEXT,    City              TEXT,    PrintOtherCity    TEXT,    State             TEXT,    PrintOtherState   TEXT,    MobileNo1         TEXT,    MobileNo2         TEXT,    PhoneNo           TEXT,    FaxNo             TEXT,    EmailID           TEXT,    Website           TEXT,    GSTN              TEXT,    DealsIN           TEXT,    RegistrationNo    TEXT,    PanNo             TEXT,    Marka             TEXT,    Other             TEXT,    Logo              BLOB,    YearStart         DATE,    Yearend           DATE,    CompData          TEXT,    tag               TEXT,    OrganizationID    INTEGER,    Password          TEXT,    Autosync          TEXT);" & _
'                             "INSERT INTO Company ( ID, CompanyName, PrintOtherName, Address, PrintOtheraddress,                      City, PrintOtherCity,  State,  PrintOtherState,   MobileNo1,  MobileNo2,  PhoneNo, FaxNo,                     EmailID,  Website,   GSTN, DealsIN, RegistrationNo, PanNo, Marka, Other,  Logo, YearStart,                        Yearend, CompData, tag )  Select ID,  CompanyName,  PrintOtherName,   Address,                     PrintOtheraddress,   City, PrintOtherCity,  State, PrintOtherState, MobileNo1, MobileNo2,                    PhoneNo, FaxNo,  EmailID, Website,  GSTN,  DealsIN,RegistrationNo,PanNo,  Marka,                           Other,  Logo, YearStart,  Yearend,  CompData, tag  FROM sqlitestudio_temp_table;DROP TABLE sqlitestudio_temp_table;PRAGMA foreign_keys = 1;"
'        clsFun.ExecNonQuery(sql)
'    End Sub
'    Private Sub updateTransaction2()
'        Dim sql As String = "PRAGMA foreign_keys = 0;CREATE TABLE sqlitestudio_temp_table AS Select *   FROM Transaction2;DROP TABLE Transaction2; " & _
'"CREATE TABLE Transaction2 ( ID INTEGER PRIMARY KEY AUTOINCREMENT, EntryDate DATETIME DEFAULT ('strftime(''%d-%m-%Y'')'), VoucherID INTEGER, TransType TEXT, BillNo   TEXT, SallerID  INTEGER, SallerName  TEXT, AccountID INTEGER, AccountName  TEXT, OtherAccountName TEXT, ItemID   INTEGER, ItemName  TEXT, OtherItemName TEXT, Cut   DECIMAL, Lot   TEXT, Nug   DECIMAL, Weight   DECIMAL, Rate  DECIMAL, SRate  DECIMAL, Per   TEXT, Amount   DECIMAL, Charges  DECIMAL, TotalAmount  DECIMAL, SallerAmt DECIMAL, CommPer  DECIMAL, CommAmt  DECIMAL, MPer  DECIMAL, MAmt  DECIMAL, RdfPer   DECIMAL, RdfAmt   DECIMAL, Tare  DECIMAL, TareAmt  DECIMAL, labour   DECIMAL, LabourAmt DECIMAL, MaintainCrate TEXT, CrateID  DECIMAL, Cratemarka  TEXT, CrateQty  DECIMAL, PurchaseTypename TEXT, PurchaseID  INTEGER, RoundOff  DECIMAL, CrateAccountID  INTEGER, CrateAccountName TEXT);" & _
'"INSERT INTO Transaction2 ( ID, EntryDate, VoucherID, TransType, BillNo, SallerID, SallerName, AccountID, AccountName, OtherAccountName, ItemID, ItemName, OtherItemName, Cut, Lot, Nug, Weight, Rate, SRate, Per, Amount, Charges, TotalAmount, SallerAmt, CommPer, CommAmt, MPer, MAmt, RdfPer, RdfAmt, Tare, TareAmt, labour, LabourAmt, MaintainCrate, CrateID, Cratemarka, CrateQty, PurchaseTypename, PurchaseID) Select ID, EntryDate, VoucherID, TransType, BillNo, SallerID, SallerName, AccountID, AccountName, OtherAccountName, ItemID, ItemName, OtherItemName, Cut, Lot, Nug, Weight, Rate, SRate, Per, Amount, Charges, TotalAmount, SallerAmt, CommPer, CommAmt, MPer, MAmt, RdfPer, RdfAmt, Tare, TareAmt, labour, LabourAmt, MaintainCrate, CrateID, Cratemarka, CrateQty, PurchaseTypename, PurchaseID  FROM sqlitestudio_temp_table;" & _
'"DROP TABLE sqlitestudio_temp_table;DROP VIEW Stock_Sale_Report;" & _
'"CREATE VIEW Stock_Sale_Report AS Select v.VehicleNo AS VehicleNo,   V.id AS VoucherID,   V.entryDate,   v.billNo AS BillNo,   v.SallerID AS SallerID,   v.sallerName AS SallerName,   t.ItemName AS ItemName,   t.Lot AS lot,   t.accountName AS AccountName,   t.nug AS nug,   t.Weight AS weight,   t.rate AS rate,   t.per AS per,   t.Amount AS amount,   t.Charges AS charges,   t.TotalAmount AS totalAmount,   v.TransType AS transtype  FROM Vouchers v   INNER JOIN   Transaction2 t ON v.id = t.VoucherID;PRAGMA foreign_keys = 1; "
'        clsFun.ExecNonQuery(sql)
'    End Sub

'    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
'        If e.KeyCode = Keys.F10 Then
'            Dim a As String = MsgBox("Are You Sure To Take Backup ?", vbInformation + MsgBoxStyle.YesNo, "BACKUP")
'            If a = vbYes Then
'                If Directory.Exists(Application.StartupPath & "\Backup") = False Then
'                    Directory.CreateDirectory(Application.StartupPath & "\Backup")
'                End If
'                Dim compfolder As String = dg1.SelectedRows(0).Cells(7).Value ''clsFun.ExecScalarStr("Select CompData from Company")
'                Dim compfolder1 As String = compfolder
'                compfolder = compfolder.Substring(0, compfolder.LastIndexOf("\"))
'                If Directory.Exists(Application.StartupPath & "\Backup\" & compfolder) = False Then
'                    Directory.CreateDirectory(Application.StartupPath & "\Backup\" & compfolder)
'                End If
'                Dim FileName As String = "Data-" & clsFun.GetServerDate().Replace("-", "") & ".db"
'                File.Copy(Application.StartupPath & "\Data\" & compfolder1, Application.StartupPath & "\Backup\" & compfolder & "\" & FileName, True)
'                MessageBox.Show("Backup Successfully Done")
'                ''  clsFun.CopyDirectory(Application.StartupPath & "\SMS.db", Application.StartupPath & "\BackupFolder\" & FileName)
'            End If

'        End If

'        clsFun.changeCompany()
'        If e.KeyCode = Keys.F3 Then
'            Create_Company.MdiParent = ShowCompanies
'            Create_Company.Show()
'            If Not Create_Company Is Nothing Then
'                Create_Company.BringToFront()
'            End If
'        End If
'        If e.KeyCode = Keys.Enter Then
'            If dg1.RowCount = 0 Then Exit Sub
'            If clsFun.CheckIfColumnExists("Company", "OrganizationID") = False Then
'                updateCompanytbl()
'            End If
'            Dim data As String = String.Empty
'            data = dg1.SelectedRows(0).Cells(7).Value
'            sCompData = dg1.SelectedRows(0).Cells(7).Value
'            Dim tmpsql As String = String.Empty
'            Dim tmpSql1 As String = String.Empty
'            tmpSql1 = "INSERT INTO Company (ID,CompanyName,PrintOtherName,Address,PrintOtheraddress,City,PrintOtherCity,State,PrintOtherState,MobileNo1,MobileNo2,PhoneNo,FaxNo,EmailID,Website,GSTN,DealsIN,RegistrationNo,PanNo,Marka,Other,Logo,YearStart,Yearend,CompData,tag,OrganizationID,Password,AutoSync) "
'            If data <> "" Then
'                isCompanyOpen = True
'                tmpsql = tempSqlQuery(dg1.SelectedRows(0).Cells(0).Value)
'                tmpSql1 = tmpSql1 & tmpsql
'                changeCompany("Data\" & data)
'                If clsFun.CheckIfColumnExists("Company", "OrganizationID") = False Then
'                    updateCompanytbl()
'                End If
'                If clsFun.CheckIfColumnExists("Transaction2", "CrateAccountID") = False Then
'                    updateTransaction2()
'                End If
'                clsFun.ExecNonQuery("delete from Company")
'            End If
'            clsFun.ExecNonQuery("delete from Company")
'            If clsFun.ExecScalarInt("Select count(*) from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "") = 0 And tmpsql <> "" Then
'                clsFun.ExecNonQuery("delete from Company")
'                clsFun.ExecNonQuery(tmpSql1)
'            End If
'            CompanyInfo()
'            Login.MdiParent = ShowCompanies
'            Login.Show()
'            If Not Login Is Nothing Then
'                Login.BringToFront()
'                e.SuppressKeyPress = True
'            End If
'        End If
'        If e.KeyCode = Keys.Delete Then
'            Delete()
'        End If
'    End Sub
'    Private Sub Delete()
'        Try
'            If MessageBox.Show(" Are You Sure want to delete comapny it can't be Reverse ??", "Delete Company At Own Risk", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
'                If clsFun.ExecNonQuery("DELETE from Company WHERE ID=" & dg1.SelectedRows(0).Cells(0).Value & "") > 0 Then
'                    MsgBox("Company Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
'                    retrive()
'                End If
'            End If
'        Catch ex As Exception
'            MsgBox(ex.Message)
'        End Try
'    End Sub
'    Public Sub CompanyInfo()
'        sCompCode = dg1.SelectedRows(0).Cells(0).Value
'        compname = dg1.SelectedRows(0).Cells(1).Value
'        ' compnameHindi = dg1.SelectedRows(0).Cells(2).Value
'        compnameHindi = clsFun.ExecScalarStr("Select PrintOtherName from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Mob1 = clsFun.ExecScalarStr("Select MobileNo1 from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Mob2 = clsFun.ExecScalarStr("Select MobileNo2 from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Address = clsFun.ExecScalarStr("Select Address from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        AddressHindi = clsFun.ExecScalarStr("Select PrintOtheraddress from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        City = clsFun.ExecScalarStr("Select City from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        CityHindi = clsFun.ExecScalarStr("Select PrintOtherCity from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        State = clsFun.ExecScalarStr("Select State from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        StateHindi = clsFun.ExecScalarStr("Select PrintOtherState from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Fax = clsFun.ExecScalarStr("Select FaxNo from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Email = clsFun.ExecScalarStr("Select EmailID from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Phone = clsFun.ExecScalarStr("Select PhoneNo from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Website = clsFun.ExecScalarStr("Select Website from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Gstn = clsFun.ExecScalarStr("Select Gstn from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        DealsIN = clsFun.ExecScalarStr("Select DealsIN from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Registration = clsFun.ExecScalarStr("Select RegistrationNo from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Pan = clsFun.ExecScalarStr("Select PanNo from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        Marka = clsFun.ExecScalarStr("Select Marka from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'        other = clsFun.ExecScalarStr("Select Other from Company where id=" & dg1.SelectedRows(0).Cells(0).Value & "")
'    End Sub
'    Private Sub CompanyList_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
'        If e.KeyCode = Keys.F3 Then
'            Create_Company.MdiParent = ShowCompanies
'            Create_Company.Show()
'            If Not Create_Company Is Nothing Then
'                Create_Company.BringToFront()
'            End If
'        End If
'        If e.KeyCode = Keys.Enter Then
'            Login.MdiParent = ShowCompanies
'            Login.Show()
'            If Not Login Is Nothing Then
'                Login.BringToFront()
'            End If
'        End If
'    End Sub
'    Private Sub CompanyList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        rowColums() : retrive()
'        Dim sql As String
'        ' sql = clsFun.ExecScalarStr("Select CompData from Company Where ID = " & dg1.SelectedRows(0).Cells(0).Value & "")
'        'txtPath.Text = sql
'    End Sub
'    Public Sub rowColums()
'        dg1.ColumnCount = 8
'        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
'        dg1.Columns(1).Name = "Company Name" : dg1.Columns(1).Width = 300
'        dg1.Columns(2).Name = "Address" : dg1.Columns(2).Width = 200
'        dg1.Columns(3).Name = "City" : dg1.Columns(3).Width = 170
'        dg1.Columns(4).Name = "Finacial Year Start" : dg1.Columns(4).Width = 100
'        dg1.Columns(5).Name = "Finacial Year End" : dg1.Columns(5).Width = 100
'        dg1.Columns(6).Name = "FYID" : dg1.Columns(6).Width = False
'        dg1.Columns(7).Name = "CompData" : dg1.Columns(7).Width = False
'        retrive()
'    End Sub
'    Public Sub retrive()
'        Dim dt As New DataTable
'        dg1.Rows.Clear()
'        dt = clsFun.ExecDataTable("Select * from Company")
'        Try
'            If dt.Rows.Count > 0 Then
'                For i = 0 To dt.Rows.Count - 1
'                    dg1.Rows.Add()
'                    With dg1.Rows(i)
'                        .Cells(0).Value = dt.Rows(i)("id").ToString()
'                        .Cells(1).Value = dt.Rows(i)("CompanyName").ToString()
'                        .Cells(2).Value = dt.Rows(i)("Address").ToString()
'                        .Cells(3).Value = dt.Rows(i)("City").ToString()
'                        .Cells(6).Value = dt.Rows(i)("id").ToString()
'                        .Cells(4).Value = CDate(dt.Rows(i)("YearStart")).ToString("dd-MM-yyyy")
'                        .Cells(5).Value = CDate(dt.Rows(i)("YearEnd")).ToString("dd-MM-yyyy")
'                        '.Cells(4).Value = Format(dt.Rows(i)("YearStart").ToString()
'                        ' .Cells(5).Value = Format(dt.Rows(i)("Yearend"), "dd-MM-yyyy")
'                        .Cells(7).Value = dt.Rows(i)("CompData").ToString()
'                    End With
'                Next
'            End If
'            dt.Dispose()

'        Catch ex As Exception
'            MsgBox(ex.Message, vbOKOnly + vbInformation, "Finacle")
'        End Try
'    End Sub

'    Private Sub BtnRetrive_Click(sender As Object, e As EventArgs) Handles BtnRetrive.Click
'        retrive()
'    End Sub
'    Sub changeCompany()
'        Try
'            Dim Con As New StringBuilder("")
'            Con.Append("Data Source=|DataDirectory|\")
'            Con.Append("data.db;Version=3;New=True;Compress=True;synchronous=ON;")
'            Dim strCon As String = Con.ToString()
'            updateConfigFile(strCon)
'            Dim Db As New SQLite.SQLiteConnection()
'            ConfigurationManager.RefreshSection("connectionStrings")
'            Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
'            clsFun.ConStr = Db.ConnectionString
'        Catch E As Exception
'            MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
'        End Try
'    End Sub
'    Private Sub btnClose_Click(sender As Object, e As EventArgs)
'        changeCompany()
'        Application.Exit()


'    End Sub
'    Sub changeCompany(ByVal Data As String)
'        Try
'            Dim Con As New StringBuilder("")
'            Con.Append("Data Source=|DataDirectory|\")
'            Con.Append(Data & ";Version=3;New=True;Compress=True;synchronous=ON;")
'            'Con.Append(Data & ";Version=3;Password=smi3933;")
'            Dim strCon As String = Con.ToString()
'            updateConfigFile(strCon)
'            Dim Db As New SQLite.SQLiteConnection()
'            ConfigurationManager.RefreshSection("connectionStrings")
'            Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
'            clsFun.ConStr = Db.ConnectionString
'        Catch E As Exception
'            MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
'        End Try
'    End Sub
'    Public Sub updateConfigFile(con As String)
'        Dim XmlDoc As New XmlDocument()
'        XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
'        For Each xElement As XmlElement In XmlDoc.DocumentElement
'            If xElement.Name = "connectionStrings" Then
'                xElement.FirstChild.Attributes(1).Value = con
'            End If
'        Next
'        XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
'    End Sub

'    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
'        SetPathForm.Show()
'        If Not SetPathForm Is Nothing Then
'            SetPathForm.BringToFront()
'        End If
'    End Sub
'    Function tempSqlQuery(ByVal compId As Integer) As String
'        tempSqlQuery = String.Empty
'        Dim dt As New DataTable
'        Dim i As Integer = 0
'        dt = clsFun.ExecDataTable("Select * from Company where id=" & compId & "")
'        If dt.Rows.Count > 0 Then
'            tempSqlQuery = "Select"
'            For i = 0 To dt.Columns.Count - 1
'                If i = dt.Columns.Count - 1 Then

'                    If dt.Columns(i).DataType.Name.ToString() = "String" Then
'                        tempSqlQuery = tempSqlQuery & " '" & dt.Rows(0)(i).ToString() & "'"
'                    ElseIf dt.Columns(i).DataType.Name.ToString() = "DateTime" Then
'                        tempSqlQuery = tempSqlQuery & " '" & CDate(dt.Rows(0)(i).ToString()).ToString("yyyy-MM-dd") & "'"
'                    Else
'                        tempSqlQuery = tempSqlQuery & " " & Val(dt.Rows(0)(i).ToString()) & ""
'                    End If
'                Else
'                    If dt.Columns(i).DataType.Name.ToString() = "String" Then
'                        tempSqlQuery = tempSqlQuery & " '" & dt.Rows(0)(i).ToString() & "',"
'                    ElseIf dt.Columns(i).DataType.Name.ToString() = "DateTime" Then
'                        tempSqlQuery = tempSqlQuery & " '" & CDate(dt.Rows(0)(i).ToString()).ToString("yyyy-MM-dd") & "',"
'                    Else
'                        tempSqlQuery = tempSqlQuery & " " & Val(dt.Rows(0)(i).ToString()) & ","
'                    End If
'                End If
'            Next
'        End If
'        dt.Dispose()

'        Return tempSqlQuery
'    End Function

'    Private Sub CompanyList_Resize(sender As Object, e As EventArgs) Handles Me.Resize
'        'rs.ResizeAllControls(Me)
'    End Sub

'End Class