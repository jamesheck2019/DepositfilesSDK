using DepositfilesSDK.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static DepositfilesSDK.Basic;
using static DepositfilesSDK.Utilitiez;

namespace DepositfilesSDK
{
    public class FolderClient : IFolder
    {
        private string FolderID { get; set; }
        public FolderClient(string FolderID) => this.FolderID = FolderID;


        public async Task<bool> Delete()
        {
            var parameters = new AuthDictionary() { { "folder_id", FolderID } };
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("folder/delete", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data").Value<bool>("result") : throw ShowError(result);
                }
            }
        }

        public async Task<bool> DeleteParentWithoutSubs()
        {
            var parameters = new AuthDictionary
            {
                { "folder_id", FolderID },
                { "files_action", "2" }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("folder/delete", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data").Value<bool>("result") : throw ShowError(result);
                }
            }
        }

        public async Task<bool> Rename(string NewName)
        {
            var parameters = new AuthDictionary
            {
                { "folder_id", FolderID },
                { "name", NewName }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("folder/rename", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data").Value<bool>("result") : throw ShowError(result);
                }
            }
        }

        public async Task<JSON_SearchFolder> SearchFolder(string SearchKeyword, int OffSet)
        {
            var parameters = new AuthDictionary
            {
                { "folder_id", string.IsNullOrEmpty(FolderID) ? "_root" : FolderID },
                { "pattern", SearchKeyword },
                { "page", OffSet.ToString() }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("file/search", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (result.CheckStatus() && result.Jobj().SelectToken("status").ToString() == "OK")
                    {
                        if (result.Jobj().SelectToken("data").ToString() == "[]")
                        {
                            return new JSON_SearchFolder() { FilesList = new Dictionary<string, JSON_FileMetadata>() };
                        }
                        else
                        {
                            return JsonConvert.DeserializeObject<JSON_SearchFolder>(result, JSONhandler);
                        }
                    }
                    else
                    {
                        throw ShowError(result);
                    }
                }
            }
        }

        private async Task<JSON_GetUploadUrl> GetUploadUrl()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("upload/regular", new AuthDictionary())).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (result.CheckStatus())
                    {
                        var userInfo = JsonConvert.DeserializeObject<JSON_GetUploadUrl>(result, JSONhandler);
                        userInfo.Success = true;
                        return userInfo;
                    }
                    else
                    {
                        throw ShowError(result);
                    }
                }
            }
        }

        public async Task<string> Upload(object FileToUpload, UploadTypes UploadType, string FileName, IProgress<ReportStatus> ReportCls = null, System.Threading.CancellationToken token = default)
        {
            var uploadUrl = await GetUploadUrl();

            ReportCls = ReportCls ?? new Progress<ReportStatus>();
            ReportCls.Report(new ReportStatus() { Finished = false, TextStatus = "Initializing..." });
            try
            {
                System.Net.Http.Handlers.ProgressMessageHandler progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler(new HCHandler());
                progressHandler.HttpSendProgress += (sender, e) => { ReportCls.Report(new ReportStatus() { ProgressPercentage = e.ProgressPercentage, BytesTransferred = e.BytesTransferred, TotalBytes = e.TotalBytes ?? 0, TextStatus = "Uploading..." }); };
                using (HtpClient localHttpClient = new HtpClient(progressHandler))
                {
                    HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(uploadUrl.data.upload_url));
                    var MultipartsformData = new MultipartFormDataContent("--");
                    // 'remove the quotes from the boundary
                    var boundary = MultipartsformData.Headers.ContentType.Parameters.First(o => o.Name == "boundary");
                    boundary.Value = boundary.Value.Replace("\"", string.Empty);
                    // '
                    HttpContent streamContent = null;
                    switch (UploadType)
                    {
                        case UploadTypes.FilePath:
                            streamContent = new StreamContent(new System.IO.FileStream(FileToUpload.ToString(), System.IO.FileMode.Open, System.IO.FileAccess.Read));
                            break;
                        case UploadTypes.Stream:
                            streamContent = new StreamContent((System.IO.Stream)FileToUpload);
                            break;
                        case UploadTypes.BytesArry:
                            streamContent = new StreamContent(new System.IO.MemoryStream((byte[])FileToUpload));
                            break;
                    }
                    streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "\"files\"", FileName = "\"" + FileName + "\"" };
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    MultipartsformData.Add(streamContent);
                    // '''''''''''''''''''''''''
                    MultipartsformData.Add(new StringContent(string.IsNullOrEmpty(FolderID) ? "_root" : FolderID), "\"fm\"");
                    MultipartsformData.Add(new StringContent(AccToken), "\"autologin\"");

                    HtpReqMessage.Content = MultipartsformData;
                    // ''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
                    using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, System.Net.Http.HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
                    {
                        string result = await ResPonse.Content.ReadAsStringAsync();

                        token.ThrowIfCancellationRequested();
                        if (ResPonse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (result.Contains("ud_download_url"))
                            {
                                var parsUrls = (ExtractLinks(result, false)).Where(x => x.Contains("http://depositfiles.com/files/")).Select(x => x).FirstOrDefault(); // .Replace("http://picimage.net/][img]", "").Replace("[/img][/url", "")
                                ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"[{FileName}] Uploaded successfully" });
                                return parsUrls;
                            }
                            else
                            {
                                ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"The request returned with HTTP status code {result}" });
                                return null;
                            }
                        }
                        else
                        {
                            throw new DepositfilesException(ResPonse.ReasonPhrase, (int)ResPonse.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportCls.Report(new ReportStatus() { Finished = true });
                if (ex.Message.ToString().ToLower().Contains("a task was canceled"))
                {
                    ReportCls.Report(new ReportStatus() { TextStatus = ex.Message });
                }
                else
                {
                    throw new DepositfilesException(ex.Message, 1001);
                }
                return null;
            }
        }

        public async Task<JSON_RemoteUpload> RemoteUpload(Uri FileUrl)
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                var RequestUri = new pUri("upload/remote/add", new AuthDictionary() { { "url", FileUrl.ToString() } });
                using (HttpResponseMessage response = await localHttpClient.GetAsync(RequestUri).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? JsonConvert.DeserializeObject<JSON_RemoteUpload>(result, JSONhandler) : throw ShowError(result);
                }
            }
        }

        public async Task<JSON_RemoteUpload> RemoteUploadMultiple(string[] Urls)
        {
            var encodedContent = new FormUrlEncodedContent(new AuthDictionary() { { "url", string.Join(Environment.NewLine, Urls) } });
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("upload/remote/add")) { Content = encodedContent };
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? JsonConvert.DeserializeObject<JSON_RemoteUpload>(result, JSONhandler) : throw ShowError(result);
                }
            }
        }

        public async Task<JSON_List> ListFiles()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                var RequestUri = new pUri("file/listing", new AuthDictionary() { { "folder_id", FolderID } });
                using (HttpResponseMessage response = await localHttpClient.GetAsync(RequestUri).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (result.CheckStatus())
                    {
                        if (result.Jobj().SelectToken("data").ToString() == "[]")
                        {
                            return new JSON_List() { FilesList = new Dictionary<string, JSON_FileMetadata>() };
                        }
                        else
                        {
                            return JsonConvert.DeserializeObject<JSON_List>(result, JSONhandler);
                        }
                    }
                    else
                    {
                        throw ShowError(result);
                    }
                }
            }
        }


    }
}
