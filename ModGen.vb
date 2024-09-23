Option Explicit On

Module ModGen

    Public blnErrorLog As Boolean
    Public strAuditFolderPath As String
    Public strErrorFolderPath As String
    Public strValidationPath As String
    Public strMasterFilePath As String
    Public strInputFolderPath As String
    Public strOutputFolderPath As String
    Public strOutputFolderPath_Epay As String
    Public strOutputFolderPath_Advice As String
    Public strArchivedFolderSuc As String
    Public strArchivedFolderUnSuc As String
    
    Public gstrInputFile As String
    Public gstrInputFolder As String
    Public gstrInputFile2 As String

    Public gstrOutputFile As String
    Public gstrOutputFile_EPAY As String
    Public gstrOutputFile_ADV As String

    Public strProceed As String
    Public strInvalidTrans As String
    Public FileCounter As String


    '  Public blnErrorLog As Boolean = False
    Public blnAuditLog As Boolean = False
    Public blnOpLog As Boolean = False

    '-Client Details-
    Public strClientCode As String
    Public strClientName As String
    Public strInputDateFormat As String
    Public strUserName As String
    Public TxnRefNo As String
    Public strTypeOfConvertor As String     ''Silent','UI'
      Public strTransactionNo As String
    Public strTempFolderPath As String
    Public strReportFolderPath As String            ' Report folder path

    Public gstrUserName As String
    'Encryption
    Public strYBLEncryptionEpayFile As String
    Public strYBLEncryptionAdviceFile As String
    Public strYBLBatchFilePath As String
    Public strYBLPICKDPath As String
    Public strYBLDROPDPath As String
    Public strYBLCRCPPath As String
    Public strEncryptionTime As String
End Module

