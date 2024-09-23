Imports System.IO
Imports System.Text
Imports System.Drawing
Imports Microsoft.Office.Interop
Imports System.Data
Imports System.Diagnostics
Imports Excel = Microsoft.Office.Interop.Excel
Module GenrateOutput
    Dim objLogCls As New ClsErrLog
    Dim objGetSetINI As ClsShared
    Dim objBaseClass As ClsBase
    Dim objFileValidate As ClsValidation

    Public Function GenerateOutPutFile(ByRef dtEPAY As DataTable, ByVal strFileName As String) As Boolean
        Dim gstrA2Afile As String = String.Empty
        Dim DebitAccNo As String

        Try
            objBaseClass = New ClsBase(My.Application.Info.DirectoryPath & "\settings.ini")
            objFileValidate = New ClsValidation("", My.Application.Info.DirectoryPath & "\settings.ini")

            FileCounter = objBaseClass.GetINISettings("General", "File Counter", My.Application.Info.DirectoryPath & "\settings.ini")

            If FileCounter <> "" Then
                FileCounter = FileCounter + 1
                If Len(FileCounter) < 3 Then
                    FileCounter = FileCounter.PadLeft(4, "0").Trim()
                    FileCounter = FileCounter.Substring(FileCounter.Length - 3, 3)
                End If

                'TxnRefNo = objBaseClass.GetINISettings("Client Details", "Txn Ref No", My.Application.Info.DirectoryPath & "\settings.ini")
                'TxnRefNo = TxnRefNo + 1
                'If Len(TxnRefNo) < 6 Then
                '    TxnRefNo = TxnRefNo.PadLeft(6, "0").Trim()

                'End If

                TxnRefNo = dtEPAY.Rows(dtEPAY.Rows.Count - 1)("Txn Ref No")
                TxnRefNo = TxnRefNo + 1
                If Len(TxnRefNo) < 6 Then
                    TxnRefNo = TxnRefNo.PadLeft(6, "0").Trim()

                End If

                strFileName = Path.GetFileNameWithoutExtension(strFileName)
                DebitAccNo = dtEPAY.Rows(0).Item("Ordering Account Number").ToString.ToUpper.Trim().Replace("'", "")
                DebitAccNo = DebitAccNo.Substring(DebitAccNo.Length - 4, 4)

                ' gstrOutputFile_EPAY = "E-PAY" & "_" & DebitAccNo & "_" & strUserName.Substring(0, 2) & "_" & Format(CDate(Now.Date()), "ddMMyyyy") & "_" & FileCounter & ".XLS"

                Dim FileCount As Integer = dtEPAY.DefaultView.ToTable(True, "File_No").Rows.Count
                For index = 1 To FileCount



                    If FileCount = 1 Then
                        gstrOutputFile_EPAY = "E-PAY" & "_" & DebitAccNo & "_" & strUserName.Substring(0, 2) & "_" & Format(CDate(Now.Date()), "ddMMyyyy") & "_" & FileCounter & ".XLS"
                    Else
                        gstrOutputFile_EPAY = "E-PAY" & "_" & DebitAccNo & "_" & strUserName.Substring(0, 2) & "_" & Format(CDate(Now.Date()), "ddMMyyyy") & "_" & FileCounter & "_" & index & ".XLS"
                    End If


                    If Generate_Output_EPAY(dtEPAY, gstrOutputFile_EPAY, index) = False Then
                        GenerateOutPutFile = True
                    Else
                        GenerateOutPutFile = False


                        'Encryption for Epay File
                        If strYBLEncryptionEpayFile.ToString().Trim().ToUpper() = "Y" Then
                            objBaseClass.FileMove(strTempFolderPath & "\" & gstrOutputFile_EPAY, strYBLPICKDPath & "\" & gstrOutputFile_EPAY.Replace(" ", ""))
                            objBaseClass.LogEntry("YBL Encrypting file " & gstrOutputFile_EPAY & " is Started")
                            gstrOutputFile_EPAY = gstrOutputFile_EPAY.Replace(" ", "")

                            objBaseClass.FileDelete(strYBLBatchFilePath & "\" & "Test.bat")
                            Dim stremWriter As New StreamWriter(strYBLBatchFilePath & "\" & "Test.bat")
                            stremWriter.WriteLine("cd\")
                            stremWriter.WriteLine("C:")
                            stremWriter.WriteLine("cd encrypt")
                            stremWriter.WriteLine("encrypt " & strYBLPICKDPath & "\" & gstrOutputFile_EPAY & " " & strYBLDROPDPath & "\" & gstrOutputFile_EPAY & ".enc" & " " & strYBLCRCPPath & "\" & gstrOutputFile_EPAY & ".crc")

                            stremWriter.WriteLine("END")

                            objBaseClass.ObjectDispose(stremWriter)
                            objBaseClass.Execute_Batch_file(strYBLBatchFilePath)
                            objBaseClass.LogEntry("YBL Encrypting file " & gstrOutputFile_EPAY & " is Completed by YBL.")


                            objBaseClass.FileMove(strYBLDROPDPath & "\" & gstrOutputFile_EPAY & ".enc", strOutputFolderPath & "\" & gstrOutputFile_EPAY & ".enc")

                            Threading.Thread.Sleep(Integer.Parse(strEncryptionTime.ToString()))
                            ''--

                            objBaseClass.FileDelete(strYBLPICKDPath & "\" & gstrOutputFile_EPAY)
                            objBaseClass.FileDelete(strYBLCRCPPath & "\" & gstrOutputFile_EPAY & ".crc")

                        Else
                            objBaseClass.FileMove(strTempFolderPath & "\" & gstrOutputFile_EPAY, strOutputFolderPath & "\" & gstrOutputFile_EPAY)
                        End If


                        Call objBaseClass.SetINISettings("General", "File Counter", Val(FileCounter), My.Application.Info.DirectoryPath & "\settings.ini")
                        Call objBaseClass.SetINISettings("Client Details", "Txn Ref No", TxnRefNo, My.Application.Info.DirectoryPath & "\settings.ini")
                    End If
                Next


            End If
        Catch ex As Exception
            GenerateOutPutFile = True
            objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "GenerateOutput", "GenerateOutPutFile")

        Finally

        End Try


    End Function

    Private Function Generate_Output_EPAY(ByRef dt As DataTable, ByVal strFileName As String, ByVal FileNo As Integer) As Boolean

        Dim ExlApp As New Excel.Application
        Dim ExlWb As Excel.Workbook
        Dim ExSht As Excel.Worksheet
        Dim DoubAmount As Double = 0
        Try

            If dt.Rows.Count > 0 Then
                objBaseClass.LogEntry("Epay File Generation Process Start")
                FrmSonataEpay.LblStatus.Text = "Epay File Generation Process Start"

                Dim RowNo As Integer = 1
                Dim ColNo As Integer = 1
                Dim DrRow As DataRow() = Nothing

                ExlApp.Visible = False
                ExlWb = ExlApp.Workbooks.Add
                ExSht = DirectCast(ExlWb.ActiveSheet, Excel.Worksheet)
                ExSht.Name = "Sheet1"
                Dim RecordCount As Integer = 0

                '-Header Section

                ExSht.Cells(RowNo, 1) = "H"
                ExSht.Cells(RowNo, 2) = "'" & Format(Now, "dd\/MM\/yyyy")
                ExSht.Cells(RowNo, 3) = objFileValidate.IsJustAlpha(Path.GetFileNameWithoutExtension(strFileName), 9, "N").PadRight(20, " ").Substring(0, 20).Trim()

                ''--

                '-Details Section
                For Each drRBI As DataRow In dt.Select("File_No=" & FileNo)
                    'strOutputStream = ""
                    RowNo += 1
                    ColNo = 1
                    RecordCount += 1
                    For index = 0 To dt.Columns.Count - 4
                        If dt.Columns(index).ColumnName.ToString.Trim().ToUpper = "Ordering Account Number".ToString.Trim().ToUpper Or dt.Columns(index).ColumnName.ToString.Trim().ToUpper = "Bene Account No".ToString.Trim().ToUpper Or dt.Columns(index).ColumnName.ToString.Trim().ToUpper = "Txn Ref No".ToString.Trim().ToUpper Or dt.Columns(index).ColumnName.ToString.Trim().ToUpper = "Date".ToString.Trim().ToUpper Or dt.Columns(index).ColumnName.ToString.Trim().ToUpper = "Amount".ToString.Trim().ToUpper Then
                            ExSht.Cells(RowNo, ColNo) = "'" & drRBI(index).ToString().Trim() & ""
                        Else
                            ExSht.Cells(RowNo, ColNo) = drRBI(index).ToString().Trim()
                        End If
                        ColNo += 1
                    Next
                    DoubAmount = DoubAmount + Math.Round(Val(drRBI("Amount").ToString().Trim()), 2)
                Next
                '-
                ''Footer0
                RowNo += 1

                ExSht.Cells(RowNo, 1) = "F"
                ExSht.Cells(RowNo, 2) = "'" & Format(RecordCount, "00000")
                ExSht.Cells(RowNo, 3) = "'" & Format(DoubAmount, "00000000000.00")

                Dim iLastRow As Long
                Dim iLastColumn As Long

                iLastRow = ExSht.Cells(ExSht.Rows.Count, 1).End(Excel.XlDirection.xlUp).Row
                iLastColumn = ExSht.Cells(1, ExSht.Columns.Count).End(Excel.XlDirection.xlToLeft).Column

                ExSht.Columns.AutoFit()
                ExlWb.Sheets("Sheet1").Activate()
                ' ExSht.Range("A1").Select()
                ' ExSht.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Select()
                ''  ExSht.Range("A1" & ExSht.AutoFilter.Range.Offset(5, 3).SpecialCells(Excel.XlCellType.xlCellTypeVisible)(1).Row).Select()

                ' ExSht.Range("E" & ExSht.AutoFilter.Range.Offset(1, 0).SpecialCells(Excel.XlCellType.xlCellTypeVisible)(1).Row).Select()

                '  ExSht.Range("C" & RowNo).End(Excel.XlDirection.xlUp).Select
                ''  ExSht.Range("C" & RowNo).Select()
                ''ExSht.Range("A:R").NumberFormat = "@"
                ExSht = Nothing
                ExlApp.DisplayAlerts = False
                ExlWb.SaveAs(strTempFolderPath & "\" & strFileName, Excel.XlFileFormat.xlWorkbookNormal)
                ExlWb.Close()
                ExlApp.Quit()


                objBaseClass.LogEntry("Epay File [" & strFileName & "] Generated Successfully")
                FrmSonataEpay.LblStatus.Text = "Epay File [" & strFileName & "] Generated Successfully"

                Generate_Output_EPAY = True
            Else
                objBaseClass.LogEntry("Epay Record Not Found")
                FrmSonataEpay.LblStatus.Text = "Epay Record Not Found"
            End If


        Catch ex As Exception
            Generate_Output_EPAY = False
            objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "Generate_Output_EPAY", "Generate_Output_EPAY")
            ExlWb.Close()
            ExlApp.Quit()

        End Try
    End Function

    'Public Function Generate_Output_ADV(ByRef dtADV As DataTable, ByVal strFileName As String) As Boolean
    '    Dim strOutPutLine As String
    '    Dim intRowCount As Integer

    '    Dim objStrmWriter As StreamWriter
    '    objStrmWriter = New StreamWriter(strTempAdviceFolderPath & "\" & strFileName)

    '    Try
    '        For Each dtRow As DataRow In dtADV.Rows

    '            strOutPutLine = ""
    '            strOutPutLine = Left(dtRow("Payment Doc No").ToString().Trim(), 13) & "|" 'Payment Doc No    '1
    '            strOutPutLine = strOutPutLine & Left(dtRow("PAYEE NAME").ToString().Trim(), 150) & "|" 'PAYEE NAME  '2
    '            strOutPutLine = strOutPutLine & Left(dtRow("Invoice number").ToString().Trim(), 150) & "|" 'Invoice number  '3
    '            strOutPutLine = strOutPutLine & Left(dtRow("Column1").ToString().Trim(), 35) & "|" 'MISC_FIELD 2   '4
    '            strOutPutLine = strOutPutLine & Left(dtRow("Document number").ToString().Trim(), 20) & "|" 'Document number  '5
    '            strOutPutLine = strOutPutLine & dtRow("Value Date").ToString().Trim() & "|"  'Value Date   '6
    '            strOutPutLine = strOutPutLine & dtRow("Payment_Date").ToString().Trim() & "|"  'Payment_Date   '7
    '            strOutPutLine = strOutPutLine & Left(dtRow("Invoice amount").ToString().Trim(), 14) & "|"   'Invoice amount  '8
    '            strOutPutLine = strOutPutLine & Left(dtRow("Net Amount").ToString(), 14).Trim() & "|"   'Net Amount  '9
    '            strOutPutLine = strOutPutLine & Left(dtRow("Payment_Amount").ToString().Trim(), 14) & "|"   'Payment_Amount  '10
    '            strOutPutLine = strOutPutLine & Left(dtRow("Gross Adj").ToString().Trim(), 14) & "|"  'Gross Adj  '11
    '            strOutPutLine = strOutPutLine & Left(dtRow("Document date").ToString().Trim(), 14) & "|"   'Document date   '12
    '            strOutPutLine = strOutPutLine & Left(dtRow("Deduction").ToString().Trim(), 10) & "|"   'Deduction   '13
    '            strOutPutLine = strOutPutLine & Left(dtRow("Currency").ToString().Trim(), 10) & "|"   'Currency   '14
    '            strOutPutLine = strOutPutLine & Left(dtRow("PAYMENT DOCUMENT").ToString().Trim(), 35) & "|"   'PAYMENT DOCUMENT   '15
    '            strOutPutLine = strOutPutLine & Left(dtRow("PAN").ToString().Trim(), 10) & "|"   'PAN   '16
    '            strOutPutLine = strOutPutLine & Left(dtRow("Email id").ToString().Trim(), 150) & "|"  ' Email id  '17
    '            strOutPutLine = strOutPutLine & Left(dtRow("RETENTION").ToString().Trim(), 15) & "|"  'RETENTION   '18

    '            objStrmWriter.WriteLine(strOutPutLine, strFileName)

    '        Next
    '        objBaseClass.LogEntry("Output file [" & strFileName & "] is generated successfully", False)

    '        Generate_Output_ADV = True



    '    Catch ex As Exception
    '        Generate_Output_ADV = False
    '        objBaseClass.WriteErrorToTxtFile(Err.Number, Err.Description, "Generate_Output", "GenerateOutPutFile")
    '    Finally
    '        If Not objStrmWriter Is Nothing Then
    '            objStrmWriter.Close()
    '            objStrmWriter.Dispose()

    '        End If
    '    End Try
    'End Function
End Module


