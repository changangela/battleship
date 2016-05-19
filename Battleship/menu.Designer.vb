<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainMenu
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainMenu))
        Me.lblName = New System.Windows.Forms.Label()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnLeaderboards = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Logo = New System.Windows.Forms.PictureBox()
        Me.Word = New System.Windows.Forms.Timer(Me.components)
        Me.btnInstructions = New System.Windows.Forms.Button()
        Me.btnAbout = New System.Windows.Forms.Button()
        CType(Me.Logo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Font = New System.Drawing.Font("Lucida Console", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.Color.Chartreuse
        Me.lblName.Location = New System.Drawing.Point(15, 206)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(185, 19)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Welcome aboard, "
        '
        'btnStart
        '
        Me.btnStart.Font = New System.Drawing.Font("Lucida Console", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStart.Location = New System.Drawing.Point(87, 89)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(182, 45)
        Me.btnStart.TabIndex = 3
        Me.btnStart.Text = "Start Game"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnSettings
        '
        Me.btnSettings.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSettings.Location = New System.Drawing.Point(87, 139)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(90, 24)
        Me.btnSettings.TabIndex = 4
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'btnLeaderboards
        '
        Me.btnLeaderboards.Font = New System.Drawing.Font("Lucida Console", 8.25!)
        Me.btnLeaderboards.Location = New System.Drawing.Point(179, 139)
        Me.btnLeaderboards.Name = "btnLeaderboards"
        Me.btnLeaderboards.Size = New System.Drawing.Size(90, 24)
        Me.btnLeaderboards.TabIndex = 5
        Me.btnLeaderboards.Text = "Leaderboards"
        Me.btnLeaderboards.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Lucida Console", 8.25!)
        Me.btnExit.Location = New System.Drawing.Point(271, 234)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(86, 23)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Logo
        '
        Me.Logo.BackColor = System.Drawing.Color.Transparent
        Me.Logo.Image = Global.Battleship.My.Resources.Resources.Logo
        Me.Logo.Location = New System.Drawing.Point(66, 12)
        Me.Logo.Name = "Logo"
        Me.Logo.Size = New System.Drawing.Size(221, 50)
        Me.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Logo.TabIndex = 8
        Me.Logo.TabStop = False
        '
        'Word
        '
        '
        'btnInstructions
        '
        Me.btnInstructions.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInstructions.Location = New System.Drawing.Point(87, 169)
        Me.btnInstructions.Name = "btnInstructions"
        Me.btnInstructions.Size = New System.Drawing.Size(90, 25)
        Me.btnInstructions.TabIndex = 9
        Me.btnInstructions.Text = "Instructions"
        Me.btnInstructions.UseVisualStyleBackColor = True
        '
        'btnAbout
        '
        Me.btnAbout.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAbout.Location = New System.Drawing.Point(179, 169)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(90, 25)
        Me.btnAbout.TabIndex = 10
        Me.btnAbout.Text = "About"
        Me.btnAbout.UseVisualStyleBackColor = True
        '
        'MainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.Battleship.My.Resources.Resources.Menubg
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(363, 269)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.btnInstructions)
        Me.Controls.Add(Me.Logo)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnLeaderboards)
        Me.Controls.Add(Me.btnSettings)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.lblName)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainMenu"
        Me.Text = "Menu"
        CType(Me.Logo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents btnSettings As System.Windows.Forms.Button
    Friend WithEvents btnLeaderboards As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Logo As System.Windows.Forms.PictureBox
    Friend WithEvents Word As System.Windows.Forms.Timer
    Friend WithEvents btnInstructions As System.Windows.Forms.Button
    Friend WithEvents btnAbout As System.Windows.Forms.Button
End Class
