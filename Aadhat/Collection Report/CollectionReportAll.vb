Public Class CollectionReportAll
    Private Sub Collection_Report_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Collection_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim maxdate As String = String.Empty : Dim minDate As String = String.Empty
        '  mindate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger")
        ' maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger ")
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
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 17
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account name" : dg1.Columns(1).Width = 270
        dg1.Columns(2).Name = "Op. Bal." : dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Sales" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Charges" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Total" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Receipts" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Grand Total" : dg1.Columns(7).Width = 150
        dg1.Columns(8).Name = "Crate Qty" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "otherName" : dg1.Columns(9).Visible = False
        dg1.Columns(10).Name = "CommAmt" : dg1.Columns(10).Visible = False
        dg1.Columns(11).Name = "MTax" : dg1.Columns(11).Visible = False
        dg1.Columns(12).Name = "RDF" : dg1.Columns(12).Visible = False
        dg1.Columns(13).Name = "Tare" : dg1.Columns(13).Visible = False
        dg1.Columns(14).Name = "Labour" : dg1.Columns(14).Visible = False
        dg1.Columns(15).Name = "TotNugs" : dg1.Columns(15).Visible = False
        dg1.Columns(16).Name = "TotWeight" : dg1.Columns(16).Visible = False
    End Sub
    Sub calc()
        txttotalSale.Text = Format(0, "0.00") : txttotCharges.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotReceipt.Text = Format(0, "0.00")
        txtTotGross.Text = Format(0, "0.00") : txtTotCrate.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txttotalSale.Text = Format(Val(txttotalSale.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txttotCharges.Text = Format(Val(txttotCharges.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotReceipt.Text = Format(Val(txtTotReceipt.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotGross.Text = Format(Val(txtTotGross.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotCrate.Text = txtTotCrate.Text + Val(dg1.Rows(i).Cells(8).Value)
        Next
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub
    Private Sub retrive()

        Application.DoEvents()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        'Dim Condtion As String
        Dim count As Integer = 0
        'ssql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
        '       "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
        '       "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " & _
        '       " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
        '       " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0 and GroupID in(16,32) " & condtion & " Order by AccountName ;"

        ssql = "Select ID,Accountname,Area,DC,OtherName,Mobile1, Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" &
                         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " &
                         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" &
                         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal,  " &
                         " Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger  Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')  " &
                         " -(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID   and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID" &
                         " and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
                         " end),2) as  Opbal from Accounts Where RestBal<>0  and GroupID in(16,32) Order by upper(AccountName) ;"
        '   ssql = "Select ID,AccountName,OtherName from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))  "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            pnlWait.Visible = True : pb1.Minimum = 0 : pb1.Maximum = dt.Rows.Count
            For i = 0 To dt.Rows.Count - 1
                If Application.OpenForms().OfType(Of CollectionReportAll).Any = False Then Exit Sub
                pb1.Value = i
                If Val(dt.Rows(i)("Opbal").ToString()) > 0 Then
                    opbal = Format(Math.Abs(Val(dt.Rows(i)("Opbal").ToString())), "0.00") & " Dr"
                Else
                    opbal = Format(Math.Abs(Val(dt.Rows(i)("Opbal").ToString())), "0.00") & " Cr"
                End If
                acID = Val(dt.Rows(i)("ID").ToString())

                '''''''''''''crate Balance
                crateopbal = Val(0)
                ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'   union all" & _
                    " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(acID) > 0, "and AccountID=" & Val(acID) & "", "") & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'     "
                Dim cratetmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                Dim cratetmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(acID) & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                Dim cratetmpamt As String = Val(Val(crateopbal) + Val(cratetmpamtdr)) - Val(cratetmpamtcr)
                If cratetmpamt > 0 Then
                    crateopbal = Val(cratetmpamt)
                Else
                    crateopbal = -Val(cratetmpamt)
                End If
                dg1.Rows.Add()
                With dg1.Rows(i)
                    Application.DoEvents()
                    'If 1827 = Val(dt.Rows(i)("ID").ToString()) Then MsgBox("a")
                    .Cells(0).Value = dt.Rows(i)("Id").ToString()
                    .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(2).Value = opbal
                    Dim sales As String = clsFun.ExecScalarStr("Select sum(Amount) From Transaction2 Where AccountID=" & dt.Rows(i)("ID").ToString() & "  and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    .Cells(3).Value = IIf(Val(sales) <> 0, Format(Val(sales), "0.00"), "")
                    Dim Charges As String = clsFun.ExecScalarStr("Select sum(Charges) From Transaction2 Where AccountID=" & dt.Rows(i)("ID").ToString() & "  and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    .Cells(4).Value = IIf(Val(Charges) <> 0, Format(Val(Charges), "0.00"), "") 'Format((Val(Charges)), "0.00")
                    Dim TotalAmount As String = clsFun.ExecScalarStr("Select sum(TotalAmount) From Transaction2 Where AccountID=" & dt.Rows(i)("ID").ToString() & "  and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    .Cells(5).Value = IIf(Val(TotalAmount) <> 0, Format(Val(TotalAmount), "0.00"), "")  ' Format(Val(TotalAmount), "0.00")
                    Dim Receipts As String = clsFun.ExecScalarStr("Select sum(BasicAmount) from Vouchers where Transtype='Receipt' and accountID=" & Val(dt.Rows(i)("Id")) & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    .Cells(6).Value = IIf(Val(Receipts) <> 0, Format(Val(Receipts), "0.00"), "") 'Format(Val(Receipts), "0.00")
                    .Cells(7).Value = IIf(Val(dt.Rows(i)("RestBal").ToString()) >= 0, Format(Math.Abs(Val(dt.Rows(i)("RestBal").ToString())), "0.00") & " Dr", Format(Math.Abs(Val(dt.Rows(i)("RestBal").ToString())), "0.00") & " Cr")
                    .Cells(8).Value = IIf(crateopbal <= 0, IIf(Math.Abs(crateopbal) = 0, "", "(" & Math.Abs(crateopbal) & ")"), Math.Abs(crateopbal))
                    .Cells(9).Value = dt.Rows(i)("OtherName").ToString()
                    .Cells(10).Value = Format(Val(clsFun.ExecScalarDec(" Select ifnull(sum(CommAmt),0) as Commamt FROM Transaction2  WHERE AccountID = " & dt.Rows(i)("Id").ToString() & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(11).Value = Format(Val(clsFun.ExecScalarDec(" Select ifnull(sum(MAmt),0) FROM Transaction2  WHERE AccountID = " & dt.Rows(i)("Id").ToString() & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(12).Value = Format(Val(clsFun.ExecScalarDec(" Select ifnull(sum(RdfAmt),0) FROM Transaction2  WHERE AccountID = " & dt.Rows(i)("Id").ToString() & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(13).Value = Format(Val(clsFun.ExecScalarDec(" Select ifnull(sum(TareAmt),0) FROM Transaction2  WHERE AccountID = " & dt.Rows(i)("Id").ToString() & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(14).Value = Format(Val(clsFun.ExecScalarDec(" Select ifnull(sum(LabourAmt),0) FROM Transaction2  WHERE AccountID = " & dt.Rows(i)("Id").ToString() & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(15).Value = Format(Val(clsFun.ExecScalarDec(" Select ifnull(sum(Nug),0) FROM Transaction2  WHERE AccountID = " & dt.Rows(i)("Id").ToString() & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                    .Cells(16).Value = Format(Val(clsFun.ExecScalarDec(" Select ifnull(sum(Weight),0) FROM Transaction2  WHERE AccountID = " & dt.Rows(i)("Id").ToString() & " and EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")), "0.00")
                End With
            Next i
        End If
        pnlWait.Visible = False
        lblRecordCount.Visible = True
        lblRecordCount.Text = "Total Record : " & dg1.RowCount
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            Report_Viewer.printReport("\Collection.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub

    Private Sub PrintRecord()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        pnlWait.Visible = True
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            Application.DoEvents()
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With dg1.Rows(LastRecord)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "','" & MsktoDate.Text & "'," &
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                    "'" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "'," &
                      "'" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & txttotalSale.Text & "', " &
                    "'" & txttotCharges.Text & "','" & txtTotBasic.Text & "','" & txtTotReceipt.Text & "'," &
                    "'" & txtTotGross.Text & "','" & txtTotCrate.Text & "','" & .Cells(9).Value & "'," &
                        "'" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                    "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "'"
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            ' LastRecord = LastCount
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,P1, P2,P3, P4,P5,P6, P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20,P21,P22)  " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try

        Next
        pnlWait.Visible = False
    End Sub

    Private Sub PrintRecord1()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            Application.DoEvents()
            If Application.OpenForms().OfType(Of CollectionReportAll).Any = False Then Exit Sub
            pnlWait.Visible = True
            pb1.Minimum = 0
            pb1.Maximum = dg1.Rows.Count
            pb1.Value = IIf(row.Index = -1, 0, row.Index)
            With row
                sql = "insert into Printing(D1,D2,P1, P2,P3, P4,P5,P6, P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20,P21,P22) " &
                    " values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," &
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                    "'" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "'," &
                      "'" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & txttotalSale.Text & "', " &
                    "'" & txttotCharges.Text & "','" & txtTotBasic.Text & "','" & txtTotReceipt.Text & "'," &
                    "'" & txtTotGross.Text & "','" & txtTotCrate.Text & "','" & .Cells(9).Value & "'," &
                        "'" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                    "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
        pnlWait.Visible = False
        '  CircularProgressBar1.Visible = False
    End Sub
    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            Report_Viewer.printReport("\Reports\CollectionHindi.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
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

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
End Class