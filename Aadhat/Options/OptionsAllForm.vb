Public Class OptionsAllForm
    Dim SuperSaleVehicleNo As String = ""
    Dim SuperSaleCut As String = ""
    Private Sub saveChanges()
        Dim cmd As New SQLite.SQLiteCommand
        If CkSuperSaleVehicle.CheckState = CheckState.Checked Then SuperSaleVehicleNo = "Y" Else SuperSaleVehicleNo = "N"
        If ckSuperSaleCut.CheckState = CheckState.Checked Then SuperSaleCut = "Y" Else SuperSaleCut = "N"
        If CkOctroi.CheckState = CheckState.Checked Then Octroi = "Y" Else Octroi = "N"
        If ckDiff.CheckState = CheckState.Checked Then diffrence = "Y" Else diffrence = "N"
        If ckCrateRate.CheckState = CheckState.Checked Then CrateRate = "Y" Else CrateRate = "N"
        If ckChargesAfter.CheckState = CheckState.Checked Then Chargesafter = "Y" Else Chargesafter = "N"
        If ckbardanaSameAccount.CheckState = CheckState.Checked Then SameAccount = "Y" Else SameAccount = "N"
        Dim sql As String = "Update Options SET SuperSaleVehicleNo='" & SuperSaleVehicleNo & "', " & _
            "SuperSaleCut='" & SuperSaleCut & "',Octroi='" & Octroi & "',IsDiff='" & diffrence & "',CrateRate='" & CrateRate & "',SellOutCharges='" & Chargesafter & "',BardanaInSameAccount='" & sameAccount & "' WHERE ID=" & Val(txtID.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                StopSpeedSale() : StopSuperSale() : StopkStockSale() : StandardSale() : ReceiptPayment()
                SpeedSale.FillSpeedSale() : Super_Sale.FillSuperSale() : Stock_Sale.FillStockSale() : Standard_Sale.FillstdSale() : ReceiptForm.FillRecipt() : PayMentform.FillPayment()
                MsgBox("Setting Updated Successfully.", vbInformation + vbOKOnly, "Options Updated")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub StopSpeedSale()
        clsFun.ExecNonQuery("Delete From Option1")
        Dim sql As String = "insert into Option1(CommPerStop, CommAmtStop,  MandiTaxStop, MandiTaxAmtStop, RDFPerStop, RDFAmtStop, TareStop, TareAmtStop," _
                            & " LabourStop, LabourAmtStop,TareONWeight,RoundOff,NetRate,ChooseDate)" _
                            & "values ('" & IIf(ckCommPer.Checked = True, "Y", "N") & "','" & IIf(ckCommAmt.Checked = True, "Y", "N") & "','" & IIf(ckMTaxPer.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckMAmt.Checked = True, "Y", "N") & "','" & IIf(ckRdfPer.Checked = True, "Y", "N") & "','" & IIf(ckRdfAmt.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckTare.Checked = True, "Y", "N") & "','" & IIf(ckTareAmt.Checked = True, "Y", "N") & "','" & IIf(ckLabour.Checked = True, "Y", "N") & "'," & _
                            "'" & IIf(ckLabourAmt.Checked = True, "Y", "N") & "','" & IIf(CkSpeedbardanaOnKg.Checked = True, "Y", "N") & "','" & IIf(ckSpeedRoundoff.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckSpeedNetRate.Checked = True, "Y", "N") & "','" & CbSpeedDefault.SelectedIndex & "')"
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub StopSuperSale()
        clsFun.ExecNonQuery("Delete From Option2")
        Dim sql As String = "insert into Option2(CommPerStop, CommAmtStop,  MandiTaxStop, MandiTaxAmtStop, RDFPerStop, RDFAmtStop, TareStop, TareAmtStop," _
                            & " LabourStop, LabourAmtStop,TareONWeight,VehicleNo,AskForPrint,AskForCut,RoundOff,NetRate,chooseDate)" _
                            & "values ('" & IIf(ckSuperCommPer.Checked = True, "Y", "N") & "','" & IIf(ckSuperCommAmt.Checked = True, "Y", "N") & "','" & IIf(CkSuperTarePer.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckSuperTaxAmt.Checked = True, "Y", "N") & "','" & IIf(ckSuperRdfPer.Checked = True, "Y", "N") & "','" & IIf(ckSuperRdfAmt.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(CkSuperTarePer.Checked = True, "Y", "N") & "','" & IIf(CkSuperTareAmt.Checked = True, "Y", "N") & "','" & IIf(ckSuperLabour.Checked = True, "Y", "N") & "'," & _
                            "'" & IIf(CkSuperLabourAmt.Checked = True, "Y", "N") & "','" & IIf(ckSuperBardanaOnkg.Checked = True, "Y", "N") & "','" & IIf(CkSuperSaleVehicle.Checked = True, "Y", "N") & "'," & _
                            "'" & IIf(ckSuperSalePrint.Checked = True, "Y", "N") & "','" & IIf(ckSuperSaleCut.Checked = True, "Y", "N") & "','" & IIf(ckSuperRoundoff.Checked = True, "Y", "N") & "','" & IIf(ckStockNetRate.Checked = True, "Y", "N") & "','" & cbSuperDefault.SelectedIndex & "')"
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub StopkStockSale()
        clsFun.ExecNonQuery("Delete From Option3")
        Dim sql As String = "insert into Option3(CommPerStop, CommAmtStop,  MandiTaxStop, MandiTaxAmtStop, RDFPerStop, RDFAmtStop, TareStop, TareAmtStop," _
                            & " LabourStop, LabourAmtStop,TareONWeight,RoundOff,NetRate,ChooseDate)" _
                            & "values ('" & IIf(ckStockCommPer.Checked = True, "Y", "N") & "','" & IIf(ckStockCommAmt.Checked = True, "Y", "N") & "','" & IIf(CkStockMPer.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckStockMAmt.Checked = True, "Y", "N") & "','" & IIf(CkStockRDFPer.Checked = True, "Y", "N") & "','" & IIf(ckStockRDFAmt.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckStockTare.Checked = True, "Y", "N") & "','" & IIf(CkStockTareAmt.Checked = True, "Y", "N") & "','" & IIf(ckStockLabourPer.Checked = True, "Y", "N") & "'," & _
                            "'" & IIf(ckStockLabourAmt.Checked = True, "Y", "N") & "','" & IIf(ckStockBardanaonKg.Checked = True, "Y", "N") & "','" & IIf(ckStockRoundOff.Checked = True, "Y", "N") & "','" & IIf(ckStockNetRate.Checked = True, "Y", "N") & "','" & cbStockDefault.SelectedIndex & "')"
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub StandardSale()
        clsFun.ExecNonQuery("Delete From Option4")
        Dim sql As String = "insert into Option4(CommPerStop, CommAmtStop,  MandiTaxStop, MandiTaxAmtStop, RDFPerStop, RDFAmtStop, TareStop, TareAmtStop," _
                            & " LabourStop, LabourAmtStop,TareONWeight,RoundOff,NetRate,ChooseDate)" _
                            & "values ('" & IIf(ckStdCommPer.Checked = True, "Y", "N") & "','" & IIf(ckStdCommAmt.Checked = True, "Y", "N") & "','" & IIf(ckStndMandiPer.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckstdMandiAmt.Checked = True, "Y", "N") & "','" & IIf(ckStdRdfPer.Checked = True, "Y", "N") & "','" & IIf(ckStdRdfAmt.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckStdTarePer.Checked = True, "Y", "N") & "','" & IIf(ckStdTareAmt.Checked = True, "Y", "N") & "','" & IIf(ckStdLabourPer.Checked = True, "Y", "N") & "'," & _
                            "'" & IIf(CkStdLoabourAmt.Checked = True, "Y", "N") & "','" & IIf(CkStdBardanaOnKg.Checked = True, "Y", "N") & "','" & IIf(ckStdRoundOff.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckStdNetRate.Checked = True, "Y", "N") & "','" & cbStdDefault.SelectedIndex & "')"
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ReceiptPayment()
        Dim RecPrint As String = String.Empty
        Dim PayPrint As String = String.Empty

        If RadioRecNoPrint.Checked = True Then RecPrint = "N"
        If RadioRecAsk.Checked = True Then RecPrint = "A"
        If RadioRecDirect.Checked = True Then RecPrint = "D"
        If RadioPayNoPrint.Checked = True Then PayPrint = "N"
        If RadioPayAsk.Checked = True Then PayPrint = "A"
        If RadioPayDirect.Checked = True Then PayPrint = "D"
        clsFun.ExecNonQuery("Delete From Option5")
        Dim sql As String = "insert into Option5(RecDate, RecNo,  RecDiscount, RecTotal, RecRemark, PayDate, PayNo, PayDiscount,PayTotal," _
                            & " PayRemark, RecPrint,PayPrint,ReceiptDate,PaymentDate)" _
                            & "values ('" & IIf(CkRecDate.Checked = True, "Y", "N") & "','" & IIf(ckRecNo.Checked = True, "Y", "N") & "','" & IIf(ckRecDis.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckRecTot.Checked = True, "Y", "N") & "','" & IIf(ckRecRemark.Checked = True, "Y", "N") & "','" & IIf(ckPayDate.Checked = True, "Y", "N") & "', " & _
                            "'" & IIf(ckPayNo.Checked = True, "Y", "N") & "','" & IIf(ckPayDis.Checked = True, "Y", "N") & "','" & IIf(ckPayTot.Checked = True, "Y", "N") & "'," & _
                            "'" & IIf(ckPayRemark.Checked = True, "Y", "N") & "','" & RecPrint & "','" & PayPrint & "','" & cbReceipt.SelectedIndex & "','" & cbPayement.SelectedIndex & "')"
        cmd = New SQLite.SQLiteCommand(Sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(Sql) > 0 Then
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub FillGeneral(ByVal id As Integer)
        ssql = "Select * from Options where id=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            SuperSaleVehicleNo = dt.Rows(0)("SuperSaleVehicleNo").ToString().Trim()
            If SuperSaleVehicleNo = "Y" Then CkSuperSaleVehicle.CheckState = CheckState.Checked Else CkSuperSaleVehicle.CheckState = CheckState.Unchecked
            SuperSaleCut = dt.Rows(0)("SuperSaleCut").ToString().Trim()
            If SuperSaleCut = "Y" Then ckSuperSaleCut.CheckState = CheckState.Checked Else ckSuperSaleCut.CheckState = CheckState.Unchecked
            Octroi = dt.Rows(0)("Octroi").ToString().Trim()
            If Octroi = "Y" Then CkOctroi.CheckState = CheckState.Checked Else CkOctroi.CheckState = CheckState.Unchecked
            Diff = dt.Rows(0)("IsDiff").ToString().Trim()
            If Diff = "Y" Then ckDiff.CheckState = CheckState.Checked Else ckDiff.CheckState = CheckState.Unchecked
            CrateRate = dt.Rows(0)("CrateRate").ToString().Trim()
            If CrateRate = "Y" Then ckCrateRate.CheckState = CheckState.Checked Else ckCrateRate.CheckState = CheckState.Unchecked
            ChargesAfter = dt.Rows(0)("SellOutCharges").ToString().Trim()
            If ChargesAfter = "Y" Then ckChargesAfter.CheckState = CheckState.Checked Else ckChargesAfter.CheckState = CheckState.Unchecked
            SameAccount = dt.Rows(0)("BardanaInSameAccount").ToString().Trim()
            If SameAccount = "Y" Then ckbardanaSameAccount.CheckState = CheckState.Checked Else ckbardanaSameAccount.CheckState = CheckState.Unchecked
            txtID.Text = dt.Rows(0)("ID").ToString()
        End If
        FillSpeedSale() : FillSuperSale() : FillStockSale() : FillStdSale() : FillRecPayment()
    End Sub
    Private Sub FillSpeedSale()
        ssql = "Select * from Option1 "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CommPerStop").ToString().Trim() = "Y" Then ckCommPer.Checked = True Else ckCommPer.Checked = False
            If dt.Rows(0)("CommAmtStop").ToString().Trim() = "Y" Then ckCommAmt.Checked = True Else ckCommAmt.Checked = False
            If dt.Rows(0)("MandiTaxStop").ToString().Trim() = "Y" Then ckMTaxPer.Checked = True Else ckMTaxPer.Checked = False
            If dt.Rows(0)("MandiTaxAmtStop").ToString().Trim() = "Y" Then ckMAmt.Checked = True Else ckMAmt.Checked = False
            If dt.Rows(0)("RDFPerStop").ToString().Trim() = "Y" Then ckRdfPer.Checked = True Else ckRdfPer.Checked = False
            If dt.Rows(0)("RDFAmtStop").ToString().Trim() = "Y" Then ckRdfAmt.Checked = True Else ckRdfAmt.Checked = False
            If dt.Rows(0)("TareStop").ToString().Trim() = "Y" Then ckTare.Checked = True Else ckTare.Checked = False
            If dt.Rows(0)("TareAmtStop").ToString().Trim() = "Y" Then ckTareAmt.Checked = True Else ckTareAmt.Checked = False
            If dt.Rows(0)("LabourStop").ToString().Trim() = "Y" Then ckLabour.Checked = True Else ckLabour.Checked = False
            If dt.Rows(0)("LabourAmtStop").ToString().Trim() = "Y" Then ckLabourAmt.Checked = True Else ckLabourAmt.Checked = False
            If dt.Rows(0)("TareONWeight").ToString().Trim() = "Y" Then CkSpeedbardanaOnKg.Checked = True Else CkSpeedbardanaOnKg.Checked = False
            If dt.Rows(0)("Roundoff").ToString().Trim() = "Y" Then ckSpeedRoundoff.Checked = True Else ckSpeedRoundoff.Checked = False
            If dt.Rows(0)("NetRate").ToString().Trim() = "Y" Then ckSpeedNetRate.Checked = True Else ckSpeedNetRate.Checked = False
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 0 Then CbSpeedDefault.SelectedIndex = 0
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 1 Then CbSpeedDefault.SelectedIndex = 1
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 2 Then CbSpeedDefault.SelectedIndex = 2
        End If
    End Sub
    Private Sub FillSuperSale()
        ssql = "Select * from Option2 "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CommPerStop").ToString().Trim() = "Y" Then ckSuperCommPer.Checked = True Else ckSuperCommPer.Checked = False
            If dt.Rows(0)("CommAmtStop").ToString().Trim() = "Y" Then ckSuperCommAmt.Checked = True Else ckSuperCommAmt.Checked = False
            If dt.Rows(0)("MandiTaxStop").ToString().Trim() = "Y" Then ckSuperTaxPer.Checked = True Else ckSuperTaxPer.Checked = False
            If dt.Rows(0)("MandiTaxAmtStop").ToString().Trim() = "Y" Then ckSuperTaxAmt.Checked = True Else ckSuperTaxAmt.Checked = False
            If dt.Rows(0)("RDFPerStop").ToString().Trim() = "Y" Then ckSuperRdfPer.Checked = True Else ckSuperRdfPer.Checked = False
            If dt.Rows(0)("RDFAmtStop").ToString().Trim() = "Y" Then ckSuperRdfAmt.Checked = True Else ckSuperRdfAmt.Checked = False
            If dt.Rows(0)("TareStop").ToString().Trim() = "Y" Then CkSuperTarePer.Checked = True Else CkSuperTarePer.Checked = False
            If dt.Rows(0)("TareAmtStop").ToString().Trim() = "Y" Then CkSuperTareAmt.Checked = True Else CkSuperTareAmt.Checked = False
            If dt.Rows(0)("LabourStop").ToString().Trim() = "Y" Then ckSuperLabour.Checked = True Else ckSuperLabour.Checked = False
            If dt.Rows(0)("LabourAmtStop").ToString().Trim() = "Y" Then CkSuperLabourAmt.Checked = True Else CkSuperLabourAmt.Checked = False
            If dt.Rows(0)("TareONWeight").ToString().Trim() = "Y" Then ckSuperBardanaOnkg.Checked = True Else ckSuperBardanaOnkg.Checked = False
            If dt.Rows(0)("VehicleNo").ToString().Trim() = "Y" Then CkSuperSaleVehicle.Checked = True Else CkSuperSaleVehicle.Checked = False
            If dt.Rows(0)("AskForPrint").ToString().Trim() = "Y" Then ckSuperSalePrint.Checked = True Else ckSuperSalePrint.Checked = False
            If dt.Rows(0)("AskForCut").ToString().Trim() = "Y" Then ckSuperSalePrint.Checked = True Else ckSuperSalePrint.Checked = False
            If dt.Rows(0)("RoundOff").ToString().Trim() = "Y" Then ckSuperRoundoff.Checked = True Else ckSuperRoundoff.Checked = False
            If dt.Rows(0)("NetRate").ToString().Trim() = "Y" Then ckSuperNetRate.Checked = True Else ckSuperNetRate.Checked = False
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 0 Then cbSuperDefault.SelectedIndex = 0
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 1 Then cbSuperDefault.SelectedIndex = 1
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 2 Then cbSuperDefault.SelectedIndex = 2
        End If
    End Sub
    Private Sub FillStockSale()
        ssql = "Select * from Option3"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CommPerStop").ToString().Trim() = "Y" Then ckStockCommPer.Checked = True Else ckStockCommPer.Checked = False
            If dt.Rows(0)("CommAmtStop").ToString().Trim() = "Y" Then ckStockCommAmt.Checked = True Else ckStockCommAmt.Checked = False
            If dt.Rows(0)("MandiTaxStop").ToString().Trim() = "Y" Then CkStockMPer.Checked = True Else CkStockMPer.Checked = False
            If dt.Rows(0)("MandiTaxAmtStop").ToString().Trim() = "Y" Then ckStockMAmt.Checked = True Else ckStockMAmt.Checked = False
            If dt.Rows(0)("RDFPerStop").ToString().Trim() = "Y" Then CkStockRDFPer.Checked = True Else CkStockRDFPer.Checked = False
            If dt.Rows(0)("RDFAmtStop").ToString().Trim() = "Y" Then ckStockRDFAmt.Checked = True Else ckStockRDFAmt.Checked = False
            If dt.Rows(0)("TareStop").ToString().Trim() = "Y" Then ckStockTare.Checked = True Else ckStockTare.Checked = False
            If dt.Rows(0)("TareAmtStop").ToString().Trim() = "Y" Then CkStockTareAmt.Checked = True Else CkStockTareAmt.Checked = False
            If dt.Rows(0)("LabourStop").ToString().Trim() = "Y" Then ckStockLabourPer.Checked = True Else ckStockLabourPer.Checked = False
            If dt.Rows(0)("LabourAmtStop").ToString().Trim() = "Y" Then ckStockLabourAmt.Checked = True Else ckStockLabourPer.Checked = False
            If dt.Rows(0)("TareONWeight").ToString().Trim() = "Y" Then ckStockBardanaonKg.Checked = True Else ckStockBardanaonKg.Checked = False
            If dt.Rows(0)("RoundOff").ToString().Trim() = "Y" Then ckStockRoundOff.Checked = True Else ckStockRoundOff.Checked = False
            If dt.Rows(0)("NetRate").ToString().Trim() = "Y" Then ckStockNetRate.Checked = True Else ckStockNetRate.Checked = False
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 0 Then cbStockDefault.SelectedIndex = 0
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 1 Then cbStockDefault.SelectedIndex = 1
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 2 Then cbStockDefault.SelectedIndex = 2
        End If
    End Sub
    Private Sub FillStdSale()
        ssql = "Select * from Option4"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CommPerStop").ToString().Trim() = "Y" Then ckStdCommPer.Checked = True Else ckStdCommPer.Checked = False
            If dt.Rows(0)("CommAmtStop").ToString().Trim() = "Y" Then ckStdCommAmt.Checked = True Else ckStdCommAmt.Checked = False
            If dt.Rows(0)("MandiTaxStop").ToString().Trim() = "Y" Then ckStndMandiPer.Checked = True Else ckStndMandiPer.Checked = False
            If dt.Rows(0)("MandiTaxAmtStop").ToString().Trim() = "Y" Then ckstdMandiAmt.Checked = True Else ckstdMandiAmt.Checked = False
            If dt.Rows(0)("RDFPerStop").ToString().Trim() = "Y" Then ckStdRdfPer.Checked = True Else ckStdRdfPer.Checked = False
            If dt.Rows(0)("RDFAmtStop").ToString().Trim() = "Y" Then ckStdRdfAmt.Checked = True Else ckStdRdfAmt.Checked = False
            If dt.Rows(0)("TareStop").ToString().Trim() = "Y" Then ckStdTarePer.Checked = True Else ckStdTarePer.Checked = False
            If dt.Rows(0)("TareAmtStop").ToString().Trim() = "Y" Then ckStdTareAmt.Checked = True Else ckStdTareAmt.Checked = False
            If dt.Rows(0)("LabourStop").ToString().Trim() = "Y" Then ckStdLabourPer.Checked = True Else ckStdLabourPer.Checked = False
            If dt.Rows(0)("LabourAmtStop").ToString().Trim() = "Y" Then CkStdLoabourAmt.Checked = True Else CkStdLoabourAmt.Checked = False
            If dt.Rows(0)("TareONWeight").ToString().Trim() = "Y" Then CkStdBardanaOnKg.Checked = True Else CkStdBardanaOnKg.Checked = False
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 0 Then cbStdDefault.SelectedIndex = 0
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 1 Then cbStdDefault.SelectedIndex = 1
            If Val(dt.Rows(0)("ChooseDate").ToString().Trim()) = 2 Then cbStdDefault.SelectedIndex = 2
        End If
    End Sub
    Private Sub FillRecPayment()
        ssql = "Select * from Option5"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("RecDate").ToString().Trim() = "Y" Then CkRecDate.Checked = True Else CkRecDate.Checked = False
            If dt.Rows(0)("RecNo").ToString().Trim() = "Y" Then ckRecNo.Checked = True Else ckRecNo.Checked = False
            If dt.Rows(0)("RecDiscount").ToString().Trim() = "Y" Then ckRecDis.Checked = True Else ckRecDis.Checked = False
            If dt.Rows(0)("RecTotal").ToString().Trim() = "Y" Then ckRecTot.Checked = True Else ckRecTot.Checked = False
            If dt.Rows(0)("RecRemark").ToString().Trim() = "Y" Then ckRecRemark.Checked = True Else ckRecRemark.Checked = False
            If dt.Rows(0)("PayDate").ToString().Trim() = "Y" Then ckPayDate.Checked = True Else ckPayDate.Checked = False
            If dt.Rows(0)("PayNo").ToString().Trim() = "Y" Then ckPayNo.Checked = True Else ckPayNo.Checked = False
            If dt.Rows(0)("PayDiscount").ToString().Trim() = "Y" Then ckPayDis.Checked = True Else ckPayDis.Checked = False
            If dt.Rows(0)("PayTotal").ToString().Trim() = "Y" Then ckPayTot.Checked = True Else ckPayTot.Checked = False
            If dt.Rows(0)("PayRemark").ToString().Trim() = "Y" Then ckPayRemark.Checked = True Else ckPayRemark.Checked = False


            If dt.Rows(0)("RecPrint").ToString().Trim() = "N" Then RadioRecNoPrint.Checked = True
            If dt.Rows(0)("RecPrint").ToString().Trim() = "A" Then RadioRecAsk.Checked = True
            If dt.Rows(0)("RecPrint").ToString().Trim() = "D" Then RadioRecDirect.Checked = True
            If dt.Rows(0)("PayPrint").ToString().Trim() = "N" Then RadioPayNoPrint.Checked = True
            If dt.Rows(0)("PayPrint").ToString().Trim() = "A" Then RadioPayAsk.Checked = True
            If dt.Rows(0)("PayPrint").ToString().Trim() = "D" Then RadioPayDirect.Checked = True

            If Val(dt.Rows(0)("ReceiptDate").ToString().Trim()) = 0 Then cbReceipt.SelectedIndex = 0
            If Val(dt.Rows(0)("ReceiptDate").ToString().Trim()) = 1 Then cbReceipt.SelectedIndex = 1
            If Val(dt.Rows(0)("ReceiptDate").ToString().Trim()) = 2 Then cbReceipt.SelectedIndex = 2
            If Val(dt.Rows(0)("ReceiptDate").ToString().Trim()) = 0 Then cbPayement.SelectedIndex = 0
            If Val(dt.Rows(0)("PaymentDate").ToString().Trim()) = 1 Then cbPayement.SelectedIndex = 1
            If Val(dt.Rows(0)("PaymentDate").ToString().Trim()) = 2 Then cbPayement.SelectedIndex = 2
        End If
    End Sub
    Private Function Sql() As Object
        Throw New NotImplementedException
    End Function

    Private Sub OptionsAllForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()

    End Sub

    Private Sub OptionsAllForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CbSpeedDefault.SelectedIndex = 0 : cbSuperDefault.SelectedIndex = 0 : cbStockDefault.SelectedIndex = 0
        cbPurchaseDefault.SelectedIndex = 0 : cbStdDefault.SelectedIndex = 0 : cbReceipt.SelectedIndex = 0 : cbPayement.SelectedIndex = 0
        RadioRecNoPrint.Checked = True : RadioPayNoPrint.Checked = True
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True : FillGeneral(txtID.Text)

    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        saveChanges()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Label41_Click(sender As Object, e As EventArgs) Handles Label41.Click

    End Sub

    Private Sub GbSpeed_Enter(sender As Object, e As EventArgs) Handles GbSpeed.Enter

    End Sub
End Class