Imports System
Imports System.Data
Imports System.IO

Public Class ClsValidation
    Implements IDisposable


    Private ObjBaseClass As ClsBase                 ''need to be dispose
    Private DtValidation As DataTable               ''need to be dispose
    Private DtMaster As DataTable
    Private DtSpCharValidation As DataTable

    ''need to be dispose
    Private DtTempRev As DataTable
    Public DtInputEpay As DataTable                     ''need to be dispose
    Public DtUnSucInputEpay As DataTable                ''need to be dispose
    Public DtInputAdvice As DataTable                     ''need to be dispose
    Public DtUnSucInputAdvice As DataTable                ''need to be dispose
    Public DtTemp As DataTable                 ''need to be dispose

   
    Private StrFilePath As String
    Private ValidationPath As String
    Public ErrorMessage As String

    Public Sub New(ByVal _strFilePath As String, ByVal _SettINIPath As String)
        StrFilePath = _strFilePath
        Try
            ObjBaseClass = New ClsBase(_SettINIPath)
            ValidationPath = ObjBaseClass.GetINISettings("General", "Validation File", _SettINIPath)


            DtInputEpay = New DataTable("Input")
            DefineColumnForEpay(DtInputEpay)
            DtUnSucInputEpay = New DataTable("UnSucInput")
            DefineColumnForEpay(DtUnSucInputEpay)


        Catch ex As Exception
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "Constructor")
        End Try
    End Sub

    Private Sub DefineColumnForEpay(ByRef DtInput As DataTable)
        DtInput.Columns.Add(New DataColumn("Detail Indicator")) ''0
        DtInput.Columns.Add(New DataColumn("Msg Type")) ''1
        DtInput.Columns.Add(New DataColumn("Ordering Account Number"))   ''2
        DtInput.Columns.Add(New DataColumn("Ordering Customer Name"))    ''3
        DtInput.Columns.Add(New DataColumn("Address Line 1")) ''4
        DtInput.Columns.Add(New DataColumn("Address Line 2"))  ''5
        DtInput.Columns.Add(New DataColumn("Address Line 3"))   ''6
        DtInput.Columns.Add(New DataColumn("IFSC Code"))   ''7
        DtInput.Columns.Add(New DataColumn("Bene Account No"))   ''8
        DtInput.Columns.Add(New DataColumn("Bene Name"))   ''9
        DtInput.Columns.Add(New DataColumn("Bene Add Line 1"))   ''10
        DtInput.Columns.Add(New DataColumn("Bene Add Line 2"))   ''11
        DtInput.Columns.Add(New DataColumn("Bene Add Line 3")) ''12
        DtInput.Columns.Add(New DataColumn("Bene Add Line 4"))    ''13
        DtInput.Columns.Add(New DataColumn("Txn Ref No"))    ''14
        DtInput.Columns.Add(New DataColumn("Date"))    ''15
        DtInput.Columns.Add(New DataColumn("Amount"))    ''16
        DtInput.Columns.Add(New DataColumn("Sender To Rcvr Info"))    ''17
        DtInput.Columns.Add(New DataColumn("Add Info 1"))    ''18
        DtInput.Columns.Add(New DataColumn("Add Info 2"))    ''19
        DtInput.Columns.Add(New DataColumn("Add Info 3"))    ''20
        DtInput.Columns.Add(New DataColumn("Add Info 4"))    ''21

        DtInput.Columns.Add(New DataColumn("TXN_NO"))    '22
        DtInput.Columns.Add(New DataColumn("File_NO", System.Type.GetType("System.Int32")))   ''23
        '  DtInput.Columns.Add(New DataColumn("SUBTXN_NO"))    '23
        DtInput.Columns.Add(New DataColumn("Reason"))    '24

    End Sub


    Public Function CheckValidateFile(ByVal strInputFileName As String) As Boolean

        Try
            If Not File.Exists(StrFilePath) Then
                Call ObjBaseClass.Handle_Error(New ApplicationException("Input file path is incorrect or not file found. [" & StrFilePath & "]"), "ClsValidation", -123, "CheckValidateFile")
                CheckValidateFile = False
                Exit Function
            End If

            If File.Exists(ValidationPath) Then
                CheckValidateFile = Validate(strInputFileName)
            Else
                Call ObjBaseClass.Handle_Error(New ApplicationException("Validation file path is incorrect. [" & ValidationPath & "]"), "ClsValidation", -123, "CheckValidateFile")
            End If

        Catch ex As Exception
            CheckValidateFile = False
            ErrorMessage = ex.Message
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "CheckValidateFile")
        End Try

    End Function

    Private Function Validate(ByVal strInputFileName As String) As Boolean

        'Validation for Epay

        Validate = False

        Dim DrValidOutputColumn() As DataRow = Nothing

        Dim InputLineNumber As Int32 = 0
        Dim StrDataRow(24) As String

        Dim ArrDataRow As Object

        Dim IFSCStrt As String = ""

        '  Dim intI As Integer

        Dim TXN_NO As Integer
        Dim SUBTXN_NO As Integer

        Dim intPosField As Integer
        Dim MandatoryPos As Integer
        Dim LengthPosMax As Integer
        Dim CharType As Integer
        Dim HardCode As Integer
        Dim ReplaceSpace As Integer
        Dim TransactionNo As Integer = 0
        Try
            ErrorMessage = ""

            TxnRefNo = ObjBaseClass.GetINISettings("Client Details", "Txn Ref No", My.Application.Info.DirectoryPath & "\settings.ini")
            DtValidation = ObjBaseClass.GetDataTable_ExcelSheet(ValidationPath, "Epay", "")
            RemoveBlankRow(DtValidation)
            DrValidOutputColumn = DtValidation.Select()
            DtTemp = ObjBaseClass.GetDataTable_ExcelSheetInput(strInputFileName, "", "")
            RemoveBlankRow(DtTemp)

            InputLineNumber = 0
            TXN_NO = 0
            SUBTXN_NO = 0

            HardCode = 2
            intPosField = 3
            MandatoryPos = 4
            LengthPosMax = 5
            CharType = 6
            ReplaceSpace = 7

            If DtTemp.Rows.Count > 0 Then
                Dim i As Integer = 0
                For Each ROW As DataRow In DtTemp.Rows
                    InputLineNumber += 1

                    If InputLineNumber > 4 Then
                        ArrDataRow = ROW.ItemArray()
                        ClearArray(StrDataRow)
                        ArrDataRow = ROW.ItemArray

                        ' TXN_NO += 1
                        SUBTXN_NO = 1
                        '  IFSCStrt = ROW(3)
                        For StrIndex As Int32 = 0 To DrValidOutputColumn.Length - 1

                            If Val(DrValidOutputColumn(StrIndex)(intPosField).ToString.Trim()) <> 0 Then
                                StrDataRow(StrIndex) = GetValueFormArray(ArrDataRow, Val(DrValidOutputColumn(StrIndex)(intPosField).ToString.Trim()))
                                If StrDataRow(StrIndex) = "~ERROR~" Then
                                    StrDataRow(24) = StrDataRow(24).ToString() & "Input Line No. :" & InputLineNumber & ", Invalid input field position defined in validation file. [ Reference : Input data array length = " & ArrDataRow.Length & " , Field Position = " & Val(DrValidOutputColumn(StrIndex)(intPosField).ToString.Trim()) & "]" & "| "
                                End If
                            Else
                                StrDataRow(StrIndex) = ""
                            End If

                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Ordering Account Number".ToUpper Then
                                StrDataRow(StrIndex) = DtTemp.Rows(0)(1).ToString.Trim()
                            End If

                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Ordering Customer Name".ToUpper Then
                                StrDataRow(StrIndex) = DtTemp.Rows(2)(1).ToString.Trim()
                            End If

                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Ordering Account Number".ToUpper Or DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Bene Account No".ToUpper Then
                            Else
                                StrDataRow(StrIndex) = StrDataRow(StrIndex).Replace("&", "and")
                            End If

                            'HardCode Value
                            If DrValidOutputColumn(StrIndex)(HardCode).ToString().Trim() <> "" Then
                                StrDataRow(StrIndex) = DrValidOutputColumn(StrIndex)(HardCode).ToString()
                            End If

                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim() = "Txn Ref No" Then
                                StrDataRow(StrIndex) = TxnRefNo.ToString.Trim
                            End If

                            If StrDataRow(StrIndex) <> "" Then
                                StrDataRow(StrIndex) = RemoveJunk(StrDataRow(StrIndex).ToString).Replace("&", "and")
                            End If

                            'Charactervalidation()
                            If Val(DrValidOutputColumn(StrIndex)(CharType).ToString().Trim()) > 0 Then
                                StrDataRow(StrIndex) = IsJustAlpha(StrDataRow(StrIndex).Trim(), Val(DrValidOutputColumn(StrIndex)(CharType).ToString().Trim()), DrValidOutputColumn(StrIndex)(ReplaceSpace).ToString().Trim())
                            End If


                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Msg Type".ToUpper Then
                                Dim Amount As Double = 0
                                If ROW(4).ToString().Trim() <> "" Then
                                    ROW(4) = ROW(4).Replace(",", "").Replace(Convert.ToChar(0).ToString(), "")
                                    Amount = IsJustAlpha(Val(ROW(4).ToString()), 2, "N")
                                    'Else
                                    '    Amount = 0
                                End If

                                IFSCStrt = IsJustAlpha(ROW(3).ToString.Trim(), 6, "N")
                                ' IFSCStrt = ROW(3)
                                IFSCStrt = IFSCStrt.ToString().Trim().Substring(0, 6)
                            
                                If IFSCStrt.ToString().ToUpper().Trim() = "YESB00" Then
                                    StrDataRow(StrIndex) = "A"
                                ElseIf (StrDataRow(StrIndex).ToString().Trim() = "R" And IFSCStrt.ToUpper() <> "YESB00" And Amount >= 200000) Then
                                    StrDataRow(StrIndex) = "R41"
                                Else
                                    StrDataRow(StrIndex) = "N06"
                                End If
                            End If


                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Date".ToUpper Then
                                Dim str As String = Now.Date

                                If GetValidateDate(str) = True Then
                                    StrDataRow(StrIndex) = Format(CDate(str), "dd\/MM\/yyyy")
                                Else
                                    StrDataRow(24) = StrDataRow(24) & "Input Line " & InputLineNumber & "column Name " & DrValidOutputColumn(StrIndex)(1).ToString().Trim() & "[" & StrDataRow(StrIndex) & "] Is Not Valid Date Format|"
                                End If
                            End If

                          

                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim() = "Amount" Then
                                StrDataRow(StrIndex) = StrDataRow(StrIndex).ToString().Trim()
                            End If

                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim() = "IFSC Code" Then
                                StrDataRow(StrIndex) = StrDataRow(StrIndex).Replace(" ", "")

                                If StrDataRow(StrIndex).Length() <> 11 Then
                                    StrDataRow(24) = StrDataRow(24) & "Input Line " & InputLineNumber & " IFSC Code [" & StrDataRow(StrIndex) & "] should be 11 Digit|"
                                End If
                            End If

                            If DrValidOutputColumn(StrIndex)(1).ToString().Trim() = "Ordering Account Number" Then
                                StrDataRow(StrIndex) = StrDataRow(StrIndex).Replace(" ", "")
                                If StrDataRow(StrIndex).Length() <> 15 Then
                                    StrDataRow(24) = StrDataRow(24) & "Input Line :" & 1 & "  " & DrValidOutputColumn(StrIndex)(1).ToString().Trim() & "[" & StrDataRow(StrIndex) & "] should be 15 Digit|"
                                End If
                            End If



                            '------------End Here
                            '--------------Check mandatory 
                            If DrValidOutputColumn(StrIndex)(MandatoryPos).ToString().Trim() = "M" And StrDataRow(StrIndex).ToString.Trim() = "" Then
                                If DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Ordering Account Number".ToUpper Then
                                    StrDataRow(24) = StrDataRow(24) & "Input Line : " & 1 & "  " & DrValidOutputColumn(StrIndex)(1).ToString().Trim() & " This is Mandatory Field & it is Blank |"
                                ElseIf (DrValidOutputColumn(StrIndex)(1).ToString().Trim().ToUpper = "Ordering Customer Name".ToUpper) Then
                                    StrDataRow(24) = StrDataRow(24) & "Input Line : " & 3 & "  " & DrValidOutputColumn(StrIndex)(1).ToString().Trim() & " This is Mandatory Field & it is Blank |"
                                Else
                                    StrDataRow(24) = StrDataRow(24) & "Input Line : " & InputLineNumber & "  " & DrValidOutputColumn(StrIndex)(1).ToString().Trim() & " This is Mandatory Field & it is Blank |"
                                End If

                            End If
                            If StrDataRow(StrIndex).Length > Val(DrValidOutputColumn(StrIndex)(LengthPosMax).ToString()) Then
                                StrDataRow(StrIndex) = Left(StrDataRow(StrIndex).PadRight(Val(DrValidOutputColumn(StrIndex)(LengthPosMax).ToString()), ""), Val(DrValidOutputColumn(StrIndex)(LengthPosMax).ToString())).Trim()
                            End If
                        Next

                    
                        TXN_NO += 1
                        StrDataRow(22) = TXN_NO
                        ' StrDataRow(23) = SUBTXN_NO
                        If StrDataRow(24).ToString().Trim() = "" Then
                            TxnRefNo = TxnRefNo + 1
                            If Len(TxnRefNo) < 6 Then
                                TxnRefNo = TxnRefNo.PadLeft(6, "0").Trim()
                            End If

                            If Val(strTransactionNo) <> 0 Then
                                TransactionNo += 1
                                If TransactionNo > Val(strTransactionNo) Then
                                    SUBTXN_NO += 1
                                    TransactionNo = 1
                                End If
                            End If
                            StrDataRow(23) = SUBTXN_NO
                            DtInputEpay.Rows.Add(StrDataRow)
                        Else
                            '  If StrDataRow(24).ToString().Trim() <> "" Then
                            StrDataRow(23) = 0
                            DtUnSucInputEpay.Rows.Add(StrDataRow)
                        End If
                    End If
                Next

                Validate = True

            Else
                Call ObjBaseClass.Handle_Error(New ApplicationException("Validation is not maintained properly in " & Path.GetFileName(ValidationPath) & " validation file. It must be atleast 24 columns defination."), "ClsValidation", -123, "Validate")
            End If

            Validate = True

        Catch ex As Exception
            Validate = False
            ErrorMessage = ex.Message
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "Validate_Epay")
        Finally
            DrValidOutputColumn = Nothing
            ObjBaseClass.ObjectDispose(DtValidation)
            ObjBaseClass.ObjectDispose(DtTemp)
        End Try

    End Function

    

    Private Sub ClearArray(ByRef ArrRow() As String)
        Try
            For i As Integer = 0 To ArrRow.Length - 1
                ArrRow(i) = ""
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Function RemoveBlankRow(ByRef _DtTemp As DataTable)
        'To Remove Blank Row Exists in DataTable
        Dim blnRowBlank As Boolean

        Try
            For Each vRow As DataRow In _DtTemp.Rows
                blnRowBlank = True

                For intCol As Int32 = 0 To _DtTemp.Columns.Count - 1
                    If vRow.Item(intCol).ToString().Trim() <> "" Then
                        blnRowBlank = False
                        Exit For
                    End If
                Next

                If blnRowBlank = True Then
                    _DtTemp.Rows(vRow.Table.Rows.IndexOf(vRow)).Delete()
                End If

            Next
            _DtTemp.AcceptChanges()

        Catch ex As Exception
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "RemoveBlankRow")

        End Try

    End Function

    Public Function GetValidateDate(ByRef pStrDate As String) As Boolean

        Try
           
            strInputDateFormat = strInputDateFormat.ToUpper()

            Dim TmpstrInputDateFormat() As String
            Dim TempStrDateValue() As String = pStrDate.Split(" ")

            If InStr(TempStrDateValue(0), "/") > 0 Then
                TempStrDateValue = TempStrDateValue(0).Split("/")
                If strInputDateFormat.Contains("/") Then
                    TmpstrInputDateFormat = strInputDateFormat.Split("/")
                ElseIf (strInputDateFormat.Contains("-")) Then
                    TmpstrInputDateFormat = strInputDateFormat.Split("-")
                End If

            ElseIf InStr(TempStrDateValue(0), "-") > 0 Then
                TempStrDateValue = TempStrDateValue(0).Split("-")
                TmpstrInputDateFormat = strInputDateFormat.Split("-")
                If strInputDateFormat.Contains("/") Then
                    TmpstrInputDateFormat = strInputDateFormat.Split("/")
                ElseIf (strInputDateFormat.Contains("-")) Then
                    TmpstrInputDateFormat = strInputDateFormat.Split("-")
                End If
            End If

            Dim HsUserDate As New Hashtable
            Dim HsSystemDate As New Hashtable
            Dim StrFinalDate As String

            If TempStrDateValue.Length = 3 Then
                For IntStr As Integer = 0 To TempStrDateValue.Length - 1

                    HsUserDate.Add(GetShort(TmpstrInputDateFormat(IntStr).ToString().Trim()), TempStrDateValue(IntStr))
                Next
                Dim SysDate() As String
                Dim dtSys As String = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToUpper()
                ' Dim dtSys As String = strInputDateFormat
                If InStr(dtSys, "/") > 0 Then
                    SysDate = dtSys.Split("/")
                ElseIf InStr(dtSys, "-") > 0 Then
                    SysDate = dtSys.Split("-")
                End If

                StrFinalDate = ""
                For IntStr As Integer = 0 To SysDate.Length - 1
                    If StrFinalDate = "" Then
                        StrFinalDate += HsUserDate(GetShort(SysDate(IntStr))).ToString().Trim()
                    Else
                        StrFinalDate += "/" & HsUserDate(GetShort(SysDate(IntStr))).ToString().Trim()
                    End If
                Next

                Try
                    
                    pStrDate = CDate(StrFinalDate)

                    GetValidateDate = True

                Catch ex As Exception
                    GetValidateDate = False

                End Try
            Else
                GetValidateDate = False
            End If

        Catch ex As Exception
            GetValidateDate = False

        End Try

    End Function

    Private Function GetShort(ByVal pStr As String) As String

        pStr = pStr.ToUpper

        If InStr(pStr, "D") > 0 Then
            GetShort = "D"
        ElseIf InStr(pStr, "M") > 0 Or InStr(pStr, "MMMM") Then
            GetShort = "M"
        ElseIf InStr(pStr, "Y") > 0 Then
            GetShort = "Y"
        End If

    End Function

    Private Function GetValueFormArray(ByRef pArray() As Object, ByVal pPosition As Int16) As String
        Try

            If pArray.Length >= pPosition Then
                GetValueFormArray = pArray(pPosition - 1).ToString()
            Else
                ErrorMessage = "~ERROR~"
            End If

        Catch ex As Exception
            ErrorMessage = "~ERROR~"
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "GetValueFormArray")
        End Try
    End Function

    Public Function RemoveJunk(ByVal sText As String) As String
        ''-To remove Junk Characters-
        Try
            ''PURPOSE: To return only the alpha chars A-Z or a-z or 0-9 and special chars in a string and ignore junk chars.
            Dim iTextLen As Integer = Len(sText)
            Dim n As Integer
            Dim sChar As String = ""

            If sText <> "" Then
                For n = 1 To iTextLen
                    sChar = Mid(sText, n, 1)
                    If IsAlpha(sChar) Then
                        RemoveJunk = RemoveJunk + sChar
                    End If
                Next
            End If

        Catch ex As Exception
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "RemoveJunk")

        End Try

    End Function

    Public Function IsAlpha(ByVal sChr As String) As Boolean
        '-To remove Junk Characters-

        IsAlpha = sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[0-9]" _
        Or sChr Like "[.]" Or sChr Like "[,]" Or sChr Like "[;]" Or sChr Like "[:]" _
        Or sChr Like "[<]" Or sChr Like "[>]" Or sChr Like "[?]" Or sChr Like "[/]" _
        Or sChr Like "[']" Or sChr Like "[""]" Or sChr Like "[|]" Or sChr Like "[\]" _
        Or sChr Like "[{]" Or sChr Like "[[]" Or sChr Like "[}]" Or sChr Like "[]]" _
        Or sChr Like "[+]" Or sChr Like "[=]" Or sChr Like "[_]" Or sChr Like "[-]" _
        Or sChr Like "[(]" Or sChr Like "[)]" Or sChr Like "[*]" Or sChr Like "[&]" _
        Or sChr Like "[^]" Or sChr Like "[%]" Or sChr Like "[$]" Or sChr Like "[#]" _
        Or sChr Like "[@]" Or sChr Like "[!]" Or sChr Like "[`]" Or sChr Like "[~]" Or sChr Like "[ ]"

    End Function

    

    Public Function IsJustAlpha(ByVal sText As String, ByVal num As Integer, ByVal ReplaceWithSpace As String) As String
        Try
            Dim iTextLen As Integer = Len(sText)
            Dim n As Integer
            Dim sChar As String = ""

            'If sText <> "" Then
            For n = 1 To iTextLen
                sChar = Mid(sText, n, 1)
                If ChkText(sChar, num) Then
                    IsJustAlpha = IsJustAlpha + sChar
                Else
                    If (ReplaceWithSpace = "Y") Then
                        IsJustAlpha = IsJustAlpha + " "
                    End If

                End If
            Next
            'End If

            If Not IsJustAlpha Is Nothing Then
                Return IsJustAlpha
            Else
                Return ""
            End If


        Catch ex As Exception
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "IsJustAlpha")
        End Try
    End Function

    Private Function ChkText(ByVal sChr As String, ByVal num As Integer) As Boolean

        Try
            Select Case num
                Case 1
                    '- name field 
                    ChkText = sChr Like "[A-Z]" Or sChr Like "[a-z]"
                    'ChkText = True
                Case 2
                    '- amount field
                    ChkText = sChr Like "[0-9]" Or sChr Like "[.]" 'Or sChr Like "[,]"
                    'ChkText = True
                Case 3
                    '- alhpa numeric field
                    ChkText = sChr Like "[0-9]" Or sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[,]" Or sChr Like "[/]" Or sChr Like "[\]" Or sChr Like "[ ]" Or sChr Like "[.]" Or sChr Like "[(]" Or sChr Like "[)]" Or sChr Like "[:]"
                    'ChkText = True
                Case 4
                    '- address field
                    ChkText = sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[0-9]" Or sChr Like "[(]" Or sChr Like "[)]" Or sChr Like "[+]" Or sChr Like "[/]" Or sChr Like "[.]" Or sChr Like "[,]" Or sChr Like "[-]" Or sChr Like "[?]" Or sChr Like "[:]" Or sChr Like "[ ]"
                    'ChkText = True
                Case 5
                    '- number field
                    ChkText = sChr Like "[0-9]"
                    'ChkText = True
                Case 6
                    '- alhpa and numeric field
                    ChkText = sChr Like "[0-9]" Or sChr Like "[A-Z]" Or sChr Like "[a-z]"
                    'ChkText = True
                Case 7
                    '- Date field
                    ChkText = sChr Like "[0-9]" Or sChr Like "[:]" Or sChr Like "[/]" Or sChr Like "[\]" Or sChr Like "[-]" Or sChr Like "[.]"
                    'ChkText = True
                Case 8
                    '- alhpa numeric field & All Characters on Keyboard
                    ChkText = sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[0-9]" Or sChr Like "[(]" Or sChr Like "[)]" Or sChr Like "[+]" Or sChr Like "[/]" Or sChr Like "[.]" Or sChr Like "[,]" Or sChr Like "[-]" Or sChr Like "[?]" Or sChr Like "[:]" Or sChr Like "[_]" Or sChr Like "[&]" Or sChr Like "[$]" Or sChr Like "[@]" Or sChr Like "[!]" Or sChr Like "[\]" Or sChr Like "[[]" Or sChr Like "[]]" Or sChr Like "[{]" Or sChr Like "[}]" Or sChr Like "[<]" Or sChr Like "[>]" Or sChr Like "[']" Or sChr Like "[ ]" Or sChr Like "[;]" Or sChr Like "[#]" Or sChr Like "[%]" Or sChr Like "[^]" Or sChr Like "[*]" Or sChr Like "[=]" Or sChr Like "[|]"
                    'ChkText = True
                Case 9
                    '- alhpa and numeric field
                    ChkText = sChr Like "[0-9]" Or sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[ ]"
                Case 10
                    '- alhpa and numeric field
                    ChkText = sChr Like "[0-9]" Or sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[-]" Or sChr Like "[ ]" Or sChr Like "[_]"

                Case 11
                    '- alhpa numeric field
                    ChkText = sChr Like "[0-9]" Or sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[,]" Or sChr Like "[ ]" Or sChr Like "[.]"
                Case 12
                    '- address field
                    ChkText = sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[0-9]" Or sChr Like "[{]" Or sChr Like "[}]" Or sChr Like "[|]" Or sChr Like "[!]" Or sChr Like "[#]" Or sChr Like "[@]" Or sChr Like "[-]" Or sChr Like "[?]" Or sChr Like "[:]" Or sChr Like "[%]" Or sChr Like "[ ]"
                    'ChkText = True
                Case 13
                    '- name field 
                    ChkText = sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[ ]"
                Case 14
                    '- Bene ID
                    ChkText = sChr Like "[0-9]" Or sChr Like "[A-Z]" Or sChr Like "[a-z]" Or sChr Like "[_]" Or sChr Like "[-]" Or sChr Like "[/]"
                Case 15
                    '- PayDate
                    ChkText = sChr Like "[0-9]" Or sChr Like "[/]" Or sChr Like "[|]" Or sChr Like "[~]"
                Case Else
                    ChkText = False
            End Select

            Return ChkText

        Catch ex As Exception
            Call ObjBaseClass.Handle_Error(ex, "ClsValidation", Err.Number, "ChkText")
        End Try
    End Function
    
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
