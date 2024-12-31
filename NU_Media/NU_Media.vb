
Imports System
Imports System.Text
Imports System.Drawing
Imports System.IO.Ports
Imports System.Windows.Forms
Imports System.Threading
Imports System.ComponentModel
Imports System.Net.NetworkInformation
Imports System.Net.IPAddress
Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Security.Cryptography
Imports Microsoft.Win32
Imports System.IO.IsolatedStorage
Imports System.Threading.Tasks



Public Class NU_Media

    Public Appversion As String = "3.3 Desktop app"
    Dim ServerName, DataBaseName, DataBaseUser, DataBasePasswordEncrypted, DataBasePassword, DisplayUpdateIntervalSeconds
    Dim LogListLimit, Loglocation, NumberofLogstoHold, LogUpdateinterval, UserPassword, AdminPassword As String 'General Application setup
    Dim LotFullMessageLine1, SendProtocolPrefixLine1, SendProtocolSufixLine1 As String 'Sign Line-1 parameter setup
    Dim EnableSignLine2, LotOpenMessageLine2, AddLotnamewithLotOpenMessageLine2, LotFullMessageLine2, SendProtocolPrefixLine2, SendProtocolSufixLine2 As String     'Sign Line-2 parameter setup
    Dim EnableDirectiontoOpenLots, EnableCustomDirectiontoOpenLots, LotNamesWhereSignsAreInstalled, OpenLotDirectionPrefixLine2, AllLotsFullMessageLine2, OpenLotDirectionSequence As String    'Line-2- Direction messages open lots
    Dim SignNumbers, ComPorts, CounterShortNames, IPaddresses, SignNames As String     'General Sign parameter Setup
    Dim CarloGavazzi As Boolean
    Dim Modbus_TCP_IP, Modbus_TCP_Port, StartRegister, RegisterCount, SlaveIDCount As String 'Carlo Gavazzi parameter Setup'

    'Internal application variables
    ' Public logmessage, Startuplogmessage, Arrlogmessage(), ArrlogCountererrors(), ArrSignStatus(), ArrSignNumbers(), ArrSignNames() As String
    ' Public ArrAvailableSpace(), ArrComPorts(), ArrCounterShortNames(), ArrIPaddresses(), ArrLotNamesWhereSignsAreInstalled(), ArrLotNamesWhereSignsAreInstalledOriginal(), ArrOpenLotDirectionSequence(), ArrTempLotDirection(), ArrDMessage() As String ', ArrLotNamesRelatedToSigns()

    Public ArrLotFullMessageLine1(), ArrSendProtocolPrefixLine1(), ArrSendProtocolSufixLine1() As String 'Sign Line-1 parameter setup
    Public ArrEnableSignLine2(), ArrLotOpenMessageLine2(), ArrAddLotnamewithLotOpenMessageLine2(), ArrLotFullMessageLine2(), ArrSendProtocolPrefixLine2(), ArrSendProtocolSufixLine2() As String     'Sign Line-2 parameter setup
    Public ArrEnableDirectiontoOpenLots(), ArrEnableCustomDirectiontoOpenLots(), ArrLotNamesWhereSignsAreInstalled(), ArrOpenLotDirectionPrefixLine2(), ArrAllLotsFullMessageLine2(), ArrOpenLotDirectionSequence() As String    'Line-2- Direction messages open lots
    Public Arrlogmessage(), ArrlogCountererrors(), ArrSignStatus(), ArrSignNumbers(), ArrSignNames() As String


    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dim frm As Frmpassword = New Frmpassword
        f = frm
        f.Show()
    End Sub

    Public ArrAvailableSpace(), ArrComPorts(), ArrCounterShortNames(), ArrIPaddresses(), ArrLotNamesWhereSignsAreInstalledOriginal(), ArrTempLotDirection(), ArrDMessage() As String ', ArrLotNamesRelatedToSigns()
    Public logmessage, Startuplogmessage As String


    Public tmrpollingcount, Statuscount, GeneralCounter As Integer
    Public Passwordentered As Boolean = False
    Dim Displays, SBlue, SRedGreen, Line1D, Line2D, Lbl2D, Serialports As New Collection
    Dim f As Frmpassword = New Frmpassword
    Dim frmrestartload As FrmRestart = New FrmRestart
    Dim DatabaseConnect As String
    Dim SQL As New SQLConnect
    Dim Myport As Array

    Dim cts As New CancellationTokenSource()
    Dim cancellationToken As CancellationToken = cts.Token


    Private Sub NU_Media_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try


            LoadParameters() 'Load parameter from appconfig
            AddArrayObjects() 'Put the 10 starting Sign objects in an array
            FillSetupTap()  'It just fills up the Setup tap on the main window
            FillParameterSetuTap() 'It just fills up the FillParameterSetu tap on the main window
            CheckOpenComPorts() 'Check Comport followed by ping to determine which Signs are online

            StartApplication() 'Start application - which will start the  timer
            CG_Connect()
        Catch ex As Exception
            Log("Error from NU_Media_Load Function-->" & ex.Message)
        End Try
    End Sub

    Private Sub LoadParameters()
        Try
            'General Application setup
            ServerName = System.Configuration.ConfigurationManager.AppSettings("ServerName")
            DataBaseName = System.Configuration.ConfigurationManager.AppSettings("DataBaseName")
            DataBaseUser = System.Configuration.ConfigurationManager.AppSettings("DataBaseUser")
            DataBasePasswordEncrypted = System.Configuration.ConfigurationManager.AppSettings("DataBasePasswordEncrypted")
            DataBasePassword = Decrypt(DataBasePasswordEncrypted) 'Decrypt password
            DisplayUpdateIntervalSeconds = System.Configuration.ConfigurationManager.AppSettings("DisplayUpdateIntervalSeconds")

            LogListLimit = System.Configuration.ConfigurationManager.AppSettings("LogListLimit")
            Loglocation = System.Configuration.ConfigurationManager.AppSettings("LogLocation")
            NumberofLogstoHold = System.Configuration.ConfigurationManager.AppSettings("NumberofLogstoHold")
            LogUpdateinterval = System.Configuration.ConfigurationManager.AppSettings("LogUpdateinterval")
            UserPassword = System.Configuration.ConfigurationManager.AppSettings("UserPassword")
            AdminPassword = System.Configuration.ConfigurationManager.AppSettings("AdminPassword")

            'General Sign parameter Setup
            SignNumbers = System.Configuration.ConfigurationManager.AppSettings("SignNumbers")
            SignNames = System.Configuration.ConfigurationManager.AppSettings("SignNames")
            ComPorts = System.Configuration.ConfigurationManager.AppSettings("ComPorts")
            CounterShortNames = System.Configuration.ConfigurationManager.AppSettings("CounterShortNames")
            IPaddresses = System.Configuration.ConfigurationManager.AppSettings("IPadresses")

            'Carlo Gavazzi Parameter Setup
            CarloGavazzi = System.Configuration.ConfigurationManager.AppSettings("CarloGavazzi")
            Modbus_TCP_IP = System.Configuration.ConfigurationManager.AppSettings("Modbus_TCP_IP")
            Modbus_TCP_Port = System.Configuration.ConfigurationManager.AppSettings("Modbus_TCP_Port")
            StartRegister = System.Configuration.ConfigurationManager.AppSettings("StartRegister")
            RegisterCount = System.Configuration.ConfigurationManager.AppSettings("RegisterCount")
            SlaveIDCount = System.Configuration.ConfigurationManager.AppSettings("SlaveIDCount")

            'Sign Line-1 parameter setup
            LotFullMessageLine1 = System.Configuration.ConfigurationManager.AppSettings("LotFullMessageLine1")
            SendProtocolPrefixLine1 = System.Configuration.ConfigurationManager.AppSettings("SendProtocolPrefixLine1")
            SendProtocolSufixLine1 = System.Configuration.ConfigurationManager.AppSettings("SendProtocolSufixLine1")

            'Sign Line-2 parameter setup
            EnableSignLine2 = System.Configuration.ConfigurationManager.AppSettings("EnableSignLine2")
            LotOpenMessageLine2 = System.Configuration.ConfigurationManager.AppSettings("LotOpenMessageLine2")
            AddLotnamewithLotOpenMessageLine2 = System.Configuration.ConfigurationManager.AppSettings("AddLotnamewithLotOpenMessageLine2")
            LotFullMessageLine2 = System.Configuration.ConfigurationManager.AppSettings("LotFullMessageLine2")
            SendProtocolPrefixLine2 = System.Configuration.ConfigurationManager.AppSettings("SendProtocolPrefixLine2")
            SendProtocolSufixLine2 = System.Configuration.ConfigurationManager.AppSettings("SendProtocolSufixLine2")

            'Line-2- Direction messages open lots
            EnableDirectiontoOpenLots = System.Configuration.ConfigurationManager.AppSettings("EnableDirectiontoOpenLots")
            EnableCustomDirectiontoOpenLots = System.Configuration.ConfigurationManager.AppSettings("EnableCustomDirectiontoOpenLots")
            LotNamesWhereSignsAreInstalled = System.Configuration.ConfigurationManager.AppSettings("LotNamesWhereSignsAreInstalled")
            OpenLotDirectionPrefixLine2 = System.Configuration.ConfigurationManager.AppSettings("OpenLotDirectionPrefixLine2")
            AllLotsFullMessageLine2 = System.Configuration.ConfigurationManager.AppSettings("AllLotsFullMessageLine2")
            OpenLotDirectionSequence = System.Configuration.ConfigurationManager.AppSettings("OpenLotDirectionSequence")

            'Put inputs an array that came from appconfig into 
            ArrSignNumbers = Split(SignNumbers, ";")
            ArrSignNames = Split(SignNames, ";")
            ArrComPorts = Split(ComPorts, ";")
            ArrCounterShortNames = Split(CounterShortNames, ";")
            ArrIPaddresses = Split(IPaddresses, ";")

            ArrLotFullMessageLine1 = Split(LotFullMessageLine1, ";")
            ArrSendProtocolPrefixLine1 = Split(SendProtocolPrefixLine1, ";")
            ArrSendProtocolSufixLine1 = Split(SendProtocolSufixLine1, ";")

            ArrEnableSignLine2 = Split(EnableSignLine2, ";")
            ArrLotOpenMessageLine2 = Split(LotOpenMessageLine2, ";")
            ArrAddLotnamewithLotOpenMessageLine2 = Split(AddLotnamewithLotOpenMessageLine2, ";")
            ArrLotFullMessageLine2 = Split(LotFullMessageLine2, ";")
            ArrSendProtocolPrefixLine2 = Split(SendProtocolPrefixLine2, ";")
            ArrSendProtocolSufixLine2 = Split(SendProtocolSufixLine2, ";")

            ArrEnableDirectiontoOpenLots = Split(EnableDirectiontoOpenLots, ";")
            ArrEnableCustomDirectiontoOpenLots = Split(EnableCustomDirectiontoOpenLots, ";")
            ArrLotNamesWhereSignsAreInstalled = Split(LotNamesWhereSignsAreInstalled, ";")
            ArrOpenLotDirectionPrefixLine2 = Split(OpenLotDirectionPrefixLine2, ";")
            ArrAllLotsFullMessageLine2 = Split(AllLotsFullMessageLine2, ";")
            ArrOpenLotDirectionSequence = Split(OpenLotDirectionSequence, ";")
            'ArrLotNamesWhereSignsAreInstalledOriginal = Split(LotNamesWhereSignsAreInstalledOriginal, ";")

            'Redim all arrays to match their size with ArrSignNumbers
            ReDim Preserve ArrSignNames(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrComPorts(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrCounterShortNames(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrIPaddresses(ArrSignNumbers.Length - 1)


            ReDim Preserve ArrLotFullMessageLine1(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrSendProtocolPrefixLine1(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrSendProtocolSufixLine1(ArrSignNumbers.Length - 1)

            ReDim Preserve ArrEnableSignLine2(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrLotOpenMessageLine2(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrAddLotnamewithLotOpenMessageLine2(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrLotFullMessageLine2(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrSendProtocolPrefixLine2(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrSendProtocolSufixLine2(ArrSignNumbers.Length - 1)

            ReDim Preserve ArrEnableDirectiontoOpenLots(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrEnableCustomDirectiontoOpenLots(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrLotNamesWhereSignsAreInstalled(ArrSignNumbers.Length - 1)
            'ReDim Preserve ArrLotNamesWhereSignsAreInstalledOriginal(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrOpenLotDirectionPrefixLine2(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrAllLotsFullMessageLine2(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrOpenLotDirectionSequence(ArrSignNumbers.Length - 1)

            ReDim Preserve ArrSignStatus(ArrSignNumbers.Length - 1)
            ReDim Preserve Arrlogmessage(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrlogCountererrors(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrAvailableSpace(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrDMessage(ArrSignNumbers.Length - 1)
            ReDim Preserve ArrTempLotDirection(ArrSignNumbers.Length - 1)

        Catch ex As Exception
            Log("Error from LoadParameters Function-->" & ex.Message)
        End Try
    End Sub

    Private Sub AddArrayObjects()

        'This function activates the objects (Sign object main window based on ArrSignNumbers configured in the xml file

        Try
            Displays.Add(Display1) : Displays.Add(Display2) : Displays.Add(Display3) : Displays.Add(Display4) : Displays.Add(Display5)
            Displays.Add(Display6) : Displays.Add(Display7) : Displays.Add(Display8) : Displays.Add(Display9) : Displays.Add(Display10)
            SBlue.Add(Sblue1) : SBlue.Add(Sblue2) : SBlue.Add(Sblue3) : SBlue.Add(Sblue4) : SBlue.Add(Sblue5)
            SBlue.Add(Sblue6) : SBlue.Add(Sblue7) : SBlue.Add(Sblue8) : SBlue.Add(Sblue9) : SBlue.Add(Sblue10)
            SRedGreen.Add(SRedGreen1) : SRedGreen.Add(SRedGreen2) : SRedGreen.Add(SRedGreen3) : SRedGreen.Add(SRedGreen4) : SRedGreen.Add(SRedGreen5)
            SRedGreen.Add(SRedGreen6) : SRedGreen.Add(SRedGreen7) : SRedGreen.Add(SRedGreen8) : SRedGreen.Add(SRedGreen9) : SRedGreen.Add(SRedGreen10)
            Line1D.Add(Line1D1) : Line1D.Add(Line1D2) : Line1D.Add(Line1D3) : Line1D.Add(Line1D4) : Line1D.Add(Line1D5)
            Line1D.Add(Line1D6) : Line1D.Add(Line1D7) : Line1D.Add(Line1D8) : Line1D.Add(Line1D9) : Line1D.Add(Line1D10)
            Line2D.Add(Line2D1) : Line2D.Add(Line2D2) : Line2D.Add(Line2D3) : Line2D.Add(Line2D4) : Line2D.Add(Line2D5)
            Line2D.Add(Line2D6) : Line2D.Add(Line2D7) : Line2D.Add(Line2D8) : Line2D.Add(Line2D9) : Line2D.Add(Line2D10)
            Lbl2D.Add(Lbl2D1) : Lbl2D.Add(Lbl2D2) : Lbl2D.Add(Lbl2D3) : Lbl2D.Add(Lbl2D4) : Lbl2D.Add(Lbl2D5)
            Lbl2D.Add(Lbl2D6) : Lbl2D.Add(Lbl2D7) : Lbl2D.Add(Lbl2D8) : Lbl2D.Add(Lbl2D9) : Lbl2D.Add(Lbl2D10)
            Serialports.Add(SerialPort1) : Serialports.Add(SerialPort2) : Serialports.Add(SerialPort3) : Serialports.Add(SerialPort4) : Serialports.Add(SerialPort5)
            Serialports.Add(SerialPort6) : Serialports.Add(SerialPort7) : Serialports.Add(SerialPort8) : Serialports.Add(SerialPort9) : Serialports.Add(SerialPort10)

        Catch ex As Exception
            Log("Error from AddArrayObjects Function-->" & ex.Message)
            Log("Make sure the quantity of sign numbers configures matchs to the quantity of SignNames, ComPorts, IPaddresses & CounterShortNames")
        End Try
    End Sub


    Private Sub FillSetupTap()

        'This function just fills up the Setup tap on the main window

        Try
            'Dim arraylist As New ArrayList
            Dim dt As New DataTable
            Dim j As Integer


            'DGVsetup.Rows.Clear()
            'DGVsetup.Columns.Clear()
            'DGVsetup.Refresh()
            DGVsetup.DataSource = Nothing

            'ArrayList.Add(ArrSignNumbers)
            'arraylist.Add(ArrSignNames)
            dt.Columns.Add("SignNumber")
            dt.Columns.Add("SignName")
            dt.Columns.Add("ComPort")
            dt.Columns.Add("IPaddress")
            dt.Columns.Add("CounterName")

            For j = 0 To ArrSignNumbers.Length - 1
                Dim dr As DataRow
                dr = dt.NewRow()
                dr.Item(0) = ArrSignNumbers(j)
                dr.Item(1) = ArrSignNames(j)
                dr.Item(2) = ArrComPorts(j)
                dr.Item(3) = ArrIPaddresses(j)
                dr.Item(4) = ArrCounterShortNames(j)
                dt.Rows.Add(dr)
                Log("Sign Configuration-->" & dr.Item(0) & "-->" & dr.Item(1) & "-" & dr.Item(2) & "-" & dr.Item(3) & "-" & dr.Item(4))
                dr = Nothing
            Next
            DGVsetup.DataSource = dt
            j = Nothing
            dt = Nothing
            j = Nothing
            ' arraylist = Nothing
        Catch ex As Exception
            Log("Error from FillSetup Function-->" & ex.Message)
            Log("Make sure the quantity of sign numbers configures matchs to the quantity of SignNames, ComPorts, IPaddresses & CounterShortNames")
        End Try
    End Sub

    Private Sub FillParameterSetuTap()

        Try

            TxtServerName.Text = ServerName
            TxtDatabaseName.Text = DataBaseName
            TxtLogListLimit.Text = LogListLimit
            TxtLogLocation.Text = Loglocation
            TxtNorOfLogsToHold.Text = NumberofLogstoHold
            TxtLogUpdateInterval.Text = LogUpdateinterval

            TxtLotFullMessageLine1.Text = LotFullMessageLine1
            TxtSendProtocolPrefixLine1.Text = SendProtocolPrefixLine1
            TxtSendProtocolSufixLine1.Text = SendProtocolSufixLine1


            TxtLotOpenMessageLine2.Text = LotOpenMessageLine2
            TxtLotFullMessageLine2.Text = LotFullMessageLine2
            TxtSendProtocolPrefixLine2.Text = SendProtocolPrefixLine2
            TxtSendProtocolSufixLine2.Text = SendProtocolSufixLine2

            TxtOpenLotDirectionPrefix.Text = OpenLotDirectionPrefixLine2
            TxtAllLotsFullMessage.Text = AllLotsFullMessageLine2


            If (UCase(EnableDirectiontoOpenLots) = "TRUE" Or EnableSignLine2 = "1") Then
                ChkEnableDirectionToOpenLots.Checked = True
                TxtOpenLotDirectionPrefix.Enabled = True
                TxtAllLotsFullMessage.Enabled = True
                TxtLotFullMessageLine2.Enabled = False
            Else
                ChkEnableDirectionToOpenLots.Checked = False
                TxtOpenLotDirectionPrefix.Enabled = False
                TxtAllLotsFullMessage.Enabled = False
                If ChkEnableLine2.Checked = True Then
                    TxtLotFullMessageLine2.Enabled = True
                End If
            End If

            If (UCase(EnableSignLine2) = "TRUE" Or EnableSignLine2 = "1") Then
                ChkEnableLine2.Checked = True
                GboxLine2.Enabled = True
                If ChkEnableDirectionToOpenLots.Checked = False Then
                    TxtLotFullMessageLine2.Enabled = True
                Else
                    TxtLotFullMessageLine2.Enabled = False
                End If
            Else
                GboxLine2.Enabled = False
                ChkEnableLine2.Checked = False
            End If

            CombSignNumber.Items.Clear()
            CombSignNumber.Items.AddRange(ArrSignNumbers)
            CombSignNumber.Text = ArrSignNumbers(0)

        Catch ex As Exception

        End Try
    End Sub


    Private Sub CheckOpenComPorts()

        'This function checks Comport followed by ping to determine which Signs are online

        Try
            Dim i, j, a As Integer

            'Get all open comports and put the list in portComboBox which is located on the main window
            Myport = IO.Ports.SerialPort.GetPortNames
            portComboBox.Items.Clear()
            portComboBox.Items.AddRange(Myport)


            'Check if the comports configured in the XML file are in the portComboBox list
            For i = 0 To ArrSignNumbers.Length - 1
                For j = 0 To portComboBox.Items.Count - 1
                    If portComboBox.Items.Item(j) = ArrComPorts(i) Then
                        'If the comport is available then ping the IP address associated with the comport to determine if Com# much with the IP of the lantronix
                        a = i
                        Dim trd As System.Threading.Thread = New System.Threading.Thread(Sub() ping(a))
                        trd.IsBackground = True
                        trd.Start()
                        System.Threading.Thread.Sleep(100)
                        a = Nothing
                    End If
                Next j
            Next i

            i = Nothing
            j = Nothing
        Catch ex As Exception
            Log("Error from CheckOpenComPorts Function-->" & ex.Message)
            Log("Make sure the quantity of sign numbers configures matchs to the quantity of ComPorts. & Make sure the comports configured are valid")
        End Try
    End Sub

    Private Sub ping(ByVal i As Integer)

        'This function pings IP address of Sign and it is always triggered by function CheckOpenComPorts()

        Try
            Dim bstrIPAddress As String = ArrIPaddresses(i)
            Dim IPAddressForTest As System.Net.IPAddress = System.Net.IPAddress.Parse(bstrIPAddress)
            'IPAddressForTest= System.Net.IPAddress.Parse(bstrIPAddress)
            Dim ping As Ping = New Ping
            Dim reply As PingReply = ping.Send(IPAddressForTest)

            'If ping is success then that means the comport & Sign are online 
            If reply.Status = IPStatus.Success Then
                ArrSignStatus(i) = 1
            Else
                ArrSignStatus(i) = 0
            End If

            i = Nothing
            IPAddressForTest = Nothing
            ping = Nothing
            reply = Nothing
            bstrIPAddress = Nothing

        Catch ex As Exception
            'Log("Error from SendToDisplay Function-->" & ex.Message)
            ' Log("Make sure the quantity of sign numbers configures matchs to the quantity of IPaddresses. And Make sure the IPaddresses configured are valid")
        End Try
    End Sub

    Private Sub StartApplication()
        Try
            Dim i As Integer
            ' Me.Text = Appversion
            'set TxtTimeversion & LogListView on the main window. LogListView is to view the logs that will be saved afterwards into the log txt file
            TxtTimeversion.Text = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") & "   ----- Version - " & Appversion

            LogListView.Clear()
            CarloLogListView.Clear()
            LogListView.Columns.Add("Date-Time", 150, HorizontalAlignment.Left)
            LogListView.Columns.Add("Message", 1000, HorizontalAlignment.Left)
            CarloLogListView.Columns.Add("Date-Time", 150, HorizontalAlignment.Left)
            CarloLogListView.Columns.Add("Message", 1000, HorizontalAlignment.Left)

            Startuplogmessage = vbCrLf & "******************************************************************" & vbCrLf & vbCrLf &
             "VMS -----    Version  " & Appversion & vbCrLf &
             DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") & vbCrLf & vbCrLf &
             "******************************************************************" & vbCrLf
            Log(Startuplogmessage)

            logmessage = "Connecting to " & "  Servername=" & ServerName & "  DatabaseName=" & DataBaseName
            Log(logmessage)

            DatabaseConnect = SQL.openconnection(ServerName, DataBaseName, DataBaseUser, DataBasePassword) 'Connect to Database
            Log("Database connection Status: " & DatabaseConnect)

            'trying to connect to Ecounting database by calling function SQL.openconnection()
            'DatabaseConnect = SQL.openconnection(ServerName, DataBaseName)
            If DatabaseConnect <> "Successful" Then
                Log("Database Connection Error")
                TxtEcountingDB.BackColor = Color.Red
                TxtEcountingDB.Text = "Offline"
            Else
                Log("Database Connection Established")
            End If

            'Diactivate all signs (to be activated on the next for loop)
            For i = 0 To 9
                Displays(i + 1).Visible = False
                Lbl2D(i + 1).Visible = False
                Line2D(i + 1).Visible = False
            Next

            'Activate Signs on main window based on SignNumbers configured in the xml file 
            For i = 0 To ArrSignNumbers.Length - 1
                Displays(i + 1).Visible = True
                Displays(i + 1).Text = "SignNumber " & ArrSignNumbers(i) & "  - " & ArrSignNames(i)
                If (UCase(EnableSignLine2) = "TRUE" Or EnableSignLine2 = "1") Then
                    Lbl2D(i + 1).Visible = True
                    Line2D(i + 1).Visible = True
                End If

                Log("Trying to connect to -->" & Displays(i + 1).Text)
            Next

            i = Nothing

            'Reset SignStatus & logmessage to 0. The arrays values will be updated by timer & checkopencomports
            For i = 0 To ArrSignNumbers.Length - 1
                ArrSignStatus(i) = 0
                Arrlogmessage(i) = 0
                ArrlogCountererrors(i) = 1
            Next i



            'Finally start the timer
            tmrpolling.Enabled = True

            i = Nothing

        Catch ex As Exception
            Log("Error from NU_Media_Load Function-->" & ex.Message)
        End Try
    End Sub

    Public Sub CG_Connect()

        If CarloGavazzi Then
            Task.Run(Async Function()
                         Await CG_Read(cancellationToken)
                     End Function)
        End If

    End Sub

    Public Async Function CG_Read(cancellationToken As CancellationToken) As Task
        While Not cancellationToken.IsCancellationRequested
            Dim CGResult As WSMBT.Result
            WsmbtControl1.LicenseKey("356D-9FEA-7E14-6F9D-0569-FBA0")
            WsmbtControl1.Mode = WSMBT.Mode.TCP_IP
            WsmbtControl1.ConnectTimeout = 1000
            WsmbtControl1.ResponseTimeout = 1000
            CGResult = WsmbtControl1.Connect(Modbus_TCP_IP, Modbus_TCP_Port)

            If CGResult <> WSMBT.Result.SUCCESS Then

                MessageBox.Show(WsmbtControl1.GetLastErrorString())
                CGLog("Device Not Connected ")
                If CGConnect.InvokeRequired Then
                    CGConnect.Invoke(Sub()
                                         CGConnect.BackColor = Color.Red
                                         CGConnect.Text = "Offline"
                                     End Sub)
                End If

            Else
                CGLog("Device Connected ")
                If CGConnect.InvokeRequired Then
                    CGConnect.Invoke(Sub()
                                         CGConnect.BackColor = Color.Lime
                                         CGConnect.Text = "Online"
                                     End Sub)
                End If

            End If

            Dim Registers(254) As Short

            ' Create a mapping of register numbers to labels
            Dim registerLabels As New Dictionary(Of Integer, String) From {
            {4000, "A"}, {4001, "B"}, {4002, "C"}, {4003, "D"}, {4004, "E"},
            {4005, "F"}, {4006, "G"}, {4007, "H"}, {4008, "I"}, {4009, "J"},
            {4010, "K"}, {4011, "L"}, {4012, "M"}, {4013, "N"}, {4014, "O"},
            {4015, "P"}
        }

            ' Declare logMessage at the beginning of the method
            Dim logMessage As String = String.Empty


            Dim totalActiveDevicesAcrossSlaves As Integer = 0 ' Total active devices across all slaves

            Try
                ' Loop through each slave (1 to 16)
                For slaveId As Integer = 1 To SlaveIDCount
                    ' Send request to read registers for the current slave
                    Dim Rslt As WSMBT.Result

                    Rslt = WsmbtControl1.ReadInputRegisters(slaveId, CInt(StartRegister), CInt(RegisterCount), Registers)

                    If Rslt = WSMBT.Result.SUCCESS Then
                        Dim totalActiveDevicesForSlave As Integer = 0 ' Total active devices for this slave

                        ' Process each register for the current slave
                        For t = 0 To CInt(RegisterCount) - 1
                            Dim registerAddress As Integer = CInt(StartRegister) + t
                            Dim registerValue As UShort = WsmbtControl1.RegisterToUInt16(Registers(t))

                            ' Get the label for the register if it exists
                            Dim registerLabel As String = If(registerLabels.ContainsKey(registerAddress), registerLabels(registerAddress), "Unknown")

                            ' Add register and value information to the ListBox and write to log file
                            logMessage = $"Slave #{slaveId} - Reg. # {registerAddress} ({registerLabel}): {registerValue}"
                            CGLog(logMessage)

                            ' Decode the binary representation of the value
                            Dim binaryString As String = Convert.ToString(registerValue, 2).PadLeft(16, "0"c)
                            logMessage = $"Binary: {binaryString}"
                            CGLog(logMessage)

                            ' Check each bit and identify which devices are ON
                            Dim activeDevices As New List(Of Integer)
                            For i As Integer = 0 To 7 ' For 8 devices per register
                                If (registerValue And (1 << i)) <> 0 Then
                                    activeDevices.Add(i + 1) ' Device numbers start from 1
                                End If
                            Next

                            ' Display the active devices for this register and write to log file
                            If activeDevices.Count > 0 Then
                                logMessage = $"Active Devices: {String.Join(", ", activeDevices)}"
                                CGLog(logMessage)
                            Else
                                logMessage = "No devices are ON."
                                CGLog(logMessage)
                            End If

                            ' Add the count of active devices for this register to the slave's total
                            totalActiveDevicesForSlave += activeDevices.Count
                        Next

                        ' Add the total active devices for the current slave
                        logMessage = $"Total Active Devices for Slave #{slaveId}: {totalActiveDevicesForSlave}"
                        CGLog(logMessage)
                        totalActiveDevicesAcrossSlaves += totalActiveDevicesForSlave
                    Else
                        logMessage = $"Error reading registers for Slave #{slaveId}: {Rslt}"
                        CGLog(logMessage)
                    End If

                    ' Close the connection for the current slave
                    WsmbtControl1.Close()
                Next

                ' Display the total number of active devices across all slaves and write to log file
                logMessage = $"Total Active Devices Across All Slaves: {totalActiveDevicesAcrossSlaves}"
                CGLog(logMessage)
            Catch ex As Exception
                ' Log the exception message
                logMessage = "Error: " & ex.Message
                CGLog(logMessage)
            End Try

            Console.WriteLine("CG_Read is running")
            Await Task.Delay(2000)
        End While

        Console.WriteLine("CG_Read has been stopped.")

    End Function

    Public Sub AvailableSpaces()

        'This function is activated by the timer 
        Dim TempLotOpenMessageLine2 As String
        Try
            Dim a, i, AvailableSpace, NumberofOpenlots, Countlots As Integer
            Dim Dmessage As String = OpenLotDirectionPrefixLine2
            Dim msgline1, msgline2, onlinemesage, tempmesg As String

            'Reset available space array. This array is required in the timer to send log information
            For a = 0 To ArrSignNumbers.Length - 1
                ArrAvailableSpace(a) = 0
            Next a

ConfirmDBconnect:
            If DatabaseConnect <> "Successful" Then
                For a = 0 To ArrSignNumbers.Length - 1
                    ArrAvailableSpace(a) = "----"
                Next a
                GoTo SendtoDisplay  'Just jump to SendtoDisplay: to display "----"
            End If

            'If DatabaseConnect = True Then
            ' onlinemesage = LotOpenMessageLine2
            ' Else 'If there is no commenection to database & if the display is online then it will display just "----" on line1 but nothing on line2
            ' For a = 0 To ArrSignNumbers.Length - 1
            ' ArrAvailableSpace(a) = "----"
            ' Next a
            ' onlinemesage = ""
            ' GoTo SendtoDisplay  'Just jump to SendtoDisplay: to display "----"
            ' End If

            For a = 0 To ArrSignNumbers.Length - 1
                AvailableSpace = SQL.GetAvailableSpace(ArrCounterShortNames(a)) 'Get vailable space from database using function SQL.GetAvailableSpace() 
                If AvailableSpace > 0 Then
                    ArrAvailableSpace(a) = AvailableSpace
                ElseIf AvailableSpace = 0 Then
                    'ArrAvailableSpace(a) = LotFullMessageLine1
                    ArrAvailableSpace(a) = ArrLotFullMessageLine1(a)

                ElseIf AvailableSpace = -1 Then
                    ArrAvailableSpace(a) = "----"
                    If ArrlogCountererrors(a) = 1 Then
                        Log("Counter " & ArrCounterShortNames(a) & " for Sign number-->" & ArrSignNumbers(a) & " not found.")
                        ArrlogCountererrors(a) = 0
                    End If
                Else
                    ' DatabaseConnect = False
                    TxtEcountingDB.BackColor = Color.Red
                    TxtEcountingDB.Text = "Offline"
                    GoTo ConfirmDBconnect 'If database connection fails then jump to ConfirmDBconnect
                End If
            Next a

            If (UCase(EnableSignLine2) = "TRUE" Or EnableSignLine2 = "1") Then

                If (UCase(EnableDirectiontoOpenLots) = "TRUE" Or EnableDirectiontoOpenLots = "1") Then
                    'The next three for loops help to determine what needs to be display on line2 of the displays.
                    'This works only if EnableLine2 = "True"
                    For a = 0 To ArrLotNamesWhereSignsAreInstalled.Length - 1  'Determines which signs are syschronized
                        For i = 0 To ArrLotNamesWhereSignsAreInstalled.Length - 1
                            If i <> a And ArrLotNamesWhereSignsAreInstalled(a) = ArrLotNamesWhereSignsAreInstalled(i) Then
                                ArrLotNamesWhereSignsAreInstalled(i) = "0"
                            End If
                        Next i
                    Next a

                    For a = 0 To ArrSignNumbers.Length - 1 'Determine how many lots are open
                        'If ArrAvailableSpace(a) <> LotFullMessageLine1 And ArrAvailableSpace(a) <> "----" And ArrLotNamesWhereSignsAreInstalled(a) <> 0 Then
                        If ArrAvailableSpace(a) <> ArrLotFullMessageLine1(a) And ArrAvailableSpace(a) <> "----" And ArrLotNamesWhereSignsAreInstalled(a) <> 0 Then
                            NumberofOpenlots = NumberofOpenlots + 1
                        End If
                    Next a

                    For a = 0 To ArrSignNumbers.Length - 1 'Make Line2 message of all displays based on the synchronized signs
                        ' If ArrAvailableSpace(a) <> LotFullMessageLine1 And ArrAvailableSpace(a) <> "----" And ArrLotNamesWhereSignsAreInstalled(a) <> 0 Then
                        If ArrAvailableSpace(a) <> ArrLotFullMessageLine1(a) And ArrAvailableSpace(a) <> "----" And ArrLotNamesWhereSignsAreInstalled(a) <> 0 Then
                            Countlots = Countlots + 1
                            If Dmessage = OpenLotDirectionPrefixLine2 Then
                                Dmessage = Dmessage & ArrLotNamesWhereSignsAreInstalled(a)
                            Else
                                If Countlots = NumberofOpenlots And Countlots > 1 Then
                                    Dmessage = Dmessage & " OR " & ArrLotNamesWhereSignsAreInstalled(a)
                                Else
                                    Dmessage = Dmessage & ", " & ArrLotNamesWhereSignsAreInstalled(a)
                                End If
                            End If
                        End If
                    Next a

                    If Dmessage = LotOpenMessageLine2 Then 'If all lots are full then just display the all lots full message
                        Dmessage = AllLotsFullMessageLine2
                    End If

                End If

                If Dmessage = OpenLotDirectionPrefixLine2 Then 'If all lots are full then just display the all lots full message
                    Dmessage = LotFullMessageLine2
                End If

                If (UCase(EnableCustomDirectiontoOpenLots) = "TRUE" Or EnableCustomDirectiontoOpenLots = "1") Then

                    For i = 0 To ArrSignNumbers.Length - 1
                        ArrDMessage(i) = AllLotsFullMessageLine2
                    Next i

                    For i = 0 To ArrSignNumbers.Length - 1
                        tempmesg = ArrOpenLotDirectionSequence(i)
                        ArrTempLotDirection = Split(tempmesg, "-")
                        For j = 0 To ArrTempLotDirection.Length - 1
                            'tempmesg = ArrTempLotDirection(j)
                            For a = 0 To ArrSignNumbers.Length - 1 'Make Line2 message of all displays based on the synchronized signs
                                'If ArrAvailableSpace(a) <> LotFullMessageLine1 And ArrAvailableSpace(a) <> "----" And ArrLotNamesWhereSignsAreInstalled(a) <> 0 And ArrTempLotDirection(j) = ArrLotNamesWhereSignsAreInstalled(a) Then
                                If ArrAvailableSpace(a) <> ArrLotFullMessageLine1(a) And ArrAvailableSpace(a) <> "----" And ArrLotNamesWhereSignsAreInstalled(a) <> 0 And ArrTempLotDirection(j) = ArrLotNamesWhereSignsAreInstalled(a) Then

                                    ArrDMessage(i) = OpenLotDirectionPrefixLine2 & ArrLotNamesWhereSignsAreInstalled(a)
                                    a = ArrSignNumbers.Length
                                    j = ArrTempLotDirection.Length
                                    'i = ArrSignNumbers.Length
                                End If
                            Next a
                        Next j
                    Next i

                Else



                End If



            End If


SendtoDisplay:
            'This is were information will be sent to displays and updates the main window line1D & line2D labels
            For a = 0 To ArrSignNumbers.Length - 1
                i = a
                If ArrSignStatus(i) = 1 Then
                    'If ArrAvailableSpace(i) = LotFullMessageLine1 Then
                    If ArrAvailableSpace(i) = ArrLotFullMessageLine1(i) Then
                        Line1D(i + 1).ForeColor = Color.Red
                        'Line1D(i + 1).text = LotFullMessageLine1
                        Line1D(i + 1).text = ArrLotFullMessageLine1(i)
                        'Line2D(i + 1).text = Dmessage
                        Line2D(i + 1).text = ArrDMessage(i)
                        'Dim trd As System.Threading.Thread = New System.Threading.Thread(Sub() SendToDisplay(i + 1, ArrComPorts(i), LotFullMessageLine1, Dmessage))
                        Dim trd As System.Threading.Thread = New System.Threading.Thread(Sub() SendToDisplay(i + 1, ArrComPorts(i), ArrLotFullMessageLine1(i), ArrDMessage(i)))
                        trd.IsBackground = True
                        trd.Start()
                        System.Threading.Thread.Sleep(50)
                    ElseIf ArrAvailableSpace(i) = "----" Then
                        Line1D(i + 1).ForeColor = Color.Black
                        Line1D(i + 1).text = ArrAvailableSpace(a)
                        Line2D(i + 1).text = ""
                        Dim trd As System.Threading.Thread = New System.Threading.Thread(Sub() SendToDisplay(i + 1, ArrComPorts(i), ArrAvailableSpace(i), ""))
                        trd.IsBackground = True
                        trd.Start()
                        System.Threading.Thread.Sleep(50)
                    Else
                        Line1D(i + 1).ForeColor = Color.Black
                        Line1D(i + 1).text = ArrAvailableSpace(a)

                        If (UCase(AddLotnamewithLotOpenMessageLine2) = "TRUE" Or AddLotnamewithLotOpenMessageLine2 = "1") Then
                            TempLotOpenMessageLine2 = LotOpenMessageLine2 & "LOT " & ArrLotNamesWhereSignsAreInstalledOriginal(i)
                        Else
                            TempLotOpenMessageLine2 = LotOpenMessageLine2
                        End If
                        Line2D(i + 1).text = TempLotOpenMessageLine2
                        Dim trd As System.Threading.Thread = New System.Threading.Thread(Sub() SendToDisplay(i + 1, ArrComPorts(i), ArrAvailableSpace(i), TempLotOpenMessageLine2))
                        trd.IsBackground = True
                        trd.Start()
                        System.Threading.Thread.Sleep(50)
                    End If

                    msgline1 = ArrSendProtocolPrefixLine1(i) & ArrAvailableSpace(i).PadLeft(4) & ArrSendProtocolSufixLine1(i)
                    msgline2 = SendProtocolPrefixLine2 & Line2D(i + 1).text & SendProtocolSufixLine2

                    If (UCase(EnableSignLine2) = "TRUE" Or EnableSignLine2 = "1") Then
                        logmessage = "Sign Online. SignNumber" & ArrSignNumbers(i) & "  Message sento to    Comport:" & ArrComPorts(i) & "---Line1=" & msgline1 & "---Line2=" & msgline2
                    Else
                        logmessage = "Sign Online. SignNumber" & ArrSignNumbers(i) & "  Message sento to    Comport:" & ArrComPorts(i) & "---Line1=" & msgline1
                    End If

                Else
                    Line1D(i + 1).ForeColor = Color.Black
                    Line1D(i + 1).text = " "
                    Line2D(i + 1).text = " "
                    logmessage = "Sign Offline. SignNumber" & ArrSignNumbers(i)
                End If

                If logmessage <> Arrlogmessage(a) Then
                    Log(logmessage)
                    Arrlogmessage(a) = logmessage
                End If

            Next a

            a = Nothing
            i = Nothing
            logmessage = Nothing
            Dmessage = Nothing
            msgline1 = Nothing
            msgline2 = Nothing
            logmessage = Nothing
            onlinemesage = Nothing
            AvailableSpace = Nothing
            NumberofOpenlots = Nothing
            Countlots = Nothing



        Catch ex As Exception
            Log("Error from AvailableSpaces Function-->" & ex.Message)
            Log("Make sure the quantity of sign numbers configures matchs to the quantity of SignNames, ComPorts, IPaddresses & CounterShortNames")
        End Try
    End Sub

    Private Sub SendToDisplay(ByVal signnumber As Integer, ByVal Comport As String, ByVal Line1 As String, ByVal Line2 As String)

        'This function is activated by the AvailableSpaces() function. The function just check if the comport is open and it send messages 

        Try
            Dim msgline1, msgline2 As String

            If Serialports(signnumber).IsOpen = False Then
                Serialports(signnumber).PortName = Comport
                Serialports(signnumber).Open()
                Exit Sub
            End If

            msgline1 = ArrSendProtocolPrefixLine1(signnumber - 1) & Line1.PadLeft(4) & ArrSendProtocolSufixLine1(signnumber - 1)
            Serialports(signnumber).Write(msgline1)

            If (UCase(EnableSignLine2) = "TRUE" Or EnableSignLine2 = "1") Then
                msgline2 = SendProtocolPrefixLine2 & Line2 & SendProtocolSufixLine2
                Serialports(signnumber).Write(msgline2)
            End If

            msgline1 = Nothing
            msgline2 = Nothing

        Catch ex As Exception
            Log("Error from SendToDisplay-->" & ex.Message)
        End Try

    End Sub


    Private Sub btnExtendHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtendHide.Click

        'This is triggered by clicking btnExtendHide button. It extends the main form window if customer entes the righ password

        Try

            If btnExtendHide.Text = "Hide Extended" Then
                'Passwordentered = True
                'GeneralCounter = 0
                Me.Size = New Size(500, Me.Height)
                btnExtendHide.Text = "Show Extended"
                'ElseIf btnExtendHide.Text = "Show Extended" And Passwordentered = True Then
                'Me.Size = New Size(1100, Me.Height)
                ' btnExtendHide.Text = "Hide Extended"
            Else
                f.Close()
                Dim frm As Frmpassword = New Frmpassword
                f = frm
                f.Show()
                f.Text = "PasswordCheck - Extend"
                f.Location = Me.Location
            End If
        Catch ex As Exception
            Log("Error from btnExtendHide-->" & ex.Message)
        End Try
    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Try
            f.Close()
            Dim frm As Frmpassword = New Frmpassword
            f = frm
            f.Show()
            f.Text = "PasswordCheck - Exit"
            f.Location = Me.Location
            cancellationTokenSource.Cancel()
        Catch ex As Exception
            Log("Error from btnExit-->" & ex.Message)
        End Try
    End Sub
    Private Sub NU_Media_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        'This is triggered by clicking CLOSE(X) button. It terminates the application after confirmation

        Try
            e.Cancel = True
            f.Close()
            Dim frm As Frmpassword = New Frmpassword
            f = frm
            f.Show()
            f.Text = "PasswordCheck - Exit"
            f.Location = Me.Location

        Catch ex As Exception
            Log("Error from btnExit_Click-->" & ex.Message)
        End Try

    End Sub








    Private Sub ChkEnableLine2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkEnableLine2.CheckedChanged
        Try
            If ChkEnableLine2.Checked = True Then
                GboxLine2.Enabled = True
                If ChkEnableDirectionToOpenLots.Checked = False Then
                    TxtLotFullMessageLine2.Enabled = True
                Else
                    TxtLotFullMessageLine2.Enabled = False
                End If
            Else
                GboxLine2.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ChkEnableDirectionToOpenLots_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkEnableDirectionToOpenLots.CheckedChanged
        Try
            If ChkEnableDirectionToOpenLots.Checked = True Then
                TxtOpenLotDirectionPrefix.Enabled = True
                TxtAllLotsFullMessage.Enabled = True
                TxtLotFullMessageLine2.Enabled = False

            Else

                'TxtOpenLotDirectionPrefix.Text = ""
                'TxtAllLotsFullMessage.Text = ""
                TxtOpenLotDirectionPrefix.Enabled = False
                TxtAllLotsFullMessage.Enabled = False
                If ChkEnableLine2.Checked = True Then
                    TxtLotFullMessageLine2.Enabled = True
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub btnsaveappsetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsaveappsetup.Click
        Try

            Dim Answer As MsgBoxResult
            Dim configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            Answer = MsgBox("Are you sure you want to save the changes?", MessageBoxButtons.YesNo + MessageBoxIcon.Question, "VMS")
            If Answer = MsgBoxResult.Yes Then

                settings("LogListLimit").Value = TxtLogListLimit.Text
                settings("ServerName").Value = TxtServerName.Text
                settings("DataBaseName").Value = TxtDatabaseName.Text
                settings("LogListLimit").Value = TxtLogListLimit.Text

                settings("Loglocation").Value = TxtLogLocation.Text

                settings("NumberofLogstoHold").Value = TxtNorOfLogsToHold.Text
                settings("LogUpdateinterval").Value = TxtLogUpdateInterval.Text
                settings("LotFullMessageLine1").Value = TxtLotFullMessageLine1.Text
                settings("SendProtocolPrefixLine1").Value = TxtSendProtocolPrefixLine1.Text
                settings("SendProtocolSufixLine1").Value = TxtSendProtocolSufixLine1.Text

                If ChkEnableLine2.Checked = True Then
                    settings("EnableSignLine2").Value = "True"
                Else : settings("EnableSignLine2").Value = "False"
                End If

                If TxtLotOpenMessageLine2.Enabled = True Then settings("LotOpenMessageLine2").Value = TxtLotOpenMessageLine2.Text
                If TxtLotFullMessageLine2.Enabled = True Then settings("LotFullMessageLine2").Value = TxtLotFullMessageLine2.Text
                If TxtSendProtocolPrefixLine2.Enabled = True Then settings("SendProtocolPrefixLine2").Value = TxtSendProtocolPrefixLine2.Text
                If TxtSendProtocolSufixLine2.Enabled = True Then settings("SendProtocolSufixLine2").Value = TxtSendProtocolSufixLine2.Text

                If ChkEnableDirectionToOpenLots.Enabled = True And ChkEnableDirectionToOpenLots.Checked = True Then
                    settings("EnableDirectiontoOpenLots").Value = "True"
                Else : settings("EnableDirectiontoOpenLots").Value = "False"
                End If

                If TxtOpenLotDirectionPrefix.Enabled = True Then settings("OpenLotDirectionPrefixLine2").Value = TxtOpenLotDirectionPrefix.Text
                If TxtAllLotsFullMessage.Enabled = True Then settings("AllLotsFullMessageLine2").Value = TxtAllLotsFullMessage.Text

                configFile.Save(System.Configuration.ConfigurationSaveMode.Modified)
                System.Configuration.ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
            ElseIf Answer = MsgBoxResult.No Then
                Exit Sub
            End If

            MsgBox("All changes saved.", MessageBoxButtons.YesNo + MessageBoxIcon.Information, "VMS")

        Catch ex As Exception
            Log("Error from AvailableSpaces Function-->" & ex.Message)
        End Try
    End Sub

    Private Sub Log(ByVal logmessage As String)

        Try
            Dim item As New ListViewItem
            Dim arr(2), LogFile, TemplogLocation As String
            Dim file As System.IO.StreamWriter
            Dim i As Integer
            Dim currentdirectory As String = My.Application.Info.DirectoryPath

            'display latest information in Loglistview of the Main Window
            arr(0) = DateTime.Now.Date & "  " & DateTime.Now.ToLongTimeString
            arr(1) = logmessage
            item = New ListViewItem(arr)

            LogListView.Items.Insert(0, item)

            'Don't display items in loglistview if they are over the max limit of row allowed to be displayed. Max limit defined in table DisplayProp 
            If LogListView.Items.Count >= LogListLimit Then
                For i = LogListLimit To LogListView.Items.Count
                    LogListView.Items.RemoveAt(LogListView.Items.Count - 1)
                Next
                arr(0) = "------->"
                arr(1) = "For older information check the log file in---- " & Loglocation
                item = New ListViewItem(arr)
                LogListView.Items.Add(item)
            End If

            TemplogLocation = Loglocation
            If TemplogLocation = "" Then
                TemplogLocation = currentdirectory & "\Log\"
            End If

            'If the LogLocation directory doesn't exist then create it
            If (Not System.IO.Directory.Exists(TemplogLocation)) Then
                System.IO.Directory.CreateDirectory(TemplogLocation)
            End If



            'Create a log file,if it doesn't exist, in the above location as DisplayLog-date
            ' Loglocation = Loglocation & "DisplayLog-" & DateTime.Now.ToLongDateString & ".txt"
            LogFile = TemplogLocation & "DisplayLog-" & DateTime.Now.ToString("yyyy-MMM-dd") & ".txt"

            If System.IO.File.Exists(LogFile) = False Then
                logmessage = Startuplogmessage
            End If

            If logmessage = Startuplogmessage Then
                file = My.Computer.FileSystem.OpenTextFileWriter(LogFile, True)
                file.WriteLine(logmessage)
                file.Flush()
                file.Close()
                file = Nothing

            Else
                file = My.Computer.FileSystem.OpenTextFileWriter(LogFile, True)
                file.WriteLine(DateTime.Now.Date & "  " & DateTime.Now.ToLongTimeString & "---> " & logmessage)
                file.Flush()
                file.Close()
                file = Nothing
            End If


            'Delete files older 
            Dim directory As New IO.DirectoryInfo(TemplogLocation)
            For Each files As IO.FileInfo In directory.GetFiles
                Dim FilenameLen As Integer = Len(files.Name)
                Dim filename = Microsoft.VisualBasic.Left(files.Name, 10)
                'Dim x As Integer = (Now - files.LastWriteTime).Days
                'If (NumberofLogstoHold > 0) And (Now - files.CreationTime).Days > NumberofLogstoHold And filename = "DisplayLog" And FilenameLen = 25 Then
                If (NumberofLogstoHold > 0) And (Now - files.LastWriteTime).Days >= NumberofLogstoHold And filename = "DisplayLog" And FilenameLen = 26 Then
                    files.Delete()
                End If
                FilenameLen = Nothing
                filename = Nothing
            Next

            logmessage = Nothing
            file = Nothing
            item = Nothing
            arr = Nothing
            logmessage = Nothing
            currentdirectory = Nothing
            directory = Nothing
            LogFile = Nothing
            i = Nothing

        Catch ex As Exception

        End Try
    End Sub

    Private Sub CGLog(ByVal logmessage As String)
        Try
            Dim item As New ListViewItem
            Dim arr(2), LogFile, TemplogLocation As String
            Dim file As System.IO.StreamWriter
            Dim i As Integer
            Dim currentdirectory As String = My.Application.Info.DirectoryPath

            ' Check if we need to invoke to the UI thread
            If CarloLogListView.InvokeRequired Then
                CarloLogListView.Invoke(Sub()
                                            ' display latest information in LogListView of the Main Window
                                            arr(0) = DateTime.Now.Date & "  " & DateTime.Now.ToLongTimeString
                                            arr(1) = logmessage
                                            item = New ListViewItem(arr)

                                            CarloLogListView.Items.Insert(0, item)

                                            ' Don't display items in loglistview if they are over the max limit of rows allowed to be displayed
                                            If CarloLogListView.Items.Count >= LogListLimit Then
                                                For i = LogListLimit To CarloLogListView.Items.Count
                                                    CarloLogListView.Items.RemoveAt(CarloLogListView.Items.Count - 1)
                                                Next
                                                arr(0) = "------->"
                                                arr(1) = "For older information check the log file in---- " & Loglocation
                                                item = New ListViewItem(arr)
                                                CarloLogListView.Items.Add(item)
                                            End If
                                        End Sub)
            Else
                ' If on the UI thread, directly update the ListView
                arr(0) = DateTime.Now.Date & "  " & DateTime.Now.ToLongTimeString
                arr(1) = logmessage
                item = New ListViewItem(arr)
                CarloLogListView.Items.Insert(0, item)

                ' Don't display items in loglistview if they are over the max limit of rows allowed to be displayed
                If CarloLogListView.Items.Count >= LogListLimit Then
                    For i = LogListLimit To CarloLogListView.Items.Count
                        CarloLogListView.Items.RemoveAt(CarloLogListView.Items.Count - 1)
                    Next
                    arr(0) = "------->"
                    arr(1) = "For older information check the log file in---- " & Loglocation
                    item = New ListViewItem(arr)
                    CarloLogListView.Items.Add(item)
                End If
            End If

            TemplogLocation = Loglocation
            If TemplogLocation = "" Then
                TemplogLocation = currentdirectory & "\Log\"
            End If

            ' If the LogLocation directory doesn't exist then create it
            If (Not System.IO.Directory.Exists(TemplogLocation)) Then
                System.IO.Directory.CreateDirectory(TemplogLocation)
            End If

            ' Create a log file if it doesn't exist, in the above location as DisplayLog-date
            LogFile = TemplogLocation & "CarloGavazziLog-" & DateTime.Now.ToString("yyyy-MMM-dd") & ".txt"

            If System.IO.File.Exists(LogFile) = False Then
                logmessage = Startuplogmessage
            End If

            If logmessage = Startuplogmessage Then
                file = My.Computer.FileSystem.OpenTextFileWriter(LogFile, True)
                file.WriteLine(logmessage)
                file.Flush()
                file.Close()
                file = Nothing
            Else
                file = My.Computer.FileSystem.OpenTextFileWriter(LogFile, True)
                file.WriteLine(DateTime.Now.Date & "  " & DateTime.Now.ToLongTimeString & "---> " & logmessage)
                file.Flush()
                file.Close()
                file = Nothing
            End If

            ' Delete files older than the specified number of days
            Dim directory As New IO.DirectoryInfo(TemplogLocation)
            For Each files As IO.FileInfo In directory.GetFiles
                Dim FilenameLen As Integer = Len(files.Name)
                Dim filename = Microsoft.VisualBasic.Left(files.Name, 10)
                If (NumberofLogstoHold > 0) And (Now - files.LastWriteTime).Days >= NumberofLogstoHold And filename = "CarloGavazziLog" And FilenameLen = 29 Then
                    files.Delete()
                End If
            Next

            ' Clear unused variables
            logmessage = Nothing
            file = Nothing
            item = Nothing
            arr = Nothing
            currentdirectory = Nothing
            directory = Nothing
            LogFile = Nothing
            i = Nothing
        Catch ex As Exception
            ' Handle any errors here if needed (e.g., log or display a message)
            Console.WriteLine("Error in CGLog: " & ex.Message)
        End Try
    End Sub

    Private Sub tmrpolling_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrpolling.Tick

        Try
            Dim i As Integer
            If (tmrpollingcount >= 1 And tmrpollingcount < 6) Or (tmrpollingcount >= 10 And tmrpollingcount < 16) Then
                If tmrpollingcount = 10 Then
                    CheckOpenComPorts()
                End If
                For i = 0 To ArrSignNumbers.Length - 1
                    SBlue(i + 1).BackColor = Color.White
                    SRedGreen(i + 1).BackColor = Color.White
                Next i

            ElseIf (tmrpollingcount >= 6 And tmrpollingcount < 8) Or (tmrpollingcount >= 16 And tmrpollingcount < 18) Then

                For i = 0 To ArrSignNumbers.Length - 1
                    SBlue(i + 1).BackColor = Color.Blue
                    SRedGreen(i + 1).BackColor = Color.White
                Next i

            ElseIf (tmrpollingcount >= 8 And tmrpollingcount < 10) Or (tmrpollingcount >= 18) Then


                For i = 0 To ArrSignNumbers.Length - 1
                    If ArrSignStatus(i) = 1 Then SRedGreen(i + 1).BackColor = Color.Lime Else SRedGreen(i + 1).BackColor = Color.Red
                Next i

                If (tmrpollingcount = 20) Then
                    AvailableSpaces()
                    tmrpollingcount = 0
                End If

            End If

            If DatabaseConnect <> "Successful" Then

                'DatabaseConnect = SQL.openconnection(ServerName, DataBaseName)
                DatabaseConnect = SQL.openconnection(ServerName, DataBaseName, DataBaseUser, DataBasePassword)
                'Connect to Database
                If DatabaseConnect = "Successful" Then
                    Log("Database Connected")
                    TxtEcountingDB.BackColor = Color.Lime
                    TxtEcountingDB.Text = "Online"
                Else
                    Log("Database NOT Connected")
                    TxtEcountingDB.BackColor = Color.Red
                    TxtEcountingDB.Text = "Offline"
                End If
            Else
                TxtEcountingDB.BackColor = Color.Lime
                TxtEcountingDB.Text = "Online"
            End If


            If (Statuscount / 10 >= LogUpdateinterval) Or (Statuscount = 10 And LogUpdateinterval = 0) Then
                For s = 0 To ArrSignNumbers.Length - 1
                    Arrlogmessage(s) = 0
                    ArrlogCountererrors(s) = 1
                Next s
                Statuscount = 0
            End If

            If (GeneralCounter = 100) And btnExtendHide.Text = "Show Extended" Then
                Passwordentered = False
            ElseIf (GeneralCounter > 100) Then
                GeneralCounter = 0
            End If

            Statuscount += 1
            tmrpollingcount += 1
            GeneralCounter += 1



            i = Nothing
        Catch ex As Exception
            Log("Error from Timer1_Tick Function-->" & ex.Message)
            Log("Make sure the quantity of sign numbers configures matchs to the quantity of SignNames, ComPorts, IPaddresses & CounterShortNames")
        End Try
    End Sub

    Private Sub BtnRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRestart.Click
        Try

            Dim Answer As MsgBoxResult

            Answer = MsgBox("Are you sure you want to restart the application?", MessageBoxButtons.YesNo + MessageBoxIcon.Error, "VMS")
            If Answer = MsgBoxResult.Yes Then
                RestartApplication()
                'tmrpolling.Enabled = False
                Dim frmrestartload As FrmRestart = New FrmRestart
                frmrestartload.Show()
                frmrestartload.Location = Me.Location
            End If
        Catch ex As Exception
            Log("Error from BtnRestart Function-->" & ex.Message)
        End Try
    End Sub
    Private Sub RestartApplication()
        LoadParameters() 'Load parameter from appconfig
        AddArrayObjects() 'Put the 10 starting Sign objects in an array
        FillSetupTap()  'It just fills up the Setup tap on the main window
        FillParameterSetuTap() 'It just fills up the FillParameterSetu tap on the main window
        CheckOpenComPorts() 'Check Comport followed by ping to determine which Signs are online

        StartApplication() 'Start application - which will start the  timer

    End Sub

    Private Sub CombSignNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CombSignNumber.SelectedIndexChanged
        ' Dim i As Integer
        CombSignNumber.Text = ArrSignNumbers(CombSignNumber.SelectedIndex)
        Combcomports.Text = ArrComPorts(CombSignNumber.SelectedIndex)
        TxtSignName.Text = ArrSignNames(CombSignNumber.SelectedIndex)
        TxtIPAddress.Text = ArrIPaddresses(CombSignNumber.SelectedIndex)
        TxtCounterShortNames.Text = ArrCounterShortNames(CombSignNumber.SelectedIndex)
        'TxtLotNameWhereTheSignisInstalled.Text = ArrLotNamesWhereSignsAreInstalledOriginal(CombSignNumber.SelectedIndex)
    End Sub

    Private Sub btnAddNewSign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewSign.Click
        CombSignNumber.Visible = False
        TxtSignNumber.Visible = True
        Combcomports.Text = ""
        TxtSignName.Text = ""
        TxtIPAddress.Text = ""
        TxtCounterShortNames.Text = ""
        TxtLotNameWhereTheSignisInstalled.Text = ""
        Combcomports.Items.Clear()
        Combcomports.Items.AddRange(Myport)
        TxtSignNumber.Focus()
    End Sub

    Private Sub TxtSignNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSignNumber.TextChanged

    End Sub

    Private Sub Combcomports_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Combcomports.Click
        Dim temport As String

        temport = Combcomports.Text
        Combcomports.Items.Clear()
        Combcomports.Items.AddRange(Myport)
        Combcomports.Text = temport
    End Sub

    Private Sub Combcomports_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Combcomports.SelectedIndexChanged

    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        CombSignNumber.Visible = True
        TxtSignNumber.Visible = False
        CombSignNumber.Items.Clear()
        CombSignNumber.Items.AddRange(ArrSignNumbers)
        CombSignNumber.Text = ArrSignNumbers(0)
    End Sub



    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Try

            Dim Answer As MsgBoxResult
            Dim ArrTemSignNumbers(), ArrTempComports(), ArrTempCounterShortNames(), ArrTempIPaddresses(), ArrTempSignNames(), ArrTempLotNamesWhereSignsAreInstalledOriginal() As String
            Dim StrTemSignNumbers, StrTempComports, StrTempCounterShortNames, StrTempIPaddresses, StrTempSignNames, StrTempLotNamesWhereSignsAreInstalledOriginal As String

            Dim configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            Answer = MsgBox("Are you sure you want to save the change?", MessageBoxButtons.YesNo + MessageBoxIcon.Question, "VMS")

            If Answer = MsgBoxResult.Yes Then
                ArrTemSignNumbers = ArrSignNumbers
                ArrTempComports = ArrComPorts
                ArrTempCounterShortNames = ArrCounterShortNames
                ArrTempIPaddresses = ArrIPaddresses
                ArrTempSignNames = ArrSignNames
                ArrTempLotNamesWhereSignsAreInstalledOriginal = ArrLotNamesWhereSignsAreInstalledOriginal

                If CombSignNumber.Visible = True Then
                    ArrTempComports(CombSignNumber.SelectedIndex) = Combcomports.Text
                    ArrTempCounterShortNames(CombSignNumber.SelectedIndex) = TxtCounterShortNames.Text
                    ArrTempIPaddresses(CombSignNumber.SelectedIndex) = TxtIPAddress.Text
                    ArrTempSignNames(CombSignNumber.SelectedIndex) = TxtSignName.Text
                    ArrTempLotNamesWhereSignsAreInstalledOriginal(CombSignNumber.SelectedIndex) = TxtLotNameWhereTheSignisInstalled.Text

                    StrTemSignNumbers = ArrSignNumbers(0)
                    StrTempComports = ArrComPorts(0)
                    StrTempCounterShortNames = ArrCounterShortNames(0)
                    StrTempIPaddresses = ArrIPaddresses(0)
                    StrTempSignNames = ArrSignNames(0)
                    StrTempLotNamesWhereSignsAreInstalledOriginal = ArrLotNamesWhereSignsAreInstalledOriginal(0)

                    For i = 1 To ArrSignNumbers.Length - 1
                        StrTemSignNumbers = StrTemSignNumbers & ";" & ArrSignNumbers(i)
                        StrTempComports = StrTempComports & ";" & ArrComPorts(i)
                        StrTempCounterShortNames = StrTempCounterShortNames & ";" & ArrCounterShortNames(i)
                        StrTempIPaddresses = StrTempIPaddresses & ";" & ArrIPaddresses(i)
                        StrTempSignNames = StrTempSignNames & ";" & ArrSignNames(i)
                        StrTempLotNamesWhereSignsAreInstalledOriginal = StrTempLotNamesWhereSignsAreInstalledOriginal & ";" & ArrLotNamesWhereSignsAreInstalledOriginal(i)
                    Next i

                Else

                    StrTemSignNumbers = ArrSignNumbers(0)
                    StrTempComports = ArrComPorts(0)
                    StrTempCounterShortNames = ArrCounterShortNames(0)
                    StrTempIPaddresses = ArrIPaddresses(0)
                    StrTempSignNames = ArrSignNames(0)
                    StrTempLotNamesWhereSignsAreInstalledOriginal = ArrLotNamesWhereSignsAreInstalledOriginal(0)

                    For i = 1 To ArrSignNumbers.Length - 1
                        StrTemSignNumbers = StrTemSignNumbers & ";" & ArrSignNumbers(i)
                        StrTempComports = StrTempComports & ";" & ArrComPorts(i)
                        StrTempCounterShortNames = StrTempCounterShortNames & ";" & ArrCounterShortNames(i)
                        StrTempIPaddresses = StrTempIPaddresses & ";" & ArrIPaddresses(i)
                        StrTempSignNames = StrTempSignNames & ";" & ArrSignNames(i)
                        StrTempLotNamesWhereSignsAreInstalledOriginal = StrTempLotNamesWhereSignsAreInstalledOriginal & ";" & ArrLotNamesWhereSignsAreInstalledOriginal(i)
                    Next i

                    StrTemSignNumbers = StrTemSignNumbers & ";" & TxtSignNumber.Text
                    StrTempComports = StrTempComports & ";" & Combcomports.Text
                    StrTempCounterShortNames = StrTempCounterShortNames & ";" & TxtCounterShortNames.Text
                    StrTempIPaddresses = StrTempIPaddresses & ";" & TxtIPAddress.Text
                    StrTempSignNames = StrTempSignNames & ";" & TxtSignName.Text
                    StrTempLotNamesWhereSignsAreInstalledOriginal = StrTempLotNamesWhereSignsAreInstalledOriginal & ";" & TxtLotNameWhereTheSignisInstalled.Text


                End If

                settings("SignNumbers").Value = StrTemSignNumbers
                settings("ComPorts").Value = StrTempComports
                settings("CounterShortNames").Value = StrTempCounterShortNames
                settings("IPadresses").Value = StrTempIPaddresses
                settings("SignNames").Value = StrTempSignNames
                settings("LotNamesWhereSignsAreInstalled").Value = StrTempLotNamesWhereSignsAreInstalledOriginal



                configFile.Save(System.Configuration.ConfigurationSaveMode.Modified)
                System.Configuration.ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
                MsgBox("Change is saved." & vbCrLf & "Application will now restart for the changes to take effect", MessageBoxButtons.OK + MessageBoxIcon.Information, "VMS")

                RestartApplication()
                Dim frmrestartload As FrmRestart = New FrmRestart
                frmrestartload.Show()
                frmrestartload.Location = Me.Location
                CombSignNumber.Visible = True
                TxtSignNumber.Visible = False
                CombSignNumber.Items.Clear()
                CombSignNumber.Items.AddRange(ArrSignNumbers)
                CombSignNumber.Text = ArrSignNumbers(0)

            ElseIf Answer = MsgBoxResult.No Then
                Exit Sub
            End If


        Catch ex As Exception
            Log("Error from AvailableSpaces Function-->" & ex.Message)
        End Try
    End Sub

    Private Sub BtnDeleteSign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDeleteSign.Click
        Try

            Dim Answer As MsgBoxResult
            Dim StrTemSignNumbers, StrTempComports, StrTempCounterShortNames, StrTempIPaddresses, StrTempSignNames, StrTempLotNamesWhereSignsAreInstalledOriginal As String
            Dim j As Integer
            Dim configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            Answer = MsgBox("Are you sure you want to delete the sign?", MessageBoxButtons.YesNo + MessageBoxIcon.Error, "VMS")

            If Answer = MsgBoxResult.Yes And CombSignNumber.Visible = True Then
                If CombSignNumber.SelectedIndex <> 0 Then
                    StrTemSignNumbers = ArrSignNumbers(0)
                    StrTempComports = ArrComPorts(0)
                    StrTempCounterShortNames = ArrCounterShortNames(0)
                    StrTempIPaddresses = ArrIPaddresses(0)
                    StrTempSignNames = ArrSignNames(0)
                    StrTempLotNamesWhereSignsAreInstalledOriginal = ArrLotNamesWhereSignsAreInstalledOriginal(0)
                    j = 1
                Else
                    StrTemSignNumbers = ArrSignNumbers(1)
                    StrTempComports = ArrComPorts(1)
                    StrTempCounterShortNames = ArrCounterShortNames(1)
                    StrTempIPaddresses = ArrIPaddresses(1)
                    StrTempSignNames = ArrSignNames(1)
                    StrTempLotNamesWhereSignsAreInstalledOriginal = ArrLotNamesWhereSignsAreInstalledOriginal(1)
                    j = 2
                End If

                For i = j To ArrSignNumbers.Length - 1
                    If i <> CombSignNumber.SelectedIndex Then
                        StrTemSignNumbers = StrTemSignNumbers & ";" & ArrSignNumbers(i)
                        StrTempComports = StrTempComports & ";" & ArrComPorts(i)
                        StrTempCounterShortNames = StrTempCounterShortNames & ";" & ArrCounterShortNames(i)
                        StrTempIPaddresses = StrTempIPaddresses & ";" & ArrIPaddresses(i)
                        StrTempSignNames = StrTempSignNames & ";" & ArrSignNames(i)
                        StrTempLotNamesWhereSignsAreInstalledOriginal = StrTempLotNamesWhereSignsAreInstalledOriginal & ";" & ArrLotNamesWhereSignsAreInstalledOriginal(i)
                    End If
                Next i

                settings("SignNumbers").Value = StrTemSignNumbers
                settings("ComPorts").Value = StrTempComports
                settings("CounterShortNames").Value = StrTempCounterShortNames
                settings("IPadresses").Value = StrTempIPaddresses
                settings("SignNames").Value = StrTempSignNames
                settings("LotNamesWhereSignsAreInstalled").Value = StrTempLotNamesWhereSignsAreInstalledOriginal
                configFile.Save(System.Configuration.ConfigurationSaveMode.Modified)
                System.Configuration.ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
                MsgBox("Change is saved." & vbCrLf & "Application will now restart for the changes to take effect", MessageBoxButtons.OK + MessageBoxIcon.Information, "VMS")

                RestartApplication()
                Dim frmrestartload As FrmRestart = New FrmRestart
                frmrestartload.Show()
                frmrestartload.Location = Me.Location

            ElseIf Answer = MsgBoxResult.No Then
                CombSignNumber.Visible = True
                TxtSignNumber.Visible = False
                CombSignNumber.Items.Clear()
                CombSignNumber.Items.AddRange(ArrSignNumbers)
                CombSignNumber.Text = ArrSignNumbers(0)
                Exit Sub
            End If

        Catch ex As Exception
            Log("Error from BtnDeleteSign Function-->" & ex.Message)
        End Try
    End Sub
    Private Function Decrypt(ByVal cipherText As String) As String
        Try
            Dim EncryptionKey As String = "Hinote@2900"
            Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
            Using encryptor As Aes = Aes.Create()
                Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
                 &H65, &H64, &H76, &H65, &H64, &H65,
                 &H76})
                encryptor.Key = pdb.GetBytes(32)
                encryptor.IV = pdb.GetBytes(16)
                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                        cs.Write(cipherBytes, 0, cipherBytes.Length)
                        cs.Close()
                    End Using
                    cipherText = Encoding.Unicode.GetString(ms.ToArray())
                End Using
            End Using
            Return cipherText
        Catch ex As Exception
            Return cipherText
            Log("Password decrytion error-->" & ex.Message)
        End Try
    End Function
    Private Sub btnChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePassword.Click
        f.Close()
        Dim frm As Frmpassword = New Frmpassword
        f = frm
        f.Show()
        f.Text = "Password - Change"
        f.Size = New Size(730, f.Size.Height)
        f.Location = Me.Location

    End Sub
End Class
