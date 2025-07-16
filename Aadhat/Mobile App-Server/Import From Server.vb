Public Class Import_From_Server
    Dim VchId As String = String.Empty

    'Public Function GetConnectionstring() As SqlConnection
    '    Dim conn As SqlConnection

    '    Return conn
    'End Function
    Private Sub Import_From_Server_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Import_From_Server_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtOrgID.Text = clsFun.ExecScalarStr("Select OrganizationID From Company")
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums() : rowColums2()
        ' pnlWarning.Visible = True
    End Sub
    Private Sub rowColums()

        dg1.ColumnCount = 12
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Width = 110
        dg1.Columns(3).Name = "Mode" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Rcpt No." : dg1.Columns(4).Width = 99
        dg1.Columns(5).Name = "AccountID" : dg1.Columns(5).Visible = False
        dg1.Columns(6).Name = "Account Name" : dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "Amount" : dg1.Columns(7).Width = 100
        dg1.Columns(8).Name = "Discount" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "Total" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "Remark" : dg1.Columns(10).Width = 200
        dg1.Columns(11).Name = "Cancel" : dg1.Columns(11).Width = 60
    End Sub
    Private Sub rowColums2()
        dg2.ColumnCount = 14
        dg2.Columns(0).Name = "ID" : dg2.Columns(0).Visible = False
        dg2.Columns(1).Name = "No" : dg2.Columns(1).Width = 50
        dg2.Columns(2).Name = "Date" : dg2.Columns(2).Width = 100
        dg2.Columns(3).Name = "AccountID" : dg2.Columns(3).Visible = False
        dg2.Columns(4).Name = "Account Name" : dg2.Columns(4).Width = 150
        dg2.Columns(5).Name = "Type" : dg2.Columns(5).Width = 150
        dg2.Columns(6).Name = "CrateId" : dg2.Columns(6).Visible = False
        dg2.Columns(7).Name = "Marka" : dg2.Columns(7).Width = 100
        dg2.Columns(8).Name = "Qty" : dg2.Columns(8).Width = 50
        dg2.Columns(9).Name = "Rate" : dg2.Columns(9).Width = 100
        dg2.Columns(10).Name = "Amount" : dg2.Columns(10).Width = 100
        dg2.Columns(11).Name = "CashPaid" : dg2.Columns(11).Width = 100
        dg2.Columns(12).Name = "Remark" : dg2.Columns(12).Width = 210
        dg2.Columns(13).Name = "Cancel" : dg2.Columns(13).Width = 60
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
        dt = clsfunSql.ExecDataTable("Select * FROM Vouchers Where OrgID=" & Val(txtOrgID.Text) & " ")
        If dt.Rows.Count > 10 Then dg1.Columns(10).Width = 180
        Try
            For i = 0 To dt.Rows.Count - 1
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells(0).Value = dt.Rows(i)("id").ToString()
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                    .Cells(3).Value = dt.Rows(i)("Mode").ToString()
                    .Cells(4).Value = dt.Rows(i)("ReceiptNo").ToString()
                    .Cells(5).Value = dt.Rows(i)("AccountID").ToString()
                    .Cells(6).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(7).Value = dt.Rows(i)("Amount").ToString()
                    .Cells(8).Value = dt.Rows(i)("Discount").ToString()
                    .Cells(9).Value = dt.Rows(i)("Total").ToString()
                    .Cells(10).Value = dt.Rows(i)("Remark").ToString()
                    If dt.Rows(i)("IsCanceled").ToString() = True Then
                        .Cells(11).Value = "Yes"
                    Else
                        .Cells(11).Value = "No"
                    End If
                    If dt.Rows(i)("TransType").ToString() = "Payment" Then
                        dg1.Rows(i).Cells(1).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(2).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(3).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(4).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(5).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(6).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(7).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(8).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(9).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(10).Style.BackColor = Color.IndianRed
                        dg1.Rows(i).Cells(11).Style.BackColor = Color.IndianRed

                    ElseIf dt.Rows(i)("IsCanceled").ToString() = True Then
                        dg1.Rows(i).Cells(1).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(2).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(3).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(4).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(5).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(6).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(7).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(8).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(9).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(10).Style.BackColor = Color.Coral
                        dg1.Rows(i).Cells(11).Style.BackColor = Color.Coral

                    End If
                End With
            Next
        Catch ex As Exception

        End Try
        dg1.ClearSelection()
    End Sub

    Private Sub retrive2(Optional ByVal condtion As String = "")
        dg2.Rows.Clear()
        Dim dt As New DataTable
        dt = clsfunSql.ExecDataTable("Select * FROM tempCrateVoucher Where OrgID='" & Val(txtOrgID.Text) & "'")
        If dt.Rows.Count > 10 Then dg2.Columns(10).Width = 180
        Try
            For i = 0 To dt.Rows.Count - 1
                dg2.Rows.Add()
                With dg2.Rows(i)
                    .Cells(0).Value = dt.Rows(i)("id").ToString()
                    .Cells(1).Value = dt.Rows(i)("SlipNo").ToString()
                    .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")

                    .Cells(3).Value = dt.Rows(i)("AccountID").ToString()
                    .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = dt.Rows(i)("CrateType").ToString()
                    .Cells(6).Value = dt.Rows(i)("CrateID").ToString()
                    .Cells(7).Value = dt.Rows(i)("CrateName").ToString()
                    .Cells(8).Value = dt.Rows(i)("Qty").ToString()
                    .Cells(9).Value = dt.Rows(i)("Rate").ToString()
                    .Cells(10).Value = dt.Rows(i)("Amount").ToString()
                    If dt.Rows(i)("CashPaid").ToString() = True Then
                        .Cells(11).Value = "Yes"
                    Else
                        .Cells(11).Value = "No"
                    End If
                    .Cells(12).Value = dt.Rows(i)("Remark").ToString()
                    If dt.Rows(i)("IsCanceled").ToString() = True Then
                        .Cells(13).Value = "Yes"
                    Else
                        .Cells(13).Value = "No"
                    End If
                    If dt.Rows(i)("CrateType").ToString() = "Crate Out" Then
                        dg2.Rows(i).Cells(1).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(2).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(3).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(4).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(5).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(6).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(7).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(8).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(9).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(10).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(11).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(12).Style.BackColor = Color.IndianRed
                        dg2.Rows(i).Cells(13).Style.BackColor = Color.IndianRed
                    ElseIf dt.Rows(i)("IsCanceled").ToString() = True Then
                        dg2.Rows(i).Cells(1).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(2).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(3).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(4).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(5).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(6).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(7).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(8).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(9).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(10).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(11).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(12).Style.BackColor = Color.Coral
                        dg2.Rows(i).Cells(13).Style.BackColor = Color.Coral
                    End If
                End With
            Next
        Catch ex As Exception

        End Try
        dg2.ClearSelection()
    End Sub
    Private Sub save()
        Dim id As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            Application.DoEvents()
            With row
                If .Cells(11).Value <> "Yes" Then
                    Dim vno As String = ""
                    If .Cells(2).Value = "Receipt" Then
                        .Cells(2).Value = "Receipt"
                    Else
                        .Cells(2).Value = .Cells(2).Value
                    End If
                    If vno = clsFun.ExecScalarStr("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & .Cells(2).Value & "'") > 0 Then
                        vno = vno + 1
                    Else
                        vno = clsFun.ExecScalarStr("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & .Cells(2).Value & "'")
                        vno = vno + 1
                    End If
                    Dim sallerID As String = clsFun.ExecScalarStr("Select ID From Accounts Where AccountName='" & .Cells(3).Value & "'")
                    Dim sql As String = "insert into Vouchers (EntryDate,TransType,SallerID,sallerName,billNo,AccountID,AccountName,BasicAmount,DiscountAmount,TotalAmount,Remark,InvoiceID) " &
                                        "values ('" & CDate(.Cells(1).Value).ToString("yyyy-MM-dd") & "','" & .Cells(2).Value & "','" & sallerID & "','" & .Cells(3).Value & "','" & vno & "', " &
                                        "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & vno & "')"
                    Try
                        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                        If cmd.ExecuteNonQuery() > 0 Then
                            If .Cells(2).Value = "Receipt" Then
                                Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                                Dim Remark2 As String = clsFun.ExecScalarStr("Select ' Receipt No. : '|| billNo  ||', Received Amt : ' ||BasicAmount ||', Dis Amt : ' ||DiscountAmount ||', Total Amt : ' ||TotalAmount  From Vouchers Where ID=" & Val(VchId) & "")
                                If Val(.Cells(7).Value) > 0 Then ''Party Account
                                    clsFun.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, "C", Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & " Imported From Server", .Cells(6).Value)
                                    ClsFunserver.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, "C", 1, OrgID, Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & " Imported From Server", .Cells(6).Value)
                                End If
                                If Val(.Cells(8).Value) > 0 Then ''Discount Amount
                                    clsFun.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, 17, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17"), .Cells(8).Value, "D", Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                    ClsFunserver.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, 17, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17"), .Cells(8).Value, "D", 1, OrgID, Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                End If
                                If Val(.Cells(9).Value) > 0 Then ''Total Amout
                                    If Val(sallerID) > 0 Then ''Party Account
                                        clsFun.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, sallerID, .Cells(3).Value, .Cells(9).Value, "D", Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                        ClsFunserver.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, 17, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17"), .Cells(8).Value, "D", 1, OrgID, Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                    End If
                                End If
                            Else
                                Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                                Dim Remark2 As String = clsFun.ExecScalarStr("Select ' Receipt No. : '|| billNo  ||', Received Amt : ' ||BasicAmount ||', Dis Amt : ' ||DiscountAmount ||', Total Amt : ' ||TotalAmount  From Vouchers Where ID=" & Val(VchId) & "")
                                If Val(.Cells(7).Value) > 0 Then ''Party Account
                                    clsFun.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, "D", Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & " Imported From Server", .Cells(6).Value)
                                    ClsFunserver.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, "D", 1, OrgID, Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & " Imported From Server", .Cells(6).Value)
                                End If
                                If Val(.Cells(8).Value) > 0 Then ''Discount Amount
                                    clsFun.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, 17, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17"), .Cells(8).Value, "C", Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                    ClsFunserver.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, 17, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17"), .Cells(8).Value, "C", 1, OrgID, Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                End If
                                If Val(.Cells(9).Value) > 0 Then ''Total Amout
                                    If Val(sallerID) > 0 Then ''Party Account
                                        clsFun.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, sallerID, .Cells(3).Value, .Cells(9).Value, "C", Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                        ClsFunserver.Ledger(0, VchId, CDate(.Cells(1).Value).ToString("yyyy-MM-dd"), .Cells(2).Value, sallerID, .Cells(3).Value, .Cells(9).Value, "C", 1, OrgID, Remark2 & IIf(.Cells(10).Value = "", "", ", Remark :" & .Cells(10).Value) & "Imported From Server", .Cells(6).Value)
                                    End If
                                End If
                            End If
                            id = id & row.Cells(0).Value & ","
                        End If
                        clsFun.CloseConnection()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        clsFun.CloseConnection()
                    End Try
                End If
            End With
        Next
        If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
        If id = "" Then Exit Sub
        id = id.Remove(id.LastIndexOf(","))
        clsfunSql.ExecScalarInt("Delete Vouchers Where ID in(" & id & ")")
        'MsgBox("Imported Data Form Server Completed Successfully....", vbInformation, "Imported")
        retrive()
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
        retrive() : retrive2()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
        ButtonControl() : save() : saveCrate() : ButtonControl() : MsgBox("Imported Data Form Server Completed Successfully....", vbInformation, "Imported")
    End Sub
    Private Sub saveCrate()
        Dim sql As String = String.Empty
        Dim vno As String = String.Empty
        Dim id As String = String.Empty
        For Each row As DataGridViewRow In dg2.Rows
            Application.DoEvents()
            With row
                If vno = clsFun.ExecScalarStr("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & .Cells(5).Value & "'") > 0 Then
                    vno = vno + 1
                Else
                    vno = clsFun.ExecScalarStr("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & .Cells(5).Value & "'")
                    vno = vno + 1
                End If
                sql = "insert into Vouchers (TransType,EntryDate,BillNo,Remark) values ('" & .Cells(5).Value & "','" & CDate(.Cells(2).Value).ToString("yyyy-MM-dd") & "','" & vno & "','(Import From Server)')"
                Try
                    cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                    If cmd.ExecuteNonQuery() > 0 Then
                        VchId = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                        Dim cashpaid As String = ""
                        If .Cells(11).Value = "Yes" Then
                            cashpaid = "Y"
                        Else
                            cashpaid = "N"
                        End If
                        clsFun.CrateLedger(0, VchId, vno, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, Val(.Cells(3).Value), .Cells(4).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, .Cells(8).Value, "Imported From Server", .Cells(9).Value, .Cells(10).Value, cashpaid)
                        ClsFunserver.CrateLedger(0, VchId, vno, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, Val(.Cells(3).Value), .Cells(4).Value, .Cells(5).Value, .Cells(6).Value, .Cells(7).Value, .Cells(8).Value, "Imported From Server", .Cells(9).Value, .Cells(10).Value, cashpaid, 1, Val(OrgID))
                        Dim Remark2 As String = .Cells(7).Value & ", Qty : " & .Cells(8).Value & ", Rate /- " & .Cells(9).Value & " ( " & IIf(.Cells(11).Value = "Yes", "Cash Recived", "Bardana Recived") & " )"
                        If Val(.Cells(10).Value) > 0 Then ''Party Account
                            clsFun.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, Val(.Cells(3).Value), .Cells(4).Value, Val(.Cells(10).Value), "C", Remark2 & IIf(.Cells(12).Value = "", "Import From Server ", " Import From Server, Remark :" & .Cells(12).Value), .Cells(4).Value)
                            ClsFunserver.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, Val(.Cells(3).Value), .Cells(4).Value, Val(.Cells(10).Value), "C", 1, Val(OrgID), Remark2 & IIf(.Cells(12).Value = "", "Import From Server ", " Import From Server, Remark :" & .Cells(12).Value), .Cells(4).Value)
                        End If
                        If Val(.Cells(10).Value) > 0 Then ''Total Amout
                            If Val(.Cells(6).Value) > 0 Then ''Party Account
                                If .Cells(11).Value = "Yes" Then
                                    clsFun.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, 7, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=7"), Val(.Cells(10).Value), "D", Remark2 & IIf(.Cells(12).Value = "", "Import From Server ", " Import From Server, Remark :" & .Cells(12).Value), .Cells(4).Value)
                                    ClsFunserver.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, 7, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=7"), Val(.Cells(10).Value), "D", 1, Val(OrgID), Remark2 & IIf(.Cells(12).Value = "", "Import From Server ", " Import From Server, Remark :" & .Cells(12).Value), .Cells(4).Value)

                                    '  clsFun.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), Me.Text, 7, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=7"), Val(txtCrateAmt.Text), "D", Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text), txtAccount.Text)
                                Else
                                    clsFun.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, 4, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=4"), Val(.Cells(10).Value), "D", Remark2 & IIf(.Cells(12).Value = "", "Import From Server ", " Import From Server, Remark :" & .Cells(12).Value), .Cells(4).Value)
                                    ClsFunserver.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), .Cells(5).Value, 4, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=4"), Val(.Cells(10).Value), "D", 1, Val(OrgID), Remark2 & IIf(.Cells(12).Value = "", "Import From Server ", " Import From Server, Remark :" & .Cells(12).Value), .Cells(4).Value)
                                    ' clsFun.Ledger(0, VchId, CDate(.Cells(2).Value).ToString("yyyy-MM-dd"), Me.Text, 4, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=4"), Val(txtCrateAmt.Text), "D", Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text), txtAccount.Text)
                                End If
                            End If
                        End If
                        id = id & row.Cells(0).Value & ","
                    End If
                    clsFun.CloseConnection()
                Catch ex As Exception
                    MsgBox(ex.Message)
                    clsFun.CloseConnection()
                End Try
            End With
        Next
        If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
        If id = "" Then Exit Sub
        id = id.Remove(id.LastIndexOf(","))
        clsfunSql.ExecScalarInt("Delete TempCrateVoucher Where ID in(" & id & ")")
        retrive2()
        ' Purchase.BtnRefresh.PerformClick()
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPanlhide_Click(sender As Object, e As EventArgs) Handles btnPanlhide.Click
        pnlWarning.Visible = False
    End Sub

    Private Sub pnlWarning_Paint(sender As Object, e As PaintEventArgs) Handles pnlWarning.Paint

    End Sub

    Private Sub pnlWarning_VisibleChanged(sender As Object, e As EventArgs) Handles pnlWarning.VisibleChanged
        If pnlWarning.Visible = True Then
            btnShow.Visible = False
        Else
            btnShow.Visible = True
        End If
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
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ButtonControl() : save() : ButtonControl() : MsgBox("Imported Data Form Server Completed Successfully....", vbInformation, "Imported")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MessageBox.Show("Are you Want to Delete Canceled Vouchers Permanently....??", "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
            Dim id As String = String.Empty
            For Each row As DataGridViewRow In dg1.Rows
                With row
                    If .Cells(11).Value = "Yes" Then
                        id = id & row.Cells(0).Value & ","
                    End If
                End With
            Next
            If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
            If id <> "" Then
                If id = "" Then Exit Sub
                id = id.Remove(id.LastIndexOf(","))
                ButtonControl()
                clsfunSql.ExecScalarInt("Delete Vouchers Where ID in(" & id & ")")
                ButtonControl()
                MsgBox("Cencelled Record Deleted Permanentaly....", vbInformation, "Deleted Permanent")
            Else
                MsgBox("No Canceled Record Found...", vbInformation, "No Canceled Voucher")
            End If
            retrive()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        If MessageBox.Show("Are you Want to Delete Canceled Crate Vouchers Permanently....??", "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
            Dim id As String = String.Empty
            For Each row As DataGridViewRow In dg2.Rows
                With row
                    If .Cells(13).Value = "Yes" Then
                        id = id & row.Cells(0).Value & ","
                    End If
                End With
            Next
            If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
            If id <> "" Then
                If id = "" Then Exit Sub
                id = id.Remove(id.LastIndexOf(","))
                ButtonControl()
                clsfunSql.ExecScalarInt("Delete TempCrateVoucher Where ID in(" & id & ")")
                ButtonControl()

                MsgBox("Cencelled Record Deleted Permanentaly....", vbInformation, "Deleted Permanent")
            Else
                MsgBox("No Canceled Record Found...", vbInformation, "No Canceled Crates")
            End If
            retrive2()
        End If

    End Sub
End Class