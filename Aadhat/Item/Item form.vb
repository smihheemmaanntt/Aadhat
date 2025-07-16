Public Class Item_form
    Public ItemNameValue As String = ""
    Public ItemNameID As String = ""
    Public OpenedFromItems As Boolean = False
    Public Opener As Form
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub

    Private Sub Item_form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If OpenedFromItems Then
            Opener.Focus()
            If TypeOf Opener Is Super_Sale Then
                DirectCast(Opener, Super_Sale).txtItem.Focus()
                If ItemNameValue.Trim Is Nothing Then Exit Sub
                Super_Sale.txtItem.Text = ItemNameValue
                Super_Sale.txtItemID.Text = Val(ItemNameID)
            ElseIf TypeOf Opener Is SpeedSale Then
                DirectCast(Opener, SpeedSale).txtItem.Focus()
                If ItemNameValue.Trim Is Nothing Then Exit Sub
                SpeedSale.txtItem.Text = ItemNameValue
                SpeedSale.txtItemID.Text = Val(ItemNameID)
            ElseIf TypeOf Opener Is Standard_Sale Then
                DirectCast(Opener, Standard_Sale).txtItem.Focus()
                If ItemNameValue.Trim Is Nothing Then Exit Sub
                Standard_Sale.txtItem.Text = ItemNameValue
                Standard_Sale.txtItemID.Text = Val(ItemNameID)
            ElseIf TypeOf Opener Is Purchase Then
                DirectCast(Opener, Purchase).txtItem.Focus()
                If ItemNameValue.Trim Is Nothing Then Exit Sub
                Purchase.txtItem.Text = ItemNameValue
                Purchase.txtItemID.Text = Val(ItemNameID)
            ElseIf TypeOf Opener Is Sellout_Mannual Then
                DirectCast(Opener, Sellout_Mannual).txtItem.Focus()
                If ItemNameValue.Trim Is Nothing Then Exit Sub
                Sellout_Mannual.txtItem.Text = ItemNameValue
                Sellout_Mannual.txtItemID.Text = Val(ItemNameID)
            End If
        End If
    End Sub

    Private Sub Item_form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Item_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True : BtnDelete.Visible = False
        rowColums() : CbRateas.SelectedIndex = 0
        cbTrackStock.SelectedIndex = 0
        ' PnlTranslate.Visible = False
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 13
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Item Name" : dg1.Columns(1).Width = 200
        dg1.Columns(2).Name = "Item Name" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Commission" : dg1.Columns(3).Width = 90
        dg1.Columns(4).Name = "Mandi Tax " : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Tare" : dg1.Columns(5).Width = 50
        dg1.Columns(6).Name = "Labour" : dg1.Columns(6).Width = 90
        dg1.Columns(7).Name = "RDF" : dg1.Columns(7).Width = 50
        dg1.Columns(8).Name = "Crate" : dg1.Columns(8).Width = 50
        dg1.Columns(9).Name = "Weight" : dg1.Columns(9).Width = 60
        dg1.Columns(10).Name = "Cut" : dg1.Columns(10).Width = 50
        dg1.Columns(11).Name = "Rate as" : dg1.Columns(11).Width = 90
        dg1.Columns(12).Name = "Stock as" : dg1.Columns(12).Width = 100
        retrive()
    End Sub
    Public Sub FillContros(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnDelete.Visible = True
        BtnSave.BackColor = Color.Coral
        BtnSave.Image = My.Resources.EditItem
        BtnSave.Text = "&Update" : lblName.Text = "MODIFY ITEM"
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from items where id=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtItemName.Text = ds.Tables("a").Rows(0)("ItemName").ToString()
            txtOtherName.Text = ds.Tables("a").Rows(0)("OtherName").ToString()
            txtCommission.Text = Format(Val(ds.Tables("a").Rows(0)("CommisionPer").ToString()), "0.00")
            txtuserCharges.Text = Format(Val(ds.Tables("a").Rows(0)("UserChargesPer").ToString()), "0.00")
            txtTare.Text = Format(Val(ds.Tables("a").Rows(0)("Tare").ToString()), "0.00")
            txtLabour.Text = Format(Val(ds.Tables("a").Rows(0)("Labour").ToString()), "0.00")
            txtRdf.Text = Format(Val(ds.Tables("a").Rows(0)("RDFPer").ToString()), "0.00")
            txtWeightPerNug.Text = ds.Tables("a").Rows(0)("WeightPerNug").ToString()
            txtcutPerNug.Text = ds.Tables("a").Rows(0)("CutPerNug").ToString()
            Crate = ds.Tables("a").Rows(0)("MaintainCrate").ToString().Trim()
            CbRateas.Text = ds.Tables("a").Rows(0)("RateAs").ToString()
            cbTrackStock.Text = ds.Tables("a").Rows(0)("TrackStock").ToString().Trim()
            If Crate = "Y" Then
                CBMaintainCrate.CheckState = CheckState.Checked
            Else
                CBMaintainCrate.CheckState = CheckState.Unchecked
            End If
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
        End If
    End Sub
    Private Sub save()
        If clsFun.ExecScalarInt("Select count(*)from items where upper(itemName)=upper('" & txtItemName.Text & "')") = 1 Then
            MsgBox("Item Already Exists...", vbOkOnly, "Access Denied") : txtItemName.Focus() : Exit Sub
        End If
        Dim guid As Guid = guid.NewGuid()
        Dim cmd As New SQLite.SQLiteCommand
        If txtItemName.Text = "" Then
            txtItemName.Focus()
            MsgBox("Please Fill Item Name... ", MsgBoxStyle.Exclamation, "Empty")
        Else
            Dim Crate As String = ""
            If CBMaintainCrate.Checked Then
                Crate = "Y"
            Else
                Crate = "N"
            End If
            Dim sql As String = "insert into Items (ItemName,OtherName,CommisionPer,UserChargesPer,Tare,Labour,RDFPer,WeightPerNug,CutPerNug,MaintainCrate,Rateas,TrackStock,guid) values (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13)"
            Try
                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                cmd.Parameters.AddWithValue("@1", txtItemName.Text.Trim)
                cmd.Parameters.AddWithValue("@2", txtOtherName.Text.Trim)
                cmd.Parameters.AddWithValue("@3", Val(txtCommission.Text.Trim))
                cmd.Parameters.AddWithValue("@4", Val(txtuserCharges.Text.Trim))
                cmd.Parameters.AddWithValue("@5", Val(txtTare.Text.Trim))
                cmd.Parameters.AddWithValue("@6", Val(txtLabour.Text.Trim))
                cmd.Parameters.AddWithValue("@7", Val(txtRdf.Text.Trim))
                cmd.Parameters.AddWithValue("@8", Val(txtWeightPerNug.Text.Trim))
                cmd.Parameters.AddWithValue("@9", Val(txtcutPerNug.Text.Trim))
                cmd.Parameters.AddWithValue("@10", Crate)
                cmd.Parameters.AddWithValue("@11", CbRateas.Text.Trim)
                cmd.Parameters.AddWithValue("@12", cbTrackStock.Text.Trim)
                cmd.Parameters.AddWithValue("@13", guid.ToString())
                If cmd.ExecuteNonQuery() > 0 Then
                    ItemNameValue = txtItemName.Text : ItemNameID = Val(txtid.Text)
                    Textclear() : If OpenedFromItems Then Me.Close()
                    Me.Alert("Success Alert", msgAlert.enmType.Success)
                End If
                clsFun.CloseConnection()
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
    End Sub
    Private Sub Textclear()
        txtItemName.Text = "" : txtOtherName.Text = ""
        txtWeightPerNug.Text = "" : txtcutPerNug.Text = ""
        txtCommission.Text = "" : txtuserCharges.Text = ""
        txtTare.Text = "" : txtLabour.Text = ""
        txtRdf.Text = "" : CBMaintainCrate.Checked = False
        BtnSave.Image = My.Resources.AddItem
        txtItemName.Focus() : retrive()
        BtnSave.Text = "&Save" : lblName.Text = "ITEM ENTRY"
        BtnDelete.Visible = False
        BtnSave.BackColor = Color.ForestGreen
        BtnSave.Image = My.Resources.AddItem
    End Sub
    Public Sub Alert(ByVal msg As String, ByVal type As msgAlert.enmType)
        Dim frm As msgAlert = New msgAlert()
        frm.showAlert(msg, type)
    End Sub
    Private Sub UpdateItems()
        If clsFun.ExecScalarInt("Select count(*)from items where upper(itemName)=upper('" & txtItemName.Text & "')") > 1 Then
            MsgBox("Item Already Exists...", vbOkOnly, "Access Denied") : txtItemName.Focus() : Exit Sub
        End If
        Dim cmd As New SQLite.SQLiteCommand
        If txtItemName.Text.Trim = "" Then
            txtItemName.Focus()
            MsgBox("Please Item Name... ", MsgBoxStyle.Exclamation, "Access Denied")
        Else
            Dim Crate As String = ""
            If CBMaintainCrate.CheckState = CheckState.Checked Then Crate = "Y"
            If CBMaintainCrate.CheckState = CheckState.Unchecked Then Crate = "N"
            '  Dim sql As String = "Update AccountGroup SET GroupName='" & txtGroupName.Text & "',DC='" & lbldc.Text & "',UndergrpID=" & CbUnderGroup.SelectedValue & ",IsPrimary='" & primary & "',ISCHNGDEL=0 WHERE ID=" & Val(txtid.Text) & ""
            Dim sql As String = "Update Items SET ItemName='" & txtItemName.Text.Trim & "',OtherName='" & txtOtherName.Text.Trim & "',CommisionPer=" & Val(txtCommission.Text) & ", " & _
                "UserChargesPer=" & Val(txtuserCharges.Text.Trim) & ",Tare=" & Val(txtTare.Text.Trim) & ",Labour=" & Val(txtLabour.Text.Trim) & ",RDFPer=" & Val(txtRdf.Text.Trim) & ", " & _
                "WeightPerNug=" & Val(txtWeightPerNug.Text.Trim) & ",CutPerNug=" & Val(txtcutPerNug.Text) & ",MaintainCrate='" & Crate & "',Rateas='" & CbRateas.Text & "',TrackStock='" & cbTrackStock.Text & "' WHERE ID=" & Val(txtid.Text) & ""
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    ItemNameValue = txtItemName.Text : ItemNameID = Val(txtid.Text)
                    Textclear() : If OpenedFromItems Then Me.Close() : Exit Sub
                    Me.Alert("Updated Successful...", msgAlert.enmType.Update)

                End If
                'con.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                'con.Close()
            End Try
        End If
    End Sub
    Private Sub Delete()
        If clsFun.ExecScalarInt("Select count(*) from Transaction2 where ItemID='" & Val(txtid.Text) & "'") <> 0 Then
            MsgBox("Item Already Used in Sale", vbOkOnly, "Access Denied")
            Exit Sub
        ElseIf clsFun.ExecScalarInt("Select count(*) from Transaction1 where ItemID='" & Val(txtid.Text) & "'") <> 0 Then
            MsgBox("Item Already Used in Scrip(Beejak)", vbOkOnly, "Access Denied")
            Exit Sub
        ElseIf clsFun.ExecScalarInt("Select count(*) from Purchase where ItemID='" & Val(txtid.Text) & "'") <> 0 Then
            MsgBox("Item Already Used in Purchase / Stock In", vbOkOnly, "Access Denied")
            Exit Sub
        Else
            'Exit Sub
            Try
                '  If clsFun.ExecScalarInt("Select tag From AccountGroup  WHERE ID=" & txtid.Text & "") = 0 Then MsgBox("access denied", MsgBoxStyle.Critical) : Exit Sub
                If clsFun.ExecScalarInt("Select count(*) from Vouchers where ItemID='" & Val(txtid.Text) & "'") = 0 Then
                    If clsFun.ExecScalarInt("Select count(*) from Transaction1 where ItemID='" & Val(txtid.Text) & "'") = 0 Then
                        If MessageBox.Show("Are you Sure want to Delete Item : " & txtItemName.Text & " ??", "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                            If clsFun.ExecNonQuery("DELETE from Items WHERE ID=" & Val(txtid.Text) & "") > 0 Then
                                'MsgBox("Successfully deleted")
                                Textclear()
                                'GroupList.BtnRefresh.PerformClick()
                                Me.Alert("Item Deleted Successful...", msgAlert.enmType.Delete)
                                retrive()
                            End If
                        End If
                    Else
                        MsgBox("Item Group Cannot delete alreday use in Databse", vbOkOnly, "Access Denied")
                    End If
                Else
                    MsgBox("Item Group Cannot delete alreday use in Databse", vbOkOnly, "Access Denied")
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Items " & condtion & " order by ItemName ")
        Try
            If dt.Rows.Count > 0 Then
                Application.DoEvents()
                For i = 0 To dt.Rows.Count - 1
                    If Application.OpenForms().OfType(Of Item_form).Any = False Then Exit Sub
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        'dg1.Rows.Clear()
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(2).Value = dt.Rows(i)("OtherName").ToString()
                        .Cells(3).Value = Format(Val(dt.Rows(i)("CommisionPer").ToString()), "0.00")
                        .Cells(4).Value = Format(Val(dt.Rows(i)("UserChargesPer").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Tare").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Labour").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("RDFPer").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("MaintainCrate").ToString()
                        .Cells(9).Value = dt.Rows(i)("WeightPerNug").ToString()
                        .Cells(10).Value = dt.Rows(i)("CutPerNug").ToString()
                        .Cells(11).Value = dt.Rows(i)("Rateas").ToString()
                        .Cells(12).Value = dt.Rows(i)("TrackStock").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
        lblCountItem.Text = "Total Items : " & Val(dg1.Rows.Count)
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarStr("Select count(*)from Items where ItemName='" & txtItemName.Text.Trim & "'") = 1 Then
                MsgBox("Item Already Exists...", vbOkOnly, "Access Denied")
                txtItemName.Focus() : Exit Sub
            End If
            save()
        Else
            Dim CheckID As Integer = clsFun.ExecScalarInt("Select ID from Items where upper(ItemName)=upper('" & txtItemName.Text.Trim & "')")
            If clsFun.ExecScalarStr("Select count(*) from items where upper(ItemName)=upper('" & txtItemName.Text.Trim & "') ") > 1 Then
                MsgBox("Item Name Already Exists...", vbOkOnly, "Access Denied") : txtItemName.Focus() : Exit Sub
            End If
            If Val(CheckID) = 0 Then
                UpdateItems()
            ElseIf Val(CheckID) = Val(txtid.Text) Then
                UpdateItems()
            ElseIf Val(CheckID) <> Val(txtid.Text) Then
                MsgBox("Account Already Exists...", MsgBoxStyle.Critical, "Access Denied") : txtItemName.Focus() : Exit Sub
            End If
        End If
        retrive()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown

        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            FillContros(dg1.SelectedRows(0).Cells(0).Value)
            txtItemName.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Up Then
            If dg1.CurrentCell.RowIndex = 0 Then
                txtItemName.Focus()
                dg1.ClearSelection()
                e.SuppressKeyPress = True
            End If
        End If
        If e.KeyCode = Keys.Down Then
            ' Agar current row last row hai to TextBox par focus karein
            If dg1.CurrentCell.RowIndex = dg1.RowCount - 1 Then
                txtItemName.Focus()
                dg1.ClearSelection()
                e.SuppressKeyPress = True ' Ye event ko stop karta hai taaki aur kuch na ho
            End If
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        FillContros(dg1.SelectedRows(0).Cells(0).Value)
        txtItemName.Focus()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub

    Private Sub CBMaintainCrate_GotFocus(sender As Object, e As EventArgs) Handles CBMaintainCrate.GotFocus
        CBMaintainCrate.ForeColor = Color.BlueViolet
    End Sub
    Private Sub CBMaintainCrate_Leave(sender As Object, e As EventArgs) Handles CBMaintainCrate.Leave
        CBMaintainCrate.ForeColor = Color.Red
    End Sub
    Private Sub txtCommission_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCommission.KeyPress,
        txtcutPerNug.KeyPress, txtLabour.KeyPress, txtTare.KeyPress, txtuserCharges.KeyPress, txtWeightPerNug.KeyPress, txtRdf.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtItemName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemName.KeyPress, txtOtherName.KeyPress
        If e.KeyChar = "'"c Then
            ' Agar haan, to event ko handle karke usko prevent kar dein
            e.Handled = True
        End If
    End Sub
    Private Sub txtItemName_Leave(sender As Object, e As EventArgs) Handles txtItemName.Leave
        If txtItemName.Text <> txtItemName.Text.ToUpper Then
            txtItemName.Text = StrConv(txtItemName.Text, VbStrConv.ProperCase)
        End If
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarInt("Select count(*)from items where upper(itemName)=upper('" & txtItemName.Text & "')") = 1 Then
                MsgBox("Item Already Exists...", vbOkOnly, "Access Denied")
            End If
        Else
            If clsFun.ExecScalarInt("Select count(*)from items where upper(itemName)=upper('" & txtItemName.Text & "')") > 1 Then
                MsgBox("Item Already Exists...", vbOkOnly, "Access Denied")
            End If
        End If
        'PnlTranslate.Visible = True
        'txtTranslate.Focus()
    End Sub
    Private Sub txtItemName_TextChanged(sender As Object, e As EventArgs) Handles txtItemName.TextChanged
        If BtnSave.Text = "&Save" Then
            txtOtherName.Text = txtItemName.Text
        End If
    End Sub
    Private Sub txtItemName_Gotfocus(sender As Object, e As EventArgs) Handles txtItemName.GotFocus,
           txtOtherName.GotFocus, txtWeightPerNug.GotFocus, txtcutPerNug.GotFocus, txtCommission.GotFocus, txtuserCharges.GotFocus,
           txtTare.GotFocus, txtRdf.GotFocus, txtLabour.GotFocus, txtItemSearch.GotFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.PaleTurquoise
        tb.SelectAll()
    End Sub

    Private Sub txtItemName_LostFocus(sender As Object, e As EventArgs) Handles txtItemName.LostFocus,
           txtOtherName.LostFocus, txtWeightPerNug.LostFocus, txtcutPerNug.LostFocus, txtCommission.LostFocus, txtuserCharges.LostFocus,
           txtTare.LostFocus, txtRdf.LostFocus, txtLabour.LostFocus, txtItemSearch.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.FromArgb(247, 220, 111)
    End Sub

    Private Sub txtItemName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItemName.KeyDown,
           txtOtherName.KeyDown, txtWeightPerNug.KeyDown, txtcutPerNug.KeyDown, txtCommission.KeyDown, txtuserCharges.KeyDown,
           txtTare.KeyDown, txtRdf.KeyDown, txtLabour.KeyDown, CBMaintainCrate.KeyDown, CbRateas.KeyDown, cbTrackStock.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            ' Agar focus combobox par hai, to dropdown kholna
            If TypeOf sender Is ComboBox Then
                Dim comboBox As ComboBox = DirectCast(sender, ComboBox)
                If Not comboBox.DroppedDown Then
                    comboBox.DroppedDown = True
                    e.SuppressKeyPress = True
                End If
            Else
                ' DataGridView par focus karna
                If dg1.RowCount = 0 Then Exit Sub
                dg1.Focus()
                dg1.CurrentCell = dg1.Rows(0).Cells(1) ' Pehli row ke pehle column par focus
                dg1.Rows(0).Selected = True
            End If
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.PerformClick()
                Me.Close()
        End Select
    End Sub

    Private Sub txtItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtItemSearch.Text.Trim() <> "" Then
                retrive(" where ItemName Like '" & txtItemSearch.Text.Trim() & "%'")
            Else
                retrive()
            End If
        End If
    End Sub

    Private Sub txtItemSearch_TextChanged(sender As Object, e As EventArgs) Handles txtItemSearch.TextChanged

    End Sub

    Private Sub txtOtherName_Leave(sender As Object, e As EventArgs) Handles txtOtherName.Leave
        Try
            SendKeys.Send("+%")
        Catch ex As InvalidOperationException
            ' Do nothing
        End Try
        ' pbHindi.Visible = False
    End Sub

    Private Sub txtOtherName_GotFocus(sender As Object, e As EventArgs) Handles txtOtherName.GotFocus
        Try
            SendKeys.Send("+%")
        Catch ex As InvalidOperationException
            ' Do nothing
        End Try
        'pbHindi.Visible = True
    End Sub

    Private Sub txtOtherName_TextChanged(sender As Object, e As EventArgs) Handles txtOtherName.TextChanged

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub LineShape4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtCommission_TextChanged(sender As Object, e As EventArgs) Handles txtCommission.TextChanged

    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) values(" & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                   "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "'," & _
                  "'" & .Cells(9).Value & "','" & .Cells(10).Value & "')"
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
        Report_Viewer.printReport("\Items.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub txtcutPerNug_TextChanged(sender As Object, e As EventArgs) Handles txtcutPerNug.TextChanged

    End Sub
End Class