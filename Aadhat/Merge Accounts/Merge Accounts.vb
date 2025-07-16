Public Class Merge_Accounts

    Private Sub Merge_Accounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clsFun.FillDropDownList(cbSelectAccount, "Select ID,AccountName FROM Accounts Order by AccountName ", "AccountName", "ID", "")
        clsFun.FillDropDownList(cbDestination, "Select ID,AccountName FROM Accounts Order by AccountName", "AccountName", "ID", "")
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Select

        ' clsFun.ExecScalarStr("Update Accounts Set,Opbal= AccountID='" & Val(cbDestination.SelectedValue) & "', AccountName='" & cbDestination.Text & "' Where AccountID='" & Val(cbSelectAccount.SelectedValue) & "'")


        clsFun.ExecScalarStr("Update Vouchers Set AccountID='" & Val(cbDestination.SelectedValue) & "', AccountName='" & cbDestination.Text & "' Where AccountID='" & Val(cbSelectAccount.SelectedValue) & "'")
        clsFun.ExecScalarStr("Update Transaction2 Set AccountID='" & Val(cbDestination.SelectedValue) & "', AccountName='" & cbDestination.Text & "' Where AccountID='" & Val(cbSelectAccount.SelectedValue) & "'")
        clsFun.ExecScalarStr("Update Ledger Set AccountID='" & Val(cbDestination.SelectedValue) & "', AccountName='" & cbDestination.Text & "' Where AccountID='" & Val(cbSelectAccount.SelectedValue) & "'")
        MsgBox("Account Merge Seccessfully....", vbInformation, "Successful")
    End Sub
End Class