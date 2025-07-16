Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Xml
'Imports System.Threading.task
Public Class Change_Financial_Year
    Dim data As String = String.Empty

    Private Sub Change_Financial_Year_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        CompanyList.Enabled = True
    End Sub
    Private Sub Change_Financial_Year_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Change_Financial_Year_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Control.CheckForIllegalCrossThreadCalls = False
        Me.Top = 130 : Me.Left = 84
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.GhostWhite
        txtAccountID.Text = Val(CompanyList.dg1.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = CompanyList.dg1.SelectedRows(0).Cells(1).Value
        mskCurrFromDate.Text = CompanyList.dg1.SelectedRows(0).Cells(4).Value
        MskCurrToDate.Text = CompanyList.dg1.SelectedRows(0).Cells(5).Value
        txtCurrentPath.Text = CompanyList.dg1.SelectedRows(0).Cells(7).Value
        mskNewFromDate.Text = CDate(mskCurrFromDate.Text).AddYears(1).ToString("dd-MM-yyyy")
        mskNewtoDate.Text = CDate(MskCurrToDate.Text).AddYears(1).ToString("dd-MM-yyyy")
        mskNewFromDate.Focus() : Me.KeyPreview = True
        CompanyList.Enabled = False
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Public Sub CreateDb(ByVal dbname As String, ByVal compid As Integer)
        Dim CopyDestPath As String = String.Empty
        Dim tmpDataPath As String = String.Empty
        Dim i As Integer = 0
        directoryName = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last()
        ' If directoryName = "Data" Then directoryName = ""
        CopyDestPath = "Data\Data"
        tmpDataPath = "Data"
        For i = 1 To 100
            If Directory.Exists(Application.StartupPath & "\" & CopyDestPath & i) = False Then
                CompData = CopyDestPath & i & "\"
                tmpDataPath = tmpDataPath & i
                tmpDataPath = tmpDataPath & "\Data.db"
                Directory.CreateDirectory(Application.StartupPath & "\" & CopyDestPath & i)
                CopyDestPath = (Application.StartupPath & "\" & CopyDestPath & i).ToString()
                Exit For
            End If
        Next
        Dim ssql As String = ""
        CopyDirectory(Application.StartupPath & "\Data\" & directoryName & "\data.db", CopyDestPath)
        clsFun.ExecNonQuery("Update COMPANY set LinkedDB='" & CopyDestPath & "\data.db" & "'")
        GlobalData.PrvPath = GlobalData.ConnectionPath
        GlobalData.ConnectionPath = CopyDestPath & "\data.db"
        AcBal() : btnChangeYear.Text = "Successful"
    End Sub

    Private Shared Sub CopyDirectory(ByVal sourcePath As String, ByVal destPath As String)
        Dim CompData As String = String.Empty
        For Each file__1 As String In Directory.GetFiles(Path.GetDirectoryName(sourcePath))
            Dim dest As String = Path.Combine(destPath, Path.GetFileName(file__1))
            File.Copy(file__1, dest)
            CompData = CompData & file__1
        Next

        For Each folder As String In Directory.GetDirectories(Path.GetDirectoryName(sourcePath))
            Dim dest As String = Path.Combine(destPath, Path.GetFileName(folder))
            CopyDirectory(folder, dest)
        Next
    End Sub

    'Private Sub AcBal1()
    '    'lblStatus.Text = "Importing Balancing From Accounts..."
    '    Dim condtion As String
    '    Dim i As Integer
    '    Dim dt As New DataTable
    '    Dim ssql As String = "" : ProgressBar1.Visible = True
    '    '''''Account Balance Transfer---------
    '    Dim sql As String = String.Empty
    '    sql = "Select ID,Accountname,DC, Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
    '        " Where AccountID=Accounts.ID and DC='D')-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID " & _
    '        " and DC='C' ))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
    '        "  Where AccountID=Accounts.ID and DC='C') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
    '        " Where AccountID=Accounts.ID and DC='D'))  end),2) as  Restbal from Accounts   ;"
    '    dt = clsFun.ExecDataTable(sql)
    '    For i = 0 To dt.Rows.Count - 1
    '        Application.DoEvents()
    '        ProgressBar1.Maximum = dt.Rows.Count
    '        lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
    '        ProgressBar1.Value = i
    '        If lblAcBalance.Visible = True Then lblAcBalance.Visible = False Else lblAcBalance.Visible = True
    '        '  If 936 = Val(dt.Rows(i)("id").ToString()) Then MsgBox("a")
    '        If Val(dt.Rows(i)("Restbal").ToString()) = 0 Then
    '            ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , DC='" & dt.Rows(i)("DC").ToString() & "' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
    '        ElseIf Val(dt.Rows(i)("Restbal").ToString()) > 0 Then
    '            ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , Dc='Dr' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
    '        ElseIf Val(dt.Rows(i)("Restbal").ToString()) < 0 Then
    '            ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , Dc='Cr' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
    '        End If
    '    Next
    '    Dim a As Integer = clsFun.ExecNonQuery(ssql, True)
    '    lblAcBalance.Visible = True
    '    lblAcBalance.Text = "Accounts Updated..."
    '    'If a > 0 Then
    '    '    '''''Refresh Tables---------
    '    ssql = ""
    '    ssql = "Delete from Vouchers; Delete from sqlite_sequence where name='Vouchers';Delete from Ledger; Delete from sqlite_sequence where name='Ledger';Delete from Transaction1; Delete from sqlite_sequence where name='Transaction1'; " & _
    '           "CREATE TABLE TempPurchase AS SELECT * FROM Purchase;Delete from Purchase; Delete from sqlite_sequence where name='Purchase';delete from ChargesTrans;Delete from sqlite_sequence where name='ChargesTrans'; Delete from Licence;" & _
    '           "CREATE TABLE TempCrateVoucher AS SELECT * FROM CrateVoucher;Delete from CrateVoucher;Delete from sqlite_sequence where name='CrateVoucher';" & _
    '           " Update Company set Autosync='N',YearStart='" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "',YearEnd='" & CDate(mskNewtoDate.Text).ToString("yyyy-MM-dd") & "',CompData='" & tmpDataPath & "';"
    '    Dim a1 As Integer = clsFun.ExecNonQuery(ssql, True)

    '    ssql = ""
    '    dt = clsFun.ExecDataTable("Select CrateID,CrateName,AccountID,AccountName FROM TempCrateVoucher Where AccountID Not in(0,7) Group by CrateName,AccountID   order by AccountID ")
    '    Try
    '        If dt.Rows.Count > 0 Then
    '            For i = 0 To dt.Rows.Count - 1
    '                Application.DoEvents()
    '                'If Application.OpenForms().OfType(Of Change_Financial_Year).Any = False Then Exit Sub
    '                ProgressBar1.Maximum = dt.Rows.Count
    '                lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
    '                ProgressBar1.Value = i
    '                If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
    '                sql = "Select  ((Select ifnull(Sum(Qty),0) From TempCrateVoucher Where CrateType='Crate In'  and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "')" & _
    '                                    "-(Select ifnull(Sum(Qty),0) From TempCrateVoucher Where CrateType='Crate Out' and  AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "')) as  Restbal " & _
    '                                    " from TempCrateVoucher   where  AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID " & condtion & "  order by AccountName ;"
    '                Dim crateTotbal As String = clsFun.ExecScalarStr(sql)
    '                If Val(crateTotbal) <> 0 Then
    '                    Dim tmpamtdr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from TempCrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
    '                    Dim tmpamtcr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
    '                    tmpamt1 = Val(tmpamtdr1) - Val(tmpamtcr1) '- Val(opbal)
    '                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from TempCrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
    '                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from TempCrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
    '                    Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
    '                    If tmpamt <> 0 Then
    '                        If Val(tmpamtcr) > Val(tmpamtdr) Then
    '                            cratebal = Math.Abs(Val(tmpamt))
    '                            ssql = "Insert Into CrateVoucher (VoucherID,SlipNo,EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName,Qty,Remark,Rate,Amount,CashPaid) values" & _
    '                                "(0," & Val(i + 1) & ",'" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','Op Bal'," & Val(dt.Rows(i)("AccountID").ToString()) & ", " & _
    '                                " '" & dt.Rows(i)("AccountName").ToString() & "','Crate Out','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "', " & _
    '                                " '" & Val(cratebal) & "','Opening Crate Balance',0,0,'N')"
    '                            a1 = clsFun.ExecNonQuery(ssql, True)
    '                            If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
    '                        ElseIf Val(tmpamtcr) < Val(tmpamtdr) Then
    '                            cratebal = Math.Abs(Val(tmpamt))
    '                            ssql = "Insert Into CrateVoucher (VoucherID,SlipNo,EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName,Qty,Remark,Rate,Amount,CashPaid) values" & _
    '                                   "(0," & Val(i + 1) & ",'" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','Op Bal'," & Val(dt.Rows(i)("AccountID").ToString()) & ", " & _
    '                                   " '" & dt.Rows(i)("AccountName").ToString() & "','Crate In','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "', " & _
    '                                   " '" & Val(cratebal) & "','Opening Crate Balance',0,0,'N')"
    '                            a1 = clsFun.ExecNonQuery(ssql, True)
    '                            If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
    '                        End If
    '                    End If
    '                End If
    '            Next

    '        End If
    '    Catch ex As Exception

    '    End Try
    '    lblCrateBalance.Visible = True
    '    lblCrateBalance.Text = "Crates Updated..."

    '    ' ''''''        Update(Stock)
    '    ''Dim dt As New DataTable
    '    sql = String.Empty
    '    'sql = "Select AccountID,AccountName, StockHolderID,StockHolderName,StorageID,StorageName,ItemID, ItemName,Sum(Nug) as PurchaseNug, " & _
    '    '      " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale') and ItemID=TempPurchase.ItemID) as soldNug,(Sum(Nug) - " & _
    '    '      " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale')   and ItemID=TempPurchase.ItemID)) as RestNug " & _
    '    '      " From TempPurchase Group by ItemID   Having RestNug > 0  order by StockHolderName,ItemID,ItemName"
    '    sql = "Select AccountID,AccountName,StockHolderID,StockHolderName From TempPurchase Group By AccountID,StockHolderID"
    '    dt = clsFun.ExecDataTable(sql)
    '    Try
    '        If dt.Rows.Count > 0 Then
    '            For i = 0 To dt.Rows.Count - 1
    '                Application.DoEvents()
    '                ProgressBar1.Maximum = dt.Rows.Count
    '                lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
    '                ProgressBar1.Value = i
    '                If lblStockBalance.Visible = True Then lblStockBalance.Visible = False Else lblStockBalance.Visible = True
    '                Dim totQty As Integer = 0
    '                Dim accID As Integer = 0
    '                Dim accName As String = String.Empty
    '                Dim stockHolderID As Integer = 0
    '                Dim stockHolderName As String = String.Empty
    '                Dim StorageID As String = String.Empty
    '                Dim StorageName As String = String.Empty
    '                Dim ItemID As Integer = 0
    '                Dim ItemName As String = String.Empty
    '                Dim Nugs As Integer = 0
    '                Dim Weights As String = String.Empty
    '                Dim VehicleNo As String = String.Empty
    '                Dim Rate As String = String.Empty
    '                Dim PurcahseTypeName As String = String.Empty
    '                Dim VChID As Integer
    '                '  Dim VchId As Integer = 1
    '                sql = " Select VoucherID, AccountID,AccountName, StockHolderID,StockHolderName,VehicleNo,Weight,PurchaseTypeName,StorageID,StorageName,ItemID, ItemName,LotNo,Sum(Nug) as PurchaseNug,Rate,Per,MaintainCrate,CrateID,CrateName,CrateQty, " & _
    '                       " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot = TempPurchase.LotNo and ItemID=TempPurchase.ItemID " & _
    '                       " and PurchaseID=TempPurchase.VoucherID )as soldNug,(Sum(Nug) - " & _
    '                       " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot=TempPurchase.LotNo and ItemID=TempPurchase.ItemID" & _
    '                       " and PurchaseID=TempPurchase.VoucherID)) as RestNug From TempPurchase Where AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' and StockHolderID='" & Val(dt.Rows(i)("StockHolderID").ToString()) & "'  Group by ItemID,LotNo,VoucherID Having RestNug <> 0  " & _
    '                       " order by AccountName,ItemID,ItemName,LotNo"
    '                dt1 = clsFun.ExecDataTable(sql)

    '                If dt1.Rows.Count > 0 Then
    '                    sqll = "Insert Into Vouchers(Transtype, EntryDate,BillNo,SallerID, SallerName,VehicleNo,Nug,Kg,BasicAmount,DiscountAmount,TotalAmount,PurchaseType, " _
    '                         & "StorageID,StorageName,InvoiceID) Values ('Purchase','" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','" & VChID & "','" & Val(dt.Rows(i)("AccountID").ToString()) & "', " & _
    '                           " '" & dt.Rows(i)("AccountName").ToString() & "'," & Val(totQty) & ",0,0,0,0,0,'" & PurcahseTypeName & "','" & Val(StorageID) & "','" & StorageName & "'," & Val(VChID) & ")"
    '                    If clsFun.ExecNonQuery(sqll) Then
    '                        VChID = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
    '                        sql = String.Empty
    '                        For j = 0 To dt1.Rows.Count - 1
    '                            totQty = totQty + Val(dt1.Rows(j)("RestNug").ToString())
    '                            Weights = Val(Weights) + Val(dt1.Rows(j)("Weight").ToString())
    '                            sql = sql & "insert into Purchase(EntryDate,TransType,VoucherID,BillNo,VehicleNo,PurchaseTypeName,AccountID,AccountName,StorageID,StorageName, " _
    '                            & "ItemID,ItemName,LotNo, Nug, Weight,Rate,Per, Amount, MaintainCrate, CrateID, CrateName, CrateQty,StockHolderID,StockHolderName) values " _
    '                            & "('" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','Purchase'," & Val(VChID) & ", " & _
    '                            "'" & Val(i + 1) & "','" & Val(VChID) & "','Purchase'," & _
    '                            "" & Val(dt1.Rows(j)("AccountID").ToString()) & ",'" & dt1.Rows(j)("AccountName").ToString() & "'," & Val(dt1.Rows(j)("StorageID").ToString()) & ", " & _
    '                            "'" & dt1.Rows(j)("StorageName").ToString() & "'," & _
    '                            "" & Val(dt1.Rows(j)("ItemID").ToString()) & ",'" & dt1.Rows(j)("ItemName").ToString() & "','" & dt1.Rows(j)("LotNo").ToString() & "', " & _
    '                            "" & Val(dt1.Rows(j)("RestNug").ToString()) & "," & Val(0) & "," & Val(dt1.Rows(j)("Rate").ToString()) & ",'" & dt1.Rows(j)("Per").ToString() & "'," & _
    '                            "" & Val(0) & ",'" & dt1.Rows(j)("MaintainCrate").ToString() & "'," & Val(dt1.Rows(j)("CrateID").ToString()) & ",'" & dt1.Rows(j)("CrateName").ToString() & "', " & _
    '                            "" & Val(dt1.Rows(j)("CrateQty").ToString()) & "," & IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, (Val(28)), Val(dt1.Rows(j)("AccountID").ToString())) & ", " & _
    '                            "'" & IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, "Mall Khata Purchase A/c", dt1.Rows(j)("AccountName").ToString()) & "');"
    '                            stockHolderID = IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, (Val(28)), Val(dt1.Rows(j)("AccountID").ToString()))
    '                            stockHolderName = IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, "Mall Khata Purchase A/c", dt1.Rows(j)("AccountName").ToString())
    '                            PurcahseTypeName = dt1.Rows(j)("PurchaseTypeName").ToString()
    '                            StorageID = dt1.Rows(j)("StorageID").ToString()
    '                            StorageName = dt1.Rows(j)("StorageName").ToString()
    '                            VehicleNo = dt1.Rows(j)("VehicleNo").ToString()
    '                        Next
    '                        sqll = ""
    '                        sqll = "Update Vouchers Set TransType='Purchase', EntryDate='" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "',BillNo='" & Val(VChID) & "'," _
    '                            & "SallerID='" & Val(dt.Rows(i)("AccountID").ToString()) & "', SallerName='" & dt.Rows(i)("AccountName").ToString() & "',VehicleNo='" & VehicleNo & "'," _
    '                            & "Nug='" & Val(totQty) & "',Kg='" & Val(Weights) & "',BasicAmount='" & Val(0) & "'," _
    '                            & " DiscountAmount= '" & Val(0) & "',TotalAmount='" & Val(0) & "'," _
    '                            & " PurchaseType='" & PurcahseTypeName & "',StorageID='" & Val(StorageID) & "',StorageName='" & StorageName & "', " _
    '                            & "InvoiceID='" & Val(VChID) & "' Where ID=" & Val(VChID) & ""
    '                        If clsFun.ExecNonQuery(sqll) Then
    '                            clsFun.ExecNonQuery(sql)
    '                        End If

    '                    End If

    '                End If
    '            Next
    '        End If

    '    Catch ex As Exception

    '    End Try

    '    '" " & _

    '    ssql = "Delete from Transaction2; Delete from sqlite_sequence where name='Transaction2'; drop table if  exists TempCrateVoucher;drop table if  exists TempPurchase;"
    '    a1 = clsFun.ExecNonQuery(ssql, True)
    '    lblStockBalance.Visible = True
    '    lblStockBalance.Text = "Stock Updated..."
    '    If a1 > 0 Then
    '        MsgBox("Financial Year Changed Successfully...")
    '        CompanyList.BtnRetrive.PerformClick()
    '        Me.Close()
    '    End If

    '    '  End If
    'End Sub
    Private Sub AcBal()
        'lblStatus.Text = "Importing Balancing From Accounts..."
        ' Dim condtion As String
        Dim tmpDataPath As String = String.Empty
        Dim i As Integer
        Dim dt As New DataTable
        Dim ssql As String = "" : ProgressBar1.Visible = True
        '''''Account Balance Transfer---------
        Dim sql As String = String.Empty
        sql = "Select ID,Accountname,DC, Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
    " Where AccountID=Account_AcGrp.ID and DC='D')-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID " &
    " and DC='C' ))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
    "  Where AccountID=Account_AcGrp.ID and DC='C') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
    " Where AccountID=Account_AcGrp.ID and DC='D'))  end),2) as  Restbal from Account_AcGrp Where ParentID Not In(22,23,24,25,26,27);"
        dt = clsFun.ExecDataTable(sql)
        For i = 0 To dt.Rows.Count - 1
            Application.DoEvents()
            ProgressBar1.Maximum = dt.Rows.Count
            lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
            ProgressBar1.Value = i
            If lblAcBalance.Visible = True Then lblAcBalance.Visible = False Else lblAcBalance.Visible = True
            '  If 936 = Val(dt.Rows(i)("id").ToString()) Then MsgBox("a")
            If Val(dt.Rows(i)("Restbal").ToString()) = 0 Then
                ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , DC='" & dt.Rows(i)("DC").ToString() & "' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
            ElseIf Val(dt.Rows(i)("Restbal").ToString()) > 0 Then
                ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , Dc='Dr' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
            ElseIf Val(dt.Rows(i)("Restbal").ToString()) < 0 Then
                ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , Dc='Cr' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
            End If
        Next
        Dim GroupID As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(ac.ID) AS Concatenated_IDs FROM Accounts AS ac INNER JOIN AccountGroup AS grp ON ac.Groupid = grp.ID" & _
                              " WHERE grp.UnderGroupID in(22,23,24,25,26,27) or Ac.GroupID  in (22,23,24,25,26,27);")
        clsFun.ExecNonQuery("Update Accounts Set Opbal=0 Where ID in(" & GroupID & ") ")
        Dim a As Integer = clsFun.ExecNonQuery(ssql, True)
        ssql = String.Empty
        Dim GPGl As Decimal = clsFun.ExecScalarDec(" Select Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D')" & _
                                    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' )) " & _
                                    " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C')" & _
                                    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D'))  end) as  GroupBal from Accounts " & _
                                    " Where  GroupID  in(22,23,24,25,26,27);")

        If Val(GPGl) > 0 Then
            ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(GPGl)) & " , Dc='Dr' where id=38;"
        ElseIf Val(GPGl) < 0 Then
            ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(GPGl)) & " , Dc='Cr' where id=38;"
        End If
        a = clsFun.ExecNonQuery(ssql, True)
        lblAcBalance.Visible = True
        lblAcBalance.Text = "Accounts Updated..."
        'If a > 0 Then
        '    '''''Refresh Tables---------
        ssql = ""
        ssql = "Delete from Vouchers; Delete from sqlite_sequence where name='Vouchers';Delete from Ledger; Delete from sqlite_sequence where name='Ledger';Delete from Transaction1; Delete from sqlite_sequence where name='Transaction1'; " &
               "CREATE TABLE TempPurchase AS SELECT * FROM Purchase;Delete from Purchase; Delete from sqlite_sequence where name='Purchase';delete from ChargesTrans;Delete from sqlite_sequence where name='ChargesTrans'; Delete from Licence;" &
               "CREATE TABLE TempCrateVoucher AS SELECT * FROM CrateVoucher;Delete from CrateVoucher;Delete from sqlite_sequence where name='CrateVoucher';" &
               " Update Company set Autosync='N',OrganizationID='',Password='',SyncDate='',YearStart='" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "',YearEnd='" & CDate(mskNewtoDate.Text).ToString("yyyy-MM-dd") & "',CompData='" & GlobalData.ConnectionPath & "',lINKEDdB='" & GlobalData.PrvPath & "';"
        Dim a1 As Integer = clsFun.ExecNonQuery(ssql, True)
        ssql = ""
        dt = clsFun.ExecDataTable("Select CrateID,CrateName,AccountID,AccountName FROM TempCrateVoucher Where AccountID Not in(0) Group by CrateName,AccountID   order by AccountID ")
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Application.DoEvents()
                    'If Application.OpenForms().OfType(Of Change_Financial_Year).Any = False Then Exit Sub
                    ProgressBar1.Maximum = dt.Rows.Count
                    lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
                    ProgressBar1.Value = i
                    If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
                    sql = "Select  ((Select ifnull(Sum(Qty),0) From TempCrateVoucher Where CrateType='Crate In'  and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "')" &
                                        "-(Select ifnull(Sum(Qty),0) From TempCrateVoucher Where CrateType='Crate Out' and  AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "')) as  Restbal " &
                                        " from TempCrateVoucher   where  AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID   order by AccountName ;"
                    Dim crateTotbal As String = clsFun.ExecScalarStr(sql)
                    If Val(crateTotbal) <> 0 Then
                        Dim tmpamtdr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from TempCrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        Dim tmpamtcr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        tmpamt1 = Val(tmpamtdr1) - Val(tmpamtcr1) '- Val(opbal)
                        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from TempCrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from TempCrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
                        If tmpamt <> 0 Then
                            If Val(tmpamtcr) > Val(tmpamtdr) Then
                                cratebal = Math.Abs(Val(tmpamt))
                                ssql = "Insert Into CrateVoucher (VoucherID,SlipNo,EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName,Qty,Remark,Rate,Amount,CashPaid) values" &
                                    "(0," & Val(i + 1) & ",'" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','Op Bal'," & Val(dt.Rows(i)("AccountID").ToString()) & ", " &
                                    " '" & dt.Rows(i)("AccountName").ToString() & "','Crate Out','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "', " &
                                    " '" & Val(cratebal) & "','Opening Crate Balance',0,0,'N')"
                                a1 = clsFun.ExecNonQuery(ssql, True)
                                If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
                            ElseIf Val(tmpamtcr) < Val(tmpamtdr) Then
                                cratebal = Math.Abs(Val(tmpamt))
                                ssql = "Insert Into CrateVoucher (VoucherID,SlipNo,EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName,Qty,Remark,Rate,Amount,CashPaid) values" &
                                       "(0," & Val(i + 1) & ",'" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','Op Bal'," & Val(dt.Rows(i)("AccountID").ToString()) & ", " &
                                       " '" & dt.Rows(i)("AccountName").ToString() & "','Crate In','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "', " &
                                       " '" & Val(cratebal) & "','Opening Crate Balance',0,0,'N')"
                                a1 = clsFun.ExecNonQuery(ssql, True)
                                If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
                            End If
                        End If
                    End If
                Next

            End If
        Catch ex As Exception

        End Try
        lblCrateBalance.Visible = True
        lblCrateBalance.Text = "Crates Updated..."

        ' ''''''        Update(Stock)
        ''Dim dt As New DataTable
        sql = String.Empty
        'sql = "Select AccountID,AccountName, StockHolderID,StockHolderName,StorageID,StorageName,ItemID, ItemName,Sum(Nug) as PurchaseNug, " & _
        '      " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale') and ItemID=TempPurchase.ItemID) as soldNug,(Sum(Nug) - " & _
        '      " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale')   and ItemID=TempPurchase.ItemID)) as RestNug " & _
        '      " From TempPurchase Group by ItemID   Having RestNug > 0  order by StockHolderName,ItemID,ItemName"
        sql = "Select AccountID,AccountName,StockHolderID,StockHolderName From TempPurchase Group By AccountID,StockHolderID"
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Application.DoEvents()
                    ProgressBar1.Maximum = dt.Rows.Count
                    lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
                    ProgressBar1.Value = i
                    If lblStockBalance.Visible = True Then lblStockBalance.Visible = False Else lblStockBalance.Visible = True
                    Dim totQty As Integer = 0
                    Dim accID As Integer = 0
                    Dim accName As String = String.Empty
                    Dim stockHolderID As Integer = 0
                    Dim stockHolderName As String = String.Empty
                    Dim StorageID As String = String.Empty
                    Dim StorageName As String = String.Empty
                    Dim ItemID As Integer = 0
                    Dim ItemName As String = String.Empty
                    Dim Nugs As Integer = 0
                    Dim Weights As String = String.Empty
                    Dim VehicleNo As String = String.Empty
                    Dim Rate As String = String.Empty
                    Dim PurcahseTypeName As String = String.Empty
                    Dim VChID As Integer
                    '  Dim VchId As Integer = 1
                    sql = " Select VoucherID, AccountID,AccountName, StockHolderID,StockHolderName,VehicleNo,Weight,PurchaseTypeName,StorageID,StorageName,ItemID, ItemName,LotNo,Sum(Nug) as PurchaseNug,Rate,Per,MaintainCrate,CrateID,CrateName,CrateQty, " &
                           " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot = TempPurchase.LotNo and ItemID=TempPurchase.ItemID " &
                           " and PurchaseID=TempPurchase.VoucherID )as soldNug,(Sum(Nug) - " &
                           " (Select ifnull(sum(Nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out')  and Lot=TempPurchase.LotNo and ItemID=TempPurchase.ItemID" &
                           " and PurchaseID=TempPurchase.VoucherID)) as RestNug From TempPurchase Where AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' and StockHolderID='" & Val(dt.Rows(i)("StockHolderID").ToString()) & "'  Group by ItemID,LotNo,VoucherID Having RestNug <> 0  " &
                           " order by AccountName,ItemID,ItemName,LotNo"
                    dt1 = clsFun.ExecDataTable(sql)

                    If dt1.Rows.Count > 0 Then
                        sqll = "Insert Into Vouchers(Transtype, EntryDate,BillNo,SallerID, SallerName,VehicleNo,Nug,Kg,BasicAmount,DiscountAmount,TotalAmount,PurchaseType, " _
                             & "StorageID,StorageName,InvoiceID) Values ('Purchase','" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','" & VChID & "','" & Val(dt.Rows(i)("AccountID").ToString()) & "', " &
                               " '" & dt.Rows(i)("AccountName").ToString() & "'," & Val(totQty) & ",0,0,0,0,0,'" & PurcahseTypeName & "','" & Val(StorageID) & "','" & StorageName & "'," & Val(VChID) & ")"
                        If clsFun.ExecNonQuery(sqll) Then
                            VChID = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
                            sql = String.Empty
                            For j = 0 To dt1.Rows.Count - 1
                                totQty = totQty + Val(dt1.Rows(j)("RestNug").ToString())
                                Weights = Val(Weights) + Val(dt1.Rows(j)("Weight").ToString())
                                sql = sql & "insert into Purchase(EntryDate,TransType,VoucherID,BillNo,VehicleNo,PurchaseTypeName,AccountID,AccountName,StorageID,StorageName, " _
                                & "ItemID,ItemName,LotNo, Nug, Weight,Rate,Per, Amount, MaintainCrate, CrateID, CrateName, CrateQty,StockHolderID,StockHolderName) values " _
                                & "('" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','Purchase'," & Val(VChID) & ", " &
                                "'" & Val(i + 1) & "','" & Val(VChID) & "','Purchase'," &
                                "" & Val(dt1.Rows(j)("AccountID").ToString()) & ",'" & dt1.Rows(j)("AccountName").ToString() & "'," & Val(dt1.Rows(j)("StorageID").ToString()) & ", " &
                                "'" & dt1.Rows(j)("StorageName").ToString() & "'," &
                                "" & Val(dt1.Rows(j)("ItemID").ToString()) & ",'" & dt1.Rows(j)("ItemName").ToString() & "','" & dt1.Rows(j)("LotNo").ToString() & "', " &
                                "" & Val(dt1.Rows(j)("RestNug").ToString()) & "," & Val(0) & "," & Val(dt1.Rows(j)("Rate").ToString()) & ",'" & dt1.Rows(j)("Per").ToString() & "'," &
                                "" & Val(0) & ",'" & dt1.Rows(j)("MaintainCrate").ToString() & "'," & Val(dt1.Rows(j)("CrateID").ToString()) & ",'" & dt1.Rows(j)("CrateName").ToString() & "', " &
                                "" & Val(dt1.Rows(j)("CrateQty").ToString()) & "," & IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, (Val(28)), Val(dt1.Rows(j)("AccountID").ToString())) & ", " &
                                "'" & IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, "Mall Khata Purchase A/c", dt1.Rows(j)("AccountName").ToString()) & "');"
                                stockHolderID = IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, (Val(28)), Val(dt1.Rows(j)("AccountID").ToString()))
                                stockHolderName = IIf(Val(dt1.Rows(j)("StockHolderID").ToString()) = 28, "Mall Khata Purchase A/c", dt1.Rows(j)("AccountName").ToString())
                                PurcahseTypeName = dt1.Rows(j)("PurchaseTypeName").ToString()
                                StorageID = dt1.Rows(j)("StorageID").ToString()
                                StorageName = dt1.Rows(j)("StorageName").ToString()
                                VehicleNo = dt1.Rows(j)("VehicleNo").ToString()
                            Next
                            sqll = ""
                            sqll = "Update Vouchers Set TransType='Purchase', EntryDate='" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "',BillNo='" & Val(VChID) & "'," _
                                & "SallerID='" & Val(dt.Rows(i)("AccountID").ToString()) & "', SallerName='" & dt.Rows(i)("AccountName").ToString() & "',VehicleNo='" & VehicleNo & "'," _
                                & "Nug='" & Val(totQty) & "',Kg='" & Val(Weights) & "',BasicAmount='" & Val(0) & "'," _
                                & " DiscountAmount= '" & Val(0) & "',TotalAmount='" & Val(0) & "'," _
                                & " PurchaseType='" & PurcahseTypeName & "',StorageID='" & Val(StorageID) & "',StorageName='" & StorageName & "', " _
                                & "InvoiceID='" & Val(VChID) & "' Where ID=" & Val(VChID) & ""
                            If clsFun.ExecNonQuery(sqll) Then
                                clsFun.ExecNonQuery(sql)
                            End If

                        End If

                    End If
                Next
            End If

        Catch ex As Exception

        End Try

        '" " & _

        ssql = "Delete from Transaction2; Delete from sqlite_sequence where name='Transaction2'; drop table if  exists TempCrateVoucher;drop table if  exists TempPurchase;"
        a1 = clsFun.ExecNonQuery(ssql, True)
        lblStockBalance.Visible = True
        lblStockBalance.Text = "Stock Updated..."
        If a1 > 0 Then
            clsFun.ExecNonQuery("Vacuum;")
            MsgBox("Financial Year Changed Successfully...")
            CompanyList.BtnRetrive.PerformClick()
            Me.Close()
        End If
        '  End If
    End Sub

    Private Shared Sub updateConfigFile(con As String)
        Dim XmlDoc As New XmlDocument()
        XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        For Each xElement As XmlElement In XmlDoc.DocumentElement
            If xElement.Name = "connectionStrings" Then
                xElement.FirstChild.Attributes(1).Value = con
            End If
        Next
        XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
    End Sub
    Private Shared Sub changeCompany(ByVal Data As String)
        Try
            Dim Con As New StringBuilder("")
            Con.Append("Data Source=|DataDirectory|\")
            Con.Append(Data & ";Version=3;New=True;Compress=True;synchronous=ON;")
            'Con.Append(Data & ";Version=3;Password=smi3933;")
            Dim strCon As String = Con.ToString()
            updateConfigFile(strCon)
            Dim Db As New SQLite.SQLiteConnection()
            ConfigurationManager.RefreshSection("connectionStrings")
            Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
            clsFun.ConStr = Db.ConnectionString
        Catch E As Exception
            MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
        End Try
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnChangeYear.Click
        If mskNewFromDate.Text = mskNewtoDate.Text Then MsgBox("Finacial Year End Date Must Be Diffrence Finacial Year Start Date", MsgBoxStyle.Critical, "Check Dates...")
        Dim tmpid As Integer = txtAccountID.Text
        Dim data As String = txtCurrentPath.Text
        If MessageBox.Show("Are you Sure want to Change Fiancial Year?", "Change Finacial Year", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            lblStatus.Visible = True ' lblStatus.Text = "New Database Created Successfully..."
            btnChangeYear.Enabled = False
            btnChangeYear.Text = "Please Wait" : CreateDb(data, tmpid)
            ProgressBar1.Visible = False : lblStatus.Visible = False
            btnChangeYear.Enabled = True
            CompanyList.rowColums() : CompanyList.getCompanies()
        Else
            Exit Sub
        End If
        'BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub mskNewFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskNewFromDate.GotFocus
        mskNewFromDate.SelectionStart = 0 : mskNewFromDate.SelectionLength = Len(mskNewFromDate.Text)
    End Sub
    'Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
    '    Dim tmpid As Integer = txtAccountID.Text
    '    Dim data As String = txtCurrentPath.Text
    '    lblStatus.Visible = True : lblStatus.Text = "Please Wait... Don't Click Anywhere..."
    '    If MessageBox.Show("Are you Sure want to Change Fiancial Year?", "Change Finacial Year", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '        lblStatus.Visible = True ' lblStatus.Text = "New Database Created Successfully..."
    '        btnChangeYear.Text = "Please Wait" : CreateDb(data, tmpid)
    '        ProgressBar1.Visible = False : lblStatus.Visible = False
    '    Else
    '        Exit Sub
    '    End If
    'End Sub
    'Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
    '    ProgressBar1.Value = e.ProgressPercentage
    '    lblStatus.Text = e.ProgressPercentage
    'End Sub


    'Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

    '    Me.Close()
    'End Sub

    Private Sub mskNewFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskNewFromDate.MaskInputRejected

    End Sub

    Private Sub mskNewFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskNewFromDate.Validating
        mskNewFromDate.Text = clsFun.DateValidate(mskNewFromDate.Text)
    End Sub

    Private Sub mskNewtoDate_GotFocus(sender As Object, e As EventArgs) Handles mskNewtoDate.GotFocus
        mskNewtoDate.SelectionStart = 0 : mskNewtoDate.SelectionLength = Len(mskNewtoDate.Text)
    End Sub

    Private Sub mskNewtoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskNewtoDate.Validating
        mskNewtoDate.Text = clsFun.DateValidate(mskNewtoDate.Text)
    End Sub

    Private Sub mskNewFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskNewFromDate.KeyDown, mskNewtoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnChangeYear.Focus()
        End Select
    End Sub
End Class