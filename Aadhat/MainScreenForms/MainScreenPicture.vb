Public Class MainScreenPicture

    Dim ClsCommon As CommonClass = New CommonClass()
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
        lblOrgName.Text = clsFun.ExecScalarStr("Select CompanyName from Company").ToUpper
        Dim Yearend As String = clsFun.ExecScalarStr("Select YearEnd from Company").ToUpper
        Dim YearStart As String = clsFun.ExecScalarStr("Select YearStart from Company").ToUpper
        lblFinYear.Text = CDate(YearStart).Year & " - " & CDate(Yearend).Year
        lblCity.Text = clsFun.ExecScalarStr("Select City from Company").ToUpper
        '  pbWait.Visible = True
        ' MainScreenForm.Enabled = False
        PartnerDetails() : retrive() : retrive2()
        'MainScreenForm.Enabled = True
        pbWait.Visible = False
        lblPath.Text = "Path : " & GlobalData.ConnectionPath
        '   If ClsCommon.IsInternetConnect() Then pbAadhat.Image = New System.Drawing.Bitmap(New IO.MemoryStream(New System.Net.WebClient().DownloadData("https://softmanagementindia.in/images/AadhatKit.png")))
    End Sub
    Public Sub PartnerDetails()
        Dim sql As String = String.Empty
        sql = "create table if not exists Channel(D1 TEXT,D2 TEXT,D3 TEXT,D4 TEXT,YN TEXT);"
        ClsFunPrimary.ExecNonQuery(sql)
        If ClsFunPrimary.ExecScalarStr("Select YN from Channel") = "Y" Then
            pnlpartnerDetails.Visible = True : pnlFirm.Visible = True
            lblpartnerName.Text = ClsFunPrimary.ExecScalarStr("Select D2 from Channel").ToUpper
            lblMobile.Text = ClsFunPrimary.ExecScalarStr("Select D3 from Channel").ToUpper
            lblWeb.Text = ClsFunPrimary.ExecScalarStr("Select D4 from Channel")
            lblFirmName.Text = ClsFunPrimary.ExecScalarStr("Select D1 from Channel").ToUpper
        Else
            pnlpartnerDetails.Visible = False : pnlFirm.Visible = False
        End If

    End Sub
    Public Sub Dashboard()
        Application.DoEvents()
        lblSpeedAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Transaction2 Where TransType='Speed Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblSpeedTotal.Text = Val(clsFun.ExecScalarStr("Select Count(*) from Transaction2 Where TransType='Speed Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblSuperAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Transaction2 Where TransType='Super Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblSuperTotal.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Super Sale' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblStockAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Stock Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        LblStockTotal.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Stock Sale' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblStdAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Standard Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblStdTotal.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Standard Sale' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblPurchaseAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Purchase' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblPurchaseTotal.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Purchase' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblTotReceiptamt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Receipt' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblTotReceipt.Text = Val(clsFun.ExecScalarStr("Select Count(*) from Vouchers Where TransType='Receipt' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblPaymentAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Payment' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblPayment.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Payment' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblAutoReceipt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Auto Beejak' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblAuto.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Auto Beejak' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        lblMannualAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Beejak' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        lblMannual.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Beejak' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
    End Sub
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
        dg1.Rows.Clear()
        Dim dt As DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers AS ac INNER JOIN Transaction2 AS grp ON ac.ID = grp.VoucherID where ac.Transtype in('Speed Sale','Stock Sale', 'Super Sale', 'Standard Sale')  and  ac.EntryDate='" & CDate(DateTime.Now).ToString("yyyy-MM-dd") & "'   Order By ID Desc LIMIT 10")
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
        dg2.Rows.Clear()
        Dim dt As DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers  where Transtype in('Payment','Receipt') and EntryDate='" & CDate(DateTime.Now).ToString("yyyy-MM-dd") & "'  Order By ID Desc LIMIT 10")
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
End Class