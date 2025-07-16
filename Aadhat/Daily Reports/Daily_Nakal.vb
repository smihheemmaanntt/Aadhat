Public Class Daily_Nakal

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub CustomerWiseSaleSummary_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub CustomerWiseSaleSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) from transaction2 where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) from transaction2 where transtype='" & Me.Text & "'")
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
        rowColums()
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Expenses Name" : dg1.Columns(1).Width = 400 : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Total" : dg1.Columns(2).Width = 180 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 400 : dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).Name = "Total" : dg1.Columns(4).Width = 180 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Private Sub retrive()
        dg1.Rows.Clear()
        ' Get Expense Data
        Dim dtExp As New DataTable
        dtExp = clsFun.ExecDataTable("Select sum(CommAmt) as Commission, sum(MAmt) as MandiTax, sum(RdfAmt) as RDF, sum(TareAmt) as Bardana, sum(LabourAmt) as Labour, sum(Charges) as Charges  from Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary)

        ' Get Account Data
        Dim dtAcc As New DataTable
        dtAcc = clsFun.ExecDataTable("Select AccountName, sum(TotalAmount) as TotalAmount from Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary & " Group By AccountName order by AccountName")

        Try
            Dim maxRows As Integer = Math.Max(dtExp.Rows.Count, dtAcc.Rows.Count) ' Find maximum rows to iterate

            For i As Integer = 0 To maxRows - 1
                dg1.Rows.Add()
                ' Add Expense Data in Column 1 and 2
                If i = 0 Then dg1.Rows(i).Cells(1).Value = "Customers Expenses ----------------" : dg1.Rows(i).Cells(2).Value = "----------------"
                If dtExp.Rows.Count > 0 Then
                    Select Case i
                        Case 1 : dg1.Rows(i).Cells(1).Value = "Commission" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Commission").ToString()), "0.00")
                        Case 2 : dg1.Rows(i).Cells(1).Value = "Mandi Tax" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("MandiTax").ToString()), "0.00")
                        Case 3 : dg1.Rows(i).Cells(1).Value = "RDF" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("RDF").ToString()), "0.00")
                        Case 4 : dg1.Rows(i).Cells(1).Value = "Bardana" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Bardana").ToString()), "0.00")
                        Case 5 : dg1.Rows(i).Cells(1).Value = "Labour" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Labour").ToString()), "0.00")
                        Case 6 : dg1.Rows(i).Cells(1).Value = "========================" : dg1.Rows(i).Cells(2).Value = "===================="
                        Case 7 : dg1.Rows(i).Cells(1).Value = "Total Expenses" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Charges").ToString()), "0.00")
                        Case 8 : dg1.Rows(i).Cells(1).Value = "========================" : dg1.Rows(i).Cells(2).Value = "===================="
                    End Select
                End If

                ' Add Account Data in Column 3 and 4
                If dtAcc.Rows.Count > i Then
                    dg1.Rows(i).Cells(3).Value = dtAcc.Rows(i)("AccountName").ToString()
                    dg1.Rows(i).Cells(4).Value = Format(Val(dtAcc.Rows(i)("TotalAmount").ToString()), "0.00")
                End If
            Next

            ' Clear Unnecessary Selections
            dg1.ClearSelection()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
    End Sub

   
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\DailyNakal.rpt")
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
                      "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "', " & _
                      "'" & .Cells(4).Value & "'"
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            ' LastRecord = LastCount
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,P1, P2,P3, P4) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try

        Next

    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
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

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
End Class