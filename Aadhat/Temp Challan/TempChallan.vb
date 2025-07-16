Public Class TempChallan
    Dim vno As Integer
    Dim VchId As Integer
    Dim sql As String = String.Empty
    Dim count As Integer = 0
    Dim MaxID As String = String.Empty
    Dim CalcType As String = String.Empty
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Standard_Sale_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If MessageBox.Show("Are You Sure want to Exit Standard Sale ??", "Exit Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        End If


    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub AcBal()
        'Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtAccountID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = Math.Round(Math.Abs(Val(opbal)), 2) & " Cr"
        Else
            opbal = Math.Round(Math.Abs(Val(opbal)), 2) & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Math.Round(Math.Abs(Val(opbal)), 2) & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " Cr"
            Else
                opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " Dr"
            End If
        End If
        lblAcBal.Visible = True
        lblAcBal.Text = "Account Balance : " & opbal
    End Sub
    Private Sub Standard_Sale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        '   clsFun.FillDropDownList(CbAccountName, "Select ID, AccountName From Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))", "AccountName", "Id", "")
        ' clsFun.FillDropDownList(cbitem, "Select * From Items", "ItemName", "Id", "")
        ' clsFun.FillDropDownList(CbCharges, "Select * From Charges", "ChargeName", "Id", "")
        Cbper.SelectedIndex = 0
        mskEntryDate.Focus()
        Me.KeyPreview = True
        pnlMarka.Visible = False
        BtnDelete.Enabled = False
        VNumber() : rowColums() : FillstdSale()
    End Sub
    Public Sub FillstdSale()
        ssql = "Select * from Option4"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CommPerStop").ToString().Trim() = "Y" Then txtComPer.TabStop = True Else txtComPer.TabStop = False
            If dt.Rows(0)("CommAmtStop").ToString().Trim() = "Y" Then txtComAmt.TabStop = True Else txtComAmt.TabStop = False
            If dt.Rows(0)("MandiTaxStop").ToString().Trim() = "Y" Then txtMPer.TabStop = True Else txtMPer.TabStop = False
            If dt.Rows(0)("MandiTaxAmtStop").ToString().Trim() = "Y" Then txtMAmt.TabStop = True Else txtMAmt.TabStop = False
            If dt.Rows(0)("RDFPerStop").ToString().Trim() = "Y" Then txtRdfPer.TabStop = True Else txtRdfPer.TabStop = False
            If dt.Rows(0)("RDFAmtStop").ToString().Trim() = "Y" Then txtRdfAmt.TabStop = True Else txtRdfAmt.TabStop = False
            If dt.Rows(0)("TareStop").ToString().Trim() = "Y" Then txtTare.TabStop = True Else txtTare.TabStop = False
            If dt.Rows(0)("TareAmtStop").ToString().Trim() = "Y" Then txtTareAmt.TabStop = True Else txtTareAmt.TabStop = False
            If dt.Rows(0)("LabourStop").ToString().Trim() = "Y" Then txtLabour.TabStop = True Else txtLabour.TabStop = False
            If dt.Rows(0)("LabourAmtStop").ToString().Trim() = "Y" Then txtLaboutAmt.TabStop = True Else txtLaboutAmt.TabStop = False
        End If
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 48
            .Columns(0).Name = "ID"
            .Columns(0).Visible = False
            .Columns(1).Name = "EntryDate"
            .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo"
            .Columns(2).Width = 159
            .Columns(3).Name = "SallerName"
            .Columns(3).Width = 59
            .Columns(4).Name = "BillingType"
            .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo"
            .Columns(5).Width = 159
            .Columns(6).Name = "itemName"
            .Columns(6).Width = 69
            .Columns(7).Name = "Cut"
            .Columns(7).Width = 76
            .Columns(8).Name = "Nug"
            .Columns(8).Width = 90
            .Columns(9).Name = "Weight"
            .Columns(9).Width = 86
            .Columns(10).Name = "Rate"
            .Columns(10).Width = 90
            .Columns(11).Name = "per"
            .Columns(11).Width = 50
            .Columns(12).Name = "Amount"
            .Columns(12).Width = 95
            .Columns(13).Name = "ChargeName"
            .Columns(13).Width = 159
            .Columns(14).Name = "onValue"
            .Columns(14).Width = 159
            .Columns(15).Name = "@"
            .Columns(15).Width = 59
            .Columns(16).Name = "=/-"
            .Columns(16).Width = 59
            .Columns(17).Name = "ChargeAmount"
            .Columns(17).Width = 69
            .Columns(18).Name = "TotalNug"
            .Columns(18).Width = 76
            .Columns(19).Name = "TotalWeight"
            .Columns(19).Width = 90
            .Columns(20).Name = "TotalBasicAmount"
            .Columns(20).Width = 86
            .Columns(21).Name = "RoundOff"
            .Columns(21).Width = 90
            .Columns(22).Name = "TotalAmount"
            .Columns(22).Width = 90
            .Columns(23).Name = "OtherItemName"
            .Columns(23).Width = 95
            .Columns(24).Name = "OtherAccountName"
            .Columns(24).Width = 159
            .Columns(25).Name = "AmountInWords"
            .Columns(25).Width = 159
            .Columns(26).Name = "CommPer"
            .Columns(26).Width = 90
            .Columns(27).Name = "CommAmt"
            .Columns(27).Width = 86
            .Columns(28).Name = "UCPer"
            .Columns(28).Width = 90
            .Columns(29).Name = "UcAmt"
            .Columns(29).Width = 90
            .Columns(30).Name = "RdfPer"
            .Columns(30).Width = 95
            .Columns(31).Name = "RdfAmt"
            .Columns(31).Width = 159
            .Columns(32).Name = "Bardanaper"
            .Columns(32).Width = 159
            .Columns(33).Name = "BardanaAmt"
            .Columns(33).Width = 90
            .Columns(34).Name = "labourPer"
            .Columns(34).Width = 90
            .Columns(35).Name = "LabourAmt"
            .Columns(35).Width = 95
            .Columns(36).Name = "TotalCharges"
            .Columns(36).Width = 159
            .Columns(37).Name = "GrossAmount"
            .Columns(37).Width = 159
            .Columns(38).Name = "TotalNugs"
            .Columns(38).Width = 159
            .Columns(39).Name = "TotalWeight"
            .Columns(39).Width = 159
            .Columns(40).Name = "TotalCommission"
            .Columns(40).Width = 159
            .Columns(41).Name = "TotalMarketFees"
            .Columns(41).Width = 159
            .Columns(42).Name = "TotalRDF"
            .Columns(42).Width = 159
            .Columns(43).Name = "TotalTare"
            .Columns(43).Width = 159
            .Columns(44).Name = "TotalLabour"
            .Columns(44).Width = 159
            .Columns(45).Name = "Driver Name"
            .Columns(45).Width = 159
            .Columns(46).Name = "Mobile"
            .Columns(46).Width = 159
            .Columns(47).Name = "Remark"
            .Columns(47).Width = 159
        End With
    End Sub
    Sub retrive2()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        dt = clsFun.ExecDataTable("Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.AccountName, Vouchers.VehicleNo," _
                                  & "Transaction2.ItemName, Transaction2.Lot, Transaction2.Nug as TransNug, Transaction2.Weight, Transaction2.Rate," _
                                  & "Transaction2.Per, Transaction2.Amount,Transaction2.CommPer,Transaction2.CommAmt,Transaction2.MPer,Transaction2.MAmt," _
                                  & "Transaction2.RdfPer,Transaction2.RdfAmt,Transaction2.Tare,Transaction2.TareAmt,Transaction2.labour,Transaction2.LabourAmt," _
                                  & "Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, Vouchers.DiscountAmount, Vouchers.TotalCharges, vouchers.SubTotal,Vouchers.T1,Vouchers.T2,Vouchers.T3, " _
                                  & "Items.OtherName, Accounts.OtherName as AccountOtherName FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)" _
                                  & "INNER JOIN Items ON Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.AccountID = Accounts.ID  Where (Vouchers.ID=" & Val(txtid.Text) & ")")

        'dt = clsFun.ExecDataTable("Select Vouchers.Entrydate, Vouchers.billNo, Vouchers.AccountName, Vouchers.VehicleNo, Transaction2.ItemName, Transaction2.Lot, " _
        '                         & " Transaction2.Nug, Transaction2.Weight, Transaction2.Rate, Transaction2.Per, Transaction2.Amount, Vouchers.Nug, Vouchers.Kg, " _
        '                         & " Vouchers.BasicAmount, Vouchers.TotalAmount, Vouchers.TotalCharges, Accounts.OtherName as AccountOtherName, Items.OtherName, " _
        '                         & "Transaction2.CommPer, Transaction2.CommAmt, Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, " _
        '                         & "Transaction2.Tare, Transaction2.Tareamt, Transaction2.labour, Transaction2.LabourAmt,Vouchers.ID,Vouchers.DiscountAmount, Vouchers.SubTotal FROM Items INNER JOIN " _
        '                         & "(Accounts INNER JOIN (Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID) ON " _
        '                         & " Accounts.ID = Vouchers.AccountID) ON Items.ID = Transaction2.ItemID  Where (Vouchers.ID=" & Val(txtid.Text) & ")")
        '  tmpgrid.Rows.Clear()
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Lot").ToString()
                    .Cells(8).Value = .Cells(8).Value & Format(Val(dt.Rows(i)("TransNug").ToString()), "0.00")
                    .Cells(9).Value = .Cells(9).Value & Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = .Cells(10).Value & Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = .Cells(12).Value & Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(18).Value = .Cells(18).Value & Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = .Cells(19).Value & Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = .Cells(20).Value & Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = .Cells(21).Value & Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = .Cells(22).Value & Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("AccountOtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & Format(Val(dt.Rows(i)("CommPer").ToString()), "0.00") & vbCrLf
                    .Cells(27).Value = .Cells(27).Value & Format(Val(dt.Rows(i)("CommAmt").ToString()), "0.00") & vbCrLf
                    .Cells(28).Value = .Cells(28).Value & Format(Val(dt.Rows(i)("Mper").ToString()), "0.00") & vbCrLf
                    .Cells(29).Value = .Cells(29).Value & Format(Val(dt.Rows(i)("Mamt").ToString()), "0.00") & vbCrLf
                    .Cells(30).Value = .Cells(30).Value & Format(Val(dt.Rows(i)("RdfPer").ToString()), "0.00") & vbCrLf
                    .Cells(31).Value = .Cells(31).Value & Format(Val(dt.Rows(i)("rdfAmt").ToString()), "0.00") & vbCrLf
                    .Cells(32).Value = .Cells(32).Value & Format(Val(dt.Rows(i)("Tare").ToString()), "0.00") & vbCrLf
                    .Cells(33).Value = .Cells(33).Value & Format(Val(dt.Rows(i)("tareamt").ToString()), "0.00") & vbCrLf
                    .Cells(34).Value = .Cells(34).Value & Format(Val(dt.Rows(i)("Labour").ToString()), "0.00") & vbCrLf
                    .Cells(35).Value = .Cells(35).Value & Format(Val(dt.Rows(i)("Labouramt").ToString()), "0.00") & vbCrLf
                    .Cells(36).Value = .Cells(36).Value & Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                    .Cells(37).Value = .Cells(37).Value & Format(Val(dt.Rows(i)("SubTotal").ToString()), "0.00")
                    .Cells(38).Value = Format(Val(dt.Compute("Sum(TransNug)", "")), "0.00")
                    .Cells(39).Value = Format(Val(dt.Compute("Sum(Weight)", "")), "0.00")
                    .Cells(40).Value = Format(Val(dt.Compute("Sum(CommAmt)", "")), "0.00")
                    .Cells(41).Value = Format(Val(dt.Compute("Sum(Mamt)", "")), "0.00")
                    .Cells(42).Value = Format(Val(dt.Compute("Sum(rdfAmt)", "")), "0.00")
                    .Cells(43).Value = Format(Val(dt.Compute("Sum(tareamt)", "")), "0.00")
                    .Cells(44).Value = Format(Val(dt.Compute("Sum(Labouramt)", "")), "0.00")
                    .Cells(45).Value = dt.Rows(i)("T1").ToString()
                    .Cells(46).Value = dt.Rows(i)("T2").ToString()
                    .Cells(47).Value = dt.Rows(i)("T3").ToString()

                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Amount").ToString() & vbCrLf
                        Next
                    Else
                        .Cells(13).Value = ""
                        .Cells(14).Value = ""
                        .Cells(15).Value = ""
                        .Cells(16).Value = ""
                        .Cells(17).Value = ""
                    End If
                    'dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    'If dt1.Rows.Count > 0 Then
                    '    For j = 0 To dt1.Rows.Count - 1
                    '        .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                    '        .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                    '        .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                    '        .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                    '        .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Amount").ToString() & vbCrLf
                    '    Next
                    'Else
                    '    .Cells(13).Value = ""
                    '    .Cells(14).Value = ""
                    '    .Cells(15).Value = ""
                    '    .Cells(16).Value = ""
                    '    .Cells(17).Value = ""
                    'End If
                End With
                '  End If
            Next
        End If
        dt.Clear()
        dt1.Clear()
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.SendToBack()
        DgAccountSearch.Visible = False
        txtItem.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            'CreateAccount.txtGroup.Text = Val(clsFun.ExecScalarStr(" Select GroupName FROM AccountGroup WHERE ID =33"))
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            '    CustomerFill()
            DgAccountSearch.Visible = False
            txtItem.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 320
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 160
        DgAccountSearch.BringToFront() : DgAccountSearch.Visible = True
        retriveAccounts()
    End Sub

    'Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
    '    ' If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
    '    If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
    '    If dgCharges.Visible = True Then dgCharges.Visible = False
    'End Sub
    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        AccountRowColumns()
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
            Exit Sub
        End If
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))" & condtion & " order by AccountName")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
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
    Private Sub save()
        If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        dg1.ClearSelection()
        Dim cmd As SQLite.SQLiteCommand
        sql = "Insert Into Vouchers(Transtype, EntryDate,BillNo,AccountID, AccountName,VehicleNo,Nug,Kg,BasicAmount,DiscountAmount,TotalAmount,TotalCharges,Subtotal,T1,T2,T3) Values (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@3", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@4", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@5", txtAccount.Text)
            cmd.Parameters.AddWithValue("@6", txtVehicleNo.Text)
            cmd.Parameters.AddWithValue("@7", txtTotalNug.Text)
            cmd.Parameters.AddWithValue("@8", txttotalWeight.Text)
            cmd.Parameters.AddWithValue("@9", txtbasicTotal.Text)
            cmd.Parameters.AddWithValue("@10", txttotalCharges.Text)
            cmd.Parameters.AddWithValue("@11", txtTotalNetAmount.Text)
            cmd.Parameters.AddWithValue("@12", txtotherCharges.Text)
            cmd.Parameters.AddWithValue("@13", txtTotGross.Text)
            cmd.Parameters.AddWithValue("@14", txtDriverName.Text)
            cmd.Parameters.AddWithValue("@15", txtMobile.Text)
            cmd.Parameters.AddWithValue("@16", txtRemark.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                clsFun.CloseConnection()
            End If
            VchId = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
            Dg1Record()
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            FootertextClear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub Dg1Record()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells("Item Name").Value <> "" Then
                    sql = "insert into Transaction2(EntryDate,VoucherID,TransType,AccountID,AccountName,ItemID,ItemName,Lot,Nug,Weight, " _
                        & " Rate, Per,Amount,Charges,TotalAmount,CommPer,CommAmt,MPer,MAmt,RdfPer,RdfAmt," _
                        & "Tare,TareAmt,Labour, LabourAmt,CrateID,Cratemarka,CrateQty, MaintainCrate)  values('" & SqliteEntryDate & "'," & VchId & ", '" & Me.Text & "'," &
                             "'" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "'," & Val(.Cells("ItemID").Value) & "," &
                             "'" & .Cells("Item Name").Value & "','" & .Cells("Lot No").Value & "', " &
                             " '" & .Cells("Nug").Value & "','" & Val(.Cells("Weight").Value) & "','" & Val(.Cells("Rate").Value) & "'," &
                             " '" & .Cells("per").Value & "','" & Val(.Cells("Amount").Value) & "'," &
                             "'" & Val(.Cells("TotalCharges").Value) & "'," &
                             " '" & .Cells("Total").Value & "','" & Val(.Cells("CommPer").Value) & "'," &
                             " '" & .Cells("CommAmt").Value & "','" & Val(.Cells("MPer").Value) & "','" & Val(.Cells("MAmt").Value) & "'," &
                             " '" & .Cells("RdfPer").Value & "','" & Val(.Cells("Rdfamt").Value) & "','" & Val(.Cells("TarePer").Value) & "'," &
                             " '" & .Cells("TareAmt").Value & "','" & Val(.Cells("LabourPer").Value) & "','" & Val(.Cells("LabourAmt").Value) & "'," &
                             " '" & .Cells("CrateID").Value & "','" & .Cells("CrateName").Value & "','" & Val(.Cells("CrateQty").Value) & "'," &
                             "'" & .Cells("CrateY/N").Value & "')"
                    Try
                        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                        If cmd.ExecuteNonQuery() > 0 Then count = +1
                        ' MsgBox("Successfully Inserted")
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        clsFun.CloseConnection()
                    End Try
                End If
            End With
        Next
        clsFun.CloseConnection()
    End Sub
    Private Sub insertLedger()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        ' Cutomer Account
        If txtAccountID.Text > 0 Then ''Party Account
            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, Val(txtAccountID.Text), txtAccount.Text, Val(txtTotalNetAmount.Text), "D", clsFun.ExecScalarStr("Select  ItemName ||' Nug : '||(nug)||', Weight : '|| (weight) ||', On '||(rate)||' /- '|| Per From Transaction2 where VoucherID=" & VchId & " and TransType='Standard Sale'"))
        End If
        ''Maal Khata Account
        If Val(txtbasicTotal.Text) > 0 Then ''Maal Khata Account
            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 29, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29"), Val(txtbasicTotal.Text), "C", txtAccount.Text)
        End If
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells("Item Name").Value <> "" Then
                    If Val(.Cells(10).Value) > 0 Then ''Commission Account
                        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 10, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10"), Val(.Cells(10).Value), "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text)
                    End If
                    If Val(.Cells(12).Value) > 0 Then ''Mandi Tax Account
                        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 30, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30"), Val(.Cells(12).Value), "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text)
                    End If
                    If Val(.Cells(14).Value) > 0 Then ''RDF Account
                        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 39, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39"), Val(.Cells(14).Value), "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text)
                    End If
                    If Val(.Cells(16).Value) > 0 Then ''Tare Account
                        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 4, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4"), Val(.Cells(16).Value), "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text)
                    End If
                    If Val(.Cells(18).Value) > 0 Then ''Labour Account
                        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 23, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23"), Val(.Cells(18).Value), "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text)
                    End If
                End If
            End With
        Next
    End Sub
    Private Sub UpdateCrateLedger()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If Val(.Cells(22).Value) > 0 Then ''Party Account
                    clsFun.CrateLedger(0, VchId, txtVoucherNo.Text, SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, "Crate Out", .Cells(21).Value, .Cells(20).Value, .Cells(22).Value, "", "", "", "")
                End If
            End With
        Next
    End Sub
    'Private Sub dg2Record()
    '    For Each row As DataGridViewRow In Dg2.Rows
    '        With row
    '            If .Cells("Charge Name").Value <> "" Then
    '                sql = "insert into ChargesTrans(VoucherID, ChargesID, ChargeName, OnValue, Calculate, ChargeType, Amount) values(" & VchId & "," & _
    '                    "'" & .Cells("ChargeID").Value & "','" & .Cells("Charge Name").Value & "','" & .Cells("On Value").Value & "'," & _
    '                    "'" & .Cells("Cal").Value & "','" & .Cells("+/-").Value & "','" & .Cells("Amount").Value & "')"
    '                Try
    '                    cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
    '                    If cmd.ExecuteNonQuery() > 0 Then count = +1
    '                Catch ex As Exception
    '                    MsgBox(ex.Message)
    '                    clsFun.CloseConnection()
    '                End Try
    '            End If
    '        End With
    '    Next
    'End Sub
    'Private Sub InsertCharges()
    '    Dim SqliteEntryDate As String = mskEntryDate.Text
    '    SqliteEntryDate = Date.Today.ToString("yyyy-MM-dd")
    '    For Each row As DataGridViewRow In Dg2.Rows
    '        'For Each row As DataGridViewRow In .Rows
    '        With row
    '            ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
    '            If .Cells("Charge Name").Value <> "" Then
    '                Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
    '                Dim ssql As String
    '                ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
    '                Dim AccName As String = ssql
    '                If .Cells(3).Value = "+" Then
    '                    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text)
    '                Else
    '                    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", Me.Text & ", Voucher No:" & txtVoucherNo.Text)
    '                End If
    '            End If
    '        End With
    '    Next
    'End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        Dim Dg2 As New DataGridView()
        BtnSave.Text = "&Update"
        BtnSave.BackColor = Color.Coral
        BtnSave.Image = My.Resources.Edit
        BtnDelete.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & id
        Dim sql As String = "Select * from Transaction2 where VoucherID=" & id
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds, "b")
        ad2.Fill(ds, "c")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("AccountID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("DiscountAmount").ToString()), "0.00")
            txtTotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("billNo").ToString()
            txtotherCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txtTotGross.Text = Format(Val(ds.Tables("a").Rows(0)("SubTotal").ToString()), "0.00")
            txtDriverName.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtMobile.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtRemark.Text = ds.Tables("a").Rows(0)("T3").ToString()
        End If
        If ds.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells("Item Name").Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells("Lot No").Value = ds.Tables("b").Rows(i)("Lot").ToString()
                .Rows(i).Cells("Nug").Value = Format(Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells("Weight").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells("Rate").Value = Format(Val(ds.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells("per").Value = ds.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells("ItemID").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells("Total").Value = Format(Val(ds.Tables("b").Rows(i)("TotalAmount").ToString()), "0.00")
                .Rows(i).Cells("CommPer").Value = Format(Val(ds.Tables("b").Rows(i)("CommPer").ToString()), "0.00")
                .Rows(i).Cells("CommAmt").Value = Format(Val(ds.Tables("b").Rows(i)("CommAmt").ToString()), "0.00")
                .Rows(i).Cells("Mper").Value = Format(Val(ds.Tables("b").Rows(i)("MPer").ToString()), "0.00")
                .Rows(i).Cells("MAmt").Value = Format(Val(ds.Tables("b").Rows(i)("Mamt").ToString()), "0.00")
                .Rows(i).Cells("RDFPer").Value = Format(Val(ds.Tables("b").Rows(i)("RDFPer").ToString()), "0.00")
                .Rows(i).Cells("RDFAmt").Value = Format(Val(ds.Tables("b").Rows(i)("RDFAmt").ToString()), "0.00")
                .Rows(i).Cells("TarePer").Value = Format(Val(ds.Tables("b").Rows(i)("Tare").ToString()), "0.00")
                .Rows(i).Cells("TareAmt").Value = Format(Val(ds.Tables("b").Rows(i)("TareAmt").ToString()), "0.00")
                .Rows(i).Cells("LabourPer").Value = Format(Val(ds.Tables("b").Rows(i)("Labour").ToString()), "0.00")
                .Rows(i).Cells("LabourAmt").Value = Format(Val(ds.Tables("b").Rows(i)("LabourAmt").ToString()), "0.00")
                .Rows(i).Cells("CrateName").Value = ds.Tables("b").Rows(i)("Cratemarka").ToString()
                .Rows(i).Cells("CrateQty").Value = ds.Tables("b").Rows(i)("CrateQty").ToString()
                .Rows(i).Cells("CrateY/N").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                Dim tmpamt As Double = Val(ds.Tables("b").Rows(i)("CommAmt").ToString()) + Val(ds.Tables("b").Rows(i)("Mamt").ToString()) + Val(ds.Tables("b").Rows(i)("RDFAmt").ToString()) + Val(ds.Tables("b").Rows(i)("TareAmt").ToString()) + Val(ds.Tables("b").Rows(i)("LabourAmt").ToString())
                .Rows(i).Cells("TotalCharges").Value = Format(tmpamt, "0.00")
            Next
        End With
        If ds.Tables("c").Rows.Count > 0 Then
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        dg1.ClearSelection()
        Dg2.ClearSelection()
    End Sub
    Private Sub UpdateRecord()
        If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        dg1.ClearSelection()
        'Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicleNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  AccountID='" & Val(txtAccountID.Text) & "', AccountName='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtbasicTotal.Text & "', TotalAmount='" & txtTotalNetAmount.Text & "',TotalCharges='" & txtotherCharges.Text & "',DiscountAmount='" & txttotalCharges.Text & "'," _
                                & " Subtotal= '" & txtTotGross.Text & "',T1= '" & txtDriverName.Text & "',T2= '" & txtMobile.Text & "',T3= '" & txtRemark.Text & "'  where ID =" & txtid.Text & ""

        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
            End If
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & txtid.Text & ";DELETE from Transaction2 WHERE VoucherID=" & txtid.Text & ";DELETE from ChargesTrans WHERE VoucherID=" & txtid.Text & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & txtid.Text & "") > 0 Then
            End If
            VchId = Val(txtid.Text)
            Dg1Record()
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
            FootertextClear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub delete()
        If MessageBox.Show("Sure ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & txtid.Text & "") > 0 Then
            End If
            If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & txtid.Text & "") > 0 Then
            End If
            If clsFun.ExecNonQuery("DELETE from Transaction2 WHERE VoucherID=" & txtid.Text & "") > 0 Then
            End If
            If clsFun.ExecNonQuery("DELETE from ChargesTrans WHERE VoucherID=" & txtid.Text & "") > 0 Then
            End If
            If clsFun.ExecNonQuery("DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & txtid.Text & "") > 0 Then
            End If
            MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Saved")
            cleartxt()
            FootertextClear()
            dg1.Rows.Clear()
            BtnSave.Text = "&Save"
        End If
    End Sub

    Private Sub itemfill()
        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        txtComPer.Text = clsFun.ExecScalarStr(" Select CommisionPer FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        txtMPer.Text = clsFun.ExecScalarStr(" Select UserChargesPer FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        txtTare.Text = clsFun.ExecScalarStr(" Select tare FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        txtLabour.Text = clsFun.ExecScalarStr(" Select Labour FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        txtRdfPer.Text = clsFun.ExecScalarStr(" Select rdfper FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        txtKg.Text = clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        '     End If
    End Sub
    Private Sub VNumber()
        vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        txtVoucherNo.Text = vno + 1
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 24
        dg1.Columns(0).Name = "Item Name"
        dg1.Columns(0).Width = 371
        dg1.Columns(1).Name = "Lot No"
        dg1.Columns(1).Width = 119
        dg1.Columns(2).Name = "Nug"
        dg1.Columns(2).Width = 119
        dg1.Columns(3).Name = "Weight"
        dg1.Columns(3).Width = 119
        dg1.Columns(4).Name = "Rate"
        dg1.Columns(4).Width = 119
        dg1.Columns(5).Name = "per"
        dg1.Columns(5).Width = 87
        dg1.Columns(6).Name = "Amount"
        dg1.Columns(6).Width = 120
        dg1.Columns(7).Name = "Total"
        dg1.Columns(7).Width = 120
        dg1.Columns(8).Name = "ItemID"
        dg1.Columns(8).Width = 200
        dg1.Columns(9).Name = "CommPer"
        dg1.Columns(9).Width = 260
        dg1.Columns(10).Name = "CommAmt"
        dg1.Columns(10).Width = 200
        dg1.Columns(11).Name = "MPer"
        dg1.Columns(11).Width = 200
        dg1.Columns(12).Name = "MAmt"
        dg1.Columns(12).Width = 100
        dg1.Columns(13).Name = "RDFPer"
        dg1.Columns(13).Width = 200
        dg1.Columns(14).Name = "RdfAmt"
        dg1.Columns(14).Width = 200
        dg1.Columns(15).Name = "TarePer"
        dg1.Columns(15).Width = 200
        dg1.Columns(16).Name = "TareAmt"
        dg1.Columns(16).Width = 200
        dg1.Columns(17).Name = "LabourPer"
        dg1.Columns(17).Width = 200
        dg1.Columns(18).Name = "LabourAmt"
        dg1.Columns(18).Width = 200
        dg1.Columns(19).Name = "CrateY/N"
        dg1.Columns(19).Width = 200
        dg1.Columns(20).Name = "CrateName"
        dg1.Columns(20).Width = 200
        dg1.Columns(21).Name = "CrateID"
        dg1.Columns(21).Width = 200
        dg1.Columns(22).Name = "CrateQty"
        dg1.Columns(22).Width = 200
        dg1.Columns(23).Name = "TotalCharges"
        dg1.Columns(23).Visible = 200
    End Sub
    Sub calc()

        txtTotalNug.Text = Format(0, "0.00")
        txtbasicTotal.Text = Format(0, "0.00")
        txtTotGross.Text = Format(0, "0.00")
        txttotalWeight.Text = Format(0, "0.00")
        txttotalCharges.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1  ''''For Items Dg1
            txtTotalNug.Text = Format(Val(txtTotalNug.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txttotalWeight.Text = Format(Val(txttotalWeight.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtbasicTotal.Text = Format(Val(txtbasicTotal.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txttotalCharges.Text = Format(Val(txttotalCharges.Text) + Val(dg1.Rows(i).Cells(23).Value), "0.00")
            txtTotGross.Text = Format(Val(txtTotGross.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
        Next
    End Sub
    Private Sub cbitem_GotFocus(sender As Object, e As EventArgs)
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper <> DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper = DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            itemfill() : AcBal()
            DgAccountSearch.Visible = False
            '  txtCustomer.SelectionStart = 0 : txtCustomer.SelectionLength = Len(txtCustomer.Text)
            '  txtAccount.Focus()
        Else
            txtAccount.Focus()
        End If
    End Sub
    Private Sub txtLotNo_GotFocus(sender As Object, e As EventArgs) Handles txtLotNo.GotFocus
        If txtItem.Text = "" Then txtItem.Focus() : Exit Sub
        If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
        If txtItem.Text.ToUpper <> dgItemSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtItem.Focus() : Exit Sub
        If txtItem.Text = "" Then txtItem.Focus() : Exit Sub
        If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
        If txtItem.Text.ToUpper = dgItemSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
            If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
            If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
            txtNug.SelectionStart = 0 : txtNug.SelectionLength = Len(txtNug.Text)
        Else
            txtItem.Focus()
        End If
        If lblCrate.Text = "Y" Then
            clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
            pnlMarka.Visible = True
            pnlMarka.BringToFront()
        ElseIf lblCrate.Text = "N" Then
            cbCrateMarka.Text = ""
            cbCrateMarka.SelectedValue = "0"
        End If
    End Sub

    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        ItemRowColumns()
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper <> DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper = DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            itemfill() : AcBal()
            DgAccountSearch.Visible = False
            txtItem.SelectionStart = 0 : txtItem.SelectionLength = Len(txtItem.Text)
            '  txtAccount.Focus()
        Else
            txtAccount.Focus()
        End If
    End Sub
    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        itemfill()
        dgItemSearch.Visible = False
        txtLotNo.Focus()
        Dim CutStop As String = String.Empty
    End Sub
    Private Sub ItemRowColumns()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 186
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 186
        retriveItems()
    End Sub
    Private Sub txtItem_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItem.KeyDown
        If e.KeyCode = Keys.Down Then dgItemSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
            Dim CutStop As String = String.Empty
        End If
    End Sub
    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        ItemRowColumns()
        dgItemSearch.Visible = True
        If txtItem.Text.Trim() <> "" Then
            dgItemSearch.Visible = True
            retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
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
    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtItem.Clear()
            txtItemID.Clear()
            txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            itemfill()
            dgItemSearch.Visible = False
            txtLotNo.Focus()
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub txtRate_GotFocus(sender As Object, e As EventArgs) Handles txtRate.GotFocus
        txtRate.SelectionStart = 0 : txtRate.SelectionLength = Len(txtRate.Text)
    End Sub

    Private Sub txtVehicleNo_GotFocus(sender As Object, e As EventArgs) Handles txtVehicleNo.GotFocus
        pnlSendingDetails.Visible = True
    End Sub
    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown, txtVehicleNo.KeyDown,
          txtLotNo.KeyDown, txtKg.KeyDown, txtRate.KeyDown, Cbper.KeyDown, txtAccount.KeyDown, txtItem.KeyDown, txtDriverName.KeyDown,
          txtMobile.KeyDown, txtRemark.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus()
        End Select

        If txtAccount.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
            Dim tmpID As String = Val(txtAccountID.Text)
            If e.KeyCode = Keys.F1 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                CreateAccount.FillContros(tmpID)
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.F3 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
            End If
            Dim tmpID As String = Val(txtItemID.Text)
            If e.KeyCode = Keys.F1 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                Item_form.FillContros(tmpID)
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
            End If
        End If
    End Sub
    Private Sub SpeedCalculation()
        If Cbper.SelectedIndex = 0 Then
            txtBasicAmount.Text = Format(Val(txtNug.Text) * Val(txtRate.Text), "0.00")
            txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
            txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
            lblTotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
            txtTotal.Text = Val(lblTotCharges.Text) + Val(txtBasicAmount.Text)
        ElseIf Cbper.SelectedIndex = 1 Then
            txtBasicAmount.Text = Format(Val(txtKg.Text) * Val(txtRate.Text), "0.00")
            txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
            txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
            lblTotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
            txtTotal.Text = Format(Val(lblTotCharges.Text) + Val(txtBasicAmount.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 40 * Val(txtKg.Text), "0.00")
            txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
            txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
            txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
            lblTotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
            txtTotal.Text = Format(Val(lblTotCharges.Text) + Val(txtBasicAmount.Text), "0.00")

        End If
    End Sub


    'Private Sub CbCharges_KeyDown(sender As Object, e As KeyEventArgs)
    '    If e.KeyCode = Keys.Enter Then
    '        e.SuppressKeyPress = True
    '        SendKeys.Send("{TAB}")
    '    End If
    '    Select Case e.KeyCode
    '        Case Keys.End
    '            e.Handled = True
    '            BtnSave.Focus()
    '    End Select
    '    If txtCharges.Focused Then
    '        If e.KeyCode = Keys.F3 Then
    '            ChargesForm.MdiParent = MainScreenForm
    '            ChargesForm.Show()
    '            If Not ChargesForm Is Nothing Then
    '                ChargesForm.BringToFront()
    '            End If
    '        End If
    '        If e.KeyCode = Keys.F1 Then
    '            Dim tmpChargeID As Integer = (Dg2.SelectedRows(0).Cells(0).Value)
    '            ChargesForm.MdiParent = MainScreenForm
    '            ChargesForm.Show()
    '            ChargesForm.FillContros(tmpChargeID)
    '            If Not ChargesForm Is Nothing Then
    '                ChargesForm.BringToFront()
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
            txtLotNo.Text = dg1.SelectedRows(0).Cells(1).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
            txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
            txtBasicAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtTotal.Text = dg1.SelectedRows(0).Cells(7).Value
            '  cbitem.SelectedValue = dg1.SelectedRows(0).Cells(8).Value
            txtComPer.Text = dg1.SelectedRows(0).Cells(9).Value
            txtComAmt.Text = dg1.SelectedRows(0).Cells(10).Value
            txtMPer.Text = dg1.SelectedRows(0).Cells(11).Value
            txtMAmt.Text = dg1.SelectedRows(0).Cells(12).Value
            txtRdfPer.Text = dg1.SelectedRows(0).Cells(13).Value
            txtRdfAmt.Text = dg1.SelectedRows(0).Cells(14).Value
            txtTare.Text = dg1.SelectedRows(0).Cells(15).Value
            txtTareAmt.Text = dg1.SelectedRows(0).Cells(16).Value
            txtLabour.Text = dg1.SelectedRows(0).Cells(17).Value
            txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(18).Value
            lblCrate.Text = dg1.SelectedRows(0).Cells(19).Value
            cbCrateMarka.Text = dg1.SelectedRows(0).Cells(20).Value
            ' cbCrateMarka.SelectedValue = dg1.SelectedRows(0).Cells(21).Value
            txtCrateQty.Text = dg1.SelectedRows(0).Cells(22).Value
            lblTotCharges.Text = dg1.SelectedRows(0).Cells(23).Value
            e.SuppressKeyPress = True
            '    dg1.ClearSelection()
        End If
        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
                calc()
                'ClearDetails()
            End If
        End If
        '  e.SuppressKeyPress = True
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
        txtLotNo.Text = dg1.SelectedRows(0).Cells(1).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
        txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
        txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
        Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
        txtBasicAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        txtTotal.Text = dg1.SelectedRows(0).Cells(7).Value
        txtItemID.Text = dg1.SelectedRows(0).Cells(8).Value
        txtComPer.Text = dg1.SelectedRows(0).Cells(9).Value
        txtComAmt.Text = dg1.SelectedRows(0).Cells(10).Value
        txtMPer.Text = dg1.SelectedRows(0).Cells(11).Value
        txtMAmt.Text = dg1.SelectedRows(0).Cells(12).Value
        txtRdfPer.Text = dg1.SelectedRows(0).Cells(13).Value
        txtRdfAmt.Text = dg1.SelectedRows(0).Cells(14).Value
        txtTare.Text = dg1.SelectedRows(0).Cells(15).Value
        txtTareAmt.Text = dg1.SelectedRows(0).Cells(16).Value
        txtLabour.Text = dg1.SelectedRows(0).Cells(17).Value
        txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(18).Value
        lblCrate.Text = dg1.SelectedRows(0).Cells(19).Value
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(20).Value
        ' cbCrateMarka.SelectedValue = dg1.SelectedRows(0).Cells(21).Value
        txtCrateQty.Text = dg1.SelectedRows(0).Cells(22).Value
        '  dg1.ClearSelection()
    End Sub
    'Private Sub txtchargesAmount_KeyDown(sender As Object, e As KeyEventArgs)
    '    If e.KeyCode = Keys.Enter Then
    '        If Dg2.SelectedRows.Count = 1 Then
    '            Dg2.SelectedRows(0).Cells(0).Value = txtCharges.Text
    '            Dg2.SelectedRows(0).Cells(1).Value = txtOnValue.Text
    '            Dg2.SelectedRows(0).Cells(2).Value = txtCalculatePer.Text
    '            Dg2.SelectedRows(0).Cells(3).Value = txtPlusMinus.Text
    '            Dg2.SelectedRows(0).Cells(4).Value = txtchargesAmount.Text
    '            Dg2.SelectedRows(0).Cells(5).Value = txtChargeID.Text
    '            calc()
    '            txtCharges.Focus()
    '            Dg2.ClearSelection()
    '            cleartxtCharges()
    '        Else
    '            Dg2.Rows.Add(txtCharges.Text, txtOnValue.Text, txtCalculatePer.Text, txtPlusMinus.Text, txtchargesAmount.Text, txtChargeID.Text)
    '            calc()
    '            cleartxtCharges()
    '            txtCharges.Focus()
    '            Dg2.ClearSelection()
    '        End If
    '    End If
    'End Sub
    'Private Sub Dg2_KeyDown(sender As Object, e As KeyEventArgs)
    '    If e.KeyCode = Keys.Delete Then
    '        If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
    '            Dg2.Rows.Remove(Dg2.SelectedRows(0))
    '            calc()
    '            'ClearDetails()
    '        End If
    '    End If
    'End Sub
    'Private Sub Dg2_MouseDoubleClick(sender As Object, e As MouseEventArgs)
    '    txtCharges.Text = Dg2.SelectedRows(0).Cells(0).Value
    '    txtOnValue.Text = Dg2.SelectedRows(0).Cells(1).Value
    '    txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(2).Value
    '    txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(3).Value
    '    txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(4).Value
    '    txtChargeID.Text = Dg2.SelectedRows(0).Cells(5).Value
    'End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        mskEntryDate.Focus()
        If BtnSave.Text = "&Save" Then
            save()
        Else
            UpdateRecord()
        End If
        Dim res = MessageBox.Show("Do you want to Print Slip", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If res = Windows.Forms.DialogResult.Yes Then
            btnPrint.Enabled = True
            btnPrint.PerformClick()
        Else
        End If
    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        delete()
    End Sub
    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        'Me.Refresh()
        'clsFun.FillDropDownList(CbAccountName, "Select ID, AccountName From Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))", "AccountName", "Id", "")
        'clsFun.FillDropDownList(cbitem, "Select * From Items", "ItemName", "Id", "")
        'clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
        'clsFun.FillDropDownList(CbCharges, "Select * From Charges", "ChargeName", "Id", "")
    End Sub
    Private Sub cleartxt()
        txtLotNo.Text = "" : txtNug.Text = ""
        txtKg.Text = "" : txtBasicAmount.Text = ""
        txtTotal.Text = "" : lblTotCharges.Text = ""
        txtItem.Focus()
    End Sub
    Private Sub FootertextClear()
        txtid.Text = VchId
        Try
            lblInword.Text = AmtInWord(txtTotalNetAmount.Text)
        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
        VNumber()
        txtTotalNug.Text = "" : txtbasicTotal.Text = ""
        txttotalWeight.Text = "" : txtTotalNetAmount.Text = ""
        txttotalCharges.Text = "" : txtotherCharges.Text = ""
        txtVehicleNo.Text = "" : txtTotGross.Text = "" : txtRate.Text = ""
        BtnSave.Text = "&Save" : BtnSave.BackColor = Color.DarkTurquoise
        BtnSave.Image = My.Resources.Save
        : BtnDelete.Enabled = False
        txtLotNo.Text = "" : txtNug.Text = ""
        txtKg.Text = "" : txtBasicAmount.Text = ""
        txtTotal.Text = "" : lblTotCharges.Text = ""
        txtDriverName.Text = "" : txtMobile.Text = "" : txtRemark.Text = ""
        pnlSendingDetails.Visible = False
        dg1.Rows.Clear()
    End Sub
    'Private Sub cleartxtCharges()
    '    txtOnValue.Text = ""
    '    txtCalculatePer.Text = ""
    '    txtPlusMinus.Text = ""
    '    txtchargesAmount.Text = ""
    'End Sub
    'Private Sub ChargesCalculation()
    '    ' If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
    '    If calctype = "Percentage" Then
    '        If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
    '        txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text) / 100, "#######.00")
    '    ElseIf calctype = "Nug" Then
    '        If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
    '        txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "#######.00")
    '    ElseIf calctype = "Weight" Then
    '        If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
    '        txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "#######.00")
    '    ElseIf calctype = "Aboslute" Then
    '        If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
    '        txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "#######.00")
    '    End If
    'End Sub
    Private Sub Cbper_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbper.SelectedIndexChanged
        SpeedCalculation()
    End Sub

    Private Sub txtBasicAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBasicAmount.KeyDown
        If e.KeyCode = Keys.Enter Then txtTotal.Focus()
    End Sub

    Private Sub txtNug_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNug.KeyDown
        If lblCrate.Text = "Y" Then
            If e.KeyCode = Keys.Enter Then cbCrateMarka.Focus()
        Else
            If e.KeyCode = Keys.Enter Then txtKg.Focus()
        End If

    End Sub
    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtKg.KeyPress,
        txtRate.KeyPress, txtBasicAmount.KeyPress, txttotalCharges.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub


    Private Sub txtNug_TextChanged(sender As Object, e As EventArgs) Handles txtNug.TextChanged, Cbper.TextChanged,
        txtKg.TextChanged, txtRate.TextChanged, txtBasicAmount.TextChanged,
        txtTotalNetAmount.TextChanged, txtbasicTotal.TextChanged,
        txttotalCharges.TextChanged, txtTotal.TextChanged
        SpeedCalculation()
    End Sub

    Private Sub btnClose_Click1(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub txtTotal_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotal.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtNug.Text) = 0 And Val(txtKg.Text) = 0 Then
                MsgBox("please fill Nug or Kg", vbOKOnly, "Access Denied")
                txtNug.Focus()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(0).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(1).Value = txtLotNo.Text
                dg1.SelectedRows(0).Cells(2).Value = Format(Val(txtNug.Text), "0.00")
                dg1.SelectedRows(0).Cells(3).Value = Format(Val(txtKg.Text), "0.00")
                dg1.SelectedRows(0).Cells(4).Value = Format(Val(txtRate.Text), "0.00")
                dg1.SelectedRows(0).Cells(5).Value = Cbper.Text
                dg1.SelectedRows(0).Cells(6).Value = Format(Val(txtBasicAmount.Text), "0.00")
                dg1.SelectedRows(0).Cells(7).Value = Format(Val(txtTotal.Text), "0.00")
                dg1.SelectedRows(0).Cells(8).Value = Val(txtItemID.Text)
                dg1.SelectedRows(0).Cells(9).Value = Format(Val(txtComPer.Text), "0.00")
                dg1.SelectedRows(0).Cells(10).Value = Format(Val(txtComAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(11).Value = Format(Val(txtMPer.Text), "0.00")
                dg1.SelectedRows(0).Cells(12).Value = Format(Val(txtMAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(13).Value = Format(Val(txtRdfPer.Text), "0.00")
                dg1.SelectedRows(0).Cells(14).Value = Format(Val(txtRdfAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(15).Value = Format(Val(txtTare.Text), "0.00")
                dg1.SelectedRows(0).Cells(16).Value = Format(Val(txtTareAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(17).Value = Format(Val(txtLabour.Text), "0.00")
                dg1.SelectedRows(0).Cells(18).Value = Format(Val(txtLaboutAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(19).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(20).Value = cbCrateMarka.Text
                dg1.SelectedRows(0).Cells(21).Value = Val(cbCrateMarka.SelectedValue)
                dg1.SelectedRows(0).Cells(22).Value = Val(txtCrateQty.Text)
                dg1.SelectedRows(0).Cells(23).Value = Format(Val(lblTotCharges.Text), "0.00")

            Else

                dg1.Rows.Add(txtItem.Text, txtLotNo.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtKg.Text), "0.00"), Format(Val(txtRate.Text), "0.00"), Cbper.Text,
                           Format(Val(txtBasicAmount.Text), "0.00"), Format(Val(txtTotal.Text), "0.00"), Val(txtItemID.Text), Format(Val(txtComPer.Text), "0.00"),
                            Format(Val(txtComAmt.Text), "0.00"), Format(Val(txtMPer.Text), "0.00"), Format(Val(txtMAmt.Text), "0.00"), Format(Val(txtRdfPer.Text), "0.00"), Format(Val(txtRdfAmt.Text), "0.00"),
                            Format(Val(txtTare.Text), "0.00"), Format(Val(txtTareAmt.Text), "0.00"), Format(Val(txtLabour.Text), "0.00"), Format(Val(txtLaboutAmt.Text), "0.00"), lblCrate.Text,
                            cbCrateMarka.Text, Val(cbCrateMarka.SelectedValue), Val(txtCrateQty.Text), Format(Val(lblTotCharges.Text), "0.00"))
            End If
        End If
        '        End If
        calc() : cleartxt()

        dg1.ClearSelection()
    End Sub
    Private Sub txtCrateQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCrateQty.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlMarka.Visible = False
            txtKg.Focus()
            pnlMarka.SendToBack()
        End If
    End Sub
    Private Sub txtCrateQty_Leave(sender As Object, e As EventArgs) Handles txtCrateQty.Leave
        pnlMarka.Visible = False
        pnlMarka.SendToBack()
    End Sub
    Private Sub cbCrateMarka_KeyDown(sender As Object, e As KeyEventArgs) Handles cbCrateMarka.KeyDown
        If e.KeyCode = Keys.F3 Then
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            If Not CrateForm Is Nothing Then
                CrateForm.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then txtCrateQty.Focus()
    End Sub

    Private Sub cbCrateMarka_Leave(sender As Object, e As EventArgs) Handles cbCrateMarka.Leave
        If clsFun.ExecScalarInt("Select count(*)from Cratemarka") = 0 Then
            Exit Sub
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
                    sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " & _
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " & _
                        " P21,P22,P23,P24,P25,P26,P27,P28,P29,P30,P31,P32,P33,P34)" & _
                        "  values('" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," & _
                                "'" & lblInword.Text & "','" & .Cells(36).Value & "','" & .Cells(37).Value & "','" & .Cells(38).Value & "'," & _
                                "'" & .Cells(39).Value & "','" & .Cells(40).Value & "','" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "'," & _
                                "'" & .Cells(44).Value & "','" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "')"
                    Try
                        cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
                        ClsFunPrimary.ExecNonQuery(sql)
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        ClsFunPrimary.CloseConnection()
                    End Try
                End If
            End With
        Next
        ' clsFun.ExecNonQuery(sql)
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If txtid.Text <> "" Then
            TempRowColumn() : retrive2()
            ClsFunPrimary.CloseConnection()
            '    ClsFunPrimary.changeCompany()
            PrintRecord()
            Report_Viewer.printReport("\Challan.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    'Private Sub ChargesRowColums()
    '    dgCharges.ColumnCount = 3
    '    dgCharges.Columns(0).Name = "ID" : dgCharges.Columns(0).Visible = False
    '    dgCharges.Columns(1).Name = "Item Name" : dgCharges.Columns(1).Width = 130
    '    dgCharges.Columns(2).Name = "ApplyType" : dgCharges.Columns(2).Width = 130
    '    RetriveCharges()
    'End Sub
    'Private Sub RetriveCharges(Optional ByVal condtion As String = "")
    '    Dim dt As New DataTable
    '    dt = clsFun.ExecDataTable("Select * from Charges " & condtion & " order by ChargeName")
    '    Try
    '        If dt.Rows.Count > 0 Then
    '            dgCharges.Rows.Clear()
    '            For i = 0 To dt.Rows.Count - 1
    '                dgCharges.Rows.Add()
    '                With dgCharges.Rows(i)
    '                    .Cells(0).Value = dt.Rows(i)("id").ToString()
    '                    .Cells(1).Value = dt.Rows(i)("ChargeName").ToString()
    '                    .Cells(2).Value = dt.Rows(i)("ApplyType").ToString()
    '                End With
    '            Next

    '        End If
    '        dt.Dispose()

    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
    '    End Try
    'End Sub
    'Private Sub FillCharges()
    '    CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
    '    txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
    '    txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & txtChargeID.Text & "'")
    '    If CalcType = "Aboslute" Then
    '        txtOnValue.TabStop = False
    '        txtCalculatePer.TabStop = False
    '        txtchargesAmount.Focus()
    '    ElseIf CalcType = "Weight" Then
    '        txtOnValue.Text = txttotalWeight.Text
    '        txtOnValue.TabStop = True
    '        txtCalculatePer.TabStop = True
    '    ElseIf CalcType = "Percentage" Then
    '        txtOnValue.Text = txtbasicTotal.Text
    '        txtOnValue.TabStop = True
    '        txtCalculatePer.TabStop = True
    '    ElseIf CalcType = "Nug" Then
    '        txtOnValue.Text = txtTotalNug.Text
    '        txtOnValue.TabStop = True
    '        txtCalculatePer.TabStop = True
    '    End If
    'End Sub
    'Private Sub dgCharges_CellClick(sender As Object, e As DataGridViewCellEventArgs)
    '    txtCharges.Clear()
    '    txtChargeID.Clear()
    '    txtChargeID.Text = dgCharges.SelectedRows(0).Cells(0).Value
    '    txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
    '    dgCharges.Visible = False
    '    txtOnValue.Focus()
    '    FillCharges()

    'End Sub

    'Private Sub dgCharges_KeyDown(sender As Object, e As KeyEventArgs)
    '    If e.KeyCode = Keys.Enter Then
    '        txtCharges.Clear()
    '        txtChargeID.Clear()
    '        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
    '        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
    '        dgCharges.Visible = False : txtOnValue.Focus()
    '        FillCharges() : e.SuppressKeyPress = True
    '    End If
    '    If e.KeyCode = Keys.Back Then txtCharges.Focus()
    'End Sub

    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs)
        If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        ' If dgCharges.Visible = True Then dgCharges.Visible = False
    End Sub
    'Private Sub txtCharges_KeyUp(sender As Object, e As KeyEventArgs)
    '    ChargesRowColums()
    '    If txtCharges.Text.Trim() <> "" Then
    '        'dgCharges.Visible = True
    '        RetriveCharges(" Where upper(ChargeName) Like upper('%" & txtCharges.Text.Trim() & "%')")
    '    End If
    '    If e.KeyCode = Keys.Escape Then dgCharges.Visible = False
    'End Sub

    'Private Sub txtCharges_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    ChargesRowColums()
    '    dgCharges.Visible = True
    'End Sub


    Private Sub CbAccountName_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub



    Private Sub TextBox3_Leave(sender As Object, e As EventArgs) Handles txtRemark.Leave
        pnlSendingDetails.Visible = False : txtAccount.Focus()
    End Sub

    Private Sub txtVehicleNo_TextChanged(sender As Object, e As EventArgs) Handles txtVehicleNo.TextChanged

    End Sub
End Class