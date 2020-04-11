using Newtonsoft.Json;
using System.Collections.Generic;

namespace DepositfilesSDK.JSON
{

    // // or for all properties in a class
    // <JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)>
    public class Response<TResult>
    {
        public TResult data { get; set; }
    }


    public class JSON_Error
    {
        [JsonProperty("error")] public string _ErrorMessage { get; set; }
        [JsonProperty("error_code")] public int _ErrorCode { get; set; }
    }

    // <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)>


    public class JSON_GetToken : Response<JSON_GetTokenData>
    {
    }
    public class JSON_GetTokenData
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string mode { get; set; }
        public object gold_expired { get; set; }
        public bool is_mail_send_enabled { get; set; }
        public string token { get; set; }
        public string member_passkey { get; set; }
        public string is_reseller { get; set; }
        public int new_messages { get; set; }
        public bool storage_exist { get; set; }
        public string show_notifications { get; set; }
        public string language { get; set; }
        public string active_package { get; set; }
        public int oauth_session { get; set; }
    }


    public class JSON_FolderMetadata
    {
        public string folder_id { get; set; }
        public string name { get; set; }
        [JsonProperty("files_cnt")]public string FilesCount { get; set; }
        public string folder_hash { get; set; }
        public string FolderUrl { get { return string.Format("http://depositfiles.com/folders/{0}", folder_id); } }
    }

    public class JSON_ListFiles
    {
        [JsonProperty("data")]public IDictionary<string, JSON_FileMetadata> FilesList { get; set; }
    }
    public class JSON_FileMetadata
    {
        public string dt_added { get; set; }
        public string filename { get; set; }
        public string file_password { get; set; }
        public string size { get; set; }
        public string folder_id { get; set; }
        public string dt_expires { get; set; }
        public string file_id { get; set; }
        [JsonProperty("download_cnt")]public int DownloadCount { get; set; }
        public string FileUrl { get { return string.Format("http://depositfiles.com/files/{0}", file_id); } }
    }

    // Public Class JSON_ListTEMP
    // <JsonProperty("error", NullValueHandling:=NullValueHandling.Ignore)> Public Property _ErrorMessage As String
    // Public Property status As String
    // Public Property data As Object
    // End Class
    public class JSON_List
    {
        [JsonProperty("data")] public Dictionary<string, JSON_FileMetadata> FilesList { get; set; }
    }



    public class JSON_GetDownloadToken : Response<JSON_GetDownloadTokenData>
    {
    }
    public class JSON_GetDownloadTokenData
    {
        public string mode { get; set; }
        public int delay { get; set; }
        public string download_token { get; set; }
    }

    public class JSON_GetUploadUrl
    {
        public bool Success { get; set; }
        public JSON_GetUploadUrlData data { get; set; }
    }
    public class JSON_GetUploadUrlData
    {
        public string upload_url { get; set; }
        public string progress_url { get; set; }
        public int max_file_size_mb { get; set; }
    }

    public class JSON_RemoteUpload : Response<List<JSON_RemoteUploadDatum>>
    {
    }
    public class JSON_RemoteUploadDatum
    {
        public string url { get; set; }
        [JsonProperty("task_id")]public long JobID { get; set; } // 'JOB ID
    }

    public class JSON_SearchFolder
    {
        [JsonProperty("data")]public Dictionary<string, JSON_FileMetadata> FilesList { get; set; }
    }
}


