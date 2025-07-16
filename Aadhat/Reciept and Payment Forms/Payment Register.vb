Public Class Payment_Register
    Private headerCheckBox As CheckBox = New CheckBox()
    Dim ID As String
    Private Sub Payment_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If PnlDeleteBills.Visible = True Then PnlDeleteBills.Visible = False : mskFromDate.Focus() : Exit Sub
            Me.Close()
        End If
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown
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
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_KeyDown(sender As Object, e As KeyEventArgs) Handles MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then btnShow.Focus()
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Payment_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        mskFromDate.Focus() : rowColums()
    End Sub
    Private Sub HeaderCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        'Necessary to end the edit mode of the Cell.
        dg1.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In dg1.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = headerCheckBox.Checked
        Next
    End Sub
    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        'Check to ensure that the row CheckBox is clicked.
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In dg1.Rows
                If Convert.ToBoolean(row.Cells("checkBoxColumn").EditedFormattedValue) = False Then
                    isChecked = False
                    Exit For
                End If
            Next
            headerCheckBox.Checked = isChecked
        End If
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 9
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        headerCheckBox.Location = New Point(headerCellLocation.X + 8, headerCellLocation.Y + 2)
        headerCheckBox.BackColor = Color.GhostWhite
        headerCheckBox.Size = New Size(18, 18)
        AddHandler headerCheckBox.Click, AddressOf HeaderCheckBox_Clicked
        dg1.Controls.Add(headerCheckBox)
        Dim checkBoxColumn As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = ""
        checkBoxColumn.Width = 30
        checkBoxColumn.Name = "checkBoxColumn"
        dg1.Columns.Insert(0, checkBoxColumn)
        AddHandler dg1.CellContentClick, AddressOf dg1_CellClick
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Date" : dg1.Columns(2).Width = 98
        dg1.Columns(3).Name = "Mode" : dg1.Columns(3).Width = 171
        dg1.Columns(4).Name = "Rcpt No." : dg1.Columns(4).Width = 99
        dg1.Columns(5).Name = "Account Name" : dg1.Columns(5).Width = 180
        dg1.Columns(6).Name = "Amount" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Discount" : dg1.Columns(7).Width = 108
        dg1.Columns(8).Name = "Total" : dg1.Columns(8).Width = 120
        dg1.Columns(9).Name = "Remark" : dg1.Columns(9).Width = 260
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub
    Sub calc()
        Dim i As Integer
        txtTotBasic.Text = Format(0, "0.00") : txtTotDisCount.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotDisCount.Text = Format(Val(txtTotDisCount.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
        Next
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " and transtype='" & Me.Text & "'Order By EntryDate")
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(1).Value = dt.Rows(i)("ID").ToString()
                        .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(3).Value = dt.Rows(i)("SallerName").ToString()
                        .Cells(4).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(5).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(9).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(1).readOnly = True : .Cells(2).readOnly = True
                        .Cells(3).readOnly = True : .Cells(4).readOnly = True
                        .Cells(5).readOnly = True : .Cells(6).readOnly = True
                        .Cells(7).readOnly = True : .Cells(8).readOnly = True
                        .Cells(9).readOnly = True ': .Cells(0).readOnly = True
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub btnShow_Click_1(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
            PayMentform.MdiParent = MainScreenForm
            PayMentform.Show()
            PayMentform.FillControls(tmpID)
            If Not PayMentform Is Nothing Then
                PayMentform.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
        PayMentform.MdiParent = MainScreenForm
        PayMentform.Show()
        PayMentform.FillControls(tmpID)
        If Not PayMentform Is Nothing Then
            PayMentform.BringToFront()
        End If
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,T1,T2,T3,M1) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells("Date").Value & "','" & .Cells("Mode").Value & "','" & .Cells("Rcpt No.").Value & "','" & .Cells("Account Name").Value & "'," & _
                    "'" & Format(Val(.Cells("Amount").Value), "0.00") & "'," & Format(Val(.Cells("Discount").Value), "0.00") & ",'" & Format(Val(.Cells("Total").Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells("Remark").Value), "0.00") & "'," & Format(Val(txtTotBasic.Text), "0.00") & "," & _
                    " " & Format(Val(txtTotDisCount.Text), "0.00") & "," & Format(Val(TxtGrandTotal.Text), "0.00") & ",'" & lblName.Text & "')"
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
        Report_Viewer.printReport("\Reports\PaymentRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            retrive(" And AccountName Like '%" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PnlDeleteBills.Visible = True : txtDelete.Focus()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ID = String.Empty
        Dim checkBox As DataGridViewCheckBoxCell
        If txtDelete.Text <> "SURE" Then MsgBox("captcha Mis Match, Unable to Delete Bills", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        For Each row As DataGridViewRow In dg1.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                ID = ID & row.Cells(1).Value & ","
            End If
        Next
        If ID <> "" Then
            ID = ID.Remove(ID.LastIndexOf(","))
        End If
        If ID = "" Then MsgBox("Please Select atleast 1 Entry to Delete", MsgBoxStyle.Critical, "Access Denied") : txtDelete.Clear() : Exit Sub
        If MessageBox.Show("Are you Sure want to Delete Selected Entries ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            UpdateLedger()
            If clsFun.ExecNonQuery("Delete From Vouchers Where ID in (" & ID & "); " & _
                                    "Delete From Ledger Where VourchersID in(" & ID & ");") Then
                MsgBox("Selected Bills Deleted Successfully...", MsgBoxStyle.Information, "Sucessful")
                retrive() : PnlDeleteBills.Visible = False : mskFromDate.Focus() : txtDelete.Clear() : txtDelete.Focus()
            End If
        End If
    End Sub
    Private Sub UpdateLedger()
        ClsFunserver.ExecScalarStr("Delete from Ledger where VourchersID in (" & ID & ")")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Ledger where VourchersID in (" & ID & ")")
        '  sSql = "Insert into CrateVoucher(VoucherID,SlipNo, EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName, Qty,Remark,Rate,Amount,CashPaid,ServerTag,OrgID)"
        Dim fastQuery As String = String.Empty
        Try
            If dt.Rows.Count > 0 Then
                ServerTag = 0
                For i = 0 To dt.Rows.Count - 1
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(dt.Rows(i)("VourchersID").ToString()) & ",'" & CDate(dt.Rows(i)("EntryDate").ToString()).tostring("yyyy-MM-dd") & "'," & _
                                        "'" & Me.Text & "',  " & Val(dt.Rows(i)("AccountID").ToString()) & ",'" & dt.Rows(i)("AccountName").ToString() & "','" & Val(dt.Rows(i)("Amount").ToString()) & "','" & dt.Rows(i)("DC").ToString() & "', " & Val(ServerTag) & "," & Val(OrgID) & "," & _
                                       "'" & dt.Rows(i)("Remark").ToString() & "','" & dt.Rows(i)("Narration").ToString() & "','" & dt.Rows(i)("RemarkHindi").ToString() & "'"
                Next
            End If
            If fastQuery = String.Empty Then Exit Sub
            ClsFunserver.FastLedger(fastQuery)
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub txtDelete_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDelete.KeyDown
        If e.KeyCode = Keys.Enter Then btnDelete.Focus()
    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub

    Private Sub txtRectNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRectNo.KeyDown
        If txtRectNo.Text.Trim() <> "" Then
            retrive(" And BillNo Like '%" & txtRectNo.Text.Trim() & "%'")
        End If
        If txtRectNo.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtRectNo_TextChanged(sender As Object, e As EventArgs) Handles txtRectNo.TextChanged

    End Sub
End Class