using DepositfilesSDK.JSON;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using static DepositfilesSDK.Basic;

namespace DepositfilesSDK
{
    public class DClient : IClient
    {

        public DClient(string Username, string Password, ConnectionSettings Settings = null)
        {
            ServicePointManager.Expect100Continue = true; ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            AccToken = Authentication.Token(Username, Password).Result.data.token;
            ConnectionSetting = Settings;

            if (Settings == null)
            {
                m_proxy = null;
            }
            else
            {
                m_proxy = Settings.Proxy;
                m_CloseConnection = Settings.CloseConnection ?? true;
                m_TimeOut = Settings.TimeOut ?? TimeSpan.FromMinutes(60);
            }
        }

        public DClient(string accessToken, ConnectionSettings Settings = null)
        {
            ServicePointManager.Expect100Continue = true; ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            AccToken = accessToken;
            if (Settings == null)
            {
                m_proxy = null;
            }
            else
            {
                m_proxy = Settings.Proxy;
                m_CloseConnection = Settings.CloseConnection ?? true;
                m_TimeOut = Settings.TimeOut ?? TimeSpan.FromMinutes(60);
            }

        }


        public IFile File(string FileID) => new FileClient(FileID);
        public IFiles Files(string[] FilesID) => new FilesClient(FilesID);
        public IFolder Folder(string FolderID)=> new FolderClient(FolderID);
        public IRoot Root() => new RootClient();


        public async Task<JSON_GetToken> UserInfo()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (System.Net.Http.HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("user/profile", new AuthDictionary())).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? JsonConvert.DeserializeObject<JSON_GetToken>(result, JSONhandler) : throw ShowError(result);
                }
            }
        }


    }
}
