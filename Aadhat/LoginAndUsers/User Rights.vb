Public Class User_Rights

    Private Sub User_Rights_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub User_Rights_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.KeyPreview = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 3
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "User Name"
        dg1.Columns(1).Width = 300
        dg1.Columns(2).Name = "Password"
        dg1.Columns(2).Width = 150
        retrive()
    End Sub
    Private Sub retrive()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select UserTypeID,UserTypeName from UserRights Where Tag<> 0 Group By UserTypeID")
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("UserTypeID").ToString()
                        .Cells(1).Value = dt.Rows(i)("UserTypeName").ToString()
                        ' .Cells(2).Value = dt.Rows(i)("Password").ToString("*")
                        '.cells(2).value=
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        dg1.ClearSelection()
    End Sub
    Private Sub fillControls(ByVal ID As Integer)
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        BtnDelete.Visible = True
        BtnSave.Text = "&Update"
        Dim saleAdd As String = String.Empty
        Dim saleUpdate As String = String.Empty
        Dim saleDelete As String = String.Empty
        Dim Saleview As String = String.Empty
        Dim SalePrint As String = String.Empty

        Dim PurchaseAdd As String = String.Empty
        Dim PurchaseUpdate As String = String.Empty
        Dim PurchaseDelete As String = String.Empty
        Dim Purchaseview As String = String.Empty
        Dim PurchasePrint As String = String.Empty

        Dim VoucherAdd As String = String.Empty
        Dim VoucherUpdate As String = String.Empty
        Dim VoucherDelete As String = String.Empty
        Dim VoucherView As String = String.Empty
        Dim VoucherPrint As String = String.Empty

        Dim SelloutAdd As String = String.Empty
        Dim SelloutUpdate As String = String.Empty
        Dim SelloutDelete As String = String.Empty
        Dim SelloutView As String = String.Empty
        Dim SelloutPrint As String = String.Empty

        Dim crateAdd As String = String.Empty
        Dim crateUpdate As String = String.Empty
        Dim crateDelete As String = String.Empty
        Dim crateView As String = String.Empty
        Dim cratePrint As String = String.Empty

        Dim MobileView As String = String.Empty
        Dim PasswordView As String = String.Empty
        Dim Server As String = String.Empty
        Dim PrintBills As String = String.Empty
        Dim Reports As String = String.Empty
        Dim DontAllowBack As String = String.Empty

        ssql = "Select * from UserRights where UserTypeID=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                txtID.Text = Val(dt.Rows(i)("UserTypeID").ToString())
                txtFullName.Text = dt.Rows(i)("UserTypeName").ToString()
                If dt.Rows(i)("EntryType").ToString() = "Sale" Then
                    saleAdd = dt.Rows(i)("Save").ToString()
                    If saleAdd = "Y" Then ckaddSale.Checked = True Else ckaddSale.Checked = False
                    saleUpdate = dt.Rows(i)("Modify").ToString()
                    If saleUpdate = "Y" Then ckUpdateSale.Checked = True Else ckUpdateSale.Checked = False
                    saleDelete = dt.Rows(i)("Remove").ToString()
                    If saleDelete = "Y" Then ckDeleteSale.Checked = True Else ckDeleteSale.Checked = False
                    Saleview = dt.Rows(i)("See").ToString()
                    If Saleview = "Y" Then ckViewSale.Checked = True Else ckViewSale.Checked = False
                    SalePrint = dt.Rows(i)("Print").ToString()
                    If SalePrint = "Y" Then ckPrintSale.Checked = True Else ckPrintSale.Checked = False
                End If
                If dt.Rows(i)("EntryType").ToString() = "Purchase" Then
                    PurchaseAdd = dt.Rows(i)("Save").ToString()
                    If PurchaseAdd = "Y" Then ckAddPurchase.Checked = True Else ckAddPurchase.Checked = False
                    PurchaseUpdate = dt.Rows(i)("Modify").ToString()
                    If PurchaseUpdate = "Y" Then ckUpdatePurchase.Checked = True Else ckUpdatePurchase.Checked = False
                    PurchaseDelete = dt.Rows(i)("Remove").ToString()
                    If PurchaseDelete = "Y" Then ckDeletePurchase.Checked = True Else ckDeletePurchase.Checked = False
                    Purchaseview = dt.Rows(i)("See").ToString()
                    If Purchaseview = "Y" Then ckViewPurcahse.Checked = True Else ckViewPurcahse.Checked = False
                    PurchasePrint = dt.Rows(i)("Print").ToString()
                    If PurchasePrint = "Y" Then ckPrintPurchase.Checked = True Else ckPrintPurchase.Checked = False
                End If
                If dt.Rows(i)("EntryType").ToString() = "Voucher" Then
                    VoucherAdd = dt.Rows(i)("Save").ToString()
                    If VoucherAdd = "Y" Then ckAddVouchers.Checked = True Else ckAddVouchers.Checked = False
                    VoucherUpdate = dt.Rows(i)("Modify").ToString()
                    If VoucherUpdate = "Y" Then ckUpdateVouchers.Checked = True Else ckUpdateVouchers.Checked = False
                    VoucherDelete = dt.Rows(i)("Remove").ToString()
                    If VoucherDelete = "Y" Then ckDeleteVouchers.Checked = True Else ckDeleteVouchers.Checked = False
                    VoucherView = dt.Rows(i)("See").ToString()
                    If VoucherView = "Y" Then ckViewVouchers.Checked = True Else ckViewVouchers.Checked = False
                    VoucherPrint = dt.Rows(i)("Print").ToString()
                    If VoucherPrint = "Y" Then ckPrintVouchers.Checked = True Else ckPrintVouchers.Checked = False
                End If
                If dt.Rows(i)("EntryType").ToString() = "Sellout" Then
                    SelloutAdd = dt.Rows(i)("Save").ToString()
                    If SelloutAdd = "Y" Then ckaddSellout.Checked = True Else ckaddSellout.Checked = False
                    SelloutUpdate = dt.Rows(i)("Modify").ToString()
                    If SelloutUpdate = "Y" Then ckUpdateSellOut.Checked = True Else ckUpdateSellOut.Checked = False
                    SelloutDelete = dt.Rows(i)("Remove").ToString()
                    If SelloutDelete = "Y" Then ckDeleteSellOut.Checked = True Else ckDeleteSellOut.Checked = False
                    SelloutView = dt.Rows(i)("See").ToString()
                    If SelloutView = "Y" Then ckViewSellOut.Checked = True Else ckViewSellOut.Checked = False
                    SelloutPrint = dt.Rows(i)("Print").ToString()
                    If SelloutPrint = "Y" Then ckPrintSellOut.Checked = True Else ckPrintSellOut.Checked = False
                End If
                If dt.Rows(i)("EntryType").ToString() = "Crate" Then
                    crateAdd = dt.Rows(i)("Save").ToString()
                    If crateAdd = "Y" Then ckAddCrates.Checked = True Else ckAddCrates.Checked = False
                    crateUpdate = dt.Rows(i)("Modify").ToString()
                    If crateUpdate = "Y" Then ckUpdateCrates.Checked = True Else ckUpdateCrates.Checked = False
                    crateDelete = dt.Rows(i)("Remove").ToString()
                    If PurchaseDelete = "Y" Then ckDeleteCrates.Checked = True Else ckDeleteCrates.Checked = False
                    crateView = dt.Rows(i)("See").ToString()
                    If crateView = "Y" Then ckViewCrates.Checked = True Else ckViewCrates.Checked = False
                    cratePrint = dt.Rows(i)("Print").ToString()
                    If cratePrint = "Y" Then ckPrintCrates.Checked = True Else ckPrintCrates.Checked = False
                End If
                If dt.Rows(i)("EntryType").ToString() = "Other" Then
                    MobileView = dt.Rows(i)("Mobileapp").ToString()
                    If MobileView = "Y" Then ckMobile.Checked = True Else ckMobile.Checked = False
                    Server = dt.Rows(i)("Server").ToString()
                    If Server = "Y" Then ckServer.Checked = True Else ckServer.Checked = False
                    Reports = dt.Rows(i)("Reports").ToString()
                    If Reports = "Y" Then ckReports.Checked = True Else ckReports.Checked = False
                    PrintBills = dt.Rows(i)("BillPrints").ToString()
                    If PrintBills = "Y" Then ckPrintBills.Checked = True Else ckPrintBills.Checked = False
                    PasswordView = dt.Rows(i)("AppPassword").ToString()
                    If PasswordView = "Y" Then ckAppPassword.Checked = True Else ckAppPassword.Checked = False
                    DontAllowBack = dt.Rows(i)("DontAllowBack").ToString()
                    If DontAllowBack = "Y" Then ckDontAllowBack.Checked = True Else ckDontAllowBack.Checked = False
                End If
            Next

        End If
    End Sub
    Private Sub Save()
        Dim saleAdd As String = String.Empty
        If ckaddSale.Checked = True Then saleAdd = "Y" Else saleAdd = "N"
        Dim saleUpdate As String = String.Empty
        If ckUpdateSale.Checked = True Then saleUpdate = "Y" Else saleUpdate = "N"
        Dim saleDelete As String = String.Empty
        If ckDeleteSale.Checked = True Then saleDelete = "Y" Else saleDelete = "N"
        Dim Saleview As String = String.Empty
        If ckViewSale.Checked = True Then Saleview = "Y" Else Saleview = "N"
        Dim SalePrint As String = String.Empty
        If ckPrintSale.Checked = True Then SalePrint = "Y" Else SalePrint = "N"

        Dim PurchaseAdd As String = String.Empty
        If ckAddPurchase.Checked = True Then PurchaseAdd = "Y" Else PurchaseAdd = "N"
        Dim PurchaseUpdate As String = String.Empty
        If ckUpdatePurchase.Checked = True Then PurchaseUpdate = "Y" Else PurchaseUpdate = "N"
        Dim PurchaseDelete As String = String.Empty
        If ckDeletePurchase.Checked = True Then PurchaseDelete = "Y" Else PurchaseDelete = "N"
        Dim Purchaseview As String = String.Empty
        If ckViewPurcahse.Checked = True Then Purchaseview = "Y" Else Purchaseview = "N"
        Dim PurchasePrint As String = String.Empty
        If ckPrintPurchase.Checked = True Then PurchasePrint = "Y" Else PurchasePrint = "N"

        Dim VoucherAdd As String = String.Empty
        If ckAddVouchers.Checked = True Then VoucherAdd = "Y" Else VoucherAdd = "N"
        Dim VoucherUpdate As String = String.Empty
        If ckUpdateVouchers.Checked = True Then VoucherUpdate = "Y" Else VoucherUpdate = "N"
        Dim VoucherDelete As String = String.Empty
        If ckDeleteVouchers.Checked = True Then VoucherDelete = "Y" Else VoucherDelete = "N"
        Dim VoucherView As String = String.Empty
        If ckViewVouchers.Checked = True Then VoucherView = "Y" Else VoucherView = "N"
        Dim VoucherPrint As String = String.Empty
        If ckPrintVouchers.Checked = True Then VoucherPrint = "Y" Else VoucherPrint = "N"

        Dim SelloutAdd As String = String.Empty
        If ckaddSellout.Checked = True Then SelloutAdd = "Y" Else SelloutAdd = "N"
        Dim SelloutUpdate As String = String.Empty
        If ckUpdateSellOut.Checked = True Then SelloutUpdate = "Y" Else SelloutUpdate = "N"
        Dim SelloutDelete As String = String.Empty
        If ckDeleteSellOut.Checked = True Then SelloutDelete = "Y" Else SelloutDelete = "N"
        Dim SelloutView As String = String.Empty
        If ckViewSellOut.Checked = True Then SelloutView = "Y" Else SelloutView = "N"
        Dim SelloutPrint As String = String.Empty
        If ckPrintSellOut.Checked = True Then SelloutPrint = "Y" Else SelloutPrint = "N"

        Dim crateAdd As String = String.Empty
        If ckAddCrates.Checked = True Then crateAdd = "Y" Else crateAdd = "N"
        Dim crateUpdate As String = String.Empty
        If ckUpdateCrates.Checked = True Then crateUpdate = "Y" Else crateUpdate = "N"
        Dim crateDelete As String = String.Empty
        If ckDeleteSellOut.Checked = True Then crateDelete = "Y" Else crateDelete = "N"
        Dim crateView As String = String.Empty
        If ckViewCrates.Checked = True Then crateView = "Y" Else crateView = "N"
        Dim cratePrint As String = String.Empty
        If ckPrintCrates.Checked = True Then cratePrint = "Y" Else cratePrint = "N"

        Dim MobileView As String = String.Empty
        If ckMobile.Checked = True Then MobileView = "Y" Else MobileView = "N"
        Dim PasswordView As String = String.Empty
        If ckAppPassword.Checked = True Then PasswordView = "Y" Else PasswordView = "N"
        Dim Server As String = String.Empty
        If ckServer.Checked = True Then Server = "Y" Else Server = "N"
        Dim PrintBills As String = String.Empty
        If ckPrintBills.Checked = True Then PrintBills = "Y" Else PrintBills = "N"
        Dim Reports As String = String.Empty
        If ckReports.Checked = True Then Reports = "Y" Else Reports = "N"
        Dim DontAllowBack As String = String.Empty
        If ckDontAllowBack.Checked = True Then DontAllowBack = "Y" Else DontAllowBack = "N"
        If clsFun.ExecScalarInt("Select count(*) from UserRights where upper(UserTypeID)=upper('" & Val(txtID.Text) & "')") = 0 Then
            Dim UsertypeID As Integer = clsFun.ExecScalarInt("sELECT MAX(UserTypeID) From UserRights")
            txtID.Text = UsertypeID + 1
        End If
        clsFun.ExecScalarStr("Delete From UserRights Where UserTypeID ='" & Val(txtID.Text) & "'")
        Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = String.Empty
        sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " & _
                            " ('" & txtID.Text & "','" & txtFullName.Text & "','Sale','" & saleAdd & "','" & saleUpdate & "','" & saleDelete & "','" & Saleview & "', " & _
                            "'" & SalePrint & "',1);"
        sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " & _
                     " ('" & txtID.Text & "','" & txtFullName.Text & "','Purchase','" & PurchaseAdd & "','" & PurchaseUpdate & "','" & PurchaseDelete & "','" & Purchaseview & "', " & _
                    "'" & PurchasePrint & "',1);"
        sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " & _
                   " ('" & txtID.Text & "','" & txtFullName.Text & "','Voucher','" & VoucherAdd & "','" & VoucherUpdate & "','" & VoucherDelete & "','" & VoucherView & "', " & _
                    "'" & VoucherPrint & "',1);"
        sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " & _
                     " ('" & txtID.Text & "','" & txtFullName.Text & "','Sellout','" & SelloutAdd & "','" & SelloutUpdate & "','" & SelloutDelete & "','" & SelloutView & "', " & _
                    "'" & SelloutPrint & "',1);"
        sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Save,Modify,Remove,See,Print,Tag) values " & _
                     " ('" & txtID.Text & "','" & txtFullName.Text & "','Crate','" & crateAdd & "','" & crateUpdate & "','" & crateDelete & "','" & crateView & "', " & _
                    "'" & cratePrint & "',1);"
        sql = sql & "insert into UserRights(UserTypeID,UserTypeName,EntryType, Mobileapp,Server,Reports,BillPrints,AppPassword,DontAllowBack,Tag) values " & _
                     " ('" & txtID.Text & "','" & txtFullName.Text & "','Other','" & MobileView & "','" & Server & "','" & Reports & "','" & PrintBills & "', " & _
                    "'" & PasswordView & "','" & DontAllowBack & "',1);"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then
                MessageBox.Show("User Roles Created Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
                retrive() : txtID.Text = ""
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub ckGetDefaults_CheckedChanged(sender As Object, e As EventArgs) Handles ckGetDefaults.CheckedChanged
        If ckGetDefaults.Checked = True Then
            DefaultSettings()
        End If
    End Sub
    Private Sub DefaultSettings()
        ckDontAllowBack.Checked = True

        ckaddSale.Checked = True
        ckUpdateSale.Checked = False
        ckDeleteSale.Checked = False
        ckViewSale.Checked = True
        ckPrintSale.Checked = True

        ckAddPurchase.Checked = True
        ckUpdatePurchase.Checked = False
        ckDeletePurchase.Checked = False
        ckViewPurcahse.Checked = True
        ckPrintPurchase.Checked = True

        ckAddVouchers.Checked = True
        ckUpdateVouchers.Checked = False
        ckDeleteVouchers.Checked = False
        ckViewVouchers.Checked = True
        ckPrintVouchers.Checked = True

        ckaddSellout.Checked = True
        ckUpdateSellOut.Checked = False
        ckDeleteSellOut.Checked = False
        ckViewSellOut.Checked = True
        ckPrintSellOut.Checked = True

        ckAddCrates.Checked = True
        ckUpdateCrates.Checked = False
        ckDeleteCrates.Checked = False
        ckViewCrates.Checked = True
        ckPrintCrates.Checked = True

        ckMobile.Checked = True
        ckServer.Checked = False
        ckPrintBills.Checked = True
        ckAppPassword.Checked = False
        ckReports.Checked = True

    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpid As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
        fillControls(tmpid)
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpid As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
            fillControls(tmpid)
        End If
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Save()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        delete()
    End Sub
    Private Sub delete()
        If clsFun.ExecScalarInt("Select tag From UserRights  WHERE UserTypeID=" & Val(txtID.Text) & "") = 0 Then MsgBox("Pre-Define Master. You Can't Delete it.", vbOkOnly, "Access Denied") : Exit Sub
        If clsFun.ExecScalarInt("Select count(*) from Users where UserTypeID=" & Val(txtID.Text) & "") <> 0 Then
            MsgBox("User Role Alloted to user...", vbOkOnly, "Access Denied")
            Exit Sub
        End If
        If MessageBox.Show("are you Sure want to Delete User Role... ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from UserRights WHERE UserTypeID=" & Val(txtID.Text) & "") > 0 Then
                MsgBox("User Role Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                retrive() : txtID.Text = "" : txtFullName.Text = ""
            End If
        End If
    End Sub
End Class