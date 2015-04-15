Imports System
Imports System.Text
Imports ip4   ' this is from the reference OptoMMP2.dll


Public Class Form1
    ' Tcp_Client is an assembly of the OptoMMP2.dll, this must be added as a
    ' reference to this project
    Dim objTcpClient As Tcp_Client = New Tcp_Client
    Dim sRecv As String
    Dim sXmit As String
    Dim bRunning As Boolean = False
    Dim bTransmit As Boolean = False


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim i32Module As Integer
        Dim i32Channel As Integer
        Dim i32Port As Integer
        Dim i32Result As Integer

        If (Not bRunning) Then
            ' convert text to values
            i32Module = Convert.ToInt32(textModule.Text)
            i32Channel = Convert.ToInt32(textChannel.Text)
            ' 22500 is the starting location of SNAP Serial Module TCP/IP ports
            i32Port = 22500 + i32Module * 2 + i32Channel

            ' open the connection
            i32Result = objTcpClient.Open(textIpAddress.Text, i32Port)

            If (i32Result = 0) Then
                ' initialize the timer's interval and start it (10 ms)
                Timer1.Interval = 10
                Timer1.Start()

                Button1.Text = "Disconnect"

                bRunning = True

                sRecv = ""
                sXmit = ""
            End If
        Else
            ' stop the timer
            Timer1.Stop()

            ' close the tcp connection
            objTcpClient.Close()

            ' change the button text
            Button1.Text = "Connect"

            ' indicate we're no longer running
            bRunning = False
        End If


    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim byaryRecv(10) As Byte
        Dim i32Result As Integer

        ' If i32Result is negative, close the session and reopen it.
        ' A negative return indicates a socket oriented error occurred.

        ' A real application should employ a timer on the receive.
        ' It is possible that if a networking connection fails it is possible that
        ' i32Result will return a zero response (no data received), forever.
        ' The timer will indicate when an abnormal cessation of data occurs.

        ' When reconnecting, the first attempt should be tried 1 second after disconnect.
        ' For successive failure, double the delay time; 2, 4, up to 8 seconds. This
        ' delay will prevent a race condition that may expire too many device sockets.

        ' this receives any pending data that has arrived on our TCP/IP socket
        i32Result = objTcpClient.Receive(byaryRecv, 0)
        If (i32Result > 0) Then
            ' we received data...
            Dim sTemp As String
            sTemp = ""
            ' Create two different encodings.

            ' convert bytes to string, this is for an ASCII type binary encoding
            Dim enc As New System.Text.ASCIIEncoding()
            sTemp = enc.GetString(byaryRecv, 0, i32Result)

            ' For users that need to convert binary byte streams to numeric data
            ' use the "BitConverter" assembly to convert byte streams to numeric
            ' data or vice versa

            ' If you need to reverse the bytes, use the IpAddress.HostToNetworkOrder
            ' or IpAddress.NetworkToHostOrder methods to reverse the bytes.

            ' see if we should trim the string a little
            sTemp = textReceived.Text + sTemp

            If (sTemp.Length > 80) Then
                sTemp = sTemp.Substring(sTemp.Length - 80, 80)
            End If

            ' update the text box
            textReceived.Text = sTemp
        End If

        ' check if a transmit is requested
        If (bTransmit) Then
            ' clear the transmit command
            bTransmit = False

            ' convert string to bytes
            Dim encoding As New System.Text.ASCIIEncoding()
            Dim byaryXmit As Byte() = encoding.GetBytes(textTransmitted.Text)
            textTransmitted.Text = ""

            ' send the string
            objTcpClient.Send(byaryXmit, byaryXmit.Length)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        textReceived.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        bTransmit = True
    End Sub
End Class
