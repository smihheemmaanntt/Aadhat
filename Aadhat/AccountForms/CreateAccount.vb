Imports System.IO
Imports System.Data.SQLite

Public Class CreateAccount
    Dim CustImagePath As String = String.Empty
    Dim GurImagePath As String = String.Empty
    Dim tmpid As Integer = 0
    Dim rs As New Resizer
    Public FrmName As Form
    Public AccountNameValue As String = ""
    Public AccountID As String = ""
    Public OpenedFromItems As Boolean = False
    Public Opener As Form : Public TextBoxSender As String
    Private translation As New Translation()
    Dim Language As String = String.Empty
    Private Sub CreateAccount_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If OpenedFromItems Then
            Opener.Focus()
            If TypeOf Opener Is Super_Sale Then
                If TextBoxSender = "txtAccount" Then
                    DirectCast(Opener, Super_Sale).txtAccount.Focus()
                    If AccountNameValue.Trim Is Nothing Then Exit Sub
                    Super_Sale.txtAccount.Text = AccountNameValue
                    Super_Sale.txtAccountID.Text = Val(AccountID)
                    Super_Sale.txtAccount.SelectAll()
                ElseIf TextBoxSender = "txtCustomer" Then
                    DirectCast(Opener, Super_Sale).txtCustomer.Focus()
                    If AccountNameValue.Trim Is Nothing Then Exit Sub
                    Super_Sale.txtCustomer.Text = AccountNameValue
                    Super_Sale.txtcustomerID.Text = Val(AccountID)
                    Super_Sale.txtCustomer.SelectAll()
                End If
            ElseIf TypeOf Opener Is SpeedSale Then
                DirectCast(Opener, SpeedSale).txtAccount.Focus()
                If AccountNameValue.Trim Is Nothing Then Exit Sub
                SpeedSale.txtAccount.Text = AccountNameValue
                SpeedSale.txtAccountID.Text = Val(AccountID)
                SpeedSale.txtAccount.SelectAll()
            ElseIf TypeOf Opener Is Stock_Sale Then
                DirectCast(Opener, Stock_Sale).txtAccount.Focus()
                If AccountNameValue.Trim Is Nothing Then Exit Sub
                Stock_Sale.txtAccount.Text = AccountNameValue
                Stock_Sale.txtAccountID.Text = Val(AccountID)
                Stock_Sale.txtAccount.SelectAll()
            ElseIf TypeOf Opener Is Standard_Sale Then
                If TextBoxSender = "txtAccount" Then
                    DirectCast(Opener, Standard_Sale).txtAccount.Focus()
                    If AccountNameValue.Trim Is Nothing Then Exit Sub
                    Standard_Sale.txtAccount.Text = AccountNameValue
                    Standard_Sale.txtAccountID.Text = Val(AccountID)
                    Standard_Sale.txtAccount.SelectAll()
                ElseIf TextBoxSender = "cbAccountName" Then
                    clsFun.FillDropDownList(Standard_Sale.cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33) order by AccountName ", "AccountName", "ID", "--N./A.--")
                    Standard_Sale.cbAccountName.SelectedValue = Val(AccountID)
                    Standard_Sale.cbAccountName.Text = AccountNameValue
                    DirectCast(Opener, Standard_Sale).cbAccountName.Focus() ' Combobox par focus
                End If
            ElseIf TypeOf Opener Is Purchase Then
                DirectCast(Opener, Purchase).txtAccount.Focus()
                If AccountNameValue.Trim Is Nothing Then Exit Sub
                Purchase.txtAccount.Text = AccountNameValue
                Purchase.txtAccountID.Text = Val(AccountID)
                Purchase.txtAccount.SelectAll()
            ElseIf TypeOf Opener Is Sellout_Mannual Then
                DirectCast(Opener, Sellout_Mannual).txtAccount.Focus()
                If AccountNameValue.Trim Is Nothing Then Exit Sub
                Sellout_Mannual.txtAccount.Text = AccountNameValue
                Sellout_Mannual.txtAccountID.Text = Val(AccountID)
                Sellout_Auto.txtAccount.SelectAll()
            End If
        End If
    End Sub
    Private Sub CreateAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then btnClose.PerformClick()
        Select Case e.KeyCode
            Case Keys.F1
                translation.ChangeKeyboardLayout(translation.KLID_ENGLISH)
            Case Keys.F2
                translation.ChangeKeyboardLayout(translation.KLID_HINDI)
            Case Keys.F3
                translation.ChangeKeyboardLayout(translation.KLID_GUJARATI)
            Case Keys.F4
                translation.ChangeKeyboardLayout(translation.KLID_PUNJABI)
            Case Keys.F5
                translation.ChangeKeyboardLayout(translation.KLID_MARATHI)
            Case Keys.F6
                translation.ChangeKeyboardLayout(translation.KLID_TAMIL)
            Case Keys.F7
                translation.ChangeKeyboardLayout(translation.KLID_TELUGU)
            Case Keys.F8
                translation.ChangeKeyboardLayout(translation.KLID_BENGALI)
        End Select
    End Sub
    Private Sub CreateAccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        clsFun.FillDropDownList(cbGroup, "Select ID,GroupName FROM AccountGroup", "GroupName", "ID", "")
        Language = clsFun.ExecScalarStr("Select Language From Controls")
        cbDrCr.SelectedIndex = 0
        BtnDelete.Text = "&Reset"
        fontchange()
        LoadAutoCompleteData()
    End Sub


    Private Sub LoadAutoCompleteData()
        ' Step 1: Create AutoCompleteStringCollection to store suggestions
        Dim suggestions As New AutoCompleteStringCollection()

        Try
            ' Step 2: Get connection from your clsfun.GetConnection() function
            Using connection As SQLiteConnection = clsfun.GetConnection()

                ' Step 3: Write SQL query to get unique Area from Accounts table
                Dim query As String = "SELECT DISTINCT Area FROM Accounts WHERE Area IS NOT NULL AND Area <> ''"

                ' Step 4: Execute the SQL query using SQLiteCommand and SQLiteDataReader
                Using command As New SQLiteCommand(query, connection)
                    Using reader As SQLiteDataReader = command.ExecuteReader()

                        ' Step 5: Loop through the reader and add each Area to the suggestions
                        While reader.Read()
                            suggestions.Add(reader("Area").ToString())
                        End While

                    End Using ' End of reader
                End Using ' End of command
            End Using ' End of connection

            ' Step 6: Set AutoComplete properties of txtArea after suggestions are filled
            txtArea.AutoCompleteMode = AutoCompleteMode.SuggestAppend ' Type karte waqt suggestions show honge
            txtArea.AutoCompleteSource = AutoCompleteSource.CustomSource ' Custom source use karenge
            txtArea.AutoCompleteCustomSource = suggestions ' Jo suggestions mile wo AutoComplete mein set kar do

        Catch ex As SQLiteException
            MessageBox.Show("Database Error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub fontchange()
        'For Each c As Control In Me.Controls
        '    If TypeOf c Is TextBox Then
        '        c.Font = New Font("Times New Roman", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '    End If
        'Next
        'For Each c As Control In Me.Controls
        '    If TypeOf c Is Label Then
        '        c.Font = New Font("Times New Roman", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '    End If
        'Next
        'For Each c As Control In Me.Controls
        '    If TypeOf c Is Button Then
        '        c.Font = New Font("Times New Roman", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '    End If
        'Next
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Public Sub saveImage()
        '  Dim sJpegPicFileName As String
        FileName = String.Empty
        If Directory.Exists(Application.StartupPath & "\Images") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Images")
        End If
        If File.Exists(Application.StartupPath & "\Images\" & txtName.text.trim & ".jpg") = True Then

        End If
        If picCustomer.Image IsNot Nothing Then
            picCustomer.Image.Save(Application.StartupPath & "\Images\" & txtName.text.trim & ".jpg")
        End If
        If PicGurronter.Image IsNot Nothing Then
            PicGurronter.Image.Save(Application.StartupPath & "\Images\" & txtName.text.trim & "-Gurronter.jpg")
        End If
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarStr("Select count(*) from Accounts where upper(AccountName)=upper('" & txtName.text.trim.Trim & "')") = 1 Then
                MsgBox("Account Already Exists...", MsgBoxStyle.Critical, "Access Denied")
                txtName.Focus() : Exit Sub
            End If
            Save()
        Else
            Dim accountName As String = txtName.text.trim.Trim().ToUpper()
            Dim countQuery As String = "SELECT COUNT(*) FROM Accounts WHERE UPPER(AccountName) = '" & accountName & "'"
            Dim existingIDQuery As String = "SELECT ID FROM Accounts WHERE UPPER(AccountName) = '" & accountName & "'"
            Dim count As Integer = clsFun.ExecScalarInt(countQuery)
            Dim existingID As Integer = clsFun.ExecScalarInt(existingIDQuery)
            If count > 1 OrElse (existingID <> 0 AndAlso existingID <> Val(txtID.Text)) Then
                MsgBox("Account already exists.", MsgBoxStyle.Critical, "Access Denied")
                txtName.Focus()
                Exit Sub
            Else
                UpdateRecord()
            End If
        End If
        If Application.OpenForms().OfType(Of Account_List).Any = True Then Account_List.FillWithNevigation()
        SpeedSale.RefreshAccounts() : Stock_Sale.RefreshAccounts()
    End Sub
    Private Sub Save()
        If clsFun.ExecScalarStr("Select count(*)from Accounts where upper(AccountName)=upper('" & txtName.text.trim & "')") = 1 Then
            MsgBox("Account Already Exists...", vbOKOnly, "Access Denied")
            txtName.Focus() : Exit Sub
        End If
        Dim guid As Guid = guid.NewGuid()
        Dim cmd As SQLite.SQLiteCommand
        Dim CustImageName As String = String.Empty
        Dim GurimageName As String = String.Empty
        Dim Deactivate As String = String.Empty
        If ckDeactivate.Checked = True Then Deactivate = "Y" Else Deactivate = "N"
        If txtName.text.trim = "" Then
            txtName.Focus()
            MsgBox("Account Name is Blank. Please Fill Account Name... ", MsgBoxStyle.Critical, "Empty")
        ElseIf cbGroup.Text = "" Then
            cbGroup.Focus()
            MsgBox("Account Group is Blank. Please Fill Select Group... ", MsgBoxStyle.Critical, "Empty")
        Else
            Dim sql As String = "insert into Accounts(AccountName, Groupid,  DC, OpBal, OtherName, address, LFNo, Area, City," _
                                & " State, Phone, Contact, Mobile1, Mobile2, MailID, BankName, AccNo," _
                                & " IFSC, GName, Gmobile1, Gmobile2, Gaddress, GCity, Gstate,tag,CommPer,Mper,RdfPer,TarePer,LabourPer,Deactivate,PostingID,POSTINGACNAME,GUID)" _
                                & "values (@1, @2, @4, @5, @6, @7, @8, @9, @10,@11, @12, @13, @14, @15, @16, @17, @18, @19, @20," _
                                & "@21, @22, @23, @24, @25,@27,@28,@29, @30,@31,@32,@33,@34,@35,@36)"

            Try
                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                cmd.Parameters.AddWithValue("@1", txtName.text.trim.Trim) : cmd.Parameters.AddWithValue("@2", Val(cbGroup.SelectedValue))
                ' cmd.Parameters.AddWithValue("@3", cbGroup.Text)
                cmd.Parameters.AddWithValue("@4", cbDrCr.Text) : cmd.Parameters.AddWithValue("@5", txtOPBal.Text)
                cmd.Parameters.AddWithValue("@6", txtOtherName.Text.Trim) : cmd.Parameters.AddWithValue("@7", txtAddress.Text.Trim)
                cmd.Parameters.AddWithValue("@8", txtLf.Text) : cmd.Parameters.AddWithValue("@9", txtArea.Text.Trim)
                cmd.Parameters.AddWithValue("@10", txtCity.Text.Trim) : cmd.Parameters.AddWithValue("@11", txtState.Text.Trim)
                cmd.Parameters.AddWithValue("@12", txtPhone.Text) : cmd.Parameters.AddWithValue("@13", txtContact.Text)
                cmd.Parameters.AddWithValue("@14", txtMob1.Text) : cmd.Parameters.AddWithValue("@15", txtMob2.Text)
                cmd.Parameters.AddWithValue("@16", txtMail.Text) : cmd.Parameters.AddWithValue("@17", txtBank.Text)
                cmd.Parameters.AddWithValue("@18", txtACNo.Text) : cmd.Parameters.AddWithValue("@19", txtIfsc.Text)
                cmd.Parameters.AddWithValue("@20", txtGName.Text) : cmd.Parameters.AddWithValue("@21", txtGmob.Text)
                cmd.Parameters.AddWithValue("@22", txtGMob2.Text) : cmd.Parameters.AddWithValue("@23", TxtGAddress.Text)
                cmd.Parameters.AddWithValue("@24", TxtGCity.Text) : cmd.Parameters.AddWithValue("@25", txtGState.Text)
                cmd.Parameters.AddWithValue("@27", 1) : cmd.Parameters.AddWithValue("@28", Val(txtCommPer.Text))
                cmd.Parameters.AddWithValue("@29", Val(txtMPer.Text)) : cmd.Parameters.AddWithValue("@30", Val(txtRdfPer.Text))
                cmd.Parameters.AddWithValue("@31", Val(txtTarePer.Text)) : cmd.Parameters.AddWithValue("@32", Val(txtLabourPer.Text))
                cmd.Parameters.AddWithValue("@33", Deactivate) : cmd.Parameters.AddWithValue("@34", Val(txtPostingID.Text))
                cmd.Parameters.AddWithValue("@35", txtAcPosting.Text) : cmd.Parameters.AddWithValue("@36", guid.ToString)
                '''''  Update ServerDb'''''
                If cmd.ExecuteNonQuery() > 0 Then
                    saveImage() : ServerDbAdd()
                    'RetriveAccounts()
                    AccountNameValue = txtName.text.trim : AccountID = Val(txtID.Text)
                    ClearText() : If OpenedFromItems Then Me.Close() : Exit Sub
                    Me.Alert("Success Alert", msgAlert.enmType.Info)
                    'Call clsFun.FillDropDownList(cbGroup, "Select * From AccountGroup", "GroupName", "Id", "")
                End If
                clsFun.CloseConnection()
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
    End Sub
    Private Sub ServerDbAdd()
        If OrgID = 0 Then Exit Sub
        Dim AccountID As Integer = 0
        If BtnSave.Text = "&Save" Then
            AccountID = clsFun.ExecScalarInt("Select Max(ID) from Accounts")
        Else
            ClsFunserver.ExecScalarStr("Delete From Accounts Where ID='" & Val(txtID.Text) & "' and OrgID='" & Val(OrgID) & "'")
            AccountID = Val(txtID.Text)
        End If
        Dim cmd1 As SQLite.SQLiteCommand
        Dim ssql As String = "insert into Accounts(AccountName, Groupid,  DC, OpBal, OtherName, address, LFNo, Area, City," _
                 & " State, Phone, Contact, Mobile1, Mobile2, MailID, BankName, AccNo,IFSC, GName, Gmobile1, Gmobile2," _
                 & "  Gaddress, GCity, Gstate,tag,CommPer,Mper,RdfPer,TarePer,LabourPer,ServerTag,OrgID,ID)" _
                 & "values (@1, @2, @4, @5, @6, @7, @8, @9, @10,@11, @12, @13, @14, @15, @16, @17, @18, @19, @20," _
                 & "@21, @22, @23, @24, @25,@27,@28,@29, @30,@31,@32,@33,@34,@35)"
        Try
            cmd1 = New SQLite.SQLiteCommand(ssql, ClsFunserver.GetConnection())
            cmd1.Parameters.AddWithValue("@1", txtName.text.trim) : cmd1.Parameters.AddWithValue("@2", Val(cbGroup.SelectedValue))
            cmd1.Parameters.AddWithValue("@4", cbDrCr.Text) : cmd1.Parameters.AddWithValue("@5", txtOPBal.Text)
            cmd1.Parameters.AddWithValue("@6", txtOtherName.Text) : cmd1.Parameters.AddWithValue("@7", txtAddress.Text)
            cmd1.Parameters.AddWithValue("@8", txtLf.Text) : cmd1.Parameters.AddWithValue("@9", txtArea.Text)
            cmd1.Parameters.AddWithValue("@10", txtCity.Text) : cmd1.Parameters.AddWithValue("@11", txtState.Text)
            cmd1.Parameters.AddWithValue("@12", txtPhone.Text) : cmd1.Parameters.AddWithValue("@13", txtContact.Text)
            cmd1.Parameters.AddWithValue("@14", txtMob1.Text) : cmd1.Parameters.AddWithValue("@15", txtMob2.Text)
            cmd1.Parameters.AddWithValue("@16", txtMail.Text) : cmd1.Parameters.AddWithValue("@17", txtBank.Text)
            cmd1.Parameters.AddWithValue("@18", txtACNo.Text) : cmd1.Parameters.AddWithValue("@19", txtIfsc.Text)
            cmd1.Parameters.AddWithValue("@20", txtGName.Text) : cmd1.Parameters.AddWithValue("@21", txtGmob.Text)
            cmd1.Parameters.AddWithValue("@22", txtGMob2.Text) : cmd1.Parameters.AddWithValue("@23", TxtGAddress.Text)
            cmd1.Parameters.AddWithValue("@24", TxtGCity.Text) : cmd1.Parameters.AddWithValue("@25", txtGState.Text)
            cmd1.Parameters.AddWithValue("@27", 1) : cmd1.Parameters.AddWithValue("@28", Val(txtCommPer.Text))
            cmd1.Parameters.AddWithValue("@29", Val(txtMPer.Text)) : cmd1.Parameters.AddWithValue("@30", Val(txtRdfPer.Text))
            cmd1.Parameters.AddWithValue("@31", Val(txtTarePer.Text)) : cmd1.Parameters.AddWithValue("@32", Val(txtLabourPer.Text))
            cmd1.Parameters.AddWithValue("@33", Val(1)) : cmd1.Parameters.AddWithValue("@34", Val(OrgID))
            cmd1.Parameters.AddWithValue("@35", Val(AccountID))
            cmd1.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try

    End Sub
    Public Sub Alert(ByVal msg As String, ByVal type As msgAlert.enmType)
        Dim frm As msgAlert = New msgAlert()
        frm.showAlert(msg, type)
    End Sub

    Private Sub txtMob2_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            txtMail.Focus()
        End If
    End Sub
    Private Sub ClearText()
        txtName.Text = "" : txtLimit.Text = ""
        cbDrCr.Text = "" : txtOPBal.Text = ""
        txtOtherName.Text = "" : txtLf.Text = ""
        txtAddress.Text = "" : txtArea.Text = ""
        txtCity.Text = "" : txtState.Text = ""
        txtPhone.Text = "" : txtContact.Text = ""
        txtMob1.Text = "" : txtMob2.Text = ""
        txtMail.Text = "" : txtBank.Text = ""
        txtACNo.Text = "" : txtIfsc.Text = ""
        txtGName.Text = "" : TxtGAddress.Text = ""
        txtGmob.Text = "" : txtGMob2.Text = ""
        txtState.Text = "" : txtCity.Text = ""
        txtAcPosting.Text = "" : txtPostingID.Text = Val(0)
        'picCustomer.Image = My.Resources.Account
        'PicGurronter.Image = My.Resources.Gur
        BtnDelete.Text = "&Reset" : BtnSave.Text = "&Save"
        txtName.Focus() : Account_List.btnRetrive.PerformClick()
        lblName.Text = "CREATE ACCOUNT" : LoadAutoCompleteData()
    End Sub
    Public Sub FillContros(ByVal id As Integer)
        Dim sSql As String = String.Empty
        lblName.Text = "MODIFY ACCOUNT"
        BtnSave.Visible = True
        BtnDelete.Text = "&Delete"
        BtnSave.Text = "&Update"
        Dim Deactivate As String = String.Empty
        '  Panel1.BackColor = Color.PaleVioletRed
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Accounts ac inner join AccountGroup grp on ac.GroupID=grp.ID where ac.id=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            tmpid = id
            txtName.Text = ds.Tables("a").Rows(0)("AccountName").ToString().Trim
            cbGroup.Text = ds.Tables("a").Rows(0)("GroupName").ToString()
            cbDrCr.Text = ds.Tables("a").Rows(0)("DC").ToString()
            txtOPBal.Text = Format(Val(ds.Tables("a").Rows(0)("OpBal").ToString()), "0.00")
            txtOtherName.Text = ds.Tables("a").Rows(0)("OtherName").ToString()
            txtAddress.Text = ds.Tables("a").Rows(0)("address").ToString()
            txtLf.Text = ds.Tables("a").Rows(0)("LFNo").ToString()
            txtArea.Text = ds.Tables("a").Rows(0)("Area").ToString()
            txtCity.Text = ds.Tables("a").Rows(0)("City").ToString()
            txtState.Text = ds.Tables("a").Rows(0)("State").ToString()
            txtPhone.Text = ds.Tables("a").Rows(0)("Phone").ToString()
            txtContact.Text = ds.Tables("a").Rows(0)("Contact").ToString()
            txtMob1.Text = ds.Tables("a").Rows(0)("Mobile1").ToString()
            txtMob2.Text = ds.Tables("a").Rows(0)("Mobile2").ToString()
            txtMail.Text = ds.Tables("a").Rows(0)("MailID").ToString()
            txtBank.Text = ds.Tables("a").Rows(0)("BankName").ToString()
            txtACNo.Text = ds.Tables("a").Rows(0)("AccNo").ToString()
            txtIfsc.Text = ds.Tables("a").Rows(0)("IFSC").ToString()
            txtGName.Text = ds.Tables("a").Rows(0)("GName").ToString()
            txtGmob.Text = ds.Tables("a").Rows(0)("Gmobile1").ToString()
            txtGMob2.Text = ds.Tables("a").Rows(0)("Gmobile2").ToString()
            TxtGAddress.Text = ds.Tables("a").Rows(0)("Gaddress").ToString()
            TxtGCity.Text = ds.Tables("a").Rows(0)("GCity").ToString()
            txtGState.Text = ds.Tables("a").Rows(0)("Gstate").ToString()
            txtLimit.Text = ds.Tables("a").Rows(0)("Limit").ToString()
            txtCommPer.Text = Val(ds.Tables("a").Rows(0)("CommPer").ToString())
            txtMPer.Text = Val(ds.Tables("a").Rows(0)("Mper").ToString())
            txtRdfPer.Text = Val(ds.Tables("a").Rows(0)("RdfPer").ToString())
            txtTarePer.Text = Val(ds.Tables("a").Rows(0)("TarePer").ToString())
            txtLabourPer.Text = Val(ds.Tables("a").Rows(0)("LabourPer").ToString())
            Deactivate = ds.Tables("a").Rows(0)("Deactivate").ToString()
            txtPostingID.Text = Val(ds.Tables("a").Rows(0)("PostingID").ToString())
            If Val(txtPostingID.Text) <> 0 Then txtAcPosting.Visible = True : lblPost.Visible = True
            txtAcPosting.Text = ds.Tables("a").Rows(0)("postingacname").ToString()
            If Deactivate = "Y" Then ckDeactivate.Checked = True Else ckDeactivate.Checked = False
            txtID.Text = Val(tmpid)
            If Directory.Exists(Application.StartupPath & "\Images") = False Then
                Directory.CreateDirectory(Application.StartupPath & "\Images")
            End If
            If File.Exists(Application.StartupPath & "\Images\" & txtName.text.trim & ".jpg") = True Then
                picCustomer.Load(Application.StartupPath & "\Images\" & txtName.text.trim & ".jpg")
                picCustomer.Update()
            End If
            If File.Exists(Application.StartupPath & "\Images\" & txtName.text.trim & "-Gurronter.jpg") = True Then
                PicGurronter.Load(Application.StartupPath & "\Images\" & txtName.text.trim & "-Gurronter.jpg")
            End If

        End If
    End Sub

    Private Sub UpdateRecord()
        Dim Deactivate As String = String.Empty
        If ckDeactivate.Checked = True Then Deactivate = "Y" Else Deactivate = "N"
        If clsFun.ExecScalarStr("Select count(*)from Accounts where upper(AccountName)=upper('" & txtName.text.trim & "')") > 1 Then
            MsgBox("Account Already Exists...", vbOKOnly, "Access Denied")
            txtName.Focus() : Exit Sub
        End If
        If txtName.text.trim = "" Then
            MsgBox("Account Name is Blank. Please Fill Account Name... ", MsgBoxStyle.Critical, "Empty")
            txtName.Focus()
        Else
            Dim sql As String = "Update Accounts SET AccountName='" & txtName.text.trim.Trim & "',GroupId=" & Val(cbGroup.SelectedValue) & ",DC='" & cbDrCr.Text & "'," _
                                 & " Opbal='" & txtOPBal.Text & "',address='" & txtAddress.Text & "',LFNo='" & txtLf.Text & "',OtherName='" & txtOtherName.Text.Trim & "'," _
                                 & " Area='" & txtArea.Text.Trim & "',city='" & txtCity.Text.Trim & "',State='" & txtState.Text.Trim & "',Phone='" & txtPhone.Text & "'," _
                                 & " Mobile1='" & txtMob1.Text & "',Mobile2='" & txtMob2.Text & "',MailID='" & txtMail.Text & "',BankName='" & txtBank.Text & "'," _
                                 & " AccNo='" & txtACNo.Text & "',IFSC='" & txtIfsc.Text & "',Gname='" & txtGName.Text & "'," _
                                 & " GMobile1='" & txtGmob.Text & "',GMobile2='" & txtGMob2.Text & "',Gaddress='" & TxtGAddress.Text & "',GCity='" & TxtGCity.Text & "'," _
                                 & " Gstate='" & txtGState.Text & "',CommPer='" & Val(txtCommPer.Text) & "',Mper='" & Val(txtMPer.Text) & "',RdfPer='" & Val(txtRdfPer.Text) & "' " _
                                 & ",TarePer='" & Val(txtTarePer.Text) & "',LabourPer='" & Val(txtLabourPer.Text) & "',Deactivate='" & Deactivate & "',POSTINGID='" & Val(txtPostingID.Text) & "',PostingACName='" & txtAcPosting.Text & "'  WHERE ID=" & Val(txtID.Text) & ""
            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    ServerDbAdd()
                    '            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                    Account_List.btnRetrive.PerformClick()
                    saveImage() 'RetriveAccounts()
                    AccountNameValue = txtName.text.trim : AccountID = Val(txtID.Text)
                    ClearText() : If OpenedFromItems Then Me.Close() : Exit Sub
                    Me.Alert("Update Alert", msgAlert.enmType.Update)
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub cbGroup_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then txtOPBal.Focus()
    End Sub


    Private Sub cbGroup_SelectedIndexChanged(sender As Object, e As EventArgs)
        cbDrCr.Text = clsFun.ExecScalarStr(" Select DC FROM AccountGroup WHERE GroupName like '%" + cbGroup.Text + "%'")
    End Sub
    Private Sub txtGState_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then BtnSave.Focus()
    End Sub
    Private Sub Delete()
        If clsFun.ExecScalarInt("Select count(*) from Ledger where AccountID=" & Val(txtID.Text) & "") <> 0 Then
            MsgBox("Account Already Used in Transactions", vbOKOnly, "Access Denied")
            Exit Sub
        ElseIf clsFun.ExecScalarInt("Select count(*) from Purchase where AccountID=" & Val(txtID.Text) & "") <> 0 Then
            MsgBox("Account Already Used in Purchase...", vbOKOnly, "Access Denied")
            Exit Sub
        ElseIf clsFun.ExecScalarInt("Select count(*) from CrateVoucher where AccountID=" & Val(txtID.Text) & "") <> 0 Then
            MsgBox("Account Already Used in Crates...", vbOKOnly, "Access Denied")
            Exit Sub
        Else
            Try
                If clsFun.ExecScalarInt("Select tag From Accounts  WHERE ID=" & Val(txtID.Text) & "") = 0 Then MsgBox("Pre-Define Master. You Can't Delete it.", vbOKOnly, "Access Denied") : Exit Sub
                If MessageBox.Show("Are you Sure want to Delete Account : " & txtName.text.trim & " ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                    If clsFun.ExecNonQuery("DELETE from Accounts WHERE ID=" & Val(txtID.Text) & "") > 0 Then
                        ClsFunserver.ExecScalarStr("Update Accounts Set ServerTag=0 Where ID='" & Val(txtID.Text) & "' and OrgID='" & Val(OrgID) & "'")
                        ClearText()
                        Me.Alert("Deleted Successful...", msgAlert.enmType.Delete)
                        If Application.OpenForms().OfType(Of Account_List).Any = True Then Account_List.FillWithNevigation()
                        'MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")

                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If BtnDelete.Text = "&Reset" Then
            ClearText()
        ElseIf BtnDelete.Text = "&Delete" Then
            Delete()
        End If

    End Sub

    Private Sub txtOtherName_GotFocus(sender As Object, e As EventArgs) Handles txtOtherName.GotFocus
        Try
            SendKeys.Send("+%")
        Catch ex As InvalidOperationException
            ' Do nothing
        End Try
        'If Language = "Hindi" Then
        '    translation.ChangeKeyboardLayout(translation.KLID_HINDI)
        'ElseIf Language = "Gujarati" Then
        '    translation.ChangeKeyboardLayout(translation.KLID_GUJARATI)
        'ElseIf Language = "Punjabi" Then
        '    translation.ChangeKeyboardLayout(translation.KLID_PUNJABI)
        'ElseIf Language = "Marathi" Then
        '    translation.ChangeKeyboardLayout(translation.KLID_MARATHI)
        'ElseIf Language = "Tamil" Then
        '    translation.ChangeKeyboardLayout(translation.KLID_TAMIL)
        'ElseIf Language = "Telugu" Then
        '    translation.ChangeKeyboardLayout(translation.KLID_TELUGU)
        'ElseIf Language = "Bengali" Then
        '    translation.ChangeKeyboardLayout(translation.KLID_BENGALI)
        'Else
        '    translation.ChangeKeyboardLayout(translation.KLID_ENGLISH)
        'End If
    End Sub
    Private Sub txtName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtName.KeyPress, txtLimit.KeyPress,
     txtOtherName.KeyPress, txtLf.KeyPress, txtAddress.KeyPress,
     txtArea.KeyPress, txtCity.KeyPress, txtState.KeyPress, txtPhone.KeyPress, txtContact.KeyPress, txtMob1.KeyPress,
     txtMob2.KeyPress, txtMail.KeyPress, txtBank.KeyPress, txtACNo.KeyPress, txtIfsc.KeyPress, txtGName.KeyPress,
     txtGmob.KeyPress, txtGMob2.KeyPress, TxtGAddress.KeyPress, TxtGCity.KeyPress, txtGState.KeyPress
        If e.KeyChar = "'"c Then
            ' Agar haan, to event ko handle karke usko prevent kar dein
            e.Handled = True
        End If
    End Sub

    Private Sub txtName_GotFocus(sender As Object, e As EventArgs) Handles txtName.GotFocus, txtLimit.GotFocus,
     txtOPBal.GotFocus, txtOtherName.GotFocus, txtLf.GotFocus, txtAddress.GotFocus,
     txtArea.GotFocus, txtCity.GotFocus, txtState.GotFocus, txtPhone.GotFocus, txtContact.GotFocus, txtMob1.GotFocus,
     txtMob2.GotFocus, txtMail.GotFocus, txtBank.GotFocus, txtACNo.GotFocus, txtIfsc.GotFocus, txtGName.GotFocus,
     txtGmob.GotFocus, txtGMob2.GotFocus, TxtGAddress.GotFocus, TxtGCity.GotFocus, txtGState.GotFocus, txtAcPosting.GotFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.PaleTurquoise
        tb.ForeColor = Color.Maroon
        tb.SelectAll()
    End Sub

    Private Sub txtName_LostFocus(sender As Object, e As EventArgs) Handles txtName.LostFocus, txtLimit.LostFocus,
 txtOPBal.LostFocus, txtOtherName.LostFocus, txtLf.LostFocus, txtAddress.LostFocus,
 txtArea.LostFocus, txtCity.LostFocus, txtState.LostFocus, txtPhone.LostFocus, txtContact.LostFocus, txtMob1.LostFocus,
 txtMob2.LostFocus, txtMail.LostFocus, txtBank.LostFocus, txtACNo.LostFocus, txtIfsc.LostFocus, txtGName.LostFocus,
 txtGmob.LostFocus, txtGMob2.LostFocus, TxtGAddress.LostFocus, TxtGCity.LostFocus, txtGState.LostFocus, txtAcPosting.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        ' tb.BackColor = SystemColors.Control
        tb.BackColor = Color.FromArgb(247, 220, 111)
        tb.ForeColor = Color.Black
        ' tb.SelectAll()
    End Sub

    Private Sub txtName_KeyDown1(sender As Object, e As KeyEventArgs) Handles txtName.KeyDown, txtLimit.KeyDown,
  cbGroup.KeyDown, txtOPBal.KeyDown, cbDrCr.KeyDown, txtOtherName.KeyDown, txtLf.KeyDown, txtAddress.KeyDown,
        txtArea.KeyDown, txtCity.KeyDown, txtState.KeyDown, txtPhone.KeyDown, txtContact.KeyDown, txtMob1.KeyDown,
        txtMob2.KeyDown, txtMail.KeyDown, txtBank.KeyDown, txtACNo.KeyDown, txtIfsc.KeyDown, txtGName.KeyDown,
        txtGmob.KeyDown, txtGMob2.KeyDown, TxtGAddress.KeyDown, TxtGCity.KeyDown, txtGState.KeyDown, txtAcPosting.KeyDown

        If e.KeyCode = Keys.Enter Then
            If txtAcPosting.Focused = True Then
                If txtAcPosting.Text.Trim <> Nothing Then
                    txtPostingID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
                    txtAcPosting.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
                    DgAccountSearch.Visible = False
                    txtBank.Focus() : Exit Sub
                Else
                    txtPostingID.Text = 0 : DgAccountSearch.Visible = False
                End If
            End If
        End If
        If e.KeyCode = Keys.Down Then
            If txtAcPosting.Focused = True Then DgAccountSearch.Focus()

        End If

        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.PerformClick()
                Me.Close()
        End Select
    End Sub

    Private Sub cbGroup_Leave1(sender As Object, e As EventArgs) Handles cbGroup.Leave
        Dim GroupID As Integer = clsFun.ExecScalarInt("Select (case When UnderGroupID=0 then GroupID else UnderGroupID end) from Account_AcGrp where (Groupid in(" & Val(cbGroup.SelectedValue) & ")  or UnderGroupID in (" & Val(cbGroup.SelectedValue) & "))")
        If Val(GroupID) = 17 Or Val(GroupID) = 33 Then
            txtAcPosting.Visible = True : lblPost.Visible = True
        Else
            txtAcPosting.Visible = False : lblPost.Visible = False
        End If

    End Sub

    Private Sub cbGroup_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles cbGroup.SelectedIndexChanged
        cbDrCr.Text = clsFun.ExecScalarStr(" Select DC FROM AccountGroup WHERE GroupName like '%" + cbGroup.Text + "%'")

    End Sub

    Private Sub CreateAccount_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub btnCustSelect_Click(sender As Object, e As EventArgs) Handles btnCustSelect.Click
        OpenFileDialog1.FileName = String.Empty
        OpenFileDialog1.Filter = "Image Files (*.png *.jpg *.bmp) |*.png; *.jpg; *.bmp|All Files(*.*) |*.*"
        Me.OpenFileDialog1.ShowDialog()
        picCustomer.BackgroundImageLayout = ImageLayout.Center
        Dim fs As FileStream
        If OpenFileDialog1.FileName <> "" Then
            fs = File.OpenRead(OpenFileDialog1.FileName)
            picCustomer.Image = Image.FromFile(OpenFileDialog1.FileName)
            GurImagePath = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub btnGurSelect_Click(sender As Object, e As EventArgs) Handles btnGurSelect.Click
        OpenFileDialog1.FileName = String.Empty
        OpenFileDialog1.Filter = "Image Files (*.png *.jpg *.bmp) |*.png; *.jpg; *.bmp|All Files(*.*) |*.*"
        Me.OpenFileDialog1.ShowDialog()
        PicGurronter.BackgroundImageLayout = ImageLayout.Center
        Dim fs As FileStream
        If OpenFileDialog1.FileName <> "" Then
            fs = File.OpenRead(OpenFileDialog1.FileName)
            PicGurronter.Image = Image.FromFile(OpenFileDialog1.FileName)
            GurImagePath = OpenFileDialog1.FileName
        End If
    End Sub



    Private Sub txtName_Leave(sender As Object, e As EventArgs) Handles txtName.Leave
        If txtName.text.trim <> txtName.text.trim.ToUpper Then
            txtName.Text = Trim(StrConv(txtName.Text.Trim, VbStrConv.ProperCase))
        End If
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarStr("Select count(*) from Accounts where upper(AccountName)=upper('" & txtName.text.trim.Trim & "')") = 1 Then
                MsgBox("Account Already Exists...", vbOKOnly, "Access Denied") : txtName.Focus() : Exit Sub
            End If
        Else
            If clsFun.ExecScalarStr("Select count(*) from Accounts where upper(AccountName)=upper('" & txtName.text.trim.Trim & "')") > 1 Then
                MsgBox("Account Already Exists...", vbOKOnly, "Access Denied") : txtName.Focus() : Exit Sub
            End If
        End If
    End Sub

    Private Sub cbDrCr_Leave(sender As Object, e As EventArgs) Handles cbDrCr.Leave
        'PnlTranslate.Visible = True
        'txtTranslate.Focus()
    End Sub

    Private Sub cbDrCr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbDrCr.SelectedIndexChanged

    End Sub

    Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        If BtnSave.Text = "&Save" Then
            txtOtherName.Text = txtName.text.trim
        End If
        ' txtTranslate.Text = txtName.text.trim
    End Sub
    Private Sub txtOtherName_Leave(sender As Object, e As EventArgs) Handles txtOtherName.Leave
        Try
            SendKeys.Send("+%")
        Catch ex As InvalidOperationException
            ' Do nothing
        End Try
        ' translation.ChangeKeyboardLayout(translation.KLID_ENGLISH)
    End Sub

    Private Sub txtOtherName_TextChanged(sender As Object, e As EventArgs) Handles txtOtherName.TextChanged

    End Sub

    Private Sub btnAccountList_Click(sender As Object, e As EventArgs) Handles btnAccountList.Click
        Account_List.MdiParent = MainScreenForm
        Account_List.Show()
        If Not Account_List Is Nothing Then
            Account_List.BringToFront()
        End If
    End Sub

    Private Sub txtAddress_Leave(sender As Object, e As EventArgs) Handles txtAddress.Leave
        If txtAddress.Text <> txtAddress.Text.ToUpper Then
            txtAddress.Text = StrConv(txtAddress.Text, VbStrConv.ProperCase)
        End If

    End Sub

    Private Sub txtAddress_TextChanged(sender As Object, e As EventArgs) Handles txtAddress.TextChanged

    End Sub

    Private Sub txtCity_Leave(sender As Object, e As EventArgs) Handles txtCity.Leave
        If txtCity.Text <> txtCity.Text.ToUpper Then
            txtCity.Text = StrConv(txtCity.Text, VbStrConv.ProperCase)
        End If

    End Sub

    Private Sub txtCity_TextChanged(sender As Object, e As EventArgs) Handles txtCity.TextChanged

    End Sub

    Private Sub txtState_Leave(sender As Object, e As EventArgs) Handles txtState.Leave
        If txtState.Text <> txtState.Text.ToUpper Then
            txtState.Text = StrConv(txtState.Text, VbStrConv.ProperCase)
        End If

    End Sub

    Private Sub txtGName_Leave(sender As Object, e As EventArgs) Handles txtGName.Leave
        If txtGName.Text <> txtGName.Text.ToUpper Then
            txtGName.Text = StrConv(txtGName.Text, VbStrConv.ProperCase)
        End If
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        picCustomer.Image = Nothing
    End Sub

    Private Sub btnGRemove_Click(sender As Object, e As EventArgs) Handles btnGRemove.Click
        PicGurronter.Image = Nothing
    End Sub

    Private Sub AccountRowColumns()

        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 230
        DgAccountSearch.Columns(2).Name = "Group" : DgAccountSearch.Columns(2).Width = 100
        retriveAccounts()
    End Sub

    Private Sub txtAcPosting_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAcPosting.KeyPress

    End Sub

    Private Sub txtAcPosting_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAcPosting.KeyUp
        If DgAccountSearch.Visible = False Then DgAccountSearch.Visible = True : DgAccountSearch.BringToFront()
        If DgAccountSearch.RowCount = 0 Then AccountRowColumns()
        If txtAcPosting.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAcPosting.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub

    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(17)  or UnderGroupID in (17))" & condtion & " order by AccountName Limit 11")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("GroupName").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub txtMob2_TextChanged(sender As Object, e As EventArgs) Handles txtMob2.TextChanged

    End Sub

    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Back Then txtAcPosting.Focus()
        If e.KeyCode = Keys.Enter Then
            If DgAccountSearch.SelectedRows.Count = 0 Then txtBank.Focus() : txtPostingID.Text = 0 : txtAcPosting.Text = "" : Exit Sub
            txtPostingID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            txtAcPosting.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False
            txtBank.Focus() : Exit Sub
        End If
    End Sub

    Private Sub txtOPBal_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOPBal.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub
End Class