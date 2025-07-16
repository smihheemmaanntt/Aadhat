Public Class RateMaster

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub

    Private Sub RateMaster_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub RateMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0 'System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        clsFun.FillDropDownList(CbAccountName, "Select ID,AccountName FROM Account_AcGrp Where (Groupid in(16,17)  or UnderGroupID in (16,17)) Order by Upper(AccountName)", "Accountname", "ID", "--Choose Account--")
        Me.KeyPreview = True : RowColums()
    End Sub
    Private Sub RowColums()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "SR No." : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "Item Name" : dg1.Columns(2).Width = 400
        dg1.Columns(3).Name = "Hind Name" : dg1.Columns(3).Width = 400
        dg1.Columns(4).Name = "Special Rate" : dg1.Columns(4).Width = 200
        dg1.Columns(1).ReadOnly = True : dg1.Columns(2).ReadOnly = True
        dg1.Columns(3).ReadOnly = True
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        Dim currentCell = dg1.CurrentCell

        ' Handle Enter key

        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True ' Prevent the default behavior

            Dim nextRowIndex = currentCell.RowIndex + 1

            If nextRowIndex < dg1.Rows.Count Then
                dg1.CurrentCell = dg1.Rows(nextRowIndex).Cells(4)

            Else
                dg1.ClearSelection() : btnSave.Visible = True : SendKeys.Send("{TAB}")
                ' MessageBox.Show("End")
            End If

            ' Handle Arrow keys and Tab key
        ElseIf e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Tab Then
            e.SuppressKeyPress = True ' Prevent the default behavior

            If e.KeyCode = Keys.Up Then
                If currentCell.RowIndex > 0 Then
                    dg1.CurrentCell = dg1.Rows(currentCell.RowIndex - 1).Cells(4)
                End If
            ElseIf e.KeyCode = Keys.Down Then
                If currentCell.RowIndex < dg1.Rows.Count - 1 Then
                    dg1.CurrentCell = dg1.Rows(currentCell.RowIndex + 1).Cells(4)
                End If

            ElseIf e.KeyCode = Keys.Tab Then
                If currentCell.RowIndex < dg1.Rows.Count - 1 Then
                    dg1.CurrentCell = dg1.Rows(currentCell.RowIndex + 1).Cells(4)
                Else
                    dg1.ClearSelection() : btnSave.Visible = True : SendKeys.Send("{TAB}")
                    'MessageBox.Show("End")
                End If
            End If
        ElseIf e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Right Then
            If currentCell.RowIndex >= 0 AndAlso currentCell.ColumnIndex >= 0 Then
                dg1.CurrentCell = dg1.Rows(currentCell.RowIndex).Cells(4)
            End If
        End If
    End Sub

    Private Sub dg1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dg1.CellBeginEdit
        If e.ColumnIndex <> 4 Then
            e.Cancel = True ' Make sure only the "Special Rate" column is editable
        End If
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        ' Set the current cell to the "Special Rate" column if a row is clicked
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            dg1.CurrentCell = dg1.Rows(e.RowIndex).Cells(4)
        End If
    End Sub

    Private Sub dg1_SelectionChanged(sender As Object, e As EventArgs) Handles dg1.SelectionChanged
        ' Ensure the selected cell is always in the "Special Rate" column
        If dg1.SelectedRows.Count > 0 Then
            dg1.CurrentCell = dg1.SelectedRows(0).Cells(4)
        End If
    End Sub
    'Private Sub Retriveitems(Optional ByVal condtion As String = "")
    '    dg1.Rows.Clear()
    '    Dim dt As New DataTable
    '    dt = clsFun.ExecDataTable("Select ID,ItemName,Othername from Items Order by Upper(ItemName)  ")
    '    Try
    '        If dt.Rows.Count > 0 Then
    '            dg1.Rows.Clear()
    '            For i = 0 To dt.Rows.Count - 1
    '                dg1.Rows.Add()
    '                With dg1.Rows(i)
    '                    .Cells(4).ReadOnly = False
    '                    .Cells(0).Value = dt.Rows(i)("id").ToString()
    '                    .Cells(1).Value = i + 1
    '                    .Cells(2).Value = dt.Rows(i)("ItemName").ToString()
    '                    .Cells(3).Value = dt.Rows(i)("OtherName").ToString()
    '                End With
    '                ' Dg1.Rows.Add()
    '            Next
    '        End If
    '        dt.Dispose()
    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
    '    End Try
    '    lblCount.Text = "Total Items : " & dt.Rows.Count
    '    dg1.ClearSelection()
    'End Sub

    Private Sub Retrieveitems(Optional ByVal condition As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select ID, ItemName, OtherName from Items Order by Upper(ItemName)")

        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                Dim accountId As String = CbAccountName.SelectedValue.ToString()
                Dim ratesDt As New DataTable

                For i = 0 To dt.Rows.Count - 1
                    ' Get ItemID from the current row
                    Dim itemId As String = dt.Rows(i)("ID").ToString()

                    ' Query to get the rate for the current item and selected account
                    Dim rateQuery As String = "SELECT Rate FROM PartyRates WHERE AccountID = '" & Val(accountId) & "' AND ItemID = '" & Val(itemId) & "'"
                    ratesDt = clsFun.ExecDataTable(rateQuery)

                    ' Get the rate if it exists
                    Dim rate As String = If(ratesDt.Rows.Count > 0, ratesDt.Rows(0)("Rate").ToString(), String.Empty)

                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(4).ReadOnly = False
                        .Cells(0).Value = itemId
                        .Cells(1).Value = i + 1
                        .Cells(2).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(3).Value = dt.Rows(i)("OtherName").ToString()
                        .Cells(4).Value = If(Val(rate) > 0, Format(Val(rate), "0.00"), "")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try

        lblCount.Text = "Total Items: " & dt.Rows.Count
        dg1.ClearSelection()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub CbAccountName_KeyDown(sender As Object, e As KeyEventArgs) Handles CbAccountName.KeyDown

        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32", "GroupName", "ID", "")
            CreateAccount.BringToFront()
        End If
        If e.KeyCode = Keys.F1 Then
            Dim AccountID As String = Val(CbAccountName.SelectedValue)
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show() : CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            CreateAccount.FillContros(AccountID)
            CreateAccount.BringToFront()
        End If
    End Sub

    Private Sub btnSave_GotFocus(sender As Object, e As EventArgs) Handles btnSave.GotFocus
        btnSave.BackColor = Color.Maroon
    End Sub
    Private Sub btnSave_KeyDown(sender As Object, e As KeyEventArgs) Handles btnSave.KeyDown, CbAccountName.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True ' Prevent the default behavior
            If sender Is CbAccountName Then
                If CbAccountName.SelectedValue = 0 Then dg1.Rows.Clear() : CbAccountName.Focus() : Exit Sub
                Retrieveitems()
                ' Set focus to the "Special Rate" column of the first row
                If dg1.Rows.Count > 0 Then
                    dg1.CurrentCell = dg1.Rows(0).Cells(4)
                    dg1.Focus()
                End If
            ElseIf sender Is btnSave Then
                If e.KeyCode = Keys.Enter Then
                    SendKeys.Send("{TAB}")
                    e.SuppressKeyPress = True
                End If
                If e.KeyCode = Keys.End Then
                    btnSave.Visible = True
                    btnSave.Focus()
                    e.SuppressKeyPress = True
                End If
                ' Handle Enter key for btnSave if needed
                ' For example, save data or other actions
            End If
        End If
    End Sub
    Private Sub dg1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellEndEdit
        If e.ColumnIndex = 4 Then
            Dim cellValue As Object = dg1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

            If cellValue IsNot Nothing AndAlso IsNumeric(cellValue) Then
                dg1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(CDec(cellValue), "0.00")
            End If
        End If
    End Sub
    Private Sub dg1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dg1.EditingControlShowing
        Dim tb As TextBox = CType(e.Control, TextBox)
        RemoveHandler tb.KeyPress, AddressOf TextBox_KeyPress
        AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
    End Sub

    Private Sub TextBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' Allow only numbers, decimal point, and control characters
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso (e.KeyChar <> "."c) Then
            e.Handled = True
        End If

        ' Allow only one decimal point
        Dim tb As TextBox = CType(sender, TextBox)
        If (e.KeyChar = "."c) AndAlso (tb.Text.IndexOf("."c) > -1) Then
            e.Handled = True
        End If
    End Sub
    
    Private Sub btnSave_LostFocus(sender As Object, e As EventArgs) Handles btnSave.LostFocus
        btnSave.BackColor = Color.Brown
    End Sub
    Private Sub SaveRecord()
        If CbAccountName.SelectedValue = 0 Then CbAccountName.Focus() : Exit Sub
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim count As Integer = 0
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If Val(.Cells(4).Value) <> 0 Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(CbAccountName.SelectedValue) & "," &
                                "'" & CbAccountName.Text & "','" & Val(.Cells(0).Value) & "','" & .Cells(2).Value & "'," & Val(.Cells(4).Value) & ""
                    count += 1
                End If
            End With
        Next
        Try
            If count = 0 Then MsgBox("Here is No Record to Save") : Exit Sub
            clsFun.ExecNonQuery("Delete From PartyRates Where AccountID='" & Val(CbAccountName.SelectedValue) & "'")
            Sql = "INSERT INTO PartyRates (AccountID,AccountName,ItemID,ItemName,Rate) " & FastQuery & ""
            If FastQuery = String.Empty Then Exit Sub
            If clsFun.ExecNonQuery(Sql) > 0 Then MsgBox(CbAccountName.Text & " Rates Are Updated Successufully...")
            ClearColumn4() : dg1.ClearSelection() : CbAccountName.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        clsFun.CloseConnection()
    End Sub
    Private Sub ClearColumn4()
        Dim rows = dg1.Rows.Cast(Of DataGridViewRow)().ToArray()
        For Each row As DataGridViewRow In rows
            row.Cells(4).Value = Nothing
        Next
    End Sub



    Private Sub CbAccountName_Leave(sender As Object, e As EventArgs) Handles CbAccountName.Leave
        'If CbAccountName.SelectedValue = 0 Then dg1.ClearSelection() : CbAccountName.Focus() : Exit Sub
        'Retrieveitems() : dg1.CurrentCell = dg1.Rows(0).Cells(4)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveRecord()
    End Sub
End Class