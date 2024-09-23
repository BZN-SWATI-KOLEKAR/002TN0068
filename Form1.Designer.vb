<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSonataEpay
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSonataEpay))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LblSelect = New System.Windows.Forms.Label()
        Me.TxtFilePath = New System.Windows.Forms.TextBox()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.LinkAudit = New System.Windows.Forms.LinkLabel()
        Me.LinkOutput = New System.Windows.Forms.LinkLabel()
        Me.LinkError = New System.Windows.Forms.LinkLabel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CmdExit = New System.Windows.Forms.Button()
        Me.CmdGenerate = New System.Windows.Forms.Button()
        Me.CmdSelect = New System.Windows.Forms.Button()
        Me.dlgDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.FolderDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.LinkReport = New System.Windows.Forms.LinkLabel()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(93, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        '
        'LblSelect
        '
        Me.LblSelect.AutoSize = True
        Me.LblSelect.BackColor = System.Drawing.Color.Transparent
        Me.LblSelect.Font = New System.Drawing.Font("Arial Narrow", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSelect.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.LblSelect.Location = New System.Drawing.Point(66, 109)
        Me.LblSelect.Name = "LblSelect"
        Me.LblSelect.Size = New System.Drawing.Size(72, 20)
        Me.LblSelect.TabIndex = 162
        Me.LblSelect.Text = "Select File"
        '
        'TxtFilePath
        '
        Me.TxtFilePath.Enabled = False
        Me.TxtFilePath.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFilePath.Location = New System.Drawing.Point(155, 111)
        Me.TxtFilePath.Name = "TxtFilePath"
        Me.TxtFilePath.Size = New System.Drawing.Size(315, 20)
        Me.TxtFilePath.TabIndex = 161
        '
        'LblStatus
        '
        Me.LblStatus.AutoSize = True
        Me.LblStatus.BackColor = System.Drawing.Color.Transparent
        Me.LblStatus.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LblStatus.Location = New System.Drawing.Point(123, 201)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(0, 16)
        Me.LblStatus.TabIndex = 160
        '
        'LinkAudit
        '
        Me.LinkAudit.AutoSize = True
        Me.LinkAudit.BackColor = System.Drawing.Color.Transparent
        Me.LinkAudit.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.LinkAudit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LinkAudit.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LinkAudit.Location = New System.Drawing.Point(329, 242)
        Me.LinkAudit.Name = "LinkAudit"
        Me.LinkAudit.Size = New System.Drawing.Size(74, 15)
        Me.LinkAudit.TabIndex = 159
        Me.LinkAudit.TabStop = True
        Me.LinkAudit.Text = "Audit Report"
        '
        'LinkOutput
        '
        Me.LinkOutput.AutoSize = True
        Me.LinkOutput.BackColor = System.Drawing.Color.Transparent
        Me.LinkOutput.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.LinkOutput.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LinkOutput.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LinkOutput.Location = New System.Drawing.Point(482, 242)
        Me.LinkOutput.Name = "LinkOutput"
        Me.LinkOutput.Size = New System.Drawing.Size(66, 15)
        Me.LinkOutput.TabIndex = 158
        Me.LinkOutput.TabStop = True
        Me.LinkOutput.Text = "Output File"
        '
        'LinkError
        '
        Me.LinkError.AutoSize = True
        Me.LinkError.BackColor = System.Drawing.Color.Transparent
        Me.LinkError.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.LinkError.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LinkError.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LinkError.Location = New System.Drawing.Point(249, 242)
        Me.LinkError.Name = "LinkError"
        Me.LinkError.Size = New System.Drawing.Size(74, 15)
        Me.LinkError.TabIndex = 157
        Me.LinkError.TabStop = True
        Me.LinkError.Text = "Error Report"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.Controls.Add(Me.CmdExit)
        Me.GroupBox1.Controls.Add(Me.CmdGenerate)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(385, 169)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(164, 41)
        Me.GroupBox1.TabIndex = 156
        Me.GroupBox1.TabStop = False
        '
        'CmdExit
        '
        Me.CmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CmdExit.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdExit.ForeColor = System.Drawing.Color.Black
        Me.CmdExit.Location = New System.Drawing.Point(86, 13)
        Me.CmdExit.Name = "CmdExit"
        Me.CmdExit.Size = New System.Drawing.Size(72, 26)
        Me.CmdExit.TabIndex = 3
        Me.CmdExit.Text = "E&xit"
        Me.CmdExit.UseVisualStyleBackColor = True
        '
        'CmdGenerate
        '
        Me.CmdGenerate.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdGenerate.ForeColor = System.Drawing.Color.Black
        Me.CmdGenerate.Location = New System.Drawing.Point(6, 13)
        Me.CmdGenerate.Name = "CmdGenerate"
        Me.CmdGenerate.Size = New System.Drawing.Size(72, 26)
        Me.CmdGenerate.TabIndex = 2
        Me.CmdGenerate.Text = "&Generate "
        Me.CmdGenerate.UseVisualStyleBackColor = True
        '
        'CmdSelect
        '
        Me.CmdSelect.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdSelect.ForeColor = System.Drawing.Color.Salmon
        Me.CmdSelect.Location = New System.Drawing.Point(479, 110)
        Me.CmdSelect.Name = "CmdSelect"
        Me.CmdSelect.Size = New System.Drawing.Size(62, 24)
        Me.CmdSelect.TabIndex = 155
        Me.CmdSelect.Text = "Browse"
        Me.CmdSelect.UseVisualStyleBackColor = True
        '
        'LinkReport
        '
        Me.LinkReport.ActiveLinkColor = System.Drawing.Color.Red
        Me.LinkReport.AutoSize = True
        Me.LinkReport.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.LinkReport.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkReport.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.LinkReport.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LinkReport.Location = New System.Drawing.Point(409, 242)
        Me.LinkReport.Name = "LinkReport"
        Me.LinkReport.Size = New System.Drawing.Size(67, 15)
        Me.LinkReport.TabIndex = 164
        Me.LinkReport.TabStop = True
        Me.LinkReport.Text = "Report File"
        '
        'FrmSonataEpay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(572, 268)
        Me.Controls.Add(Me.LinkReport)
        Me.Controls.Add(Me.LblSelect)
        Me.Controls.Add(Me.TxtFilePath)
        Me.Controls.Add(Me.LblStatus)
        Me.Controls.Add(Me.LinkAudit)
        Me.Controls.Add(Me.LinkOutput)
        Me.Controls.Add(Me.LinkError)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CmdSelect)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmSonataEpay"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SONATA EPAY_Convertor"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LblSelect As System.Windows.Forms.Label
    Friend WithEvents TxtFilePath As System.Windows.Forms.TextBox
    Friend WithEvents LblStatus As System.Windows.Forms.Label
    Friend WithEvents LinkAudit As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkOutput As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkError As System.Windows.Forms.LinkLabel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CmdExit As System.Windows.Forms.Button
    Friend WithEvents CmdGenerate As System.Windows.Forms.Button
    Friend WithEvents CmdSelect As System.Windows.Forms.Button
    Friend WithEvents dlgDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FolderDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents LinkReport As System.Windows.Forms.LinkLabel

End Class
