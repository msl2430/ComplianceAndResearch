<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
		Me.components = New System.ComponentModel.Container
		Me.Label1 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.textIpAddress = New System.Windows.Forms.TextBox
		Me.textModule = New System.Windows.Forms.TextBox
		Me.textChannel = New System.Windows.Forms.TextBox
		Me.Button1 = New System.Windows.Forms.Button
		Me.textReceived = New System.Windows.Forms.TextBox
		Me.textTransmitted = New System.Windows.Forms.TextBox
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Button2 = New System.Windows.Forms.Button
		Me.Button3 = New System.Windows.Forms.Button
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(112, 28)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(82, 13)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "PAC IP Address"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(112, 51)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(82, 13)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Module # (0-15)"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(112, 75)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(99, 13)
		Me.Label3.TabIndex = 2
		Me.Label3.Text = "Serial Channel (0-1)"
		'
		'textIpAddress
		'
		Me.textIpAddress.Location = New System.Drawing.Point(243, 24)
		Me.textIpAddress.Name = "textIpAddress"
		Me.textIpAddress.Size = New System.Drawing.Size(105, 20)
		Me.textIpAddress.TabIndex = 3
		Me.textIpAddress.Text = "Enter IP Address"
		'
		'textModule
		'
		Me.textModule.Location = New System.Drawing.Point(243, 48)
		Me.textModule.Name = "textModule"
		Me.textModule.Size = New System.Drawing.Size(105, 20)
		Me.textModule.TabIndex = 4
		'
		'textChannel
		'
		Me.textChannel.Location = New System.Drawing.Point(243, 72)
		Me.textChannel.Name = "textChannel"
		Me.textChannel.Size = New System.Drawing.Size(105, 20)
		Me.textChannel.TabIndex = 5
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(167, 111)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(124, 32)
		Me.Button1.TabIndex = 6
		Me.Button1.Text = "Connect"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'textReceived
		'
		Me.textReceived.Location = New System.Drawing.Point(18, 185)
		Me.textReceived.Name = "textReceived"
		Me.textReceived.Size = New System.Drawing.Size(426, 20)
		Me.textReceived.TabIndex = 7
		'
		'textTransmitted
		'
		Me.textTransmitted.Location = New System.Drawing.Point(18, 264)
		Me.textTransmitted.Name = "textTransmitted"
		Me.textTransmitted.Size = New System.Drawing.Size(426, 20)
		Me.textTransmitted.TabIndex = 8
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(192, 169)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(79, 13)
		Me.Label4.TabIndex = 9
		Me.Label4.Text = "Data Received"
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(189, 248)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(89, 13)
		Me.Label5.TabIndex = 10
		Me.Label5.Text = "Data To Transmit"
		'
		'Button2
		'
		Me.Button2.Location = New System.Drawing.Point(167, 212)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(124, 22)
		Me.Button2.TabIndex = 11
		Me.Button2.Text = "Clear Receive Buffer"
		Me.Button2.UseVisualStyleBackColor = True
		'
		'Button3
		'
		Me.Button3.Location = New System.Drawing.Point(167, 292)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(124, 22)
		Me.Button3.TabIndex = 12
		Me.Button3.Text = "Send Transmit Buffer"
		Me.Button3.UseVisualStyleBackColor = True
		'
		'Timer1
		'
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(456, 346)
		Me.Controls.Add(Me.Button3)
		Me.Controls.Add(Me.Button2)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.textTransmitted)
		Me.Controls.Add(Me.textReceived)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.textChannel)
		Me.Controls.Add(Me.textModule)
		Me.Controls.Add(Me.textIpAddress)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Name = "Form1"
		Me.Text = "Serial Module"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents textIpAddress As System.Windows.Forms.TextBox
    Friend WithEvents textModule As System.Windows.Forms.TextBox
    Friend WithEvents textChannel As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents textReceived As System.Windows.Forms.TextBox
    Friend WithEvents textTransmitted As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
