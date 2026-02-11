<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        lbl_ftpStatus = New Label()
        btnBrowseFolder = New Button()
        btnStart = New Button()
        cboServerType = New ComboBox()
        txtRootDir = New TextBox()
        txtUserName = New TextBox()
        txtPassword = New TextBox()
        Panel1 = New Panel()
        Label9 = New Label()
        Label8 = New Label()
        numPort = New NumericUpDown()
        cbAutoStart = New CheckBox()
        cbEncrypt = New CheckBox()
        Panel1.SuspendLayout()
        CType(numPort, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.ForeColor = Color.White
        Label1.Location = New Point(12, 51)
        Label1.Name = "Label1"
        Label1.Size = New Size(55, 15)
        Label1.TabIndex = 0
        Label1.Text = "Protocol:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.ForeColor = Color.White
        Label2.Location = New Point(12, 82)
        Label2.Name = "Label2"
        Label2.Size = New Size(32, 15)
        Label2.TabIndex = 1
        Label2.Text = "Port:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.ForeColor = Color.White
        Label3.Location = New Point(12, 113)
        Label3.Name = "Label3"
        Label3.Size = New Size(86, 15)
        Label3.TabIndex = 2
        Label3.Text = "Root Directory:"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.ForeColor = Color.White
        Label4.Location = New Point(12, 143)
        Label4.Name = "Label4"
        Label4.Size = New Size(63, 15)
        Label4.TabIndex = 3
        Label4.Text = "Username:"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.ForeColor = Color.White
        Label5.Location = New Point(12, 175)
        Label5.Name = "Label5"
        Label5.Size = New Size(60, 15)
        Label5.TabIndex = 4
        Label5.Text = "Password:"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.ForeColor = Color.White
        Label6.Location = New Point(12, 222)
        Label6.Name = "Label6"
        Label6.Size = New Size(98, 15)
        Label6.TabIndex = 5
        Label6.Text = "File Server Status:"
        ' 
        ' lbl_ftpStatus
        ' 
        lbl_ftpStatus.AutoSize = True
        lbl_ftpStatus.ForeColor = Color.White
        lbl_ftpStatus.Location = New Point(119, 222)
        lbl_ftpStatus.Name = "lbl_ftpStatus"
        lbl_ftpStatus.Size = New Size(75, 15)
        lbl_ftpStatus.TabIndex = 6
        lbl_ftpStatus.Text = "Not Running"
        ' 
        ' btnBrowseFolder
        ' 
        btnBrowseFolder.BackColor = Color.FromArgb(CByte(74), CByte(74), CByte(74))
        btnBrowseFolder.FlatStyle = FlatStyle.Flat
        btnBrowseFolder.ForeColor = Color.White
        btnBrowseFolder.Location = New Point(437, 107)
        btnBrowseFolder.Name = "btnBrowseFolder"
        btnBrowseFolder.Size = New Size(75, 23)
        btnBrowseFolder.TabIndex = 7
        btnBrowseFolder.Text = "Browse"
        btnBrowseFolder.UseVisualStyleBackColor = False
        ' 
        ' btnStart
        ' 
        btnStart.BackColor = Color.FromArgb(CByte(74), CByte(74), CByte(74))
        btnStart.FlatStyle = FlatStyle.Flat
        btnStart.ForeColor = Color.White
        btnStart.Location = New Point(437, 218)
        btnStart.Name = "btnStart"
        btnStart.Size = New Size(75, 23)
        btnStart.TabIndex = 8
        btnStart.Text = "Start"
        btnStart.UseVisualStyleBackColor = False
        ' 
        ' cboServerType
        ' 
        cboServerType.BackColor = Color.FromArgb(CByte(74), CByte(74), CByte(74))
        cboServerType.DrawMode = DrawMode.OwnerDrawFixed
        cboServerType.DropDownStyle = ComboBoxStyle.DropDownList
        cboServerType.FlatStyle = FlatStyle.Flat
        cboServerType.ForeColor = Color.White
        cboServerType.FormattingEnabled = True
        cboServerType.Items.AddRange(New Object() {"SFTP", "FTP"})
        cboServerType.Location = New Point(113, 48)
        cboServerType.MaxDropDownItems = 2
        cboServerType.Name = "cboServerType"
        cboServerType.Size = New Size(62, 24)
        cboServerType.TabIndex = 9
        ' 
        ' txtRootDir
        ' 
        txtRootDir.BackColor = Color.FromArgb(CByte(74), CByte(74), CByte(74))
        txtRootDir.BorderStyle = BorderStyle.None
        txtRootDir.ForeColor = Color.White
        txtRootDir.Location = New Point(113, 108)
        txtRootDir.Name = "txtRootDir"
        txtRootDir.Size = New Size(318, 16)
        txtRootDir.TabIndex = 11
        ' 
        ' txtUserName
        ' 
        txtUserName.BackColor = Color.FromArgb(CByte(74), CByte(74), CByte(74))
        txtUserName.BorderStyle = BorderStyle.None
        txtUserName.ForeColor = Color.White
        txtUserName.Location = New Point(113, 140)
        txtUserName.Name = "txtUserName"
        txtUserName.Size = New Size(318, 16)
        txtUserName.TabIndex = 12
        ' 
        ' txtPassword
        ' 
        txtPassword.BackColor = Color.FromArgb(CByte(74), CByte(74), CByte(74))
        txtPassword.BorderStyle = BorderStyle.None
        txtPassword.ForeColor = Color.White
        txtPassword.Location = New Point(113, 172)
        txtPassword.Name = "txtPassword"
        txtPassword.Size = New Size(318, 16)
        txtPassword.TabIndex = 13
        txtPassword.UseSystemPasswordChar = True
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.Yellow
        Panel1.Controls.Add(Label9)
        Panel1.Controls.Add(Label8)
        Panel1.Location = New Point(8, 9)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(500, 35)
        Panel1.TabIndex = 14
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(119, 11)
        Label9.Name = "Label9"
        Label9.Size = New Size(338, 15)
        Label9.TabIndex = 15
        Label9.Text = "This file server is intended to be used for testing purposes only."
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point)
        Label8.Location = New Point(56, 11)
        Label8.Name = "Label8"
        Label8.Size = New Size(57, 15)
        Label8.TabIndex = 15
        Label8.Text = "Warning:"
        ' 
        ' numPort
        ' 
        numPort.BackColor = Color.FromArgb(CByte(74), CByte(74), CByte(74))
        numPort.BorderStyle = BorderStyle.FixedSingle
        numPort.ForeColor = Color.White
        numPort.Location = New Point(113, 78)
        numPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        numPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        numPort.Name = "numPort"
        numPort.Size = New Size(318, 23)
        numPort.TabIndex = 15
        numPort.Value = New Decimal(New Integer() {21, 0, 0, 0})
        ' 
        ' cbAutoStart
        ' 
        cbAutoStart.AutoSize = True
        cbAutoStart.ForeColor = Color.White
        cbAutoStart.Location = New Point(299, 218)
        cbAutoStart.Name = "cbAutoStart"
        cbAutoStart.Size = New Size(76, 19)
        cbAutoStart.TabIndex = 16
        cbAutoStart.Text = "AutoStart"
        cbAutoStart.UseVisualStyleBackColor = True
        ' 
        ' cbEncrypt
        ' 
        cbEncrypt.AutoSize = True
        cbEncrypt.ForeColor = Color.White
        cbEncrypt.Location = New Point(299, 193)
        cbEncrypt.Name = "cbEncrypt"
        cbEncrypt.Size = New Size(111, 19)
        cbEncrypt.TabIndex = 17
        cbEncrypt.Text = "Encrypt Settings"
        cbEncrypt.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(86), CByte(86), CByte(86))
        ClientSize = New Size(524, 251)
        Controls.Add(cbEncrypt)
        Controls.Add(cbAutoStart)
        Controls.Add(numPort)
        Controls.Add(Panel1)
        Controls.Add(txtPassword)
        Controls.Add(txtUserName)
        Controls.Add(txtRootDir)
        Controls.Add(cboServerType)
        Controls.Add(btnStart)
        Controls.Add(btnBrowseFolder)
        Controls.Add(lbl_ftpStatus)
        Controls.Add(Label6)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MaximumSize = New Size(600, 290)
        MinimumSize = New Size(510, 290)
        Name = "Form1"
        Text = "In-Sight File Server Configuration v2"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(numPort, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents lbl_ftpStatus As Label
    Friend WithEvents btnBrowseFolder As Button
    Friend WithEvents btnStart As Button
    Friend WithEvents cboServerType As ComboBox
    Friend WithEvents txtRootDir As TextBox
    Friend WithEvents txtUserName As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents numPort As NumericUpDown
    Friend WithEvents cbAutoStart As CheckBox
    Friend WithEvents cbEncrypt As CheckBox

End Class
