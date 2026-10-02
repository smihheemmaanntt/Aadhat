Imports System.Data.SQLite

Public Class MainScreenPicture

    Private NotInheritable Class DashboardSnapshot
        Public TransactionStats As DataTable
        Public VoucherStats As DataTable
        Public SalesRows As DataTable
        Public PaymentRows As DataTable
    End Class
    Private _dashboardLoadStarted As Boolean

    Dim ClsCommon As CommonClass = New CommonClass()
    Public Property FinYearStartValue As String = String.Empty
    Public Property FinYearEndValue As String = String.Empty
    Public Sub New()
        Me.SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub MainScreenPicture_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Dim company As DataTable = clsFun.ExecDataTable("Select CompanyName, YearEnd, YearStart, City from Company Limit 1")
        If company.Rows.Count > 0 Then
            lblOrgName.Text = company.Rows(0)("CompanyName").ToString().ToUpper()
            Dim Yearend As String = company.Rows(0)("YearEnd").ToString().ToUpper()
            Dim YearStart As String = company.Rows(0)("YearStart").ToString().ToUpper()
            FinYearStartValue = YearStart
            FinYearEndValue = Yearend
            lblFinYear.Text = CDate(YearStart).Year & " - " & CDate(Yearend).Year
            lblCity.Text = company.Rows(0)("City").ToString().ToUpper()
        End If
        '  pbWait.Visible = True
        ' MainScreenForm.Enabled = False
        PartnerDetails()
        'MainScreenForm.Enabled = True
        pbWait.Visible = False
        lblPath.Text = "Path : " & GlobalData.ConnectionPath
        '   If ClsCommon.IsInternetConnect() Then pbAadhat.Image = New System.Drawing.Bitmap(New IO.MemoryStream(New System.Net.WebClient().DownloadData("https://softmanagementindia.in/images/AadhatKit.png")))
    End Sub
    Public Sub PartnerDetails()
        Dim sql As String = String.Empty
        sql = "create table if not exists Channel(D1 TEXT,D2 TEXT,D3 TEXT,D4 TEXT,YN TEXT);"
        ClsFunPrimary.ExecNonQuery(sql)
        Dim channel As DataTable = ClsFunPrimary.ExecDataTable("Select D1, D2, D3, D4, YN from Channel Limit 1")
        If channel.Rows.Count > 0 AndAlso channel.Rows(0)("YN").ToString() = "Y" Then
            pnlpartnerDetails.Visible = True : pnlFirm.Visible = True
            lblpartnerName.Text = channel.Rows(0)("D2").ToString().ToUpper()
            lblMobile.Text = channel.Rows(0)("D3").ToString().ToUpper()
            lblWeb.Text = channel.Rows(0)("D4").ToString()
            lblFirmName.Text = channel.Rows(0)("D1").ToString().ToUpper()
        Else
            pnlpartnerDetails.Visible = False : pnlFirm.Visible = False
        End If

    End Sub
    Public Sub Dashboard()
        ' Do not pump the message queue while the dashboard values are being
        ' read.  Doing so allows Windows to paint a half-updated card (blank
        ' labels/pictures) before the remaining database values are applied.
        ' The normal UI loop repaints once this method finishes.
        Dim today As String = Date.Now.ToString("yyyy-MM-dd")
        ' Aggregate each source table in one pass; keep the original sources
        ' for each card (some counts come from Vouchers while amounts come from Transaction2).
        Dim transactionStats As DataTable = clsFun.ExecDataTable("SELECT TransType, SUM(TotalAmount) AS Amount, COUNT(*) AS RowCount FROM Transaction2 WHERE TransType IN ('Speed Sale','Super Sale') AND EntryDate='" & today & "' GROUP BY TransType")
        Dim voucherStats As DataTable = clsFun.ExecDataTable("SELECT TransType, SUM(TotalAmount) AS Amount, COUNT(*) AS RowCount FROM Vouchers WHERE TransType IN ('Super Sale','Stock Sale','Standard Sale','Purchase','Receipt','Payment','Auto Beejak','Beejak') AND EntryDate='" & today & "' GROUP BY TransType")
        ApplyDashboardStats(transactionStats, voucherStats)
    End Sub

    Private Sub ApplyDashboardStats(ByVal transactionStats As DataTable, ByVal voucherStats As DataTable)
        Dim amounts As New Dictionary(Of String, Double)(StringComparer.OrdinalIgnoreCase)
        Dim counts As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        For Each row As DataRow In transactionStats.Rows
            amounts(row("TransType").ToString()) = Val(row("Amount").ToString())
            counts(row("TransType").ToString()) = Val(row("RowCount").ToString())
        Next
        For Each row As DataRow In voucherStats.Rows
            ' Voucher counts override transaction counts for cards that historically used Vouchers.
            If row("TransType").ToString() <> "Speed Sale" Then counts(row("TransType").ToString()) = Val(row("RowCount").ToString())
            If Not amounts.ContainsKey(row("TransType").ToString()) Then amounts(row("TransType").ToString()) = Val(row("Amount").ToString())
        Next
        lblSpeedAmt.Text = Format(GetStatAmount(amounts, "Speed Sale"), "0.00")
        lblSpeedTotal.Text = GetStatCount(counts, "Speed Sale")
        lblSuperAmt.Text = Format(GetStatAmount(amounts, "Super Sale"), "0.00")
        lblSuperTotal.Text = GetStatCount(counts, "Super Sale")
        lblStockAmt.Text = Format(GetStatAmount(amounts, "Stock Sale"), "0.00")
        LblStockTotal.Text = GetStatCount(counts, "Stock Sale")
        lblStdAmt.Text = Format(GetStatAmount(amounts, "Standard Sale"), "0.00")
        lblStdTotal.Text = GetStatCount(counts, "Standard Sale")
        lblPurchaseAmt.Text = Format(GetStatAmount(amounts, "Purchase"), "0.00")
        lblPurchaseTotal.Text = GetStatCount(counts, "Purchase")
        lblTotReceiptamt.Text = Format(GetStatAmount(amounts, "Receipt"), "0.00")
        lblTotReceipt.Text = GetStatCount(counts, "Receipt")
        lblPaymentAmt.Text = Format(GetStatAmount(amounts, "Payment"), "0.00")
        lblPayment.Text = GetStatCount(counts, "Payment")
        lblAutoReceipt.Text = Format(GetStatAmount(amounts, "Auto Beejak"), "0.00")
        lblAuto.Text = GetStatCount(counts, "Auto Beejak")
        lblMannualAmt.Text = Format(GetStatAmount(amounts, "Beejak"), "0.00")
        lblMannual.Text = GetStatCount(counts, "Beejak")
    End Sub

    Public Sub QueueDashboardLoad()
        If _dashboardLoadStarted OrElse IsDisposed Then Return
        _dashboardLoadStarted = True
        System.Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadDashboardSnapshot)
    End Sub

    Private Sub LoadDashboardSnapshot(ByVal state As Object)
        Dim snapshot As New DashboardSnapshot()
        Try
            Dim today As String = Date.Now.ToString("yyyy-MM-dd")
            Dim connectionString As String
            If String.Equals(GlobalData.ConnectionPath, "Data", StringComparison.OrdinalIgnoreCase) Then
                connectionString = "Data Source=|DataDirectory|" & GlobalData.ConnectionPath & ";Version=3;New=True;Compress=True;synchronous=ON;cache_size =100000;Page Size=2048;PRAGMA mmap_size=268435456;"
            Else
                connectionString = "Data Source= " & GlobalData.ConnectionPath & ";Version=3;New=True;Compress=True;synchronous=ON;cache_size =100000;Page Size=2048;PRAGMA mmap_size=268435456;"
            End If
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()
                snapshot.TransactionStats = ReadDashboardTable(connection,
                    "SELECT TransType, SUM(TotalAmount) AS Amount, COUNT(*) AS RowCount FROM Transaction2 WHERE TransType IN ('Speed Sale','Super Sale') AND EntryDate='" & today & "' GROUP BY TransType")
                snapshot.VoucherStats = ReadDashboardTable(connection,
                    "SELECT TransType, SUM(TotalAmount) AS Amount, COUNT(*) AS RowCount FROM Vouchers WHERE TransType IN ('Super Sale','Stock Sale','Standard Sale','Purchase','Receipt','Payment','Auto Beejak','Beejak') AND EntryDate='" & today & "' GROUP BY TransType")
                snapshot.SalesRows = ReadDashboardTable(connection,
                    "SELECT ac.ID, ac.TransType, grp.AccountName AS AccountName1, ac.Nug AS Nug, grp.Nug AS Nug1, grp.Weight, grp.TotalAmount AS TotalAmount1 FROM Vouchers AS ac INNER JOIN Transaction2 AS grp ON ac.ID = grp.VoucherID WHERE ac.TransType IN ('Speed Sale','Stock Sale','Super Sale','Standard Sale') AND ac.EntryDate='" & today & "' ORDER BY ac.ID DESC LIMIT 10")
                snapshot.PaymentRows = ReadDashboardTable(connection,
                    "SELECT ID, TransType, AccountName, BasicAmount, DiscountAmount, TotalAmount FROM Vouchers WHERE TransType IN ('Payment','Receipt') AND EntryDate='" & today & "' ORDER BY ID DESC LIMIT 10")
            End Using

            If IsDisposed OrElse Not IsHandleCreated Then Return
            BeginInvoke(New MethodInvoker(Sub()
                                              If IsDisposed Then Return
                                              ApplyDashboardStats(snapshot.TransactionStats, snapshot.VoucherStats)
                                              ApplySalesRows(snapshot.SalesRows)
                                              ApplyPaymentRows(snapshot.PaymentRows)
                                          End Sub))
        Catch
            ' Dashboard cards are secondary; a read failure must not delay login
            ' or prevent the main workspace from opening.
        End Try
    End Sub

    Private Shared Function ReadDashboardTable(ByVal connection As SQLiteConnection, ByVal sql As String) As DataTable
        Using adapter As New SQLiteDataAdapter(sql, connection)
            Dim table As New DataTable()
            adapter.Fill(table)
            Return table
        End Using
    End Function

    Private Function GetStatAmount(stats As Dictionary(Of String, Double), transType As String) As Double
        If stats.ContainsKey(transType) Then Return stats(transType)
        Return 0
    End Function

    Private Function GetStatCount(stats As Dictionary(Of String, Integer), transType As String) As Integer
        If stats.ContainsKey(transType) Then Return stats(transType)
        Return 0
    End Function
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Type" : dg1.Columns(1).Width = 80
        Me.dg1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Name" : dg1.Columns(2).Width = 100
        Me.dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "Nug" : dg1.Columns(3).Width = 50
        Me.dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).Name = "Kg" : dg1.Columns(4).Width = 50
        Me.dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).Name = "Total" : dg1.Columns(5).Width = 80
        Me.dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Public Sub retrive()
        Dashboard()
        Dim dt As DataTable = clsFun.ExecDataTable("SELECT ac.ID, ac.TransType, grp.AccountName AS AccountName1, ac.Nug AS Nug, grp.Nug AS Nug1, grp.Weight, grp.TotalAmount AS TotalAmount1 FROM Vouchers AS ac INNER JOIN Transaction2 AS grp ON ac.ID = grp.VoucherID WHERE ac.TransType IN ('Speed Sale','Stock Sale','Super Sale','Standard Sale') AND ac.EntryDate='" & CDate(DateTime.Now).ToString("yyyy-MM-dd") & "' ORDER BY ac.ID DESC LIMIT 10")
        ApplySalesRows(dt)
    End Sub

    Private Sub ApplySalesRows(ByVal dt As DataTable)
        dg1.Rows.Clear()
        If dt.Rows.Count > 0 And dg1.ColumnCount = 0 Then pnlSale.Visible = True : rowColums()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    '          Application.DoEvents()
                    dg1.ClearSelection()
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
                        .Cells(1).Value = dt.Rows(i)("Transtype").tostring
                        If "Speed Sale" = dt.Rows(i)("Transtype").tostring Or "Super Sale" = dt.Rows(i)("Transtype").tostring Then
                            .Cells(2).Value = dt.Rows(i)("AccountName1").ToString()
                            .Cells(3).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                            .Cells(4).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                            .Cells(5).Value = Format(Val(dt.Rows(i)("TotalAmount1").ToString()), "0.00")
                        ElseIf "Stock Sale" = dt.Rows(i)("Transtype").tostring Then
                            .Cells(2).Value = dt.Rows(i)("AccountName1").ToString()
                            .Cells(3).Value = Format(Val(dt.Rows(i)("Nug1").ToString()), "0.00")
                            .Cells(4).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                            .Cells(5).Value = Format(Val(dt.Rows(i)("TotalAmount1").ToString()), "0.00")
                        ElseIf "Standard Sale" = dt.Rows(i)("Transtype").tostring Then
                            .Cells(2).Value = dt.Rows(i)("AccountName1").ToString()
                            .Cells(3).Value = Format(Val(dt.Rows(i)("Nug1").ToString()), "0.00")
                            .Cells(4).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                            .Cells(5).Value = Format(Val(dt.Rows(i)("TotalAmount1").ToString()), "0.00")
                        End If
                        .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox("a")
        End Try
        dg1.ClearSelection()
    End Sub
    Private Sub rowColums2()
        dg2.ColumnCount = 6
        dg2.Columns(0).Name = "ID" : dg2.Columns(0).Visible = False
        dg2.Columns(1).Name = "Type" : dg2.Columns(1).Width = 80
        dg2.Columns(2).Name = "Name" : dg2.Columns(2).Width = 100
        dg2.Columns(3).Name = "Basic" : dg2.Columns(3).Width = 50
        dg2.Columns(4).Name = "Dis" : dg2.Columns(4).Width = 50
        dg2.Columns(5).Name = "Total" : dg2.Columns(5).Width = 80
    End Sub
    Public Sub retrive2()
        Dim dt As DataTable = clsFun.ExecDataTable("Select ID, Transtype, AccountName, BasicAmount, DiscountAmount, TotalAmount FROM Vouchers where Transtype in('Payment','Receipt') and EntryDate='" & CDate(DateTime.Now).ToString("yyyy-MM-dd") & "'  Order By ID Desc LIMIT 10")
        ApplyPaymentRows(dt)
    End Sub

    Private Sub ApplyPaymentRows(ByVal dt As DataTable)
        dg2.Rows.Clear()
        If dt.Rows.Count > 0 And dg2.ColumnCount = 0 Then Panel10.Visible = True : rowColums2()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    ' Application.DoEvents()
                    dg2.ClearSelection()
                    dg2.Rows.Add()
                    With dg2.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
                        .Cells(1).Value = dt.Rows(i)("Transtype").tostring
                        .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(3).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(4).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox("a")
        End Try
        dg2.ClearSelection()
    End Sub

    Private Sub GenericResponsiveFormScaler_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        GenericResponsiveFormScaler.Bind(Me)
    End Sub
End Class
