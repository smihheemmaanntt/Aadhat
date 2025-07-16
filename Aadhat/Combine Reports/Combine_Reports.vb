Imports System.Data.SQLite
Imports System.IO
Public Class Combine_Reports
    Dim root As String = Application.StartupPath
    Private Sub MultiCompaniesLedger_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub MultiCompaniesLedger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0 : Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : rowColums() : getCompanies()
    End Sub
    Public Sub rowColums()
        dg1.ColumnCount = 8
        Dim checkboxColumn As New DataGridViewCheckBoxColumn()
        dg1.Columns.Insert(0, checkboxColumn)
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Company Name" : dg1.Columns(2).Width = 400
        dg1.Columns(3).Name = "Address" : dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "City" : dg1.Columns(4).Visible = False
        dg1.Columns(5).Name = "Year Start" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Year End" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "FYID" : dg1.Columns(7).Visible = False
        dg1.Columns(8).Name = "DbPath" : dg1.Columns(8).Visible = False
    End Sub
    Public Sub getCompanies()
        Dim Connectionstring As String = String.Empty
        dg1.Rows.Clear()
        Dim i As Integer = 1 ' NewCompanies
        If ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path").ToUpper = ("Data").ToUpper Or ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path").ToUpper = "" Then
            root = root
        Else
            root = ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path")
        End If

        For Each sDir In Directory.GetDirectories(root, "Data", SearchOption.AllDirectories)
            For Each FilePath In Directory.GetFiles(sDir, "*data*.db", SearchOption.AllDirectories)
                Application.DoEvents()
                Dim detailedfile As New IO.FileInfo(FilePath)
                Dim dt As New DataTable
                Dim cmdText As String = "Select * from Company"
                If ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path").ToUpper = ("Data").ToUpper Then
                    Connectionstring = "Data Source=|DataDirectory|" & FilePath.ToString & ";Version=3;New=True;Compress=True;synchronous=ON;"
                Else
                    Connectionstring = "Data Source=" & FilePath.ToString & ";Version=3;New=True;Compress=True;synchronous=ON;"
                End If

                Dim con As New SQLite.SQLiteConnection(Connectionstring)
                Dim ad As New SQLiteDataAdapter(cmdText, con)
                ad.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dg1.Rows.Add()
                    With dg1.Rows(i - 1)
                        .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                        .Cells(3).ReadOnly = True : .Cells(4).ReadOnly = True
                        .Cells(5).ReadOnly = True : .Cells(6).ReadOnly = True
                        .Cells(7).ReadOnly = True : .Cells(8).ReadOnly = True
                        .Cells(1).Value = dt.Rows(0)("id").ToString()
                        If FilePath = GlobalData.ConnectionPath Then
                            .Cells(2).Value = dt.Rows(0)("CompanyName").ToString() & " (Current Year)"
                            .Cells(0).Value = True

                        Else
                            .Cells(2).Value = dt.Rows(0)("CompanyName").ToString()

                        End If

                        .Cells(3).Value = dt.Rows(0)("Address").ToString()
                        .Cells(4).Value = dt.Rows(0)("City").ToString()
                        .Cells(5).Value = CDate(dt.Rows(0)("YearStart")).ToString("dd-MM-yyyy")
                        .Cells(6).Value = CDate(dt.Rows(0)("YearEnd")).ToString("dd-MM-yyyy")
                        .Cells(7).Value = dt.Rows(0)("id").ToString()
                        .Cells(8).Value = FilePath.ToString
                    End With
                    i = i + 1
                End If
                '  dg1.Rows.Add("", detailedfile.Name, FilePath)
            Next
        Next
        dg1.ClearSelection()
        ' dg1.EditMode = DataGridViewEditMode.EditProgrammatically
    End Sub





    Private Sub dg1_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellEnter
        If e.ColumnIndex > 0 Then
            BeginInvoke(Sub() dg1.CurrentCell = dg1.Rows(e.RowIndex).Cells(0))
            dg1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End If
    End Sub

    Private Sub dg1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dg1.CellFormatting
        If e.RowIndex >= 0 AndAlso dg1.Rows(e.RowIndex).Cells(0).Value = True Then
            e.CellStyle.BackColor = Color.Orange 'set the background color of the cell to LightGreen
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
    End Sub

    Private Sub dg1_SelectionChanged(sender As Object, e As EventArgs) Handles dg1.SelectionChanged
        If dg1.RowCount = 0 Or dg1.SelectedRows.Count = 0 Then Exit Sub
        txtPath.Text = dg1.SelectedRows(0).Cells(8).Value
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLedgers.Click
        btnLedgers.Text = "PLease Wait"
        Show_Accounts.MdiParent = MainScreenForm
        Show_Accounts.retriveAccounts()
        Show_Accounts.Show()
        btnLedgers.Text = "Get Ledgers"
        Show_Accounts.BringToFront()

    End Sub
End Class