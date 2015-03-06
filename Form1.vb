Imports System.Text
Imports System.IO

Public Class Form1
    Dim WithEvents ping As New System.Net.NetworkInformation.Ping()
    Dim latence As New Dictionary(Of UInteger, UInteger)
    Dim lat As New Dictionary(Of Date, Net.NetworkInformation.PingCompletedEventArgs)
    Dim ipadr As New ip
    Dim ipa As Net.IPAddress
    Private Sub ping_PingCompleted(sender As Object, e As Net.NetworkInformation.PingCompletedEventArgs) Handles ping.PingCompleted
        If ToolStripMenuItem1.Checked Then
            lat.Add(Now, e)
            Chart1.Series(0).Points.AddXY(TimeOfDay, e.Reply.RoundtripTime())
            If Chart1.Series(0).Points.Count > 5 Then

            End If
            If latence.ContainsKey(e.Reply.RoundtripTime()) Then
                latence(e.Reply.RoundtripTime()) += 1
            Else
                latence.Add(e.Reply.RoundtripTime(), 1)
            End If
            Chart2.Series(0).Points.DataBindXY(latence.Keys, latence.Values)
            Timer1.Enabled = True
            Dim sb As New StringBuilder
            sb.AppendLine(e.Reply.Address.ToString)
            sb.AppendLine(e.Reply.Status.ToString)
            'sb.AppendLine(e.Error.ToString)
            For Each b As Byte In e.Reply.Buffer
                sb.Append(b)
                sb.Append("-")
            Next
            Label1.Text = sb.ToString
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ipa = Net.IPAddress.Parse("8.8.8.8")
        ping.SendAsync(ipa, Nothing)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ping.SendAsync(ipa, Nothing)
        Timer1.Enabled = False
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        SaveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        SaveFileDialog1.DefaultExt = "*.txt"
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim rw As New StreamWriter(SaveFileDialog1.FileName)
            For Each ee In lat
                rw.Write(ee.Key.ToString)
                rw.Write(" - ")
                rw.WriteLine(ee.Value.Reply.RoundtripTime)
            Next
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        ToolStripMenuItem1.Checked = Not ToolStripMenuItem1.Checked
    End Sub
End Class
