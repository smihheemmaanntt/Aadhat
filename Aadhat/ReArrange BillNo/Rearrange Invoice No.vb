Public Class Rearrange_Invoice_No

    Private Sub Rearrange_Invoice_No_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clsFun.FillDropDownList(Cbper, "Select ID,TransType FROM Vouchers  Group by TransType", "TransType", "ID", "")
        RadioInvoiceID.Checked = True
    End Sub
    Private Sub Rearrange()
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID FROM Vouchers Where TransType='" & Cbper.Text & "' Order by ID,EntryDate   "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            Pb1.Minimum = 0
            Pb1.Maximum = dt.Rows.Count
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Pb1.Value = i
                If RadioBoth.Checked = True Then
                    sql = "Update Vouchers Set BillNo='" & Val(i + 1) & "',InvoiceID='" & Val(i + 1) & "' Where ID='" & Val(dt.Rows(i)(0).ToString) & "'"
                    clsFun.ExecNonQuery(sql)
                ElseIf RadioInvoiceID.Checked = True Then
                    sql = "Update Vouchers Set InvoiceID='" & Val(i + 1) & "' Where ID='" & Val(dt.Rows(i)(0).ToString) & "'"
                    clsFun.ExecNonQuery(sql)
                ElseIf RadioBillNo.Checked = True Then
                    sql = "Update Vouchers Set BillNo='" & Val(i + 1) & "' Where ID='" & Val(dt.Rows(i)(0).ToString) & "'"
                    clsFun.ExecNonQuery(sql)
                End If
            Next
        End If
        MsgBox("Invoice Number Updated Successfully.", vbInformation + vbOKOnly, "Updated")
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If MessageBox.Show("Warning..." & vbNewLine & "Are you Sure want to Rearrange Transactionn : " & Cbper.Text & " ??" & vbNewLine & "It Will Not Undo Again....  ", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
            If RadioAccountID.Checked = False Then
                Rearrange()
            Else
                Rearrange2()
            End If

        End If

    End Sub
    Private Sub Rearrange2()
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID FROM Accounts Where Tag=1"
        Dim dt As DataTable
        Dim Lastval As Integer = 1000
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            Pb1.Minimum = 0
            Pb1.Maximum = dt.Rows.Count
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Lastval = Lastval + 1
                Pb1.Value = i
                sql = "Update Accounts Set ID='" & Val(Lastval) & "' Where ID='" & Val(dt.Rows(i)(0).ToString) & "'"
                clsFun.ExecNonQuery(sql)

            Next
        End If
        MsgBox("Invoice Number Updated Successfully.", vbInformation + vbOKOnly, "Updated")
    End Sub
End Class