Public Class Group_Summary
    Dim RecordDate As String
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub Trail_Balance_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Trail_Balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
        End Select
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name"
        dg1.Columns(1).Width = 500
        dg1.Columns(2).Name = "Group Name"
        dg1.Columns(2).Width = 250
        dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Debit"
        dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "Credit"
        dg1.Columns(5).Width = 200
        ' retrive()
    End Sub

    Public Sub retrive(ByVal ID As String, Optional ByVal Date1 As String = "")
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        Dim i As Integer
        Dim amt As Decimal = 0
        Dim opbaltot As Decimal = 0.0
        Dim drtot As Decimal = 0.0
        Dim crtot As Decimal = 0.0
        Dim lastval As Integer = 0
        Dim tmpval As Integer = 0
        RecordDate = Date1
        ssql = "Select ID,Accountname,GroupName From Account_AcGrp Where   GroupID in (" & ID & ")  Group By ID   ORDER BY AccountName"
        dt = clsFun.ExecDataTable(ssql)
        dg1.Rows.Clear()
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                Dim opbal As String = "" : Dim ClBal As String = ""
                opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(i)("ID").ToString()) & "")
                Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(Date1).ToString("yyyy-MM-dd") & "'")
                Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(Date1).ToString("yyyy-MM-dd") & "'")
                Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(i)("ID").ToString()) & "")
                If drcr = "Dr" Then
                    tmpamtdr = Val(opbal) + Val(tmpamtdr)
                Else
                    tmpamtcr = Val(opbal) + Val(tmpamtcr)
                End If
                Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr) '- Val(opbal)
                If tmpamt > 0 Then
                    dg1.Rows.Add()
                    With dg1.Rows(tmpval)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("GroupName").ToString()
                        .Cells(4).Value = Format(Math.Abs(Val(tmpamt)), "0.00")
                        drtot = drtot + .Cells(4).Value
                        tmpval = tmpval + 1
                    End With
                ElseIf tmpamt < 0 Then
                    dg1.Rows.Add()
                    With dg1.Rows(tmpval)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("GroupName").ToString()
                        .Cells(5).Value = Format(Math.Abs(Val(tmpamt)), "0.00")
                        crtot = crtot + .Cells(5).Value
                        tmpval = tmpval + 1
                    End With
                End If
            Next
        End If


        If dg1.RowCount = 0 Then calc() : Exit Sub
        'If crtot > drtot Then
        '    dg1.Rows.Add()
        '    dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
        '    dg1.Rows(tmpval).Cells(4).Value = Format(Val(crtot - drtot), "0.00")
        'ElseIf crtot < drtot Then
        '    dg1.Rows.Add()
        '    dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
        '    dg1.Rows(tmpval).Cells(5).Value = Format(Val(drtot - crtot), "0.00")
        'End If
        calc() : dg1.ClearSelection()

    End Sub
    Private Sub calc()
        txtDramt.Text = Format(0, "00000.00") : txtcrAmt.Text = Format(0, "00000.00")
        For i = 0 To dg1.Rows.Count - 1
            txtDramt.Text = Format(Val(txtDramt.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtcrAmt.Text = Format(Val(txtcrAmt.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
        Next
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Ledger.MdiParent = MainScreenForm
        Ledger.Show()
        Ledger.cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(0).Value)
        Ledger.BringToFront()
        Ledger.mskFromDate.Text = clsFun.convdate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
        Ledger.btnShow.PerformClick()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Ledger.MdiParent = MainScreenForm
            Ledger.Show()
            Ledger.cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(0).Value)
            Ledger.BringToFront()
            Ledger.mskFromDate.Text = clsFun.convdate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
            Ledger.btnShow.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(D1,P1, P2,P3, P4, P5,P6,P7) values(" & _
                    "" & RecordDate & ",'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                   "'" & .Cells(5).Value & "' ,'" & Format(Val(txtDramt.Text), "0.00") & "','" & Format(Val(txtcrAmt.Text), "0.00") & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        PrintRecord()
        Report_Viewer.printReport("\Reports\GroupSummary.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
End Class