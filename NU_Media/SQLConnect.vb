Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Configuration



Public Class SQLConnect
    Public DSdcount, DSstatus As New DataSet
    Public connectionstring As String
    Public connection As SqlConnection

    'Public da As New SqlDataAdapter(cmd)

    Public Function openconnection(ByVal ServerName, ByVal DataBaseName, ByVal DataBaseUser, ByVal Password) As String
        Try

            Dim connectionstring As String
            If ServerName = "" Then
                ServerName = "(local)"
            End If

            If DataBaseName = "" Then
                DataBaseName = "Ecounting"
            End If

            'connectionstring = "Server=" & ServerName & ";database=" & DataBaseName & ";user=sa;Pwd=Aladin1;"
            '& ";Connect Timeout=1"
            connectionstring = "Server=" & ServerName & ";database=" & DataBaseName & ";user=" & DataBaseUser & ";Pwd=" & Password & ";"
            connection = New SqlConnection(connectionstring)

            connection.Close()
            connection.Open()

            Return "Successful"
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    Public Function GetAvailableSpace(ByVal CounterShortName As String) As Integer
        Try
            Dim ds, ds2 As New DataSet()
            Dim str1 As String = "SELECT element_id, Value FROM ECCounters where short_name=" & "'" & CounterShortName & "'"
            Dim cmd As New SqlCommand(str1, connection)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds, "ECCounters")
            Dim counter_element_id As Integer
            Dim AvailableSpace As String
            counter_element_id = ds.Tables(0).Rows(0).Item(0)
            Dim str2 As String = "SELECT Value FROM ECLimits where counter_element_id=" & "'" & counter_element_id & "'"
            Dim cmd2 As New SqlCommand(str2, connection)
            Dim da2 As New SqlDataAdapter(cmd2)
            da2.Fill(ds2, "ECLimits")

            AvailableSpace = ds2.Tables(0).Rows(0).Item(0) - ds.Tables(0).Rows(0).Item(1)

            If AvailableSpace < 0 Then
                AvailableSpace = 0
            ElseIf AvailableSpace = "" Then
                Return -1
            End If

            Return AvailableSpace

        Catch ex As Exception
            'Returning -1 mean database off line
            Dim v As String = ex.Message
            If v = "There is no row at position 0." Then
                Return -1
            Else
                Return -2
            End If
        End Try
    End Function

    
End Class
