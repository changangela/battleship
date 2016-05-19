Public Class LeaderBoards
    Private leaderBoard(100) As String
    Sub loadScore()
        'loads the high score information from the existing text files made previously
        txtScores.Text = ""
        Dim readerScore As System.IO.StreamReader
        Dim readerName As System.IO.StreamReader
        If My.Computer.FileSystem.FileExists("Score.txt") = True Then
            'if file exists, load it onto the textbox
            readerScore = My.Computer.FileSystem.OpenTextFileReader("Score.txt")
            readerName = My.Computer.FileSystem.OpenTextFileReader("Name.txt")
            For x = 0 To 10
                leaderBoard(x) = readerScore.ReadLine & vbTab & readerName.ReadLine
            Next
            Array.Sort(leaderBoard)
            Array.Reverse(leaderBoard)
            For x = 0 To 9
                txtScores.Text = txtScores.Text & leaderBoard(x) & vbCrLf
            Next
            readerScore.Close()
            readerName.Close()
        End If

    End Sub
    Private Sub LeaderBoards_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'loads the scores onto the textbox
        loadScore()
    End Sub
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        'deletes the textfiles containing the high score information
        If My.Computer.FileSystem.FileExists("Name.txt") = True Then
            My.Computer.FileSystem.DeleteFile("Name.txt")
            My.Computer.FileSystem.DeleteFile("Score.txt")
        End If
        'clears the high score textbox
        loadScore()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class