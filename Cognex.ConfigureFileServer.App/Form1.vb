Imports System.Diagnostics
Imports System.IO
Imports System.Xml.Linq
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Win32
Imports System.Security.Cryptography
Imports System.Text
Imports System.Globalization


Public Class Form1
    Dim ftpMode As String
    Dim ftpStatus(2) As Boolean
    Dim basePath As String = "C:\Program Files\Cognex\In-Sight\In-Sight Vision Suite\FtpServer"
    Private helperProcess As Process
    Private helperPid As Integer = 0

    ' Match the actual exe names
    Private ReadOnly helperExeName As String = "FTP Launcher.exe"
    Private ReadOnly ftpServerExeName As String = "RebexTinyFtpServer.exe"
    Private ReadOnly sftpServerExeName As String = "RebexTinySftpServer.exe"
    Private Const AutoStartName As String = "FTP Launcher"
    Private Const AutoStartPath As String = "Software\Microsoft\Windows\CurrentVersion\Run"



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Look for helper process by name (without .exe)
        Dim procs = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(helperExeName))
        If procs.Length > 0 Then
            helperProcess = procs(0)
            helperProcess.EnableRaisingEvents = True
            AddHandler helperProcess.Exited, AddressOf HelperExited
            UpdateUiRunning()
            DetectAndUpdateServerType()
        Else
            UpdateUiStopped()
        End If
        Dim autoType = GetAutoStartServerType()
        If autoType = "FTP" OrElse autoType = "SFTP" Then
            cboServerType.SelectedItem = autoType
            cbAutoStart.Checked = True
        Else
            cbAutoStart.Checked = False
        End If

    End Sub

    Private Sub cboServerType_DrawItem(sender As Object, e As DrawItemEventArgs) Handles cboServerType.DrawItem

        ' Skip if nothing to draw
        If e.Index < 0 Then Return

        ' Decide colors based on selection state
        Dim backColor As Color
        Dim foreColor As Color = Color.White

        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            backColor = ColorTranslator.FromHtml("#565656") ' custom selected background  
        Else
            backColor = ColorTranslator.FromHtml("#4A4A4A")
        End If

        ' Paint background
        Using bg As New SolidBrush(backColor)
            e.Graphics.FillRectangle(bg, e.Bounds)
        End Using

        ' Paint text
        Dim text As String = cboServerType.Items(e.Index).ToString()
        Using fg As New SolidBrush(foreColor)
            e.Graphics.DrawString(text, e.Font, fg, e.Bounds)
        End Using

        ' Draw focus rectangle for keyboard navigation
        e.DrawFocusRectangle()

    End Sub

    Private Sub cboServerType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboServerType.SelectedIndexChanged
        ftpStatus = CheckFTP()
        If (cboServerType.SelectedItem = "FTP") Then
            If ftpStatus(0) = False Then
                lbl_ftpStatus.Text = "Not Running"
            Else
                lbl_ftpStatus.Text = "Running"
                SetServerControlsEnabled(False)
                btnStart.Text = "Stop"
            End If
        End If
        If (cboServerType.SelectedItem = "SFTP") Then
            If ftpStatus(1) = False Then
                lbl_ftpStatus.Text = "Not Running"
            Else
                lbl_ftpStatus.Text = "Running"
                SetServerControlsEnabled(False)
                btnStart.Text = "Stop"
            End If
        End If
        ftpMode = cboServerType.SelectedItem.ToString()
        If (cboServerType.SelectedItem = "FTP") Then
            Dim encFile As String = Path.Combine(basePath, "RebexTinyFtpServer.exe.enc")
            Dim rawFile As String = Path.Combine(basePath, "RebexTinyFtpServer.exe.config")
            If File.Exists(encFile) Then
                cbEncrypt.Checked = True
                ' Delete any unencrypted .config files
                File.Delete(rawFile)
            Else
                cbEncrypt.Checked = False
            End If
        End If
        If (cboServerType.SelectedItem = "SFTP") Then
            Dim encFile As String = Path.Combine(basePath, "RebexTinySftpServer.exe.enc")
            Dim rawFile As String = Path.Combine(basePath, "RebexTinySftpServer.exe.config")
            If File.Exists(encFile) Then
                cbEncrypt.Checked = True
                ' Delete any unencrypted .config files
                File.Delete(rawFile)
            Else
                cbEncrypt.Checked = False
            End If
        End If

        LoadConfig()


    End Sub

    Private Sub btnBrowseFolder_Click(sender As Object, e As EventArgs) Handles btnBrowseFolder.Click
        Using dlg As New OpenFileDialog()
            ' Trick the file dialog into folder mode
            dlg.CheckFileExists = False
            dlg.ValidateNames = False
            dlg.FileName = "SelectFolder"   ' dummy filename

            If dlg.ShowDialog() = DialogResult.OK Then
                ' Extract the folder path from the selected "file"
                Dim folderPath As String = Path.GetDirectoryName(dlg.FileName)
                txtRootDir.Text = folderPath
            End If
        End Using

    End Sub

    Private Function CheckFTP() As Boolean()
        'Check to see if FTP is running
        'RebexTinyFtpServer.exe or RebexTinySftpServer.exe
        Dim procFTP() As Process = Process.GetProcessesByName("RebexTinyFtpServer")
        Dim procSFTP() As Process = Process.GetProcessesByName("RebexTinySftpServer")
        Dim procStats(2) As Boolean
        If procFTP.Length > 0 Then
            procStats(0) = 1
        Else
            procStats(0) = 0
        End If
        If procSFTP.Length > 0 Then
            procStats(1) = 1
        Else
            procStats(1) = 0
        End If
        Return procStats
    End Function

    Private Sub LoadConfig()
        Dim configFile As String = ""
        Dim portKey As String = ""

        ' Decide which config file
        If cboServerType.SelectedItem IsNot Nothing Then
            Select Case cboServerType.SelectedItem.ToString().ToUpper()
                Case "FTP"
                    configFile = Path.Combine(basePath, "RebexTinyFtpServer.exe.config")
                    portKey = "ftpPort"
                Case "SFTP"
                    configFile = Path.Combine(basePath, "RebexTinySftpServer.exe.config")
                    portKey = "sshPort"
                Case Else
                    MessageBox.Show("Unsupported server type selected.")
                    Return
            End Select
        End If

        Dim doc As XDocument

        If cbEncrypt.Checked Then
            ' Load from encrypted file
            Dim encFile As String = Path.ChangeExtension(configFile, ".enc")
            If Not File.Exists(encFile) Then
                MessageBox.Show("Encrypted configuration file not found: " & encFile)
                Return
            End If
            Dim decryptedXml As String = DecryptFile(encFile)
            doc = XDocument.Parse(decryptedXml)
        Else
            ' Load from plaintext file
            If Not File.Exists(configFile) Then
                MessageBox.Show("Configuration file not found: " & configFile)
                Return
            End If
            doc = XDocument.Load(configFile)
        End If

        ' Helper to read a key
        Dim readKey = Function(keyName As String) As String
                          Dim element = doc.<configuration>.<appSettings>.Elements("add").
                          FirstOrDefault(Function(x) x.Attribute("key")?.Value = keyName)
                          If element IsNot Nothing Then
                              Return element.Attribute("value")?.Value
                          End If
                          Return ""
                      End Function

        ' Populate controls
        txtUserName.Text = readKey("userName")
        txtPassword.Text = readKey("userPassword")
        txtRootDir.Text = readKey("userRootDir")

        Dim portValue As String = readKey(portKey)
        Dim portNum As Integer
        If Integer.TryParse(portValue, portNum) Then
            numPort.Value = Math.Min(Math.Max(numPort.Minimum, portNum), numPort.Maximum)
        End If
    End Sub

    Public Sub SaveServerConfig()
        Dim configFile As String = ""
        Dim portKey As String = ""

        ' Decide which config file
        If cboServerType.SelectedItem IsNot Nothing Then
            Select Case cboServerType.SelectedItem.ToString().ToUpper()
                Case "FTP"
                    configFile = Path.Combine(basePath, "RebexTinyFtpServer.exe.config")
                    portKey = "ftpPort"
                Case "SFTP"
                    configFile = Path.Combine(basePath, "RebexTinySftpServer.exe.config")
                    portKey = "sshPort"
                Case Else
                    MessageBox.Show("Unsupported server type selected.")
                    Return
            End Select
        End If

        Dim doc As XDocument

        If cbEncrypt.Checked Then
            ' Work with decrypted XML in memory, then encrypt back
            Dim encFile As String = Path.ChangeExtension(configFile, ".enc")
            If File.Exists(encFile) Then
                Dim decryptedXml As String = DecryptFile(encFile)
                doc = XDocument.Parse(decryptedXml)
            Else
                ' If no encrypted file yet, fall back to plaintext
                doc = XDocument.Load(configFile)
            End If
        Else
            doc = XDocument.Load(configFile)
        End If

        ' Update keys
        Dim updateKey = Sub(keyName As String, newValue As String)
                            Dim element = doc.<configuration>.<appSettings>.Elements("add").
                            FirstOrDefault(Function(x) x.Attribute("key")?.Value = keyName)
                            If element IsNot Nothing Then
                                element.SetAttributeValue("value", newValue)
                            End If
                        End Sub

        updateKey("userName", txtUserName.Text)
        updateKey("userPassword", txtPassword.Text)
        updateKey("userRootDir", txtRootDir.Text)
        updateKey(portKey, numPort.Value.ToString())

        If cbEncrypt.Checked Then
            ' Save plaintext temporarily
            doc.Save(configFile)

            ' Immediately encrypt and delete plaintext
            EncryptFile(configFile, Path.ChangeExtension(configFile, ".enc"))
        Else
            ' Save plaintext normally
            doc.Save(configFile)
        End If
    End Sub

    Private Sub EncryptFile(inputFile As String, outputFile As String)
        Dim plainBytes As Byte() = File.ReadAllBytes(inputFile)
        Dim encBytes As Byte() = ProtectedData.Protect(plainBytes, Nothing, DataProtectionScope.LocalMachine)
        File.WriteAllBytes(outputFile, encBytes)
        File.Delete(inputFile)
    End Sub

    Private Function DecryptFile(inputFile As String) As String
        ' Read encrypted bytes
        Dim encBytes As Byte() = File.ReadAllBytes(inputFile)

        ' Unprotect using machine scope
        Dim plainBytes As Byte() = ProtectedData.Unprotect(encBytes, Nothing, DataProtectionScope.LocalMachine)

        ' Decode as UTF-8
        Dim decryptedXml As String = Encoding.UTF8.GetString(plainBytes)

        ' Strip BOM if present
        If decryptedXml.Length > 0 AndAlso decryptedXml(0) = ChrW(&HFEFF) Then
            decryptedXml = decryptedXml.Substring(1)
        End If

        Return decryptedXml
    End Function

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        If btnStart.Text = "Stop" Then
            StopHelperAndServer()
            UpdateUiStopped()
            cbEncrypt.Enabled = True
        Else
            If (cboServerType.SelectedItem = "SFTP" And txtPassword.Text = "") Then
                MessageBox.Show("Password Required for SFTP", "Missing Password", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If LaunchHelper() Then
                UpdateUiRunning()
            End If
            SaveServerConfig()
            cbEncrypt.Enabled = False
        End If
    End Sub

    Private Function HelperPath() As String
        Return Path.Combine(Application.StartupPath, helperExeName)
    End Function

    Private Function LaunchHelper() As Boolean
        Try
            Dim serverType As String = cboServerType.SelectedItem?.ToString()
            If String.IsNullOrWhiteSpace(serverType) Then
                MessageBox.Show("Select a server type.")
                Return False
            End If

            helperProcess = New Process()
            helperProcess.StartInfo.FileName = HelperPath()
            helperProcess.StartInfo.Arguments = serverType
            helperProcess.EnableRaisingEvents = True
            AddHandler helperProcess.Exited, AddressOf HelperExited
            helperProcess.Start()
            helperPid = helperProcess.Id
            Return True
        Catch ex As Exception
            MessageBox.Show("Failed to start helper: " & ex.Message)
            helperPid = 0
            Return False
        End Try
    End Function

    Private Sub StopHelperAndServer()
        Try
            ' Preferred: send a stop signal to helper so it shuts down the server
            ' Example: Process.Start(HelperPath(), "--stop")

            ' Fallback: kill server then helper
            KillServerProcess()
            KillHelperProcess()
        Catch ex As Exception
            MessageBox.Show("Could not stop processes: " & ex.Message)
        End Try
    End Sub

    Private Sub KillHelperProcess()
        If helperProcess IsNot Nothing AndAlso Not helperProcess.HasExited Then
            Try
                helperProcess.Kill()
                helperProcess.WaitForExit()
            Catch
            End Try
        End If
        helperPid = 0
    End Sub

    Private Sub KillServerProcess()
        Dim serverName = Path.GetFileNameWithoutExtension(ServerExeName())
        Dim procs = Process.GetProcessesByName(serverName)
        If procs.Length > 0 Then
            Try
                procs(0).Kill()
                procs(0).WaitForExit()
            Catch
            End Try
        End If
    End Sub

    Private Function ServerExeName() As String
        Dim t = cboServerType.SelectedItem?.ToString()
        If String.Equals(t, "SFTP", StringComparison.OrdinalIgnoreCase) Then
            Return sftpServerExeName
        Else
            Return ftpServerExeName
        End If
    End Function

    Private Sub HelperExited(sender As Object, e As EventArgs)
        helperPid = 0
        If Me.IsHandleCreated Then
            Me.BeginInvoke(Sub() UpdateUiStopped())
        End If
    End Sub

    ' Centralized UI updates
    Private Sub UpdateUiRunning()
        lbl_ftpStatus.Text = "Running"
        btnStart.Text = "Stop"
        SetServerControlsEnabled(False)
    End Sub

    Private Sub UpdateUiStopped()
        lbl_ftpStatus.Text = "Not Running"
        btnStart.Text = "Start"
        SetServerControlsEnabled(True)
    End Sub

    Private Sub SetServerControlsEnabled(isEnabled As Boolean)
        cboServerType.Enabled = isEnabled
        numPort.Enabled = isEnabled
        txtRootDir.Enabled = isEnabled
        txtUserName.Enabled = isEnabled
        txtPassword.Enabled = isEnabled
        btnBrowseFolder.Enabled = isEnabled
    End Sub

    Private Sub DetectAndUpdateServerType()
        ' Only update if combo is not already FTP or SFTP
        Dim currentType As String = cboServerType.SelectedItem?.ToString()
        If Not String.Equals(currentType, "FTP", StringComparison.OrdinalIgnoreCase) AndAlso
       Not String.Equals(currentType, "SFTP", StringComparison.OrdinalIgnoreCase) Then

            ' Look for FTP server
            Dim ftpProcs = Process.GetProcessesByName("RebexTinyFtpServer")
            If ftpProcs.Length > 0 Then
                cboServerType.SelectedItem = "FTP"
                Return
            End If

            ' Look for SFTP server
            Dim sftpProcs = Process.GetProcessesByName("RebexTinySftpServer")
            If sftpProcs.Length > 0 Then
                cboServerType.SelectedItem = "SFTP"
                Return
            End If
        End If
    End Sub

    Private Sub SetAutoStart(enabled As Boolean, serverType As String)
        Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(
        "Software\Microsoft\Windows\CurrentVersion\Run", True)

            If enabled Then
                ' Quote the path, then add the argument
                Dim cmd As String = $"""{HelperPath()}"" {serverType}"
                key.SetValue("FTP Launcher", cmd)
            Else
                key.DeleteValue("FTP Launcher", False)
            End If
        End Using
    End Sub

    Private Function IsAutoStartEnabled() As Boolean
        Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(AutoStartPath, False)
            Dim value = key?.GetValue(AutoStartName)
            Return value IsNot Nothing
        End Using
    End Function

    Private Function GetAutoStartServerType() As String
        Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(
        "Software\Microsoft\Windows\CurrentVersion\Run", False)

            Dim value = key?.GetValue("FTP Launcher")?.ToString()
            If String.IsNullOrEmpty(value) Then Return Nothing

            ' Parse argument (after exe path)
            Dim parts = value.Split(" "c)
            If parts.Length > 1 Then
                Return parts(1).ToUpper()
            End If
            Return Nothing
        End Using
    End Function


    Private Sub cbAutoStart_CheckedChanged(sender As Object, e As EventArgs) Handles cbAutoStart.CheckedChanged
        Dim serverType As String = cboServerType.SelectedItem?.ToString()
        If Not String.IsNullOrEmpty(serverType) Then
            SetAutoStart(cbAutoStart.Checked, serverType)
        End If

    End Sub

    Private Sub EncryptFile(inputFile As String, outputFile As String, key As Byte(), iv As Byte())
        Using fsInput As New FileStream(inputFile, FileMode.Open)
            Using fsEncrypted As New FileStream(outputFile, FileMode.Create)
                Using aes As Aes = aes.Create()
                    aes.Key = key
                    aes.IV = iv
                    Using cs As New CryptoStream(fsEncrypted, aes.CreateEncryptor(), CryptoStreamMode.Write)
                        fsInput.CopyTo(cs)
                    End Using
                End Using
            End Using
        End Using
        File.Delete(inputFile)
    End Sub

    Private Function DecryptFile(inputFile As String, key As Byte(), iv As Byte()) As String
        Using fsEncrypted As New FileStream(inputFile, FileMode.Open)
            Using aes As Aes = Aes.Create()
                aes.Key = key
                aes.IV = iv
                Using cs As New CryptoStream(fsEncrypted, aes.CreateDecryptor(), CryptoStreamMode.Read)
                    Using reader As New StreamReader(cs)
                        Return reader.ReadToEnd()
                    End Using
                End Using
            End Using
        End Using
    End Function

    Private Sub cbEncrypt_CheckedChanged(sender As Object, e As EventArgs) Handles cbEncrypt.CheckedChanged
        If cbEncrypt.Checked = False Then
            ' FTP
            Dim ftpEnc As String = Path.Combine(basePath, "RebexTinyFtpServer.exe.enc")
            If File.Exists(ftpEnc) Then
                Dim decryptedXml As String = DecryptFile(ftpEnc)
                File.WriteAllText(Path.Combine(basePath, "RebexTinyFtpServer.exe.config"), decryptedXml, New UTF8Encoding(False))
                File.Delete(ftpEnc)

            End If

            ' SFTP
            Dim sftpEnc As String = Path.Combine(basePath, "RebexTinySftpServer.exe.enc")
            If File.Exists(sftpEnc) Then
                Dim decryptedXml As String = DecryptFile(sftpEnc)
                File.WriteAllText(Path.Combine(basePath, "RebexTinySftpServer.exe.config"), decryptedXml, New UTF8Encoding(False))
                File.Delete(sftpEnc)

            End If
        End If
    End Sub

End Class
