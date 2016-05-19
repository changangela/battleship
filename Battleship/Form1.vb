Public Class Battleship
    Public game As New Form
    'Start Changeable
    Private Const GRID_X As Integer = 8
    Private Const GRID_Y As Integer = 8

    Private Const GRID_SIZE As Integer = 35
    Private Const POSITION_X As Integer = GRID_SIZE
    Private Const POSITION_Y As Integer = 0

    Private Const MINI_SIZE As Integer = 20

    'upon changing the following 2 variables, the values in the "AssignBoatLengths" function must also be changed there is no error returned
    Private Const NUM_CPU_BOATS As Integer = 4
    Private Const NUM_PLY_BOATS As Integer = 3
    'End Changeable

    Private cpuElement(GRID_X + 1, GRID_Y + 1, 4), plyElement(GRID_X + 1, GRID_Y + 1, 4) As Boolean

    Private Const ISBOAT As Integer = 1
    Private Const CPUCHOSE As Integer = 2
    Private Const CANBOAT As Integer = 3
    Private Const RESTRICTED As Integer = 4
    Private Const PLYCHOSE As Integer = 2

    Private cpuBoats(NUM_CPU_BOATS, 8, 2), plyBoats(NUM_CPU_BOATS, 8, 2) As Integer
    Private Const BOAT_X As Integer = 1
    Private Const BOAT_Y As Integer = 2

    Private cpuBoatSank As Integer
    Private cpuBoatNumber As Integer = 1
    Private cpuBoatPart As Integer = 1
    Private cpuBoatLength(NUM_CPU_BOATS) As Integer

    Private plyBoatSank As Integer
    Private plyBoatLength(NUM_PLY_BOATS) As Integer

    Private plyButtons(GRID_X, GRID_Y), cpuButtons(GRID_X, GRID_Y) As Button
    Private plyGrids(GRID_X, GRID_Y), cpuGrids(GRID_X, GRID_Y) As PictureBox

    Private cpuReset, cpuBack, cpuNext, plyReset, btnQuit As Button
    Private lblAxis(2) As Label

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AutoSize = True
        Randomize()
        makeTimers()
        cpuPlay()
        dynCreateButton(btnQuit, POSITION_X - GRID_SIZE, POSITION_Y + (GRID_X + 2) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Quit")

        FileOpen(1, "scores.txt", OpenMode.Input, OpenShare.Default, -1)

    End Sub
    Private Function plyPlay()
        makeButtons(plyButtons)
        makeGrids(plyGrids)
        dynCreateButton(plyReset, POSITION_X + (GRID_Y + 1) * GRID_SIZE, POSITION_Y + (GRID_X + 2) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Reset")
        plyPlaceBoats()
    End Function
    Private Function cpuPlay()
        assignBoatLengths()
        makeButtons(cpuButtons)
        makeGrids(cpuGrids)
        dynCreateButton(cpuReset, POSITION_X + (GRID_Y + 1) * GRID_SIZE, POSITION_Y + (GRID_X + 2) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Reset")
        dynCreateButton(cpuBack, POSITION_X - GRID_SIZE, POSITION_Y + (GRID_X + 1) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Back")
        dynCreateButton(cpuNext, POSITION_X + (GRID_Y + 1) * GRID_SIZE, POSITION_Y + (GRID_X + 1) * GRID_SIZE, 2 * GRID_SIZE, GRID_SIZE, "Next")
        cpuBack.Visible = False
        cpuNext.Visible = False
    End Function
    Private btnselected As Button
    Private Sub plyButtons_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim intx, inty As Integer
        btnselected = sender
        buttonshrink.Start()
        For x = 1 To GRID_X
            For y = 1 To GRID_Y
                If plyButtons(x, y) Is sender Then
                    intx = x
                    inty = y
                    plyElement(x, y, PLYCHOSE) = True
                End If
            Next
        Next
        If plyIsHit(intx, inty) = True Then
            If plyShipSank(intx, inty) = True Then
                plyBoatSank = plyBoatSank + 1
                If plyBoatSank = NUM_PLY_BOATS Then
                    'Player has sank all boats
                Else
                    'Player has sank a boat
                End If
            Else
                'Player has hit a boat
            End If
        End If
        cpuPickGrid()
    End Sub
    Private Function randomPositionX() As Integer
        Return Int(Rnd() * GRID_X + 1)
    End Function
    Private Function randomPositionY() As Integer
        Return Int(Rnd() * GRID_Y + 1)
    End Function
    Private Function randomDirection() As Integer
        Return Int(Rnd() * 2 + 1)
    End Function
    Private Function plyInitBoats()
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
    End Function
    Private Function plyPlaceBoats()
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
    End Function
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
    Private Function cpuPickGrid()
        Dim intx, inty As Integer

        Do
            intx = randomPositionX()
            inty = randomPositionY()
        Loop Until cpuElement(intx, inty, CPUCHOSE) = False

        cpuElement(intx, inty, CPUCHOSE) = True
        cpuGrids(intx, inty).BackColor = BUTTON_CLR

        If cpuIsHit(intx, inty) = True Then
            If cpuShipSank(intx, inty) = True Then
                cpuBoatSank = cpuBoatSank + 1
                If cpuBoatSank = NUM_CPU_BOATS Then
                    'Computer has won
                Else
                    'Computer has sank a boat
                End If
            Else
                'Computer has hit a boat
            End If
            cpuGrids(intx, inty).BackColor = HIT_CLR
        End If
    End Function
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
    Private Function cpuPickProcedure()
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
    End Function
    Private Function cpuPlaceBoats()
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
                End If
            Next
        Next
    End Function
    Private Function cpuFirstFilter(ByVal intx As Integer, ByVal inty As Integer, ByVal length As Integer)
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
    End Function
    Private Function cpuSecondFilter(ByVal intx As Integer, ByVal inty As Integer, ByVal length As Integer)
        For x = 0 To cpuBoatPart - 1
            If intx = cpuBoats(cpuBoatNumber, cpuBoatPart - 1, BOAT_X) Then
                cpuElement(intx, cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_Y) - 1, CANBOAT) = True
                cpuElement(intx, cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_Y) + 1, CANBOAT) = True
            Else
                cpuElement(cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_X) - 1, inty, CANBOAT) = True
                cpuElement(cpuBoats(cpuBoatNumber, cpuBoatPart - x, BOAT_X) + 1, inty, CANBOAT) = True
            End If
        Next
    End Function
    Private Function cpuRestrict(ByVal length As Integer)
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
    End Function
    Private Function cpuDisplay()
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
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    cpuButtons(x, y).Enabled = False
                    cpuButtons(x, y).BackColor = WATER_CLR
                Next
            Next
        End If
    End Function
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
            cpuBack.Visible = False
            makeButtons(cpuButtons)
            makeGrids(cpuGrids)
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
            dropgrids.Start()
            cpuBoatNumber = 1
            cpuBoatPart = 1
            plyPlay()
            Me.Controls.Remove(cpuBack)
            Me.Controls.Remove(cpuNext)
            Me.Controls.Remove(cpuReset)
        ElseIf sender Is plyReset Then
            'handles plyReset
            cpuPlay()
            For x = 1 To GRID_X
                For y = 1 To GRID_Y
                    Me.Controls.Remove(plyGrids(x, y))
                    Me.Controls.Remove(plyButtons(x, y))
                    plyElement(x, y, ISBOAT) = False
                    plyElement(x, y, PLYCHOSE) = False
                    Me.Controls.Remove(cpuGrids(x, y))
                Next
            Next
            Me.Controls.Remove(plyReset)
        ElseIf sender Is btnQuit Then
            'handles btnQuit
            Me.Close()
        End If
    End Sub
    Private Function makeButtons(ByVal button(,) As Button)
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
    End Function
    Private Function makeGrids(ByVal grid(,) As PictureBox)
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
    End Function

    Private Function dynCreateButton(ByRef button As Button, ByVal POSITION_X As Integer, ByVal POSITION_Y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal text As String)
        Dim gridpoint As Point
        gridpoint.X = POSITION_X
        gridpoint.Y = POSITION_Y

        button = New Button
        button.Location = gridpoint
        button.Text = text
        button.Font = New Font("lucida console", height / 4)
        button.Height = height
        button.Width = width
        Me.Controls.Add(button)
        AddHandler button.Click, AddressOf FormControls_click
    End Function
    Private colorchange, buttonshrink, dropgrids As Timer
    Private Function makeTimers()
        buttonshrink = New Timer
        colorchange = New Timer
        dropgrids = New Timer
        buttonshrink.Interval = 20
        colorchange.Interval = 20
        dropgrids.Interval = 1
        AddHandler buttonshrink.Tick, AddressOf buttonShrink_tick
        AddHandler colorchange.Tick, AddressOf colorchange_tick
        AddHandler dropgrids.Tick, AddressOf dropGrids_tick
    End Function
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
    Private Sub colorchange_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
                cpuGrids(1, y).Top = cpuGrids(1, y).Top + GRID_X * GRID_SIZE / (GRID_SIZE - 20) * INT_CHANGE
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
        Else
            sender.stop()
        End If
    End Sub
    Private Function assignBoatLengths()
        'CPU_BOATS
        cpuBoatLength(1) = 3
        cpuBoatLength(2) = 3
        cpuBoatLength(3) = 4
        cpuBoatLength(4) = 2
        'cpuBoatLength(5) = 1
        'cpuBoatLength(6) = 4
        'cpuBoatLength(7) = 8

        'PLY_BOATS
        plyBoatLength(1) = 2
        plyBoatLength(2) = 3
        plyBoatLength(3) = 4
        'plyBoatLength(4) = 4
        'plyBoatLength(5) = 4
        'plyBoatLength(6) = 4
    End Function
    Private BUTTON_CLR As Color = Drawing.Color.White
    Private BOAT_CLR As Color = Drawing.Color.Black
    Private HIT_CLR As Color = Drawing.Color.FromArgb(255, 192, 192)
    Private WATER_CLR As Color = Drawing.Color.LightBlue
    Private FILTER_CLR As Color = Drawing.Color.FromArgb(192, 255, 192)

End Class


