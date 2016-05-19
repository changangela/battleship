Imports System.Windows.Forms

Public Class Confirm
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        MainMenu.blConfirm = True
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        MainMenu.blConfirm = False
        Me.Close()
    End Sub
End Class
