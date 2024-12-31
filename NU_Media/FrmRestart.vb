Public Class FrmRestart
    Dim timercount As Integer
    Private Sub ToolStripProgressBar3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FrmRestart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tmrrestart.Enabled = True
    End Sub

    Private Sub tmrrestart_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrrestart.Tick
        If timercount >= 30 Then
            Me.Close()
        End If
        timercount += 1

        If ProgressBar1.Value <= 90 Then
            ProgressBar1.Value = ProgressBar1.Value + 5
        End If
    End Sub
End Class