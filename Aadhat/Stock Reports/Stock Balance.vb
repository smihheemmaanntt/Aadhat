Public Class Stock_Balance

    Private Sub Stock_Balance_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.SelectionStart = 0 : mskEntryDate.SelectionLength = Len(mskEntryDate.Text)
    End Sub

    Private Sub Stock_Balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        cbMoreSearch.SelectedIndex = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Stock Holder"
        dg1.Columns(1).Width = 220
        dg1.Columns(2).Name = "Account Name"
        dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Items"
        dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Purchase"
        dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Sold"
        dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Balance"
        dg1.Columns(6).Width = 150
        ' retrive()
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If cbMoreSearch.SelectedIndex = 0 Then
                If txtCustomerSearch.Text.Trim() <> "" Then
                    Retrive("And StockHolderName Like '" & txtCustomerSearch.Text.Trim() & "%'")
                End If
                If txtCustomerSearch.Text.Trim() = "" Then
                    Retrive()
                End If
            ElseIf cbMoreSearch.SelectedIndex = 1 Then
                If txtCustomerSearch.Text.Trim() <> "" Then
                    Retrive("And AccountName Like '" & txtCustomerSearch.Text.Trim() & "%'")
                End If
                If txtCustomerSearch.Text.Trim() = "" Then
                    Retrive()
                End If
            End If
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtItemSearch.Text.Trim() <> "" Then
                Retrive("And ItemName Like '" & txtItemSearch.Text.Trim() & "%'")
            End If
            If txtItemSearch.Text.Trim() = "" Then
                Retrive()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub
    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub Retrive(Optional ByVal condtion As String = "", Optional ByVal condtion1 As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim soldData As New Dictionary(Of String, Integer)

        ' Fetch Purchase Data
        Dim query As String = "SELECT StockHolderID, StockHolderName, StorageName, ItemID, AccountName, ItemName, SUM(nug) AS PurchaseNug " & _
                              "FROM Purchase WHERE EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " " & _
                              "GROUP BY StockHolderID, ItemID ORDER BY StockHolderName, ItemName;"
        dt = clsFun.ExecDataTable(query)

        ' Fetch Sold Data in One Query
        Dim soldQuery As String = "SELECT sallerID, ItemID, SUM(Nug) AS SoldNug FROM Transaction2 " & _
                                  "WHERE Transtype IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
                                  "AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & _
                                  "GROUP BY sallerID, ItemID"
        Dim dtSold As DataTable = clsFun.ExecDataTable(soldQuery)
        ' Store Sold Data in Dictionary for Fast Lookup
        For Each row As DataRow In dtSold.Rows
            Dim key As String = row("sallerID").ToString() & "_" & row("ItemID").ToString()
            soldData(key) = Val(row("SoldNug"))
        Next

        ' Populate DataGridView Efficiently
        If dt.Rows.Count > 0 Then
            dg1.SuspendLayout() ' Temporarily disable UI updates
            For Each row As DataRow In dt.Rows
                Dim stockHolderID As Integer = Val(row("StockHolderID"))
                Dim itemID As Integer = Val(row("ItemID"))
                Dim purchaseNug As Integer = Val(row("PurchaseNug"))
                ' Fetch SoldNug from Dictionary (Avoiding Additional SQL Queries)
                Dim key As String = stockHolderID.ToString() & "_" & itemID.ToString()
                Dim soldNug As Integer = If(soldData.ContainsKey(key), soldData(key), 0)
                Dim restbal As Integer = purchaseNug - soldNug

                If restbal <> 0 Then
                    dg1.Rows.Add(stockHolderID, row("StockHolderName"), row("AccountName"), row("ItemName"), purchaseNug, soldNug, restbal)
                End If
            Next
            dg1.ResumeLayout() ' Re-enable UI updates
        End If

        calc()
        dg1.ClearSelection()
    End Sub


    'Private Sub Retrive(Optional ByVal condtion As String = "", Optional ByVal condtion1 As String = "")
    '    dg1.Rows.Clear()
    '    Dim dt As New DataTable
    '    Dim i As Integer
    '    Dim count As Integer = 0
    '    Dim tmpval As Integer = 0
    '    ssql = "Select StockHolderID,StockHolderName,StorageName,ItemID,AccountName, ItemName,sum(nug) as PurchaseNug From Purchase Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "   Group by StockHolderID,ItemID order by StockHolderName,ItemName;"
    '    dt = clsFun.ExecDataTable(ssql)
    '    If dt.Rows.Count > 0 Then
    '        For i = 0 To dt.Rows.Count - 1
    '            Application.DoEvents()
    '            'If Val(dt.Rows(i)("StockHolderID")) = 1126 Then MsgBox("a")
    '            Dim SoldNug As String = clsFun.ExecScalarStr("Select sum(Nug) as SoldNug,lot from Transaction2 where Transtype in('Stock Sale','On Sale', 'Standard Sale','Store Out') and sallerID=" & Val(dt.Rows(i)("StockHolderID")) & " and ItemID=" & Val(dt.Rows(i)("ItemID")) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '            Dim restbal As Integer = Val(dt.Rows(i)("PurchaseNug").ToString()) - Val(SoldNug)
    '            If restbal <> 0 Then
    '                dg1.Rows.Add()
    '                With dg1.Rows(tmpval)
    '                    Application.DoEvents()
    '                    .Cells(0).Value = dt.Rows(i)("StockHolderID").ToString()
    '                    .Cells(1).Value = dt.Rows(i)("StockHolderName").ToString()
    '                    .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
    '                    .Cells(3).Value = dt.Rows(i)("ItemName").ToString()
    '                    .Cells(4).Value = dt.Rows(i)("PurchaseNug").ToString()
    '                    .Cells(5).Value = Val(SoldNug)
    '                    .Cells(6).Value = restbal 'Val(dt.Rows(i)("PurchaseNug").ToString()) - Val(SoldNug)
    '                    tmpval = tmpval + 1
    '                End With
    '            End If
    '        Next i
    '    End If
    '    calc() : dg1.ClearSelection()
    'End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()
    End Sub
    Sub calc()
        txtTotCrate.Text = Format(0, "0.00") : txtPurchasedNugs.Text = Format(0, "0.00") : txtSoldNugs.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtPurchasedNugs.Text = Format(Val(txtPurchasedNugs.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtSoldNugs.Text = Format(Val(txtSoldNugs.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotCrate.Text = Format(Val(txtTotCrate.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
        Next
    End Sub
    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,P1, P2,P3, P4, P5, P6,P7,P8,P9) values('" & mskEntryDate.Text & "','" & .Cells(1).Value & "'," & _
                    "'" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "', " & _
                    "'" & txtPurchasedNugs.Text & "','" & txtSoldNugs.Text & "','" & txtTotCrate.Text & "');"
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
            'clsFun.ExecNonQuery("COMMIT;")
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub


    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\StockBalance.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
End Class