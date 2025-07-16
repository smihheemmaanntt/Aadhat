Imports System.Data.OleDb

Public Class Account_List
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 20 : Dim Offset As Integer = 0
    Dim SearchText As String = ""
    Private Sub Account_List_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.Escape Then
            If PnlExport.Visible = True Then
                PnlExport.Visible = False
            ElseIf PnlImport.Visible = True Then
                PnlImport.Visible = False
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Account_List_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True : txtSearch.Focus()
        cbMoreSearch.SelectedIndex = 0
        'clsFun.FillDropDownList(cmbSearch, "Select * From Accounts", "AccountName", "Id", "")
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name"
        dg1.Columns(1).Width = 250
        dg1.Columns(2).Name = "Group"
        dg1.Columns(2).Width = 250
        dg1.Columns(3).Name = "LF No."
        dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Balance"
        dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Area"
        dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "City"
        dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "State"
        dg1.Columns(7).Width = 150
        dg1.Columns(8).Name = "Mobile"
        dg1.Columns(8).Width = 120
        FillWithNevigation() '  retrive()
    End Sub
    Private Sub calc()
        txtpagebal.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtpagebal.Text = Format(Val(txtpagebal.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
        Next
    End Sub
    Public Sub FillWithNevigation(Optional ByVal condtion As String = "")
        If dg1.ColumnCount = 0 Then rowColums()
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) From Accounts ac inner join AccountGroup grp on ac.GroupID=grp.ID Where ac.Tag=1  " & condtion & "")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        dt = clsFun.ExecDataTable("Select * from Accounts ac inner join AccountGroup grp on ac.GroupID=grp.ID Where ac.Tag=1 " & condtion & " Order By AccountName limit " + RowCount.ToString() + " OFFSET " + Offset.ToString() + "")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    If ckShowAllAccounts.Checked = True Then
                        Application.DoEvents()
                    End If
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("GroupName").ToString()
                        '   .Cells(3).Value = dt.Rows(i)("LFNo").ToString()
                        .Cells(4).Value = Format(Val(dt.Rows(i)("OpBal").ToString()), "0.00") & " " & clsFun.ExecScalarStr("Select DC From Accounts Where ID= " & dt.Rows(i)("id").ToString() & "")
                        .Cells(5).Value = dt.Rows(i)("Area").ToString()
                        .Cells(6).Value = dt.Rows(i)("City").ToString()
                        .Cells(7).Value = dt.Rows(i)("State").ToString()
                        .Cells(8).Value = dt.Rows(i)("Mobile1").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        txtTotal.Text = Format(Val(clsFun.ExecScalarStr("Select sum(Opbal) From Accounts")), "0.00")
        calc() : dg1.ClearSelection()
        lblTotalPage.Text = "Total Pages : " & TotalPages
        lblCurrentpage.Text = "Current Pages : " & (Offset / RowCount) + 1
        lblTotalAccounts.Text = "Total Count : " & recordsCount
        lblTotalCount.Text = "Record Count : " & dg1.RowCount
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Accounts ac inner join AccountGroup grp on ac.GroupID=grp.ID Where ac.Tag in (1,0 )" & condtion & " Order By AccountName")
        If dt.Rows.Count > 20 Then dg1.Columns(8).Width = 105
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    Application.DoEvents()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("GroupName").ToString()
                        '   .Cells(3).Value = dt.Rows(i)("LFNo").ToString()
                        .Cells(4).Value = Format(Val(dt.Rows(i)("OpBal").ToString()), "0.00") & " " & clsFun.ExecScalarStr("Select DC From Accounts Where ID= " & dt.Rows(i)("id").ToString() & "")
                        .Cells(5).Value = dt.Rows(i)("Area").ToString()
                        .Cells(6).Value = dt.Rows(i)("City").ToString()
                        .Cells(7).Value = dt.Rows(i)("State").ToString()
                        .Cells(8).Value = dt.Rows(i)("Mobile1").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
        lblTotalCount.Visible = True
        lblTotalCount.Text = "Record Count : " & dg1.RowCount
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                ' btnShow.PerformClick()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpID)
            CreateAccount.BringToFront()
            CreateAccount.txtName.SelectAll()
            e.SuppressKeyPress = True
        End If

        If e.KeyCode = Keys.Up Then
            If dg1.CurrentCell.RowIndex = 0 Then
                txtSearch.Focus()
                dg1.ClearSelection()
                e.SuppressKeyPress = True
            End If
        End If
        If e.KeyCode = Keys.Down Then
            ' Agar current row last row hai to TextBox par focus karein
            If dg1.CurrentCell.RowIndex = dg1.RowCount - 1 Then
                txtSearch.Focus()
                dg1.ClearSelection()
                e.SuppressKeyPress = True ' Ye event ko stop karta hai taaki aur kuch na ho
            End If
        End If
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        CreateAccount.MdiParent = MainScreenForm
        CreateAccount.Show()
        CreateAccount.FillContros(tmpID)
        CreateAccount.BringToFront()
        CreateAccount.txtName.SelectAll()
    End Sub
    Private Sub btnRetrive_Click(sender As Object, e As EventArgs) Handles btnRetrive.Click
        retrive()
        SpeedSale.Refresh()
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) values('" & .Cells(1).Value & "'," & _
                    "'" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "'," & _
                    "'" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & txtpagebal.Text & "','" & txtTotal.Text & "')"
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
        Report_Viewer.printReport("\AccountList.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CreateAccount.MdiParent = MainScreenForm
        CreateAccount.Show()
        If Not CreateAccount Is Nothing Then
            CreateAccount.BringToFront()
        End If
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtSearch.Text.Trim() <> "" Then
                If ckShowAllAccounts.Checked = True Then
                    retrive(" and accountname Like '" & txtSearch.Text.Trim() & "%'")
                Else
                    FillWithNevigation(" and accountname Like '" & txtSearch.Text.Trim() & "%'")
                End If

            Else
                If ckShowAllAccounts.Checked = True Then
                    retrive()
                Else
                    FillWithNevigation()
                End If
            End If
        End If
        If e.KeyCode = Keys.Down Then
            If dg1.RowCount = 0 Then Exit Sub
            dg1.Focus()
            dg1.CurrentCell = dg1.Rows(0).Cells(1) ' Pehli row ke pehle column par focus
            dg1.Rows(0).Selected = True
        End If
    End Sub

    Private Sub txtSearch_TextChanged_1(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

    End Sub

    Private Sub txtSearcGroup_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearcGroup.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtSearcGroup.Text.Trim() <> "" Then
                If ckShowAllAccounts.Checked = True Then
                    retrive(" and GroupName Like '" & txtSearcGroup.Text.Trim() & "%'")
                Else
                    FillWithNevigation(" and GroupName Like '" & txtSearcGroup.Text.Trim() & "%'")
                End If

            Else
                If ckShowAllAccounts.Checked = True Then
                    retrive()
                Else
                    FillWithNevigation()
                End If
            End If
        End If
        If e.KeyCode = Keys.Down Then dg1.Focus()
    End Sub

    Private Sub txtSearcGroup_TextChanged(sender As Object, e As EventArgs) Handles txtSearcGroup.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label42_Click(sender As Object, e As EventArgs) Handles Label42.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If PnlExport.Visible = True Then PnlExport.Visible = True
        PnlImport.Visible = True
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If PnlImport.Visible = True Then PnlImport.Visible = False
        PnlExport.Visible = True
        radioExportAll.Checked = True
        ''export all Accounts Where Tag
    End Sub

    Private Sub btnSelectFile_Click(sender As Object, e As EventArgs) Handles btnSelectFile.Click
        ' Dim connExcel As OleDbConnection
        Dim cmdExcel As OleDbCommand = New OleDbCommand()
        Dim oda As OleDbDataAdapter = New OleDbDataAdapter()
        'Dim importrecod As Long
        'Dim unimportrecod As Long
        Try
            Dim OpenFileDialog As OpenFileDialog = New OpenFileDialog()
            OpenFileDialog.Filter = "Excel Files | *.xlsx; *.xls;| All Files (*.*)| *.*"
            If (OpenFileDialog.ShowDialog() = DialogResult.OK And OpenFileDialog.FileName <> "") Then
                txtpath.Text = OpenFileDialog.FileName
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub btnimportcont_Click(sender As Object, e As EventArgs) Handles btnimportcont.Click
        ImportAccounts()
    End Sub
    Private Sub ImportAccounts()
        Dim connExcel As OleDbConnection = Nothing
        Dim cmdExcel As OleDbCommand = New OleDbCommand()
        Dim oda As OleDbDataAdapter = New OleDbDataAdapter()
        Dim importrecod As Long
        Dim unimportrecod As Long
        Dim i As Integer
        Try
            If (txtpath.Text <> "") Then
                Me.Cursor = Cursors.WaitCursor
                Dim pathname As String = txtpath.Text
                connExcel = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathname + ";Extended Properties=Excel 8.0;")
                cmdExcel.Connection = connExcel
                connExcel.Open()
                Dim dtExcelSchema As DataTable
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
                cmdExcel.CommandText = "Select * From [" & SheetName & "]"
                Dim dt As DataTable = New DataTable()
                oda.SelectCommand = cmdExcel
                oda.Fill(dt)

                Dim ssql As String = ""
                Dim j As Integer = 0
                pbImport.Value = 0
                pbImport.Minimum = 0
                pbImport.Maximum = dt.Rows.Count + 1
                pbImport.Visible = True

                Dim existingAccounts As New HashSet(Of String) ' Local Duplicate Check

                For j = 0 To dt.Rows.Count - 1
                    Application.DoEvents()
                    Dim accountName As String = dt.Rows(j)("AccountName").ToString().Trim()

                    ' ✅ Excel ke andar bhi duplicate check karein
                    If existingAccounts.Contains(accountName) Then
                        unimportrecod += 1
                        Continue For
                    End If

                    ' ✅ Database ke andar duplicate check karein
                    If (clsFun.ExecScalarInt("Select Count(*) from Accounts where AccountName='" & accountName & "'")) = 0 Then
                        With dt
                            Dim grpid As Integer = clsFun.ExecScalarInt("Select Id from AccountGroup where upper(GroupName)='" & .Rows(j)("Group Name").ToString().ToUpper() & "'")
                            ssql = "Insert into Accounts(AccountName,GroupID,DC,OpBal,OtherName,LFNo,Area,address,City,State,Phone,Contact,Mobile1,Mobile2,MailID,BankName,AccNo, " & _
                                "IFSC,GName,Gmobile1,Gmobile2,Gaddress,GCity,Gstate,Tag)" & _
                                " Values('" & accountName & "'," & grpid & ",'" & .Rows(j)("DC").ToString() & "','" & .Rows(j)("OPBal").ToString() & "'," & _
                                "  '" & .Rows(j)("OtherName").ToString() & "','" & .Rows(j)("LFNo").ToString() & "','" & .Rows(j)("Area").ToString() & "','" & .Rows(j)("Address").ToString() & "','" & .Rows(j)("City").ToString() & "'," & _
                                " '" & .Rows(j)("State").ToString() & "','" & .Rows(j)("Phone").ToString() & "','" & .Rows(j)("Contact").ToString() & "','" & .Rows(j)("Mobile1").ToString() & "'," & _
                                "'" & .Rows(j)("Mobile2").ToString() & "','" & .Rows(j)("MailID").ToString() & "','" & .Rows(j)("BankName").ToString() & "','" & .Rows(j)("AccNo").ToString() & "'," & _
                                "'" & .Rows(j)("IFSC").ToString() & "','" & .Rows(j)("Gname").ToString() & "','" & .Rows(j)("Gmobile1").ToString() & "','" & .Rows(j)("Gmobile2").ToString() & "'," & _
                                "'" & .Rows(j)("Gaddress").ToString() & "','" & .Rows(j)("Gcity").ToString() & "','" & .Rows(j)("GState").ToString() & "',1);"
                        End With
                        clsFun.ExecNonQuery(ssql)
                        importrecod += 1
                        existingAccounts.Add(accountName) ' ✅ Duplicate ko avoid karne ke liye HashSet me add karein
                        lblMSg.Text = "Accounts Imported : " & importrecod
                    Else
                        unimportrecod += 1
                    End If
                    pbImport.Value = importrecod + 1
                    lblMSg.Visible = True
                Next

                If importrecod > 0 Then
                    lblMSg.Text = "Please wait.... Accounts Managing....."
                    If clsFun.ExecNonQuery(ssql) > 0 Then
                        MsgBox("Accounts Successfully Imported" & vbCrLf & "Total Imported: " & importrecod & vbCrLf & "Unimported Records: " & unimportrecod)
                        pbImport.Value = 0
                        retrive()
                        lblMSg.Text = "Enjoy.... Accounts Imported"
                        PnlImport.Visible = False
                    End If
                End If
            End If
            connExcel.Close()
            oda.Dispose()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    'Private Sub ImportAccounts()
    '    Dim connExcel As OleDbConnection = Nothing
    '    Dim cmdExcel As OleDbCommand = New OleDbCommand()
    '    Dim oda As OleDbDataAdapter = New OleDbDataAdapter()
    '    Dim importrecod As Long
    '    Dim unimportrecod As Long
    '    Dim i As Integer
    '    Try
    '        If (txtpath.Text <> "") Then
    '            Me.Cursor = Cursors.WaitCursor
    '            Dim pathname As String = txtpath.Text
    '            connExcel = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathname + ";Extended Properties=Excel 8.0;")
    '            cmdExcel.Connection = connExcel
    '            connExcel.Open()
    '            Dim dtExcelSchema As DataTable
    '            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
    '            Dim SheetName As String = dtExcelSchema.Rows(i)("TABLE_NAME").ToString()
    '            cmdExcel.CommandText = "Select * From [" & SheetName & "]"
    '            Dim dt As DataTable = New DataTable()
    '            oda.SelectCommand = cmdExcel
    '            oda.Fill(dt)
    '            'If dt.Columns.Count <> 21 Then
    '            '    MsgBox("Invalid Sheet")
    '            '    connExcel.Close()
    '            '    Exit Sub
    '            'Else
    '            Dim ssql As String = ""
    '            Dim j As Integer = 0
    '            pbImport.Value = 0
    '            pbImport.Minimum = 0
    '            pbImport.Maximum = dt.Rows.Count + 1
    '            pbImport.Visible = True
    '            For j = 0 To dt.Rows.Count - 1
    '                Application.DoEvents()
    '                If (clsFun.ExecScalarInt("Select Count(*) from Accounts where AccountName='" & dt.Rows(j)("AccountName").ToString() & "'")) = 0 Then
    '                    With dt
    '                        Dim grpid As Integer = clsFun.ExecScalarInt("Select Id from AccountGroup where upper(GroupName)='" & .Rows(j)("Group Name").ToString().ToUpper() & "'")
    '                        ssql = "Insert into Accounts(AccountName,GroupID,DC,OpBal,OtherName,LFNo,Area,address,City,State,Phone,Contact,Mobile1,Mobile2,MailID,BankName,AccNo, " &
    '                            "IFSC,GName,Gmobile1,Gmobile2,Gaddress,GCity,Gstate,Tag)" & _
    '                            " Values('" & .Rows(j)("AccountName").ToString() & "'," & grpid & ",'" & .Rows(j)("DC").ToString() & "','" & .Rows(j)("OPBal").ToString() & "'," & _
    '                            "  '" & .Rows(j)("OtherName").ToString() & "','" & .Rows(j)("LFNo").ToString() & "','" & .Rows(j)("Area").ToString() & "','" & .Rows(j)("Address").ToString() & "','" & .Rows(j)("City").ToString() & "'," & _
    '                            " '" & .Rows(j)("State").ToString() & "','" & .Rows(j)("Phone").ToString() & "','" & .Rows(j)("Contact").ToString() & "','" & .Rows(j)("Mobile1").ToString() & "'," & _
    '                            "'" & .Rows(j)("Mobile2").ToString() & "','" & .Rows(j)("MailID").ToString() & "','" & .Rows(j)("BankName").ToString() & "','" & .Rows(j)("AccNo").ToString() & "'," & _
    '                            "'" & .Rows(j)("IFSC").ToString() & "','" & .Rows(j)("Gname").ToString() & "','" & .Rows(j)("Gmobile1").ToString() & "','" & .Rows(j)("Gmobile2").ToString() & "'," & _
    '                            "'" & .Rows(j)("Gaddress").ToString() & "','" & .Rows(j)("Gcity").ToString() & "','" & .Rows(j)("GState").ToString() & "',1);"
    '                    End With
    '                    clsFun.ExecNonQuery(ssql)
    '                    importrecod = importrecod + 1
    '                    lblMSg.Text = "Accounts Imported : " & importrecod
    '                Else
    '                    unimportrecod = unimportrecod + 1
    '                End If
    '                pbImport.Value = importrecod + 1
    '                ' pbImport.Visible = False
    '                lblMSg.Visible = True

    '            Next
    '            If importrecod > 0 Then
    '                lblMSg.Text = "Please wait.... Accounts Manageing....."
    '                If clsFun.ExecNonQuery(ssql) > 0 Then
    '                    MsgBox("Accounts Succsfully Imported" & vbCrLf & "Total imported " & importrecod & vbCrLf & " Unimported Record " & unimportrecod)
    '                    pbImport.Value = 0
    '                    retrive()
    '                    lblMSg.Text = "Enjoy.... Accounts Imported"
    '                    PnlImport.Visible = False
    '                End If
    '            End If

    '        End If
    '        ' End If
    '        connExcel.Close()
    '        oda.Dispose()
    '        Me.Cursor = Cursors.Default
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '        '   connExcel.Close()
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub
    Private Sub btnExportAccounts_Click(sender As Object, e As EventArgs) Handles btnExportAccounts.Click
        Dim sSql As String = ""
        sSql = "Select AccountName,   ag.GroupName as [Group Name],ac.DC as [DC],ac.Tag as Tag, OpBal,OtherName, address, LFNo, Area, City, State, Phone, Contact, " &
                "Mobile1, Mobile2, MailID, BankName, AccNo,IFSC,GName,Gmobile1,Gmobile2,Gaddress,GCity,Gstate FROM Accounts ac inner join AccountGroup ag" & _
            " on ac.GroupID=ag.ID where 1=1"

        If RadioExportDebtors.Checked = True Then
            sSql = sSql & " and ag.GroupName ='Sundry Debtors' or   ag.GroupName 'Customer' "
        End If

        If RadioExportCrediors.Checked = True Then
            sSql = sSql & " and ag.GroupName ='Sundry Creditors' or   ag.GroupName ='Supplier' "
        End If
        Me.Cursor = Cursors.WaitCursor
        DatatableToExcel(clsFun.ExecDataTable(sSql))
        MsgBox("Record Successfully exported")
        PnlExport.Visible = False
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub DatatableToExcel(ByVal dtTemp As DataTable)
        Dim _excel As New Microsoft.Office.Interop.Excel.Application
        Dim wBook As Microsoft.Office.Interop.Excel.Workbook
        Dim wSheet As Microsoft.Office.Interop.Excel.Worksheet

        wBook = _excel.Workbooks.Add()
        wSheet = wBook.ActiveSheet()

        Dim dt As System.Data.DataTable = dtTemp
        Dim dc As System.Data.DataColumn
        Dim dr As System.Data.DataRow
        Dim colIndex As Integer = 0
        Dim rowIndex As Integer = 0

        For Each dc In dt.Columns
            Application.DoEvents()
            colIndex = colIndex + 1
            _excel.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr In dt.Rows
            Application.DoEvents()
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc In dt.Columns
                colIndex = colIndex + 1
                _excel.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next
        wSheet.Columns.AutoFit()
        Dim strFileName As String = Application.StartupPath & "\Accountlist.xlsx"
        If System.IO.File.Exists(strFileName) Then
            System.IO.File.Delete(strFileName)
        End If
        wBook.SaveAs(strFileName)
        wBook.Close()
        _excel.Quit()
    End Sub
    Private Sub btnImportClose_Click(sender As Object, e As EventArgs) Handles btnImportClose.Click
        PnlImport.Visible = False
    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles btnExportClose.Click
        PnlExport.Visible = False
    End Sub
    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtMoreSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtMoreSearch.KeyUp
        If txtMoreSearch.Text.Trim() <> "" Then
            If cbMoreSearch.SelectedIndex = 0 Then
                retrive(" and Area Like '" & txtMoreSearch.Text.Trim() & "%'")
            ElseIf cbMoreSearch.SelectedIndex = 1 Then
                retrive(" and City Like '" & txtMoreSearch.Text.Trim() & "%'")
            ElseIf cbMoreSearch.SelectedIndex = 2 Then
                retrive(" and State Like '" & txtMoreSearch.Text.Trim() & "%'")
            ElseIf cbMoreSearch.SelectedIndex = 3 Then
                retrive(" and Mobile1 Like '" & txtMoreSearch.Text.Trim() & "%'")
            ElseIf cbMoreSearch.SelectedIndex = 4 Then
                retrive(" and Mobile2 Like '" & txtMoreSearch.Text.Trim() & "%'")
            ElseIf cbMoreSearch.SelectedIndex = 5 Then
                retrive(" and Opbal Like '" & txtMoreSearch.Text.Trim() & "%'")
            ElseIf cbMoreSearch.SelectedIndex = 6 Then
                retrive(" and ac.DC Like '" & txtMoreSearch.Text.Trim() & "%'")
            ElseIf cbMoreSearch.SelectedIndex = 7 Then
                retrive(" and ac.Tag Like '" & txtMoreSearch.Text.Trim() & "%'")
            End If
        Else
            retrive()
        End If
    End Sub

    Private Sub ckShowAllAccounts_CheckedChanged(sender As Object, e As EventArgs) Handles ckShowAllAccounts.CheckedChanged
        retrive()
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        FillWithNevigation(SearchText)
    End Sub
    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Offset -= RowCount
        If Offset <= 0 Then
            Offset = 0
        End If
        FillWithNevigation(SearchText)
    End Sub
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim currentPage As Integer = (Offset / RowCount) + 1
        If currentPage <> TotalPages Then
            Offset += RowCount
        End If
        FillWithNevigation(SearchText)
    End Sub
    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        Offset = (TotalPages - 1) * RowCount
        FillWithNevigation(SearchText)
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
End Class