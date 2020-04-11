using DepositfilesSDK.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static DepositfilesSDK.Basic;

namespace DepositfilesSDK
{
    public class FileClient : IFile
    {

        private string FileID { get; set; }
        public FileClient(string FileID) => this.FileID = FileID;


        public async Task<bool> Delete()
        {
            var parameters = new AuthDictionary() { { "file_id", FileID } };
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("file/delete", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data").Value<bool>("result") : throw ShowError(result);
                }
            }
        }

        public async Task<bool> Move(string DestinationFolderID)
        {
            var parameters = new Dictionary<string, string>
            {
                { "destination_folder_id", string.IsNullOrEmpty(DestinationFolderID) ? "_root" : DestinationFolderID },
                { "file_id", FileID },
                { "token", AccToken }
            };
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("file/move")) { Content = encodedContent };
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data").Value<bool>("result") : throw ShowError(result);
                }
            }
        }

        public async Task<string> Rename(string NewName)
        {
            var parameters = new AuthDictionary();
            parameters.Add("file_id", FileID);
            parameters.Add("name", NewName);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("file/rename", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data.name").ToString() : throw ShowError(result);
                }
            }
        }

        public async Task<bool> Lock(string Password)
        {
            var parameters = new AuthDictionary
            {
                { "file_id", FileID },
                { "password", Password }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("file/edit", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (result.CheckStatus())
                    {
                        return result.Jobj().SelectToken("data.file_id").ToString() == FileID ? true : false;
                    }
                    else
                    {
                        throw ShowError(result);
                    }
                }
            }
        }

        public async Task<bool> Unlock()
        {
            var parameters = new AuthDictionary
            {
                { "file_id", FileID },
                { "password", string.Empty }
            };
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("file/edit")) { Content = encodedContent };
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (result.CheckStatus())
                    {
                        return result.Jobj().SelectToken("data.file_id").ToString() == FileID ? true : false;
                    }
                    else
                    {
                        throw ShowError(result);
                    }
                }
            }
        }

        public async Task<string> GetThumb()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new Uri($"https://depositfiles.com/thumb/{FileID}")).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var fin = Utilitiez.ExtractLinks(result, true);
                        return fin.Count.Equals(0) ? null : fin[0];
                    }
                    else
                    {
                        throw new DepositfilesException(response.ReasonPhrase, (int)response.StatusCode);
                    }
                }
            }
        }

        public async Task<JSON_GetDownloadToken> GetDownloadToken()
        {
            var parameters = new Dictionary<string, string> { { "file_id", FileID }, { "token", AccToken } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("download/file")) { Content = encodedContent };
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? JsonConvert.DeserializeObject<JSON_GetDownloadToken>(result, JSONhandler) : throw ShowError(result);
                }
            }
        }


    }
}