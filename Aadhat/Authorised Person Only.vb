Public Class Authorised_Person_Only

    Private Sub btnUpdateSuperSale_Click(sender As Object, e As EventArgs) Handles btnUpdateSuperSale.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Super Sale' And EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                'Application.DoEvents()
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.BringToFront()
                Super_Sale.Show()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Super_Sale.FillControls(VID)
                Super_Sale.MultiUpdate()
            Next
        End If
        MsgBox("Super Sale Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub btnUpdatePurchase_Click(sender As Object, e As EventArgs) Handles btnUpdatePurchase.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Purchase' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Purchase.MdiParent = MainScreenForm
                Purchase.Show()
                Purchase.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Purchase.FillControls(VID)
                Purchase.MultiUpdatePurchase()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim sql As String = String.Empty
        Dim ssql As String = "Select VoucherID,VehicleNo From Purchase WHERE EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim mcid As Integer = clsFun.ExecScalarInt("Select VoucherID From Transaction1 Where PurchaseID='" & Val(dt.Rows(i)(0).ToString) & "'   Group by PurchaseID ")
                clsFun.ExecNonQuery("update Vouchers Set VehicleNo ='" & dt.Rows(i)(1).ToString & "' Where ID='" & Val(mcid) & "' And TransType='Auto Beejak' ")
            Next
        End If
        MsgBox("Updated")
    End Sub
    Private Sub Day_book_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Authorised_Person_Only_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.GhostWhite
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        txtPassword.UseSystemPasswordChar = True
        txtPassword.Focus()
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtPassword.Text = "smi@933933" Then
                GBPassword.Visible = False
            Else
                MsgBox("You are Not Authroised Person to Access Please Contact to Autorised Person... ", MsgBoxStyle.Critical, "Access Denied")
                Me.Close() : Exit Sub
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        PartnerDetails.MdiParent = MainScreenForm
        PartnerDetails.Show()
        PartnerDetails.BringToFront()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'   "
        '   Dim ssql As String = "Select VoucherID From Transaction2 where ItemID=223 and TransType='Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                SpeedSale.MdiParent = MainScreenForm
                SpeedSale.Show()
                SpeedSale.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                SpeedSale.FillContros(VID)
                SpeedSale.MultiUpdateSpeed()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Beejak' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Sellout_Mannual.MdiParent = MainScreenForm
                Sellout_Mannual.Show()
                Sellout_Mannual.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Sellout_Mannual.FillContros(VID)
                Sellout_Mannual.UpdateMulti()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Auto Beejak' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Sellout_Auto.MdiParent = MainScreenForm
                Sellout_Auto.Show()
                Sellout_Auto.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Sellout_Auto.FillFromData(VID)
                Sellout_Auto.MultiUpdate()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If clsFun.ExecScalarInt("Select Count(*) From Accounts Where ID=56") = 0 Then
            Dim Sql As String = "Insert Into Accounts (ID,AccountName,GroupID,DC,Tag,Opbal,OtherName) values(56,'Difference A/c',10,'Dr',0,0,'Difference A/c')"
            clsFun.ExecNonQuery(Sql)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Stock Sale' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Stock_Sale.MdiParent = MainScreenForm
                Stock_Sale.Show()
                Stock_Sale.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Stock_Sale.FillControls(VID)
                Stock_Sale.MultiUpdateStockSale()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Receipt' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                ReceiptForm.MdiParent = MainScreenForm
                ReceiptForm.Show()
                ReceiptForm.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                ReceiptForm.FillControls(VID)
                ReceiptForm.MultiUpdateReceipt()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Payment' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                PayMentform.MdiParent = MainScreenForm
                PayMentform.Show()
                PayMentform.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                PayMentform.FillControls(VID)
                PayMentform.MultiUpdateReceipt()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnBankEntry.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where BankEntry='Bank Entry' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Bank_Entry.MdiParent = MainScreenForm
                Bank_Entry.Show()
                Bank_Entry.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Bank_Entry.FillControls(VID)
                Bank_Entry.UpdateMultiBankEntries()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Standard Sale' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Standard_Sale.MdiParent = MainScreenForm
                Standard_Sale.Show()
                Standard_Sale.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Standard_Sale.FillControls(VID)
                Standard_Sale.UpdateMulti()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Purchase (Loose)' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Loose_Purchase.MdiParent = MainScreenForm
                Loose_Purchase.Show()
                Loose_Purchase.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Loose_Purchase.FillControls(VID)
                Loose_Purchase.MultiUpdatePurchase()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sql As String = String.Empty
        Dim ssql As String = "Select ID From Vouchers Where TransType='Sale (Loose)' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If (dt.Rows.Count) > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Application.DoEvents()
                Loose_Sale.MdiParent = MainScreenForm
                Loose_Sale.Show()
                Loose_Sale.BringToFront()
                Dim VID As Integer = Val(dt.Rows(i)(0).ToString)
                Loose_Sale.FillControls(VID)
                Loose_Sale.UpdateMulti()
            Next
        End If
        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged

    End Sub
End Class