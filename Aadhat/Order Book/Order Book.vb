Public Class Order_Book

    Private Sub Order_Book_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub CreateTble()
        Dim sql As String = String.Empty
        sql = "CREATE TABLE if not exists OrderBook (ID INTEGER PRIMARY KEY AUTOINCREMENT,OrderNo " & _
            "TEXT,EntryDate DATE,AccountID INTEGER,AccountName TEXT,TotalNug DECIMAL,TotalWeight DECIMAL);"
        clsFun.ExecNonQuery(sql)
    End Sub
    Private Sub Order_Book_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CreateTble()
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums() : VNumber()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub VNumber()
        Vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Orderbook")
        txtVoucherNo.Text = Vno + 1
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "S.No" : dg1.Columns(1).Width = 93
        dg1.Columns(2).Name = "Itemid" : dg1.Columns(2).Visible = False
        dg1.Columns(3).Name = "Item Name" : dg1.Columns(3).Width = 500
        dg1.Columns(4).Name = "Nug" : dg1.Columns(4).Width = 287
        dg1.Columns(5).Name = "Kg" : dg1.Columns(5).Width = 295
    End Sub
    Private Sub ItemRowColumns()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 544
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 245
        retriveItems()
    End Sub
    Private Sub retriveItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Items " & condtion & " order by ItemName")
        Try
            If dt.Rows.Count > 0 Then
                dgItemSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgItemSearch.Rows.Add()
                    With dgItemSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(2).Value = dt.Rows(i)("OtherName").ToString()
                    End With
                Next

            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        If txtAccount.Text.ToUpper <> DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        If txtAccount.Text.ToUpper = DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtItemID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False
            txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
            '  txtAccount.Focus()
        Else
            txtAccount.Focus()
        End If
    End Sub
    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        ItemRowColumns()
        If txtItem.Text.Trim() <> "" Then
            dgItemSearch.Visible = True
            retriveItems(" Where upper(ItemName) Like upper('%" & txtItem.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
    End Sub

    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
        ItemRowColumns()
        dgItemSearch.BringToFront()
        dgItemSearch.Visible = True
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        dgItemSearch.Visible = False
        txtNug.Focus()
    End Sub



    Private Sub txtAccount_Click(sender As Object, e As EventArgs) Handles txtAccount.Click
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 180
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 150
        retriveAccounts()
    End Sub
    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.BringToFront()
        ' AccountRowColumns()
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        AccountRowColumns()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('%" & txtAccount.Text.Trim() & "%')")
        End If
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        If ckShowSupplier.Checked Then
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16,17)  or UnderGroupID in (11,16,17))" & condtion & " order by AccountName")
        Else
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))" & condtion & " order by AccountName")
        End If

        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("City").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            Dim AcID As String = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(AcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
    End Sub
    Private Sub txtItem_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItem.KeyDown
        If e.KeyCode = Keys.F3 Then
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            Dim ItemID As String = dgItemSearch.SelectedRows(0).Cells(0).Value
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            Item_form.FillContros(ItemID)
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Down Then dgItemSearch.Focus()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown,
        txtAccount.KeyDown, txtItem.KeyDown, txtNug.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus()
        End Select
    End Sub

    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear() : txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        txtItem.Focus()
    End Sub

    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtItem.Clear() : txtItemID.Clear()
            txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            dgItemSearch.Visible = False
            txtNug.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtItem.Focus()
    End Sub

    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False
            txtItem.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtWeight_KeyDown(sender As Object, e As KeyEventArgs) Handles txtWeight.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then
                dg1.Rows.Add("", dg1.Rows.Count + 1, Val(txtItemID.Text), txtItem.Text, txtNug.Text, txtWeight.Text)
            Else
                ' dg1.SelectedRows(0).Cells(1).Value = dg1.Rows.Count + 1
                dg1.SelectedRows(0).Cells(2).Value = txtItemID.Text
                dg1.SelectedRows(0).Cells(3).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(4).Value = txtNug.Text
                dg1.SelectedRows(0).Cells(5).Value = txtWeight.Text
            End If
            dg1.ClearSelection() : txtItem.Focus() : calc()
        End If
    End Sub
    Private Sub save()
        Dim sql As String = String.Empty
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim cmd As SQLite.SQLiteCommand
        sql = "Insert Into OrderBook(OrderNo,EntryDate, AccountID,AccountName,TotalNug,TotalWeight) Values (@1,@2,@3,@4,@5,@6)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@2", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@3", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@4", txtAccount.Text)
            cmd.Parameters.AddWithValue("@5", txtTotalNugs.Text)
            cmd.Parameters.AddWithValue("@6", txtTotalWeight.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                clsFun.CloseConnection()
            End If
            txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from OrderBook"))
            dg1Record()
            dg1.ClearSelection()
            MsgBox("Order Saved Successfully.", vbInformation + vbOKOnly, "Saved") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            retrive2() : textclear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub textclear()
        dg1.ClearSelection() : dg1.Rows.Clear()
        txtAccount.Clear() : VNumber()
        mskEntryDate.Focus() : BtnSave.Text = "&Save"
    End Sub
    Private Sub UpdateRecord()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        Dim cmd As SQLite.SQLiteCommand
        sql = "Update OrderBook SET OrderNo='" & txtVoucherNo.Text & "',Entrydate='" & SqliteEntryDate & "', " _
               & "AccountID='" & Val(txtAccountID.Text) & "',AccountName='" & txtAccount.Text & "', " _
                & "TotalNug='" & Val(txtTotalNugs.Text) & "',TotalWeight='" & Val(txtTotalWeight.Text) & "' " _
               & "where ID =" & Val(txtid.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        If clsFun.ExecNonQuery(sql) > 0 Then
            clsFun.CloseConnection()
        End If
        If clsFun.ExecNonQuery("DELETE from Orders WHERE OrderID=" & Val(txtid.Text) & "") > 0 Then
            dg1Record()
        End If
        MsgBox("Order Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
        retrive2() : textclear()
    End Sub
    Private Sub dg1Record()
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                Dim ssql As String = "insert into Orders(OrderID, SNo,ItemID, ItemName, Qty, Weight) " &
                                             " VALUES(" & txtid.Text & ",'" & .Cells(1).Value & "','" & .Cells(2).Value & "'," &
                                            " '" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "')"
                cmd = New SQLite.SQLiteCommand(ssql, clsFun.GetConnection())
            End With
            If cmd.ExecuteNonQuery() > 0 Then
                ' MsgBox("Record Inerted")
            End If
        Next
        clsFun.CloseConnection()
    End Sub
    Private Sub DgvAutoSerialNumbering()
        Dim SlNumber As Integer = 0
        If Me.dg1.Rows.Count = 0 Then
            SlNumber = 1
        ElseIf Me.dg1.Rows.Count > 0 Then
            For i As Integer = 0 To Me.dg1.Rows.Count - 1
                SlNumber = Me.dg1.Rows.Count
                Me.dg1.CurrentRow.Cells(1).Value = SlNumber
            Next
        End If
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then
                    sql = sql & "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8)" &
                        "  values('" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "');"
                End If
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try

    End Sub

    Private Sub Delete()
        If MessageBox.Show("Are You Sure Want to Delete ?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from Orderbook WHERE ID=" & Val(txtid.Text) & "") > 0 Then
            End If
            If clsFun.ExecNonQuery("DELETE from Orders WHERE VouchersID=" & Val(txtid.Text) & "") > 0 Then
            End If
            MsgBox("Record Deleted Successfully", vbInformation + vbOKOnly, "Deleted")
            textclear() : DgvAutoSerialNumbering()
        End If
    End Sub
    Sub calc()
        txtTotalNugs.Text = Format(0, "0.00") : txtTotalWeight.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNugs.Text = Format(Val(txtTotalNugs.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtTotalWeight.Text = Format(Val(txtTotalWeight.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
        Next
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnDelete.Enabled = True : btnPrint.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Orderbook where id=" & id
        Dim sql As String = "Select * from Orders where OrderID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds, "b")
        If ds.Tables("a").Rows.Count > 0 Then
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("AccountID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
        End If
        If ds.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells("ID").Value = ds.Tables("b").Rows(i)("OrderID").ToString()
                .Rows(i).Cells("S.No").Value = ds.Tables("b").Rows(i)("SNo").ToString()
                .Rows(i).Cells("Itemid").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells("Item Name").Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells("Nug").Value = Format(Val(ds.Tables("b").Rows(i)("Qty").ToString()), "0.00")
                .Rows(i).Cells("Kg").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
            Next
        End With
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        TempRowColumn()
        If BtnSave.Text = "&Save" Then
            save()
        Else
            UpdateRecord()
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            txtItemID.Text = dg1.SelectedRows(0).Cells(2).Value
            txtItem.Text = dg1.SelectedRows(0).Cells(3).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(4).Value
            txtWeight.Text = dg1.SelectedRows(0).Cells(5).Value
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are You Sure Want to Remove Item ?", "Remove item", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
                DgvAutoSerialNumbering() : BtnDelete.Visible = False
                dg1.ClearSelection() : calc()
                e.SuppressKeyPress = True
            End If
        End If
    End Sub

    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        txtItemID.Text = dg1.SelectedRows(0).Cells(2).Value
        txtItem.Text = dg1.SelectedRows(0).Cells(3).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(4).Value
        txtWeight.Text = dg1.SelectedRows(0).Cells(5).Value
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        retrive2()
        PrintRecord()
        Report_Viewer.printReport("\Order.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 26
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "BillNo" : .Columns(1).Width = 95
            .Columns(2).Name = "EntryDate" : .Columns(2).Width = 159
            .Columns(3).Name = "AccountName" : .Columns(3).Width = 159
            .Columns(4).Name = "TotalNugs" : .Columns(4).Width = 59
            .Columns(5).Name = "TotalWeight" : .Columns(5).Width = 59
            .Columns(6).Name = "AccountHindi" : .Columns(6).Width = 69
            .Columns(7).Name = "Sno" : .Columns(7).Width = 76
            .Columns(8).Name = "ItemName" : .Columns(8).Width = 90
            .Columns(9).Name = "Nugs" : .Columns(9).Width = 86
            .Columns(10).Name = "Weight" : .Columns(10).Width = 90
            .Columns(11).Name = "ItemHindi" : .Columns(11).Width = 50
        End With
    End Sub
    Sub retrive2()
        TempRowColumn()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        '  dt = clsFun.ExecDataTable("Select * From ScripPrint Where (ID=" & txtid.Text & ")")
        dt = clsFun.ExecDataTable(" Select * From orderBook Where ID=" & Val(txtid.Text) & "")
        tmpgrid.Rows.Clear()
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
                    .Cells(1).Value = .Cells(1).Value & dt.Rows(i)("OrderNo").ToString()
                    .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(4).Value = Format(Val(dt.Rows(i)("TotalNug").ToString()), "0.00")
                    .Cells(5).Value = Format(Val(dt.Rows(i)("TotalWeight").ToString()), "0.00")
                    .Cells(6).Value = clsFun.ExecScalarStr("Select OtherName From Accounts  Where ID=" & dt.Rows(i)("AccountID").ToString() & "")
                    dt1 = clsFun.ExecDataTable("Select * FROM Orders WHERE OrderID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(7).Value = .Cells(7).Value & dt1.Rows(j)("SNo").ToString() & vbCrLf
                            .Cells(8).Value = .Cells(8).Value & dt1.Rows(j)("ItemName").ToString() & vbCrLf
                            .Cells(9).Value = .Cells(9).Value & dt1.Rows(j)("Qty").ToString() & vbCrLf
                            .Cells(10).Value = .Cells(10).Value & Format(Val(dt1.Rows(j)("Weight").ToString()), "0.00") & vbCrLf
                            .Cells(11).Value = .Cells(11).Value & clsFun.ExecScalarStr("Select OtherName From Items  Where ID=" & dt1.Rows(j)("ItemID").ToString() & "") & vbCrLf

                        Next
                    Else
                        .Cells(7).Value = ""
                        .Cells(8).Value = ""
                        .Cells(9).Value = ""
                        .Cells(10).Value = ""
                        .Cells(11).Value = ""
                    End If

                End With
                '  End If
            Next
        End If
        dt.Clear()
        dt1.Clear()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
End Class