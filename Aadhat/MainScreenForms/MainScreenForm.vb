Imports System.IO
Imports System.Management
Imports System.Reflection
'Imports ICSharpCode.SharpZipLib.Zip
Imports Ionic.Zip
Public Class MainScreenForm
    Public Sub New()
        Me.SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Dim rs As New Resizer
    Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
    Private blinkState As Boolean = False
    Private Sub NewAccountToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewAccountToolStripMenuItem.Click
        CreateAccount.MdiParent = Me
        CreateAccount.Show()
        If Not CreateAccount Is Nothing Then
            CreateAccount.BringToFront()
        End If
    End Sub

    Private Sub btnAccount_Click(sender As Object, e As EventArgs) Handles btnAccount.Click
        CreateAccount.MdiParent = Me
        CreateAccount.Show()
        CreateAccount.BringToFront()
        CreateAccount.txtName.Focus()
    End Sub

    Private Sub NewCaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewCaseToolStripMenuItem.Click
        SpeedSale.MdiParent = Me
        SpeedSale.Show()
        If Not SpeedSale Is Nothing Then
            SpeedSale.BringToFront()
        End If
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        OfflineBackup()
        'If Directory.Exists(Application.StartupPath & "\Backup") = False Then
        '    Directory.CreateDirectory(Application.StartupPath & "\Backup")
        'End If
        'Dim compfolder As String = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last() & "\Data.db"
        'If compfolder = "Data\Data.db" Then compfolder = "Data.db"
        'Dim compfolder1 As String = compfolder
        'If compfolder <> "Data.db" Then compfolder = compfolder.Substring(0, compfolder.LastIndexOf("\"))
        'If Directory.Exists(Application.StartupPath & "\Backup\" & compfolder) = False Then
        '    Directory.CreateDirectory(Application.StartupPath & "\Backup\" & compfolder)
        'End If
        'Dim FileName As String = "Data-" & clsFun.GetServerDate().Replace("-", "") & ".db"
        ''clsFun.CloseConnection()
        'File.Copy(Application.StartupPath & "\Data\" & compfolder1, Application.StartupPath & "\Backup\" & compfolder & "\" & FileName, True)
        Me.Dispose() : ShowCompanies.Show()
    End Sub
    Private Sub btnItem_Click(sender As Object, e As EventArgs) Handles btnItem.Click
        Item_form.MdiParent = Me
        Item_form.Show()
        Item_form.BringToFront()
        Item_form.txtItemName.Focus()
    End Sub

    Private Sub GroupMasterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupMasterToolStripMenuItem.Click
        Item_form.MdiParent = Me
        Item_form.Show()
        Item_form.BringToFront()
        Item_form.txtItemName.Focus()
    End Sub

    Private Sub AccountListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AccountListToolStripMenuItem.Click
        Account_List.MdiParent = Me
        Account_List.Show()
        If Not Account_List Is Nothing Then
            Account_List.BringToFront()
        End If
    End Sub

    Private Sub AddGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddGroupToolStripMenuItem.Click
        Account_Group.MdiParent = Me
        Account_Group.Show()
        If Not Account_Group Is Nothing Then
            Account_Group.BringToFront()
        End If
    End Sub

    Private Sub ListGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListGroupToolStripMenuItem.Click
        account_group_list.MdiParent = Me
        account_group_list.Show()
        If Not account_group_list Is Nothing Then
            account_group_list.BringToFront()
        End If
    End Sub

    Private Sub TeamViewerQSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TeamViewerQSToolStripMenuItem.Click
        Dim p As New System.Diagnostics.Process
        p.StartInfo.FileName = Application.StartupPath & "\TeamViewerQS.exe"
        p.Start()
    End Sub

    Private Sub AnyDeskToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AnyDeskToolStripMenuItem.Click
        Dim p As New System.Diagnostics.Process
        Dim filename As String = Application.StartupPath & "\anydesk.exe"
        If Not File.Exists(filename) Then Exit Sub
        If p.StartInfo.FileName = Application.StartupPath & "\anydesk.exe" = True Then
            p.Start()
        End If

    End Sub

    Private Sub btnReceipt_Click(sender As Object, e As EventArgs) Handles btnSpeedSale.Click
        SpeedSale.MdiParent = Me
        SpeedSale.Show()
        SpeedSale.BringToFront()
        SpeedSale.mskEntryDate.Focus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles BtnReceipt.Click
        ReceiptForm.MdiParent = Me
        ReceiptForm.Show()
        ReceiptForm.BringToFront()
        ReceiptForm.mskEntryDate.Focus()
        ReceiptForm.Top = 0 : ReceiptForm.Left = 0
    End Sub

    Private Sub btnLedger_Click(sender As Object, e As EventArgs) Handles btnSpeedRegister.Click
        Speed_Sale_Register.MdiParent = Me
        Speed_Sale_Register.Show()
        Speed_Sale_Register.BringToFront()
        Speed_Sale_Register.mskFromDate.Focus()
    End Sub
    Private Sub NewCaseRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Ledger.MdiParent = Me
        Ledger.Show()
        Ledger.BringToFront()
        Ledger.cbAccountName.Focus()
    End Sub
    Private Sub ReceiptVoucherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReceiptVoucherToolStripMenuItem.Click
        ReceiptForm.MdiParent = Me
        ReceiptForm.Show()
        ReceiptForm.BringToFront()
        ReceiptForm.mskEntryDate.Focus()
        ReceiptForm.Top = 0 : ReceiptForm.Left = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnSelloutMannual.Click
        Sellout_Mannual.MdiParent = Me
        Sellout_Mannual.Show()
        Sellout_Mannual.BringToFront()
        Sellout_Mannual.mskEntryDate.Focus()
        Sellout_Mannual.Top = 0 : Sellout_Mannual.Left = 0
    End Sub
    Public Sub OfflineBackup()
        If Directory.Exists(Application.StartupPath & "\Backup") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Backup")
        End If
        Dim compfolder As String = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last() & "\Data.db"
        If compfolder = "Data\Data.db" Then compfolder = "Data.db"
        Dim compfolder1 As String = compfolder
        If compfolder <> "Data.db" Then compfolder = compfolder.Substring(0, compfolder.LastIndexOf("\"))
        If Directory.Exists(Application.StartupPath & "\Backup\" & compfolder) = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Backup\" & compfolder)
        End If

        Dim FileName As String = "Data-" & clsFun.GetServerDate().Replace("-", "") & ".db"
        'clsFun.CloseConnection()
        Dim defaultpath As String = ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path").ToUpper()
        If defaultpath = ("Data").ToUpper Then
            File.Copy(Application.StartupPath & "\Data\" & compfolder1, Application.StartupPath & "\Backup\" & compfolder & "\" & FileName, True)
        Else
            File.Copy(defaultpath & "\Data\" & compfolder1, Application.StartupPath & "\Backup\" & compfolder & "\" & FileName, True)
        End If
        If Directory.Exists(Application.StartupPath & "\BackupGoogle") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\BackupGoogle")
        End If
        If Directory.Exists(Application.StartupPath & "\BackupGoogle\" & compfolder) = False Then
            Directory.CreateDirectory(Application.StartupPath & "\BackupGoogle\" & compfolder)
        End If
        FileName = "Data.db"
        If defaultpath = ("Data").ToUpper Then
            File.Copy(Application.StartupPath & "\Data\" & compfolder1, Application.StartupPath & "\BackupGoogle\" & compfolder & "\" & FileName, True)
        Else
            File.Copy(defaultpath & "\Data\" & compfolder1, Application.StartupPath & "\BackupGoogle\" & compfolder & "\" & FileName, True)
        End If
    End Sub

    Private Sub MainScreenForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If ClsCommon.IsInternetConnect() Then
            Me.Hide()
            Backup_On_Server.Show()
            Backup_On_Server.BringToFront()
            BackUp() : Exit Sub
        Else
            BackUp() : Backup_On_Server.ZipBackup() : Application.Exit()
        End If
    End Sub
    Public Sub BackUp()
        If Directory.Exists(Application.StartupPath & "\BackupGoogle") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\BackupGoogle")
        End If
        Dim sourceDirectory As String = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last() & "\Data.db"
        sourceDirectory = Application.StartupPath & "\Data\" & sourceDirectory.Replace("\Data.db", "")
        Dim zipFilePath As String = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last() & ""
        zipFilePath = Application.StartupPath & "\BackUp\" & zipFilePath & "(" & clsFun.GetServerDate().Replace("-", "") & ").zip"
        Try
            Using zipFile As ZipFile = New ZipFile()
                zipFile.AddDirectory(sourceDirectory) ' Add the entire folder and its contents.
                zipFile.Save(zipFilePath)
            End Using
        Catch ex As Exception
            Console.WriteLine("Error creating zip folder: " & ex.Message)
        End Try
    End Sub

    Private Sub blinkTimer_Tick(sender As Object, e As EventArgs) Handles blinkTimer.Tick
        lblARC.Visible = blinkState
        blinkState = Not blinkState
    End Sub

    Private Sub MainScreenForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim fecha As Date = IO.File.GetCreationTime(Assembly.GetExecutingAssembly().Location)
        'rs.FindAllControls(Me)
        MainScreenPicture.MdiParent = Me
        MainScreenPicture.Show()
        Dim q As New SelectQuery("Win32_bios")
        Dim search As New ManagementObjectSearcher(q)
        Dim info As New ManagementObject
        For Each info In search.Get
            Dim ssql As String
            ssql = clsFun.ExecScalarStr("Select CompanyName from Company")
            ssql = ssql.Replace("_", "&")
            FinYearStart = clsFun.ExecScalarStr("Select YearStart from Company")
            FinYearEnd = clsFun.ExecScalarStr("Select Yearend from Company")
            Dim AccName As String = ssql & " "
            Me.Text = "[Aadhat 26.0.0 #" & CDate(fecha).ToString("yyMMddhhmm") & "] [#" & AccName & "]" & " [" & CDate(FinYearStart).ToString("dd-MM-yy") & " To " & CDate(FinYearEnd).ToString("dd-MM-yy") & "]"
        Next
        If Not File.Exists(fileName) Then RegistrationToolStripMenuItem.Visible = True
        Dim version As Version = Assembly.GetExecutingAssembly().GetName().Version
        lblBuildVersion.Text = version.ToString()
    End Sub

    Private Sub BtnPayment_Click(sender As Object, e As EventArgs) Handles BtnPayment.Click
        PayMentform.MdiParent = Me
        PayMentform.Show()
        PayMentform.BringToFront()
        PayMentform.mskEntryDate.Focus()
        PayMentform.Top = 0 : PayMentform.Left = 0
    End Sub

    Private Sub BtnRcptRegister_Click(sender As Object, e As EventArgs)
        RcptRegister.MdiParent = Me
        RcptRegister.Show()
        RcptRegister.BringToFront()
        RcptRegister.mskFromDate.Focus()
    End Sub
    Private Sub BtnBankEntry_Click(sender As Object, e As EventArgs) Handles BtnBankEntry.Click
        Bank_Entry.MdiParent = Me
        Bank_Entry.Show()
        Bank_Entry.BringToFront()
        Bank_Entry.MskEntryDate.Focus()
    End Sub
    Private Sub BankEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BankEntryToolStripMenuItem.Click
        Bank_Entry.MdiParent = Me
        Bank_Entry.Show()
        Bank_Entry.BringToFront()
        Bank_Entry.MskEntryDate.Focus()
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnSuperSale.Click
        Super_Sale.MdiParent = Me
        Super_Sale.Show()
        Super_Sale.BringToFront()
    End Sub
    Private Sub ScripBeejakToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScripBeejakToolStripMenuItem.Click
        Sellout_Mannual.MdiParent = Me
        Sellout_Mannual.Show()
        Sellout_Mannual.BringToFront()
        Sellout_Mannual.Top = 0 : Sellout_Mannual.Left = 0
    End Sub
    Private Sub ChargesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChargesToolStripMenuItem.Click
        ChargesForm.MdiParent = Me
        ChargesForm.Show()
        ChargesForm.BringToFront()
        ChargesForm.TxtChargeName.Focus()
    End Sub
    Private Sub DayBookToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Day_book.MdiParent = Me
        Day_book.Show()
        Day_book.BringToFront()
        Day_book.mskFromDate.Focus()
    End Sub
    Private Sub btnCashBankBook_Click(sender As Object, e As EventArgs)
        Standard_Sale.MdiParent = Me
        Standard_Sale.Show()
        Standard_Sale.BringToFront()
        Standard_Sale.mskEntryDate.Focus()
    End Sub
    Private Sub CashBankBookToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub MainScreenForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'rs.ResizeAllControls(Me)
    End Sub
    Private Sub RegisterToolStripMenuItem_Click(sender As Object, e As EventArgs)
        ApplyLicenseKey.MdiParent = Me
        ApplyLicenseKey.Show()
        If Not ApplyLicenseKey Is Nothing Then
            ApplyLicenseKey.BringToFront()
        End If
    End Sub
    Private Sub JournalEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JournalEntryToolStripMenuItem.Click
        JournalEntry.MdiParent = Me
        JournalEntry.Show()
        JournalEntry.BringToFront()
        JournalEntry.mskEntryDate.Focus()
    End Sub
    Private Sub PaymentVoucherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaymentVoucherToolStripMenuItem.Click
        PayMentform.MdiParent = Me
        PayMentform.Show()
        PayMentform.BringToFront()
        PayMentform.mskEntryDate.Focus()
        PayMentform.Top = 0 : PayMentform.Left = 0
    End Sub
    Private Sub JournalEntryRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JournalEntryRegisterToolStripMenuItem.Click
        Journal_Register.MdiParent = Me
        Journal_Register.Show()
        Journal_Register.BringToFront()
        Journal_Register.mskFromDate.Focus()
    End Sub
    Private Sub PaymentRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaymentRegisterToolStripMenuItem.Click
        Payment_Register.MdiParent = Me
        Payment_Register.Show()
        Payment_Register.BringToFront()
        Payment_Register.mskFromDate.Focus()
    End Sub
    Private Sub btnPaymentRegister_Click(sender As Object, e As EventArgs)
        Payment_Register.MdiParent = Me
        Payment_Register.Show()
        Payment_Register.BringToFront()
        Payment_Register.mskFromDate.Focus()
    End Sub
    Private Sub ReceiptRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReceiptRegisterToolStripMenuItem.Click
        RcptRegister.MdiParent = Me
        RcptRegister.Show()
        RcptRegister.BringToFront()
        RcptRegister.mskFromDate.Focus()
    End Sub

    Private Sub BtnOuthstanding_Click(sender As Object, e As EventArgs) Handles BtnLedger.Click
        Ledger.MdiParent = Me
        Ledger.Show()
        Ledger.BringToFront()
        Ledger.cbAccountName.Focus()
        Ledger.Top = 0 : Ledger.Left = 0
    End Sub

    Private Sub BankEntryRegisterToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BankEntryRegisterToolStripMenuItem1.Click
        bank_Register.MdiParent = Me
        bank_Register.Show()
        bank_Register.BringToFront()
        bank_Register.mskFromDate.Focus()
    End Sub

    Private Sub SpeedSaleRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpeedSaleRegisterToolStripMenuItem.Click

    End Sub

    Private Sub BtnBillsPrint_Click(sender As Object, e As EventArgs) Handles BtnDayBook.Click
        Day_book.MdiParent = Me
        Day_book.Show()
        Day_book.BringToFront()
        Day_book.mskFromDate.Focus()
    End Sub

    Private Sub PrintBillsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintBillsToolStripMenuItem.Click
        Print_Bills.MdiParent = Me
        Print_Bills.Show()
        Print_Bills.Top = 0 : Print_Bills.Left = 0
        Print_Bills.BringToFront()
        Print_Bills.mskFromDate.Focus()

    End Sub
    Private Sub CrateInToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Crate_IN_Register.MdiParent = Me
        Crate_IN_Register.Show()
        Crate_IN_Register.BringToFront()
        Crate_IN_Register.mskFromDate.Focus()
    End Sub

    Private Sub CrateOutRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Crate_Out_Register.MdiParent = Me
        Crate_Out_Register.Show()
        Crate_Out_Register.BringToFront()
        Crate_Out_Register.mskFromDate.Focus()
    End Sub

    Private Sub CrateInToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CrateInToolStripMenuItem1.Click
        Crate_IN.MdiParent = Me
        Crate_IN.Show()
        Crate_IN.BringToFront()
        Crate_IN.mskEntryDate.Focus()
    End Sub

    Private Sub CrateOutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrateOutToolStripMenuItem.Click
        Crate_Out.MdiParent = Me
        Crate_Out.Show()
        Crate_Out.BringToFront()
        Crate_Out.mskEntryDate.Focus()
    End Sub


    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles btnSelloutAuto.Click
        Sellout_Auto.MdiParent = Me
        Sellout_Auto.Show()
        Sellout_Auto.BringToFront()
        Sellout_Auto.Top = 0 : Sellout_Auto.Left = 0
        Sellout_Auto.mskEntryDate.Focus()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles btnSuperRegister.Click
        Super_Sale_Register.MdiParent = Me
        Super_Sale_Register.Show()
        Super_Sale_Register.BringToFront()
        Super_Sale_Register.mskFromDate.Focus()
    End Sub
    Private Sub SuperSaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SuperSaleToolStripMenuItem.Click
        '   Super_Sale.ResumeLayout()
        Super_Sale.MdiParent = Me
        Super_Sale.Show()
        Super_Sale.BringToFront()
        Super_Sale.mskEntryDate.Focus()
        '  Super_Sale.SuspendLayout()
    End Sub
    Private Sub btnStandardSale_Click(sender As Object, e As EventArgs) Handles btnStockSaleRegister.Click
        Stock_Sale_Register.MdiParent = Me
        Stock_Sale_Register.Show()
        Stock_Sale_Register.BringToFront()
        Stock_Sale_Register.mskFromDate.Focus()
    End Sub
    Private Sub StandardSaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StandardSaleToolStripMenuItem.Click
        Standard_Sale.MdiParent = Me
        Standard_Sale.Show()
        Standard_Sale.BringToFront()
        Standard_Sale.mskEntryDate.Focus()
        Standard_Sale.Top = 0 : Standard_Sale.Left = 0
    End Sub
    Private Sub CalculatorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalculatorToolStripMenuItem.Click
        Dim p As New System.Diagnostics.Process
        p.StartInfo.FileName = "calc.exe"
        p.Start()
    End Sub
    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles btnOutStanding.Click
        OutStanding_Amount_Only.MdiParent = Me
        OutStanding_Amount_Only.Show()
        OutStanding_Amount_Only.BringToFront()
        OutStanding_Amount_Only.mskEntryDate.Focus()
        OutStanding_Amount_Only.Left = 0 : OutStanding_Amount_Only.Top = 0
    End Sub
    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles BtnBillPrints.Click
        Print_Bills.MdiParent = Me
        Print_Bills.Show()
        Print_Bills.Activate()
        Print_Bills.BringToFront()
        Print_Bills.mskFromDate.Focus()
        Print_Bills.Top = 0 : Print_Bills.Left = 0
    End Sub
    Private Sub btnLedger_Click_1(sender As Object, e As EventArgs) Handles btnCashBankBook.Click
        Cash_Bank_Book.MdiParent = Me
        Cash_Bank_Book.Show()
        Cash_Bank_Book.BringToFront()
        Cash_Bank_Book.cbAccountName.Focus()
    End Sub
    Private Sub btnDayBook_Click(sender As Object, e As EventArgs)
        Standard_Sale_Register.MdiParent = Me
        Standard_Sale_Register.Show()
        Standard_Sale_Register.BringToFront()
        Standard_Sale_Register.mskFromDate.Focus()
    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles btnPurchase.Click
        Purchase.MdiParent = Me
        Purchase.Show()
        Purchase.BringToFront()
        Purchase.mskEntryDate.Focus()
    End Sub
    Private Sub PurchseStockInToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PurchseStockInToolStripMenuItem.Click
        Purchase.MdiParent = Me
        Purchase.Show()
        Purchase.BringToFront()
        Purchase.mskEntryDate.Focus()
    End Sub
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles btnPurchaseRegister.Click
        Purchase_Register.MdiParent = Me
        Purchase_Register.Show()
        Purchase_Register.BringToFront()
        Purchase_Register.mskFromDate.Focus()
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles btnStockSale.Click
        Stock_Sale.MdiParent = Me
        Stock_Sale.Show()
        Stock_Sale.BringToFront()
        Stock_Sale.mskEntryDate.Focus()
    End Sub
    Private Sub StockSaleRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub CreateStorageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateStorageToolStripMenuItem.Click
        Store.MdiParent = Me
        Store.Show()
        Store.BringToFront()
        Store.txtStoreName.Focus()
    End Sub
    Private Sub StorageRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StorageRegisterToolStripMenuItem.Click
        Storage_Register.MdiParent = Me
        Storage_Register.Show()
        Storage_Register.BringToFront()
    End Sub
    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        Options.MdiParent = Me
        Options.Show()
        Options.BringToFront()
    End Sub
    Private Sub SMSTempletesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SMSTempletesToolStripMenuItem.Click
        SMSAPI.MdiParent = Me
        SMSAPI.Show()
        SMSAPI.BringToFront()
    End Sub
    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        If Directory.Exists(Application.StartupPath & "\Backup") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Backup")
        End If
        Dim compfolder As String = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last() & "\Data.db"
        If compfolder = "Data\Data.db" Then compfolder = "Data.db"
        Dim compfolder1 As String = compfolder
        If compfolder <> "Data.db" Then compfolder = compfolder.Substring(0, compfolder.LastIndexOf("\"))
        If Directory.Exists(Application.StartupPath & "\Backup\" & compfolder) = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Backup\" & compfolder)
        End If
        Dim FileName As String = "Data-" & clsFun.GetServerDate().Replace("-", "") & ".db"
        'clsFun.CloseConnection()
        File.Copy(Application.StartupPath & "\Data\" & compfolder1, Application.StartupPath & "\Backup\" & compfolder & "\" & FileName, True)
        Me.Dispose()
        ShowCompanies.Show()
    End Sub
    Private Sub CollectionReportToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Collection_Report.MdiParent = Me
        Collection_Report.Show()
        Collection_Report.BringToFront()
        Collection_Report.mskFromDate.Focus()
    End Sub
    Private Sub StockSaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StockSaleToolStripMenuItem.Click
        Stock_Sale.MdiParent = Me
        Stock_Sale.Show()
        Stock_Sale.BringToFront()
        Stock_Sale.mskEntryDate.Focus()
    End Sub



    Private Sub ClearDataToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        If MessageBox.Show("Are you Sure want to Record Delete, It can't Be Reverse ??", "Be careFul", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
            clsFun.ExecNonQuery("Delete from Items;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Items';")
            clsFun.ExecNonQuery("Delete  From Accounts where Tag=1;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Accounts';")
            clsFun.ExecNonQuery("Delete from Ledger;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Ledger';")
            clsFun.ExecNonQuery("Delete from Transaction1;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Transaction1';")
            clsFun.ExecNonQuery("Delete from Transaction2;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Transaction2';")
            clsFun.ExecNonQuery("Delete from purchase;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Purchase';")
            clsFun.ExecNonQuery("Delete from Vouchers;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Vouchers';")
            clsFun.ExecNonQuery("Delete from CrateMarka;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='CrateMarka';")
            clsFun.ExecNonQuery("Delete from CrateVoucher;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='CrateVoucher';")
            clsFun.ExecNonQuery("Delete from ChargesTrans;UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='ChargesTrans';")
            MsgBox("Data is Null & Fully Refreshed Now", MsgBoxStyle.Information, "Data Refreshed")
        End If

    End Sub






    Private Sub MannuaBeejakToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MannuaBeejakToolStripMenuItem.Click
        Scrip_Register.MdiParent = Me
        Scrip_Register.Show()
        Scrip_Register.BringToFront()
        Scrip_Register.mskFromDate.Focus()
    End Sub

    Private Sub AutoBeejakToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoBeejakToolStripMenuItem.Click
        Auto_Scrip_Register.MdiParent = Me
        Auto_Scrip_Register.Show()
        Auto_Scrip_Register.BringToFront()
        Auto_Scrip_Register.mskFromDate.Focus()
    End Sub


    Private Sub AccountTableUpdateToolStripMenuItem_Click(sender As Object, e As EventArgs)
        clsFun.ExecNonQuery("ALTER TABLE Ledger ADD COLUMN Narration Text ;")
        clsFun.ExecNonQuery("Update Ledger set Narration=(Select AccountName From Vouchers Where ID=ledger.VourchersID) Where Transtype='Receipt' ;")
        clsFun.ExecNonQuery("Update Ledger set Narration=(Select AccountName From Vouchers Where ID=ledger.VourchersID) Where Transtype='Payment' ;")
        clsFun.ExecNonQuery("Update Ledger set Narration=(Select SallerName From Vouchers Where ID=ledger.VourchersID) Where Transtype='Cash Deposit' ;")
        clsFun.ExecNonQuery("Update Ledger set Narration=(Select SallerName From Vouchers Where ID=ledger.VourchersID) Where Transtype='Auto Beejak' ;")
        clsFun.ExecNonQuery("Update Ledger set AccountName='Cash' Where AccountID=7 ;")

        MsgBox("Ledger Table Updated", MsgBoxStyle.Information, "Successfully")
    End Sub

    Private Sub CreateViewsToolStripMenuItem_Click(sender As Object, e As EventArgs)
        clsFun.ExecNonQuery("Create View Stock_Sale_Report as Select V.id as VoucherID,V.entryDate,v.billNo as BillNo,v.sallerName as SallerName,t.ItemName as ItemName,t.accountName As AccountName,t.nug as nug,t.Weight as weight,t.rate as rate,t.per as per,t.Amount as amount,t.Charges as charges,t.TotalAmount as totalAmount,t.TransType as transtype From Vouchers v inner join transaction2 t on v.id=t.VoucherID")
        MsgBox("Data is Refreshed, It's More Ligher Now", MsgBoxStyle.Information, "Successfully")
    End Sub



    Private Sub SpeedSaleRegisterToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SpeedSaleRegisterToolStripMenuItem1.Click
        Speed_Sale_Register.MdiParent = Me
        Speed_Sale_Register.Show()
        Speed_Sale_Register.BringToFront()
        Speed_Sale_Register.MsktoDate.Focus()
    End Sub

    Private Sub SuperSaleRegisterToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SuperSaleRegisterToolStripMenuItem1.Click
        Super_Sale_Register.MdiParent = Me
        Super_Sale_Register.Show()
        Super_Sale_Register.BringToFront()
        Super_Sale_Register.mskFromDate.Focus()
    End Sub

    Private Sub StockSaleRegisterToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StockSaleRegisterToolStripMenuItem1.Click
        Stock_Sale_Register.MdiParent = Me
        Stock_Sale_Register.Show()
        Stock_Sale_Register.BringToFront()
        Stock_Sale_Register.mskFromDate.Focus()
    End Sub
    Private Sub StockTransterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StockTransterToolStripMenuItem.Click
        On_Sale.MdiParent = Me
        On_Sale.Show()
        On_Sale.BringToFront()
        On_Sale.BringToFront()
    End Sub

    Private Sub BalanceSheetViewToolStripMenuItem_Click(sender As Object, e As EventArgs)
        clsFun.ExecNonQuery("Create View Vw_BalanceSheet as Select  grp1.id,grp1.GroupName,grp1.DC,grp1.UnderGroupName, grp1.UnderGroupID,ac.id as acid,ac.AccountName,ac.OpBal from AccountGroup grp1 left join AccountGroup grp2 on grp1.id=grp2.UnderGroupID left join accounts ac on ac.groupid=grp1.id where grp1.id not in(9,22,23,24,25,26,27,29)")
        MsgBox("Data is Refreshed, It's More Ligher Now", MsgBoxStyle.Information, "Successfully")
    End Sub
    Private Sub SuperSaleRegisterSimpleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SuperSaleRegisterSimpleToolStripMenuItem.Click
        Super_Sale_Register_Simple.MdiParent = Me
        Super_Sale_Register_Simple.Show()
        If Not Super_Sale_Register_Simple Is Nothing Then
            Super_Sale_Register_Simple.BringToFront()
        End If
    End Sub
    Private Sub UgrahiRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UgrahiRegisterToolStripMenuItem.Click
        Ugrahi_Report.MdiParent = Me
        Ugrahi_Report.Show()
        If Not Ugrahi_Report Is Nothing Then
            Ugrahi_Report.BringToFront()
        End If
    End Sub
    Private Sub TradingAccountToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles TradingAccountToolStripMenuItem1.Click
        Trading_AccountNew.MdiParent = Me
        Trading_AccountNew.Show()
        If Not Trading_AccountNew Is Nothing Then
            Trading_AccountNew.BringToFront()
        End If
    End Sub

    Private Sub ProfitLossToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ProfitLossToolStripMenuItem1.Click
        Profit_and_Loss_New.MdiParent = Me
        Profit_and_Loss_New.Show()
        Profit_and_Loss_New.BringToFront()
    End Sub

    Private Sub DayBookToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DayBookToolStripMenuItem1.Click
        Day_book.MdiParent = Me
        Day_book.Show()
        If Not Day_book Is Nothing Then
            Day_book.BringToFront()
        End If
    End Sub

    Private Sub TrailBalanceToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles TrailBalanceToolStripMenuItem1.Click
        Trial_Balance.MdiParent = Me
        Trial_Balance.Show()
        Trial_Balance.BringToFront()
    End Sub

    Private Sub LedgerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LedgerToolStripMenuItem.Click
        Ledger.MdiParent = Me
        Ledger.Show()
        Ledger.BringToFront()
        Ledger.Top = 0 : Ledger.Left = 0
    End Sub

    Private Sub CollectionRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CollectionRegisterToolStripMenuItem.Click
        Collection_Report.MdiParent = Me
        Collection_Report.Show()
        If Not Collection_Report Is Nothing Then
            Collection_Report.BringToFront()
        End If
    End Sub

    Private Sub StockBalanceToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StockBalanceToolStripMenuItem1.Click
        Stock_Balance.MdiParent = Me
        Stock_Balance.Show()
        If Not Stock_Balance Is Nothing Then
            Stock_Balance.BringToFront()
        End If
    End Sub

    Private Sub LotWiseStockToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles LotWiseStockToolStripMenuItem1.Click
        Lot_Wise_Stock_Report.MdiParent = Me
        Lot_Wise_Stock_Report.Show()
        If Not Lot_Wise_Stock_Report Is Nothing Then
            Lot_Wise_Stock_Report.BringToFront()
        End If
    End Sub

    Private Sub VehicleWiseReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VehicleWiseReportToolStripMenuItem.Click
        Vehicle_Wise_Stock.MdiParent = Me
        Vehicle_Wise_Stock.Show()
        If Not Vehicle_Wise_Stock Is Nothing Then
            Vehicle_Wise_Stock.BringToFront()
        End If
    End Sub

    Private Sub OutstandingAccountToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OutstandingAccountToolStripMenuItem.Click
        OutStanding.MdiParent = Me
        OutStanding.Show()
        OutStanding.BringToFront()
    End Sub

    Private Sub OutstandingAmountToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles OutstandingAmountToolStripMenuItem1.Click
        OutStanding_Amount_Only.MdiParent = Me
        OutStanding_Amount_Only.Show()
            OutStanding_Amount_Only.BringToFront()
        OutStanding_Amount_Only.Left = 0 : OutStanding_Amount_Only.Top = 0
    End Sub

    Private Sub CashSaleReportToolStripMenuItem1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub ItemSummaryToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub ScripProfitReportToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ScripProfitReportToolStripMenuItem1.Click
        Scrip_Profit_Report.MdiParent = Me
        Scrip_Profit_Report.Show()
        If Not Scrip_Profit_Report Is Nothing Then
            Scrip_Profit_Report.BringToFront()
        End If
    End Sub

    Private Sub CashBankBookToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles CashBankBookToolStripMenuItem.Click
        Cash_Bank_Book.MdiParent = Me
        Cash_Bank_Book.Show()
        If Not Cash_Bank_Book Is Nothing Then
            Cash_Bank_Book.BringToFront()
        End If
    End Sub

    Private Sub HandiCashBookToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Handi_Cash_Book.MdiParent = Me
        Handi_Cash_Book.Show()
        If Not Handi_Cash_Book Is Nothing Then
            Handi_Cash_Book.BringToFront()
        End If
    End Sub

    Private Sub CashBankBookGroupedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CashBankBookGroupedToolStripMenuItem.Click
        Cash_Book_Grouped.MdiParent = Me
        Cash_Book_Grouped.Show()
        If Not Cash_Book_Grouped Is Nothing Then
            Cash_Book_Grouped.BringToFront()
        End If
    End Sub

    Private Sub CrateReciveableTotalToolStripMenuItem_Click(sender As Object, e As EventArgs)
        CrateReceivableTotal.MdiParent = Me
        CrateReceivableTotal.Show()
        CrateReceivableTotal.BringToFront()
    End Sub

    Private Sub AccountBalanceEditorToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Account_Balance_Editor.MdiParent = Me
        Account_Balance_Editor.Show()
        Account_Balance_Editor.BringToFront()

    End Sub
    Public Sub Alert(ByVal msg As String, ByVal type As msgAlert.enmType)
        Dim frm As msgAlert = New msgAlert()
        frm.showAlert(msg, type)
    End Sub

    Private Sub CrateOutstandingToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Crate_Outstanding.MdiParent = Me
        Crate_Outstanding.Show()
        If Not Crate_Outstanding Is Nothing Then
            Crate_Outstanding.BringToFront()
        End If
    End Sub

    Private Sub CrateOutRegisterToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CrateOutRegisterToolStripMenuItem1.Click
        Crate_Out_Register.MdiParent = Me
        Crate_Out_Register.Show()
        If Not Crate_Out_Register Is Nothing Then
            Crate_Out_Register.BringToFront()
        End If
    End Sub

    Private Sub CrateInRecievedRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrateInRecievedRegisterToolStripMenuItem.Click
        Crate_IN_Register.MdiParent = Me
        Crate_IN_Register.Show()
        If Not Crate_IN_Register Is Nothing Then
            Crate_IN_Register.BringToFront()
        End If
    End Sub

    Private Sub CrateLedgerAccountWiseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrateLedgerAccountWiseToolStripMenuItem.Click
        CrateAccountLedger.MdiParent = Me
        CrateAccountLedger.Show()
        CrateAccountLedger.BringToFront()
    End Sub

    Private Sub CrateReciveableTotalToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CrateReciveableTotalToolStripMenuItem1.Click
        CrateReceivableTotal.MdiParent = Me
        CrateReceivableTotal.Show()
        If Not CrateReceivableTotal Is Nothing Then
            CrateReceivableTotal.BringToFront()
        End If
    End Sub

    Private Sub CrateOutstandingToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CrateOutstandingToolStripMenuItem1.Click
        Crate_Outstanding.MdiParent = Me
        Crate_Outstanding.Show()
        If Not Crate_Outstanding Is Nothing Then
            Crate_Outstanding.BringToFront()
        End If
    End Sub

    Private Sub CrateMarkaLedgerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrateMarkaLedgerToolStripMenuItem.Click
        CrateMarkaLedger.MdiParent = Me
        CrateMarkaLedger.Show()
        CrateMarkaLedger.BringToFront()
    End Sub

    Private Sub CrateAccountCumMarkaLedgerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrateAccountCumMarkaLedgerToolStripMenuItem.Click
        CrateCrateAccountCumMarkaLedger.MdiParent = Me
        CrateCrateAccountCumMarkaLedger.Show()
        CrateCrateAccountCumMarkaLedger.BringToFront()
        CrateCrateAccountCumMarkaLedger.Top = 0
        CrateCrateAccountCumMarkaLedger.Left = 0
    End Sub

    Private Sub RunSqliteQueryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunSqliteQueryToolStripMenuItem.Click
        Query_Maker.MdiParent = Me
        Query_Maker.Show()
        Query_Maker.BringToFront()
    End Sub


    Private Sub OrderEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrderEntryToolStripMenuItem.Click
        Order_Book.MdiParent = Me
        Order_Book.Show()
        If Not Order_Book Is Nothing Then
            Order_Book.BringToFront()
        End If
    End Sub

    Private Sub OrderBookToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles OrderBookToolStripMenuItem1.Click
        Order_Register.MdiParent = Me
        Order_Register.Show()
        If Not Order_Register Is Nothing Then
            Order_Register.BringToFront()
        End If
    End Sub

    Private Sub SMSConfigrationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SMSConfigrationToolStripMenuItem.Click
        SMSTemplateRegister.MdiParent = Me
        SMSTemplateRegister.Show()
        If Not SMSTemplateRegister Is Nothing Then
            SMSTemplateRegister.BringToFront()
        End If
    End Sub

    Private Sub MobileAppToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MobileAppToolStripMenuItem.Click
        Mobile_App.MdiParent = Me
        Mobile_App.Show()
        If Not Mobile_App Is Nothing Then
            Mobile_App.BringToFront()
        End If
    End Sub

    Private Sub CrateSummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrateSummaryToolStripMenuItem.Click
        Crate_Summary.MdiParent = Me
        Crate_Summary.Show()
        If Not Crate_Summary Is Nothing Then
            Crate_Summary.BringToFront()
        End If
    End Sub
    Private Sub ChangeButtonsColorToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub
    Dim ClsCommon As CommonClass = New CommonClass()
    Private Sub UpdateCompanyInfoOnServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateCompanyInfoOnServerToolStripMenuItem.Click
        ClsCommon.UpdateCutomerInfo("ADD")
    End Sub



    Private Sub RcptRegister_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs)

    End Sub

    Private Sub RegistrationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistrationToolStripMenuItem.Click
        ApplyLicenseKey.MdiParent = Me
        ApplyLicenseKey.Show()
        ApplyLicenseKey.StartPosition = FormStartPosition.CenterParent
        If Not ApplyLicenseKey Is Nothing Then ApplyLicenseKey.BringToFront()
    End Sub

    Private Sub UsersToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles UsersToolStripMenuItem1.Click

    End Sub

    Private Sub NewUserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewUserToolStripMenuItem.Click
        UserForm.MdiParent = Me
        UserForm.Show()
        UserForm.StartPosition = FormStartPosition.CenterParent
        If Not UserForm Is Nothing Then UserForm.BringToFront()
    End Sub

    Private Sub UserRegisterToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles UserRegisterToolStripMenuItem.Click
        Users_Register.MdiParent = Me
        Users_Register.Show()
        Users_Register.StartPosition = FormStartPosition.CenterParent
        If Not Users_Register Is Nothing Then Users_Register.BringToFront()
    End Sub

    Private Sub BalanceSheetExpendedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BalanceSheetExpendedToolStripMenuItem.Click
        Balance_Sheet.MdiParent = Me
        Balance_Sheet.Show()
        Balance_Sheet.StartPosition = FormStartPosition.CenterParent
        If Not Balance_Sheet Is Nothing Then Balance_Sheet.BringToFront()
    End Sub

    Private Sub UpdatePurchaseCratesToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub AuthorisedPersonOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AuthorisedPersonOnlyToolStripMenuItem.Click
        Authorised_Person_Only.MdiParent = Me
        Authorised_Person_Only.Show()
        Authorised_Person_Only.BringToFront()
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        MainScreenPicture.lbltime.Text = Format(TimeOfDay, "hh:mm tt")
        MainScreenPicture.lblDate.Text = Format(Date.Now, "dd-MM-yyyy")
        MainScreenPicture.lblDay.Text = DateTime.Now.DayOfWeek.ToString()
        Dim sec As Integer = CDate(Format(TimeOfDay, "hh:mm:ss")).Second
        If CLng(sec) Mod 2 > 0 Then
            MainScreenPicture.pbtime.Image = My.Resources.icons8_sand_watch_20px
        Else
            MainScreenPicture.pbtime.Image = My.Resources.icons8_sand_timer_20px
        End If
        '   MainScreenPicture.Dashboard()
    End Sub

    Private Sub MergeAccountsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MergeAccountsToolStripMenuItem.Click
        Merge_Accounts.MdiParent = Me
        Merge_Accounts.Show()
        Merge_Accounts.BringToFront()
    End Sub

    Private Sub CustomerWiseSaleReportToolStripMenuItem_Click(sender As Object, e As EventArgs)
        CustomerWiseSaleSummary.MdiParent = Me
        CustomerWiseSaleSummary.Show()
        CustomerWiseSaleSummary.BringToFront()
    End Sub

    Private Sub StandardSaleToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StandardSaleToolStripMenuItem1.Click
        Standard_Sale_Register.MdiParent = Me
        Standard_Sale_Register.Show()
        Standard_Sale_Register.BringToFront()
    End Sub

    Private Sub MarketFeesReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarketFeesReportToolStripMenuItem.Click
        Market_Tax.MdiParent = Me
        Market_Tax.Show()
        Market_Tax.BringToFront()
    End Sub


    Private Sub ImportFromServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportFromServerToolStripMenuItem.Click
        Import_From_Server.MdiParent = Me
        Import_From_Server.Show()
        Import_From_Server.BringToFront()
    End Sub

    Private Sub SuperSaleSupplierWiseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SuperSaleSupplierWiseToolStripMenuItem.Click
        Super_Sale_Supplier_Wise.MdiParent = Me
        Super_Sale_Supplier_Wise.Show()
        Super_Sale_Supplier_Wise.BringToFront()
    End Sub

    Private Sub PrintBillsOriginalNameToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TempChalanToolStripMenuItem_Click(sender As Object, e As EventArgs)
        TempChallan.MdiParent = Me
        TempChallan.Show()
        TempChallan.BringToFront()
    End Sub

    Private Sub TempBillChallanToolStripMenuItem_Click(sender As Object, e As EventArgs)
        TempChallanRegister.MdiParent = Me
        TempChallanRegister.Show()
        TempChallanRegister.BringToFront()
    End Sub

    Private Sub CreateIndexToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateIndexToolStripMenuItem.Click
        Dim sql As String = "Drop Index if exists AccountGroupIDX;" &
  "Drop Index if exists AccountsIDX;" &
  "Drop Index if exists ChargesIDX;" &
  "Drop Index if exists ItemsIDX;" &
  "Drop Index if exists CrateMarkaIDX;" &
  "Drop Index if exists CrateVoucherIDX;" &
  "Drop Index if exists ItemsIDX;" &
  "Drop Index if exists LedgerIDX;" &
  "Drop Index if exists PurchaseIDX;" &
  "Drop Index if exists StorageIDX;" &
  "Drop Index if exists Trans1Idx;" &
  "Drop Index if exists Trans2IDX;" &
  "Drop Index if exists UnderGroupIDX;" &
  "Drop Index if exists VoucherIDX;" &
  "Drop Index if exists UsersIDX;"
        clsFun.ExecScalarStr(sql)

        sql = "Drop Index if exists AccountIDindex;Create Index AccountIDindex on Accounts(ID,AccountName,GroupID);" &
      "Drop Index if exists AccountIndex;Create Index AccountIndex on Ledger(AccountID,AccountName);" &
      "Drop Index if exists PurchaseIDIndex;Drop Index if exists PurchaseIDIdx;CREATE INDEX PurchaseIDIndex ON Purchase (VoucherID, AccountID, StockHolderID ASC);" &
      "Drop Index if exists PurchaseLotIdx;Create index PurchaseLotIdx on Purchase(LotNo);" &
      "Drop Index if exists StockHolderIndex;CREATE INDEX StockHolderIndex ON Purchase (StockHolderID,StockHolderName);" &
      "Drop Index if exists SallerIndex;CREATE INDEX SallerIndex ON Transaction2 (SallerID);" &
      "Drop Index if exists TransItemID ;CREATE INDEX TransItemID ON Transaction2 (ItemID);" &
      "Drop Index if exists TransLotIdx;Create index TransLotIdx on Transaction2(Lot);" &
      "Drop Index if exists VoucherIDIdx;Create index VoucherIDIdx on Transaction2(VoucherID);" &
      "Drop Index if exists Trans1Idx;CREATE INDEX Trans1Idx ON Transaction1 (PurchaseID);" &
      "Drop Index if exists CrateAccountID;Create Index CrateAccountID on CrateVoucher(AccountID)"
        ' "Drop Index if exists PurcahseItemID;CREATE INDEX PurchaseItemID ON Purchase (ItemID);" & _
        clsFun.ExecScalarStr(sql)
        sql = "Drop Index if exists AccountIDindex;Drop Index if exists AccountIndex;" &
           "Drop Index if exists SallerIndex;Drop Index if exists TransLotIdx;" &
            "Drop Index if exists VoucherIDIdx;Drop Index if exists TransItemID;" &
            "Drop Index if exists CrateAccountID;"
        sql = sql & "Drop Index if exists AccountGroupIDX;CREATE INDEX AccountGroupIDX ON AccountGroup ( ID ASC,UnderGroupID ASC, ParentID ASC);" &
                    "Drop Index if exists AccountsIDX;CREATE INDEX AccountsIDX ON Accounts (ID ASC,GroupID ASC);" &
                    "Drop Index if exists CrateVoucherIDX;CREATE INDEX CrateVoucherIDX ON CrateVoucher (ID ASC,VoucherID ASC,AccountID ASC,CrateID ASC);" &
                    "Drop Index if exists LedgerIDX;CREATE INDEX LedgerIDX ON Ledger (AccountID ASC,VourchersID ASC);"

        ' "Drop Index if exists PurcahseItemID;CREATE INDEX PurchaseItemID ON Purchase (ItemID);" & _
        If Val(ClsFunserver.ExecScalarStr(sql)) > 0 Then

        End If
        clsFun.ExecScalarStr("Vacuum;") : ClsFunserver.ExecScalarStr("Vacuum;")
        MsgBox("Boost Up Completed Successfully...", vbInformation, "Sucessful")
    End Sub


    Private Sub OnSaleTransferRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnSaleTransferRegisterToolStripMenuItem.Click
        On_Sale_Register.MdiParent = Me
        On_Sale_Register.Show()
        On_Sale_Register.BringToFront()
    End Sub

    Private Sub WhatsNewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhatsNewToolStripMenuItem.Click
        Whats_New.MdiParent = Me
        Whats_New.Show()
        Whats_New.BringToFront()
    End Sub

    Private Sub OnSaleReceiptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnSaleReceiptToolStripMenuItem.Click
        OnSale_Receipt_Register.MdiParent = Me
        OnSale_Receipt_Register.Show()
        OnSale_Receipt_Register.BringToFront()
    End Sub

    Private Sub AbsentAccountsListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbsentAccountsListToolStripMenuItem.Click
        Absent_Account_List.MdiParent = Me
        Absent_Account_List.Show()
        Absent_Account_List.BringToFront()
    End Sub

    Private Sub SupplierStatementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierStatementToolStripMenuItem.Click
        Supplier_Statement.MdiParent = Me
        Supplier_Statement.Show()
        Supplier_Statement.BringToFront()
    End Sub

    Private Sub TotalSaleReportToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Total_sale_Report.MdiParent = Me
        Total_sale_Report.Show()
        Total_sale_Report.BringToFront()
    End Sub

    Private Sub SellOutPendingBillsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SellOutPendingBillsToolStripMenuItem.Click
        Sell_Out_Pending_Bills.MdiParent = Me
        Sell_Out_Pending_Bills.Show()
        Sell_Out_Pending_Bills.BringToFront()
    End Sub


    Private Sub CrateWiseOutstandingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrateWiseOutstandingToolStripMenuItem.Click
        CrateWiseOutstanding.MdiParent = Me
        CrateWiseOutstanding.Show()
        CrateWiseOutstanding.BringToFront()
    End Sub

    Private Sub btnStd_Click(sender As Object, e As EventArgs) Handles btnStandardSale.Click
        Standard_Sale.MdiParent = Me
        Standard_Sale.Show()
        Standard_Sale.BringToFront()
        Standard_Sale.mskEntryDate.Focus()
        Standard_Sale.Top = 0 : Standard_Sale.Left = 0
    End Sub

    Private Sub btnStdRegister_Click(sender As Object, e As EventArgs) Handles btnStdSaleRegister.Click
        Standard_Sale_Register.MdiParent = Me
        Standard_Sale_Register.Show()
        Standard_Sale_Register.BringToFront()
        Standard_Sale_Register.mskFromDate.Focus()
    End Sub

    Private Sub OnSaleProfitReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnSaleProfitReportToolStripMenuItem.Click
        On_Sale_Profit_Report.MdiParent = Me
        On_Sale_Profit_Report.Show()
        On_Sale_Profit_Report.BringToFront()
        On_Sale_Profit_Report.mskFromDate.Focus()
    End Sub

    Private Sub SellOutAutoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SellOutAutoToolStripMenuItem.Click
        Sellout_Auto.MdiParent = Me
        Sellout_Auto.Show()
        Sellout_Auto.BringToFront()
        Sellout_Auto.mskEntryDate.Focus()
        Sellout_Auto.Top = 0 : Sellout_Auto.Left = 0
    End Sub

    Private Sub BIllOfSupplyProfitabilityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BIllOfSupplyProfitabilityToolStripMenuItem.Click
        Standard_Sale_Profit_Report.MdiParent = Me
        Standard_Sale_Profit_Report.Show()
        Standard_Sale_Profit_Report.BringToFront()
        Standard_Sale_Profit_Report.mskFromDate.Focus()
    End Sub

    Private Sub TrialBalanceGroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TrialBalanceGroupToolStripMenuItem.Click
        Trial_Balance_Grouped.MdiParent = Me
        Trial_Balance_Grouped.Show()
        Trial_Balance_Grouped.BringToFront()
    End Sub
    Private Sub TestBalanceSheetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestBalanceSheetToolStripMenuItem.Click
        Balance_Sheet.MdiParent = Me
        Balance_Sheet.Show()
        If Not Balance_Sheet Is Nothing Then
            Balance_Sheet.BringToFront()
        End If
    End Sub

    Private Sub DaySummaryToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub UserRightsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserRightsToolStripMenuItem.Click
        User_Rights.MdiParent = Me
        User_Rights.Show()
        User_Rights.BringToFront()
    End Sub

    Private Sub PurchaseStockInRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PurchaseStockInRegisterToolStripMenuItem.Click
        Purchase_Register.MdiParent = Me
        Purchase_Register.Show()
        Purchase_Register.BringToFront()
    End Sub

    Private Sub CratePayableTotalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CratePayableTotalToolStripMenuItem.Click
        CratePayableTotal.MdiParent = Me
        CratePayableTotal.Show()
        CratePayableTotal.BringToFront()
    End Sub

    Private Sub CollectionReportAllAccountsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CollectionReportAllAccountsToolStripMenuItem.Click
        CollectionReportAll.MdiParent = Me
        CollectionReportAll.Show()
        CollectionReportAll.BringToFront()
    End Sub

    Private Sub GroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupToolStripMenuItem.Click

    End Sub

    Private Sub GroupPaymmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupPaymmentToolStripMenuItem.Click
        Group_Payment.MdiParent = Me
        Group_Payment.Show()
        Group_Payment.BringToFront()
    End Sub

    Private Sub CashBankBookPaymentDetailedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CashBankBookPaymentDetailedToolStripMenuItem.Click
        CashBookPayment.MdiParent = Me
        CashBookPayment.Show()
        CashBookPayment.BringToFront()
    End Sub

    Private Sub GroupPaymentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupPaymentToolStripMenuItem.Click
        Group_Payment_Register.MdiParent = Me
        Group_Payment_Register.Show()
        Group_Payment_Register.BringToFront()
    End Sub

    Private Sub GroupReceiptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupReceiptToolStripMenuItem.Click
        Group_Receipt.MdiParent = Me
        Group_Receipt.Show()
        Group_Receipt.BringToFront()
    End Sub

    Private Sub GroupReceiptRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupReceiptRegisterToolStripMenuItem.Click
        Group_Receipt_Register.MdiParent = Me
        Group_Receipt_Register.Show()
        Group_Receipt_Register.BringToFront()
    End Sub

    Private Sub SelloutBillsValueReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelloutBillsValueReportToolStripMenuItem.Click
        SellOUT_Value.MdiParent = Me
        SellOUT_Value.Show()
        SellOUT_Value.BringToFront()
    End Sub

    Private Sub RerarangeInvoiceNoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RerarangeInvoiceNoToolStripMenuItem.Click
        Rearrange_Invoice_No.MdiParent = Me
        Rearrange_Invoice_No.Show()
        Rearrange_Invoice_No.BringToFront()
    End Sub


    Private Sub OutstandingRecievableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OutstandingRecievableToolStripMenuItem.Click
        Outstanding_Dabit.MdiParent = Me
        Outstanding_Dabit.Show()
        Outstanding_Dabit.BringToFront()
    End Sub

    Private Sub OutstandingPayableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OutstandingPayableToolStripMenuItem.Click
        Outstanding_Credit.MdiParent = Me
        Outstanding_Credit.Show()
        Outstanding_Credit.BringToFront()
    End Sub

    Private Sub UpdateStorageInTransToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateStorageInTransToolStripMenuItem.Click
        Dim sql As String = "Update Transaction2 Set StorageID=(Select StorageID From Purchase Where VoucherID=Transaction2.PurchaseID)," _
                            & " StorageName=(Select StorageName From Purchase Where VoucherID=Transaction2.PurchaseID);"
        If clsFun.ExecNonQuery(sql) > 0 Then
            MsgBox("Storage ID and Name Updated in Sales", MsgBoxStyle.Information, "Updated")
        End If

    End Sub

    Private Sub UpdateJoinBillOptionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateJoinBillOptionToolStripMenuItem.Click
        Dim sql As String = "Drop View if exists BillPrints2;CREATE VIEW if not exists BillPrints2 AS SELECT Transaction2.EntryDate, Transaction2.ItemName, Transaction2.AccountName, sum(Transaction2.Nug) AS nug," &
         " round(sum(Transaction2.Weight), 2) AS weight,     Transaction2.Rate,  Transaction2.Per, round(sum(Transaction2.Amount), 2) AS amount," &
         " sum(Transaction2.Charges) AS Charges,sum(Transaction2.TotalAmount) AS Totalamount, Transaction2.CommPer, sum(Transaction2.CommAmt) AS CommAmt," &
         " Transaction2.MPer,sum(Transaction2.MAmt) AS MAmt, Transaction2.RdfPer, sum(Transaction2.RdfAmt) AS RdfAmt,Transaction2.Tare, " &
         " sum(Transaction2.Tareamt) AS Tareamt,  Transaction2.Labour,sum(Transaction2.LabourAmt) AS LabourAmt, Transaction2.MaintainCrate," &
         " Transaction2.Cratemarka, Transaction2.CrateQty, Items.OtherName, Accounts.Othername as AccountNameOther,Accounts.Mobile1 AS MobileNo1,Accounts.Mobile2 AS MobileNo2,Accounts.LFNo as LFNo, Accounts.Area as Area,Accounts.City,Transaction2.TransType, Transaction2.AccountID,transaction2.OnWeight,transaction2.Lot" &
         " FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID" &
         "  WHERE (((Transaction2.TransType) Not In ('Standard Sale','On Sale')))  GROUP BY Transaction2.EntryDate,Transaction2.AccountID, Transaction2.ItemID," &
         " Transaction2.Rate,Transaction2.Per ORDER BY Transaction2.AccountName,Transaction2.ItemName"
        If clsFun.ExecNonQuery(sql) > 0 Then
            MsgBox("Query Updated For Bill Join Option Sucessfully...", vbInformation, "Sucessful")
        End If
    End Sub


    Private Sub StoreTransferToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StoreTransferToolStripMenuItem.Click
        Store_Transfer.MdiParent = Me
        Store_Transfer.Show()
        Store_Transfer.BringToFront()
    End Sub

    Private Sub DayBookCumCashBookToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DayBookCumCashBookToolStripMenuItem.Click
        DayBook_Cum_CashBook.MdiParent = Me
        DayBook_Cum_CashBook.Show()
        DayBook_Cum_CashBook.BringToFront()
    End Sub

    Private Sub UpdateViewsAndPrintingTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateViewsAndPrintingTableToolStripMenuItem.Click
        Dim sql As String = String.Empty
        sql = "Drop View if  exists BillPrints;CREATE VIEW BillPrints AS SELECT Transaction2.EntryDate, Items.ItemName, Accounts.AccountName, Transaction2.Nug, Transaction2.Weight, Transaction2.Rate," &
            "Transaction2.Per, Transaction2.Amount, Transaction2.Charges, Transaction2.TotalAmount, Transaction2.CommPer, Transaction2.CommAmt," &
            "Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, Transaction2.Tare, Transaction2.Tareamt," &
            "Transaction2.Labour, Transaction2.LabourAmt, Transaction2.MaintainCrate, Transaction2.Cratemarka, Transaction2.CrateQty," &
            "Items.OtherName, Accounts.Othername as AccountNameOther, Transaction2.TransType, Transaction2.AccountID,ifnull(Transaction2.OnWeight,'') as OnWeight,ifnull(Transaction2.Lot,'') as Lot,ifnull(Transaction2.Cut,'') as Cut " &
            "FROM (Transaction2 INNER JOIN Accounts ON Transaction2.AccountID = Accounts.ID) INNER JOIN Items ON Transaction2.ItemID = Items.ID " &
            "WHERE (((Transaction2.TransType) Not In ('Standard Sale','On Sale','Store Out','Store Transfer'))) ORDER BY Transaction2.AccountName"
        If clsFun.ExecNonQuery(sql) > 0 Then
            MsgBox("View Updated")
        End If
        If ClsFunPrimary.CheckIfColumnExists("Printing", "M10") = False Then
            sql = String.Empty
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
            sql = String.Empty
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
        MsgBox("View Updated")
    End Sub

    Private Sub SystemInfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SystemInfoToolStripMenuItem.Click
        SystemInfo.MdiParent = Me
        SystemInfo.Show()
        SystemInfo.BringToFront()
    End Sub

    Private Sub LicenceInfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LicenceInfoToolStripMenuItem.Click
        LicenceInfo.MdiParent = Me
        LicenceInfo.Show()
        LicenceInfo.BringToFront()
    End Sub

    Private Sub SaleChallanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaleChallanToolStripMenuItem.Click
        Sale_Challan.MdiParent = Me
        Sale_Challan.Show()
        Sale_Challan.BringToFront()
    End Sub

    Private Sub SaleChallanRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaleChallanRegisterToolStripMenuItem.Click
        Sale_Challan_Register.MdiParent = Me
        Sale_Challan_Register.Show()
        Sale_Challan_Register.BringToFront()
    End Sub

    Private Sub PurchaseStockInRegisterDetailedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PurchaseStockInRegisterDetailedToolStripMenuItem.Click
        Purchase_RegisterDetailed.MdiParent = Me
        Purchase_RegisterDetailed.Show()
        Purchase_RegisterDetailed.BringToFront()
    End Sub

    Private Sub OutstandingDayWiseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OutstandingDayWiseToolStripMenuItem.Click
        Absent_Account_List_Day_Wise.MdiParent = Me
        Absent_Account_List_Day_Wise.Show()
        Absent_Account_List_Day_Wise.BringToFront()
    End Sub

    Private Sub TestBalanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestBalanceToolStripMenuItem.Click
        Balance_Sheet.MdiParent = Me
        Balance_Sheet.Show()
        Balance_Sheet.BringToFront()
    End Sub

    Private Sub TradingAccountBetaToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Trading_AccountNew.MdiParent = Me
        Trading_AccountNew.Show()
        Trading_AccountNew.BringToFront()
    End Sub

    Private Sub ProfitLossToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Profit_and_Loss_New.MdiParent = Me
        Profit_and_Loss_New.Show()
        Profit_and_Loss_New.BringToFront()
    End Sub

    Private Sub BalanceSheetToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Balance_Sheet.MdiParent = Me
        Balance_Sheet.Show()
        Balance_Sheet.BringToFront()
    End Sub

    Private Sub OpeningCrateEditorToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles OpeningCrateEditorToolStripMenuItem1.Click
        Crate_Balance_Editor.MdiParent = Me
        Crate_Balance_Editor.Show()
        Crate_Balance_Editor.BringToFront()
    End Sub

    Private Sub OpeningBalanceEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpeningBalanceEditorToolStripMenuItem.Click
        Account_Balance_Editor.MdiParent = Me
        Account_Balance_Editor.Show()
        Account_Balance_Editor.BringToFront()
    End Sub

    Private Sub ControlsToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub OnSaleReceiptToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles OnSaleReceiptToolStripMenuItem1.Click
        On_Sale_Receipt.MdiParent = Me
        On_Sale_Receipt.Show()
        On_Sale_Receipt.BringToFront()
    End Sub

    Private Sub StockSellerReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StockSellerReportToolStripMenuItem.Click
        Seller_Day_Book.MdiParent = Me
        Seller_Day_Book.Show()
        Seller_Day_Book.BringToFront()
    End Sub

    Private Sub StoreTransferToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StoreTransferToolStripMenuItem1.Click
        Store_Transfer_Register.MdiParent = Me
        Store_Transfer_Register.Show()
        Store_Transfer_Register.BringToFront()
    End Sub


    Private Sub ChargesSettingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChargesSettingToolStripMenuItem.Click
        ChargesSetting.MdiParent = Me
        ChargesSetting.Show()
        ChargesSetting.BringToFront()
    End Sub

    Private Sub TestCashBookToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestCashBookToolStripMenuItem.Click
        CashBookPaymentDayWise.MdiParent = Me
        CashBookPaymentDayWise.Show()
        CashBookPaymentDayWise.BringToFront()
    End Sub

    Private Sub CashBookBankBookToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CashBookBankBookToolStripMenuItem.Click
        CashBookPaymentDayWise.MdiParent = Me
        CashBookPaymentDayWise.Show()
        CashBookPaymentDayWise.BringToFront()
    End Sub


    Private Sub TestCombineReportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestCombineReportsToolStripMenuItem.Click
        Combine_Reports.MdiParent = Me
        Combine_Reports.Show()
        Combine_Reports.BringToFront()
    End Sub

    Private Sub UpdateCutEntriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateCutEntriesToolStripMenuItem.Click
        Dim Sql As String = "Update Transaction2 Set Cut=0"
        If clsFun.ExecNonQuery(Sql) > 0 Then
            MsgBox("Updated Cut Entries.")
        End If
    End Sub

    Private Sub ReceiptNetRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReceiptNetRegisterToolStripMenuItem.Click
        On_Sale_Receipt_Net_Register.MdiParent = Me
        On_Sale_Receipt_Net_Register.Show()
        On_Sale_Receipt_Net_Register.BringToFront()
    End Sub

    Private Sub NetRecieptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NetRecieptToolStripMenuItem.Click
        OnSaleReceipt_Net.MdiParent = Me
        OnSaleReceipt_Net.Show()
        OnSaleReceipt_Net.BringToFront()
    End Sub

    Private Sub NewMarkaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewMarkaToolStripMenuItem.Click
        CrateForm.MdiParent = Me
        CrateForm.Show()
        CrateForm.BringToFront()
        CrateForm.txtCratename.Focus()
    End Sub

    Private Sub MarkaListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarkaListToolStripMenuItem.Click
        Create_marka_list.MdiParent = Me
        Create_marka_list.Show()
        Create_marka_list.BringToFront()
    End Sub

    Private Sub SettleLedgerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettleLedgerToolStripMenuItem.Click
        Settle_Ledger.MdiParent = Me
        Settle_Ledger.Show()
        Settle_Ledger.BringToFront()
        Settle_Ledger.cbAccountName.Focus()
    End Sub

    Private Sub WhatsappConfigrationUnofficialToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhatsappConfigrationUnofficialToolStripMenuItem.Click
        WhatsApp_API.MdiParent = Me
        WhatsApp_API.Show()
        WhatsApp_API.BringToFront()
    End Sub

    Private Sub DayWiseSaleReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DayWiseSaleReportToolStripMenuItem.Click
        Day_Wise_Sale_Report.MdiParent = Me
        Day_Wise_Sale_Report.Show()
        If Not Day_Wise_Sale_Report Is Nothing Then
            Day_Wise_Sale_Report.BringToFront()
        End If
    End Sub

    Private Sub ItemSummaryToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ItemSummaryToolStripMenuItem1.Click
        Item_Summary.MdiParent = Me
        Item_Summary.Show()
        If Not Item_Summary Is Nothing Then
            Item_Summary.BringToFront()
        End If
    End Sub

    Private Sub DaySummaryToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DaySummaryToolStripMenuItem1.Click
        DaySummary.MdiParent = Me
        DaySummary.Show()
        DaySummary.BringToFront()
    End Sub

    Private Sub CustomerWiseSaleReportToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CustomerWiseSaleReportToolStripMenuItem1.Click
        CustomerWiseSaleSummary.MdiParent = Me
        CustomerWiseSaleSummary.Show()
        CustomerWiseSaleSummary.BringToFront()
    End Sub

    Private Sub SupplierVsItemSummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierVsItemSummaryToolStripMenuItem.Click
        Supplier_VS_Item_Summary.MdiParent = Me
        Supplier_VS_Item_Summary.Show()
        Supplier_VS_Item_Summary.BringToFront()
    End Sub

    Private Sub LosseSaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LosseSaleToolStripMenuItem.Click
        Loose_Sale.MdiParent = Me
        Loose_Sale.Show()
        Loose_Sale.BringToFront()
        Loose_Sale.Top = 0 : Loose_Sale.Left = 0
    End Sub

    Private Sub LoosePurchaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoosePurchaseToolStripMenuItem.Click
        Loose_Purchase.MdiParent = Me
        Loose_Purchase.Show()
        Loose_Purchase.BringToFront()
        Loose_Purchase.Top = 0 : Loose_Purchase.Left = 0
    End Sub

    Private Sub LoosePurchaseToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles LoosePurchaseToolStripMenuItem1.Click
        Loose_Purchase_Register.MdiParent = Me
        Loose_Purchase_Register.Show()
        Loose_Purchase_Register.BringToFront()
    End Sub

    Private Sub btnAccount_MouseEnter(sender As Object, e As EventArgs) Handles btnAccount.MouseEnter, btnItem.MouseEnter, btnSpeedSale.MouseEnter,
        btnSpeedRegister.MouseEnter, btnSelloutMannual.MouseEnter, BtnReceipt.MouseEnter, BtnPayment.MouseEnter, BtnBankEntry.MouseEnter, btnSuperSale.MouseEnter,
        btnSuperRegister.MouseEnter, btnPurchase.MouseEnter, btnPurchaseRegister.MouseEnter, btnStockSale.MouseEnter, btnStockSaleRegister.MouseEnter,
        btnStandardSale.MouseEnter, btnStdSaleRegister.MouseEnter, btnSelloutAuto.MouseEnter, BtnDayBook.MouseEnter, BtnLedger.MouseEnter, BtnPayment.MouseEnter,
        btnCashBankBook.MouseEnter, btnOutStanding.MouseEnter, BtnBillPrints.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.ForeColor = Color.White
    End Sub

    Private Sub btnAccount_MouseLeave(sender As Object, e As EventArgs) Handles btnAccount.MouseLeave, btnItem.MouseLeave, btnSpeedSale.MouseLeave,
        btnSpeedRegister.MouseLeave, btnSelloutMannual.MouseLeave, BtnReceipt.MouseLeave, BtnPayment.MouseLeave, BtnBankEntry.MouseLeave, btnSuperSale.MouseLeave,
        btnSuperRegister.MouseLeave, btnPurchase.MouseLeave, btnPurchaseRegister.MouseLeave, btnStockSale.MouseLeave, btnStockSaleRegister.MouseLeave,
        btnStandardSale.MouseLeave, btnStdSaleRegister.MouseLeave, btnSelloutAuto.MouseLeave, BtnDayBook.MouseLeave, BtnLedger.MouseLeave, BtnPayment.MouseLeave,
        btnCashBankBook.MouseLeave, btnOutStanding.MouseLeave, BtnBillPrints.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.ForeColor = Color.Black
    End Sub

    Private Sub SaleLooseRegisterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaleLooseRegisterToolStripMenuItem.Click
        Loose_Sale_Register.MdiParent = Me
        Loose_Sale_Register.Show()
        Loose_Sale_Register.BringToFront()
        Loose_Sale_Register.Top = 0
        Loose_Sale_Register.Left = 0
    End Sub

    Private Sub LooseSaleReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LooseSaleReportToolStripMenuItem.Click
        Loose_Sale_Profit_Report.MdiParent = Me
        Loose_Sale_Profit_Report.Show()
        Loose_Sale_Profit_Report.BringToFront()
    End Sub

    Private Sub SettleLedgerToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SettleLedgerToolStripMenuItem1.Click
        Sub_Ledger.MdiParent = Me
        Sub_Ledger.Show()
        Sub_Ledger.BringToFront()
    End Sub

    Private Sub MannualSelloutRegisterDetialedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MannualSelloutRegisterDetialedToolStripMenuItem.Click
        Sellout_Mannual_Detialed_Register.MdiParent = Me
        Sellout_Mannual_Detialed_Register.Show()
        Sellout_Mannual_Detialed_Register.BringToFront()
    End Sub

    Private Sub AccountMonthlySummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AccountMonthlySummaryToolStripMenuItem.Click
        Account_Monthly_Summary.MdiParent = Me
        Account_Monthly_Summary.Show()
        Account_Monthly_Summary.BringToFront()
    End Sub

    Private Sub ITEMSUMMARYSelloutMannualToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ITEMSUMMARYSelloutMannualToolStripMenuItem.Click
        Sellout_Item_Summary.MdiParent = Me
        Sellout_Item_Summary.Show()
        Sellout_Item_Summary.BringToFront()
    End Sub

    Private Sub TestAccountsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestAccountsToolStripMenuItem.Click
        TestForm.MdiParent = Me
        TestForm.Show()
        TestForm.BringToFront()
    End Sub

    Private Sub PartyWiseRateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PartyWiseRateToolStripMenuItem.Click
        RateMaster.MdiParent = Me
        RateMaster.Show()
        RateMaster.BringToFront()
    End Sub

    Private Sub SettleLedgerToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SettleLedgerToolStripMenuItem2.Click
        Settle_Ledger.MdiParent = Me
        Settle_Ledger.Show()
        Settle_Ledger.BringToFront()
    End Sub

    Private Sub CheckForUpdateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckForUpdateToolStripMenuItem.Click
        CheckUpdate.MdiParent = Me
        CheckUpdate.Show()
        CheckUpdate.BringToFront()
    End Sub

    Private Sub DailyNakalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DailyNakalToolStripMenuItem.Click
        Daily_Nakal.MdiParent = Me
        Daily_Nakal.Show()
        Daily_Nakal.BringToFront()
    End Sub

    Private Sub GroupLedgwerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupLedgwerToolStripMenuItem.Click
        Group_Ledger.MdiParent = Me
        Group_Ledger.Show()
        Group_Ledger.BringToFront()
    End Sub
End Class