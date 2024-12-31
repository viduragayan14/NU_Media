Public Class Frmpassword

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Frmpassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click

    End Sub




    Private Sub Frmpassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtpassword.Text = "ooooo"
        txtpassword.SelectionStart = 0
        txtpassword.SelectionLength = Len(txtpassword.Text)
        txtpassword.SelectAll()

        txtpassword.Focus()


    End Sub



    Private Sub txtpassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtpassword.KeyPress
        Dim tmp As System.Windows.Forms.KeyPressEventArgs = e
        If tmp.KeyChar = ChrW(Keys.Enter) Then
            If txtpassword.Text = "" Then
                NU_Media.Size = New Size(1100, NU_Media.Size.Height)
                NU_Media.btnExtendHide.Text = "Hide Extended"
                Me.Close()
            Else
                MsgBox("Wrong Password", MessageBoxIcon.Error, "NU_Media")
            End If
        End If
    End Sub



    Private Sub txtpassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtpassword.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnapply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnapply.Click
        If txtpassword.Text = "" Then
            NU_Media.Size = New Size(1100, NU_Media.Size.Height)
            NU_Media.btnExtendHide.Text = "Hide Extended"
            Me.Close()
        Else
            MsgBox("Wrong Password", MessageBoxIcon.Error, "NU_Media")
        End If
    End Sub
End Class