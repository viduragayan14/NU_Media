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

    Public Function InsertOrUpdateValue(ByVal counterShortName As String, ByVal newValue As Integer) As String
        Try
            ' Check if the record exists
            Dim ds As New DataSet()
            Dim checkQuery As String = "SELECT COUNT(*) FROM ECCounters WHERE short_name = @CounterShortName"
            Dim checkCmd As New SqlCommand(checkQuery, connection)
            checkCmd.Parameters.AddWithValue("@CounterShortName", counterShortName)

            Dim adapter As New SqlDataAdapter(checkCmd)
            adapter.Fill(ds, "RecordCheck")

            Dim recordExists As Boolean = CInt(ds.Tables(0).Rows(0)(0)) > 0

            If recordExists Then
                ' Record exists, perform an update
                Dim updateQuery As String = "UPDATE ECCounters SET Value = @NewValue WHERE short_name = @CounterShortName"
                Dim updateCmd As New SqlCommand(updateQuery, connection)
                updateCmd.Parameters.AddWithValue("@NewValue", newValue)
                updateCmd.Parameters.AddWithValue("@CounterShortName", counterShortName)

                Dim rowsAffected As Integer = updateCmd.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    Return "Value updated successfully."
                Else
                    Return "Failed to update value."
                End If
            Else
                ' Record does not exist, perform an insert
                Dim insertQuery As String = "INSERT INTO ECCounters (short_name, Value) VALUES (@CounterShortName, @NewValue)"
                Dim insertCmd As New SqlCommand(insertQuery, connection)
                insertCmd.Parameters.AddWithValue("@CounterShortName", counterShortName)
                insertCmd.Parameters.AddWithValue("@NewValue", newValue)

                Dim rowsInserted As Integer = insertCmd.ExecuteNonQuery()
                If rowsInserted > 0 Then
                    Return "Value inserted successfully."
                Else
                    Return "Failed to insert value."
                End If
            End If
        Catch ex As Exception
            Return $"Error: {ex.Message}"
        End Try
    End Function


End Class
