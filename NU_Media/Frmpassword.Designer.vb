<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frmpassword
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnapply = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtpassword = New System.Windows.Forms.TextBox()
        Me.RadUser = New System.Windows.Forms.RadioButton()
        Me.Radadmin = New System.Windows.Forms.RadioButton()
        Me.TxtNewPassword = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtNewPasswordAgain = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnapply
        '
        Me.btnapply.Location = New System.Drawing.Point(79, 73)
        Me.btnapply.Name = "btnapply"
        Me.btnapply.Size = New System.Drawing.Size(62, 24)
        Me.btnapply.TabIndex = 0
        Me.btnapply.Text = "Apply"
        Me.btnapply.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(182, 73)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(62, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtpassword
        '
        Me.txtpassword.Location = New System.Drawing.Point(205, 30)
        Me.txtpassword.Name = "txtpassword"
        Me.txtpassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtpassword.Size = New System.Drawing.Size(167, 20)
        Me.txtpassword.TabIndex = 3
        '
        'RadUser
        '
        Me.RadUser.AutoSize = True
        Me.RadUser.Checked = True
        Me.RadUser.Location = New System.Drawing.Point(13, 10)
        Me.RadUser.Name = "RadUser"
        Me.RadUser.Size = New System.Drawing.Size(47, 17)
        Me.RadUser.TabIndex = 1
        Me.RadUser.TabStop = True
        Me.RadUser.Text = "User"
        Me.RadUser.UseVisualStyleBackColor = True
        '
        'Radadmin
        '
        Me.Radadmin.AutoSize = True
        Me.Radadmin.Location = New System.Drawing.Point(13, 33)
        Me.Radadmin.Name = "Radadmin"
        Me.Radadmin.Size = New System.Drawing.Size(186, 17)
        Me.Radadmin.TabIndex = 2
        Me.Radadmin.Text = "Administrator (Password Required)"
        Me.Radadmin.UseVisualStyleBackColor = True
        '
        'TxtNewPassword
        '
        Me.TxtNewPassword.Location = New System.Drawing.Point(539, 29)
        Me.TxtNewPassword.Name = "TxtNewPassword"
        Me.TxtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxtNewPassword.Size = New System.Drawing.Size(167, 20)
        Me.TxtNewPassword.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(394, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Enter New Password: "
        '
        'TxtNewPasswordAgain
        '
        Me.TxtNewPasswordAgain.Location = New System.Drawing.Point(539, 55)
        Me.TxtNewPasswordAgain.Name = "TxtNewPasswordAgain"
        Me.TxtNewPasswordAgain.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxtNewPasswordAgain.Size = New System.Drawing.Size(167, 20)
        Me.TxtNewPasswordAgain.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(394, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(142, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Enter New Password Again: "
        '
        'Frmpassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(711, 104)
        Me.Controls.Add(Me.TxtNewPasswordAgain)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtNewPassword)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Radadmin)
        Me.Controls.Add(Me.RadUser)
        Me.Controls.Add(Me.txtpassword)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnapply)
        Me.Name = "Frmpassword"
        Me.Text = "Password Check"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnapply As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtpassword As System.Windows.Forms.TextBox
    Friend WithEvents RadUser As System.Windows.Forms.RadioButton
    Friend WithEvents Radadmin As System.Windows.Forms.RadioButton
    Friend WithEvents TxtNewPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtNewPasswordAgain As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
