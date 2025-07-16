Imports System.Data.SQLite

Public Class Day_book
    Dim rs As New Resizer
    Dim strSDate As String : Dim strEDate As String
    Dim dDate As DateTime : Dim mskstartDate As String
    Dim mskenddate As String
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub Day_book_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub Day_book_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        clsFun.FillDropDownList(Cbper, "Select VourchersID,TransType FROM Ledger  Group by TransType", "TransType", "VourchersID", "--All--")
        rowColums()
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 150
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 300
        dg1.Columns(4).Name = "Description" : dg1.Columns(4).Width = 310
        dg1.Columns(5).Name = "Debit" : dg1.Columns(5).Width = 130
        dg1.Columns(6).Name = "Credit" : dg1.Columns(6).Width = 130
    End Sub
    Sub calc()
        txtDramt.Text = Format(0, "0.00") : txtcrAmt.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtDramt.Text = Format(Val(txtDramt.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtcrAmt.Text = Format(Val(txtcrAmt.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
        Next

    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If ckMerge.Checked = True Then
            retriveMerge()
        Else
            retrive()
        End If

    End Sub
  
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        Dim ssql As String
        If Cbper.SelectedIndex = 0 Then
            ssql = "Select VourchersID,  EntryDate,TransType,AccountName,Remark,Amount as Dr,'' as Cr from Ledger where DC ='D' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  union all" & _
                   " Select VourchersID, EntryDate,TransType,AccountName,Remark,'' as Dr,Amount as Cr  from Ledger where Dc='C'  and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  "
        Else
            ssql = "Select VourchersID,  EntryDate,TransType,AccountName,Remark,Amount as Dr,'' as Cr from Ledger where DC ='D' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  AND TransType='" & Cbper.Text & "' union all" & _
                   " Select VourchersID, EntryDate,TransType,AccountName,Remark,'' as Dr,Amount as Cr  from Ledger where Dc='C'  and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'   AND TransType='" & Cbper.Text & "'  "
        End If
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 20 Then dg1.Columns(3).Width = 280 Else dg1.Columns(3).Width = 300
        Dim dvData As DataView = New DataView(dt)
        ' dvData.RowFilter = "EntryDate Between '" & mskFromDate.Text & "' And '" & MsktoDate.Text & "'"
        dvData.Sort = "VourchersID asc"
        dt = dvData.ToTable
        dg1.Rows.Clear()
        pbWait.Visible = True
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    ' Application.DoEvents()
                    If Application.OpenForms().OfType(Of Day_book).Any = False Then Exit Sub
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        '   .Cells(0).Value = dt.Rows(i)("id").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(5).Value = IIf(Val(dt.Rows(i)("Dr").ToString()) = "0", "", Format(Val(dt.Rows(i)("Dr").ToString()), "0.00"))
                        .Cells(6).Value = IIf(Val(dt.Rows(i)("Cr").ToString()) = "0", "", Format(Val(dt.Rows(i)("Cr").ToString()), "0.00"))
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next

            Else
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection() : pbWait.Visible = False
    End Sub


    Private Sub retriveMerge(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        Dim ssql As String
        If Cbper.SelectedIndex = 0 Then
            ssql = "Select VourchersID,  EntryDate,TransType,AccountName,Remark,Round(sum(Amount),2) as Dr,0 as Cr from Ledger where DC ='D' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group by AccountID,TransType,EntryDate union all" & _
                   " Select VourchersID, EntryDate,TransType,AccountName,Remark,0 as Dr,Round(sum(Amount),5)as Cr  from Ledger where Dc='C'  and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group by AccountID,TransType,EntryDate "
        Else
            ssql = "Select VourchersID,  EntryDate,TransType,AccountName,Remark,Round(sum(Amount),2)  as Dr,0 as Cr from Ledger where DC ='D' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  AND TransType='" & Cbper.Text & "' Group by AccountID,TransType,EntryDate union all" & _
                   " Select VourchersID, EntryDate,TransType,AccountName,Remark,0 as Dr,Round(sum(Amount),2)  as Cr  from Ledger where Dc='C'  and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'   AND TransType='" & Cbper.Text & "'  Group by AccountID,TransType,EntryDate "
        End If
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 20 Then dg1.Columns(3).Width = 280 Else dg1.Columns(3).Width = 300
        Dim dvData As DataView = New DataView(dt)
        ' dvData.RowFilter = "EntryDate Between '" & mskFromDate.Text & "' And '" & MsktoDate.Text & "'"
        dvData.Sort = "VourchersID asc"
        dt = dvData.ToTable
        dg1.Rows.Clear()
        pbWait.Visible = True
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    ' Application.DoEvents()
                    If Application.OpenForms().OfType(Of Day_book).Any = False Then Exit Sub
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        '   .Cells(0).Value = dt.Rows(i)("id").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(5).Value = IIf(Val(dt.Rows(i)("Dr").ToString()) = "0", "", Format(Val(dt.Rows(i)("Dr").ToString()), "0.00"))
                        .Cells(6).Value = IIf(Val(dt.Rows(i)("Cr").ToString()) = "0", "", Format(Val(dt.Rows(i)("Cr").ToString()), "0.00"))
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next

            Else
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection() : pbWait.Visible = False
    End Sub
    Private Sub Day_book_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown, Cbper.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            If Application.OpenForms().OfType(Of Day_book).Any = False Then Exit Sub
            With row
                sql = "insert into Printing(D1,D2,M1,M2, P1, P2,P3, P4, P5, P6) values('" & mskFromDate.Text & "'," & _
                    "'" & MsktoDate.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," & _
                    "'" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "','" & .Cells("Description").Value & "'," & _
                    "'" & Format(Val(.Cells("Debit").Value), "0.00") & "'," & Format(Val(.Cells("Credit").Value), "0.00") & ")"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        PrintRecord()
        Report_Viewer.printReport("\DayBook.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
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
                If Not Purchase Is Nothing Then
                    Purchase.BringToFront()
                End If
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
                If Not Sellout_Mannual Is Nothing Then
                    Sellout_Mannual.BringToFront()
                End If
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
            If Not Purchase Is Nothing Then
                Purchase.BringToFront()
            End If
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
            If Not Sellout_Mannual Is Nothing Then
                Sellout_Mannual.BringToFront()
            End If
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



    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
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
End Class