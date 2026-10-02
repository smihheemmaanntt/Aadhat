Imports System
Imports System.Data
Imports System.Windows.Forms

Public Class DbpViewer
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub DbpViewer_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.FormBorderStyle = FormBorderStyle.None
        GenericResponsiveFormScaler.Bind(Me)
        LoadPrintingRecords()
    End Sub

    Private Sub LoadPrintingRecords()
        Try
            Dim tableInfo As DataTable = ClsFunPrimary.ExecDataTable("SELECT name FROM sqlite_master WHERE type='table' AND name='Printing'")
            If tableInfo Is Nothing OrElse tableInfo.Rows.Count = 0 Then
                dgvPrinting.DataSource = Nothing
                lblStatus.Text = "The Printing table was not found in Dbp.db."
                Return
            End If

            Dim records As DataTable = ClsFunPrimary.ExecDataTable("SELECT * FROM Printing")
            If records IsNot Nothing AndAlso records.Columns.Count > 0 Then
                'The first legacy Printing field is an unused blank column; start the view at D1.
                If records.Columns(0).ColumnName Is Nothing OrElse records.Columns(0).ColumnName.Trim().Length = 0 Then records.Columns.RemoveAt(0)
            End If
            dgvPrinting.AutoGenerateColumns = True
            dgvPrinting.DataSource = records
            If records Is Nothing Then
                lblStatus.Text = "No data could be read from the Printing table."
            Else
                lblStatus.Text = String.Format("{0:N0} record(s)  |  {1:N0} column(s)  |  Source: Dbp.db / Printing", records.Rows.Count, records.Columns.Count)
            End If
        Catch ex As Exception
            dgvPrinting.DataSource = Nothing
            lblStatus.Text = "Unable to load Printing records: " & ex.Message
        End Try
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
        LoadPrintingRecords()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DbpViewer_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            e.Handled = True
            e.SuppressKeyPress = True
            Me.Close()
        End If
    End Sub
End Class
