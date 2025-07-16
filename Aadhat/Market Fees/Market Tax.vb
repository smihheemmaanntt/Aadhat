Public Class Market_Tax

    Private Sub Market_Tax_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Market_Tax_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) as entrydate from transaction2 ")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from transaction2 ")
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
        rowColums() : RadioDefault.Checked = True
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date"
        dg1.Columns(1).Width = 150
        If RadioHideItems.Checked = True Then
            dg1.Columns(2).Name = "Item Name"
        Else
            dg1.Columns(2).Name = "Account Name"
        End If
        dg1.Columns(2).Width = 400
        dg1.Columns(3).Name = "Nug"
        dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Weight"
        dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "Market Fees"
        dg1.Columns(5).Width = 200
    End Sub

    Private Sub rowColums2()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date"
        dg1.Columns(1).Width = 150
        dg1.Columns(2).Name = "Basic Amount"
        dg1.Columns(2).Width = 400
        dg1.Columns(3).Name = "Nug"
        dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Weight"
        dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "Market Fees"
        dg1.Columns(5).Width = 200
    End Sub

    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        TxtMFeesTotal.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            TxtMFeesTotal.Text = Format(Val(TxtMFeesTotal.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
        Next
    End Sub
    Private Sub retrive2()
        rowColums2()
        Dim dt As New DataTable
        'dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE (((DateValue([EntryDate])=DateValue('" & mskEntryDate.Text & "')))) and transtype='" & Me.Text & "'")
        dt = clsFun.ExecDataTable("Select Transaction2.EntryDate, Transaction2.ItemName, Sum(Transaction2.Nug) AS nugs, Sum(Transaction2.Weight) AS weights,Sum(Transaction2.Amount) AS Amount,Sum(Transaction2.MAmt) AS Mamount " & _
                                  "FROM Transaction2 where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' GROUP BY Transaction2.EntryDate;")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        '.Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(3).Value = Format(Val(dt.Rows(i)("Nugs").ToString()), "0.00")
                        .Cells(4).Value = Format(Val(dt.Rows(i)("Weights").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("MAmount").ToString()), "0.00")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub retrive()
        Dim dt As New DataTable
        If RadioHideItems.Checked Then
            dt = clsFun.ExecDataTable("Select Transaction2.EntryDate, Transaction2.ItemName, Sum(Transaction2.Nug) AS nugs, Sum(Transaction2.Weight) AS weights,Sum(Transaction2.MAmt) AS Mamount " & _
                            "FROM Transaction2 where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' GROUP BY Transaction2.EntryDate, Transaction2.ItemName ;")
        Else
            dt = clsFun.ExecDataTable("Select Transaction2.EntryDate, Transaction2.AccountName, Sum(Transaction2.Nug) AS nugs, Sum(Transaction2.Weight) AS weights,Sum(Transaction2.MAmt) AS Mamount " & _
                            "FROM Transaction2 where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' GROUP BY Transaction2.EntryDate, Transaction2.AccountName ;")
        End If
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        '.Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        If RadioHideItems.Checked = True Then .Cells(2).Value = dt.Rows(i)("ItemName").ToString() Else .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(3).Value = Format(Val(dt.Rows(i)("Nugs").ToString()), "0.00")
                        .Cells(4).Value = Format(Val(dt.Rows(i)("Weights").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("MAmount").ToString()), "0.00")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If RadioDefault.Checked = False Then
            rowColums() : retrive()
        Else
            rowColums2() : retrive2()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub MsktoDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MsktoDate.MaskInputRejected

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "'," & _
                    "'" & txtTotNug.Text & "','" & txtTotweight.Text & "','" & TxtMFeesTotal.Text & "');"
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
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\MarketFeeReport.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles Dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles Dtp2.ValueChanged
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
End Class