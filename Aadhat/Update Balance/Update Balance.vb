Public Class Update_Balance

    Private Sub Update_Balance_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Update_Balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
    End Sub
    Private Sub updateBal()
        'lblStatus.Text = "Importing Balancing From Accounts..."
        ' Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        ''  clsFun.ExecNonQuery("Update Accounts set OpBal=0")
        Dim ssql As String = "" : ProgressBar1.Visible = True
        '' clsFun.GetConnection()

        Dim CopyDestPath As String = String.Empty
        GlobalData.ConnectionPath = CopyDestPath & "\data.db"
        dt = clsFun.ExecDataTable("Select OpBal,id,dc FROM Accounts")
        For i = 0 To dt.Rows.Count - 1
            ProgressBar1.Maximum = dt.Rows.Count
            lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
            ProgressBar1.Value = i

            opbal = dt.Rows(i)("opbal").ToString()
            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("Id").ToString()) & "")
            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("Id").ToString()) & "")
            ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(i)("Id").ToString()) & "")
            If drcr = "Dr" Then
                tmpamtdr = Val(opbal) + Val(tmpamtdr)
            Else
                tmpamtcr = Val(opbal) + Val(tmpamtcr)
            End If
            Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
            If drcr = "Cr" Then
                opbal = Math.Abs(Val(opbal)) & " Cr"
            Else
                opbal = Math.Abs(Val(opbal)) & " Dr"
            End If
            Dim cntbal As Integer = 0
            cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(dt.Rows(i)("Id").ToString()) & "")
            If cntbal = 0 Then

                opbal = Math.Abs(Val(opbal)) & "," & dt.Rows(0)("Dc").ToString()
                ssql = ssql & "Update Accounts set opbal=" & opbal.Split(",")(0) & " where id=" & dt.Rows(i)("id").ToString() & ";"
                ''clsFun.ExecNonQuery("Update Accounts set obbal=" & opbal & " where id=" & dt.Rows(i)("id").ToString() & "")
            Else
                If Val(tmpamtcr) > Val(tmpamtdr) Then
                    opbal = Math.Abs(Val(tmpamt)) & ",Cr"

                Else
                    opbal = Math.Abs(Val(tmpamt)) & ",Dr"
                End If
                ssql = ssql & "Update Accounts set opbal=" & opbal.Split(",")(0) & " ,DC='" & opbal.Split(",")(1) & "' where id=" & dt.Rows(i)("id").ToString() & ";"
                '' clsFun.ExecNonQuery("Update Accounts set obbal=" & opbal.Split(",")(0) & " ,DC='" & opbal.Split(",")(0) & "' where id=" & dt.Rows(i)("id").ToString() & "")
            End If
        Next
        Dim a As Integer = clsFun.ExecNonQuery(ssql, True)

        'If a > 0 Then
        '    '" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "','" & CDate(mskNewtoDate.Text).ToString("yyyy-MM-dd") & "' ,'" & tmpDataPath & "'
        '    ssql = ""
        '    ssql = "delete from Vouchers; delete from sqlite_sequence where name='Vouchers';delete from Ledger; delete from sqlite_sequence where name='Ledger';delete from Transaction1; delete from sqlite_sequence where name='Transaction1';delete from Transaction2; delete from sqlite_sequence where name='Transaction2'; " & _
        '           "delete from Purchase; delete from sqlite_sequence where name='Purchase';delete from ChargesTrans;delete from sqlite_sequence where name='ChargesTrans'; delete from Licence;" & _
        '           " Update Company set OrganizationID=0,Password='',Autosync='N',YearStart='" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "',YearEnd='" & CDate(mskNewtoDate.Text).ToString("yyyy-MM-dd") & "',CompData='" & tmpDataPath & "';"
        '    Dim a1 As Integer = clsFun.ExecNonQuery(ssql, True)

        '    ssql = ""
        '    ssql = "Update CrateVoucher Set VoucherID=0,EntryDate='" & CDate(mskNewFromDate.Text).ToString("yyyy-MM-dd") & "'"
        '    a1 = clsFun.ExecNonQuery(ssql, True)
        '    If a1 > 0 Then MsgBox("Financial Year Changed Successfully...") : CompanyList.BtnRetrive.PerformClick() : Me.Close()
        'End If
    End Sub
End Class