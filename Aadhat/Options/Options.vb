Public Class Options

    Private Sub Options_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Options_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True : DefaultControls() : FillDefault()
    End Sub

    Private Sub DefaultControls()
        cbDecimal.SelectedIndex = 0 : CbRecipet.SelectedIndex = 0
        cbPayment.SelectedIndex = 0 : cbMannual.SelectedIndex = 0
        cbAuto.SelectedIndex = 0 : cbAnotherAc.SelectedIndex = 0
        cbCrateBardana.SelectedIndex = 0 : cbSameAc.SelectedIndex = 0
        cbDateRange.SelectedIndex = 0 : cbROPurchase.SelectedIndex = 0
        cbROSale.SelectedIndex = 0 : CbEachItems.SelectedIndex = 0
        cbChargesEffect.SelectedIndex = 0 : CbCommOnWeight.SelectedIndex = 0
        CBLimit.SelectedIndex = 0 : cbOctroi.SelectedIndex = 0
        CbStopOnBasic.SelectedIndex = 0 : cbAutoSwitch.SelectedIndex = 0
        CbNetOnSaleRec.SelectedIndex = 0
        ''''''Speed Sale
        cbSpeedComm.SelectedIndex = 0 : cbSpeedMandiTax.SelectedIndex = 0
        cbSpeedRDF.SelectedIndex = 0 : cbSpeedTare.SelectedIndex = 0
        cbSpeedLabour.SelectedIndex = 0 : cbSpeedNetRate.SelectedIndex = 0
        cbSpeedKaat.SelectedIndex = 0
        ''''''Super Sale
        cbSuperComm.SelectedIndex = 0 : cbSuperMandiTax.SelectedIndex = 0
        cbSuperRDF.SelectedIndex = 0 : cbSuperTare.SelectedIndex = 0
        cbSuperlabour.SelectedIndex = 0 : cbSuperNetRate.SelectedIndex = 0
        CbSuperKaat.SelectedIndex = 0 : CbSuperVehicle.SelectedIndex = 0
        cbSuperBasic.SelectedIndex = 0
        ''''''Std Sale
        cbStdComm.SelectedIndex = 0 : cbStdMandiTax.SelectedIndex = 0
        cbStdRDF.SelectedIndex = 0 : cbStdTare.SelectedIndex = 0
        cbStdLabour.SelectedIndex = 0 : CbStdNetRate.SelectedIndex = 0
        cbStdKaat.SelectedIndex = 0
        ''''''Stock Sale
        cbStockComm.SelectedIndex = 0 : cbStockMandiTax.SelectedIndex = 0
        cbStockRDF.SelectedIndex = 0 : cbStockTare.SelectedIndex = 0
        cbStockLabour.SelectedIndex = 0 : cbStockNetRate.SelectedIndex = 0
        cbStockKaat.SelectedIndex = 0

        ''''''rcpt
        cbRcptDate.SelectedIndex = 0 : cbRcptSlip.SelectedIndex = 0
        CbRcptDisc.SelectedIndex = 0 : cbRcptTotal.SelectedIndex = 0
        cbRcptRemark.SelectedIndex = 0

        ''''''Pymt
        cbPymtDate.SelectedIndex = 0 : CbPymtSlip.SelectedIndex = 0
        CbPymtDisc.SelectedIndex = 0 : CbPymtTotal.SelectedIndex = 0
        CbPymtRemark.SelectedIndex = 0
        '' other setting
        cbMargin.SelectedIndex = 0 : cbLanguage.SelectedIndex = 0
        cbPer.SelectedIndex = 0 : cbFarmer.SelectedIndex = 0
        cbSelloutRemark.SelectedIndex = 0

    End Sub
    Public Sub FillDefault()
        ssql = "Select * from Controls"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            cbDecimal.Text = dt.Rows(0)("Decimals").ToString().Trim()
            CbRecipet.Text = dt.Rows(0)("AskReciept").ToString().Trim()
            cbPayment.Text = dt.Rows(0)("AskPayment").ToString().Trim()
            cbMannual.Text = dt.Rows(0)("AskMannual").ToString().Trim()
            cbAuto.Text = dt.Rows(0)("AskAuto").ToString().Trim()
            cbAnotherAc.Text = dt.Rows(0)("sendDiff").ToString().Trim()
            cbCrateBardana.Text = dt.Rows(0)("CrateBardana").ToString().Trim()
            cbSameAc.Text = dt.Rows(0)("TareSameAc").ToString().Trim()
            cbDateRange.Text = dt.Rows(0)("DefaultDate").ToString().Trim()
            cbROPurchase.Text = dt.Rows(0)("PurcahseRO").ToString().Trim()
            cbROSale.Text = dt.Rows(0)("SaleRO").ToString().Trim()
            CbEachItems.Text = dt.Rows(0)("ROEachItem").ToString().Trim()
            cbChargesEffect.Text = dt.Rows(0)("ChargeEffect").ToString().Trim()
            CBLimit.Text = dt.Rows(0)("AskCreditLimit").ToString().Trim()
            CbCommOnWeight.Text = dt.Rows(0)("ApplyCommWeight").ToString().Trim()
            cbOctroi.Text = dt.Rows(0)("Octroi").ToString().Trim()
            CbStopOnBasic.Text = dt.Rows(0)("StopBasic").ToString().Trim()
            cbAutoSwitch.Text = dt.Rows(0)("AutoSwitch").ToString().Trim()
            CbNetOnSaleRec.Text = dt.Rows(0)("OnSaleNet").ToString().Trim()
            ''''''Speed Sale
            cbSpeedComm.Text = dt.Rows(0)("SpeedCommission").ToString().Trim()
            cbSpeedMandiTax.Text = dt.Rows(0)("SpeedMandiTax").ToString().Trim()
            cbSpeedRDF.Text = dt.Rows(0)("SpeedRDF").ToString().Trim()
            cbSpeedTare.Text = dt.Rows(0)("SpeedTare").ToString().Trim()
            cbSpeedLabour.Text = dt.Rows(0)("SpeedLabour").ToString().Trim()
            cbSpeedNetRate.Text = dt.Rows(0)("SpeedTaxPaid").ToString().Trim()
            cbSpeedKaat.Text = dt.Rows(0)("SpeedKaat").ToString().Trim()
            ''''''Super Sale
            cbSuperComm.Text = dt.Rows(0)("SuperCommission").ToString().Trim()
            cbSuperMandiTax.Text = dt.Rows(0)("SuperMandiTax").ToString().Trim()
            cbSuperRDF.Text = dt.Rows(0)("SuperRDF").ToString().Trim()
            cbSuperTare.Text = dt.Rows(0)("SuperTare").ToString().Trim()
            cbSuperlabour.Text = dt.Rows(0)("SuperLabour").ToString().Trim()
            cbSuperNetRate.Text = dt.Rows(0)("SuperTaxPaid").ToString().Trim()
            CbSuperKaat.Text = dt.Rows(0)("SuperKaat").ToString().Trim()
            CbSuperVehicle.Text = dt.Rows(0)("SuperVehicleNo").ToString().Trim()
            cbSuperBasic.Text = dt.Rows(0)("SuperBasic").ToString().Trim()
            ''''''Std Sale
            cbStdComm.Text = dt.Rows(0)("STDCommission").ToString().Trim()
            cbStdMandiTax.Text = dt.Rows(0)("STDMandiTax").ToString().Trim()
            cbStdRDF.Text = dt.Rows(0)("STDRDF").ToString().Trim()
            cbStdTare.Text = dt.Rows(0)("STDTare").ToString().Trim()
            cbStdLabour.Text = dt.Rows(0)("STDLabour").ToString().Trim()
            CbStdNetRate.Text = dt.Rows(0)("StdTaxPaid").ToString().Trim()
            cbStdKaat.Text = dt.Rows(0)("StdKaat").ToString().Trim()
            cbMarking.Text = dt.Rows(0)("STDMark").ToString().Trim()
            cbNoLot.Text = dt.Rows(0)("STDNoLot").ToString().Trim()
            ''''''Stock Sale
            cbStockComm.Text = dt.Rows(0)("StockCommission").ToString().Trim()
            cbStockMandiTax.Text = dt.Rows(0)("StockMandiTax").ToString().Trim()
            cbStockRDF.Text = dt.Rows(0)("StockRDF").ToString().Trim()
            cbStockTare.Text = dt.Rows(0)("StockTare").ToString().Trim()
            cbStockLabour.Text = dt.Rows(0)("StockLabour").ToString().Trim()
            cbStockNetRate.Text = dt.Rows(0)("StockTaxPaid").ToString().Trim()
            cbStockKaat.Text = dt.Rows(0)("StockKaat").ToString().Trim()
            ''''''Rcpt Sale
            cbRcptDate.Text = dt.Rows(0)("RcptDate").ToString().Trim()
            cbRcptSlip.Text = dt.Rows(0)("RcptSlip").ToString().Trim()
            CbRcptDisc.Text = dt.Rows(0)("RcptDisc").ToString().Trim()
            cbRcptTotal.Text = dt.Rows(0)("RcptTotal").ToString().Trim()
            cbRcptRemark.Text = dt.Rows(0)("RcptRemark").ToString().Trim()
            ''''''Pymt Sale
            cbPymtDate.Text = dt.Rows(0)("PymtDate").ToString().Trim()
            CbPymtSlip.Text = dt.Rows(0)("PymtSlip").ToString().Trim()
            CbPymtDisc.Text = dt.Rows(0)("PymtDisc").ToString().Trim()
            CbPymtTotal.Text = dt.Rows(0)("PymtTotal").ToString().Trim()
            CbPymtRemark.Text = dt.Rows(0)("PymtRemark").ToString().Trim()
            cbMargin.Text = dt.Rows(0)("Margin").ToString().Trim()
            cbLanguage.Text = dt.Rows(0)("Language").ToString().Trim()
            cbPer.Text = dt.Rows(0)("Per").ToString().Trim()
            cbFarmer.Text = dt.Rows(0)("AskFarmer").ToString().Trim()
            cbSelloutRemark.Text = dt.Rows(0)("SelloutRemark").ToString().Trim()
        End If
    End Sub
    Private Sub Save()
        clsFun.ExecNonQuery("Delete From Controls")
        Dim sql As String = "insert into Controls(Decimals, AskReciept,  AskPayment, AskMannual, AskAuto, sendDiff, CrateBardana, ChargeEffect," _
                            & " TareSameAc, DefaultDate,PurcahseRO,SaleRO,ROEachItem,AskCreditLimit,ApplyCommWeight,OCtroi," _
                            & "SpeedCommission,SpeedMandiTax,SpeedRDF,SpeedTare,SpeedLabour,SpeedTaxPaid,SpeedKaat,SuperCommission,SuperMandiTax,SuperRDF,SuperTare, " _
                            & "SuperLabour,SuperTaxPaid,SuperKaat,STDCommission,STDMandiTax,STDRDF,STDTare,STDLabour,StdTaxPaid,StdKaat,StockCommission,StockMandiTax," _
                            & "StockRDF,StockTare,StockLabour,StockTaxPaid,StockKaat,RcptDate,RcptSlip,RcptDisc,RcptTotal,RcptRemark,PymtDate,PymtSlip,PymtDisc,PymtTotal, " _
                            & "PymtRemark,SuperVehicleNo,StopBasic,AutoSwitch,OnSaleNet,Margin,Language,Per,STDMark,STDNoLot,SuperBasic,AskFarmer,SelloutRemark)" _
                            & "Select '" & cbDecimal.Text & "','" & CbRecipet.Text & "','" & cbPayment.Text & "','" & cbMannual.Text & "','" & cbAuto.Text & "', " _
                            & "'" & cbAnotherAc.Text & "','" & cbCrateBardana.Text & "','" & cbChargesEffect.Text & "','" & cbSameAc.Text & "','" & cbDateRange.Text & "', " _
                            & "'" & cbROPurchase.Text & "','" & cbROSale.Text & "','" & CbEachItems.Text & "','" & CBLimit.Text & "','" & CbCommOnWeight.Text & "','" & cbOctroi.Text & "', " _
                            & "'" & cbSpeedComm.Text & "','" & cbSpeedMandiTax.Text & "','" & cbSpeedRDF.Text & "','" & cbSpeedTare.Text & "','" & cbSpeedLabour.Text & "', " _
                            & "'" & cbSpeedNetRate.Text & "','" & cbSpeedKaat.Text & "','" & cbSuperComm.Text & "','" & cbSuperMandiTax.Text & "','" & cbSuperRDF.Text & "', " _
                            & "'" & cbSuperTare.Text & "','" & cbSuperlabour.Text & "','" & cbSuperNetRate.Text & "','" & CbSuperKaat.Text & "','" & cbStdComm.Text & "', " _
                            & "'" & cbStdMandiTax.Text & "','" & cbStdRDF.Text & "','" & cbStdTare.Text & "','" & cbStdLabour.Text & "','" & CbStdNetRate.Text & "'," _
                            & "'" & cbStdKaat.Text & "','" & cbStockComm.Text & "','" & cbStockMandiTax.Text & "','" & cbStockRDF.Text & "','" & cbStockTare.Text & "', " _
                            & "'" & cbStockLabour.Text & "','" & cbStockNetRate.Text & "','" & cbStockKaat.Text & "','" & cbRcptDate.Text & "','" & cbRcptSlip.Text & "','" & CbRcptDisc.Text & "', " _
                            & "'" & cbRcptTotal.Text & "','" & cbRcptRemark.Text & "','" & cbPymtDate.Text & "','" & CbPymtSlip.Text & "','" & CbPymtDisc.Text & "','" & CbPymtTotal.Text & "', " _
                            & "'" & CbPymtRemark.Text & "','" & CbSuperVehicle.Text & "','" & CbStopOnBasic.Text & "','" & cbAutoSwitch.Text & "','" & CbNetOnSaleRec.Text & "','" & cbMargin.Text & "', " _
                            & "'" & cbLanguage.Text & "','" & cbPer.Text & "','" & cbMarking.Text & "','" & cbNoLot.Text & "','" & cbSuperBasic.Text & "','" & cbFarmer.Text & "','" & cbSelloutRemark.Text & "'"
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                MsgBox("Setting Updated Successfully...", vbInformation + vbOKOnly, "Options Updated")
                FillDefault()
                If Application.OpenForms().OfType(Of SpeedSale).Any = True Then SpeedSale.FillSpeedSale()
                If Application.OpenForms().OfType(Of Super_Sale).Any = True Then Super_Sale.FillSuperSale()
                If Application.OpenForms().OfType(Of Standard_Sale).Any = True Then Standard_Sale.FillstdSale()
                If Application.OpenForms().OfType(Of Stock_Sale).Any = True Then Stock_Sale.FillStockSale()
                If Application.OpenForms().OfType(Of ReceiptForm).Any = True Then ReceiptForm.FillRecipt()
                If Application.OpenForms().OfType(Of PayMentform).Any = True Then PayMentform.FillPayment()
                If Application.OpenForms().OfType(Of Purchase).Any = True Then Purchase.fillPurchase()
                cbDecimal.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Save()
    End Sub

    Private Sub xcbMargin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbMargin.SelectedIndexChanged

    End Sub
End Class