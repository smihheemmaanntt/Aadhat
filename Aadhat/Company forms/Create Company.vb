Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Xml

Public Class Create_Company
    Dim rs As New Resizer
    'Private Shared CompData As String = String.Empty
    'Dim sql As String = clsFun.ExecScalarStr("Select CompData from Company")
    Dim ClsCommon As CommonClass = New CommonClass()

    Private Sub Create_Company_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Create_Company_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        rs.FindAllControls(Me) : Me.BackColor = Color.GhostWhite
        Me.Top = 0 : Me.Left = 0 : Me.KeyPreview = True
        ClsFunPrimary.FillDropDownList(cbState, "Select * From StateList", "StateName", "Id", "")
        cbState.SelectedIndex = 22
        'clsFun.ChangePath(Sql)
        Dim currentYear As Integer = DateTime.Now.Year
        If DateTime.Now.Month < 4 Then
            currentYear -= 1
        End If
        mskFYStart.Text = "01-04-" & currentYear.ToString()
        MskFYEnd.Text = "31-03-" & (currentYear + 1).ToString()
    End Sub
    Public Sub FillContros(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        'clsFun.changeCompany()
        BtnSave.Text = "&Update"
        BtnSave.Image = My.Resources.Edit
        BtnSave.BackColor = Color.Coral
        lblPath.Visible = True : txtPath.Visible = True
        lblLinked.Visible = True : txtPrvPath.Visible = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Company where id=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtID.Text = ds.Tables("a").Rows(0)("ID").ToString()
            '  mskFYStart.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtCompanyName.Text = ds.Tables("a").Rows(0)("CompanyName").ToString()
            txtPrintCompanyName.Text = ds.Tables("a").Rows(0)("PrintOtherName").ToString()
            txtAddress.Text = ds.Tables("a").Rows(0)("Address").ToString()
            txtPrintAddress.Text = ds.Tables("a").Rows(0)("PrintOtherAddress").ToString()
            txtCity.Text = ds.Tables("a").Rows(0)("City").ToString()
            txtprintCity.Text = ds.Tables("a").Rows(0)("PrintOtherCity").ToString()
            cbState.Text = ds.Tables("a").Rows(0)("State").ToString()
            txtPrintState.Text = ds.Tables("a").Rows(0)("PrintOtherState").ToString()
            txtMobile1.Text = ds.Tables("a").Rows(0)("MobileNo1").ToString()
            txtMobile2.Text = ds.Tables("a").Rows(0)("MobileNo2").ToString()
            txtPhone.Text = ds.Tables("a").Rows(0)("PhoneNo").ToString()
            txtFax.Text = ds.Tables("a").Rows(0)("FaxNo").ToString()
            txtEmal.Text = ds.Tables("a").Rows(0)("EMailID").ToString()
            txtWebsite.Text = ds.Tables("a").Rows(0)("Website").ToString()
            txtGSTN.Text = ds.Tables("a").Rows(0)("GSTN").ToString()
            txtDelasIn.Text = ds.Tables("a").Rows(0)("DealsIn").ToString()
            txtReg.Text = ds.Tables("a").Rows(0)("RegistrationNo").ToString()
            txtPan.Text = ds.Tables("a").Rows(0)("PanNo").ToString()
            txtMarka.Text = ds.Tables("a").Rows(0)("Marka").ToString()
            txtother.Text = ds.Tables("a").Rows(0)("Other").ToString()
            mskFYStart.Text = CDate(ds.Tables("a").Rows(0)("YearStart")).ToString("dd-MM-yyyy")
            MskFYEnd.Text = CDate(ds.Tables("a").Rows(0)("YearEnd")).ToString("dd-MM-yyyy")
            txtPath.Text = ds.Tables("a").Rows(0)("CompData").ToString()
            txtPrvPath.Text = ds.Tables("a").Rows(0)("LinkedDb").ToString()
        End If
    End Sub

    Private Sub txtPrintCompanyName_GotFocus(sender As Object, e As EventArgs) Handles txtPrintCompanyName.GotFocus, txtprintCity.GotFocus, txtPrintAddress.GotFocus, txtPrintState.GotFocus
        Try
            SendKeys.Send("+%")
        Catch ex As InvalidOperationException
            ' Do nothing
        End Try
    End Sub
    Private Sub txtPrintCompanyName_Leave(sender As Object, e As EventArgs) Handles txtPrintCompanyName.Leave, txtprintCity.Leave, txtPrintAddress.Leave, txtPrintState.Leave
        Try
            SendKeys.Send("+%")
        Catch ex As InvalidOperationException
            ' Do nothing
        End Try
    End Sub

    Private Sub txtCompanyName_GotFocus(sender As Object, e As EventArgs) Handles txtCompanyName.GotFocus, txtAddress.GotFocus, txtCity.GotFocus,
    txtMobile1.GotFocus, txtMobile2.GotFocus, txtPhone.GotFocus, txtFax.GotFocus, txtEmal.GotFocus, txtWebsite.GotFocus, txtGSTN.GotFocus,
    txtDelasIn.GotFocus, txtReg.GotFocus, txtPan.GotFocus, txtMarka.GotFocus, txtother.GotFocus, txtPrintAddress.GotFocus, txtPrintCompanyName.GotFocus,
    txtPrintState.GotFocus, txtprintCity.GotFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.Orange
        ' If tb IsNot Nothing Then tb.BorderStyle = BorderStyle.FixedSingle
        tb.SelectAll()
    End Sub


    Private Sub txtCompanyName_LostFocus(sender As Object, e As EventArgs) Handles txtCompanyName.LostFocus, txtAddress.LostFocus, txtCity.LostFocus,
     txtMobile1.LostFocus, txtMobile2.LostFocus, txtPhone.LostFocus, txtFax.LostFocus, txtEmal.LostFocus, txtWebsite.LostFocus, txtGSTN.LostFocus,
    txtDelasIn.LostFocus, txtReg.LostFocus, txtPan.LostFocus, txtMarka.LostFocus, txtother.LostFocus, txtPrintAddress.LostFocus, txtPrintCompanyName.LostFocus,
    txtPrintState.LostFocus, txtprintCity.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        ' If tb IsNot Nothing Then tb.BorderStyle = BorderStyle.None
        tb.BackColor = Color.GhostWhite
    End Sub

    Private Sub mskFYStart_GotFocus(sender As Object, e As EventArgs) Handles mskFYStart.GotFocus
        mskFYStart.SelectAll()
    End Sub

    Private Sub MskFYEnd_GotFocus(sender As Object, e As EventArgs) Handles MskFYEnd.GotFocus
        MskFYEnd.SelectAll()
    End Sub

    Private Sub txtCompanyName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCompanyName.KeyDown, txtAddress.KeyDown, txtCity.KeyDown,
        cbState.KeyDown, txtMobile1.KeyDown, txtMobile2.KeyDown, txtPhone.KeyDown, txtFax.KeyDown, txtEmal.KeyDown, txtWebsite.KeyDown, txtGSTN.KeyDown,
        txtDelasIn.KeyDown, txtReg.KeyDown, txtPan.KeyDown, txtMarka.KeyDown, txtother.KeyDown, mskFYStart.KeyDown, MskFYEnd.KeyDown, txtPrintAddress.KeyDown,
        txtPrintCompanyName.KeyDown, txtPrintState.KeyDown, txtprintCity.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
        If Not File.Exists(fileName) Then
            If BtnSave.Text = "&Save" Then
                If CompanyList.dg1.RowCount >= 1 Then
                    MessageBox.Show("Sorry... Activation Required For Multiple Companies.", "Activation Required...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Exit Sub
                End If
            Else
                If CompanyList.dg1.RowCount > 1 Then
                    MessageBox.Show("Sorry... Activation Required For Multiple Companies.", "Activation Required...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Exit Sub
                End If
            End If
        End If
        If BtnSave.Text = "&Save" Then
            ' Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
            If Not File.Exists(fileName) Then
                SaveCompanyInfo()
            Else
                If ClsCommon.IsInternetConnect Then
                    SaveCompanyInfo()
                    ClsCommon.UpdateCutomerInfo("ADD")
                Else
                    MessageBox.Show("Sorry... Your System is not Connected to Internet. Please check internet connection...", "Internet Required...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Exit Sub
                End If
            End If

        Else
            If Not File.Exists(fileName) Then
                UpdateRecord()
            Else
                If ClsCommon.IsInternetConnect Then
                    UpdateRecord()
                    ClsCommon.UpdateCutomerInfo("EDIT")
                Else
                    MessageBox.Show("Sorry... Your System is not Connected to Internet. Please check internet connection...", "Internet Required...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Exit Sub
                End If
            End If
        End If
    End Sub
    Private Sub UpdateRecord()
        Dim dt As DateTime
        Dim dt1 As DateTime
        dt = CDate(Me.mskFYStart.Text)
        dt1 = CDate(Me.MskFYEnd.Text)
        StartDate = dt.ToString("yyyy-MM-dd")
        EndDate = dt1.ToString("yyyy-MM-dd")
        StartDate = dt.ToString("yyyy-MM-dd")
        EndDate = dt1.ToString("yyyy-MM-dd")
        If txtCompanyName.Text = "" Then
            MsgBox("Please fill Company Name", MsgBoxStyle.Critical, "Empty")
            txtCompanyName.Focus() : Exit Sub
        End If
        Dim sql As String = "Update Company SET CompanyName='" & txtCompanyName.Text.Trim & "',PrintOtherName='" & txtPrintCompanyName.Text.Trim & "',State='" & cbState.Text.Trim & "'," _
                             & " Address='" & txtAddress.Text.Trim & "',PrintOtherAddress='" & txtPrintAddress.Text.Trim & "',City='" & txtCity.Text.Trim & "',PrintOtherCity='" & txtprintCity.Text.Trim & "'," _
                             & " PrintOtherState='" & txtPrintState.Text.Trim & "',MobileNo1='" & txtMobile1.Text & "',MobileNo2='" & txtMobile2.Text & "',PhoneNo='" & txtPhone.Text & "'," _
                             & " FaxNo='" & txtFax.Text & "',EmailID='" & txtEmal.Text & "',Website='" & txtWebsite.Text & "',GSTN='" & txtGSTN.Text & "'," _
                             & " DealsIn='" & txtDelasIn.Text & "',RegistrationNo='" & txtReg.Text & "',PanNo='" & txtPan.Text & "',Marka='" & txtMarka.Text & "'," _
                             & " Other='" & txtother.Text & "',YearStart='" & StartDate & "',YearEnd='" & EndDate & "'," _
                             & " Compdata='" & txtPath.Text & "',LinkedDb='" & txtPrvPath.Text & "'   WHERE ID=" & Val(txtID.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                sCompCode = txtID.Text
                ' MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                BtnSave.Text = "&Save"
                CompanyList.BtnRetrive.PerformClick()
                Panel1.BackColor = Color.Teal
                ' clsFun.ChangePath(sql)
                Me.Close()
                Me.Alert("Update Alert", msgAlert.enmType.Info)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Shared Sub CopyDirectory(ByVal sourcePath As String, ByVal destPath As String)
        'If Not Directory.Exists(destPath) Then
        '    Directory.CreateDirectory(destPath)
        'End If
        Dim CompData As String = String.Empty
        For Each file__1 As String In Directory.GetFiles(Path.GetDirectoryName(sourcePath))
            Dim dest As String = Path.Combine(destPath, Path.GetFileName(file__1))
            File.Copy(file__1, dest)
            CompData = CompData & file__1
        Next

        For Each folder As String In Directory.GetDirectories(Path.GetDirectoryName(sourcePath))
            Dim dest As String = Path.Combine(destPath, Path.GetFileName(folder))
            CopyDirectory(folder, dest)
        Next
    End Sub
    Private Sub SaveCompanyInfo()
        Dim StartDate As String = CDate(Me.mskFYStart.Text).ToString("yyyy-MM-dd")
        Dim EndDate As String = CDate(Me.MskFYEnd.Text).ToString("yyyy-MM-dd")
        If txtCompanyName.Text = "" Then MsgBox("Company Name Blank of Allowed", MsgBoxStyle.Critical, "Empty Company Name") : txtCompanyName.Focus() : Exit Sub
        Dim CopyDestPath As String = String.Empty
        Dim tmpDataPath As String = String.Empty
        Dim i As Integer = 0
        CopyDestPath = "Data\Data"
        tmpDataPath = "Data"
        For i = 1 To 50
            If Directory.Exists(Application.StartupPath & "\" & CopyDestPath & i) = False Then
                CompData = CopyDestPath & i & "\"
                tmpDataPath = tmpDataPath & i
                tmpDataPath = tmpDataPath & "\Data.db"
                Directory.CreateDirectory(Application.StartupPath & "\" & CopyDestPath & i)
                CopyDestPath = (Application.StartupPath & "\" & CopyDestPath & i).ToString()
                Exit For
            End If
        Next
        CopyDirectory(Application.StartupPath & "\Demo\Data.db", CopyDestPath)
        GlobalData.ConnectionPath = CopyDestPath & "\Data.db"
        Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "insert into Company(CompanyName, Address, City, State, MobileNo1, MobileNo2, PhoneNo, FaxNo," _
                             & " EmailID, Website, GSTN, DealsIN, RegistrationNo, PanNo, marka, other,YearStart,YearEnd,CompData," _
                             & " PrintOtherName,PrintOtheraddress,PrintOtherCity,PrintOtherState)" _
                             & "values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10,@11, @12, @13, @14, @15, @16, @17, @18, @19,@20,@21,@22,@23)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", txtCompanyName.Text.Trim)
            cmd.Parameters.AddWithValue("@2", txtAddress.Text.Trim)
            cmd.Parameters.AddWithValue("@3", txtCity.Text.Trim)
            cmd.Parameters.AddWithValue("@4", cbState.Text.Trim)
            cmd.Parameters.AddWithValue("@5", txtMobile1.Text)
            cmd.Parameters.AddWithValue("@6", txtMobile2.Text)
            cmd.Parameters.AddWithValue("@7", txtPhone.Text)
            cmd.Parameters.AddWithValue("@8", txtFax.Text)
            cmd.Parameters.AddWithValue("@9", txtEmal.Text)
            cmd.Parameters.AddWithValue("@10", txtWebsite.Text)
            cmd.Parameters.AddWithValue("@11", txtGSTN.Text)
            cmd.Parameters.AddWithValue("@12", txtDelasIn.Text)
            cmd.Parameters.AddWithValue("@13", txtReg.Text)
            cmd.Parameters.AddWithValue("@14", txtPan.Text)
            cmd.Parameters.AddWithValue("@15", txtMarka.Text)
            cmd.Parameters.AddWithValue("@16", txtother.Text)
            cmd.Parameters.AddWithValue("@17", StartDate)
            cmd.Parameters.AddWithValue("@18", EndDate)
            cmd.Parameters.AddWithValue("@19", tmpDataPath)
            cmd.Parameters.AddWithValue("@20", txtPrintCompanyName.Text.Trim)
            cmd.Parameters.AddWithValue("@21", txtPrintAddress.Text.Trim)
            cmd.Parameters.AddWithValue("@22", txtprintCity.Text.Trim)
            cmd.Parameters.AddWithValue("@23", txtPrintState.Text.Trim)
            If cmd.ExecuteNonQuery() > 0 Then
                '   MessageBox.Show("Record Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
                sCompCode = clsFun.ExecScalarStr("Select max(ID) From Company")
                CompanyList.BtnRetrive.PerformClick()
                Me.Close()
                Me.Alert("Success Alert", msgAlert.enmType.Info)
            End If
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Public Sub Alert(ByVal msg As String, ByVal type As msgAlert.enmType)
        Dim frm As msgAlert = New msgAlert()
        frm.showAlert(msg, type)
    End Sub
    Sub changeCompany(ByVal Data As String)
        Try
            'Constructing connection string from the inputs
            Dim Con As New StringBuilder("")
            Con.Append("Data Source=|DataDirectory|\")
            Con.Append(Data & ";Version=3;New=True;Compress=True;synchronous=ON;")
            Dim strCon As String = Con.ToString()
            updateConfigFile(strCon)
            'Create new sql connection
            Dim Db As New SQLite.SQLiteConnection()
            'to refresh connection string each time else it will use             previous connection string
            ConfigurationManager.RefreshSection("connectionStrings")
            Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
            clsFun.ConStr = Db.ConnectionString

        Catch E As Exception
            MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
        End Try

    End Sub
    Public Sub updateConfigFile(con As String)
        'updating config file
        Dim XmlDoc As New XmlDocument()
        'Loading the Config file
        XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        For Each xElement As XmlElement In XmlDoc.DocumentElement
            If xElement.Name = "connectionStrings" Then
                'setting the coonection string
                xElement.FirstChild.Attributes(1).Value = con
            End If
        Next
        'writing the connection string in config file
        XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
    End Sub
    Private Sub SaveFinacialYear()
        Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "insert into FinancialYear(YearStart, YearEnd)values (@1, @2)"
        ' & "@21)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", mskFYStart.Text)
            cmd.Parameters.AddWithValue("@2", MskFYEnd.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                MessageBox.Show("Record Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CompanyList.BtnRetrive.PerformClick()
            End If
            ' MsgBox("Successfully Inserted")
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub


    Private Sub txtCompanyName_Leave(sender As Object, e As EventArgs) Handles txtCompanyName.Leave
        If txtCompanyName.Text <> txtCompanyName.Text.ToUpper Then
            txtCompanyName.Text = StrConv(txtCompanyName.Text, VbStrConv.ProperCase)
        End If
    End Sub


    Private Sub txtAddress_Leave(sender As Object, e As EventArgs) Handles txtAddress.Leave
        If txtAddress.Text <> txtAddress.Text.ToUpper Then
            txtAddress.Text = StrConv(txtAddress.Text, VbStrConv.ProperCase)
        End If
    End Sub

    Private Sub txtCity_Leave(sender As Object, e As EventArgs) Handles txtCity.Leave
        If txtCity.Text <> txtCity.Text.ToUpper Then
            txtCity.Text = StrConv(txtCity.Text, VbStrConv.ProperCase)
        End If
    End Sub

    Private Sub Create_Company_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtPrintCompanyName_TextChanged(sender As Object, e As EventArgs) Handles txtPrintCompanyName.TextChanged

    End Sub

    Private Sub txtCity_TextChanged(sender As Object, e As EventArgs) Handles txtCity.TextChanged

    End Sub

    Private Sub txtPrintCompanyName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrintCompanyName.KeyPress

    End Sub

    Private Sub txtCompanyName_TextChanged(sender As Object, e As EventArgs) Handles txtCompanyName.TextChanged

    End Sub

    Private Sub mskFYStart_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFYStart.MaskInputRejected

    End Sub
End Class