<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Difficulty
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
        Me.DifficultyMenu = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Dif = New System.Windows.Forms.PictureBox()
        Me.DifficultyMenu.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Dif, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DifficultyMenu
        '
        Me.DifficultyMenu.Controls.Add(Me.TabPage1)
        Me.DifficultyMenu.Controls.Add(Me.TabPage2)
        Me.DifficultyMenu.Controls.Add(Me.TabPage3)
        Me.DifficultyMenu.Controls.Add(Me.TabPage4)
        Me.DifficultyMenu.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DifficultyMenu.Location = New System.Drawing.Point(19, 61)
        Me.DifficultyMenu.Name = "DifficultyMenu"
        Me.DifficultyMenu.SelectedIndex = 0
        Me.DifficultyMenu.Size = New System.Drawing.Size(272, 178)
        Me.DifficultyMenu.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.Azure
        Me.TabPage1.Location = New System.Drawing.Point(4, 21)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(264, 153)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Easy"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.Azure
        Me.TabPage2.Location = New System.Drawing.Point(4, 21)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(264, 153)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Medium"
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.Azure
        Me.TabPage3.Location = New System.Drawing.Point(4, 21)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(264, 153)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Hard"
        '
        'TabPage4
        '
        Me.TabPage4.BackColor = System.Drawing.Color.Azure
        Me.TabPage4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage4.Location = New System.Drawing.Point(4, 21)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(264, 153)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Custom"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Battleship.My.Resources.Resources.Ocean
        Me.PictureBox1.Location = New System.Drawing.Point(-1, -3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(313, 254)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Dif
        '
        Me.Dif.BackColor = System.Drawing.Color.Transparent
        Me.Dif.Image = Global.Battleship.My.Resources.Resources.Difficulty
        Me.Dif.Location = New System.Drawing.Point(41, 19)
        Me.Dif.Name = "Dif"
        Me.Dif.Size = New System.Drawing.Size(206, 29)
        Me.Dif.TabIndex = 6
        Me.Dif.TabStop = False
        '
        'Difficulty
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.Battleship.My.Resources.Resources.Ocean
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(311, 251)
        Me.Controls.Add(Me.DifficultyMenu)
        Me.Controls.Add(Me.Dif)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "Difficulty"
        Me.Text = "Difficulty"
        Me.DifficultyMenu.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Dif, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DifficultyMenu As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents lblGrids As System.Windows.Forms.Label
    Friend WithEvents Dif As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
