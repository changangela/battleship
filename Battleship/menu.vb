'Wendy and Angela April 21st, 2014
'Battleship
'ICS2O
'Ms. Kutsche
'A fun and interactive game of battleship for people of all ages.
Public Class MainMenu
    Public plyname As String
    Private strName As String
    Public blnSFX As Boolean = True
    Public blnBGM As Boolean = True
    Public Settings As New Form

    Private stgLblSettings, stgLblSounds, lblBGM, lblSFX As Label
    Private stgBtnChangeName, stgBtnDone, stgReset As Button
    Private picSFX, picBGM As PictureBox
    Private stgTextbox As TextBox

    Private Const LABEL As Integer = 1
    Private Const BUTTON As Integer = 2
    Private Const TEXTBOX As Integer = 3
    Private Const PICBOX As Integer = 4
    Private Const STG_FORM As Integer = 1
    Private Const DIF_FORM As Integer = 2

    Public blConfirm As Boolean
  
    Private Sub Menu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Visible = False

        Dim s = New SplashScreen1()
        s.Show()
        s.lblTitle.ForeColor = Color.White
        s.Refresh()
        System.Threading.Thread.Sleep(5000)
        s.Close()
        Me.Visible = True

        plyname = StrConv(InputBox("Ahoy there mate! What's yer name?"), vbProperCase)
        'set restrictions to player's name
        If plyname = "" Then
            plyname = "Player001"
        End If
        strName = "Welcome aboard, " & plyname & "..."
        lblName.Text = strName
        Word.Start()
        'start background music
        If blnBGM = True Then
            My.Computer.Audio.Play(My.Resources.Pirates_Hook, AudioPlayMode.BackgroundLoop)
        End If
        Settings.Width = 230
        Settings.Height = 300
        'dynamically create controls using a function 
        dynAddControls(Settings, stgLblSettings, "Settings", 81, 20, 45, 13, LABEL)
        dynAddControls(Settings, stgLblSounds, "Sounds:", 40, 140, 46, 13, LABEL)
        dynAddControls(Settings, stgBtnChangeName, "Change Name", 55, 70, 100, 25, BUTTON)
        dynAddControls(Settings, picBGM, "", 90, 130, 35, 35, PICBOX)
        dynAddControls(Settings, picSFX, "", 130, 130, 35, 35, PICBOX)
        dynAddControls(Settings, stgBtnDone, "Done", 145, 226, 55, 25, BUTTON)
        dynAddControls(Settings, stgReset, "Reset Scores", 45, 180, 122, 23, BUTTON)
        dynAddControls(Settings, stgTextbox, "", 55, 45, 100, 25, TEXTBOX)
        dynAddControls(Settings, lblBGM, "BGM", 90, 115, 35, 20, LABEL)
        dynAddControls(Settings, lblBGM, "SFX", 130, 115, 35, 20, LABEL)

        'set color to form controls
        For Each ctrl In Settings.Controls
            If TypeOf ctrl Is Label Then
                ctrl.backcolor = Color.Transparent
                ctrl.forecolor = Color.White
            ElseIf TypeOf ctrl Is Button Then
                ctrl.backcolor = Color.Transparent
            ElseIf TypeOf ctrl Is PictureBox Then
                ctrl.backcolor = Color.Transparent
            End If
        Next
        stgTextbox.Text = plyname
    End Sub
    Sub btnSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettings.Click
        'subroutine for settings button
        Settings.Show()
        Settings.BackgroundImage = My.Resources.Settingsbg
        Settings.BackgroundImageLayout = ImageLayout.Stretch
        Settings.ControlBox = False
        PlaySound()

        'toggle ON/OFF sound effects and background music
        If blnSFX = True Then
            picSFX.Image = My.Resources.sfxON
        Else
            picSFX.Image = My.Resources.sfxOFF
        End If
        picSFX.SizeMode = PictureBoxSizeMode.StretchImage

        If blnBGM = True Then
            picBGM.Image = My.Resources.bgmON
        Else
            picBGM.Image = My.Resources.bgmOFF
        End If
        picBGM.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub
    Sub dynAddControls(ByRef targetform As Form, ByRef inctrl As Control, ByVal text As String, ByVal position_X As Integer, ByVal position_Y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal typeofctrl As Integer)
        'dynamcally add controls 
        Dim ptposition As Point
        If typeofctrl = LABEL Then
            inctrl = New Label
        ElseIf typeofctrl = BUTTON Then
            inctrl = New Button
        ElseIf typeofctrl = TEXTBOX Then
            inctrl = New TextBox
        ElseIf typeofctrl = PICBOX Then
            inctrl = New PictureBox
        End If

        inctrl.Text = text
        ptposition.X = position_X
        ptposition.Y = position_Y
        inctrl.Width = width
        inctrl.Height = height

        inctrl.Location = ptposition
        'add controls to form
        targetform.Controls.Add(inctrl)
        'add handler
        AddHandler inctrl.Click, AddressOf ctrlsClicked

    End Sub
    Private Sub ctrlsClicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'subroutine that handles all controls within the dynamically created settings menu
        If sender Is stgBtnChangeName Then
            'handles stgBtnChangeName
            plyname = stgTextbox.Text
            strName = "Welcome aboard, " & plyname & "..."
            lblName.Text = strName
            MsgBox("Changes saved")
        ElseIf sender Is stgBtnDone Then
            'handles stgBtnDone
            Settings.Hide()
        ElseIf sender Is stgReset Then
            'handles stgReset
            MsgBox("Ye scores be cleared matey")
            If My.Computer.FileSystem.FileExists("Name.txt") = True Then
                My.Computer.FileSystem.DeleteFile("Name.txt")
                My.Computer.FileSystem.DeleteFile("Score.txt")
            End If
        ElseIf sender Is picSFX Then
            'handles picSFX
            If blnSFX = True Then
                blnSFX = False
                picSFX.Image = My.Resources.sfxOFF
            Else
                blnSFX = True
                picSFX.Image = My.Resources.sfxON
            End If
        ElseIf sender Is picBGM Then
            'handles picBGM
            If blnBGM = True Then
                blnBGM = False
                picBGM.Image = My.Resources.bgmOFF
                My.Computer.Audio.Stop()
            Else
                blnBGM = True
                picBGM.Image = My.Resources.bgmON
                My.Computer.Audio.Play(My.Resources.Pirates_Hook, AudioPlayMode.BackgroundLoop)
            End If

        End If
        PlaySound()
    End Sub
    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        'start button starts game upon click
        Difficulty.Show()
        Me.Hide()
        LeaderBoards.Close()
        Settings.Hide()
        PlaySound()
    End Sub
    Private Sub PlaySound()
        'plays sound effects
        If blnSFX = True Then
            Dim musicAlias As String = "myAudio"
            Dim musicPath As String = "ClickButton.mp3"
            mciSendString("close " & musicAlias, CStr(0), 0, 0)
            mciSendString("Open " & Chr(34) & musicPath & Chr(34) & " alias " & musicAlias, CStr(0), 0, 0)
            mciSendString("play " & musicAlias, CStr(0), 0, 0)
        End If
    End Sub
    Public Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'exits game
        PlaySound()
        Me.Close()
    End Sub
    Private Sub btnLeaderboards_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeaderboards.Click
        'opens leaderboard form
        PlaySound()
        LeaderBoards.Show()
    End Sub
    Private intCounter As Integer
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Word.Tick
        'animated typewriting welcome message
        intCounter = intCounter + 1
        lblName.Text = Mid(strName, 1, intCounter)
        If intCounter > Len(strName) Then
            intCounter = intCounter - 5
        End If
    End Sub
    Private Sub btnInstructions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInstructions.Click
        'open instructions
        Instructions.Show()
    End Sub
    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        'open about form
        AboutBattleship.Show()
    End Sub
End Class