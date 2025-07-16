Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Threading
Imports System.Data.SQLite

Public Class Sub_Ledger
    Dim rs As New Resizer
    Dim strSDate As String : Dim strEDate As String
    Dim dDate As DateTime : Dim mskstartDate As String
    Dim mskenddate As String
    Dim whatsappSender As New WhatsAppSender()
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    '  Private opbal As Decimal = 0.0
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown, cbAccountName.KeyDown
        If cbAccountName.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
            'SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select

    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Ledger_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then If pnlWahtsappNo.Visible = True Then pnlWahtsappNo.Visible = False : Exit Sub Else Me.Close()
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        mskFromDate.SelectAll()
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub
    Public Sub PerformButtonClick()
        btnShow.PerformClick()
    End Sub
    Private Sub Ledger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        clsFun.FillDropDownList(cbAccountName, "Select * From Ledger Where PostingID<>0 Group By PostingID ", "PostingAccount", "PostingID", "")
        Dim mindate = String.Empty : Dim maxdate As String = String.Empty
        mskFromDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyy"))
        MsktoDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyy"))
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 130
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Description" : dg1.Columns(4).Width = 545
        dg1.Columns(5).Name = "Debit" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Credit" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Balance" : dg1.Columns(7).Width = 150
        dg1.Columns(8).Name = "HindiName" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "HindiItem" : dg1.Columns(9).Width = 100
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub cbLedgerName_SelectedIndexChanged(sender As Object, e As EventArgs)


    End Sub
    Private Sub CrateMarka()
        lblCrate.Text = ""
        lblCrateDetails.Text = ""
        lblCrateDetails.Visible = False
        lblCrate.Visible = False
        Dim cratebal As String = String.Empty
        Dim CrateQty As String = String.Empty

        Dim sql As String = "Select  ((Select ifnull(Sum(Qty),0) From CrateVoucher Where val(cbAccountName.SelectedValue)=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and val(cbAccountName.SelectedValue)='" & Val(val(cbAccountName.SelectedValue)) & "')" &
      "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where val(cbAccountName.SelectedValue)=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and val(cbAccountName.SelectedValue)='" & Val(val(cbAccountName.SelectedValue)) & "')) as  Restbal from Accounts   where Restbal<>0  order by AccountName ;"
        Dim crateTotbal As String = clsFun.ExecScalarStr(sql)

        'lblCrate.Visible = True
        If Val(crateTotbal) <> 0 Then lblCrate.Visible = True
        If Val(crateTotbal) < 0 Then
            lblCrate.Text = "Total Crate : " & Math.Abs(Val(crateTotbal)) & " Out"
        Else
            lblCrate.Text = "Total Crate : " & Math.Abs(Val(crateTotbal)) & " In"
        End If
        dt = clsFun.ExecDataTable("Select CrateID,CrateName FROM CrateVoucher Where EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and val(cbAccountName.SelectedValue)='" & Val(val(cbAccountName.SelectedValue)) & "'  Group by CrateName,val(cbAccountName.SelectedValue)   order by val(cbAccountName.SelectedValue) ")
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    If Application.OpenForms().OfType(Of Ledger).Any = False Then Exit Sub
                    Application.DoEvents()
                    Dim tmpamtdr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "  and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "  and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    tmpamt1 = Val(tmpamtdr1) - Val(tmpamtcr1) '- Val(opbal)
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
                    If Val(tmpamtcr) > Val(tmpamtdr) Then
                        cratebal = dt.Rows(i)("Cratename").ToString() & " = " & Math.Abs(Val(tmpamt)) & " Out"
                        CrateQty = CrateQty & " , " & cratebal
                    ElseIf Val(tmpamtcr) < Val(tmpamtdr) Then
                        cratebal = dt.Rows(i)("Cratename").ToString() & " = " & Math.Abs(Val(tmpamt)) & " In"
                        CrateQty = CrateQty & " , " & cratebal
                    End If
                Next
                CrateQty = CrateQty.Trim().TrimStart(",")
                If CrateQty <> "" Then lblCrateDetails.Visible = True
                lblCrateDetails.Text = CrateQty
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        ButtonControl()
        Retrive() ': CrateMarka()
        pnlWahtsappNo.Visible = False
        ButtonControl()
    End Sub
    Private Sub RetriveWithoutBardana()
        Application.DoEvents()
        dg1.Rows.Clear()
        txtOpBal.Text = ""
        Dim ssql As String = String.Empty
        Dim dt As New DataTable : Dim dr As Decimal = 0
        Dim cr As Decimal = 0 : Dim tot As Decimal = 0
        Dim opbal As String = "" : Dim tmpamtdr As String = ""
        Dim tmpamtcr As String = ""
        opbal = clsFun.ExecScalarStr(" Select Round(OpBal,2) FROM Accounts WHERE ID= " & cbAccountName.SelectedValue & "")
        ssql = "Select VourchersID,Entrydate, TransType,AccountName,Remark,RemarkHindi,Narration,round(Amount,2) as Dr,'0' as Cr from Ledger where DC ='D' " & IIf(cbAccountName.SelectedValue > 0, "and val(cbAccountName.SelectedValue)=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and TransType not in ('Crate In','Crate Out')   union all" & _
            " Select VourchersID,Entrydate,  TransType,AccountName,Remark,RemarkHindi,Narration,'0' as Dr,round(Amount,2) as Cr  from Ledger where Dc='C' " & IIf(cbAccountName.SelectedValue > 0, "and val(cbAccountName.SelectedValue)=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  and TransType not in ('Crate In','Crate Out')    "
        tmpamtdr = clsFun.ExecScalarStr("Select round(sum(Amount),2) as tot from Ledger where Dc='D' and val(cbAccountName.SelectedValue)=" & Val(cbAccountName.SelectedValue) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        tmpamtcr = clsFun.ExecScalarStr("Select round(sum(Amount),2) as tot from Ledger where Dc='C' and val(cbAccountName.SelectedValue)=" & Val(cbAccountName.SelectedValue) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & cbAccountName.SelectedValue & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If

        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = -Val(opbal)
        End If

        'If Val(opbal) < Val(tmpamt) Then
        '    tmpamt = Math.Abs(Val(tmpamt)) + Math.Abs(Val(opbal))
        'Else
        '    tmpamt = Val(opbal) - Math.Abs(Val(tmpamt))
        'End If
        dt = clsFun.ExecDataTable(ssql)
        Dim dvData As DataView = New DataView(dt)
        'dvData.RowFilter = "EntryDate Between '" & mskFromDate.Text & "' And '" & MsktoDate.Text & "'"
        dvData.Sort = " [EntryDate],VourchersID asc"
        dt = dvData.ToTable
        dg1.Rows.Clear()

        opbal = tmpamt
        'If Val(tmpamt) > 0 Then
        '    opbal = Val(tmpamt)
        'Else
        '    opbal = Val(opbal) + Val(tmpamt)
        'End If

        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from LEdger where  val(cbAccountName.SelectedValue)=" & Val(cbAccountName.SelectedValue) & " and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        If cnt = 0 Then
            txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & cbAccountName.SelectedValue & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Cr"
            Else
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Dr"
            End If
            '  txtOpBal.Text = IIf(Val(opbal) > 0, Val(opbal) & " Dr", Math.Abs(Val(opbal)) & " Cr")
        End If
        ''opbal = Val(opbal) + Val(tmpamt)
        '  
        If Val(txtOpBal.Text) > 0 Then
            drcr = txtOpBal.Text.Substring(txtOpBal.Text.Length - 2)
        End If
        ' opbal = Math.Abs(val(opbal))
        Try
            If dt.Rows.Count > 0 Then
                Application.DoEvents()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(5).Value = IIf(Format(Val(dt.Rows(i)("Dr").ToString()), "0.00") = 0, "", Format(Val(dt.Rows(i)("Dr").ToString()), "0.00"))
                        .Cells(6).Value = IIf(Format(Val(dt.Rows(i)("Cr").ToString()), "0.00") = 0, "", Format(Val(dt.Rows(i)("Cr").ToString()), "0.00"))
                        .Cells(8).Value = clsFun.ExecScalarStr(" Select OtherName FROM Accounts WHERE ID= " & cbAccountName.SelectedValue & "")
                        ssql = "(Select * FROM Transaction2 AS T INNER JOIN Items AS I ON T.ItemID = I.ID)"
                        If dt.Rows(i)("TransType").ToString() = "Speed Sale" Then
                            'ssql = "Select OtherName ||' = नग  : '||(nug)||', वजन : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount as HindiRemark  From " & ssql & "  where val(cbAccountName.SelectedValue)=" & cbAccountName.SelectedValue & " and TransType='" & dt.Rows(i)("TransType").ToString() & "' and EntryDate= '" & Format(dt.Rows(i)("EntryDate"), "yyyy-MM-dd") & "'"
                            .Cells(9).Value = clsFun.ExecScalarStr("Select OtherName ||' = नग  : '||(nug)||', वजन : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount From " & ssql & " where VoucherID= " & dt.Rows(i)("VourchersID").ToString() & "")
                        Else
                            .Cells(9).Value = dt.Rows(i)("RemarkHindi").ToString()
                        End If
                        If i = 0 Then
                            If Val(.Cells(5).Value) > 0 Then
                                If drcr = "Dr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(5).Value)), "0.00")
                                Else
                                    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'If Val(.cells(5).value) > Val(opbal) Then
                                    '    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'Else
                                    '    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'End If
                                End If
                            Else
                                If drcr = "Cr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(6).Value)), "0.00")
                                Else
                                    If Val(.cells(6).value) > Val(opbal) Then
                                        tot = Format(Val(Val(.Cells(6).Value) - Val(opbal)), "0.00")
                                    Else
                                        tot = Format(Val(Val(opbal) - Val(.Cells(6).Value)), "0.00")
                                    End If
                                End If
                                If drcr = "Dr" And Val(opbal) > Val(.Cells(6).Value) Then
                                    tot = Math.Round(Val(tot), 2)
                                Else
                                    tot = -Val(tot)
                                End If
                            End If
                        Else
                            tot = tot + IIf(Val(.Cells(5).Value) > 0, Val(.Cells(5).Value), -Val(.Cells(6).Value))
                        End If
                        .Cells(7).Value = IIf(tot > 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(tot), "0.00") & " Cr")
                        dr = dr + Val(.Cells(5).Value)
                        cr = cr + Val(.Cells(6).Value)
                    End With
                Next
            Else
                tot = Format(Val(opbal), "0.00")

            End If
            If drcr = "Dr" Then
                dr = Format(Val(dr) + Math.Abs(Val(opbal)), "0.00")
            Else
                cr = Format(Val(cr) + Math.Abs(Val(opbal)), "0.00")
            End If
            txtDramt.Text = dr.ToString() : txtcrAmt.Text = cr.ToString()
            If dt.Rows.Count = 0 Then
                txtBalAmt.Text = txtOpBal.Text
            Else
                txtBalAmt.Text = IIf(Format(Val(tot), "0.00") >= 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(Val(tot)), "0.00") & " Cr")
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
    End Sub

    Private Sub Retrive()
        dg1.Rows.Clear() : txtOpBal.Text = ""
        Dim ssql As String = String.Empty
        Dim dt As New DataTable : Dim dr As Decimal = 0
        Dim cr As Decimal = 0 : Dim tot As Decimal = 0
        Dim opbal As String = "" : Dim tmpamtdr As String = ""
        Dim tmpamtcr As String = ""
        clsFun.ExecNonQuery("PRAGMA cache_size = 10000;")
        'opbal = clsFun.ExecScalarStr(" Select Round(OpBal,2) FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
        ssql = "Select VourchersID,Entrydate, TransType,PostingAccount,Remark,RemarkHindi,Narration,round(Amount,2) as Dr,'0' as Cr from Ledger where DC ='D' " & IIf(Val(cbAccountName.SelectedValue) > 0, "and PostingID=" & Val(Val(cbAccountName.SelectedValue)) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    union all" & _
            " Select VourchersID,Entrydate,  TransType,PostingAccount,Remark,RemarkHindi,Narration,'0' as Dr,round(Amount,2) as Cr  from Ledger where Dc='C' " & IIf(Val(cbAccountName.SelectedValue) > 0, "and PostingID=" & Val(Val(cbAccountName.SelectedValue)) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    "
        tmpamtdr = clsFun.ExecScalarStr("Select round(sum(Amount),2) as tot from Ledger where Dc='D' and PostingID=" & Val(Val(cbAccountName.SelectedValue)) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        tmpamtcr = clsFun.ExecScalarStr("Select round(sum(Amount),2) as tot from Ledger where Dc='C' and PostingID=" & Val(Val(cbAccountName.SelectedValue)) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If

        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = -Val(opbal)
        End If
        dt = clsFun.ExecDataTable(ssql)
        Dim dvData As DataView = New DataView(dt)
        dvData.Sort = " [EntryDate],VourchersID asc"
        dt = dvData.ToTable
        dg1.Rows.Clear()
        opbal = 0 'tmpamt
        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from LEdger where  PostingID=" & Val(Val(cbAccountName.SelectedValue)) & " and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        If cnt = 0 Then
            txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
        Else
            'If Val(tmpamtcr) > Val(tmpamtdr) Then
            '    txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Cr"
            'Else
            '    txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Dr"
            'End If
        End If
        If Val(txtOpBal.Text) > 0 Then
            drcr = txtOpBal.Text.Substring(txtOpBal.Text.Length - 2)
        End If
        Try
            If dt.Rows.Count > 0 Then
                Application.DoEvents()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("PostingAccount").ToString()
                        .Cells(4).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(5).Value = IIf(Format(Val(dt.Rows(i)("Dr").ToString()), "0.00") = 0, "", Format(Val(dt.Rows(i)("Dr").ToString()), "0.00"))
                        .Cells(6).Value = IIf(Format(Val(dt.Rows(i)("Cr").ToString()), "0.00") = 0, "", Format(Val(dt.Rows(i)("Cr").ToString()), "0.00"))
                        .Cells(8).Value = clsFun.ExecScalarStr(" Select OtherName FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
                        ssql = "(Select * FROM Transaction2 AS T INNER JOIN Items AS I ON T.ItemID = I.ID)"
                        .Cells(9).Value = dt.Rows(i)("RemarkHindi").ToString()
                        If i = 0 Then
                            If Val(.Cells(5).Value) > 0 Then
                                If drcr = "Dr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(5).Value)), "0.00")
                                Else
                                    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                End If
                            Else
                                If drcr = "Cr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(6).Value)), "0.00")
                                Else
                                    If Val(.cells(6).value) > Val(opbal) Then
                                        tot = Format(Val(Val(.Cells(6).Value) - Val(opbal)), "0.00")
                                    Else
                                        tot = Format(Val(Val(opbal) - Val(.Cells(6).Value)), "0.00")
                                    End If
                                End If
                                If drcr = "Dr" And Val(opbal) > Val(.Cells(6).Value) Then
                                    tot = Math.Round(Val(tot), 2)
                                Else
                                    tot = -Val(tot)
                                End If
                            End If
                        Else
                            tot = tot + IIf(Val(.Cells(5).Value) > 0, Val(.Cells(5).Value), -Val(.Cells(6).Value))
                        End If
                        .Cells(7).Value = IIf(tot > 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(tot), "0.00") & " Cr")
                        dr = dr + Val(.Cells(5).Value)
                        cr = cr + Val(.Cells(6).Value)
                    End With
                Next
            Else
                tot = Format(Val(opbal), "0.00")

            End If
            If drcr = "Dr" Then
                dr = Format(Val(dr) + Math.Abs(Val(opbal)), "0.00")
            Else
                cr = Format(Val(cr) + Math.Abs(Val(opbal)), "0.00")
            End If
            txtDramt.Text = dr.ToString() : txtcrAmt.Text = cr.ToString()
            If dt.Rows.Count = 0 Then
                txtBalAmt.Text = txtOpBal.Text
            Else
                txtBalAmt.Text = IIf(Format(Val(tot), "0.00") >= 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(Val(tot)), "0.00") & " Cr")
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
    End Sub
    Private Sub RetriveMerge()
        '    Dim val(cbAccountName.SelectedValue) As Integer = clsFun.ExecScalarInt("Select val(cbAccountName.SelectedValue) From Accounts Where ID='" & Val(cbAccountName.SelectedValue) & "'")
        dg1.Rows.Clear() : txtOpBal.Text = ""
        Dim ssql As String = String.Empty
        Dim dt As New DataTable : Dim dr As Decimal = 0
        Dim cr As Decimal = 0 : Dim tot As Decimal = 0
        Dim opbal As String = "" : Dim tmpamtdr As String = ""
        Dim tmpamtcr As String = ""
        clsFun.ExecNonQuery("PRAGMA cache_size = 100000;")
        'opbal =clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
        ssql = "Select val(cbAccountName.SelectedValue),VourchersID,Entrydate, TransType,AccountName,Remark,sum(Amount) as Dr,'0' as Cr from Ledger where DC ='D' " & IIf(val(cbAccountName.SelectedValue) > 0, "and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group By EntryDate,TransType   union all" & _
            " Select val(cbAccountName.SelectedValue),VourchersID,Entrydate,  TransType,AccountName,Remark,'0' as Dr,sum(Amount) as Cr  from Ledger where Dc='C' " & IIf(val(cbAccountName.SelectedValue) > 0, "and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group By EntryDate,TransType    "
        tmpamtdr = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        tmpamtcr = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If

        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = -Val(opbal)
        End If
        dt = clsFun.ExecDataTable(ssql)
        Dim dvData As DataView = New DataView(dt)
        dvData.Sort = " [EntryDate] asc , VourchersID asc "
        dt = dvData.ToTable
        dg1.Rows.Clear()
        opbal = tmpamt
        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from LEdger where  val(cbAccountName.SelectedValue)=" & Val(val(cbAccountName.SelectedValue)) & " and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        If cnt = 0 Then
            txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Cr"
            Else
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Dr"
            End If
        End If
        If Val(txtOpBal.Text) > 0 Then
            drcr = txtOpBal.Text.Substring(txtOpBal.Text.Length - 2)
        End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Application.DoEvents()
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("val(cbAccountName.SelectedValue)").ToString()
                        ssql = "Select Remark from Ledger where DC ='D' and val(cbAccountName.SelectedValue)=" & val(cbAccountName.SelectedValue) & " and EntryDate = '" & Format(dt.Rows(i)("EntryDate"), "yyyy-MM-dd") & "' and TransType='" & dt.Rows(i)("TransType").ToString() & "'   union all Select Remark  from Ledger where Dc='C' and val(cbAccountName.SelectedValue)=" & val(cbAccountName.SelectedValue) & " and EntryDate = '" & Format(dt.Rows(i)("EntryDate"), "yyyy-MM-dd") & "'and TransType='" & dt.Rows(i)("TransType").ToString() & "' "
                        Dim dt1 As DataTable
                        dt1 = clsFun.ExecDataTable(ssql)
                        If dt1.Rows.Count > 0 Then
                            For j = 0 To dt1.Rows.Count - 1
                                .Cells(4).Value = .Cells(4).Value & dt1.Rows(j)("Remark").ToString() & vbCrLf
                            Next
                        End If
                        .Cells(5).Value = IIf(Val(dt.Rows(i)("Dr").ToString()) = 0, "", Format(Val(dt.Rows(i)("Dr").ToString()), "0.00"))
                        .Cells(6).Value = IIf(Val(dt.Rows(i)("Cr").ToString()) = 0, "", Format(Val(dt.Rows(i)("Cr").ToString()), "0.00"))
                        .Cells(8).Value = clsFun.ExecScalarStr(" Select OtherName FROM Accounts WHERE ID= " & val(cbAccountName.SelectedValue) & "")
                        ssql = String.Empty
                        ssql = "(Select * FROM Transaction2 AS T INNER JOIN Items AS I ON T.ItemID = I.ID)"
                        ssql = "Select OtherName ||' = नग  : '||(nug)||', वजन : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount as HindiRemark  From " & ssql & "  where val(cbAccountName.SelectedValue)=" & val(cbAccountName.SelectedValue) & " and TransType='" & dt.Rows(i)("TransType").ToString() & "' and EntryDate= '" & Format(dt.Rows(i)("EntryDate"), "yyyy-MM-dd") & "'"
                        dt1 = clsFun.ExecDataTable(ssql)
                        If dt1.Rows.Count > 0 Then
                            For j = 0 To dt1.Rows.Count - 1
                                .Cells(9).Value = .Cells(9).Value & dt1.Rows(j)("HindiRemark").ToString() & vbCrLf
                            Next
                        End If
                        If i = 0 Then
                            If Val(.Cells(5).Value) > 0 Then
                                If drcr = "Dr" Then
                                    tot = Format(Val(opbal) + Val(.Cells(5).Value), "0.00")
                                Else
                                    If Val(.cells(5).value) > Val(opbal) Then
                                        tot = Format(Val(.Cells(5).Value) - Val(opbal), "0.00")
                                    Else
                                        tot = Format(Val(.Cells(5).Value) - Val(opbal), "0.00")
                                    End If
                                End If
                            Else
                                If drcr = "Cr" Then
                                    tot = Format(Val(opbal) + Val(.Cells(6).Value), "0.00")
                                Else
                                    If Val(.cells(6).value) > Val(opbal) Then
                                        tot = Format(Val(.Cells(6).Value) - Val(opbal), "0.00")
                                    Else
                                        tot = Format(Val(opbal) - Val(.Cells(6).Value), "0.00")
                                    End If
                                End If
                                If drcr = "Dr" And Val(opbal) > Val(.Cells(6).Value) Then
                                    tot = Math.Round(Val(tot), 2)
                                Else
                                    tot = -Val(tot)
                                End If
                            End If
                        Else
                            tot = tot + IIf(Val(.Cells(5).Value) > 0, Val(.Cells(5).Value), -Val(.Cells(6).Value))
                        End If
                        .Cells(7).Value = IIf(tot > 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(tot), "0.00") & " Cr")
                        dr = dr + Val(.Cells(5).Value)
                        cr = cr + Val(.Cells(6).Value)
                    End With
                Next
            Else
                tot = Format(Val(opbal), "0.00")

            End If
            If drcr = "Dr" Then
                dr = dr + Format(Math.Abs(Val(opbal)), "0.00")
            Else
                cr = cr + Format(Math.Abs(Val(opbal)), "0.00")

            End If
            txtDramt.Text = dr.ToString() : txtcrAmt.Text = cr.ToString()
            If dt.Rows.Count = 0 Then
                txtBalAmt.Text = txtOpBal.Text
            Else
                txtBalAmt.Text = IIf(Format(Val(tot), "0.00") > 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(Val(tot)), "0.00") & " Cr")
            End If

            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
    End Sub

    Private Sub cbAccountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbAccountName.KeyPress
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub
    Private Sub cbAccountName_Leave(sender As Object, e As EventArgs) Handles cbAccountName.Leave
        If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & cbAccountName.Text & "'") = 0 Then
            MsgBox("Account Name Not Found in Database...", vbOKOnly, "Access Denied")
            clsFun.FillDropDownList(cbAccountName, "Select * From Ledger Where PostingID<>0 Group By PostingID ", "PostingAccount", "PostingID", "")
            cbAccountName.Focus()
            Exit Sub
        End If
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) From Ledger Where PostingID=" & Val(cbAccountName.SelectedValue) & "")
        maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger Where PostingID=" & Val(cbAccountName.SelectedValue) & "")
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
    End Sub

    Private Sub cbAccountName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbAccountName.SelectedIndexChanged

    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub WithoutDiscriptionPrint()
        'Dim FastQuery As String = String.Empty
        'Dim sql As String = String.Empty
        'ClsFunPrimary.ExecNonQuery("Delete from printing")
        'If dg1.RowCount <> 0 Then
        'For Each row As DataGridViewRow In dg1.Rows
        '    With row
        '            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," & _
        '                "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," & _
        '                "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "',''," & _
        '                "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
        '        End With
        '    Next
        '    Try
        '        If FastQuery = String.Empty Then Exit Sub
        '        sql = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) " & FastQuery & ""
        '        ClsFunPrimary.ExecNonQuery(sql)
        '    Catch ex As Exception
        '        MsgBox(ex.Message)
        '        ClsFunPrimary.CloseConnection()
        '    End Try
        'Else
        '    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," & _
        '        "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "', " & _
        '        "'" & txtcrAmt.Text & "','" & txtBalAmt.Text & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
        '    Try
        '        If FastQuery = String.Empty Then Exit Sub
        '        sql = "insert into Printing(D1,D2,M1,M2, M3, M4, M5,P9,P10) " & FastQuery & ""
        '        ClsFunPrimary.ExecNonQuery(sql)
        '    Catch ex As Exception
        '        MsgBox(ex.Message)
        '        ClsFunPrimary.CloseConnection()
        '    End Try
        'End If

        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        If dg1.RowCount <> 0 Then
            For i As Integer = 0 To maxRowCount - 1
                Application.DoEvents()
                FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
                For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                    With dg1.Rows(LastRecord)
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," &
                            "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," &
                            "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "',''," &
                            "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
                    End With
                    LastRecord = Val(LastRecord + 1)
                Next
                ' LastRecord = LastCount
                Try
                    If FastQuery = String.Empty Then Exit Sub
                    sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) " & FastQuery & ""
                    ClsFunPrimary.ExecNonQuery(sQL)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            Next
        Else
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," &
                "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "', " &
                "'" & txtcrAmt.Text & "','" & txtBalAmt.Text & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5,P9,P10) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        End If

    End Sub


    Private Sub PrintRecord()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        If dg1.RowCount <> 0 Then
            For i As Integer = 0 To maxRowCount - 1
                'Application.DoEvents()
                FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
                For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                    With dg1.Rows(LastRecord)
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," &
                            "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," &
                            "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "','" & IIf(ckPrintHindi.Checked = True, .Cells("HindiItem").Value, .Cells("Description").Value) & "'," &
                            "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
                    End With
                    LastRecord = Val(LastRecord + 1)
                Next
                ' LastRecord = LastCount
                Try
                    If FastQuery = String.Empty Then Exit Sub
                    sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) " & FastQuery & ""
                    ClsFunPrimary.ExecNonQuery(sQL)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            Next
        Else
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," &
                "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "', " &
                "'" & txtcrAmt.Text & "','" & txtBalAmt.Text & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5,P9,P10) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        End If
    End Sub



    'Private Sub PrintRecord()
    '    Dim FastQuery As String = String.Empty
    '    Dim sql As String = String.Empty
    '    ClsFunPrimary.ExecNonQuery("Delete from printing")
    '    If dg1.RowCount <> 0 Then
    '        For Each row As DataGridViewRow In dg1.Rows
    '            With row
    '                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," & _
    '                    "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," & _
    '                    "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "','" & IIf(ckPrintHindi.Checked = True, .Cells("HindiItem").Value, .Cells("Description").Value) & "'," & _
    '                    "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
    '            End With
    '        Next
    '        Try
    '            If FastQuery = String.Empty Then Exit Sub
    '            sql = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) " & FastQuery & ""
    '            ClsFunPrimary.ExecNonQuery(sql)
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '            ClsFunPrimary.CloseConnection()
    '        End Try
    '    Else
    '        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," & _
    '            "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "', " & _
    '            "'" & txtcrAmt.Text & "','" & txtBalAmt.Text & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
    '        Try
    '            If FastQuery = String.Empty Then Exit Sub
    '            sql = "insert into Printing(D1,D2,M1,M2, M3, M4, M5,P9,P10) " & FastQuery & ""
    '            ClsFunPrimary.ExecNonQuery(sql)
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '            ClsFunPrimary.CloseConnection()
    '        End Try
    '    End If
    'End Sub
    'Public Sub RptPath()
    '    Dim FilePath As String = "\Ledger.rpt"
    'End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        ' clsFun.changeCompany()
        If ckJoin.Checked = True Then
            WithoutDiscriptionPrint()
        Else
            PrintRecord()
        End If
        If ckPrintHindi.Checked = True Then
            Report_Viewer.printReport("\Ledger2.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        Else
            '    PrintRecord()
            Report_Viewer.printReport("\Ledger.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If

    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim id As Integer = dg1.SelectedRows(0).Cells(0).Value
        Dim type As String = dg1.SelectedRows(0).Cells(2).Value
        If type = "Stock Sale" Then
            Stock_Sale.MdiParent = MainScreenForm
            Stock_Sale.Show()
            Stock_Sale.FillControls(id)
            If Not Stock_Sale Is Nothing Then
                Stock_Sale.BringToFront()
            End If
        ElseIf type = "Receipt" Then
            ReceiptForm.MdiParent = MainScreenForm
            ReceiptForm.Show()
            ReceiptForm.FillControls(id)
            If Not ReceiptForm Is Nothing Then
                ReceiptForm.BringToFront()
            End If
        ElseIf type = "Payment" Then
            PayMentform.MdiParent = MainScreenForm
            PayMentform.Show()
            PayMentform.FillControls(id)
            If Not PayMentform Is Nothing Then
                PayMentform.BringToFront()
            End If
        ElseIf type = "Purchase" Then
            Purchase.MdiParent = MainScreenForm
            Purchase.Show()
            Purchase.FillControls(id)
            Purchase.BringToFront()
        ElseIf type = "Purchase (Loose)" Then
            Loose_Purchase.MdiParent = MainScreenForm
            Loose_Purchase.Show()
            Loose_Purchase.FillControls(id)
            Loose_Purchase.BringToFront()
        ElseIf type = "Sale (Loose)" Then
            Loose_Sale.MdiParent = MainScreenForm
            Loose_Sale.Show()
            Loose_Sale.FillControls(id)
            Loose_Sale.BringToFront()
        ElseIf type = "Super Sale" Then
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(id)
            If Not Super_Sale Is Nothing Then
                Super_Sale.BringToFront()
            End If
        ElseIf type = "Speed Sale" Then
            SpeedSale.MdiParent = MainScreenForm
            SpeedSale.Show()
            SpeedSale.FillContros(id)
            If Not SpeedSale Is Nothing Then
                SpeedSale.BringToFront()
            End If
        ElseIf type = "Standard Sale" Then
            Standard_Sale.MdiParent = MainScreenForm
            Standard_Sale.Show()
            Standard_Sale.FillControls(id)
            If Not Standard_Sale Is Nothing Then
                Standard_Sale.BringToFront()
            End If
        ElseIf type = "Super Sale" Then
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(id)
            If Not Super_Sale Is Nothing Then
                Super_Sale.BringToFront()
            End If
        ElseIf type = "Auto Beejak" Then
            Sellout_Auto.MdiParent = MainScreenForm
            Sellout_Auto.Show()
            Sellout_Auto.FillFromData(id)
            If Not Sellout_Auto Is Nothing Then
                Sellout_Auto.BringToFront()
            End If
        ElseIf type = "Beejak" Then
            Sellout_Mannual.MdiParent = MainScreenForm
            Sellout_Mannual.Show()
            Sellout_Mannual.FillContros(id)
            Sellout_Mannual.BringToFront()
        ElseIf type = "Crate In" Then
            Crate_IN.MdiParent = MainScreenForm
            Crate_IN.Show()
            Crate_IN.FillControls(id)
            If Not Crate_IN Is Nothing Then
                Crate_IN.BringToFront()
            End If
        ElseIf type = "Crate Out" Then
            Crate_Out.MdiParent = MainScreenForm
            Crate_Out.Show()
            Crate_Out.FillControls(id)
            If Not Crate_Out Is Nothing Then
                Crate_Out.BringToFront()
            End If
        ElseIf type = "Journal" Then
            JournalEntry.MdiParent = MainScreenForm
            JournalEntry.Show()
            JournalEntry.FillContros(id)
            If Not JournalEntry Is Nothing Then
                JournalEntry.BringToFront()
            End If
        ElseIf type = "On Sale" Then
            On_Sale.MdiParent = MainScreenForm
            On_Sale.Show()
            On_Sale.FillControl(id)
            If Not On_Sale Is Nothing Then
                On_Sale.BringToFront()
            End If
        ElseIf type = "On Sale Receipt" Then
            On_Sale_Receipt.MdiParent = MainScreenForm
            On_Sale_Receipt.Show()
            On_Sale_Receipt.FillControl(id)
            If Not On_Sale_Receipt Is Nothing Then
                On_Sale_Receipt.BringToFront()
            End If
        ElseIf type = "Net Receipt" Then
            OnSaleReceipt_Net.MdiParent = MainScreenForm
            OnSaleReceipt_Net.Show()
            OnSaleReceipt_Net.FillControls(id)
            OnSaleReceipt_Net.BringToFront()
        ElseIf type = "Group Receipt" Then
            Group_Receipt.MdiParent = MainScreenForm
            Group_Receipt.Show()
            Group_Receipt.FillControls(id)
            If Not Group_Receipt Is Nothing Then
                Group_Receipt.BringToFront()
            End If
        ElseIf type = "Group Payment" Then
            Group_Payment.MdiParent = MainScreenForm
            Group_Payment.Show()
            Group_Payment.FillControls(id)
            If Not Group_Payment Is Nothing Then
                Group_Payment.BringToFront()
            End If
        Else
            Bank_Entry.MdiParent = MainScreenForm
            Bank_Entry.Show()
            Bank_Entry.FillControls(id)
            If Not Bank_Entry Is Nothing Then
                Bank_Entry.BringToFront()
            End If
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim id As Integer = dg1.SelectedRows(0).Cells(0).Value
            Dim type As String = dg1.SelectedRows(0).Cells(2).Value
            If type = "Stock Sale" Then
                Stock_Sale.MdiParent = MainScreenForm
                Stock_Sale.Show()
                Stock_Sale.FillControls(id)
                If Not Stock_Sale Is Nothing Then
                    Stock_Sale.BringToFront()
                End If
            ElseIf type = "Receipt" Then
                ReceiptForm.MdiParent = MainScreenForm
                ReceiptForm.Show()
                ReceiptForm.FillControls(id)
                If Not ReceiptForm Is Nothing Then
                    ReceiptForm.BringToFront()
                End If
            ElseIf type = "Payment" Then
                PayMentform.MdiParent = MainScreenForm
                PayMentform.Show()
                PayMentform.FillControls(id)
                If Not PayMentform Is Nothing Then
                    PayMentform.BringToFront()
                End If
            ElseIf type = "Purchase" Then
                Purchase.MdiParent = MainScreenForm
                Purchase.Show()
                Purchase.FillControls(id)
                Purchase.BringToFront()
            ElseIf type = "Purchase (Loose)" Then
                Loose_Purchase.MdiParent = MainScreenForm
                Loose_Purchase.Show()
                Loose_Purchase.FillControls(id)
                Loose_Purchase.BringToFront()
            ElseIf type = "Sale (Loose)" Then
                Loose_Sale.MdiParent = MainScreenForm
                Loose_Sale.Show()
                Loose_Sale.FillControls(id)
                Loose_Sale.BringToFront()
            ElseIf type = "Super Sale" Then
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.Show()
                Super_Sale.FillControls(id)
                If Not Super_Sale Is Nothing Then
                    Super_Sale.BringToFront()
                End If
            ElseIf type = "Speed Sale" Then
                SpeedSale.MdiParent = MainScreenForm
                SpeedSale.Show()
                SpeedSale.FillContros(id)
                If Not SpeedSale Is Nothing Then
                    SpeedSale.BringToFront()
                End If
            ElseIf type = "Standard Sale" Then
                Standard_Sale.MdiParent = MainScreenForm
                Standard_Sale.Show()
                Standard_Sale.FillControls(id)
                If Not Standard_Sale Is Nothing Then
                    Standard_Sale.BringToFront()
                End If
            ElseIf type = "Super Sale" Then
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.Show()
                Super_Sale.FillControls(id)
                If Not Super_Sale Is Nothing Then
                    Super_Sale.BringToFront()
                End If
            ElseIf type = "Auto Beejak" Then
                Sellout_Auto.MdiParent = MainScreenForm
                Sellout_Auto.Show()
                Sellout_Auto.FillFromData(id)
                If Not Sellout_Auto Is Nothing Then
                    Sellout_Auto.BringToFront()
                End If
            ElseIf type = "Beejak" Then
                Sellout_Mannual.MdiParent = MainScreenForm
                Sellout_Mannual.Show()
                Sellout_Mannual.FillContros(id)
                Sellout_Mannual.BringToFront()
            ElseIf type = "Crate In" Then
                Crate_IN.MdiParent = MainScreenForm
                Crate_IN.Show()
                Crate_IN.FillControls(id)
                If Not Crate_IN Is Nothing Then
                    Crate_IN.BringToFront()
                End If
            ElseIf type = "Crate Out" Then
                Crate_Out.MdiParent = MainScreenForm
                Crate_Out.Show()
                Crate_Out.FillControls(id)
                If Not Crate_Out Is Nothing Then
                    Crate_Out.BringToFront()
                End If
            ElseIf type = "Journal" Then
                JournalEntry.MdiParent = MainScreenForm
                JournalEntry.Show()
                JournalEntry.FillContros(id)
                If Not JournalEntry Is Nothing Then
                    JournalEntry.BringToFront()
                End If
            ElseIf type = "On Sale" Then
                On_Sale.MdiParent = MainScreenForm
                On_Sale.Show()
                On_Sale.FillControl(id)
                If Not On_Sale Is Nothing Then
                    On_Sale.BringToFront()
                End If
            ElseIf type = "On Sale Receipt" Then
                On_Sale_Receipt.MdiParent = MainScreenForm
                On_Sale_Receipt.Show()
                On_Sale_Receipt.FillControl(id)
                If Not On_Sale_Receipt Is Nothing Then
                    On_Sale_Receipt.BringToFront()
                End If
            ElseIf type = "Net Receipt" Then
                OnSaleReceipt_Net.MdiParent = MainScreenForm
                OnSaleReceipt_Net.Show()
                OnSaleReceipt_Net.FillControls(id)
                OnSaleReceipt_Net.BringToFront()
            ElseIf type = "Group Receipt" Then
                Group_Receipt.MdiParent = MainScreenForm
                Group_Receipt.Show()
                Group_Receipt.FillControls(id)
                If Not Group_Receipt Is Nothing Then
                    Group_Receipt.BringToFront()
                End If
            ElseIf type = "Group Payment" Then
                Group_Payment.MdiParent = MainScreenForm
                Group_Payment.Show()
                Group_Payment.FillControls(id)
                If Not Group_Payment Is Nothing Then
                    Group_Payment.BringToFront()
                End If
            Else
                Bank_Entry.MdiParent = MainScreenForm
                Bank_Entry.Show()
                Bank_Entry.FillControls(id)
                If Not Bank_Entry Is Nothing Then
                    Bank_Entry.BringToFront()
                End If
            End If
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'clsFun.changeCompany()
        If ckJoin.Checked = True Then
            WithoutDiscriptionPrint()
        Else
            PrintRecord()
        End If
        Report_Viewer.printReport("\Ledger-DayWise.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub MsktoDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MsktoDate.MaskInputRejected

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Panel11_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub txtOpBal_TextChanged(sender As Object, e As EventArgs) Handles txtOpBal.TextChanged

    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles Dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles Dtp2.ValueChanged
        MsktoDate.Text = Dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        txtWhatsappNo.Text = clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(cbAccountName.SelectedValue) & "'")
        pnlWahtsappNo.Visible = True : txtWhatsappNo.Focus()
        If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "WhatsApp API" Then
            cbType.SelectedIndex = 0
            Exit Sub
        ElseIf ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
            cbType.SelectedIndex = 1
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
        Else
            cbType.SelectedIndex = 2
        End If

    End Sub
    Private Sub WhatsAppDesktop()
        Dim GAP1 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP1 From API")) & "000"
        Dim GAP2 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP2 From API")) & "000"
        Dim whatsappURL As String = String.Empty
        Dim sourceFilePath As String = String.Empty
        Try
            whatsappURL = "whatsapp://send?"
            Dim psi As New ProcessStartInfo(whatsappURL)
            Dim process As Process = process.Start(psi)
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try
        If Directory.Exists(Application.StartupPath & "\Whatsapp\Pdfs") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Whatsapp\Pdfs")
        End If
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        If ckJoin.Checked = True Then
            WithoutDiscriptionPrint()
        Else
            PrintRecord()
        End If
        GlobalData.PdfName = cbAccountName.Text & " (" & mskFromDate.Text & " to " & MsktoDate.Text & ").pdf"
        If ckPrintHindi.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger2.rpt")
        ElseIf ckDaywisePrint.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger-DayWise.rpt")
        Else
            Pdf_Genrate.ExportReport("\Ledger.rpt")
        End If
        sourceFilePath = GlobalData.PdfPath
        whatsappURL = "whatsapp://send?phone=91" & txtWhatsappNo.Text.Trim & ""
        Dim psi1 As New ProcessStartInfo(whatsappURL)
        psi1.UseShellExecute = True
        psi1.WindowStyle = ProcessWindowStyle.Normal
        Dim process1 As Process = Process.Start(psi1)
        psi1.WindowStyle = ProcessWindowStyle.Minimized
        Thread.Sleep(GAP1)
        SendKeys.SendWait("^(+f)")
        SendKeys.SendWait("{ESCAPE}")
        Clipboard.SetData(DataFormats.FileDrop, {sourceFilePath})
        ' If i = 0 Then Thread.Sleep(1000)
        SendKeys.SendWait("^(v)")
        Thread.Sleep(GAP2)
        SendKeys.SendWait("{ENTER}")
        Thread.Sleep(GAP2)
        Dim processName As String = "WhatsApp"
        Dim proc As New ProcessStartInfo(processName)
        Dim processes() As Process = Process.GetProcessesByName(processName)
        If processes.Length > 0 Then
            ' Close each instance of the process
            For Each p As Process In processes
                Thread.Sleep(GAP2)
                p.Kill() : pnlWahtsappNo.Visible = False : cbAccountName.Focus() : cbAccountName.SelectAll()
                'proc.WindowStyle = ProcessWindowStyle.Minimized
            Next
            MsgBox("Ledger Send to " & cbAccountName.Text & " via WhatsApp Successful", MsgBoxStyle.Information, "Ledger Sent")

        Else
            ' The process was not found
            Console.WriteLine("Process not found.")
        End If
    End Sub

    Private Sub SendWhatsappData()
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        GlobalData.PdfName = cbAccountName.Text & "-" & mskFromDate.Text & ".pdf"
        If ckJoin.Checked = True Then
            WithoutDiscriptionPrint()
        Else
            PrintRecord()
        End If
        If ckPrintHindi.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger2.rpt")
        ElseIf ckDaywisePrint.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger-DayWise.rpt")
        Else
            Pdf_Genrate.ExportReport("\Ledger.rpt")
        End If
        sql = "insert into SendingData(val(cbAccountName.SelectedValue),AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(cbAccountName.SelectedValue) & "','" & cbAccountName.Text.Replace("/", "") & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        pnlWahtsappNo.Visible = False : cbAccountName.Focus()
    End Sub

    Private Sub UsingWhatsappAPI()
        If txtWhatsappNo.Text <= "" Then lblStatus.Visible = False : Exit Sub
        lblStatus.Visible = False
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")

        'pnlWahtsappNo.Visible = True
        'txtWhatsappNo.Focus()
        GlobalData.PdfName = cbAccountName.Text & "-" & mskFromDate.Text & ".pdf"
        If ckJoin.Checked = True Then
            WithoutDiscriptionPrint()
        Else
            PrintRecord()
        End If
        If ckPrintHindi.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger2.rpt")
        ElseIf ckDaywisePrint.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger-DayWise.rpt")
        Else
            Pdf_Genrate.ExportReport("\Ledger.rpt")
        End If
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Whatsapp\Pdfs\" & GlobalData.PdfName)
        whatsappSender.SendWhatsAppFile("91" & txtWhatsappNo.Text, "Sended By: Aadhat Software" & vbCrLf & "www.softmanagementindia.in", FilePath)
        lblStatus.Text = "PDF Sent " & whatsappSender.APIResposne
        lblStatus.Visible = True
        sql = "insert into waReport(EntryDate,AccountName,WhatsAppNo,Type,Status) SELECT '" & Date.Today.ToString("yyyy-MM-dd") & "','" & cbAccountName.Text.Replace("/", "") & "','" & txtWhatsappNo.Text & "','Ledger','" & lblStatus.Text & "'"
        clsFun.ExecNonQuery(sql)
        pnlWahtsappNo.Visible = False : cbAccountName.Focus()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(cbAccountName.SelectedValue) & "'") = "" And txtWhatsappNo.Text <> "" Then
            clsFun.ExecScalarStr("Update Accounts set Mobile1='" & txtWhatsappNo.Text & "' Where ID='" & Val(cbAccountName.SelectedValue) & "'")
        Else
            If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(cbAccountName.SelectedValue) & "'") <> txtWhatsappNo.Text Then
                If MessageBox.Show("Are you Sure to Change Mobile No In PhoneBook", "Change Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    clsFun.ExecScalarStr("Update Accounts set Mobile1='" & txtWhatsappNo.Text & "' Where ID='" & Val(cbAccountName.SelectedValue) & "'")
                End If
            End If
        End If
        If cbType.SelectedIndex = 0 Then
            instance_id = ClsFunPrimary.ExecScalarStr("Select InstanceID From API")
            If instance_id = "" Then MainScreenForm.MdiParent = Me : WhatsApp_API.Show()
            UsingWhatsappAPI()
        ElseIf cbType.SelectedIndex = 1 Then
            If txtWhatsappNo.Text <> "" Then
                SendWhatsappData()
            Else
                MsgBox("Please Enter Valid Whatsapp Contact", MsgBoxStyle.Critical, "Invalid Contact") : txtWhatsappNo.Focus()
            End If
        ElseIf cbType.SelectedIndex = 2 Then
            WhatsAppDesktop()
        End If

    End Sub

    Private Sub ckWithoutCrate_CheckedChanged(sender As Object, e As EventArgs) Handles ckWithoutCrate.CheckedChanged

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub

    Private Sub Label41_Click(sender As Object, e As EventArgs) Handles Label41.Click

    End Sub
End Class