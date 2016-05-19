Imports System.IO
Public Class Battleship
    Public GRID_X As Integer = Difficulty.GRID_X(Difficulty.Level)
    Public GRID_Y As Integer = Difficulty.GRID_Y(Difficulty.Level)

    Public GRID_SIZE As Integer = Difficulty.GRID_SIZE(Difficulty.Level)
    Public POSITION_X As Integer = GRID_SIZE

    Public Const POSITION_Y As Integer = 0

    Private Const MINI_SIZE As Integer = 20

    'upon changing the following 2 variables, the values in the "AssignBoatLengths" function must also be changed there is no error returned
    Public NUM_CPU_BOATS As Integer = Difficulty.NUM_PLY_BOATS(Difficulty.Level)
    Public NUM_PLY_BOATS As Integer = Difficulty.NUM_CPU_BOATS(Difficulty.Level)

    Private cpuElement(GRID_X + 1, GRID_Y + 1, 4), plyElement(GRID_X + 1, GRID_Y + 1, 4) As Boolean
    Private Const ISBOAT As Integer = 1
    Private Const CPUCHOSE As Integer = 2
    Private Const CANBOAT As Integer = 3
    Private Const RESTRICTED As Integer = 4
    Private Const PLYCHOSE As Integer = 2
    Private Const ISHIT As Integer = 3

    Private cpuBoats(NUM_CPU_BOATS, 8, 2), plyBoats(NUM_PLY_BOATS, 8, 2) As Integer
    Private Const BOAT_X As Integer = 1
    Private Const BOAT_Y As Integer = 2

    Private cpuBoatSank As Integer
    Private cpuBoatNumber As Integer = 1
    Private cpuBoatPart As Integer = 1
    Private cpuBoatLength(NUM_CPU_BOATS) As Integer
    Private cpuHit As Boolean = False

    Private plyBoatSank As Integer
    Private plyBoatLength(NUM_PLY_BOATS) As Integer

    Private plyButtons(GRID_X, GRID_Y), cpuButtons(GRID_X, GRID_Y) As Button
    Private plyGrids(GRID_X, GRID_Y), cpuGrids(GRID_X, GRID_Y) As PictureBox

    Private cpuReset, cpuBack, cpuNext, plyReset, btnQuit, btnMainMenu, btnSettings, btnInstructions As Button
    Private lblAxisX(GRID_X), lblAxisY(GRID_Y) As Label
    Private lblScore, lblIndicate As Label

    Private picWin, cpuPicBoat(NUM_CPU_BOATS), plyPicBoat(NUM_PLY_BOATS) As PictureBox
    Private btnselected As Button

    Public Score As Integer
    Private HIT As Integer = 50
    Private SANK As Integer = 100
    Private BONUS As Integer = 20
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'initiates form controls, sets form size and dynamically creates timers and main controls
        Me.AutoSize = True
        Randomize()
        makeTimers()
        colorchange.Start()
        makeLabels()
        lblScore.Visible = False
        cpuPlay()
        assignBoatLengths()
        dynCreateButton(btnQuit, POSITION_X - GRID_SIZE, POSITION_Y + (GRID_X + 4) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Quit")
        dynCreateButton(btnMainMenu, POSITION_X - GRID_SIZE, POSITION_Y + (GRID_X + 3) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Main Menu")
        dynCreateButton(btnInstructions, POSITION_X - GRID_SIZE, POSITION_Y + (GRID_X + 2) * GRID_SIZE, 3 * GRID_SIZE, GRID_SIZE, "Instructions")
    End Sub
    Private Sub plyPlay()
        'subroutine that initiates the actions upon the start of the player's interactive side of the game
        makeButtons(plyButtons)
        makeGrids(plyGrids)
        dynCreateButton(plyReset, POSITION_X + (GRID_Y + 1) * GRID_SIZE, POSITION_Y + (GRID_X + 4) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Reset")
        plyPlaceBoats()
    End Sub
    Private Sub cpuPlay()
        '  'subroutine that initiates the actions upon the start of the computer's interactive side of the game
        makeButtons(cpuButtons)
        makeGrids(cpuGrids)
        dynCreateButton(cpuReset, POSITION_X + (GRID_Y + 1) * GRID_SIZE, POSITION_Y + (GRID_X + 4) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Reset")
        dynCreateButton(cpuBack, POSITION_X - GRID_SIZE, POSITION_Y + (GRID_X + 1) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Undo")
        dynCreateButton(cpuNext, POSITION_X + (GRID_Y + 1) * GRID_SIZE, POSITION_Y + (GRID_X + 1) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Next")
        cpuBack.Visible = False
        cpuNext.Visible = False

    End Sub
    Private Sub plyButtons_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'handles all the actions on the dynamically created buttons in the player''s interactive grid
        Dim intx, inty As Integer
        btnselected = sender
        buttonshrink.Start() 'starts short animation that shrinks the clicked button
        'sets all the virtual elements of the player's "PLYCHOSE" to true for ease of maintenance
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                If plyButtons(x, y) Is sender Then
                    intx = x
                    inty = y
                    plyElement(x, y, PLYCHOSE) = True
                End If
            Next
        Next
        'upon player is hit, change grid background picture
        If plyIsHit(intx, inty) = True Then
            plyGrids(intx, inty).Image = My.Resources.Flame 'changes image into a little flame:)
            plyGrids(intx, inty).SizeMode = PictureBoxSizeMode.StretchImage
            If plyShipSank(intx, inty) = True Then
                plyBoatSink(intx, inty)
                plyBoatSank = plyBoatSank + 1
                If plyBoatSank = NUM_PLY_BOATS Then
                    'Player has sank all boats
                    CountScore(Score, SANK, lblScore) 'adds points onto the player's score 
                    lblIndicate.Text = "SANK!"
                    xTEMP = 1
                    yTEMP = 1
                    lblIndicate.Text = "Bonus!"
                    'starts the bonus points sequence
                    bonusTimer.Start()
                    'if the background music option is on, play victory song
                    If MainMenu.blnBGM = True Then
                        My.Computer.Audio.Play(My.Resources.BlissfulVictory, AudioPlayMode.BackgroundLoop)
                    End If
                    For x = 1 To GRID_X
                        For y = 1 To GRID_Y
                            plyButtons(x, y).Enabled = False
                        Next
                    Next
                    Exit Sub
                Else
                    'Player has sank a boat
                    CountScore(Score, SANK, lblScore)
                    lblIndicate.Text = "SANK!"
                    PlaySound("myAudio", "BombFalling.mp3")
                End If
            Else
                'Player has hit a boat
                CountScore(Score, HIT, lblScore)
                lblIndicate.Text = "HIT!"

                PlaySound("myAudio", "Explosion.wav")
            End If
        Else
            lblIndicate.Text = "MISS!"
            PlaySound("myAudio", "Splash.wav")

        End If

        tmrTransition.Start()
    End Sub
    Private Sub plyBoatSink(ByVal intx As Integer, ByVal inty As Integer)
        For x = 1 To NUM_PLY_BOATS
            For y = 1 To plyBoatLength(x)
                If plyBoats(x, y, BOAT_X) = intx And plyBoats(x, y, BOAT_Y) = inty Then
                    Dim gridCounter(2, plyBoatLength(x)) As Integer
                    For z = 1 To plyBoatLength(x)
                        gridCounter(BOAT_X, z) = plyBoats(x, z, BOAT_X)
                        gridCounter(BOAT_Y, z) = plyBoats(x, z, BOAT_Y)
                    Next
                    If plyBoatLength(x) > 1 Then
                        If gridCounter(BOAT_X, 1) = gridCounter(BOAT_X, 2) Then
                            Dim minY As Integer = 100
                            For z = 1 To plyBoatLength(x)
                                If minY > gridCounter(BOAT_Y, z) Then
                                    minY = gridCounter(BOAT_Y, z)
                                End If
                            Next
                            makePictureBoxes(plyPicBoat(plyBoatSank), My.Resources.Ship_Wendy_Picked_, plyGrids(gridCounter(BOAT_X, 1), minY).Location.X, plyGrids(gridCounter(BOAT_X, 1), minY).Location.Y, GRID_SIZE, plyBoatLength(x) * GRID_SIZE)
                        Else
                            Dim minX As Integer = 100
                            For z = 1 To plyBoatLength(x)
                                If minX > gridCounter(BOAT_X, z) Then
                                    minX = gridCounter(BOAT_X, z)
                                End If
                            Next
                            makePictureBoxes(plyPicBoat(plyBoatSank), My.Resources.BoatY, plyGrids(minX, gridCounter(BOAT_Y, 1)).Location.X, plyGrids(minX, gridCounter(BOAT_Y, 1)).Location.Y, GRID_SIZE * plyBoatLength(x), GRID_SIZE)
                        End If
                    Else
                        makePictureBoxes(plyPicBoat(plyBoatSank), My.Resources.Ship_Wendy_Picked_, plyGrids(intx, inty).Location.X, plyGrids(intx, inty).Location.Y, GRID_SIZE, GRID_SIZE)
                    End If
                End If
            Next
        Next
    End Sub
    Private Function randomPositionX() As Integer
        Return Int((Rnd() * (GRID_X - 1)) + 1)
    End Function
    Private Function randomPositionY() As Integer
        Return Int((Rnd() * (GRID_Y - 1)) + 1)
    End Function
    Private Function randomDirection() As Integer
        Return Int(Rnd() * 2 + 1)
    End Function
    Private Sub plyInitBoats()
        Dim check As Boolean = True
        Dim direction(NUM_PLY_BOATS) As Integer
        For x = 1 To NUM_PLY_BOATS
            direction(x) = randomDirection()
        Next
        Do
            check = True
            For x = 1 To NUM_PLY_BOATS
                plyBoats(x, 1, BOAT_X) = randomPositionX()
                plyBoats(x, 1, BOAT_Y) = randomPositionY()
                For y = 2 To plyBoatLength(x)
                    If direction(x) = 1 Then
                        plyBoats(x, y, BOAT_Y) = plyBoats(x, 1, BOAT_Y)
                        plyBoats(x, y, BOAT_X) = plyBoats(x, y - 1, BOAT_X) + 1
                        If plyBoats(x, y, BOAT_X) > GRID_X Then
                            plyBoats(x, y, BOAT_X) = plyBoats(x, 1, BOAT_X) - 1
                        End If
                        For z = 1 To y - 1
                            If plyBoats(x, y, BOAT_X) = plyBoats(x, y - z, BOAT_X) Then
                                plyBoats(x, y, BOAT_X) = plyBoats(x, y, BOAT_X) - 1
                            End If
                        Next
                    Else
                        plyBoats(x, y, BOAT_X) = plyBoats(x, 1, BOAT_X)
                        plyBoats(x, y, BOAT_Y) = plyBoats(x, y - 1, BOAT_Y) + 1
                        If plyBoats(x, y, BOAT_Y) > GRID_Y Then
                            plyBoats(x, y, BOAT_Y) = plyBoats(x, 1, BOAT_Y) - 1
                        End If
                        For z = 1 To y - 1
                            If plyBoats(x, y, BOAT_Y) = plyBoats(x, y - z, BOAT_Y) Then
                                plyBoats(x, y, BOAT_Y) = plyBoats(x, y, BOAT_Y) - 1
                            End If
                        Next
                    End If
                Next
            Next
            For x = 1 To NUM_PLY_BOATS
                For y = 1 To plyBoatLength(x)
                    For a = 1 To NUM_PLY_BOATS
                        For b = 1 To plyBoatLength(a)
                            If plyBoats(x, y, BOAT_X) = plyBoats(a, b, BOAT_X) And plyBoats(x, y, BOAT_Y) = plyBoats(a, b, BOAT_Y) Then
                                If Not (x = a And y = b) Then
                                    check = False
                                End If
                            End If
                        Next
                    Next
                Next
            Next
        Loop Until check = True
    End Sub
    Private Sub plyPlaceBoats()
        plyInitBoats()

        For x = 1 To NUM_PLY_BOATS
            For y = 1 To plyBoatLength(x)
                plyElement(plyBoats(x, y, BOAT_X), plyBoats(x, y, BOAT_Y), ISBOAT) = True
            Next
        Next
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                If plyElement(x, y, ISBOAT) = True Then
                    plyGrids(x, y).BackColor = BOAT_CLR
                End If
            Next
        Next
    End Sub
    Private Function plyIsHit(ByVal intx As Integer, ByVal inty As Integer) As Boolean
        If plyElement(intx, inty, ISBOAT) = True Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function plyShipSank(ByVal intx As Integer, ByVal inty As Integer) As Boolean
        For x = 1 To NUM_PLY_BOATS
            For y = 1 To plyBoatLength(x)
                If plyBoats(x, y, BOAT_X) = intx And plyBoats(x, y, BOAT_Y) = inty Then
                    For z = 1 To plyBoatLength(x)
                        If plyElement(plyBoats(x, z, BOAT_X), plyBoats(x, z, BOAT_Y), PLYCHOSE) = False Then
                            Return False
                        End If
                    Next
                End If
            Next
        Next
        Return True
    End Function
    Private Sub cpuPickGrid()

        Dim intx, inty As Integer
        If cpuHit = True Then
            Do
                cpuSmartenUp(intx, inty)
            Loop Until cpuElement(intx, inty, CPUCHOSE) = False
        Else
            Do
                intx = randomPositionX()
                inty = randomPositionY()
            Loop Until cpuElement(intx, inty, CPUCHOSE) = False
        End If

        cpuElement(intx, inty, CPUCHOSE) = True
        cpuGrids(intx, inty).BackColor = BUTTON_CLR

        If cpuIsHit(intx, inty) = True Then
            cpuHit = True
            plyElement(intx, inty, ISHIT) = True
            If cpuShipSank(intx, inty) = True Then
                cpuBoatSank = cpuBoatSank + 1
                If cpuBoatSank = NUM_CPU_BOATS Then
                    'Computer has won
                    cpuSinkBoat(intx, inty)
                    loseSequence()
                    lblIndicate.Text = "You lose"
                    For x = 1 To GRID_X
                        For y = 1 To GRID_Y
                            plyButtons(x, y).Enabled = False
                        Next
                    Next
                Else
                    cpuHit = False
                    For x = 1 To NUM_CPU_BOATS
                        For y = 1 To cpuBoatLength(x)
                            If cpuBoats(x, y, BOAT_X) = intx And cpuBoats(x, y, BOAT_Y) = inty Then
                                For z = 1 To cpuBoatLength(x)
                                    plyElement(cpuBoats(x, z, BOAT_X), cpuBoats(x, z, BOAT_Y), ISHIT) = False
                                Next
                            End If
                        Next
                    Next
                    For x = 1 To GRID_X
                        For y = 1 To GRID_Y
                            If plyElement(x, y, ISHIT) = True Then
                                cpuHit = True
                            End If
                        Next
                    Next
                    'Computer has sank a boat
                    cpuSinkBoat(intx, inty)
                End If
            Else
                'Computer has hit a boat

                cpuGrids(intx, inty).Image = My.Resources.Flame
                cpuGrids(intx, inty).SizeMode = PictureBoxSizeMode.StretchImage
            End If
            cpuGrids(intx, inty).BackColor = HIT_CLR
        Else
        End If

    End Sub   
    Private Sub cpuSinkBoat(ByVal intx As Integer, ByVal inty As Integer)
        For x = 1 To NUM_CPU_BOATS
            For y = 1 To cpuBoatLength(x)
                If cpuBoats(x, y, BOAT_X) = intx And cpuBoats(x, y, BOAT_Y) = inty Then
                    Dim gridCounter(2, cpuBoatLength(x)) As Integer
                    For z = 1 To cpuBoatLength(x)
                        gridCounter(BOAT_X, z) = cpuBoats(x, z, BOAT_X)
                        gridCounter(BOAT_Y, z) = cpuBoats(x, z, BOAT_Y)
                    Next
                    If cpuBoatLength(x) > 1 Then
                        If gridCounter(BOAT_X, 1) = gridCounter(BOAT_X, 2) Then
                            Dim minY As Integer = 100
                            For z = 1 To cpuBoatLength(x)
                                If minY > gridCounter(BOAT_Y, z) Then
                                    minY = gridCounter(BOAT_Y, z)
                                End If
                            Next
                            makePictureBoxes(cpuPicBoat(cpuBoatSank), My.Resources.Ship_Wendy_Picked_, cpuGrids(gridCounter(BOAT_X, 1), minY).Location.X, cpuGrids(gridCounter(BOAT_X, 1), minY).Location.Y, MINI_SIZE - 1, cpuBoatLength(x) * (MINI_SIZE - 1))
                        Else
                            Dim minX As Integer = 100
                            For z = 1 To cpuBoatLength(x)
                                If minX > gridCounter(BOAT_X, z) Then
                                    minX = gridCounter(BOAT_X, z)
                                End If
                            Next
                            makePictureBoxes(cpuPicBoat(cpuBoatSank), My.Resources.BoatY, cpuGrids(minX, gridCounter(BOAT_Y, 1)).Location.X, cpuGrids(minX, gridCounter(BOAT_Y, 1)).Location.Y, (MINI_SIZE - 1) * cpuBoatLength(x), MINI_SIZE - 1)
                        End If
                    Else
                        makePictureBoxes(cpuPicBoat(cpuBoatSank), My.Resources.Ship_Wendy_Picked_, cpuGrids(intx, inty).Location.X, cpuGrids(intx, inty).Location.Y, MINI_SIZE - 1, MINI_SIZE - 1)
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub CountScore(ByRef score As Integer, ByVal type As Integer, ByRef display As Label) '
        score = score + type
        display.Text = "Score: " & score
    End Sub
    Private Sub makePictureBoxes(ByRef box As PictureBox, ByVal picture As Image, ByVal locationX As Integer, ByVal locationY As Integer, ByVal height As Integer, ByVal width As Integer)
        box = New PictureBox
        box.Image = picture
        box.SizeMode = PictureBoxSizeMode.StretchImage
        box.Location = New Point(locationX, locationY)
        box.Height = height
        box.Width = width
        box.Visible = True
        Me.Controls.Add(box)
        box.BringToFront()
    End Sub
    Private Sub loseSequence()
        If MainMenu.blnBGM = True Then
            My.Computer.Audio.Play(My.Resources.Titanic, AudioPlayMode.BackgroundLoop)
        End If
        makePictureBoxes(picWin, My.Resources.GameOverPic, POSITION_X + GRID_SIZE, POSITION_Y, GRID_X * GRID_SIZE, GRID_Y * GRID_SIZE)
        timerExplode.Start()
        Me.Controls.Remove(lblScore)
        MsgBox("You Lose")
        EndSequence()
    End Sub
    Private Sub winSequence()
        makePictureBoxes(picWin, My.Resources.Winner, 0, 0, 1, 1)
        displayWinner.Start()
        EndSequence()
        recordScore(Score)
    End Sub
    Private Sub EndSequence()
        plyReset.Width = plyReset.Width * 2
        plyReset.Text = "Play Again"
        picWin.BringToFront()
        plyReset.BringToFront()
        btnMainMenu.Visible = False
        btnQuit.Visible = False
        btnInstructions.Visible = False
        btnSettings.Visible = False
        plyReset.Location = New Point(GRID_Y / 2 * GRID_SIZE, GRID_X * GRID_SIZE)
    End Sub
    Private Sub timerExplode_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim formSize As Size = Me.Size
        Me.AutoSize = False
        Me.Size = formSize

        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                If plyElement(x, y, PLYCHOSE) = False Then
                    If y > GRID_Y \ 2 Then
                        plyButtons(x, y).BringToFront()
                        plyButtons(x, y).Location = New Point(plyButtons(x, y).Location.X + 20, plyButtons(x, y).Location.Y)
                    Else
                        plyButtons(x, y).BringToFront()
                        plyButtons(x, y).Location = New Point(plyButtons(x, y).Location.X - 20, plyButtons(x, y).Location.Y)
                    End If
                End If
            Next
        Next
        intExplode = intExplode + 1
        If intExplode = 20 Then
            sender.stop()
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    Me.Controls.Remove(plyButtons(x, y))
                Next
            Next
            Me.AutoSize = True

        End If

    End Sub
    Private Sub displayWinner_tick(ByVal sender As System.Object, ByVal e As System.EventArgs) '
        If picWin.Height < GRID_X * GRID_SIZE Then
            picWin.Height = picWin.Height + 2
        End If
        If picWin.Width < GRID_Y * GRID_SIZE Then
            picWin.Width = picWin.Width + 2
        End If
        If picWin.Location.X < POSITION_X + GRID_SIZE Then
            picWin.Location = New Point(picWin.Location.X + 2, picWin.Location.Y)
        End If
        If picWin.Location.Y < POSITION_Y Then
            picWin.Location = New Point(picWin.Location.X, picWin.Location.Y + 2)
        End If

        If picWin.Height >= GRID_X * GRID_SIZE And picWin.Width >= GRID_Y * GRID_SIZE And picWin.Location.X >= POSITION_X + GRID_SIZE And picWin.Location.Y >= POSITION_Y Then
            sender.stop()
        End If
    End Sub
    Private Sub bonusTimer_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If plyElement(xTEMP, yTEMP, PLYCHOSE) = False Then
            plyButtons(xTEMP, yTEMP).Enabled = False
            plyButtons(xTEMP, yTEMP).BackColor = Color.FromArgb(192, 255, 192)
            CountScore(Score, BONUS, lblScore)
        End If
        yTEMP = yTEMP + 1
        If yTEMP = GRID_Y + 1 Then
            yTEMP = 1
            xTEMP = xTEMP + 1
        End If
        If xTEMP > GRID_X Then
            bonusTimer.Stop()
            winSequence()
        End If


    End Sub
    Private Sub cpuSmartenUp(ByRef intx As Integer, ByRef inty As Integer)
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                If plyElement(x, y, ISHIT) = True Then
                    intx = x
                    inty = y
                    If ((x + 1) <= GRID_X And plyElement(x + 1, y, ISHIT) = True) Or ((x - 1) > 0 And plyElement(x - 1, y, ISHIT) = True) Then
                        Dim xStart As Integer = x
                        Dim xEnd As Integer = x
                        While (xStart - 1 > 0)
                            If plyElement(xStart - 1, y, ISHIT) = True Then
                                xStart = xStart - 1
                            Else
                                Exit While
                            End If
                        End While
                        While (xEnd + 1 <= GRID_X)
                            If plyElement(xEnd + 1, y, ISHIT) = True Then
                                xEnd = xEnd + 1
                            Else
                                Exit While
                            End If
                        End While

                        If (cpuElement(xStart - 1, y, CPUCHOSE) = True Or xStart - 1 <= 0) And (cpuElement(xEnd + 1, y, CPUCHOSE) = True Or xEnd + 1 > GRID_X) Then
                            If randomDirection() = 1 Then
                                If randomDirection() = 1 Then
                                    If x + 1 <= GRID_X Then
                                        intx = x + 1
                                        inty = y
                                    Else
                                        intx = x - 1
                                        inty = y
                                    End If
                                Else
                                    If x - 1 > 0 Then
                                        intx = x - 1
                                        inty = y
                                    Else
                                        intx = x + 1
                                        inty = y
                                    End If
                                End If

                            Else
                                If randomDirection() = 1 Then
                                    If y + 1 <= GRID_Y Then
                                        intx = x
                                        inty = y + 1
                                        Exit For
                                    Else
                                        intx = x
                                        inty = y - 1
                                    End If
                                Else
                                    If y - 1 > 0 Then
                                        intx = x
                                        inty = y - 1
                                    Else
                                        intx = x
                                        inty = y + 1
                                    End If
                                End If
                            End If
                            Exit Sub
                        End If
                        If randomDirection() = 1 Then
                            If xStart - 1 > 0 Then
                                intx = xStart - 1
                                inty = y
                            End If
                        Else
                            If xStart + 1 <= GRID_X Then
                                intx = xEnd + 1
                                inty = y
                            End If
                        End If
                    ElseIf ((y + 1) <= GRID_Y And plyElement(x, y + 1, ISHIT) = True) Or ((y - 1) > 0 And plyElement(x, y - 1, ISHIT) = True) Then

                        Dim yStart As Integer = y
                        Dim yEnd As Integer = y
                        While (yStart - 1 > 0)
                            If plyElement(x, yStart - 1, ISHIT) = True Then
                                yStart = yStart - 1
                            Else
                                Exit While
                            End If
                        End While
                        While (yEnd + 1 <= GRID_Y)
                            If plyElement(x, yEnd + 1, ISHIT) = True Then
                                yEnd = yEnd + 1
                            Else
                                Exit While
                            End If
                        End While

                        If (cpuElement(x, yStart - 1, CPUCHOSE) = True Or yStart - 1 <= 0) And (cpuElement(x, yEnd + 1, CPUCHOSE) = True Or yEnd + 1 > GRID_Y) Then
                            If randomDirection() = 1 Then
                                If randomDirection() = 1 Then
                                    If x + 1 <= GRID_X Then
                                        intx = x + 1
                                        inty = y
                                    Else
                                        intx = x - 1
                                        inty = y
                                    End If
                                Else
                                    If x - 1 > 0 Then
                                        intx = x - 1
                                        inty = y
                                    Else
                                        intx = x + 1
                                        inty = y
                                    End If
                                End If

                            Else
                                If randomDirection() = 1 Then
                                    If y + 1 <= GRID_Y Then
                                        intx = x
                                        inty = y + 1
                                        Exit For
                                    Else
                                        intx = x
                                        inty = y - 1
                                    End If
                                Else
                                    If y - 1 > 0 Then
                                        intx = x
                                        inty = y - 1
                                    Else
                                        intx = x
                                        inty = y + 1
                                    End If
                                End If
                            End If
                            Exit Sub
                        End If

                        If randomDirection() = 1 Then
                            If yStart - 1 > 0 Then
                                intx = x
                                inty = yStart - 1
                            End If
                        Else
                            If yEnd + 1 <= GRID_Y Then
                                intx = x
                                inty = yEnd + 1
                            End If
                        End If
                    Else
                        If randomDirection() = 1 Then
                            If randomDirection() = 1 Then
                                If x + 1 <= GRID_X Then
                                    intx = x + 1
                                    inty = y
                                Else
                                    intx = x - 1
                                    inty = y
                                End If
                            Else
                                If x - 1 > 0 Then
                                    intx = x - 1
                                    inty = y
                                Else
                                    intx = x + 1
                                    inty = y
                                End If
                            End If

                        Else
                            If randomDirection() = 1 Then
                                If y + 1 <= GRID_Y Then
                                    intx = x
                                    inty = y + 1
                                    Exit For
                                Else
                                    intx = x
                                    inty = y - 1
                                End If
                            Else
                                If y - 1 > 0 Then
                                    intx = x
                                    inty = y - 1
                                Else
                                    intx = x
                                    inty = y + 1
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        Next
    End Sub
    Private Function cpuIsHit(ByVal intx As Integer, ByVal inty As Integer) As Boolean
        If cpuElement(intx, inty, ISBOAT) = True Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function cpuShipSank(ByVal intx As Integer, ByVal inty As Integer) As Boolean
        For x = 1 To NUM_CPU_BOATS
            For y = 1 To cpuBoatLength(x)
                If cpuBoats(x, y, BOAT_X) = intx And cpuBoats(x, y, BOAT_Y) = inty Then
                    For z = 1 To cpuBoatLength(x)
                        If cpuElement(cpuBoats(x, z, BOAT_X), cpuBoats(x, z, BOAT_Y), CPUCHOSE) = False Then
                            Return False
                        End If
                    Next
                End If
            Next
        Next
        Return True
    End Function
    Private Sub cpuButtons_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btnselected = sender
        buttonshrink.Start()
        cpuBack.Visible = True
        cpuPickProcedure()
    End Sub
    Private Sub cpuPickProcedure()
        cpuPlaceBoats()
        If cpuBoatPart < cpuBoatLength(cpuBoatNumber) Then
            If cpuBoatPart = 1 Then
                cpuFirstFilter(cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_X), cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_Y), cpuBoatLength(cpuBoatNumber))
            Else
                cpuSecondFilter(cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_X), cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_Y), cpuBoatLength(cpuBoatNumber))
            End If
            cpuBoatPart = cpuBoatPart + 1
        Else
            cpuBoatPart = 1
            cpuBoatNumber = cpuBoatNumber + 1
            If cpuBoatNumber <= NUM_CPU_BOATS Then
                cpuRestrict(cpuBoatLength(cpuBoatNumber))
            End If
        End If
        cpuDisplay()
    End Sub
    Private Sub cpuPlaceBoats()
        For x = 0 To GRID_X + 1
            cpuElement(x, 0, ISBOAT) = True
            cpuElement(x, GRID_Y + 1, ISBOAT) = True
        Next
        For y = 0 To GRID_Y + 1
            cpuElement(0, y, ISBOAT) = True
            cpuElement(GRID_X + 1, y, ISBOAT) = True
        Next
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                If cpuButtons(x, y) Is btnselected Then
                    cpuElement(x, y, ISBOAT) = True
                    cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_X) = x
                    cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_Y) = y
                    If cpuBoatPart = cpuBoatLength(cpuBoatNumber) Then
                        PickBoatPic(x, y)
                    End If
                End If
            Next
        Next

    End Sub
    Private Sub cpuFirstFilter(ByVal intx As Integer, ByVal inty As Integer, ByVal length As Integer)
        Const TEMPORARY As Integer = 0

        For x = 1 To length - 1
            If Not intx + x > GRID_X + 1 Then
                If cpuElement(intx + x, inty, ISBOAT) = True Then
                    For y = 1 To length - x
                        If Not intx - y < 0 Then
                            If cpuElement(intx - y, inty, ISBOAT) = True Then
                                cpuElement(intx + 1, inty, TEMPORARY) = True
                                cpuElement(intx - 1, inty, TEMPORARY) = True
                            End If
                        End If
                    Next
                End If
            End If
            If Not inty + x > GRID_Y + 1 Then
                If cpuElement(intx, inty + x, ISBOAT) = True Then
                    For y = 1 To length - x
                        If Not inty - y < 0 Then
                            If cpuElement(intx, inty - y, ISBOAT) = True Then
                                cpuElement(intx, inty + 1, TEMPORARY) = True
                                cpuElement(intx, inty - 1, TEMPORARY) = True
                            End If
                        End If
                    Next
                End If
            End If
        Next
        For x = -1 To 1 Step 2
            If cpuElement(intx + x, inty, ISBOAT) = False And cpuElement(intx + x, inty, TEMPORARY) = False Then
                cpuElement(intx + x, inty, CANBOAT) = True
            End If
            If cpuElement(intx, inty + x, ISBOAT) = False And cpuElement(intx, inty + x, TEMPORARY) = False Then
                cpuElement(intx, inty + x, CANBOAT) = True
            End If
        Next
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                cpuElement(x, y, TEMPORARY) = False
            Next
        Next
    End Sub
    Private Sub cpuSecondFilter(ByVal intx As Integer, ByVal inty As Integer, ByVal length As Integer)
        For x = 0 To cpuBoatPart - 1
            If intx = cpuBoats(cpuBoatNumber, cpuBoatPart - 1, BOAT_X) Then
                cpuElement(intx, cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_Y) - 1, CANBOAT) = True
                cpuElement(intx, cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_Y) + 1, CANBOAT) = True
            Else
                cpuElement(cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_X) - 1, inty, CANBOAT) = True
                cpuElement(cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_X) + 1, inty, CANBOAT) = True
            End If
        Next
    End Sub
    Private Sub cpuRestrict(ByVal length As Integer)
        Const PROBABLE As Integer = 0
        For z = 1 To length - 1
            For k = 0 To z
                For y = 1 To GRID_Y
                    For x = 1 To GRID_X - z + 1
                        If cpuElement(x + z, y, ISBOAT) = True And cpuElement(x - 1, y, ISBOAT) = True Then
                            cpuElement(x + k, y, PROBABLE) = True
                        End If
                    Next
                    For x = z To GRID_X
                        If cpuElement(x - z, y, ISBOAT) = True And cpuElement(x + 1, y, ISBOAT) = True Then
                            cpuElement(x - k, y, PROBABLE) = True
                        End If
                    Next
                Next
            Next
        Next
        For z = 1 To length - 1
            For k = 0 To z
                For x = 1 To GRID_X
                    For y = 1 To GRID_Y - z + 1
                        If cpuElement(x, y + z, ISBOAT) = True And cpuElement(x, y - 1, ISBOAT) = True Then
                            If cpuElement(x, y + k, PROBABLE) = True Then
                                cpuElement(x, y + k, RESTRICTED) = True
                            End If
                        End If
                    Next
                    For y = z To GRID_Y
                        If cpuElement(x, y - z, ISBOAT) = True And cpuElement(x, y + 1, ISBOAT) = True Then
                            If cpuElement(x, y - k, PROBABLE) = True Then
                                cpuElement(x, y - k, RESTRICTED) = True
                            End If
                        End If
                    Next
                Next
            Next
        Next
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                cpuElement(x, y, PROBABLE) = False
            Next
        Next
    End Sub
    Private Sub cpuDisplay()
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                If cpuElement(x, y, CANBOAT) = True Then
                    cpuButtons(x, y).BackColor = FILTER_CLR
                    cpuButtons(x, y).Enabled = True
                    cpuElement(x, y, CANBOAT) = False
                Else
                    cpuButtons(x, y).Enabled = False
                    cpuButtons(x, y).BackColor = BUTTON_CLR
                End If
                If cpuElement(x, y, ISBOAT) = True Then
                    cpuGrids(x, y).BackColor = BOAT_CLR
                    PlaySound("myAudio", "PlaceBoat.mp3")
                End If
                If cpuElement(x, y, RESTRICTED) = True Then
                    cpuButtons(x, y).BackColor = HIT_CLR
                    cpuButtons(x, y).Enabled = False
                    cpuElement(x, y, RESTRICTED) = False
                End If
            Next
        Next
        If cpuBoatPart = 1 Then
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    If cpuButtons(x, y).BackColor <> HIT_CLR Then
                        cpuButtons(x, y).Enabled = True
                    End If
                Next
            Next
        End If
        If cpuBoatNumber > NUM_CPU_BOATS Then
            cpuNext.Visible = True
            lblIndicate.Text = "Click next"
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    cpuButtons(x, y).Enabled = False
                    cpuButtons(x, y).BackColor = WATER_CLR
                Next
            Next
        End If
    End Sub
    Private Sub PickBoatPic(ByVal intx As Integer, ByVal inty As Integer)
        For x = 1 To NUM_CPU_BOATS
            For y = 1 To cpuBoatLength(x)
                If cpuBoats(x, y, BOAT_X) = intx And cpuBoats(x, y, BOAT_Y) = inty Then
                    Dim gridCounter(2, cpuBoatLength(x)) As Integer
                    For z = 1 To cpuBoatLength(x)
                        gridCounter(BOAT_X, z) = cpuBoats(x, z, BOAT_X)
                        gridCounter(BOAT_Y, z) = cpuBoats(x, z, BOAT_Y)
                    Next
                    If cpuBoatLength(x) > 1 Then
                        If gridCounter(BOAT_X, 1) = gridCounter(BOAT_X, 2) Then
                            Dim minY As Integer = 100
                            For z = 1 To cpuBoatLength(x)
                                If minY > gridCounter(BOAT_Y, z) Then
                                    minY = gridCounter(BOAT_Y, z)
                                End If
                            Next
                            makePictureBoxes(cpuPicBoat(cpuBoatNumber), My.Resources.Ship_Wendy_Picked_, cpuGrids(gridCounter(BOAT_X, 1), minY).Location.X, cpuGrids(gridCounter(BOAT_X, 1), minY).Location.Y, GRID_SIZE, cpuBoatLength(x) * GRID_SIZE)
                        Else
                            Dim minX As Integer = 100
                            For z = 1 To cpuBoatLength(x)
                                If minX > gridCounter(BOAT_X, z) Then
                                    minX = gridCounter(BOAT_X, z)
                                End If
                            Next
                            makePictureBoxes(cpuPicBoat(cpuBoatNumber), My.Resources.BoatY, cpuGrids(minX, gridCounter(BOAT_Y, 1)).Location.X, cpuGrids(minX, gridCounter(BOAT_Y, 1)).Location.Y, GRID_SIZE * cpuBoatLength(x), GRID_SIZE)
                        End If
                    Else
                        makePictureBoxes(cpuPicBoat(cpuBoatNumber), My.Resources.Ship_Wendy_Picked_, cpuGrids(intx, inty).Location.X, cpuGrids(intx, inty).Location.Y, GRID_SIZE, GRID_SIZE)
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub FormControls_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If sender Is cpuReset Then
            'handles cpuReset
            cpuBoatPart = 1
            cpuBoatNumber = 1
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    Me.Controls.Remove(cpuButtons(x, y))
                    Me.Controls.Remove(cpuGrids(x, y))
                    cpuElement(x, y, ISBOAT) = False
                Next
            Next
            For x = 0 To NUM_CPU_BOATS
                Me.Controls.Remove(cpuPicBoat(x))
            Next
            For x = 0 To NUM_PLY_BOATS
                Me.Controls.Remove(plyPicBoat(x))
            Next
            cpuBack.Visible = False
            makeButtons(cpuButtons)
            makeGrids(cpuGrids)
        ElseIf sender Is btnInstructions Then
            'handles btnInstructions
            Instructions.Show()
        ElseIf sender Is btnSettings Then
            'handles btnSettings
            MainMenu.Settings.Show()
            MainMenu.Settings.BackgroundImage = My.Resources.Settingsbg
        ElseIf sender Is cpuBack Then
            'handles cpuBack
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    cpuButtons(x, y).Enabled = True
                    cpuButtons(x, y).BackColor = BUTTON_CLR
                    If btnselected Is cpuButtons(x, y) Then
                        cpuElement(x, y, ISBOAT) = False
                        cpuGrids(x, y).BackColor = WATER_CLR
                        Dim gridpoint As Point
                        gridpoint.X = POSITION_X + y * GRID_SIZE
                        gridpoint.Y = POSITION_Y + (x - 1) * GRID_SIZE
                        cpuButtons(x, y) = New Button
                        cpuButtons(x, y).Location = gridpoint
                        cpuButtons(x, y).Width = GRID_SIZE - 1
                        cpuButtons(x, y).Height = GRID_SIZE - 1
                        Me.Controls.Add(cpuButtons(x, y))
                        AddHandler cpuButtons(x, y).Click, AddressOf cpuButtons_click
                        cpuButtons(x, y).BringToFront()
                    End If
                Next
            Next
            cpuNext.Visible = False
            For x = 1 To 2
                If cpuBoatPart > 1 Then
                    cpuBoatPart = cpuBoatPart - 1
                Else
                    Me.Controls.Remove(cpuPicBoat(cpuBoatNumber - 1))
                    cpuBoatNumber = cpuBoatNumber - 1
                    cpuBoatPart = cpuBoatLength(cpuBoatNumber)
                End If
            Next
            If cpuBoatNumber = 0 And cpuBoatPart = 0 Then
                cpuBack.Visible = False
            End If
            btnselected = cpuButtons(cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_X), cpuBoats(cpuBoatNumber, cpuBoatPart, BOAT_Y))
            cpuPickProcedure()
        ElseIf sender Is cpuNext Then
            'handles cpuNext
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    Me.Controls.Remove(cpuButtons(x, y))
                Next
            Next
            For x = 0 To NUM_CPU_BOATS
                Me.Controls.Remove(cpuPicBoat(x))
            Next
            dropgrids.Start()
            cpuBoatNumber = 1
            cpuBoatPart = 1
            plyPlay()
            Me.Controls.Remove(cpuBack)
            Me.Controls.Remove(cpuNext)
            Me.Controls.Remove(cpuReset)
            lblScore.Visible = True
            lblIndicate.Text = "Hit 'Em Down"
        ElseIf sender Is plyReset Then
            'handles plyReset
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    Me.Controls.Remove(plyGrids(x, y))
                    Me.Controls.Remove(cpuGrids(x, y))
                    Me.Controls.Remove(plyButtons(x, y))
                    Me.Controls.Remove(cpuGrids(x, y))
                Next
            Next

            For Each z As Integer In {ISBOAT, ISHIT, RESTRICTED, CANBOAT, CPUCHOSE, PLYCHOSE}
                For x = 1 To GRID_X
                    For y = 1 To GRID_Y
                        plyElement(x, y, z) = False
                        cpuElement(x, y, z) = False
                    Next
                Next
            Next
            For x = 0 To NUM_CPU_BOATS
                Me.Controls.Remove(cpuPicBoat(x))
            Next
            For x = 0 To NUM_PLY_BOATS
                Me.Controls.Remove(plyPicBoat(x))
            Next
            Me.Controls.Remove(plyReset)
            btnMainMenu.Visible = True
            btnQuit.Visible = True
            btnInstructions.Visible = True
            btnSettings.Visible = True

            cpuHit = False

            cpuPlay()
            Me.Controls.Remove(picWin)
            cpuBoatSank = 0
            plyBoatSank = 0
            Score = 0
            CountScore(Score, 0, lblScore)
            lblScore.Visible = False
            lblIndicate.Text = "Place your boats!"
            sender.text = "Reset"
            My.Computer.Audio.Stop()
            If MainMenu.blnBGM = True Then
                My.Computer.Audio.Play(My.Resources.Pirates_Hook, AudioPlayMode.BackgroundLoop)
            End If

            ElseIf sender Is btnMainMenu Then
                'handles btnMainMenu
                MainMenu.Show()
                Me.Close()
            ElseIf sender Is btnQuit Then
            'handles btnQuit
            MainMenu.Close()
                Me.Close()
            End If
            PlaySound("myAudio", "ClickButton.mp3")
    End Sub
    Private Sub makeButtons(ByVal button(,) As Button)
        Dim gridpoint As Point
        gridpoint.X = POSITION_X
        gridpoint.Y = POSITION_Y
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                button(x, y) = New Button
                button(x, y).Height = GRID_SIZE
                button(x, y).Width = GRID_SIZE
                gridpoint.X = gridpoint.X + GRID_SIZE
                button(x, y).Location = gridpoint
                button(x, y).BackColor = BUTTON_CLR
                button(x, y).BringToFront()
                Me.Controls.Add(button(x, y))
                If button(x, y) Is cpuButtons(x, y) Then
                    AddHandler button(x, y).Click, AddressOf cpuButtons_click
                Else
                    AddHandler button(x, y).Click, AddressOf plyButtons_click
                End If
            Next
            gridpoint.Y = gridpoint.Y + GRID_SIZE
            gridpoint.X = POSITION_X
        Next
    End Sub
    Private Sub makeGrids(ByVal grid(,) As PictureBox)
        Dim gridpoint As Point
        gridpoint.X = POSITION_X
        gridpoint.Y = POSITION_Y
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                grid(x, y) = New PictureBox
                grid(x, y).Height = GRID_SIZE - 1
                grid(x, y).Width = GRID_SIZE - 1
                gridpoint.X = gridpoint.X + GRID_SIZE
                grid(x, y).Location = gridpoint
                grid(x, y).BackColor = WATER_CLR
                Me.Controls.Add(grid(x, y))
            Next
            gridpoint.Y = gridpoint.Y + GRID_SIZE
            gridpoint.X = POSITION_X
        Next
    End Sub
    Private Sub makeLabels()
        Dim gridpoint As Point
        gridpoint.X = POSITION_X - GRID_SIZE / 2
        gridpoint.Y = POSITION_Y - GRID_SIZE
        For x = 1 To GRID_X
            lblAxisX(x) = New Label
            lblAxisX(x).Height = GRID_SIZE
            lblAxisX(x).Width = GRID_SIZE + 5
            lblAxisX(x).Font = New Font("Lucida Console", GRID_SIZE / 2)
            lblAxisX(x).Text = GRID_X - x + 1
            gridpoint.Y = gridpoint.Y + GRID_SIZE
            lblAxisX(x).Location = gridpoint
            lblAxisX(x).SendToBack()
            Me.Controls.Add(lblAxisX(x))
        Next

        gridpoint.X = POSITION_X
        gridpoint.Y = POSITION_Y + GRID_SIZE * GRID_X
        For x = 1 To GRID_Y
            lblAxisY(x) = New Label
            lblAxisY(x).Height = GRID_SIZE
            lblAxisY(x).Width = GRID_SIZE
            lblAxisY(x).Font = New Font("Lucida Console", GRID_SIZE / 2)
            lblAxisY(x).Text = Chr(65 + x - 1)
            gridpoint.X = gridpoint.X + GRID_SIZE
            lblAxisY(x).Location = gridpoint
            Me.Controls.Add(lblAxisY(x))
        Next
        lblScore = New Label
        lblScore.Width = GRID_SIZE * 2
        lblScore.Height = GRID_SIZE * 2
        lblScore.Location = New Point(gridpoint.X + GRID_SIZE + 10, 10)
        lblScore.Font = New Font("Lucida Console", GRID_SIZE / 3)
        Me.Controls.Add(lblScore)
        CountScore(Score, 0, lblScore)
        lblIndicate = New Label
        lblIndicate.Height = GRID_SIZE * 2
        lblIndicate.Width = GRID_SIZE * 2
        lblIndicate.Location = New Point(gridpoint.X + GRID_SIZE, lblScore.Bottom + 5)
        lblIndicate.Font = New Font("Lucida Console", GRID_SIZE / 3)
        lblIndicate.Text = "Place your boats!"

        Me.Controls.Add(lblIndicate)

    End Sub
    Private Sub dynCreateButton(ByRef button As Button, ByVal POSITION_X As Integer, ByVal POSITION_Y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal text As String)
        Dim gridpoint As Point
        gridpoint.X = POSITION_X
        gridpoint.Y = POSITION_Y

        button = New Button
        button.Location = gridpoint
        button.Text = text
        button.BackColor = Color.White
        button.Font = New Font("Lucida Console", height / 4)
        button.Height = height
        button.Width = width
        Me.Controls.Add(button)
        AddHandler button.Click, AddressOf FormControls_click
    End Sub
    Private Sub recordScore(ByVal plyscore As Integer)
        Dim writerScore As System.IO.StreamWriter
        Dim writerName As System.IO.StreamWriter
        writerScore = My.Computer.FileSystem.OpenTextFileWriter("Score.txt", True)
        writerName = My.Computer.FileSystem.OpenTextFileWriter("Name.txt", True)
        writerScore.WriteLine(plyscore)
        writerName.WriteLine(MainMenu.plyname)
        writerScore.Close()
        writerName.Close()
    End Sub
    Private colorchange, buttonshrink, dropgrids, timerExplode, displayWinner, bonusTimer, tmrTransition As Timer
    Private Sub makeTimers()
        tmrTransition = New Timer
        buttonshrink = New Timer
        colorchange = New Timer
        dropgrids = New Timer
        bonusTimer = New Timer
        displayWinner = New Timer
        timerExplode = New Timer
        buttonshrink.Interval = 20
        colorchange.Interval = 20
        dropgrids.Interval = 1
        bonusTimer.Interval = 100
        displayWinner.Interval = 5
        timerExplode.Interval = 50
        tmrTransition.Interval = 30
        AddHandler buttonshrink.Tick, AddressOf buttonShrink_tick
        AddHandler dropgrids.Tick, AddressOf dropGrids_tick
        AddHandler bonusTimer.Tick, AddressOf bonusTimer_tick
        AddHandler displayWinner.Tick, AddressOf displayWinner_tick
        AddHandler timerExplode.Tick, AddressOf timerExplode_tick
        AddHandler tmrTransition.Tick, AddressOf tmrTransition_tick
        AddHandler colorchange.Tick, AddressOf colorchange_tick
    End Sub
    Private xTEMP, yTEMP, intExplode As Integer
    Private Sub buttonShrink_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If btnselected.Height > 0 Then
            btnselected.Height = btnselected.Height - 20
            btnselected.Width = btnselected.Width - 20
            btnselected.Left = btnselected.Left + 10
            btnselected.Top = btnselected.Top + 10
        Else
            sender.stop()
        End If
    End Sub
    Private Sub dropGrids_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Const INT_CHANGE As Integer = 3
        If cpuGrids(1, 1).Height > MINI_SIZE Then
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    cpuGrids(x, y).Height = cpuGrids(x, y).Height - INT_CHANGE
                    cpuGrids(x, y).Width = cpuGrids(x, y).Width - INT_CHANGE
                Next
            Next
            For y = 1 To GRID_Y
                cpuGrids(1, y).Top = cpuGrids(1, y).Top + GRID_X * GRID_SIZE / (GRID_SIZE - MINI_SIZE + 1) * INT_CHANGE
            Next
            For x = 2 To GRID_X
                For y = 1 To GRID_Y
                    cpuGrids(x, y).Top = cpuGrids(x - 1, y).Top + cpuGrids(x, y).Width + 1
                Next
            Next
            For x = 1 To GRID_X
                cpuGrids(x, 1).Left = cpuGrids(x, 1).Left + GRID_Y / 2 * INT_CHANGE
            Next
            For y = 2 To GRID_Y
                For x = 1 To GRID_X
                    cpuGrids(x, y).Left = cpuGrids(x, y - 1).Left + cpuGrids(x, y).Width + 1
                Next
            Next
        ElseIf cpuGrids(1, 1).Top < (POSITION_Y + (GRID_X + 1) * GRID_SIZE) Then
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    cpuGrids(x, y).Top = cpuGrids(x, y).Top + INT_CHANGE
                Next
            Next
        Else
            sender.stop()
        End If
    End Sub
    Private intTimer As Integer
    Private Sub tmrTransition_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        intTimer = intTimer + 1
        If intTimer = 10 Then
            sender.stop()
            intTimer = 0
            cpuPickGrid()
        End If
    End Sub
    Private Sub assignBoatLengths()
        'CPU_BOATS
        For x = 1 To NUM_CPU_BOATS
            cpuBoatLength(x) = Difficulty.BoatLengths(Difficulty.PLY, Difficulty.Level, x)
        Next

        'PLY_BOATS
        For x = 1 To NUM_PLY_BOATS
            plyBoatLength(x) = Difficulty.BoatLengths(Difficulty.CPU, Difficulty.Level, x)
        Next
    End Sub
    Public Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer
    Private Sub PlaySound(ByVal soundType As String, ByVal musicPath As String)
        If MainMenu.blnSFX = True Then
            Dim musicAlias As String = "myAudio"
            mciSendString("close " & musicAlias, CStr(0), 0, 0)
            mciSendString("Open " & Chr(34) & musicPath & Chr(34) & " alias " & musicAlias, CStr(0), 0, 0)
            mciSendString("play " & musicAlias, CStr(0), 0, 0)
        End If
    End Sub
    Private Sub colorChange_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.BackColor = Color.FromArgb(Rv, Bv, Gv)
        If Rv + Rc >= 255 Or Rv + Rc <= 0 Then
            Rc = -Rc
        End If
        If Gv + Gc >= 255 Or Gv + Gc <= 0 Then
            Gc = -Gc
        End If
        If Bv + Bc >= 255 Or Bv + Bc <= 0 Then
            Bc = -Bc
        End If

        Rv = Rv + Rc
        Bv = Bv + Bc
        Gv = Gv + Gc
    End Sub
    Private Rc As Integer = 2
    Private Bc As Integer = 5
    Private Gc As Integer = 7
    Private Rv As Integer = 1
    Private Bv As Integer = 1
    Private Gv As Integer = 1
    Private BUTTON_CLR As Color = Drawing.Color.White
    Private BOAT_CLR As Color = Drawing.Color.Black
    Private HIT_CLR As Color = Drawing.Color.FromArgb(255, 192, 192)
    Private WATER_CLR As Color = Drawing.Color.LightBlue
    Private FILTER_CLR As Color = Drawing.Color.FromArgb(192, 255, 192)
End Class