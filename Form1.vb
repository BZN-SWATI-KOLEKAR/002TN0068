Imports System.IO
Imports System.Data
Public Class FrmSonataEpay
    Dim objBaseClass As ClsBase
    Dim objFileValidate As ClsValidation
    Dim objGetSetINI As ClsShared
    Dim StrEncrpt As String = String.Empty

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try

            Timer1.Interval = 1000
            Timer1.Enabled = False

            Conversion_Process()

            Timer1.Enabled = True

        Catch ex As Exception
            objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "Form Load", "Timer1_Tick")
        End Try
    End Sub

    Public Sub Generate_SettingFile()
        Dim strConverterCaption As String = ""
        Dim strSettingsFilePath As String = My.Application.Info.DirectoryPath & "\settings.ini"


        Try
            objGetSetINI = New ClsShared

            '-Genereate Settings.ini File-
            If Not File.Exists(strSettingsFilePath) Then
                '-General Section-
                Call objGetSetINI.SetINISettings("General", "Date", Format(Now, "dd/MM/yyyy"), strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Audit Log", My.Application.Info.DirectoryPath & "\Audit", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Error Log", My.Application.Info.DirectoryPath & "\Error", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Input Folder", My.Application.Info.DirectoryPath & "\Input", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Output", My.Application.Info.DirectoryPath & "\Output", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Validation File", My.Application.Info.DirectoryPath & "\Validation\Sonata_Validation.xls", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Archived FolderSuc", My.Application.Info.DirectoryPath & "\Archive\Success", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Archived FolderUnSuc", My.Application.Info.DirectoryPath & "\Archive\UnSuccess", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Report Folder", My.Application.Info.DirectoryPath & "\Report", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Temp Folder", My.Application.Info.DirectoryPath & "\Temp", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Converter Caption", "SONATA EPAY Convertor", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Process Output File Ignoring Invalid Transactions", "N", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "Input Date Format", "DD/MM/YYYY", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "File Counter", "0", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("General", "==", "==========================================", strSettingsFilePath) 'Separator

                '-Client Details Section-
                Call objGetSetINI.SetINISettings("Client Details", "Client Name", "SONATA FINANCE", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("Client Details", "Client Code", "SONATA FINANCE", strSettingsFilePath)

                Call objGetSetINI.SetINISettings("Client Details", "==", "==========================================", strSettingsFilePath) 'Separator

                Call objGetSetINI.SetINISettings("Client Details", "User Name", "", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("Client Details", "Txn Ref No", "000001", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("Client Details", "==", "==========================================", strSettingsFilePath) 'Separator
                Call objGetSetINI.SetINISettings("Client Details", "UI convertor(Y/N)", "Y", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("Client Details", "Number Of Records In Per Output File", "", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("Client Details", "==", "==========================================", strSettingsFilePath) 'Separator

                'YBL Encryption
                Call objGetSetINI.SetINISettings("YBL Encryption", "Encryption required Epay", "N", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("YBL Encryption", "Batch File Path", "C:\encrypt", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("YBL Encryption", "PICKDIR Path", "C:\encrypt\IN", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("YBL Encryption", "DROPDIR Path", "C:\encrypt\OUT", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("YBL Encryption", "CRCDIR Path", "C:\encrypt\CRC", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("YBL Encryption", "Encryption Time", "30", strSettingsFilePath)
                Call objGetSetINI.SetINISettings("YBL Encryption", "==", "==========================================", strSettingsFilePath) 'Separator
            End If

            '-Get Converter Caption from Settings-
            If File.Exists(strSettingsFilePath) Then
                strConverterCaption = objGetSetINI.GetINISettings("General", "Converter Caption", strSettingsFilePath)
                If strConverterCaption <> "" Then
                    Text = strConverterCaption.ToString() & " - Version " & Mid(Application.ProductVersion.ToString(), 1, 3)
                Else
                    MsgBox("Either settings.ini file does not contains the key as [ Converter Caption ] or the key value is blank" & vbCrLf & "Please refer to " & strSettingsFilePath, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                    End
                End If
            End If

        Catch ex As Exception
            MsgBox("Error - " & vbCrLf & Err.Description & "[" & Err.Number & "]", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error while Generating settings File")
            End

        Finally
            If Not objGetSetINI Is Nothing Then
                objGetSetINI.Dispose()
                objGetSetINI = Nothing
            End If

        End Try

    End Sub

    Public Function GetAllSettings() As Boolean

        Try
            GetAllSettings = False

            If Not File.Exists(My.Application.Info.DirectoryPath & "\settings.ini") Then
                GetAllSettings = True
                MsgBox("Either settings.ini file does not exists or invalid file path" & vbCrLf & My.Application.Info.DirectoryPath & "\settings.ini", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            End If

            '-Audit Folder Path-
            If strAuditFolderPath = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Audit Log folder" & vbCrLf & "Please check settings.ini file, the key as [ Audit Log ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strAuditFolderPath) Then
                    Directory.CreateDirectory(strAuditFolderPath)
                    If Not Directory.Exists(strAuditFolderPath) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Audit Log folder. Please check settings.ini file, the key as [ Audit Log ] contains invalid path specification", True)
                        End If
                        MsgBox("Invalid path for Audit Log folder" & vbCrLf & "Please check settings.ini file, the key as [ Audit Log ] contains invalid path specification", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                        Exit Function
                    End If
                End If
            End If

            '-Error Folder Path-
            If strErrorFolderPath = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Error Log folder" & vbCrLf & "Please check settings.ini file, the key as [ Error Log ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strErrorFolderPath) Then
                    Directory.CreateDirectory(strErrorFolderPath)
                    If Not Directory.Exists(strErrorFolderPath) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Error Log folder. Please check settings.ini file, the key as [ Error Log ] contains invalid path specification.", True)
                        End If
                        MsgBox("Invalid path for Error Log folder." & vbCrLf & "Please check settings.ini file, the key as [ Error Log ] contains invalid path specification.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                    End If
                End If
            End If

            '-Input Folder Path-
            If strInputFolderPath = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Input Folder " & vbCrLf & "Please check settings.ini file, the key as [ Input Folder ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strInputFolderPath) Then
                    Directory.CreateDirectory(strInputFolderPath)
                    If Not Directory.Exists(strInputFolderPath) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Input Folder. Please check settings.ini file, the key as [ Input Folder ] contains invalid path specification.", True)
                        End If
                        MsgBox("Invalid path for Input Folder", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "settings Error")
                    End If
                End If
            End If


            '-Output Folder Path-
            If strOutputFolderPath = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Output folder Epay" & vbCrLf & "Please check settings.ini file, the key as [ Output Folder Epay ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strOutputFolderPath) Then
                    Directory.CreateDirectory(strOutputFolderPath)
                    If Not Directory.Exists(strOutputFolderPath) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Output Folder Epay. Please check [ settings.ini ] file, the key as [ Output Folder Epay ] contains invalid path specification.", True)
                        End If
                        MsgBox("Invalid path for Output Folder Epay." & vbCrLf & "Please check settings.ini file, the key as [ Output Folder Epay] contains invalid path specification.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                    End If
                End If
            End If



            '-Archived Success Path-
            If strArchivedFolderSuc = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Archived Success folder" & vbCrLf & "Please check settings.ini file, the key as [ Archived Success Folder ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strArchivedFolderSuc) Then
                    Directory.CreateDirectory(strArchivedFolderSuc)
                    If Not Directory.Exists(strArchivedFolderSuc) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Archived Success Please check [ settings.ini ] file, the key as [ Archived Success Folder ] contains invalid path specification.", True)
                        End If
                        MsgBox("Invalid path for Archived Success Folder." & vbCrLf & "Please check settings.ini file, the key as [ Archived Success Folder ] contains invalid path specification.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                    End If
                End If
            End If

            '-Archived Unsuccess Path-
            If strArchivedFolderUnSuc = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Archived Unsuccess folder" & vbCrLf & "Please check settings.ini file, the key as [ Archived Unsuccess Folder ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strArchivedFolderUnSuc) Then
                    Directory.CreateDirectory(strArchivedFolderUnSuc)
                    If Not Directory.Exists(strArchivedFolderUnSuc) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Archived Unsuccess Folder. Please check [ settings.ini ] file, the key as [ Archived Unsuccess Folder ] contains invalid path specification.", True)
                        End If
                        MsgBox("Invalid path for Archived Unsuccess Folder." & vbCrLf & "Please check settings.ini file, the key as [ Archived Unsuccess Folder ] contains invalid path specification.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                    End If
                End If
            End If

            '-Temp Folder Path-
            If strTempFolderPath = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Temp folder" & vbCrLf & "Please check settings.ini file, the key as [ Temp Folder ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strTempFolderPath) Then
                    Directory.CreateDirectory(strTempFolderPath)
                    If Not Directory.Exists(strTempFolderPath) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Temp Folder. Please check [ settings.ini ] file, the key as [ Temp Folder ] contains invalid path specification.", True)
                        End If
                        MsgBox("Invalid path for Temp Folder." & vbCrLf & "Please check settings.ini file, the key as [ Temp Folder ] contains invalid path specification.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                    End If
                End If
            End If

            ''-Report Folder Path-
            If strReportFolderPath = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Report folder" & vbCrLf & "Please check settings.ini file, the key as [ Report Folder ] is either does not exist or left blank", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not Directory.Exists(strReportFolderPath) Then
                    Directory.CreateDirectory(strReportFolderPath)
                    If Not Directory.Exists(strReportFolderPath) Then
                        GetAllSettings = True
                        If Not objBaseClass Is Nothing Then
                            objBaseClass.LogEntry("Error in settings.ini file, Invalid path for Report Folder. Please check settings.ini file, the key as [ Report Folder ] contains invalid path specification.", True)
                        End If
                        MsgBox("Invalid path for Report Folder." & vbCrLf & "Please check settings.ini file, the key as [ Report Folder ] contains invalid path specification.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                    End If
                End If
            End If

            '-Validation File Path-
            If strValidationPath = "" Then
                GetAllSettings = True
                MsgBox("Path is blank for Validation file." & vbCrLf & "Please check settings.ini file, the key as [ Validation ] is either does not exist or left blank.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                Exit Function
            Else
                If Not File.Exists(strValidationPath) Then
                    GetAllSettings = True
                    If Not objBaseClass Is Nothing Then
                        objBaseClass.LogEntry("Error in settings.ini file, Validation file does not exist or invalid file path", True)
                    End If
                    MsgBox("Validation file does not exist or invalid file path" & vbCrLf & strValidationPath, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in settings.ini file")
                End If
            End If




        Catch ex As Exception
            GetAllSettings = True
            'MsgBox("Error - " & vbCrLf & Err.Description & "[" & Err.Number & "]", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error While Getting Log Path from Settings.ini File")
            objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "Form", "GetAllSettings")

        End Try

    End Function

    Private Sub Conversion_Process()
        Dim objfolderAll As DirectoryInfo
        Dim readText() As String

        Try
            If objBaseClass Is Nothing Then
                objBaseClass = New ClsBase(My.Application.Info.DirectoryPath & "\settings.ini")
            End If

            '-Get Settings-
            If GetAllSettings() = True Then
                MsgBox("Either file path is invalid or any key value is left blank in settings.ini file", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error In Settings")
                Exit Sub
            End If
            ''Check UserName Valid or not
            'If strUserName = "" Then
            '    MsgBox("UserName blank in settings.ini file", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")
            '    Exit Sub

            'Else
            '    If IsNumeric(strUserName) Then
            '        If Len(strUserName) >= 4 Then
            '            gstrUserName = strUserName.Substring(strUserName.Length - 4, 4)
            '        Else
            '            gstrUserName = strUserName
            '        End If

            '    ElseIf CheckForAlphaCharacters(strUserName) Then
            '        readText = strUserName.Split(" ")
            '        gstrUserName = ""
            '        For Each str As String In readText
            '            strUserName = str.Substring(0, 1).ToUpper
            '            gstrUserName = gstrUserName & strUserName
            '        Next

            '    ElseIf CheckForAlphaCharacters(strUserName) = False Then
            '        If Len(strUserName) >= 4 Then
            '            gstrUserName = strUserName.Substring(strUserName.Length - 4, 4)
            '        Else
            '            gstrUserName = strUserName
            '        End If
            '    Else

            '    End If

            'End If

            '-Process Input-
            If strTypeOfConvertor.ToUpper.Trim = "N" Then
                objfolderAll = New DirectoryInfo(strInputFolderPath)
                If objfolderAll.GetFiles.Length = 0 Then
                    objfolderAll = Nothing
                Else
                    LblStatus.Visible = True
                    objBaseClass.LogEntry("", False)
                    objBaseClass.LogEntry("Process Started for INPUT Files")
                    LblStatus.Text = "Process Started for INPUT Files"

                    For Each objFileOne As FileInfo In objfolderAll.GetFiles()
                        objBaseClass.isCompleteFileAvailable(objFileOne.FullName)
                        If Mid(objFileOne.FullName, objFileOne.FullName.Length - 3, 4).ToString().ToUpper() <> ".XLS" And Mid(objFileOne.FullName, objFileOne.FullName.Length - 4, 5).ToString().ToUpper() <> ".XLSX" Then
                            objBaseClass.LogEntry("Invalid File Format", False)
                            LblStatus.Text = "Invalid File Format"
                        Else
                            objBaseClass.LogEntry("", False)
                            objBaseClass.LogEntry("INPUT File [ " & objFileOne.Name & " ] -- Started At -- " & Format(Date.Now, "hh:mm:ss"), False)

                            Process_Each(objFileOne.FullName)

                            objfolderAll.Refresh()
                        End If
                    Next
                End If
            Else
                Process_Each(TxtFilePath.Text)
            End If

            '-Error Log Link-
            If blnErrorLog = True Then
                LinkError.Visible = True
            Else
                LinkError.Visible = False
            End If
            '-Audit Log Link-
            If blnAuditLog = True Then
                LinkAudit.Visible = True
            Else
                LinkAudit.Visible = False
            End If

            '-Output Log Link-
            If blnOpLog = True Then
                LinkOutput.Visible = True
            Else
                LinkOutput.Visible = False
            End If

        Catch ex As Exception
            objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "Form", "Conversion_Process")

        Finally
            If Not objBaseClass Is Nothing Then
                objBaseClass.Dispose()
                objBaseClass = Nothing
            End If
        End Try
    End Sub

    Private Sub Process_Each(ByVal strInputFileName As String)
        Dim TrnProcSuc As Boolean = False
        Try
            gstrInputFolder = strInputFileName.Substring(0, strInputFileName.LastIndexOf("\"))
            gstrInputFile = strInputFileName.Substring(strInputFileName.LastIndexOf("\"))
            gstrInputFile = gstrInputFile.Replace("\", "")

            TxnRefNo = objBaseClass.GetINISettings("Client Details", "Txn Ref No", My.Application.Info.DirectoryPath & "\settings.ini")
            If TxnRefNo = "" Then
                objBaseClass.LogEntry("Error in settings.ini file, Transaction No does not exist settings.ini file", True)
                LblStatus.Text = "Error in settings.ini file, Transaction No does not exist settings.ini file"
                Exit Sub
            End If

            CmdGenerate.Enabled = False

            '-Conversion Process-

            objBaseClass.LogEntry("", False)
            objBaseClass.LogEntry("Process Started")
            LblStatus.Text = "Process Started"
            objBaseClass.LogEntry("Reading Input File " & gstrInputFile, False)
            LblStatus.Text = "Reading Input Files " & gstrInputFile

            objFileValidate = New ClsValidation(strInputFileName, objBaseClass.gstrIniPath)

            If objFileValidate.CheckValidateFile(strInputFileName) = True Then

                objBaseClass.LogEntry("Input File Reading Completed Successfully", False)
                LblStatus.Text = "Input File Reading Completed Successfully"

                If (objFileValidate.DtUnSucInputEpay.Rows.Count = 0) Or (strInvalidTrans.ToString().Trim().ToUpper() = "Y") Then
                    objBaseClass.LogEntry("Input File Validated Successfully", False)
                    LblStatus.Text = "Input File Validated Successfully"

                    If objFileValidate.DtInputEpay.Rows.Count > 0 Then

                        objBaseClass.LogEntry("Output File Generation Process Started", False)
                        LblStatus.Text = "Output File Generation Process Started"

                        If GenerateOutPutFile(objFileValidate.DtInputEpay, gstrInputFile) = True Then       ''Generating Output
                            objBaseClass.LogEntry("Output File Generation process failed due to Error", True)
                            objBaseClass.FileMove(gstrInputFolder & "\" & gstrInputFile, strArchivedFolderUnSuc & "\" & gstrInputFile)
                            LblStatus.Text = "Output File Generation process failed due to Error"
                            blnAuditLog = True
                        Else
                            TrnProcSuc = True
                            blnOpLog = True
                            objBaseClass.LogEntry("Output Files is Generated Successfully", False)
                            objBaseClass.FileMove(gstrInputFolder & "\" & gstrInputFile, strArchivedFolderSuc & "\" & gstrInputFile)
                            LblStatus.Text = "Output File [ " & Path.GetFileName(gstrOutputFile) & " ] is Generated Successfully"
                        End If

                    Else
                        objBaseClass.LogEntry("No Valid Record present in Input File")
                        LblStatus.Text = "No Valid Record present in Input File"
                        objBaseClass.FileMove(gstrInputFolder & "\" & gstrInputFile, strArchivedFolderUnSuc & "\" & gstrInputFile)
                        blnAuditLog = True
                    End If
                Else
                    objBaseClass.LogEntry("No Valid Record present in Input File")
                    objBaseClass.FileMove(gstrInputFolder & "\" & gstrInputFile, strArchivedFolderUnSuc & "\" & gstrInputFile)
                    LblStatus.Text = "No Valid Record present in Input File"
                    blnAuditLog = True
                End If

                '-Write Summary Report-
                LinkReport.Visible = True
                Dim strSummaryFileName As String
                strSummaryFileName = Path.GetFileNameWithoutExtension(gstrInputFile)
                objBaseClass.LogEntry("[Writing Transaction Report]")
                LblStatus.Text = "Writing Transaction Report"
                Call Payment_Report()
                objBaseClass.LogEntry("Transaction Report File Generated Successfully")

                If objFileValidate.DtUnSucInputEpay.Rows.Count > 0 Then
                    objBaseClass.LogEntry("Input File contains following Discrepancies")
                    objBaseClass.LogEntry("Writing Instruction failed for  Epay File following ")
                    LblStatus.Text = "Writing Epay Transaction instruction failed in Log"

                    With objFileValidate.DtUnSucInputEpay
                        For Each _dtRow As DataRow In .Rows
                            If _dtRow("Reason").ToString().Trim() <> "" Then
                            End If
                            objBaseClass.LogEntry(_dtRow("Reason").ToString)
                        Next
                    End With
                    blnAuditLog = True
                End If


            Else
                objBaseClass.LogEntry("Invalid Input File")
                LblStatus.Text = gstrInputFile & " is not Valid Input File"
                objBaseClass.FileMove(gstrInputFolder & "\" & gstrInputFile, strArchivedFolderUnSuc & "\" & gstrInputFile)
                LinkAudit.Visible = True
            End If
            If TrnProcSuc <> False Then
                objBaseClass.LogEntry("Process Completed Successfully", False)
                LblStatus.Text = "Process Completed"
                objBaseClass.LogEntry("-------------------------------------------------------------------------------------", False)
            Else
                objBaseClass.LogEntry("Process Completed UnSuccessfully", False)
                LblStatus.Text = "Proccess Terminated"
                objBaseClass.LogEntry("-------------------------------------------------------------------------------------", False)
            End If

        Catch ex As Exception
            objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "Bagic", "CmdProcess_Click")

        Finally
            If Not objFileValidate Is Nothing Then
                objBaseClass.ObjectDispose(objFileValidate.DtInputEpay)
                objBaseClass.ObjectDispose(objFileValidate.DtUnSucInputEpay)
                objBaseClass.ObjectDispose(objFileValidate.DtInputAdvice)
                objBaseClass.ObjectDispose(objFileValidate.DtUnSucInputAdvice)
                objFileValidate.Dispose()
                objFileValidate = Nothing
            End If
        End Try
    End Sub

    Private Sub Payment_Report()
        Dim strSumFileName As String
        Dim Count_SuccRec As Integer = 0
        Dim Count_UnSuccRec As Integer = 0
        Try
            strSumFileName = "Transaction_Report_" & Path.GetFileNameWithoutExtension(gstrInputFile) & ".csv"

            objBaseClass.WriteSummaryTxt(strSumFileName, "")
            objBaseClass.WriteSummaryTxt(strSumFileName, "[" & Format(Now, "dd-MM-yyyy hh:mm:ss") & "]")

            objBaseClass.WriteSummaryTxt(strSumFileName, "Transaction Report for Input File " & gstrInputFile)
            objBaseClass.WriteSummaryTxt(strSumFileName, "Ordering Customer Name,Ordering Account Number,Amount,IFSC Code,Status,Reason")

            For Each row As DataRow In objFileValidate.DtInputEpay.Select()
                objBaseClass.WriteSummaryTxt(strSumFileName, Replace(row("Ordering Customer Name").ToString, ",", "") & "," & Replace(row("Ordering Account Number").ToString, ",", "") & "," & row("Amount").ToString & "," & row("IFSC Code").ToString & ",Successful," & row("Reason").ToString())
                Count_SuccRec += 1

            Next
            For Each row As DataRow In objFileValidate.DtUnSucInputEpay.Select()
                objBaseClass.WriteSummaryTxt(strSumFileName, Replace(row("Ordering Customer Name").ToString, ",", "") & "," & Replace(row("Ordering Account Number").ToString, ",", "") & "," & row("Amount").ToString & "," & row("IFSC Code").ToString & ",UnSuccessful," & row("Reason").ToString())
                Count_UnSuccRec += 1
            Next
            objBaseClass.WriteSummaryTxt(strSumFileName, "Successful Record Count :" & Count_SuccRec)
            objBaseClass.WriteSummaryTxt(strSumFileName, "UnSuccessful Record Count :" & Count_UnSuccRec)
        Catch ex As Exception
            objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "Form", "Payment_Report")

        End Try

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Function CheckForAlphaCharacters(ByVal StringToCheck As String)

        For i = 0 To StringToCheck.Length - 1
            If Not Char.IsLetter(StringToCheck.Chars(i)) Then
                If IsAlphaForUserName(StringToCheck.Chars(i)) Then
                Else
                    Return False
                End If

            End If
        Next
        Return True

    End Function

    Public Function IsAlphaForUserName(ByVal sChr As String) As Boolean
        '-To remove Junk Characters-

        IsAlphaForUserName = sChr Like "[A-Z]" Or sChr Like "[a-z]" _
        Or sChr Like "[.]" Or sChr Like "[,]" Or sChr Like "[;]" Or sChr Like "[:]" _
        Or sChr Like "[<]" Or sChr Like "[>]" Or sChr Like "[?]" Or sChr Like "[/]" _
        Or sChr Like "[']" Or sChr Like "[""]" Or sChr Like "[|]" Or sChr Like "[\]" _
        Or sChr Like "[{]" Or sChr Like "[[]" Or sChr Like "[}]" Or sChr Like "[]]" _
        Or sChr Like "[+]" Or sChr Like "[=]" Or sChr Like "[_]" Or sChr Like "[-]" _
        Or sChr Like "[(]" Or sChr Like "[)]" Or sChr Like "[*]" Or sChr Like "[&]" _
        Or sChr Like "[^]" Or sChr Like "[%]" Or sChr Like "[$]" Or sChr Like "[#]" _
        Or sChr Like "[@]" Or sChr Like "[!]" Or sChr Like "[`]" Or sChr Like "[~]" Or sChr Like "[ ]"

    End Function

    Private Sub FrmSonataEpay_Load(sender As Object, e As EventArgs) Handles Me.Load
        '  Generate_SettingFile()

        LinkAudit.Visible = False
        LinkError.Visible = False
        LinkOutput.Visible = False
        CmdGenerate.Enabled = False
        LinkReport.Visible = False
        blnErrorLog = False

        objBaseClass = New ClsBase(My.Application.Info.DirectoryPath & "\settings.ini")

        '-Get Settings-
        If GetAllSettings() = True Then
            MsgBox("Either file path is invalid or any key value is left blank in settings.ini file", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error In Settings")
            Exit Sub
        End If

        If strTypeOfConvertor.ToUpper().Trim = "N" Then
            Me.Opacity = 0%
            Me.Timer1.Enabled = True
            Me.ShowInTaskbar = False
        Else
            Me.Opacity = 100%
            Me.Timer1.Enabled = False
            Me.ShowInTaskbar = True
        End If
    End Sub

    Private Sub CmdSelect_Click(sender As Object, e As EventArgs) Handles CmdSelect.Click
        Dim sFileName As String
        LinkAudit.Visible = False
        LinkError.Visible = False
        LinkOutput.Visible = False
        LinkReport.Visible = False
        LblStatus.Text = ""
        LblStatus.Visible = False
        blnErrorLog = False

        Try

            'CmdSelectGEFU.Enabled = False
            blnErrorLog = False

            '-Showing Folder Dialog Box
            With dlgDialog1

                .Filter = "Excel Files (.xls, .xlsx)|*.xls;*.xlsx"
                .ShowDialog()

                '.Filter = "Text Files|*.txt"
                '.ShowDialog()
                sFileName = .FileName
                '  strInputFolderPath = .FileName
            End With
            TxtFilePath.Text = sFileName
            If TxtFilePath.Text.ToString().Trim() <> "" Then
                CmdGenerate.Enabled = True
            End If

            If sFileName = "" Then
                MsgBox("::::: Select File :::::", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error while File Selection")
                Exit Sub
            End If




            TxtFilePath.Text = sFileName

            'CmdSelect.Enabled = True
            'strInputFolderPath = Path.GetDirectoryName(sFileName)

        Catch ex As Exception

            'MsgBox("Error - " & vbCrLf & Err.Description & "[" & Err.Number & "]", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error while File Selection")
            ' objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "FrmTRF", "CmdSelect1_Click")
            MsgBox("Error-" & vbCrLf & Err.Description & "[" & Err.Number & "]", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error While File Selection")
        End Try
    End Sub

    Private Sub CmdGenerate_Click(sender As Object, e As EventArgs) Handles CmdGenerate.Click
        If TxtFilePath.Text.ToString().Trim() = "" Then
            MsgBox(" Please select Input File Path")
            Return
        End If
        gstrOutputFile = ""


        '  strInputFolderPath = TxtFilePath.Text


        Conversion_Process()

        ' TxtFilePath.Text = ""


        '  LblStatus.Text = ""
        CmdSelect.Enabled = True
        CmdGenerate.Enabled = True
    End Sub

    Private Sub LinkError_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkError.LinkClicked
        If Directory.Exists(strErrorFolderPath) Then
            System.Diagnostics.Process.Start(strErrorFolderPath)
        End If
    End Sub

    Private Sub LinkOutput_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkOutput.LinkClicked
        If Directory.Exists(strOutputFolderPath) Then
            System.Diagnostics.Process.Start(strOutputFolderPath)
        End If
    End Sub

    Private Sub LinkAudit_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkAudit.LinkClicked
        If Directory.Exists(strAuditFolderPath) Then
            System.Diagnostics.Process.Start(strAuditFolderPath)
        End If
    End Sub

    Private Sub LinkReport_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkReport.LinkClicked
        If Directory.Exists(strReportFolderPath) Then
            System.Diagnostics.Process.Start(strReportFolderPath)
        End If
    End Sub
End Class
