Imports OptoMMP2

Module Module1

    Sub Main()
        Dim objMmp As OptoMMP
        objMmp = New OptoMMP()

        Dim i32Result As Integer

        ' open a connection
        i32Result = objMmp.Open("10.192.54.46", 2001, OptoMMP.Connection.UdpIp, 1000, True)
        If i32Result <> 0 Then
            ' connection failed
        End If
        ' disable data freshness
        objMmp.SetFreshness(0)

        Dim aryWriteFloats(256) As Single
        aryWriteFloats(0) = 99999.0
        aryWriteFloats(1) = 90009.0

        Dim aryReadFloats(256) As Single

        Dim aryWriteInts(256) As Integer
        aryWriteInts(0) = 12345
        aryWriteInts(1) = 54321

        Dim aryReadInts(256) As Integer

        ' write floats and integers
        i32Result = objMmp.ScratchpadFloatWrite(aryWriteFloats, 0, 256, 0)
        If i32Result <> 0 Then
            ' fault occurred
        End If
        i32Result = objMmp.ScratchpadI32Write(aryWriteInts, 0, 256, 0)
        If i32Result <> 0 Then
            ' fault occurred
        End If

        ' read floats and integers
        i32Result = objMmp.ScratchpadFloatRead(aryReadFloats, 0, 256, 0)
        If i32Result <> 0 Then
            ' fault occurred
        End If
        i32Result = objMmp.ScratchpadI32Read(aryReadInts, 0, 256, 0)
        If i32Result <> 0 Then
            ' fault occurred
        End If

    End Sub

End Module
