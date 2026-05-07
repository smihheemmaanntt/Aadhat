Imports System.Runtime.InteropServices

Public Class Crate_Summary
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub Speed_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub txtFromDate_GotFocus(sender As Object, e As EventArgs) Handles txtFromDate.GotFocus
        txtFromDate.SelectAll()
    End Sub

    Private Sub txtToDate_GotFocus(sender As Object, e As EventArgs) Handles txttoDate.GotFocus
        txttoDate.SelectAll()
    End Sub
    Private Sub txtFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtFromDate.Validating
        txtFromDate.Text = SmartDate(txtFromDate.Text)
    End Sub
    Private Sub txtFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFromDate.KeyDown, txttoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If

    End Sub
    Private Sub txtToDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txttoDate.Validating
        txttoDate.Text = SmartDate(txttoDate.Text, True, 2)
    End Sub
    Private Sub Crate_Summary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select max(EntryDate) as entrydate from CrateVoucher ")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from CrateVoucher ")
        If mindate <> "" Then
            txtFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            txtFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            txtToDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            txtToDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 8
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account / Crate Name" : dg1.Columns(1).Width = 380
        dg1.Columns(2).Name = " Crate Name" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Op. Qty" : dg1.Columns(3).Width = 160
        dg1.Columns(4).Name = "Crate Out" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Crate In" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "OtherName" : dg1.Columns(6).Visible = False
        dg1.Columns(7).Name = "Crate Balance" : dg1.Columns(7).Width = 150
    End Sub
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr
    End Function
    Private Sub retrive(Optional ByVal condtion As String = "")
        Application.DoEvents()
        SendMessage(pb1.Handle, 1040, 3, 0)
        dg1.Rows.Clear()

        Dim oldbal As Integer = 0
        Dim totalOpInCrate As Integer = 0
        Dim totalOpOutCrate As Integer = 0
        Dim totalINCrate As Integer = 0
        Dim totalOutCrate As Integer = 0
        Dim lastval As Integer = 0

        Dim fromDate As String = CDate(txtFromDate.Text).ToString("yyyy-MM-dd")
        Dim toDate As String = CDate(txtToDate.Text).ToString("yyyy-MM-dd")

        dt = clsFun.ExecDataTable("Select AccountID,AccountName,CrateID,CrateName FROM CrateVoucher " & condtion & " Group by CrateName,AccountID order by AccountID")

        Dim vchid As Integer = 0

        Try
            If dt.Rows.Count > 0 Then

                dg1.SuspendLayout()

                pb1.Minimum = 0
                pb1.Maximum = dt.Rows.Count - 1

                For i = 0 To dt.Rows.Count - 1

                    If i Mod 50 = 0 Then
                        pb1.Value = i
                        Application.DoEvents()
                    End If

                    Dim accId As Integer = Val(dt.Rows(i)("AccountID"))
                    Dim crateId As Integer = Val(dt.Rows(i)("CrateID"))

                    '========================
                    ' 🔥 CACHE BASED QUERY
                    '========================
                    Dim tmpamtdr As Decimal = Val(clsFun.ExecScalarStr("Select sum(Qty) from CrateVoucher where CrateType='Crate In' and AccountID=" & accId & " and CrateID=" & crateId & " and EntryDate < '" & fromDate & "'"))
                    Dim tmpamtcr As Decimal = Val(clsFun.ExecScalarStr("Select sum(Qty) from CrateVoucher where CrateType='Crate Out' and AccountID=" & accId & " and CrateID=" & crateId & " and EntryDate < '" & fromDate & "'"))

                    Dim drcr As String = clsFun.ExecScalarStr("Select CrateType from CrateVoucher where AccountID=" & accId & " limit 1")

                    Dim tmpamt As Decimal
                    If tmpamtdr > tmpamtcr Then
                        tmpamt = tmpamtdr - tmpamtcr
                    Else
                        tmpamt = tmpamtcr - tmpamtdr
                    End If

                    Dim opbal As String = ""

                    If tmpamtcr > tmpamtdr Then
                        totalOpInCrate += tmpamt
                        oldbal = tmpamt
                        opbal = tmpamt & " Out"
                    Else
                        totalOpInCrate += tmpamt
                        oldbal = tmpamt
                        opbal = tmpamt & " In"
                    End If

                    '========================
                    ' 🔥 GRID ROW
                    '========================
                    dg1.Rows.Add()

                    With dg1.Rows(i)
                        .Cells(0).Value = accId
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(6).Value = clsFun.ExecScalarStr("Select OtherName from Accounts where ID=" & accId)
                        .Cells(2).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(3).Value = opbal
                        ' Period Out
                        Dim outQty As Decimal = Val(clsFun.ExecScalarStr("Select sum(Qty) from CrateVoucher where CrateType='Crate Out' and AccountID=" & accId & " and CrateID=" & crateId & " and EntryDate between '" & fromDate & "' and '" & toDate & "'"))
                        .Cells(4).Value = outQty
                        totalOutCrate += outQty
                        ' Period In
                        Dim inQty As Decimal = Val(clsFun.ExecScalarStr("Select sum(Qty) from CrateVoucher where CrateType='Crate In' and AccountID=" & accId & " and CrateID=" & crateId & " and EntryDate between '" & fromDate & "' and '" & toDate & "'"))
                        .Cells(5).Value = inQty
                        totalINCrate += inQty
                        .Cells(7).Value = (Val(oldbal) + outQty) - inQty

                    End With

                    vchid = accId
                    lastval = i + 1

                Next

                dg1.ResumeLayout()
            End If

            '========================
            ' 🔥 TOTAL ROW
            '========================
            dg1.Rows.Add()

            With dg1.Rows(lastval)
                .Cells(1).Value = "Total Crates"
                .Cells(2).Value = "All Marka"
                .Cells(3).Value = totalOpInCrate - totalOpOutCrate
                .Cells(4).Value = totalOutCrate
                .Cells(5).Value = totalINCrate
            End With
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        pb1.Value = 0
        dg1.ClearSelection()

    End Sub
    ''Private Sub retrive(Optional ByVal condtion As String = "")
    ''    SendMessage(pb1.Handle, 1040, 3, 0)
    ''    dg1.Rows.Clear()
    ''    Dim oldbal As Integer = 0
    ''    Dim totalOpInCrate As Integer = 0
    ''    Dim totalOpOutCrate As Integer = 0
    ''    Dim totalINCrate As Integer = 0
    ''    Dim totalOutCrate As Integer = 0
    ''    Dim lastval As Integer = 0
    ''    ' dt = clsFun.ExecDataTable("Select AccountID,AccountName,CrateID,CrateName FROM and AccontID in(860,863) CrateVoucher  " & condtion & " Group by CrateName,AccountID  order by AccountID ")
    ''    dt = clsFun.ExecDataTable("Select AccountID,AccountName,CrateID,CrateName FROM CrateVoucher  " & condtion & " Group by CrateName,AccountID  order by AccountID ")
    ''    Dim vchid As Integer = 0
    ''    Try
    ''        If dt.Rows.Count > 0 Then
    ''            dg1.Rows.Clear()
    ''            For i = 0 To dt.Rows.Count - 1
    ''                If Application.OpenForms().OfType(Of Crate_Summary).Any = False Then Exit Sub
    ''                Application.DoEvents()
    ''                pb1.Minimum = 0
    ''                pb1.Maximum = dt.Rows.Count - 1
    ''                pb1.Value = i
    ''                Dim ssql As String = String.Empty
    ''                Dim dr As Decimal = 0 : Dim cr As Decimal = 0 : Dim tot As Decimal = 0
    ''                Dim opbal As String = "" : opbal = Val(0)
    ''                ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(dt.Rows(i)("AccountID").ToString()) > 0, "and AccountID=" & Val(dt.Rows(i)("AccountID").ToString()) & "", "") & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate Between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(txtToDate.Text).ToString("yyyy-MM-dd") & "'    union all" & _
    ''                       " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(dt.Rows(i)("AccountID").ToString()) > 0, "and AccountID=" & Val(dt.Rows(i)("AccountID").ToString()) & "", "") & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate Between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(txtToDate.Text).ToString("yyyy-MM-dd") & "'    "
    ''                Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "'")
    ''                Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "'")
    ''                Dim drcr As String = clsFun.ExecScalarStr(" Select CrateType FROM CrateVoucher WHERE AccountID= " & Val(dt.Rows(i)("AccountID").ToString()) & "")
    ''                If drcr = "Crate In" Then
    ''                    tmpamtdr = Val(opbal) + Val(tmpamtdr)
    ''                Else
    ''                    tmpamtcr = Val(opbal) + Val(tmpamtcr)
    ''                End If
    ''                Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
    ''                If drcr = "Crate Out" Then
    ''                    opbal = -Val(opbal)
    ''                End If
    ''                opbal = tmpamt
    ''                Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from CrateVoucher where  accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and  EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "'")
    ''                If cnt = 0 Then
    ''                    opbal = Math.Abs(Val(opbal)) & " " & clsFun.ExecScalarStr(" Select CrateType   FROM CrateVoucher  WHERE ID= " & Val(dt.Rows(i)("AccountID").ToString()) & "")

    ''                    If Math.Abs(Val(opbal)) = 0 Then opbal = 0 & " Out"

    ''                Else
    ''                    If Val(tmpamtcr) > Val(tmpamtdr) Then
    ''                        totalOpInCrate = totalOpInCrate + opbal
    ''                        oldbal = opbal
    ''                        opbal = Math.Abs(Val(opbal)) & " Out"
    ''                    Else
    ''                        oldbal = opbal
    ''                        totalOpInCrate = totalOpInCrate + opbal
    ''                        opbal = Math.Abs(Val(opbal)) & " In"
    ''                    End If
    ''                End If
    ''                If Application.OpenForms().OfType(Of Crate_Summary).Any = False Then Exit Sub
    ''                dg1.Rows.Add()
    ''                With dg1.Rows(i)
    ''                    Application.DoEvents()
    ''                    .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
    ''                    If i = 0 Then
    ''                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
    ''                        .Cells(6).Value = clsFun.ExecScalarStr(" Select OtherName FROM Accounts WHERE ID= " & Val(dt.Rows(i)("AccountID").ToString()) & "")
    ''                    Else
    ''                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
    ''                        .Cells(6).Value = clsFun.ExecScalarStr(" Select OtherName FROM Accounts WHERE ID= " & Val(dt.Rows(i)("AccountID").ToString()) & "")
    ''                    End If
    ''                    .Cells(2).Value = dt.Rows(i)("CrateName").ToString()
    ''                    .Cells(3).Value = opbal
    ''                    drcr = opbal.Substring(opbal.Length - 3)
    ''                    .Cells(4).Value = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate Between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txtToDate.Text).ToString("yyyy-MM-dd") & "'")
    ''                    totalOutCrate = Val(totalOutCrate) + Val(.Cells(4).Value)
    ''                    .Cells(5).Value = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate Between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txtToDate.Text).ToString("yyyy-MM-dd") & "'")
    ''                    totalINCrate = Val(totalINCrate) + Val(.Cells(5).Value)
    ''                    .Cells(7).Value = Val(Val(oldbal) + Val(.Cells(4).Value)) - Val(.Cells(5).Value)
    ''                End With
    ''                vchid = dt.Rows(i)("AccountID").ToString()
    ''                lastval = i + 1
    ''            Next
    ''        End If
    ''        dt.Dispose()
    ''        '  lastval = lastval + 1
    ''        dg1.Rows.Add()
    ''        With dg1.Rows(lastval)
    ''            .Cells(1).Value = "Total Crates"
    ''            .Cells(2).Value = "All Marka"
    ''            .Cells(3).Value = Val(totalOpInCrate - totalOpOutCrate)
    ''            .Cells(4).Value = Val(totalOutCrate)
    ''            .Cells(5).Value = Val(totalINCrate)
    ''        End With
    ''    Catch ex As Exception
    ''        MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
    ''    End Try
    ''    dg1.ClearSelection()
    ''    'calc()
    ''End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        pnlWait.Visible = True
        retrive()
        pnlWait.Visible = False
    End Sub
    Private Sub PrintRecord()
        pnlWait.Visible = True
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            pb1.Minimum = 0
            pb1.Maximum = dg1.RowCount
            Application.DoEvents()
            With row
                pb1.Value = IIf(Val(row.Index) < 0, 0, Val(row.Index))
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5,P6,P7) values('" & txtFromDate.Text & "','" & txtToDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & Format(Val(.Cells(4).Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells(5).Value), "0.00") & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
        pnlWait.Visible = False
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        PrintRecord()
        Report_Viewer.printReport("\Reports\CrateSummary.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtCustomerSearch.Text.Trim() <> "" Then
                SearchText = " Where AccountName Like '" & txtCustomerSearch.Text.Trim() & "%'"
                Offset = 0
                retrive(SearchText)
            End If
            If txtCustomerSearch.Text.Trim() = "" Then
                SearchText = ""
                Offset = 0
                retrive()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub

    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtItemSearch.Text.Trim() <> "" Then
                SearchText = " Where CrateName Like '" & txtItemSearch.Text.Trim() & "%'"
                Offset = 0
                retrive(SearchText)
            End If
            If txtItemSearch.Text.Trim() = "" Then
                SearchText = ""
                Offset = 0
                retrive()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        PrintRecord()
        Report_Viewer.printReport("\Reports\CrateSummary2.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        txtToDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        txtToDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        txtToDate.Text = smartDate(txtToDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        txtFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        txtFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        txtFromDate.Text = smartDate(txtFromDate.Text)
    End Sub

    Private Sub txtFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs)

    End Sub
End Class