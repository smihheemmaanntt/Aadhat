Imports System.Runtime.InteropServices

Public Class CrateReceivableTotal

    Private Sub Speed_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectAll()
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub Crate_Summary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String
        mindate = clsFun.ExecScalarStr("Select max(EntryDate) as entrydate from CrateVoucher ")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        rowColums() : RadioAll.Checked = True
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 8
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name" : dg1.Columns(1).Width = 200
        dg1.Columns(2).Name = "Other Name" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Area" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Mobile" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = " Crate Name" : dg1.Columns(5).Width = 200
        dg1.Columns(6).Name = "Receivable" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Estimate Cost" : dg1.Columns(7).Width = 150
        dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        '  dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable

    End Sub
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr
    End Function
    Private Sub retrive1(Optional ByVal condtion As String = "")
        SendMessage(pb1.Handle, 1040, 3, 0)
        dg1.Rows.Clear()
        Dim dt As DataTable
        Dim lastval As Integer = 0
        Dim lastval1 As Integer = 0
        Dim totalOpOutCrate As Integer = 0
        Dim opbal As Decimal
        'dt = clsFun.ExecDataTable("Select AccountID,AccountName,(Select OtherName From Accounts Where ID=CrateVoucher.AccountID) as OtherName,(Select Area From Accounts Where ID=CrateVoucher.AccountID) as area,(Select Mobile1 From Accounts Where ID=CrateVoucher.AccountID) as Mobile,CrateID,CrateName FROM CrateVoucher Where EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & " and AccountID in(859,860,863) Group by CrateName,AccountID   order by AccountID ")
        dt = clsFun.ExecDataTable("Select AccountID,AccountName,(Select OtherName From Accounts Where ID=CrateVoucher.AccountID) as OtherName,(Select Area From Accounts Where ID=CrateVoucher.AccountID) as area,(Select Mobile1 From Accounts Where ID=CrateVoucher.AccountID) as Mobile,CrateID,CrateName FROM CrateVoucher Where EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & "  Group by CrateName,AccountID   order by upper(AccountName) ")
        Dim vchid As Integer = 0
        Dim tmpamt1 As Integer = 0
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    If Application.OpenForms().OfType(Of CrateReceivableTotal).Any = False Then Exit Sub
                    Application.DoEvents()
                    pb1.Minimum = 0
                    pb1.Maximum = dt.Rows.Count - 1
                    pb1.Value = i
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
                    If Val(tmpamtcr) > Val(tmpamtdr) Then
                        totalOpOutCrate = Val(totalOpOutCrate) + tmpamt
                        opbal = Math.Abs(Val(tmpamt))
                        If Application.OpenForms().OfType(Of CrateReceivableTotal).Any = False Then Exit Sub
                        dg1.Rows.Add()
                        With dg1.Rows(lastval)
                            .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                            .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                            .Cells(2).Value = dt.Rows(i)("OtherName").ToString()
                            .Cells(3).Value = dt.Rows(i)("Area").ToString()
                            .Cells(4).Value = dt.Rows(i)("MObile").ToString()
                            .Cells(5).Value = dt.Rows(i)("CrateName").ToString()
                            .Cells(6).Value = Val(opbal)
                            Dim cost As Double = Format(Val(clsFun.ExecScalarStr("Select Rate From CrateMarka Where ID='" & Val(dt.Rows(i)("CrateID").ToString()) & "'")), "0.00")
                            .Cells(7).Value = Format(Val(opbal) * cost, "0.00")
                            lastval = lastval + 1
                        End With
                    End If

                Next
            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection() : calc()
    End Sub

    Private Sub Retrive(Optional ByVal condtion As String = "")
        SendMessage(pb1.Handle, 1040, 3, 0)
        dg1.Rows.Clear() : Dim dt As DataTable
        Dim cost As Decimal
        Dim sql As String = String.Empty
        If RadioAll.Checked = True Then
            sql = "Select AccountID ,ACG.AccountName as AccountName,Mobile1,Area,GroupID,OtherName,CrateID,CrateName,(Select Rate From CrateMarka Where ID=CrateID)as CrateRate," & _
       " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') -" & _
       " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable" & _
       " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by AccountID,CrateID Having Reciveable>0 order by upper(ACG.AccountName);"
        ElseIf RadioCustomer.Checked = True Then
            sql = "Select AccountID ,ACG.AccountName as AccountName,Mobile1,Area,GroupID,OtherName,CrateID,CrateName,(Select Rate From CrateMarka Where ID=CrateID)as CrateRate," & _
       " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') -" & _
       " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable" & _
       " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and GroupID in(32,16) " & condtion & " Group by AccountID,CrateID Having Reciveable>0 order by upper(ACG.AccountName);"
        ElseIf RadioSupplier.Checked = True Then
            sql = "Select AccountID ,ACG.AccountName as AccountName,Mobile1,Area,GroupID,OtherName,CrateID,CrateName,(Select Rate From CrateMarka Where ID=CrateID)as CrateRate," & _
       " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') -" & _
       " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable" & _
       " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and GroupID in(33,17) " & condtion & " Group by AccountID,CrateID Having Reciveable>0 order by upper(ACG.AccountName);"
        End If
        'sql = "Select AccountID ,ACG.AccountName as AccountName,Mobile1,Area,GroupID,OtherName,CrateID,CrateName,(Select Rate From CrateMarka Where ID=CrateID)as CrateRate," & _
        '      " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '2022-06-26') -" & _
        '      " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '2022-06-26')) as Reciveable" & _
        '      " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '2022-06-26' " & condtion & " Group by AccountID,CrateID Having Reciveable>0 order by upper(ACG.AccountName);"

        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("OtherName").ToString()
                        .Cells(3).Value = dt.Rows(i)("Area").ToString()
                        .Cells(4).Value = dt.Rows(i)("MObile1").ToString()
                        .Cells(5).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(6).Value = dt.Rows(i)("Reciveable").ToString()
                        .Cells(7).Value = Format(Val(dt.Rows(i)("CrateRate").ToString()) * cost, "0.00")
                    End With
                Next
            End If

        Catch ex As Exception

        End Try

        dg1.ClearSelection() : calc()
    End Sub
    Sub calc()

        txtTotQty.Text = Format(0, "0.00") : txtTotAmt.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotQty.Text = Format(Val(txtTotQty.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotAmt.Text = Format(Val(txtTotAmt.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
        Next
        'lblRecordCount.Visible = True
        'lblRecordCount.Text = "Total Accounts : " & dg1.RowCount
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        'pnlWait.Visible = True
        Retrive()
        'pnlWait.Visible = False
    End Sub
    Private Sub PrintRecord()
        pnlWait.Visible = True
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            pb1.Minimum = 0
            pb1.Maximum = dg1.RowCount
            Application.DoEvents()
            With row
                pb1.Value = IIf(Val(row.Index) < 0, 0, Val(row.Index))
                sql = "insert into Printing(M1,D1, P1, P2,P3, P4, P5,P6,P7,P8,P9) values('" & .Cells(0).Value & "','" & mskFromDate.Text & "'," & _
                      "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                      "'" & .Cells(5).Value & "','" & Format(Val(.Cells(6).Value), "0.00") & "','" & Format(Val(.Cells(7).Value), "0.00") & "'," & _
                      "" & Format(Val(txtTotQty.Text), "0.00") & "," & Format(Val(txtTotAmt.Text), "0.00") & ")"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
        pnlWait.Visible = False
    End Sub
    Private Sub PrintRecord2()
        pnlWait.Visible = True
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            pb1.Minimum = 0
            pb1.Maximum = dg1.RowCount
            Application.DoEvents()
            With row
                pb1.Value = IIf(Val(row.Index) < 0, 0, Val(row.Index))
                sql = "insert into Printing(M1,D1, P1, P2,P3, P4, P5,P6,P7) values('" & .Cells(0).Value & "','" & mskFromDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & Format(Val(.Cells(4).Value), "0.00") & "'," & _
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
        pnlWait.Visible = False
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        PrintRecord()
        Report_Viewer.printReport("\Reports\CrateReceviable.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim offset As Integer
            Dim searchText As String
            If txtCustomerSearch.Text.Trim() <> "" Then
                searchText = " and ACG.AccountName Like '" & txtCustomerSearch.Text.Trim() & "%'"
                offset = 0
                pnlWait.Visible = True
                Retrive(searchText)
                pnlWait.Visible = False
            End If
            If txtCustomerSearch.Text.Trim() = "" Then
                searchText = ""
                offset = 0
                pnlWait.Visible = True
                Retrive()
                pnlWait.Visible = False
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub

    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim offset As Integer
            Dim SearchText As String
            If txtItemSearch.Text.Trim() <> "" Then
                SearchText = " and CrateName Like '" & txtItemSearch.Text.Trim() & "%'"
                offset = 0
                Retrive(SearchText)
            End If
            If txtItemSearch.Text.Trim() = "" Then
                SearchText = ""
                offset = 0
                Retrive()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        PrintRecord()
        Report_Viewer.printReport("\Reports\CrateReceviable2.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub



    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub



    Private Sub ReportViewer1_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtArea_KeyDown(sender As Object, e As KeyEventArgs) Handles txtArea.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim offset As Integer
            Dim SearchText As String
            If txtArea.Text.Trim() <> "" Then
                SearchText = " and Area Like '" & txtArea.Text.Trim() & "%'"
                offset = 0
                pnlWait.Visible = True
                Retrive(SearchText)
                pnlWait.Visible = False
            End If
            If txtArea.Text.Trim() = "" Then
                SearchText = ""
                offset = 0
                pnlWait.Visible = True
                Retrive()
                pnlWait.Visible = False
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtArea_TextChanged(sender As Object, e As EventArgs) Handles txtArea.TextChanged

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class