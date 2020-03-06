Public Class Form3
    Public UserName As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        UserName = TextBox1.Text
        Me.Close()
    End Sub
End Class