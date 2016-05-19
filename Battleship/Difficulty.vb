Public Class Difficulty
    Public GRID_SIZE(4) As Integer
    Public NUM_CPU_BOATS(4) As Integer
    Public NUM_PLY_BOATS(4) As Integer
    Public Level As Integer
    Private Sub Difficulty_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'initiate form through subroutine 
        AssignVariables()
        InitTabControls()
        CustomTab()
    End Sub
    Private Sub InitTabControls()
        'make labels and make buttons
        makeLabels(lblYourBoats(EASY), POSITION_X, POSITION_Y, EASY, PLY)
        makeLabels(lblYourBoats(MEDIUM), POSITION_X, POSITION_Y, MEDIUM, PLY)
        makeLabels(lblYourBoats(HARD), POSITION_X, POSITION_Y, HARD, PLY)
        makeLabels(lblYourBoats(CUSTOM), POSITION_X, POSITION_Y, CUSTOM, PLY)

        makeLabels(lblCpuBoats(EASY), OPPONENTX, POSITION_Y, EASY, CPU)
        makeLabels(lblCpuBoats(MEDIUM), OPPONENTX, POSITION_Y, MEDIUM, CPU)
        makeLabels(lblCpuBoats(HARD), OPPONENTX, POSITION_Y, HARD, CPU)
        makeLabels(lblCpuBoats(CUSTOM), OPPONENTX, POSITION_Y, CUSTOM, CPU)

        makeLabels(lblGrid(EASY), GRIDX, POSITION_Y, EASY, 0)
        makeLabels(lblGrid(MEDIUM), GRIDX, POSITION_Y, MEDIUM, 0)
        makeLabels(lblGrid(HARD), GRIDX, POSITION_Y, HARD, 0)
        makeLabels(lblGrid(CUSTOM), GRIDX, POSITION_Y, CUSTOM, 0)
        For x = EASY To CUSTOM
            makeButtons(btnStart(x), x)
        Next
    End Sub
    Private lblYourBoats(4) As Label
    Private lblCpuBoats(4) As Label
    Private lblGrid(4) As Label

    Private Const EASY As Integer = 1
    Private Const MEDIUM As Integer = 2
    Private Const HARD As Integer = 3
    Private Const CUSTOM As Integer = 4

    Public BoatLengths(2, 4, 5) As Integer
    Public Const CPU = 1
    Public Const PLY = 2

    Private Const POSITION_X As Integer = 25
    Private Const POSITION_Y As Integer = 20
    Const OPPONENTX As Integer = POSITION_X + 65
    Const GRIDX As Integer = OPPONENTX + 100

    Private btnStart(4) As Button
    Private Sub makeButtons(ByRef Button As Button, ByVal Level As Integer)
        'makes buttons
        Button = New Button
        Button.Width = 90
        Button.Height = 30
        Button.Location = New Point(80, 120)
        Button.Text = "Start Game"
        DifficultyMenu.TabPages(Level - 1).Controls.Add(Button)
        AddHandler Button.Click, AddressOf btnStart_Clicked
    End Sub
    Private Sub btnStart_Clicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'starts the game upon click
        For x = EASY To CUSTOM
            If sender Is btnStart(x) Then
                Level = x
            End If
        Next
        Battleship.Show()
        Me.Close()
        PlaySound("ClickButton.mp3")
    End Sub
    Private Sub makeLabels(ByRef Label As Label, ByVal PositionX As Integer, ByVal positionY As Integer, ByVal Level As Integer, ByVal Subject As Integer)
        'makes labels on the difficulties tabcontrol
        Label = New Label
        Label.Location = New Point(PositionX, positionY)
        If Subject = PLY Then
            Label.Text = "Your boats:"
            Label.Width = OPPONENTX - POSITION_X
        ElseIf Subject = CPU Then
            Label.Text = "Opponent's boats:"
            Label.Width = GRIDX - OPPONENTX
        Else
            Label.Text = "Grid:" & vbCrLf & GRID_X(Level) & " X " & GRID_Y(Level)
            Label.Width = 65
        End If

        If Level = CUSTOM Then
            Label.Height = 15
        Else
            Label.Height = 80
        End If
        Label.Font = New Font("Times New Roman", 9)
        If (Subject = PLY Or Subject = CPU) And (Level = EASY Or Level = MEDIUM Or Level = HARD) Then
            Dim intCounter As Integer = 1
            While BoatLengths(Subject, Level, intCounter) <> 0
                Label.Text = Label.Text & vbCrLf & "1 X " & BoatLengths(Subject, Level, intCounter)
                intCounter = intCounter + 1
            End While
        End If
        DifficultyMenu.TabPages(Level - 1).Controls.Add(Label)
    End Sub
    Public GRID_X(4) As Integer
    Public GRID_Y(4) As Integer
    Private Sub AssignVariables()
        'assign the different boat lengths for the different difficulties
        BoatLengths(PLY, EASY, 1) = 2
        BoatLengths(PLY, EASY, 2) = 3
        BoatLengths(PLY, EASY, 3) = 4

        NUM_PLY_BOATS(EASY) = 3

        BoatLengths(PLY, MEDIUM, 1) = 2
        BoatLengths(PLY, MEDIUM, 2) = 2
        BoatLengths(PLY, MEDIUM, 3) = 3
        BoatLengths(PLY, MEDIUM, 4) = 4

        NUM_PLY_BOATS(MEDIUM) = 4

        BoatLengths(PLY, HARD, 1) = 2
        BoatLengths(PLY, HARD, 2) = 2
        BoatLengths(PLY, HARD, 3) = 3
        BoatLengths(PLY, HARD, 4) = 4

        NUM_PLY_BOATS(HARD) = 4

        BoatLengths(PLY, CUSTOM, 1) = 1

        NUM_PLY_BOATS(CUSTOM) = 1

        BoatLengths(CPU, EASY, 1) = 2
        BoatLengths(CPU, EASY, 2) = 3
        BoatLengths(CPU, EASY, 3) = 4

        NUM_CPU_BOATS(EASY) = 3

        BoatLengths(CPU, MEDIUM, 1) = 2
        BoatLengths(CPU, MEDIUM, 2) = 2
        BoatLengths(CPU, MEDIUM, 3) = 3
        BoatLengths(CPU, MEDIUM, 4) = 4

        NUM_CPU_BOATS(MEDIUM) = 3

        BoatLengths(CPU, HARD, 1) = 1
        BoatLengths(CPU, HARD, 2) = 1
        BoatLengths(CPU, HARD, 3) = 2
        BoatLengths(CPU, HARD, 4) = 3

        NUM_CPU_BOATS(HARD) = 3

        BoatLengths(CPU, CUSTOM, 1) = 1

        NUM_CPU_BOATS(CUSTOM) = 1

        GRID_X(EASY) = 8
        GRID_Y(EASY) = 8

        GRID_X(MEDIUM) = 10
        GRID_Y(MEDIUM) = 10

        GRID_X(HARD) = 15
        GRID_Y(HARD) = 15

        GRID_X(CUSTOM) = 5
        GRID_Y(CUSTOM) = 5

        GRID_SIZE(1) = 40
        GRID_SIZE(2) = 30
        GRID_SIZE(3) = 25
        GRID_SIZE(4) = 40

    End Sub
    Private pnlContainer(2) As Panel
    Private Sub CustomTab()
        'initiates custom form controls
        addCustom(PLY)
        addCustom(CPU)
        makeReset()
        CustomGrid()
    End Sub
    Private Sub addCustom(ByVal Subject As Integer)
        'dynamically generate the buttons and panels on the custom tab
        If Subject = PLY Then
            makePanel(pnlContainer(PLY), POSITION_X - 10, -40)
        Else
            makePanel(pnlContainer(CPU), OPPONENTX + 10, -40)
        End If
        makeControls(lbl1x(Subject), Subject)
        makeControls(numUpDown(Subject), Subject)
        makeAdd(btnAdd(Subject), Subject)
        makeBoatLabels(Subject)
    End Sub
    Private lbl1x(2) As Label
    Private numUpDown(2) As NumericUpDown
    Private btnAdd(2) As Button
    Private Labels(2, 5) As Label
    Private Counter(2) As Integer
    Private Sub makeControls(ByRef Control As Object, ByVal Subject As Integer)
        'dynamically create controls
        If Control Is lbl1x(Subject) Then
            Control = New Label
            Control.Text = "1 x"
            Control.width = 25
            Control.Location = New Point(0, 80)
        ElseIf Control Is numUpDown(Subject) Then
            Control = New NumericUpDown
            Control.Minimum = 1
            Control.Maximum = 5
            Control.width = 30
            Control.location = New Point(25, 78)
        End If
        pnlContainer(Subject).Controls.Add(Control)
    End Sub
    Private Sub makeBoatLabels(ByVal subject)
        'dynamically create labels with appropriate argument settings
        Dim PositionX, positionY As Integer
        PositionX = 0
        positionY = 65
        For x = 1 To 5
            Labels(subject, x) = New Label
            Labels(subject, x).Location = New Point(PositionX, positionY)
            Labels(subject, x).Text = "1 x "
            Labels(subject, x).Height = 15
            pnlContainer(subject).Controls.Add(Labels(subject, x))
            positionY = positionY - 15
            Labels(subject, x).Visible = False
        Next
    End Sub
    Private Sub makeAdd(ByRef Control As Button, ByVal Subject As Integer)
        'dynamically create button with appropriate argument settings
        Control = New Button
        Control.Width = 20
        Control.Height = 20
        Control.Text = "+"
        Control.Location = New Point(55, 78)
        pnlContainer(Subject).Controls.Add(Control)
        AddHandler Control.Click, AddressOf btnAdd_clicked
    End Sub
    Private Sub btnAdd_clicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'upon click, a new row of labels will appear
        If sender Is btnAdd(PLY) Then
            addLabels(PLY)
        Else
            addLabels(CPU)
        End If
        PlaySound("ButtonAdd.mp3")
    End Sub
    Private Sub addLabels(ByVal Subject As Integer)
        'subroutine to allow the appropriate row of labels to appear given the correct subject argument
        pnlContainer(Subject).Location = New Point(pnlContainer(Subject).Location.X, pnlContainer(Subject).Location.Y + 15)
        Counter(Subject) = Counter(Subject) + 1
        BoatLengths(Subject, CUSTOM, Counter(Subject)) = numUpDown(Subject).Text
        Labels(Subject, Counter(Subject)).Visible = True
        Labels(Subject, Counter(Subject)).Text = Labels(Subject, Counter(Subject)).Text & numUpDown(Subject).Text
        numUpDown(Subject).Text = 1
        If Counter(Subject) = 5 Then
            btnAdd(Subject).Visible = False
            numUpDown(Subject).Visible = False
            lbl1x(Subject).Visible = False
        End If

        If Subject = CPU Then
            NUM_CPU_BOATS(CUSTOM) = Counter(Subject)
        Else
            NUM_PLY_BOATS(CUSTOM) = Counter(Subject)
        End If
    End Sub
    Private Sub makePanel(ByRef Panel As Panel, ByVal PositionX As Integer, ByVal PositionY As Integer)
        'dynamically create panel to hold controls on the custom difficulty tab
        Panel = New Panel
        Panel.Location = New Point(PositionX, PositionY)
        Panel.Height = 120
        Panel.Width = 90
        DifficultyMenu.TabPages(3).Controls.Add(Panel)
    End Sub
    Private btnReset As Button
    Private Sub makeReset()
        'creates a reset button on the custom tab
        btnReset = New Button
        btnReset.Location = New Point(10, 120)
        btnReset.Text = "Reset"
        btnReset.Height = 30
        btnReset.Width = 60
        DifficultyMenu.TabPages(3).Controls.Add(btnReset)
        btnReset.BringToFront()
        AddHandler btnReset.Click, AddressOf btnReset_clicked
    End Sub
    Private Sub btnReset_clicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'resets the custom pick difficulty page
        Reset(PLY)
        Reset(CPU)
        btnSetGrid.Enabled = True
        NUM_CPU_BOATS(CUSTOM) = 1
        NUM_PLY_BOATS(CUSTOM) = 1
        PlaySound("ClickButton.mp3")
    End Sub
    Private Sub Reset(ByVal Subject)
        'subroutine that resets form 
        DifficultyMenu.TabPages(3).Controls.Remove(pnlContainer(Subject))
        Counter(Subject) = 0
        addCustom(Subject)
        gridUpDown(Subject).Visible = True
        gridUpDown(Subject).BringToFront()
        DifficultyMenu.TabPages(3).Controls.Remove(lblSetGrids(Subject))
    End Sub
    Private Sub CustomGrid()
        'dynamically creates controls on the custom pick difficulty page
        makeGridUpDown()
        makeX()
        makeSet()
    End Sub
    Private gridUpDown(2) As NumericUpDown
    Private Sub makeGridUpDown()
        'subroutine to make 2 numeric up downs on the custom pick difficulty page
        gridUpDown(1) = New NumericUpDown
        gridUpDown(1).Minimum = 5
        gridUpDown(1).Maximum = 20
        gridUpDown(1).Width = 30
        gridUpDown(1).Location = New Point(GRIDX, POSITION_Y + 20)
        DifficultyMenu.TabPages(3).Controls.Add(gridUpDown(1))

        gridUpDown(2) = New NumericUpDown
        gridUpDown(2).Minimum = 5
        gridUpDown(2).Maximum = 20
        gridUpDown(2).Width = 30
        gridUpDown(2).Location = New Point(GRIDX, POSITION_Y + 60)
        DifficultyMenu.TabPages(3).Controls.Add(gridUpDown(2))
    End Sub
    Private lblX As Label
    Private Sub makeX()
        'makes a label that says "x"
        lblX = New Label
        lblX.Text = "x"
        lblX.Height = 15
        lblX.Location = New Point(GRIDX, POSITION_Y + 40)
        DifficultyMenu.TabPages(3).Controls.Add(lblX)
    End Sub
    Private btnSetGrid As Button
    Private Sub makeSet()
        'makes a button that sets the users grid selection
        btnSetGrid = New Button
        btnSetGrid.Text = "Set"
        btnSetGrid.Location = New Point(GRIDX, POSITION_Y + 85)
        btnSetGrid.Width = 35
        btnSetGrid.Height = 25
        DifficultyMenu.TabPages(3).Controls.Add(btnSetGrid)
        AddHandler btnSetGrid.Click, AddressOf btnSetGrid_clicked
    End Sub
    Private Sub btnSetGrid_clicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'upon click, secures player's grid size selection
        GRID_Y(4) = gridUpDown(1).Text
        GRID_X(4) = gridUpDown(2).Text
        makelblGrid()
        gridUpDown(1).Visible = False
        gridUpDown(2).Visible = False
        sender.enabled = False
        PlaySound("ClickButton.mp3")
    End Sub
    Private lblSetGrids(2) As Label
    Private Sub makelblGrid()
        'makes a lable for the grid
        For x = 1 To 2
            lblSetGrids(x) = New Label
            lblSetGrids(x).Location = gridUpDown(x).Location
            lblSetGrids(x).Width = gridUpDown(x).Width
            lblSetGrids(x).Height = gridUpDown(x).Height
            lblSetGrids(x).Text = gridUpDown(x).Text
            lblSetGrids(x).BringToFront()
            DifficultyMenu.TabPages(3).Controls.Add(lblSetGrids(x))
        Next
    End Sub
    Public Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer
    Private Sub PlaySound(ByVal musicPath As String)
        'subroutine to open, play, and close a sound effect
        If MainMenu.blnSFX = True Then
            Dim musicAlias As String = "myAudio"
            mciSendString("close " & musicAlias, CStr(0), 0, 0)
            mciSendString("Open " & Chr(34) & musicPath & Chr(34) & " alias " & musicAlias, CStr(0), 0, 0)
            mciSendString("play " & musicAlias, CStr(0), 0, 0)
        End If
    End Sub
End Class