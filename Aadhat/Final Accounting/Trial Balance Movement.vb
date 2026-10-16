Imports System.Data

Public Class Trial_Balance_Movement

    Public Sub New()
        InitializeComponent()
        dg1.GetType().GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).SetValue(dg1, True, Nothing)
    End Sub

    Private Sub Trial_Balance_Movement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True : Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        txtEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        dg1.Rows.Clear() : LoadGroups()
        
    End Sub

    Private Sub LoadGroups()
        cmbGroup.Items.Clear()
        cmbGroup.Items.Add("All Groups")
        Dim dt As DataTable = clsFun.ExecDataTable("Select ID, GroupName From AccountGroup Order By GroupName")
        For Each row As DataRow In dt.Rows
            cmbGroup.Items.Add(New GroupChoice(CInt(Val(row("ID").ToString())), row("GroupName").ToString()))
        Next
        cmbGroup.SelectedIndex = 0
    End Sub

    Private Class GroupChoice
        Public ReadOnly Id As Integer
        Public ReadOnly Title As String
        Public Sub New(groupId As Integer, groupName As String)
            Id = groupId
            Title = groupName
        End Sub
        Public Overrides Function ToString() As String
            Return Title
        End Function
    End Class

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        ShowBalances()
    End Sub

    Private Sub ShowBalances()
        Try
            Dim reportDate As Date = CDate(SmartDate(txtEntryDate.Text))
            txtEntryDate.Text = reportDate.ToString("dd-MM-yyyy")
            Dim dateValue As String = reportDate.ToString("yyyy-MM-dd")
            Dim groupFilter As String = ""
            If TypeOf cmbGroup.SelectedItem Is GroupChoice Then
                groupFilter = " And ac.GroupID=" & DirectCast(cmbGroup.SelectedItem, GroupChoice).Id
            End If

            Dim sql As String = "Select ac.ID, ac.AccountName, ac.Opbal, ac.DC, " &
                "ifnull((Select Round(Sum(l.Amount),2) From Ledger l Where l.AccountID=ac.ID And l.DC='D' And l.EntryDate<='" & dateValue & "'),0) As DebitTotal, " &
                "ifnull((Select Round(Sum(l.Amount),2) From Ledger l Where l.AccountID=ac.ID And l.DC='C' And l.EntryDate<='" & dateValue & "'),0) As CreditTotal " &
                "From Accounts ac Where 1=1" & groupFilter & " Order By Upper(ac.AccountName)"
            Dim dt As DataTable = clsFun.ExecDataTable(sql)
            dg1.Rows.Clear()
            lblAccountCount.Text = "Accounts: 0"
            Dim debitTotal As Decimal = 0D
            Dim creditTotal As Decimal = 0D
            Dim openingDr As Decimal = 0D
            Dim openingCr As Decimal = 0D
            Dim closingDr As Decimal = 0D
            Dim closingCr As Decimal = 0D

            For Each row As DataRow In dt.Rows
                Dim isDebit As Boolean = row("DC").ToString().Equals("Dr", StringComparison.OrdinalIgnoreCase)
                Dim openingRaw As Decimal = Convert.ToDecimal(Val(row("Opbal").ToString()))
                Dim opening As Decimal = If(isDebit, openingRaw, -openingRaw)
                Dim debit As Decimal = Convert.ToDecimal(Val(row("DebitTotal").ToString()))
                Dim credit As Decimal = Convert.ToDecimal(Val(row("CreditTotal").ToString()))
                Dim closing As Decimal = opening + debit - credit
                Dim closingSide As String = If(closing >= 0D, "Dr", "Cr")

                If opening = 0D AndAlso debit = 0D AndAlso credit = 0D Then Continue For
                Dim openingSide As String = If(isDebit, "Dr", "Cr")
                Dim index As Integer = dg1.Rows.Add(row("AccountName").ToString(), Format(Math.Abs(opening), "0.00") & " " & openingSide, Format(debit, "0.00"), Format(credit, "0.00"), Format(Math.Abs(closing), "0.00") & " " & closingSide)
                dg1.Rows(index).Tag = row("ID")
                If opening >= 0D Then openingDr += opening Else openingCr += Math.Abs(opening)
                debitTotal += debit
                creditTotal += credit
                If closing >= 0D Then closingDr += closing Else closingCr += Math.Abs(closing)
            Next

            openingDrTotal.Text = Format(openingDr, "0.00")
            openingCrTotal.Text = Format(openingCr, "0.00")
            debitTotalBox.Text = Format(debitTotal, "0.00")
            creditTotalBox.Text = Format(creditTotal, "0.00")
            closingDrTotal.Text = Format(closingDr, "0.00")
            closingCrTotal.Text = Format(closingCr, "0.00")
            lblAccountCount.Text = "Accounts: " & dg1.Rows.Count.ToString()
            dg1.ClearSelection()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Trial Balance", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If dg1.Rows.Count = 0 Then
            MessageBox.Show("Please show the trial balance before printing.", "Trial Balance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ClsFunPrimary.ExecNonQuery("Delete from Printing")
            For Each gridRow As DataGridViewRow In dg1.Rows
                InsertMovementPrintRow(txtEntryDate.Text,
                                       Convert.ToString(gridRow.Cells(0).Value),
                                       Convert.ToString(gridRow.Cells(1).Value),
                                       Convert.ToString(gridRow.Cells(2).Value),
                                       Convert.ToString(gridRow.Cells(3).Value),
                                       Convert.ToString(gridRow.Cells(4).Value),
                                       openingDrTotal.Text, openingCrTotal.Text,
                                       debitTotalBox.Text, creditTotalBox.Text,
                                       closingDrTotal.Text, closingCrTotal.Text)
            Next

            Report_Viewer.printReport("\Reports\TrialBalanceMovement.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            Report_Viewer.BringToFront()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Trial Balance Print", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub InsertMovementPrintRow(entryDate As String, accountName As String, opening As String,
                                       debit As String, credit As String, closing As String,
                                       openingDr As String, openingCr As String,
                                       debitSum As String, creditSum As String,
                                       closingDr As String, closingCr As String)
        ' Totals repeat per detail record, matching the standard Trial Balance
        ' print data pattern so the Crystal Report footer can display them.
        SaveMovementPrintValues(New List(Of String) From {
            entryDate, accountName, opening, debit, credit, closing,
            openingDr, openingCr, debitSum, creditSum, closingDr, closingCr
        })
    End Sub

    Private Sub SaveMovementPrintValues(values As List(Of String))
        Dim escaped As New List(Of String)
        For Each value As String In values
            escaped.Add("'" & value.Replace("'", "''") & "'")
        Next
        ClsFunPrimary.ExecNonQuery("Insert Into Printing(D1,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,P11) Values(" & String.Join(",", escaped.ToArray()) & ")")
    End Sub
    Private Sub Trial_Balance_Movement_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub GenericResponsiveFormScaler_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GenericResponsiveFormScaler.Bind(Me)
    End Sub
End Class
