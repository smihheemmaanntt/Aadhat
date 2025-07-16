Public Class Account_Monthly_Summary

    Private Sub CustomerWiseSaleSummary_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub CustomerWiseSaleSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        rowColums()
    End Sub
   
    Private Sub rowColums()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Month Name" : dg1.Columns(1).Width = 500 : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Debit" : dg1.Columns(2).Width = 200 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).Name = "Credit" : dg1.Columns(3).Width = 200 : dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).Name = "Balance" : dg1.Columns(4).Width = 300 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        txtAccount.BackColor = Color.LightGray
        txtAccount.ForeColor = Color.Maroon
        DgAccountSearch.Visible = True : DgAccountSearch.BringToFront()
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        txtAccount.SelectAll()
    End Sub

    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown
        If e.KeyData = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then
                If DgAccountSearch.Visible = True Then DgAccountSearch.Focus() : Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
       
    End Sub
    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.Visible = True
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16,17)  or UnderGroupID in (11,16,17))" & condtion & " order by AccountName Limit 11")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("City").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click, btnShow.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : AddMonths()
    End Sub
    Private Sub AddMonths()
        dg1.Rows.Clear() : Dim calculatedBalance As Decimal = 0
        Dim debittotal As Decimal : Dim CreditTotal As Decimal
        Dim Opbal As String = clsFun.ExecScalarStr("Select Opbal   From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
        Dim DC As String = clsFun.ExecScalarStr("Select DC From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
        txtOpbal.Text = Format(Val(Opbal), "0.00") & " " & DC
        Dim monthNos As String() = {"04", "05", "06", "07", "08", "09", "10", "11", "12", "01", "02", "03"}
        Dim monthNames As String() = {"April", "May", "June", "July", "August", "September", "October", "November", "December", "January", "February", "March"}
        For i As Integer = 0 To monthNos.Length - 1
            Dim debit As String = clsFun.ExecScalarStr("SELECT SUM(Amount) FROM Ledger WHERE AccountID='" & Val(txtAccountID.Text) & "' AND DC='D' AND strftime('%m', EntryDate) = '" & monthNos(i) & "'")
            Dim credit As String = clsFun.ExecScalarStr("SELECT SUM(Amount) FROM Ledger WHERE AccountID='" & Val(txtAccountID.Text) & "' AND DC='C' AND strftime('%m', EntryDate) = '" & monthNos(i) & "'")
            If i = 0 Then
                calculatedBalance = IIf(DC = "Dr", Val(Opbal) + Val(debit) - Val(credit), -Val(Val(Opbal) + Val(credit)) + Val(debit))
            Else
                calculatedBalance = Val(Val(calculatedBalance) + Val(debit)) - Val(credit)
            End If
            dg1.Rows.Add(0, monthNames(i), If(debit = "", "", Format(Val(debit), "0.00")), If(credit = "", "", Format(Val(credit), "0.00")), If(calculatedBalance >= 0, Format(Math.Abs(Val(calculatedBalance)), "0.00") & " Dr", Format(Math.Abs(Val(calculatedBalance)), "0.00") & " Cr"))
            debittotal = Val(debittotal) + Val(debit)
            CreditTotal = Val(CreditTotal) + Val(credit)
        Next
        txtDebitAmt.Text = Format(Val(debittotal), "0.00")
        txtCreditAmt.Text = Format(Val(CreditTotal), "0.00")
        txtClosingbal.Text = If(calculatedBalance < 0, Format(Math.Abs(Val(calculatedBalance)), "0.00") & " Cr", Format(Math.Abs(Val(calculatedBalance)), "0.00") & " Dr")
        dg1.ClearSelection()
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        PrintRecord()
        Report_Viewer.printReport("\Reports\AccountMonthlySummary.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
    End Sub
    Private Sub PrintRecord()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            Application.DoEvents()
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With dg1.Rows(LastRecord)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & txtAccount.Text & "', " & _
                        "'" & CDate(FinYearStart).ToString("yyyy") & " - " & CDate(FinYearEnd).ToString("yyyy") & "','" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "', " & _
                      "'" & .Cells(4).Value & "','" & txtOpbal.Text & "', '" & Format(Val(txtDebitAmt.Text), "0.00") & "','" & Format(Val(txtCreditAmt.Text), "0.00") & "','" & txtClosingbal.Text & "'"
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            ' LastRecord = LastCount
            Try
                If FastQuery = String.Empty Then Exit Sub
                sql = "insert into Printing(M1,M2,P1,P2,P3, P4,P5,P6,P7,P8) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sql)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try

        Next

    End Sub

    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "AccountID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 300
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 190
        ' retriveAccounts()
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        '  AccountRowColumns()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.PageUp Then
            If DgAccountSearch.CurrentRow.Index = 0 Then txtAccount.Focus()
        End If
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            Dim AccountID As String = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(AccountID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            btnShow.Focus() : DgAccountSearch.Visible = False
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
        If e.KeyCode = Keys.Up Then
            If DgAccountSearch.CurrentRow.Index = 0 Then
                txtAccount.Focus()
            End If
        End If
    End Sub
End Class