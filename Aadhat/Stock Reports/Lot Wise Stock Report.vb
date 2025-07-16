Public Class Lot_Wise_Stock_Report
    Private Sub Stock_Balance_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.SelectionStart = 0 : mskEntryDate.SelectionLength = Len(mskEntryDate.Text)
    End Sub
    Private Sub Stock_Balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums() : cbTrackStock.SelectedIndex = 0
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 11
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Entry Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "Stock Holder" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Store" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Items" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Lot No" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Purchase" : dg1.Columns(7).Width = 100
        dg1.Columns(8).Name = "Sold/Trnsfr" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "Balance" : dg1.Columns(9).Width = 150
        dg1.Columns(10).Name = "Type" : dg1.Columns(10).Visible = False
        ' retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            Retrive("And AccountName Like '" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyUp
        If txtItemSearch.Text.Trim() <> "" Then
            Retrive("And ItemName Like '" & txtItemSearch.Text.Trim() & "%'")
        End If
        If txtItemSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, cbTrackStock.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub Retrive(Optional ByVal condtion As String = "", Optional ByVal condtion1 As String = "")
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim sql As String = String.Empty
        If cbTrackStock.SelectedIndex = 0 Then
            sql = "Select VoucherID,EntryDate,Purchase.TransType as TransType, AccountID,AccountName, StockHolderName,StorageName,ItemID, ItemName,LotNo,Sum(Nug) as PurchaseNug," & _
                  " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot=Purchase.LotNo and ItemID=Purchase.ItemID" & _
                  " and PurchaseID=Purchase.VoucherID and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')as soldNug,(Sum(Nug)-" & _
                  " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot=Purchase.LotNo and ItemID=Purchase.ItemID" & _
                  " and PurchaseID=Purchase.VoucherID and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as RestNug From Purchase" & _
                  " Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " " & condtion1 & " " & _
                  " Group by ItemID,LotNo,VoucherID Having RestNug <> 0  order by EntryDate,AccountName,ItemName,LotNo "
        Else
            sql = "Select VoucherID,EntryDate,Purchase.TransType as TransType, AccountID,AccountName, StockHolderName,StorageName,ItemID, ItemName,LotNo,Sum(Weight) as PurchaseNug," & _
      " (Select ifnull(sum(Weight),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot=Purchase.LotNo and ItemID=Purchase.ItemID" & _
      " and PurchaseID=Purchase.VoucherID and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')as soldNug,(Sum(Weight)-" & _
      " (Select ifnull(sum(Weight),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot=Purchase.LotNo and ItemID=Purchase.ItemID" & _
      " and PurchaseID=Purchase.VoucherID and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as RestNug From Purchase" & _
      " Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " " & condtion1 & " " & _
      " Group by ItemID,LotNo,VoucherID Having RestNug <> 0  order by EntryDate,AccountName,ItemName,LotNo "
        End If
        dt = clsFun.ExecDataTable(sql)
        For i = 0 To dt.Rows.Count - 1
            dg1.Rows.Add()
            With dg1.Rows(i)
                .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                .Cells(1).Value = CDate(dt.Rows(i)("EntryDate").ToString()).ToString("dd-MM-yyyy")
                .Cells(2).Value = dt.Rows(i)("StockHolderName").ToString()
                .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                .Cells(4).Value = dt.Rows(i)("StorageName").ToString()
                .Cells(5).Value = dt.Rows(i)("ItemName").ToString()
                .Cells(6).Value = dt.Rows(i)("LotNo").ToString()
                .Cells(7).Value = Format(Val(dt.Rows(i)("PurchaseNug").ToString()), "0.00")
                .Cells(8).Value = Format(Val(dt.Rows(i)("SoldNug").ToString()), "0.00")
                .Cells(9).Value = Format(Val(dt.Rows(i)("RestNug").ToString()), "0.00")
                .Cells(10).Value = dt.Rows(i)("TransType").ToString()
            End With
        Next
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()
    End Sub
    Sub calc()
        txtTotCrate.Text = Format(0, "0.00") : txtPurchasedNugs.Text = Format(0, "0.00") : txtSoldNugs.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtPurchasedNugs.Text = Format(Val(txtPurchasedNugs.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtSoldNugs.Text = Format(Val(txtSoldNugs.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotCrate.Text = Format(Val(txtTotCrate.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
        Next
    End Sub

    Private Sub txtPurchaseNugSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtLotNo.KeyUp
        If txtLotNo.Text.Trim() <> "" Then
            Retrive("And LotNo Like '" & txtLotNo.Text.Trim() & "%'")
        End If
        If txtLotNo.Text.Trim() = "" Then
            Retrive()
        End If
    End Sub

    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11) values('" & mskEntryDate.Text & "','" & .Cells(1).Value & "'," & _
                    "'" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "', " & _
                    "'" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & .Cells(9).Value & "','" & txtPurchasedNugs.Text & "','" & txtSoldNugs.Text & "','" & txtTotCrate.Text & "');"
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
        Report_Viewer.printReport("\Reports\StockBalance -LotWise.rpt")
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

    Private Sub dg1_Click(sender As Object, e As EventArgs) Handles dg1.Click
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If dg1.SelectedRows(0).Cells(10).Value = "Purchase" Then
                Purchase.MdiParent = MainScreenForm
                Purchase.Show() : Purchase.BringToFront()
                Purchase.FillControls(Val(dg1.SelectedRows(0).Cells(0).Value))
            Else
                Store_Transfer.MdiParent = MainScreenForm
                Store_Transfer.Show() : Store_Transfer.BringToFront()
                Store_Transfer.FillControls(Val(dg1.SelectedRows(0).Cells(0).Value))
            End If
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count <> 0 Then Exit Sub
        If dg1.SelectedRows(0).Cells(10).Value = "Purchase" Then
            Purchase.MdiParent = MainScreenForm
            Purchase.Show() : Purchase.BringToFront()
            Purchase.FillControls(Val(dg1.SelectedRows(0).Cells(10).Value))
        Else
            Store_Transfer.MdiParent = MainScreenForm
            Store_Transfer.Show() : Store_Transfer.BringToFront()
            Store_Transfer.FillControls(Val(dg1.SelectedRows(0).Cells(10).Value))
        End If
    End Sub

    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub
End Class