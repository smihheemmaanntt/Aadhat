Public Class msgAlert
    Dim action As msgAlert.enmAction
    Dim x, y As Integer
    Public Enum enmAction
        wait
        start
        close
    End Enum

    Public Enum enmType
        Success
        Warning
        [Error]
        Info
        Delete
        Update
    End Enum


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case Me.action
            Case enmAction.wait
                Timer1.Interval = 50
                action = enmAction.close
            Case msgAlert.enmAction.start
                Me.Timer1.Interval = 1
                Me.Opacity += 0.1

                If Me.x < Me.Location.X Then
                    Me.Left -= 1
                Else

                    If Me.Opacity = 1.0 Then
                        action = msgAlert.enmAction.wait
                    End If
                End If

            Case enmAction.close
                Timer1.Interval = 1
                Me.Opacity -= 0.1
                Me.Left -= 3

                If MyBase.Opacity = 0.0 Then
                    MyBase.Close()
                End If
        End Select
    End Sub

    Private Sub pictureBox2_Click(sender As Object, e As EventArgs)
        Timer1.Interval = 1
        action = enmAction.close
    End Sub
    Public Sub showAlert(ByVal msg As String, ByVal type As enmType)
        Me.BringToFront()
        Me.Opacity = 0.0
        Me.StartPosition = FormStartPosition.Manual
        Dim fname As String

        For i As Integer = 1 To 10 - 1
            fname = "alert" & i.ToString()
            Dim frm As msgAlert = CType(Application.OpenForms(fname), msgAlert)

            If frm Is Nothing Then
                Me.Name = fname
                Me.x = Screen.PrimaryScreen.WorkingArea.Width - Me.Width + 15
                Me.y = Screen.PrimaryScreen.WorkingArea.Height - Me.Height * i - 5 * i
                Me.Location = New Point(Me.x, Me.y)
                Exit For
            End If
        Next

        Me.x = Screen.PrimaryScreen.WorkingArea.Width - MyBase.Width - 5

        Select Case type
            Case enmType.Success
                Me.PictureBox1.Image = My.Resources.success1
                Me.BackColor = Color.SeaGreen
            Case enmType.[Error]
                Me.PictureBox1.Image = My.Resources.error1
                Me.BackColor = Color.DarkRed
            Case enmType.Info
                Me.pictureBox1.Image = My.Resources.info
                Me.BackColor = Color.RoyalBlue
            Case enmType.Warning
                Me.pictureBox1.Image = My.Resources.warning
                Me.BackColor = Color.SkyBlue
            Case enmType.Update
                Me.pictureBox1.Image = My.Resources.success1
                Me.BackColor = Color.Coral
        End Select

        Me.lblMsg.Text = msg
        Me.Show()
        Me.action = enmAction.start
        Me.Timer1.Interval = 1
        Me.Timer1.Start()
    End Sub
End Class




