using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static DepositfilesSDK.Basic;

namespace DepositfilesSDK
{
  public   class FilesClient : IFiles
    {

        private string[] FilesID { get; set; }
        public FilesClient(string[] FilesID) => this.FilesID = FilesID;


        public async Task<bool> Delete()
        {
            var parameters = new List<KeyValuePair<string, string>>();
            FilesID.ToList().ForEach(x => parameters.Add(new KeyValuePair<string, string>("file_id[]", x)));
            parameters.Add(new KeyValuePair<string, string>("token", AccToken));
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("file/delete")) { Content = encodedContent };
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data[0]").Value<bool>("result") : throw ShowError(result);
                }
            }
        }

        public async Task<bool> Move( string DestinationFolderID)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            FilesID.ToList().ForEach(x => parameters.Add(new KeyValuePair<string, string>("file_id[]", x)));
            parameters.Add(new KeyValuePair<string, string>("destination_folder_id", DestinationFolderID));
            parameters.Add(new KeyValuePair<string, string>("token", AccToken));
            var encodedContent = new System.Net.Http.FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("file/move")) { Content = encodedContent };
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? result.Jobj().SelectToken("data").FirstOrDefault().Value<bool>("result") : throw ShowError(result);
                }
            }
        }



    }
}
