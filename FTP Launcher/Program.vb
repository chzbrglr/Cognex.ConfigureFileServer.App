Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Module Program
    <DllImport("user32.dll")>
    Private Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Function SetForegroundWindow(hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Function IsWindowVisible(hWnd As IntPtr) As Boolean
    End Function

    Private Const SW_HIDE As Integer = 0
    Private Const SW_RESTORE As Integer = 9

    Private serverProcess As Process
    Private hWnd As IntPtr = IntPtr.Zero

    ' DPAPI decryption helper
    Private Function DecryptFile(inputFile As String) As String
        Dim encBytes As Byte() = File.ReadAllBytes(inputFile)
        Dim plainBytes As Byte() = ProtectedData.Unprotect(encBytes, Nothing, DataProtectionScope.LocalMachine)
        Dim xml As String = Encoding.UTF8.GetString(plainBytes)
        ' Strip BOM if present
        If xml.Length > 0 AndAlso xml(0) = ChrW(&HFEFF) Then
            xml = xml.Substring(1)
        End If
        Return xml
    End Function


    Sub Main(args As String())
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Decide which server to launch
        Dim serverType As String = If(args.Length > 0, args(0).ToUpper(), "FTP")
        Dim basePath As String = "C:\Program Files\Cognex\In-Sight\In-Sight Vision Suite\FtpServer"
        Dim exePath As String = If(serverType = "SFTP",
                                   IO.Path.Combine(basePath, "RebexTinySftpServer.exe"),
                                   IO.Path.Combine(basePath, "RebexTinyFtpServer.exe"))

        ' Config paths
        Dim configFile As String = If(serverType = "SFTP",
                                      Path.Combine(basePath, "RebexTinySftpServer.exe.config"),
                                      Path.Combine(basePath, "RebexTinyFtpServer.exe.config"))
        Dim encFile As String = Path.ChangeExtension(configFile, ".enc")

        ' If encrypted config exists, decrypt to plaintext temporarily
        Dim tempConfigCreated As Boolean = False
        Try
            If File.Exists(encFile) Then
                Dim decryptedXml As String = DecryptFile(encFile)
                File.WriteAllText(configFile, decryptedXml, New UTF8Encoding(True))
                tempConfigCreated = True
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to decrypt config: " & ex.Message)
            Return
        End Try

        ' Launch server
        Try
            serverProcess = Process.Start(exePath)

            ' Poll until window handle is available
            Dim attempts As Integer = 0
            Do While hWnd = IntPtr.Zero AndAlso attempts < 20 AndAlso Not serverProcess.HasExited
                Thread.Sleep(250)
                serverProcess.Refresh()
                hWnd = serverProcess.MainWindowHandle
                attempts += 1
            Loop

            ' Hide window initially
            If hWnd <> IntPtr.Zero Then
                ShowWindow(hWnd, SW_HIDE)
            End If

            ' Once server is running, delete temporary plaintext config
            If tempConfigCreated AndAlso Not serverProcess.HasExited Then
                Try
                    File.Delete(configFile)
                Catch
                    ' Ignore errors if file is locked
                End Try
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to start server: " & ex.Message)
            Return
        End Try

        ' Create tray icon (use app icon to avoid resx corruption)
        Dim ni As New NotifyIcon() With {
            .Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
            .Text = $"{serverType} Server Controller",
            .Visible = True
        }

        ' Double‑click toggles visibility
        AddHandler ni.DoubleClick,
            Sub()
                If hWnd <> IntPtr.Zero Then
                    If IsWindowVisible(hWnd) Then
                        ShowWindow(hWnd, SW_HIDE)
                    Else
                        ShowWindow(hWnd, SW_RESTORE)
                        SetForegroundWindow(hWnd)
                    End If
                End If
            End Sub

        ' Context menu
        Dim menu As New ContextMenuStrip()
        menu.Items.Add("Stop Server", Nothing,
            Sub()
                Try
                    If serverProcess IsNot Nothing AndAlso Not serverProcess.HasExited Then
                        serverProcess.Kill()
                        serverProcess.WaitForExit()
                    End If
                Catch ex As Exception
                    MessageBox.Show("Could not stop server: " & ex.Message)
                End Try
            End Sub)

        menu.Items.Add("Exit Helper", Nothing,
            Sub()
                ni.Visible = False
                ' If server still running, stop it too
                Try
                    If serverProcess IsNot Nothing AndAlso Not serverProcess.HasExited Then
                        serverProcess.Kill()
                        serverProcess.WaitForExit()
                    End If
                Catch
                End Try
                Application.Exit()
            End Sub)

        ni.ContextMenuStrip = menu

        ' Keep app alive with message loop
        Application.Run()

        ' Cleanup when exiting
        ni.Visible = False
        If serverProcess IsNot Nothing AndAlso Not serverProcess.HasExited Then
            serverProcess.Kill()
        End If
    End Sub
End Module
