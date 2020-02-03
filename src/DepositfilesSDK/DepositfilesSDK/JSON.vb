Imports Newtonsoft.Json
Imports System.ComponentModel

Namespace JSON

    '// or for all properties in a class
    '<JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)>
    Public Class Response(Of TResult)
        Public Property data As TResult
    End Class


    Public Class JSON_Error
        <JsonProperty("error")> Public Property _ErrorMessage As String
        <JsonProperty("error_code")> Public Property _ErrorCode As Integer
    End Class

    '    <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)>


#Region "JSON_GetToken"
    Public Class JSON_GetToken
        Inherits Response(Of JSON_GetTokenData)
    End Class
    Public Class JSON_GetTokenData
        Public Property user_id As String
        Public Property username As String
        Public Property email As String
        Public Property mode As String
        Public Property gold_expired As Object
        Public Property is_mail_send_enabled As Boolean
        Public Property token As String
        Public Property member_passkey As String
        Public Property is_reseller As String
        Public Property new_messages As Integer
        Public Property storage_exist As Boolean
        Public Property show_notifications As String
        Public Property language As String
        Public Property active_package As String
        Public Property oauth_session As Integer
    End Class
#End Region


#Region "JSON_FolderMetadata"
    Public Class JSON_FolderMetadata
        Public Property folder_id As String
        Public Property name As String
        <JsonProperty("files_cnt", NullValueHandling:=NullValueHandling.Ignore)> Public Property FilesCount As String
        Public Property folder_hash As String
        Public ReadOnly Property FolderUrl As String
            Get
                Return String.Format("http://depositfiles.com/folders/{0}", folder_id)
            End Get
        End Property
    End Class
#End Region

#Region "JSON_ListFiles"
    Public Class JSON_ListFiles
        <JsonProperty("data")> Public Property FilesList As IDictionary(Of String, JSON_FileMetadata)
    End Class
    Public Class JSON_FileMetadata
        Public Property dt_added As String
        Public Property filename As String
        Public Property file_password As String
        Public Property size As String
        Public Property folder_id As String
        Public Property dt_expires As String
        Public Property file_id As String
        <JsonProperty("download_cnt", NullValueHandling:=NullValueHandling.Ignore)> Public Property DownloadCount As Integer
        Public ReadOnly Property FileUrl As String
            Get
                Return String.Format("http://depositfiles.com/files/{0}", file_id)
            End Get
        End Property
    End Class
#End Region

#Region "JSON_List"
    'Public Class JSON_ListTEMP
    '    <JsonProperty("error", NullValueHandling:=NullValueHandling.Ignore)> Public Property _ErrorMessage As String
    '    Public Property status As String
    '    Public Property data As Object
    'End Class
    Public Class JSON_List
        <JsonProperty("data")> Public Property FilesList As Dictionary(Of String, JSON_FileMetadata)
    End Class
#End Region



#Region "JSON_GetDownloadToken"
    Public Class JSON_GetDownloadToken
        Inherits Response(Of JSON_GetDownloadTokenData)
    End Class
    Public Class JSON_GetDownloadTokenData
        Public Property mode As String
        Public Property delay As Integer
        Public Property download_token As String
    End Class
#End Region

#Region "JSON_GetUploadUrl"
    Public Class JSON_GetUploadUrl
        Public Property Success As Boolean
        Public Property data As JSON_GetUploadUrlData
    End Class
    Public Class JSON_GetUploadUrlData
        Public Property upload_url As String
        Public Property progress_url As String
        Public Property max_file_size_mb As Integer
    End Class
#End Region

#Region "JSON_RemoteUpload"
    Public Class JSON_RemoteUpload
        Inherits Response(Of List(Of JSON_RemoteUploadDatum))
    End Class
    Public Class JSON_RemoteUploadDatum
        Public Property url As String
        <JsonProperty("task_id", NullValueHandling:=NullValueHandling.Ignore)> Public Property JobID As long ''JOB ID
    End Class
#End Region

#Region "JSON_SearchFolder"
    Public Class JSON_SearchFolder
        <JsonProperty("data")> Public Property FilesList As Dictionary(Of String, JSON_FileMetadata)
    End Class
#End Region


End Namespace

