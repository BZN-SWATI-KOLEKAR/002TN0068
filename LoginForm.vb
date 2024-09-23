Imports System.IO

Public Class LoginForm
    Dim objBaseClass As ClsBase
    Dim objFileValidate As ClsValidation
    Dim objGetSetINI As ClsShared
    Dim objFrmSonata As FrmSonataEpay
    Dim StrEncrpt As String = String.Empty
    Private Sub CmdLogin_Click(sender As Object, e As EventArgs) Handles CmdLogin.Click
        Dim readText() As String
        Dim UserName As String

        'Check UserName Valid or not
        objBaseClass.LogEntry("Process started for UserName validation   -- Started At -- " & Format(Date.Now, "hh:mm:ss"), False)
        If TxtUserName.Text.ToString().Trim() = "" Then
            MsgBox("UserName cannot be blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")
            objBaseClass.LogEntry("UserName cannot be blank", False)
            Exit Sub
        ElseIf (strUserName = "") Then
            MsgBox("UserName blank in settings.ini file", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")
            objBaseClass.LogEntry("UserName blank in settings.ini file", False)
            Exit Sub
        Else
            If Not IsNumeric(strUserName) Then
                If strUserName = TxtUserName.Text Then
                    gstrUserName = strUserName
                Else
                    readText = strUserName.Split(" ")
                    gstrUserName = ""
                    For Each str As String In readText
                        UserName = str.Substring(0, 1)
                        gstrUserName = gstrUserName & UserName

                    Next
                    If (gstrUserName = TxtUserName.Text) Then
                    Else
                        MsgBox("UserName is not valid ", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")
                        objBaseClass.LogEntry("UserName is not valid", False)
                        Exit Sub
                    End If
                End If
            Else
                gstrUserName = TxtUserName.Text
            End If

            objBaseClass.LogEntry("Login successfully", False)
            Me.Hide()
            objFrmSonata = New FrmSonataEpay
            objFrmSonata.Show()


            '  Me.Close()

           

        End If
    End Sub

    'Private Sub Generate_SettingFile()
    '    Dim strConverterCaption As String = ""
    '    Dim strSettingsFilePath As String = My.Application.Info.DirectoryPath & "\settings.ini"


    '    Try
    '        objGetSetINI = New ClsShared

    '        '-Genereate Settings.ini File-
    '        If Not File.Exists(strSettingsFilePath) Then
    '            '-General Section-
    '            Call objGetSetINI.SetINISettings("General", "Date", Format(Now, "dd/MM/yyyy"), strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Audit Log", My.Application.Info.DirectoryPath & "\Audit", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Error Log", My.Application.Info.DirectoryPath & "\Error", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Input Folder", My.Application.Info.DirectoryPath & "\Input", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Output", My.Application.Info.DirectoryPath & "\Output", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Validation File", My.Application.Info.DirectoryPath & "\Validation\Sonata_Validation.xls", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Archived FolderSuc", My.Application.Info.DirectoryPath & "\Archive\Success", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Archived FolderUnSuc", My.Application.Info.DirectoryPath & "\Archive\UnSuccess", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Temp Folder", My.Application.Info.DirectoryPath & "\Temp", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Converter Caption", "SONATA EPAY Convertor", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Process Output File Ignoring Invalid Transactions", "N", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "Input Date Format", "DD/MM/YYYY", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "File Counter", "0", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("General", "==", "==========================================", strSettingsFilePath) 'Separator

    '            '-Client Details Section-
    '            Call objGetSetINI.SetINISettings("Client Details", "Client Name", "SONATA FINANCE", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("Client Details", "Client Code", "SONATA FINANCE", strSettingsFilePath)

    '            Call objGetSetINI.SetINISettings("Client Details", "==", "==========================================", strSettingsFilePath) 'Separator

    '            Call objGetSetINI.SetINISettings("Client Details", "User Name", "", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("Client Details", "Txn Ref No", "1", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("Client Details", "==", "==========================================", strSettingsFilePath) 'Separator
    '            Call objGetSetINI.SetINISettings("Client Details", "UI convertor(Y/N)", "Y", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("Client Details", "Number Of Records In Per Output File", "", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("Client Details", "==", "==========================================", strSettingsFilePath) 'Separator

    '            'YBL Encryption
    '            Call objGetSetINI.SetINISettings("YBL Encryption", "Encryption required Epay", "N", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("YBL Encryption", "Batch File Path", "C:\encrypt", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("YBL Encryption", "PICKDIR Path", "C:\encrypt\IN", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("YBL Encryption", "DROPDIR Path", "C:\encrypt\OUT", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("YBL Encryption", "CRCDIR Path", "C:\encrypt\CRC", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("YBL Encryption", "Encryption Time", "30", strSettingsFilePath)
    '            Call objGetSetINI.SetINISettings("YBL Encryption", "==", "==========================================", strSettingsFilePath) 'Separator
    '        End If

    '        '-Get Converter Caption from Settings-
    '        If File.Exists(strSettingsFilePath) Then
    '            strConverterCaption = objGetSetINI.GetINISettings("General", "Converter Caption", strSettingsFilePath)
    '            If strConverterCaption <> "" Then
    '                Text = strConverterCaption.ToString() & " - Version " & Mid(Application.ProductVersion.ToString(), 1, 3)
    '            Else
    '                MsgBox("Either settings.ini file does not contains the key as [ Converter Caption ] or the key value is blank" & vbCrLf & "Please refer to " & strSettingsFilePath, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
    '                End
    '            End If
    '        End If

    '    Catch ex As Exception
    '        MsgBox("Error - " & vbCrLf & Err.Description & "[" & Err.Number & "]", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error while Generating settings File")
    '        End

    '    Finally
    '        If Not objGetSetINI Is Nothing Then
    '            objGetSetINI.Dispose()
    '            objGetSetINI = Nothing
    '        End If

    '    End Try

    'End Sub


    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        FrmSonataEpay.Generate_SettingFile()

        objBaseClass = New ClsBase(My.Application.Info.DirectoryPath & "\settings.ini")

        '-Get Settings-
        If FrmSonataEpay.GetAllSettings() = True Then
            MsgBox("Either file path is invalid or any key value is left blank in settings.ini file", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error In Settings")
            Exit Sub
        End If
    End Sub
End Class