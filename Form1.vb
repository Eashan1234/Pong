Public Class Form1
    Dim bsx As Integer
    Dim bsy As Integer
    Dim level As Integer
    Dim points As Integer
    Dim gameover As Boolean
    Dim timertick As Integer
    Dim dialogshowd As Boolean
    Dim paused As Boolean

    Structure player
        Dim UserName As String
        Dim Points As Integer
    End Structure

    Structure Leaderboard
        Public Shared players() As player
        Public Shared index As Integer

        Shared Sub New()
            ReDim players(10)
            index = 0
        End Sub

        Function AddItem(uname As String, score As Integer)
            Dim player As player

            player.UserName = uname
            player.Points = score
            players(index) = player
            Form2.ListBox1.Items.Add(players(index).UserName.ToString() + " - " + players(index).Points.ToString())
            index += 1
        End Function

        Function save()
            FileOpen(1, "C:\Data\CP2\LeaderBoard.txt", OpenMode.Output)
            For i = 0 To (index - 1)
                PrintLine(1, players(i).UserName)
                PrintLine(1, players(i).Points)
            Next
            FileClose()
        End Function

        Function retrieve()
            Dim UserName As String
            Dim Points As String
            FileOpen(1, "C:\Data\CP2\LeaderBoard.txt", OpenMode.Input)
            Do Until EOF(1)
                UserName = LineInput(1)
                Points = LineInput(1)
                AddItem(UserName, Val(Points))
            Loop
            FileClose()
        End Function

        Function Sort()
            Dim temp As Integer
            Form2.ListBox1.Items.Clear()

            For i = 0 To 9
                For j = 0 To 8
                    If (players(j + 1).Points > players(j).Points) Then
                        temp = players(j).Points
                        players(j).Points = players(j + 1).Points
                        players(j + 1).Points = temp
                    End If
                Next
            Next

            For i = 0 To 9
                Form2.ListBox1.Items.Add(players(i).UserName + " - " + players(i).Points.ToString())
            Next i
        End Function
    End Structure

    Enum State
        RUN
        START
        DONE
        GAMEOVER
        PAUSE
        IDOL
    End Enum

    Dim gameState As State = State.IDOL
    Dim leaderboard1 As Leaderboard

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If (gameState = State.RUN) Then

            picBall.Left = picBall.Left + bsx * (level + 6) * 6
            picBall.Top = picBall.Top + bsy * (level + 6) * 6

            If picBall.Top + picBall.Height + 25 > Height Then
                bsy = bsy * -1
            End If
            If picBall.Left + picBall.Width + 15 > Width Then
                bsx = bsx * -1
            End If
            If picBall.Top < 0 Then
                bsy = bsy * -1
            End If
            If picBall.Left < 300 Then
                bsx = bsx * -1
            End If
            If picBall.Left > Label1.Left Then
                If picBall.Left + picBall.Width < Label1.Left + Label1.Width Then
                    If picBall.Top + picBall.Height > Label1.Top Then
                        bsy = bsy * -1
                        level += 1
                        points += 10
                    End If
                End If
            End If
            If (picBall.Top > Label1.Top + 50) Then   'picBall.Top > Label1.Top
                dialogshowd = False
                gameover = True
                gameState = State.GAMEOVER
            End If

            Label3.Text = level.ToString
            Label2.Text = points.ToString
        ElseIf (gameState = State.START) Then
            level = 0
            points = 0
            picBall.Left = 833
            picBall.Top = 483
            picBall.Visible = True
            If (timertick >= 15) Then
                gameState = State.RUN
            End If
        ElseIf (gameState = State.IDOL) Then
            level = 0
            points = 0
            picBall.Left = 833
            picBall.Top = 483
            picBall.Visible = True
        ElseIf (gameState = State.GAMEOVER) Then
            If (gameover And timertick >= 13) Then
                gameover = False
                timertick = 0
                If (dialogshowd = False) Then
                    Form3.ShowDialog()
                    leaderboard1.AddItem(Form3.UserName, points)
                    dialogshowd = True
                End If
                picBall.Top = picBall.Top
                picBall.Left = picBall.Left
                picBall.Visible = False
            End If
        ElseIf (gameState = State.PAUSE) Then
            Dim result As DialogResult
            If (paused = False) Then
                paused = True
                picBall.Top = picBall.Top
                picBall.Left = picBall.Left
                result = MessageBox.Show("Game is Paused.", "Click OK to resume", MessageBoxButtons.OK)
            End If
            If (result = DialogResult.OK) Then
                paused = False
                gameState = State.RUN
            End If
        End If
        timertick += 1
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        bsx = 1
        bsy = 1

        level = 1
        points = 0
        gameover = True
        dialogshowd = False
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Dim x As Integer = e.X
        Label1.Left = x
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Q Then

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        gameState = State.PAUSE
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form2.Show()
        leaderboard1.Sort()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        leaderboard1.save()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        gameState = State.START
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        leaderboard1.retrieve()
    End Sub
End Class
