Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text

Public Class Standard_Sale_Register
    Private headerCheckBox As CheckBox = New CheckBox()
    Private WhatsappCheckBox As CheckBox = New CheckBox()
    Dim FilePath As String : Dim hostedFilePath As String
    Dim access_token As String = "6687c047a58e1"
    Dim instance_id As String = ClsFunPrimary.ExecScalarStr("Select InstanceID From API")
    Dim APIResposne As String : Dim whatsappSender As New WhatsAppSender()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub Standard_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then If pnlWhatsapp.Visible = True Then pnlWhatsapp.Visible = False : mskFromDate.Focus() : Exit Sub
        If e.KeyCode = Keys.Escape Then If pnlArea.Visible = True Then pnlArea.Visible = False : mskFromDate.Focus() : Exit Sub
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown, btnShow.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        'Check to ensure that the row CheckBox is clicked.
        ' dg1.Focus()
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        mskFromDate.SelectAll()
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub

    Private Sub Standard_Sale_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : mskFromDate.Focus()
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text) : MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
        rowColums() : lblitemSearch.Visible = False : txtItemSearch.Visible = False
    End Sub
    Private Sub rowColums()
        dg1.Columns.Clear()
        dg1.ColumnCount = 13
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        headerCheckBox.Location = New Point(headerCellLocation.X + 8, headerCellLocation.Y + 2)
        headerCheckBox.BackColor = Color.GhostWhite
        headerCheckBox.Size = New Size(18, 18)
        AddHandler headerCheckBox.Click, AddressOf HeaderCheckBox_Clicked
        dg1.Controls.Add(headerCheckBox)
        Dim checkBoxColumn As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = ""
        checkBoxColumn.Width = 30
        checkBoxColumn.Name = "checkBoxColumn"
        dg1.Columns.Insert(0, checkBoxColumn)
        'Assign Click event to the DataGridView Cell.
        AddHandler dg1.CellContentClick, AddressOf dg1_CellClick
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Date" : dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "V.No." : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Vehicle" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Account Name" : dg1.Columns(5).Width = 200
        dg1.Columns(6).Name = "Nug" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Weight" : dg1.Columns(7).Width = 100
        dg1.Columns(8).Name = "Rate" : dg1.Columns(8).Visible = False
        dg1.Columns(9).Name = "Net" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "Charges" : dg1.Columns(10).Width = 100
        dg1.Columns(11).Name = "Oth.exp." : dg1.Columns(11).Width = 100
        dg1.Columns(12).Name = "R.O." : dg1.Columns(12).Width = 50
        dg1.Columns(13).Name = "Total" : dg1.Columns(13).Width = 100
        dg1.Columns(0).ReadOnly = False
        ' dg1.Rows(0).ReadOnly = False
    End Sub

    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtRoundoff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(11).Value) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            txtRoundoff.Text = Format(Val(txtRoundoff.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
        Next
    End Sub
    Public Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers v LEFT JOIN Accounts a ON v.AccountID = a.ID WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "'  " & condtion & " order by EntryDate,CAST(BillNo AS INTEGER)")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                        .Cells(3).ReadOnly = True : .Cells(4).ReadOnly = True
                        .Cells(5).ReadOnly = True : .Cells(6).ReadOnly = True
                        .Cells(7).ReadOnly = True : .Cells(8).ReadOnly = True
                        .Cells(9).ReadOnly = True : .Cells(10).ReadOnly = True
                        .Cells(11).ReadOnly = True
                        .Cells(1).Value = dt.Rows(i)("id").ToString()
                        .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(3).Value = dt.Rows(i)("billNo").ToString()
                        .Cells(4).Value = dt.Rows(i)("VehicleNo").ToString()
                        .Cells(5).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                        .Cells(12).Value = Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                        .Cells(13).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
        lblstdTotal.Visible = True
        lblstdTotal.Text = "Total Bills : " & dg1.Rows.Count
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If ckShowItems.Checked = True Then
            dg1.Controls.Remove(headerCheckBox)
            lblitemSearch.Visible = True : txtItemSearch.Visible = True
            rowColums1() : retrive1()
            lblArea.Visible = False : txtItemSearch.BringToFront()
        Else
            dg1.Controls.Add(headerCheckBox)
            lblitemSearch.Visible = False : txtItemSearch.Visible = False
            rowColums() : retrive() : TempRowColumn()
            lblArea.Visible = False
        End If

    End Sub
    Private Sub rowColums1()
        dg1.Columns.Clear()
        dg1.ColumnCount = 16
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "No." : dg1.Columns(2).Width = 60
        dg1.Columns(3).Name = "V.No." : dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Customer" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Item" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Lot" : dg1.Columns(6).Width = 80
        dg1.Columns(7).Name = "Seller" : dg1.Columns(7).Width = 140
        dg1.Columns(8).Name = "Nug" : dg1.Columns(8).Width = 60
        dg1.Columns(9).Name = "Kg" : dg1.Columns(9).Width = 60
        dg1.Columns(10).Name = "Rate" : dg1.Columns(10).Width = 60
        dg1.Columns(11).Name = "Per" : dg1.Columns(11).Width = 50
        dg1.Columns(12).Name = "Basic" : dg1.Columns(12).Width = 80
        dg1.Columns(13).Name = "Charges" : dg1.Columns(13).Width = 80
        dg1.Columns(14).Name = "Oth.Ch." : dg1.Columns(14).Width = 60
        dg1.Columns(15).Name = "Total" : dg1.Columns(15).Width = 100
    End Sub
    Public Sub retrive1(Optional ByVal Primary As String = "", Optional ByVal Secondary As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        ' dt = clsFun.ExecDataTable("Select * FROM Stock_Sale_Report Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' " & Primary & "" & Secondary & "  order by EntryDate,BillNo,Voucherid ")
        dt = clsFun.ExecDataTable("Select * FROM Vouchers v   INNER JOIN   Transaction2 t ON v.id = t.VoucherID LEFT JOIN Accounts a ON t.AccountID = a.ID  WHERE V.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and V.transtype='" & Me.Text & "' " & Primary & "" & Secondary & "  order by V.EntryDate")
        Dim vchid As Integer = 0
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        If i = 0 Then
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                            .Cells(14).Value = IIf(Val(dt.Rows(i)("TotalCharges").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00"))
                            .Cells(15).Value = IIf(Val(dt.Rows(i)("TotalAmount").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00"))
                        ElseIf vchid = dt.Rows(i)("Voucherid").ToString() Then
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                            .Cells(4).Value = ""
                            .Cells(14).Value = ""
                            .Cells(15).Value = ""
                        Else
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                            .Cells(14).Value = IIf(Val(dt.Rows(i)("TotalCharges").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00"))
                            .Cells(15).Value = IIf(Val(dt.Rows(i)("TotalAmount").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00"))
                        End If
                        .Cells(5).Value = dt.Rows(i)("ItemName1").ToString()
                        .Cells(6).Value = dt.Rows(i)("Lot").ToString()
                        .Cells(7).Value = dt.Rows(i)("SallerName1").ToString()
                        .Cells(8).Value = IIf(Val(dt.Rows(i)("Nug1").ToString()) = 0, "", Format(Val(dt.Rows(i)("Nug1").ToString()), "0.00"))
                        .Cells(9).Value = IIf(Val(dt.Rows(i)("Weight").ToString()) = 0, "", Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")) 'Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("Rate1").ToString()), "0.00")
                        .Cells(11).Value = dt.Rows(i)("Per1").ToString()
                        .Cells(12).Value = IIf(Val(dt.Rows(i)("Amount").ToString()) = 0, "", Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")) 'Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(13).Value = IIf(Val(dt.Rows(i)("Charges").ToString()) = 0, "", Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")) 'Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        '.Cells(14).Value = Format(Val(.Cells(15).Value) - Val(Val(.Cells(13).Value) + Val(.Cells(12).Value)), "0.00")
                    End With
                    vchid = dt.Rows(i)("Voucherid").ToString()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc2() : dg1.ClearSelection()
    End Sub
    Sub calc2()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtRoundoff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            txtRoundoff.Text = Format(Val(txtRoundoff.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(15).Value), "0.00")
        Next
    End Sub
    Private Sub PrintRecord()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In tmpgrid.Rows
            With row
                If .Cells(6).Value <> "" Then
                    Dim amtInWords As String = String.Empty
                    Try
                        amtInWords = AmtInWord(.Cells(21).Value)
                    Catch ex As Exception
                        amtInWords = ex.ToString
                    End Try
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," & _
                                "'" & amtInWords & "','" & .Cells(36).Value & "','" & .Cells(37).Value & "','" & .Cells(38).Value & "'," & _
                                "'" & .Cells(39).Value & "','" & .Cells(40).Value & "','" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "'," & _
                                "'" & .Cells(44).Value & "','" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "'," & _
                                "'" & .Cells(49).Value & "','" & .Cells(50).Value & "','" & .Cells(51).Value & "','" & .Cells(52).Value & "','" & .Cells(53).Value & "'," & _
                                "'" & .Cells(54).Value & "','" & .Cells(55).Value & "','" & .Cells(56).Value & "','" & .Cells(57).Value & "','" & .Cells(58).Value & "'," & _
                                "'" & .Cells(59).Value & "','" & .Cells(60).Value & "','" & .Cells(61).Value & "','" & .Cells(62).Value & "','" & .Cells(63).Value & "'," & _
                                "'" & .Cells(64).Value & "','" & .Cells(65).Value & "','" & .Cells(66).Value & "','" & .Cells(67).Value & "','" & .Cells(68).Value & "', " & _
                                "'" & .Cells(69).Value & "','" & .Cells(70).Value & "','" & .Cells(71).Value & "','" & .Cells(72).Value & "','" & .Cells(73).Value & "', " & _
                                "'" & .Cells(74).Value & "','" & .Cells(75).Value & "'," & _
                                "'" & .Cells(27).Value & "','" & .Cells(29).Value & "','" & .Cells(31).Value & "','" & .Cells(33).Value & "','" & .Cells(35).Value & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " & _
                         " P21,P22,P23,P24,P25,P26,P27,P28,P29,P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42,P43,P44,P45, " & _
                         "P46,P47,P48,P49,P50,P51,P52,P53,P54,P55,P56,P57,P58,P59,P60,P61,P62,P63,P64,P65,P66,P67)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
        ' clsFun.ExecNonQuery(sql)
    End Sub

    Private Sub PrintDay2Day()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In tmpgrid.Rows
            With row

                If .Cells(6).Value <> "" Then
                    Dim amtInWords As String = String.Empty
                    Try
                        amtInWords = AmtInWord(.Cells(21).Value)
                    Catch ex As Exception
                        amtInWords = ex.ToString
                    End Try
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," & _
                                "'" & amtInWords & "','" & .Cells(36).Value & "','" & .Cells(37).Value & "','" & .Cells(38).Value & "'," & _
                                "'" & .Cells(39).Value & "','" & .Cells(40).Value & "','" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "'," & _
                                "'" & .Cells(44).Value & "','" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "'," & _
                                "'" & .Cells(49).Value & "','" & .Cells(50).Value & "','" & .Cells(51).Value & "','" & .Cells(52).Value & "','" & .Cells(53).Value & "'," & _
                                "'" & .Cells(54).Value & "','" & .Cells(55).Value & "','" & .Cells(56).Value & "','" & .Cells(57).Value & "','" & .Cells(58).Value & "'," & _
                                "'" & .Cells(59).Value & "','" & .Cells(60).Value & "','" & .Cells(61).Value & "','" & mskFromDate.Text & "','" & MsktoDate.Text & "', " & _
                                "'" & .Cells(62).Value & "','" & .Cells(63).Value & "','" & .Cells(64).Value & "','" & .Cells(65).Value & "','" & .Cells(66).Value & "'," & _
                                "'" & .Cells(67).Value & "','" & .Cells(68).Value & "','" & .Cells(69).Value & "','" & .Cells(70).Value & "','" & .Cells(71).Value & "'," & _
                                "'" & .Cells(72).Value & "','" & .Cells(73).Value & "','" & .Cells(74).Value & "','" & .Cells(75).Value & "','" & .Cells(76).Value & "', " & _
                                "'" & .Cells(77).Value & "','" & .Cells(78).Value & "','" & .Cells(79).Value & "'"


                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " & _
                         " P21,P22,P23,P24,P25,P26,P27,P28,P29,P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42,P43,P44,P45,P46,P47,P48,M9,M10, " & _
                         "P49,P50,P51,P52,P53,P54,P55,P56,P57,P58,P59,P60,P61,P62,P63,P64,P65,P66)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
        ' clsFun.ExecNonQuery(sql)
    End Sub
    Private Sub PrintRegister()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                sql = sql & "insert into Printing(D1,D2,M1,M2, P1, P2,P3, P4,P5,P6,P7,P8,P9,P10,T1,T2,T3,T4,T5,P11) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(5).Value & "'," & _
                    "'" & Val(.Cells(6).Value) & "','" & Val(.Cells(7).Value) & "','" & Val(.Cells(8).Value) & "'," & Val(.Cells(9).Value) & ",'" & Val(.Cells(10).Value) & "'," & _
                    "" & Val(.Cells(11).Value) & "," & Val(.Cells(12).Value) & "," & Val(.Cells(13).Value) & ",'" & txtTotNug.Text & "','" & txtTotweight.Text & "'," & _
                    "'" & txtTotBasic.Text & "','" & txtTotCharge.Text & "','" & TxtGrandTotal.Text & "','" & .Cells(4).Value & "');"
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
            'clsFun.ExecNonQuery("COMMIT;")
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub PrintRegister2()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            Application.DoEvents()
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With dg1.Rows(LastRecord)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                        "'" & .Cells("Date").Value & "','" & .Cells("No.").Value & "','" & .Cells("Customer").Value & "','" & .Cells("Item").Value & "','" & .Cells("Lot").Value & "','" & .Cells("Seller").Value & "'," & _
                        "'" & NullIfZero(.Cells("Nug").Value) & "','" & NullIfZero(.Cells("Kg").Value) & "'," & _
                        "'" & NullIfZero(.Cells("Rate").Value) & "','" & .Cells("Per").Value & "'," & _
                        "'" & NullIfZero(.Cells("Basic").Value) & "','" & NullIfZero(.Cells("Charges").Value) & "','" & NullIfZero(.Cells("Oth.Ch.").Value) & "'," & _
                        "'" & NullIfZero(.Cells("Total").Value) & "','" & NullIfZero(txtTotNug.Text) & "','" & NullIfZero(txtTotweight.Text) & "'," & _
                        "'" & NullIfZero(txtTotBasic.Text) & "','" & NullIfZero(txtTotCharge.Text) & "','" & NullIfZero(TxtGrandTotal.Text) & "'"

                    '        End With
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            ' LastRecord = LastCount
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,T1,T2,T3,T4,T5) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try

        Next
    End Sub
    Private Function NullIfZero(value As Object) As String
        If IsDBNull(value) OrElse value Is Nothing OrElse Val(value) = 0 Then
            Return ""
        Else
            Return Trim(value.ToString())
        End If
    End Function

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If ckShowItems.Checked = True Then
            PrintRegister2()
            Report_Viewer.printReport("\Reports\StandardRegister2.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            Ugrahi_Viewer.BringToFront()
        Else
            PrintRegister()
            Report_Viewer.printReport("\Reports\StandardRegister.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            Ugrahi_Viewer.BringToFront()
        End If

    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In dg1.Rows
                If Convert.ToBoolean(row.Cells("checkBoxColumn").EditedFormattedValue) = False Then
                    isChecked = False
                    Exit For
                End If
            Next
            headerCheckBox.Checked = isChecked
        End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As Integer
            If ckShowItems.Checked = True Then
                tmpID = Val(dg1.SelectedRows(0).Cells(0).Value)
            Else
                tmpID = Val(dg1.SelectedRows(0).Cells(1).Value)
            End If
            Standard_Sale.MdiParent = MainScreenForm
            Standard_Sale.Show()
            Standard_Sale.FillControls(tmpID)
            Standard_Sale.BringToFront()
            Standard_Sale.mskEntryDate.SelectAll()
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As Integer
        If ckShowItems.Checked = True Then
            tmpID = Val(dg1.SelectedRows(0).Cells(0).Value)
        Else
            tmpID = Val(dg1.SelectedRows(0).Cells(1).Value)
        End If
        Standard_Sale.MdiParent = MainScreenForm
        Standard_Sale.Show()
        Standard_Sale.FillControls(tmpID)
        Standard_Sale.BringToFront()
        Standard_Sale.mskEntryDate.SelectAll()
        dg1.ClearSelection()
    End Sub

    Private Sub HeaderCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        'Necessary to end the edit mode of the Cell.
        dg1.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In dg1.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = headerCheckBox.Checked
        Next
    End Sub
    Private Sub IDGentate()
        Dim checkBox As DataGridViewCheckBoxCell
        Dim id As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            retrive2(id, " And upper(Vouchers.accountname) Like upper('" & txtPrimarySearch.Text.Trim() & "%')")
        Else
            id = id.Remove(id.LastIndexOf(","))
            retrive2(id, " And upper(Vouchers.accountname) Like upper('" & txtPrimarySearch.Text.Trim() & "%')")
        End If
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 80
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "EntryDate" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "SallerName" : .Columns(3).Width = 59
            .Columns(4).Name = "BillingType" : .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo" : .Columns(5).Width = 159
            .Columns(6).Name = "itemName" : .Columns(6).Width = 69
            .Columns(7).Name = "Cut" : .Columns(7).Width = 76
            .Columns(8).Name = "Nug" : .Columns(8).Width = 90
            .Columns(9).Name = "Weight" : .Columns(9).Width = 86
            .Columns(10).Name = "Rate" : .Columns(10).Width = 90
            .Columns(11).Name = "per" : .Columns(11).Width = 50
            .Columns(12).Name = "Amount" : .Columns(12).Width = 95
            .Columns(13).Name = "ChargeName" : .Columns(13).Width = 159
            .Columns(14).Name = "onValue" : .Columns(14).Width = 159
            .Columns(15).Name = "@" : .Columns(15).Width = 59
            .Columns(16).Name = "=/-" : .Columns(16).Width = 59
            .Columns(17).Name = "ChargeAmount" : .Columns(17).Width = 69
            .Columns(18).Name = "TotalNug" : .Columns(18).Width = 76
            .Columns(19).Name = "TotalWeight" : .Columns(19).Width = 90
            .Columns(20).Name = "TotalBasicAmount" : .Columns(20).Width = 86
            .Columns(21).Name = "RoundOff" : .Columns(21).Width = 90
            .Columns(22).Name = "TotalAmount" : .Columns(22).Width = 90
            .Columns(23).Name = "OtherItemName" : .Columns(23).Width = 95
            .Columns(24).Name = "OtherAccountName" : .Columns(24).Width = 159
            .Columns(25).Name = "AmountInWords" : .Columns(25).Width = 159
            .Columns(26).Name = "CommPer" : .Columns(26).Width = 90
            .Columns(27).Name = "CommAmt" : .Columns(27).Width = 86
            .Columns(28).Name = "UCPer" : .Columns(28).Width = 90
            .Columns(29).Name = "UcAmt" : .Columns(29).Width = 90
            .Columns(30).Name = "RdfPer" : .Columns(30).Width = 95
            .Columns(31).Name = "RdfAmt" : .Columns(31).Width = 159
            .Columns(32).Name = "Bardanaper" : .Columns(32).Width = 159
            .Columns(33).Name = "BardanaAmt" : .Columns(33).Width = 90
            .Columns(34).Name = "labourPer" : .Columns(34).Width = 90
            .Columns(35).Name = "LabourAmt" : .Columns(35).Width = 95
            .Columns(36).Name = "TotalCharges" : .Columns(36).Width = 159
            .Columns(37).Name = "GrossAmount" : .Columns(37).Width = 159
            .Columns(38).Name = "TotalNugs" : .Columns(38).Width = 159
            .Columns(39).Name = "TotalNugs" : .Columns(39).Width = 159
            .Columns(40).Name = "TotalWeight" : .Columns(40).Width = 159
            .Columns(41).Name = "TotalCommission" : .Columns(41).Width = 159
            .Columns(42).Name = "TotalMarketFees" : .Columns(42).Width = 159
            .Columns(43).Name = "TotalRDF" : .Columns(43).Width = 159
            .Columns(44).Name = "TotalTare" : .Columns(44).Width = 159
            .Columns(45).Name = "TotalLabour" : .Columns(45).Width = 159
            .Columns(46).Name = "Driver Name" : .Columns(46).Width = 159
            .Columns(47).Name = "Mobile" : .Columns(47).Width = 159
            .Columns(48).Name = "Remark" : .Columns(48).Width = 159
            .Columns(49).Name = "GrNo" : .Columns(47).Width = 159
            .Columns(50).Name = "GSTN" : .Columns(48).Width = 159
            .Columns(51).Name = "Cust Mobile" : .Columns(51).Width = 159
            .Columns(52).Name = "Broker Name" : .Columns(52).Width = 159
            .Columns(53).Name = "Broker Mobile" : .Columns(53).Width = 159
            .Columns(54).Name = "TransPort" : .Columns(54).Width = 159
            .Columns(55).Name = "GRNo" : .Columns(55).Width = 159
            .Columns(56).Name = "CrateMarka" : .Columns(56).Width = 159
            .Columns(57).Name = "CrateQty" : .Columns(57).Width = 159
            .Columns(58).Name = "OpeningBal" : .Columns(58).Width = 159
            .Columns(59).Name = "Closingbal" : .Columns(59).Width = 159
            .Columns(60).Name = "TotalCrate" : .Columns(60).Width = 159
            .Columns(61).Name = "CrateDetails" : .Columns(61).Width = 159
            .Columns(62).Name = "ItemTotal" : .Columns(62).Width = 159
            .Columns(63).Name = "PrintCharges" : .Columns(63).Width = 159
            .Columns(64).Name = "Address" : .Columns(64).Width = 159
            .Columns(65).Name = "City" : .Columns(65).Width = 159
            .Columns(66).Name = "State" : .Columns(66).Width = 159
            .Columns(67).Name = "Mobile1" : .Columns(67).Width = 159
            .Columns(68).Name = "Mobile2" : .Columns(68).Width = 159
            .Columns(69).Name = "ItemTotal" : .Columns(69).Width = 159
            .Columns(70).Name = "PrintCharges" : .Columns(70).Width = 15
            .Columns(71).Name = "ReceiptAmt" : .Columns(71).Width = 15
            .Columns(72).Name = "lastRecp" : .Columns(72).Width = 15
            .Columns(73).Name = "lastCrate" : .Columns(73).Width = 15
            .Columns(74).Name = "lastRecp" : .Columns(74).Width = 15
            .Columns(75).Name = "lastCrate" : .Columns(75).Width = 15
            .Columns(76).Name = "FixOpbal" : .Columns(74).Width = 15
            .Columns(77).Name = "FixClBal" : .Columns(75).Width = 15
            .Columns(78).Name = "TotalRec" : .Columns(75).Width = 15
            .Columns(79).Name = "GrandTotal" : .Columns(78).Width = 15
        End With
    End Sub
    Public Sub retrive2(ByVal id As String, Optional ByVal condtion As String = "")
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        Dim opbal As String = String.Empty
        Dim ClBal As String = String.Empty
        Dim FixOpbal As String = String.Empty
        Dim FixClbal As String = String.Empty

        tmpgrid.Rows.Clear()
        If id <> "" Then
            id = " and Vouchers.id in (" & id & ")"
        End If
        dt = clsFun.ExecDataTable("Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo,Vouchers.AccountID, Vouchers.AccountName,Accounts.Address,Accounts.City,Accounts.State,Accounts.Mobile1,Accounts.Mobile2, Vouchers.VehicleNo," _
                                  & "Transaction2.ItemName, Transaction2.Lot, Transaction2.Nug as TransNug, Transaction2.Weight, Transaction2.Rate," _
                                  & "Transaction2.Per, Transaction2.Amount,Transaction2.CommPer,Transaction2.CommAmt,Transaction2.MPer,Transaction2.MAmt," _
                                  & "Transaction2.RdfPer,Transaction2.RdfAmt,Transaction2.Tare,Transaction2.TareAmt,Transaction2.labour,Transaction2.LabourAmt," _
                                  & " Transaction2.TotalAmount as TotAmt,Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, Vouchers.DiscountAmount, Vouchers.TotalCharges, vouchers.SubTotal, " _
                                  & "Vouchers.RoundOff,Vouchers.T1,Vouchers.T2,Vouchers.T3,Vouchers.T4,Vouchers.T5,Vouchers.T6,Vouchers.T7,Vouchers.T8,Vouchers.T9,Vouchers.T10, " _
                                  & "Items.OtherName, Accounts.OtherName as AccountOtherName,Transaction2.Cratemarka as CrateMarka, Transaction2.CrateQty as CrateQty FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)" _
                                  & "INNER JOIN Items ON Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.AccountID = Accounts.ID  Where  Vouchers.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " " & condtion & "")
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
                opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "')" & _
                                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "')) " & _
                                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "')" & _
                                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID").ToString()) & " Order by upper(AccountName) ;"))

                ''''''''''''''''''''closing balance'''''''''''''''''''''''''

                ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "')" & _
                                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "')) " & _
                                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "')" & _
                                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID").ToString()) & " Order by upper(AccountName) ;"))

                FixOpbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                        "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                        " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                        " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID").ToString()) & " Order by upper(AccountName) ;"))

                ''''''''''''''''''''closing balance'''''''''''''''''''''''''

                FixClbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("AccountID").ToString()) & " Order by upper(AccountName) ;"))


                TodaysCredit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "'"))
                todaysDebit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and EntryDate = '" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "'"))
                If ClBal < 0 Then
                    lastbal = Format(Math.Abs(Val(Val(ClBal + todaysDebit) - TodaysCredit)), "0.00") & " Cr"
                Else
                    lastbal = Format(Math.Abs(Val(Val(ClBal - todaysDebit))), "0.00") & " Dr"
                End If

                acID = Val(dt.Rows(i)("AccountID").ToString())
                ''''''Total Crates Show''''''
                Dim U As Integer = 0
                Dim cratebal As String = String.Empty
                Dim CrateQty As String = String.Empty
                Dim CrateName As String = String.Empty
                Dim CQty As String = String.Empty
                Dim SingleCrate As String = String.Empty
                Dim dtcrate As New DataTable
                dtcrate = clsFun.ExecDataTable("Select CrateName,CrateName ||':'||" & _
                " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" & _
                " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable," & _
                            " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') -" & _
                " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) as DueCrates " & _
                " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID,CrateID Having DueCrates<>0 order by upper(ACG.AccountName);")
                Try
                    If dtcrate.Rows.Count > 0 Then
                        For U = 0 To dtcrate.Rows.Count - 1
                            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
                            cratebal = dtcrate.Rows(U)("Reciveable").ToString()
                            CrateQty = CrateQty & ", " & cratebal
                            CrateName = CrateName & dtcrate.Rows(U)("CrateName").ToString() & vbCrLf
                            CQty = CQty & dtcrate.Rows(U)("DueCrates").ToString() & vbCrLf
                            SingleCrate = Val(SingleCrate) + Val(dtcrate.Rows(U)("DueCrates").ToString())
                        Next
                        CrateQty = CrateQty.Trim().TrimStart(",")
                    End If
                Catch ex As Exception

                End Try
                If tmpgrid.RowCount = 0 Then TempRowColumn()
                tmpgrid.Rows.Add()
                Dim RectAmt = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("AccountID")).ToString() & " and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Lot").ToString()
                    .Cells(8).Value = .Cells(8).Value & Val(dt.Rows(i)("TransNug").ToString())
                    .Cells(9).Value = .Cells(9).Value & Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = .Cells(10).Value & Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = .Cells(12).Value & Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(18).Value = .Cells(18).Value & Val(dt.Rows(i)("Nug").ToString())
                    .Cells(19).Value = .Cells(19).Value & Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = .Cells(20).Value & Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = .Cells(21).Value & Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = .Cells(22).Value & Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("AccountOtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & Format(Val(dt.Rows(i)("CommPer").ToString()), "0.00") & vbCrLf
                    .Cells(27).Value = .Cells(27).Value & Format(Val(dt.Rows(i)("CommAmt").ToString()), "0.00") & vbCrLf
                    .Cells(28).Value = .Cells(28).Value & Format(Val(dt.Rows(i)("Mper").ToString()), "0.00") & vbCrLf
                    .Cells(29).Value = .Cells(29).Value & Format(Val(dt.Rows(i)("Mamt").ToString()), "0.00") & vbCrLf
                    .Cells(30).Value = .Cells(30).Value & Format(Val(dt.Rows(i)("RdfPer").ToString()), "0.00") & vbCrLf
                    .Cells(31).Value = .Cells(31).Value & Format(Val(dt.Rows(i)("rdfAmt").ToString()), "0.00") & vbCrLf
                    .Cells(32).Value = .Cells(32).Value & Format(Val(dt.Rows(i)("Tare").ToString()), "0.00") & vbCrLf
                    .Cells(33).Value = .Cells(33).Value & Format(Val(dt.Rows(i)("tareamt").ToString()), "0.00") & vbCrLf
                    .Cells(34).Value = .Cells(34).Value & Format(Val(dt.Rows(i)("Labour").ToString()), "0.00") & vbCrLf
                    .Cells(35).Value = .Cells(35).Value & Format(Val(dt.Rows(i)("Labouramt").ToString()), "0.00") & vbCrLf
                    .Cells(36).Value = .Cells(36).Value & Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                    .Cells(37).Value = .Cells(37).Value & Format(Val(dt.Rows(i)("SubTotal").ToString()), "0.00")
                    .Cells(38).Value = .Cells(38).Value & Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                    .Cells(39).Value = Val(clsFun.ExecScalarStr("Select Sum(Nug) From Transaction2 Where VoucherID= " & Val(dt.Rows(i)("ID").ToString()) & ""))
                    .Cells(40).Value = Format(Val(clsFun.ExecScalarStr("Select Sum(Weight) From Transaction2 Where VoucherID= " & Val(dt.Rows(i)("ID").ToString()) & "")), "0.00") 'Format(Val(dt.Compute("Sum(Weight)", "")), "0.00")
                    .Cells(41).Value = Format(Val(clsFun.ExecScalarStr("Select Sum(CommAmt) From Transaction2 Where VoucherID= " & Val(dt.Rows(i)("ID").ToString()) & "")), "0.00") 'Format(Val(dt.Compute("Sum(CommAmt)", "")), "0.00")
                    .Cells(42).Value = Format(Val(clsFun.ExecScalarStr("Select Sum(Mamt) From Transaction2 Where VoucherID= " & Val(dt.Rows(i)("ID").ToString()) & "")), "0.00") 'Format(Val(dt.Compute("Sum(Mamt)", "")), "0.00")
                    .Cells(43).Value = Format(Val(clsFun.ExecScalarStr("Select Sum(rdfAmt) From Transaction2 Where VoucherID= " & Val(dt.Rows(i)("ID").ToString()) & "")), "0.00") 'Format(Val(dt.Compute("Sum(rdfAmt)", "")), "0.00")
                    .Cells(44).Value = Format(Val(clsFun.ExecScalarStr("Select Sum(tareamt) From Transaction2 Where VoucherID= " & Val(dt.Rows(i)("ID").ToString()) & "")), "0.00") 'Format(Val(dt.Compute("Sum(tareamt)", "")), "0.00")
                    .Cells(45).Value = Format(Val(clsFun.ExecScalarStr("Select Sum(Labouramt) From Transaction2 Where VoucherID= " & Val(dt.Rows(i)("ID").ToString()) & "")), "0.00") 'Format(Val(dt.Compute("Sum(Labouramt)", "")), "0.00")
                    .Cells(46).Value = dt.Rows(i)("T1").ToString()
                    .Cells(47).Value = dt.Rows(i)("T2").ToString()
                    .Cells(48).Value = dt.Rows(i)("T3").ToString()
                    .Cells(49).Value = dt.Rows(i)("T4").ToString()
                    .Cells(50).Value = dt.Rows(i)("T5").ToString()
                    .Cells(51).Value = dt.Rows(i)("T6").ToString()
                    .Cells(52).Value = dt.Rows(i)("T7").ToString()
                    .Cells(53).Value = dt.Rows(i)("T8").ToString()
                    .Cells(54).Value = dt.Rows(i)("T9").ToString()
                    .Cells(55).Value = dt.Rows(i)("T10").ToString()
                    .Cells(56).Value = dt.Rows(i)("CrateMarka").ToString()
                    .Cells(57).Value = dt.Rows(i)("CrateQty").ToString()
                    .Cells(58).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                    .Cells(59).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                    .Cells(60).Value = SingleCrate
                    .Cells(61).Value = CrateQty
                    .Cells(62).Value = Format(Val(dt.Rows(i)("TotAmt").ToString()), "0.00")
                    .Cells(64).Value = dt.Rows(i)("Address").ToString()
                    .Cells(65).Value = dt.Rows(i)("City").ToString()
                    .Cells(66).Value = dt.Rows(i)("State").ToString()
                    .Cells(67).Value = dt.Rows(i)("Mobile1").ToString()
                    .Cells(68).Value = dt.Rows(i)("Mobile2").ToString()
                    .Cells(71).Value = RectAmt 'dt.Rows(i)("Mobile2").ToString()
                    .Cells(72).Value = clsFun.ExecScalarStr("Select  ('Last Receipt Rs. : '|| Sum(Vouchers.TotalAmount)  || ' Disc : '|| Sum(Vouchers.DiscountAmount) ||  ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(3) & " Group by EntryDate ORDER BY Vouchers.Entrydate DESC limit 1 ;")
                    .Cells(73).Value = clsFun.ExecScalarStr("Select 'Total Amount Rec. Rs. :'||Sum(Amount) ||' of Todays Crate Rec. :'||Sum(Qty) as CrateRec  From CrateVoucher Where EntryDate ='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "' and CrateType='Crate In' And Amount<>0 and AccountID=" & dt.Rows(i)(3) & "")
                    .Cells(74).Value = clsFun.ExecScalarStr("Select  ('अंतिम जमा राशि:'|| Sum(Vouchers.TotalAmount)  || ' छूट : '|| Sum(Vouchers.DiscountAmount) ||  ' दिनांक : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(3) & " Group by EntryDate ORDER BY Vouchers.Entrydate DESC limit 1 ;")
                    .Cells(75).Value = clsFun.ExecScalarStr("Select 'आज केरेट जमा राशि:'||Sum(Amount) ||' आज जमा केरेट :'||Sum(Qty) as CrateRec  From CrateVoucher Where EntryDate ='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "' and CrateType='Crate In' And Amount<>0 and AccountID=" & dt.Rows(i)(3) & "")
                    .Cells(76).Value = If(Val(FixOpbal) >= 0, Format(Math.Abs(Val(FixOpbal)), "0.00") & " Dr", Format(Math.Abs(Val(FixOpbal)), "0.00") & " Cr")
                    .Cells(77).Value = If(Val(FixClbal) >= 0, Format(Math.Abs(Val(FixClbal)), "0.00") & " Dr", Format(Math.Abs(Val(FixClbal)), "0.00") & " Cr")
                    .Cells(78).Value = clsFun.ExecScalarStr("Select  Sum(TotaLAmount) as lastReciept FROM Vouchers where TransType='Receipt' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Accountid=" & dt.Rows(i)(3) & " ;")
                    .Cells(79).Value = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) From Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Accountid=" & dt.Rows(i)(3) & "")), "0.00")
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Amount").ToString() & vbCrLf
                            .Cells(63).Value = .Cells(63).Value & clsFun.ExecScalarStr("Select PrintName From Charges Where ID=" & Val(dt1.Rows(j)("ChargesID").ToString()) & "") & vbCrLf
                        Next
                    Else
                        .Cells(13).Value = ""
                        .Cells(14).Value = ""
                        .Cells(15).Value = ""
                        .Cells(16).Value = ""
                        .Cells(17).Value = ""
                        .Cells(63).Value = ""
                    End If
                End With
            Next
        End If
        dt.Clear()
        dt1.Clear()
    End Sub

    Private Sub btnPrintBills_Click(sender As Object, e As EventArgs) Handles btnPrintBills.Click
        If ckShowItems.Checked = True Then MsgBox("Printing Disabled In Items Details Mode", MsgBoxStyle.Critical, "Printing Disabled") : Exit Sub
        IDGentate()
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
            Report_Viewer.printReport("\StandardSale.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Private Sub dg1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentDoubleClick

    End Sub

    Private Sub dg1_ColumnHeaderMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dg1.ColumnHeaderMouseDoubleClick
        dg1.ClearSelection()
    End Sub

    Private Sub txtTotNug_TextChanged(sender As Object, e As EventArgs) Handles txtTotNug.TextChanged

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtTotweight_TextChanged(sender As Object, e As EventArgs) Handles txtTotweight.TextChanged

    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub txtPrimarySearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPrimarySearch.KeyUp
        If dg1.RowCount = 0 Then Exit Sub
        If ckShowItems.Checked = True Then
            If txtPrimarySearch.Text.Trim() <> "" Then
                retrive1(" And upper(v.accountname) Like upper('" & txtPrimarySearch.Text.Trim() & "%')")
            Else
                retrive1()
            End If
        Else
            If txtPrimarySearch.Text.Trim() <> "" Then
                retrive(" And upper(v.accountname) Like upper('" & txtPrimarySearch.Text.Trim() & "%')")
            Else
                retrive()
            End If
        End If

    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As EventArgs) Handles btnPrint2.Click
        If ckShowItems.Checked = True Then MsgBox("Printing Disabled In Items Details Mode", MsgBoxStyle.Critical, "Printing Disabled") : Exit Sub
        IDGentate()
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
            Report_Viewer.printReport("\StandardSale2.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        IDGentate()
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
            Report_Viewer.printReport("\BillofSupplyAll.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub


    Private Sub txtvehicleSearch_TextChanged(sender As Object, e As EventArgs) Handles txtvehicleSearch.TextChanged
        If ckShowItems.Checked = True Then
            If txtvehicleSearch.Text.Trim() <> "" Then
                retrive1(" And upper(VehicleNo) Like upper('" & txtvehicleSearch.Text.Trim() & "%')")
            Else
                retrive1()
            End If
        Else
            If txtvehicleSearch.Text.Trim() <> "" Then
                retrive(" And upper(VehicleNo) Like upper('" & txtvehicleSearch.Text.Trim() & "%')")
            Else
                retrive()
            End If
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ckShowItems.Checked = True Then MsgBox("Printing Disabled In Items Details Mode", MsgBoxStyle.Critical, "Printing Disabled") : Exit Sub
        IDGentate()
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintDay2Day()
            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
            Report_Viewer.printReport("\StdDay2Day.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub


    Private Sub UpdateProgressBar(value As Integer)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Integer)(AddressOf UpdateProgressBar), value)
        Else
            ProgressBar1.Value = value
        End If
    End Sub

    Private Sub UpdateProgressBarVisibility(visible As Boolean)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Boolean)(AddressOf UpdateProgressBarVisibility), visible)
        Else
            ProgressBar1.Visible = visible
        End If
    End Sub
    Private Sub WahSoft()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        UpdateProgressBarVisibility(True)
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim fastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        WABA.ExecNonQuery("Delete from SendingData")
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            With row
                UpdateProgressBar(row.Index)
                If .Cells(0).Value = True Then
                    If btnRadioEnglish.Checked = True And .Cells(3).Value <> "" Then
                        If RadioPDFMsg.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale.rpt")
                            whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                           "'" & .Cells(4).Value & "', '" & whatsappSender.FilePath & "'"
                        ElseIf RadioPdfOnly.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale.rpt")
                            whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                                           "'', '" & whatsappSender.FilePath & "'"
                        ElseIf RadioMsgOnly.Checked = True Then
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                             "'" & .Cells(4).Value & "', ''"
                        End If
                    ElseIf RadioRegional.Checked = True And .Cells(3).Value <> "" Then
                        If RadioPDFMsg.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale2.rpt")
                            whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                           "'" & .Cells(6).Value & "', '" & whatsappSender.FilePath & "'"
                        ElseIf RadioPdfOnly.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale2.rpt")
                            whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                           "'', '" & whatsappSender.FilePath & "'"
                        ElseIf RadioMsgOnly.Checked = True Then
                            If .Cells(3).Value <> "" Then
                                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                           "'" & .Cells(6).Value & "', ''"
                            End If
                        End If
                    End If
                End If
            End With
        Next
        Try
            Sql = "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) " & fastQuery & ";Update Settings Set MinState='N'"
            WABA.ExecNonQuery(Sql)
            MsgBox("Data Send to wahSoft Successfully...", vbInformation, "Sended On WahSoft")
        Catch ex As Exception
            MsgBox(ex.Message)
            UpdateProgressBarVisibility(False)
            WABA.CloseConnection()
        End Try
        UpdateProgressBarVisibility(False)
    End Sub
    Private Sub SendWhatsappData()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        UpdateProgressBarVisibility(True)
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim fastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                UpdateProgressBar(row.Index)
                If .Cells(0).Value = True Then
                    If btnRadioEnglish.Checked = True And .Cells(3).Value <> "" Then
                        If RadioPDFMsg.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale.rpt")
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                           "'" & .Cells(4).Value & "', '" & GlobalData.PdfPath & "'"
                        ElseIf RadioPdfOnly.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale.rpt")
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                                           "'', '" & GlobalData.PdfPath & "'"
                        ElseIf RadioMsgOnly.Checked = True Then
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                             "'" & .Cells(4).Value & "', ''"
                        End If
                    ElseIf RadioRegional.Checked = True And .Cells(3).Value <> "" Then
                        If RadioPDFMsg.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale2.rpt")
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                           "'" & .Cells(6).Value & "', '" & GlobalData.PdfPath & "'"
                        ElseIf RadioPdfOnly.Checked = True Then
                            retrive2(.Cells(1).Value)
                            GlobalData.PdfName = .Cells(2).Value & "-" & mskFromDate.Text & ".pdf"
                            PrintRecord()
                            Pdf_Genrate.ExportReport("\Formats\StandardSale2.rpt")
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                           "'', '" & GlobalData.PdfPath & "'"
                        ElseIf RadioMsgOnly.Checked = True Then
                            If .Cells(3).Value <> "" Then
                                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                           "'" & .Cells(6).Value & "', ''"
                            End If
                        End If
                    End If
                End If
            End With
        Next
        Try
            Sql = "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) " & fastQuery & ";Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecNonQuery(Sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        Catch ex As Exception
            MsgBox(ex.Message)
            UpdateProgressBarVisibility(False)
            ClsFunWhatsapp.CloseConnection()
        End Try
        UpdateProgressBarVisibility(False)
    End Sub
    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If cbType.SelectedIndex = 0 Then
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
            StartBackgroundTask(AddressOf SendWhatsappData)
        Else
            pnlWhatsapp.Visible = True
            Dim WhatsappFile As String = Application.StartupPath & "\WahSoft\WahSoft.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("WahSoft")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\WahSoft\WahSoft.exe"
                StartWhatsapp.Start()
            End If
            StartBackgroundTask(AddressOf WahSoft)
        End If
    End Sub
    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
            'MsgBox("A background task is running. you can Use your Task", MsgBoxStyle.Information, "Background Task")
        Else
            MsgBox("A background task is already running.", MsgBoxStyle.Information, "Background Task")
        End If
    End Sub
    Private Sub bgWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        isBackgroundWorkerRunning = True
        Dim action As Action = CType(e.Argument, Action)
        action.Invoke()
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        isBackgroundWorkerRunning = False
        pnlWhatsapp.Visible = False
    End Sub

    Private Sub ShowWhatsappContacts(Optional ByVal condtion As String = "")
        DgWhatsapp.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select VoucherID,AccountID,(Select BIllNo From Vouchers Where ID=VoucherID) as BillNo, (Select AccountName From Accounts Where ID=Transaction2.AccountID) as AccountName, ' Nug : '||sum(nug)||', Weight : '|| sum(weight) ||', Basic : '|| " &
                "sum(amount)  ||' Charges : '|| sum(Charges) ||' Total : '|| sum(TotalAmount) as Msg, " &
                "' नग : '||sum(nug)||', वज़न : '|| sum(weight) ||'बिक्री रकम : '|| sum(amount)  ||' ख़र्चे : '|| sum(Charges) ||' कुल रकम : '|| sum(TotalAmount) as Msg2, " &
                "(Select OtherName From Accounts Where ID=Transaction2.AccountID) as OtherName from Transaction2 " &
                " where AccountID<>7 and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " &
                " and TransType='" & Me.Text & "'" & condtion & " Group by VoucherID order by accountName "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                DgWhatsapp.Rows.Add()
                With DgWhatsapp.Rows(i)
                    .Cells(1).Value = dt.Rows(i)("VoucherID").ToString()
                    .Cells(2).Value = dt.Rows(i)("BillNo").ToString() & " - " & dt.Rows(i)("AccountName").ToString()
                    .Cells(3).Value = clsFun.ExecScalarStr("Select MObile1 From Accounts Where ID='" & Val(dt.Rows(i)("AccountId").ToString()) & "'")
                    .Cells(4).Value = "Dear " & dt.Rows(i)("AccountName").ToString() & "," & vbNewLine & " Todays Sale :" & dt.Rows(i)("Msg").ToString()
                    .Cells(5).Value = dt.Rows(i)("OtherName").ToString()
                    .Cells(6).Value = "पिर्य " & dt.Rows(i)("OtherName").ToString() & "," & vbNewLine & " आज की खरीद :" & dt.Rows(i)("Msg2").ToString()
                    .Cells(9).Value = dt.Rows(i)("AccountID").ToString()
                    .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                    .Cells(0).Value = True
                End With
            Next i
        End If
        DgWhatsapp.ClearSelection()
    End Sub

    Private Sub DgWhatsapp_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellEndEdit
        If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(9).Value & "'") = "" And DgWhatsapp.CurrentRow.Cells(3).Value <> "" Then
            clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(9).Value) & "'")
        Else
            If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(9).Value & "'") <> DgWhatsapp.CurrentRow.Cells(3).Value Then
                If MessageBox.Show("Are you Sure to Change Mobile No in PhoneBook", "Change Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(9).Value) & "'")
                End If
            End If
        End If
    End Sub
    Private Sub WhatsappCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        DgWhatsapp.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = WhatsappCheckBox.Checked
        Next
    End Sub

      Sub RowColumsWhatsapp()
        DgWhatsapp.Columns.Clear() : DgWhatsapp.ColumnCount = 9
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        WhatsappCheckBox.Location = New Point(headerCellLocation.X + 10, headerCellLocation.Y + 2)
        WhatsappCheckBox.BackColor = Color.GhostWhite
        WhatsappCheckBox.Size = New Size(18, 18)
        AddHandler WhatsappCheckBox.Click, AddressOf WhatsappCheckBox_Clicked
        DgWhatsapp.Controls.Add(WhatsappCheckBox)
        Dim checkBoxColumn1 As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn1.HeaderText = "" : checkBoxColumn1.Width = 30
        checkBoxColumn1.Name = "checkBoxColumn"
        DgWhatsapp.Columns.Insert(0, checkBoxColumn1)
        DgWhatsapp.Columns(0).ReadOnly = False
        AddHandler DgWhatsapp.CellContentClick, AddressOf DgWhatsapp_CellClick
        DgWhatsapp.Columns(1).Name = "ID" : DgWhatsapp.Columns(1).Visible = False
        DgWhatsapp.Columns(2).Name = "Account Name" : DgWhatsapp.Columns(2).Width = 150
        DgWhatsapp.Columns(3).Name = "Mobile No" : DgWhatsapp.Columns(3).Width = 100
        DgWhatsapp.Columns(4).Name = "message" : DgWhatsapp.Columns(4).Width = 350
        DgWhatsapp.Columns(5).Name = "Other Name" : DgWhatsapp.Columns(5).Visible = False
        DgWhatsapp.Columns(6).Name = "message2" : DgWhatsapp.Columns(6).Visible = False
        DgWhatsapp.Columns(7).Name = "Status" : DgWhatsapp.Columns(7).Width = 100
        DgWhatsapp.Columns(8).Name = "Path" : DgWhatsapp.Columns(8).Visible = False
        DgWhatsapp.Columns(9).Name = "AccountID" : DgWhatsapp.Columns(9).Visible = False
    End Sub
    Private Sub DgWhatsapp_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 1 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In DgWhatsapp.Rows
                If Convert.ToBoolean(row.Cells("chk").EditedFormattedValue) = False Then
                    isChecked = True
                    Exit For
                End If
            Next
            WhatsappCheckBox.Checked = isChecked
        End If
    End Sub
    Public Sub FillControl()
        Dim SendingMethod As String
        Dim LangugageType As String
        Dim MsgType As String
        Dim Sql As String = "Select * From API"
        Dim dt As New DataTable
        dt = ClsFunPrimary.ExecDataTable(Sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    SendingMethod = dt.Rows(i)("SendingMethod").ToString()
                    cbType.SelectedIndex = 0
                    If SendingMethod = "Easy WhatsApp" Then cbType.SelectedIndex = 0 Else cbType.SelectedIndex = 0 : cbType.Visible = True
                    LangugageType = dt.Rows(i)("LanguageType").ToString()
                    btnRadioEnglish.Checked = True
                    If LangugageType = "English" Then btnRadioEnglish.Checked = True Else RadioRegional.Checked = True
                    MsgType = dt.Rows(i)("SendingType").ToString()
                    RadioPDFMsg.Checked = True
                    If MsgType = "Pdf + Msg" Then RadioPDFMsg.Checked = True
                    If MsgType = "Pdf Only" Then RadioPdfOnly.Checked = True
                    If MsgType = "Msg Only" Then RadioMsgOnly.Checked = True

                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        'clsFun.CloseConnection()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        FillControl()
       If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
            RowColumsWhatsapp() : ShowWhatsappContacts()
            pnlWhatsapp.Visible = True
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
            cbType.SelectedIndex = 0 : Exit Sub
            'ElseIf ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "WahSoft" Then
            '    cbType.SelectedIndex = 1
        End If
        pnlWhatsapp.Visible = True
        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
        ShowWhatsappContacts()
    End Sub

    Private Sub btnPnlVisHide_Click(sender As Object, e As EventArgs) Handles btnPnlVisHide.Click
        pnlWhatsapp.Hide()
    End Sub

    Private Sub ckShowItems_CheckedChanged(sender As Object, e As EventArgs) Handles ckShowItems.CheckedChanged
        If ckShowItems.Checked = True Then
            dg1.Controls.Remove(headerCheckBox)
            lblitemSearch.Visible = True : txtItemSearch.Visible = True
            rowColums1() : retrive1()
            lblArea.Visible = False : txtItemSearch.BringToFront()
        Else
            dg1.Controls.Add(headerCheckBox)
            lblitemSearch.Visible = False : txtItemSearch.Visible = False
            rowColums() : retrive() : TempRowColumn()
            lblArea.Visible = False
        End If
    End Sub

    Private Sub txtItemSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemSearch.KeyPress, txtPrimarySearch.KeyPress
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyUp
        If dg1.RowCount = 0 Then Exit Sub
        If ckShowItems.Checked = True Then
            If txtItemSearch.Text.Trim() <> "" Then
                retrive1(" And upper(t.ItemName) Like upper('" & txtItemSearch.Text.Trim() & "%')")
            Else
                retrive1()
            End If
        End If
    End Sub

    Private Sub txtItemSearch_TextChanged(sender As Object, e As EventArgs) Handles txtItemSearch.TextChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        IDGentate() : PrintRecord()
        If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
        Report_Viewer.printReport("\Envlop.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        clsFun.FillDropDownList(cbArea, "SELECT A.ID,Area FROM Transaction2 t2 LEFT JOIN Accounts a ON t2.AccountID = a.ID WHERE t2.EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' GROUP BY a.Area;", "Area", "ID", "")
        pnlArea.Visible = True : cbArea.Focus()
    End Sub

    Private Sub btnArea_Click(sender As Object, e As EventArgs) Handles btnok.Click
        If ckShowItems.Checked = False Then
            If cbArea.Text <> "" Then
                retrive(" And upper(a.Area) Like upper('" & cbArea.Text.Trim() & "%')")
                lblArea.Text = cbArea.Text : lblArea.Visible = True : mskFromDate.Focus()
            Else
                retrive() : lblArea.Visible = False
            End If
        End If
        pnlArea.Visible = False
    End Sub

    Private Sub cbArea_KeyDown(sender As Object, e As KeyEventArgs) Handles cbArea.KeyDown
        If e.KeyCode = Keys.Enter Then btnok.Focus() : e.SuppressKeyPress = True
    End Sub
End Class