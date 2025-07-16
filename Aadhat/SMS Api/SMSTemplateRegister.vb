Public Class SMSTemplateRegister

    Private Sub SMSTemplateRegister_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub SMSTemplateRegister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.GhostWhite
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 4
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        Dim checkBoxColumn As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = "" : checkBoxColumn.Width = 30
        checkBoxColumn.Name = "checkBoxColumn" : dg1.Columns.Insert(1, checkBoxColumn)
        dg1.Columns(2).Name = "Send SMS On" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Customer" : dg1.Columns(3).Width = 400
        ' dg1.Rows.Add(1, dg1.Rows(0).Cells(1).Value = False, "Account Master", "")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SMSAPI.MdiParent = MainScreenForm
        SMSAPI.Show()
        If Not SMSAPI Is Nothing Then
            SMSAPI.Show()
        End If
    End Sub
End Class