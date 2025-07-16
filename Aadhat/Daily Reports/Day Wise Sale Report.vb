Public Class Day_Wise_Sale_Report

    Private Sub Cash_Sale_Report_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Cash_Sale_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Min(EntryDate) from Transaction2 ")
        maxdate = clsFun.ExecScalarStr("Select max(EntryDate) from Transaction2 ")
        mskFromDate.Text = IIf(mindate <> "", CDate(mindate).ToString("dd-MM-yyyy"), Date.Today.ToString("dd-MM-yyyy"))
        MsktoDate.Text = IIf(maxdate <> "", CDate(maxdate).ToString("dd-MM-yyyy"), Date.Today.ToString("dd-MM-yyyy"))
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "Nugs" : dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "Weight" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "basic Amount" : dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "Charges Amount" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Credit Amount" : dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "Cash Amount" : dg1.Columns(7).Width = 150
        dg1.Columns(8).Name = "Total Amount" : dg1.Columns(8).Width = 200
        dg1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Sub calc()
        txtTotalNug.Text = Format(0, "0.00") : txtTotalWeight.Text = Format(0, "0.00")
        txtbasicAmt.Text = Format(0, "0.00") : txtCreditSale.Text = Format(0, "0.00")
        txtTotalCash.Text = Format(0, "0.00") : txtTotalCharges.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNug.Text = Format(Val(txtTotalNug.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txtTotalWeight.Text = Format(Val(txtTotalWeight.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtbasicAmt.Text = Format(Val(txtbasicAmt.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtTotalCharges.Text = Format(Val(txtTotalCharges.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtCreditSale.Text = Format(Val(txtCreditSale.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotalCash.Text = Format(Val(txtTotalCash.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
        Next
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()
    End Sub
    Private Sub Retrive()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ' ssql = "Select VourchersID,EntryDate,round(Sum(Amount),2) as CashAmount From Ledger Where AccountID=7  and DC='D' and TransType in('Stock Sale','Super Sale' ,'Standard Sale','Speed Sale') and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'Group by EntryDate Order By EntryDate"
        ssql = "Select EntryDate,Sum(Nug)as Nug,Sum(Weight)as Weight,Sum(Amount) as Amount,Sum(Charges) as Charges From Transaction2  Where TransType <>'On Sale' and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'Group by EntryDate Order By EntryDate"

        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            pnlWait.Visible = True
            pb1.Minimum = 0
            For i = 0 To dt.Rows.Count - 1
                pb1.Maximum = dt.Rows.Count - 1
                Application.DoEvents()
                dg1.Rows.Add()
                With dg1.Rows(i)
                    pb1.Value = i
                    .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    ' .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                    .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    .Cells(2).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(3).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(4).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(5).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                    .Cells(6).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount)  From Transaction2 Where AccountID<> 7 and EntryDate='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(7).Value = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount)  From Transaction2 Where AccountID= 7 and EntryDate='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(8).Value = Format(Val(.Cells(6).Value) + Val(.Cells(7).Value), "0.00")
                End With
            Next i
        End If
        pnlWait.Visible = False
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub

    Private Sub mskFromDate_Validated(sender As Object, e As EventArgs) Handles mskFromDate.Validated

    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
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
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        pnlWait.Visible = True
        For Each row As DataGridViewRow In dg1.Rows
            Application.DoEvents()

            pb1.Minimum = 0
            pb1.Maximum = dg1.Rows.Count

            With row
                pb1.Value = Val(row.Index)
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & Format(Val(.Cells(2).Value), "0.00") & "','" & Format(Val(.Cells(3).Value), "0.00") & "','" & Format(Val(.Cells(4).Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells(5).Value), "0.00") & "'," & Format(Val(.Cells(6).Value), "0.00") & ",'" & Format(Val(.Cells(7).Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells(8).Value), "0.00") & "'," & Format(Val(txtTotalNug.Text), "0.00") & "," & _
                     " " & Format(Val(txtTotalWeight.Text), "0.00") & "," & Format(Val(txtbasicAmt.Text), "0.00") & ",'" & Format(Val(txtTotalCharges.Text), "0.00") & "'," & _
                    "'" & Format(Val(txtCreditSale.Text), "0.00") & "'," & Format(Val(txtTotalCash.Text), "0.00") & ",'" & Format(Val(TxtGrandTotal.Text), "0.00") & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    pnlWait.Visible = False
                    ClsFunPrimary.CloseConnection()
                End Try
                pnlWait.Visible = False
            End With
        Next
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        PrintRecord()
        Report_Viewer.printReport("\Reports\DayWiseSaleReport.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
End Class