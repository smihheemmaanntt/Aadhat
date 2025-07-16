Imports System.Runtime.InteropServices

Public Class CrateWiseOutstanding

    Private Sub Speed_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectAll()
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub Crate_Summary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String
        mindate = clsFun.ExecScalarStr("Select max(EntryDate) as entrydate from CrateVoucher ")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name" : dg1.Columns(1).Width = 250
        dg1.Columns(2).Name = "Other Name" : dg1.Columns(2).Width = 250
        dg1.Columns(3).Name = "Area" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Mobile" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = " Crate Name" : dg1.Columns(5).Width = 200
        dg1.Columns(6).Name = "Crate Balance" : dg1.Columns(6).Width = 150
        dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable

    End Sub
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr
    End Function
    Private Sub retrive(Optional ByVal condtion As String = "")
        SendMessage(pb1.Handle, 1040, 3, 0)
        dg1.Rows.Clear()
        Dim lastval As Integer = 0
        Dim lastval1 As Integer = 0
        Dim ClosingCrate As Integer = 0
        Dim totalOpOutCrate As Integer = 0
        Dim totalOpInCrate As Integer = 0
        Dim oldbal As Decimal = 0.00
        'dt = clsFun.ExecDataTable("Select AccountID,AccountName,(Select OtherName From Accounts Where ID=CrateVoucher.AccountID) as OtherName,(Select Area From Accounts Where ID=CrateVoucher.AccountID) as area,(Select Mobile1 From Accounts Where ID=CrateVoucher.AccountID) as Mobile,CrateID,CrateName FROM CrateVoucher Where EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & " and AccountID in(859,860,863) Group by CrateName,AccountID   order by AccountID ")
        dt = clsFun.ExecDataTable("Select AccountID,AccountName,(Select OtherName From Accounts Where ID=CrateVoucher.AccountID) as OtherName,(Select Area From Accounts Where ID=CrateVoucher.AccountID) as area,(Select Mobile1 From Accounts Where ID=CrateVoucher.AccountID) as Mobile,CrateID,CrateName FROM CrateVoucher Where EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & "  Group by CrateName,AccountID   order by Upper(AccountName) ")
        Dim vchid As Integer = 0
        Dim tmpamt1 As Integer = 0
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    ' If dt.Rows(i)("AccountID").ToString() = 1045 Then MsgBox("a")
                    If Application.OpenForms().OfType(Of CrateWiseOutstanding).Any = False Then Exit Sub
                    Application.DoEvents()
                    pb1.Minimum = 0
                    pb1.Maximum = dt.Rows.Count - 1
                    pb1.Value = i
                    Dim tmpamtdr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(vchid) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(vchid) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
                    tmpamt1 = Val(tmpamtdr1) - Val(tmpamtcr1) '- Val(opbal)

                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
                    '  If i = 4 Then MsgBox("A")
                    If Val(tmpamtcr) >= Val(tmpamtdr) Then
                        If Val(vchid) <> Val(dt.Rows(i)("AccountID").ToString()) Then ClosingCrate = IIf(totalOpOutCrate = 0, ClosingCrate, totalOpOutCrate) : totalOpOutCrate = 0
                        totalOpOutCrate = totalOpOutCrate + tmpamt
                        oldbal = tmpamt
                        opbal = Math.Abs(Val(tmpamt)) & " Out"
                    Else
                        If Val(vchid) <> Val(dt.Rows(i)("AccountID").ToString()) Then ClosingCrate = IIf(totalOpOutCrate = 0, ClosingCrate, totalOpOutCrate) : totalOpOutCrate = 0
                        oldbal = tmpamt
                        totalOpInCrate = totalOpInCrate + tmpamt
                        opbal = Math.Abs(Val(tmpamt)) & " In"
                    End If
                    ' End If
                    If oldbal <> 0 Then
                        If Application.OpenForms().OfType(Of CrateWiseOutstanding).Any = False Then Exit Sub
                        If vchid <> dt.Rows(i)("AccountID").ToString() And vchid > 0 Then
                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                dg1.Rows(lastval).Cells(1).Style.BackColor = Color.Green
                                dg1.Rows(lastval).Cells(2).Style.BackColor = Color.Green
                                dg1.Rows(lastval).Cells(3).Style.BackColor = Color.Green
                                dg1.Rows(lastval).Cells(4).Style.BackColor = Color.Green
                                dg1.Rows(lastval).Cells(5).Style.BackColor = Color.Green
                                dg1.Rows(lastval).Cells(6).Style.BackColor = Color.Green
                                dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.GhostWhite
                                dg1.Rows(lastval).Cells(5).Style.ForeColor = Color.GhostWhite
                                dg1.Rows(lastval).Cells(6).Style.ForeColor = Color.GhostWhite
                                .Cells(0).Value = vchid
                                .Cells(1).Value = "Total Crates"
                                .Cells(5).Value = "All Marka"
                                '  .Cells(6).Value = IIf(tmpamt1 > 0, tmpamt1 & " In", Math.Abs(tmpamt1) & " Out")
                                If oldbal = 0 Then
                                    .Cells(6).Value = IIf(tmpamt1 > 0, tmpamt1 & " In", Math.Abs(tmpamt1) & " Out")
                                Else
                                    .Cells(6).Value = IIf(ClosingCrate > 0, ClosingCrate & " Out", Math.Abs(ClosingCrate) & " In")
                                    'totalOpOutCrate = 0
                                End If
                                lastval = lastval + 1
                            End With
                        End If
                        dg1.Rows.Add()
                        With dg1.Rows(lastval)
                            .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                            dg1.ClearSelection() : Application.DoEvents()
                            .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                            If lastval = 0 Then
                                .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                                .Cells(2).Value = dt.Rows(i)("OtherName").ToString()
                                .Cells(3).Value = dt.Rows(i)("Area").ToString()
                                .Cells(4).Value = dt.Rows(i)("MObile").ToString()
                            ElseIf vchid = dt.Rows(i)("AccountID").ToString() Then
                                .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                            Else
                                .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                                .Cells(2).Value = dt.Rows(i)("OtherName").ToString()
                                .Cells(3).Value = dt.Rows(i)("Area").ToString()
                                .Cells(4).Value = dt.Rows(i)("MObile").ToString()
                            End If
                            .Cells(5).Value = dt.Rows(i)("CrateName").ToString()
                            '.Cells(6).Value = opbal
                            If oldbal = 0 Then
                                .Cells(6).Value = IIf(tmpamt1 > 0, tmpamt1 & " In", Math.Abs(tmpamt1) & " Out")
                            Else
                                .Cells(6).Value = opbal
                            End If
                            vchid = dt.Rows(i)("AccountID").ToString()
                        End With

                        vchid = dt.Rows(i)("AccountID").ToString()
                        lastval = lastval + 1
                    End If

                Next
                vchid1 = vchid
            End If
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(lastval).Cells(1).Style.BackColor = Color.Green
                dg1.Rows(lastval).Cells(2).Style.BackColor = Color.Green
                dg1.Rows(lastval).Cells(3).Style.BackColor = Color.Green
                dg1.Rows(lastval).Cells(4).Style.BackColor = Color.Green
                dg1.Rows(lastval).Cells(5).Style.BackColor = Color.Green
                dg1.Rows(lastval).Cells(6).Style.BackColor = Color.Green
                dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(5).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(6).Style.ForeColor = Color.GhostWhite
                .Cells(0).Value = vchid
                .Cells(1).Value = "Total Crates"
                .Cells(5).Value = "All Marka"
                If oldbal = 0 Then
                    .Cells(6).Value = IIf(tmpamt1 > 0, tmpamt1 & " In", Math.Abs(tmpamt1) & " Out")
                Else
                    .Cells(6).Value = IIf(totalOpOutCrate > 0, totalOpOutCrate & " Out", Math.Abs(totalOpOutCrate) & " In")
                End If

                lastval = lastval + 1
            End With
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        dg1.ClearSelection()
        'calc()
    End Sub

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
                sql = "insert into Printing(M1,D1, P1, P2,P3, P4, P5,P6) values('" & .Cells(0).Value & "','" & mskFromDate.Text & "'," &
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & Format(Val(.Cells(4).Value), "0.00") & "'," &
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "')"
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
    Private Sub PrintRecord2()
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
                sql = "insert into Printing(M1,D1, P1, P2,P3, P4, P5,P6) values('" & .Cells(0).Value & "','" & mskFromDate.Text & "'," &
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & Format(Val(.Cells(4).Value), "0.00") & "'," &
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "')"
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
        Report_Viewer.printReport("\Reports\CrateWiseOutstanding.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtCustomerSearch.Text.Trim() <> "" Then
                SearchText = " and AccountName Like '" & txtCustomerSearch.Text.Trim() & "%'"
                Offset = 0
                pnlWait.Visible = True
                retrive(SearchText)
                pnlWait.Visible = False
            End If
            If txtCustomerSearch.Text.Trim() = "" Then
                SearchText = ""
                Offset = 0
                pnlWait.Visible = True
                retrive()
                pnlWait.Visible = False
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub

    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtItemSearch.Text.Trim() <> "" Then
                SearchText = " and CrateName Like '" & txtItemSearch.Text.Trim() & "%'"
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
        Report_Viewer.printReport("\Reports\CrateWiseOutstanding2.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub



    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        PrintRecord2()
    End Sub

    Private Sub ReportViewer1_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtArea_KeyUp(sender As Object, e As KeyEventArgs) Handles txtArea.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtArea.Text.Trim() <> "" Then
                SearchText = " and Area Like '" & txtArea.Text.Trim() & "%'"
                Offset = 0
                retrive(SearchText)
            End If
            If txtArea.Text.Trim() = "" Then
                SearchText = ""
                Offset = 0
                retrive()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtArea.TextChanged

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class