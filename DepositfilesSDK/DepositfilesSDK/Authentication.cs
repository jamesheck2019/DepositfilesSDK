using DepositfilesSDK.JSON;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static DepositfilesSDK.Basic;

namespace DepositfilesSDK
{
    public class Authentication
    {

        public static async Task<JSON_GetToken> Token(string Username, string Password)
        {
            ServicePointManager.Expect100Continue = true; ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            var parameters = new Dictionary<string, string>() { { "login", Username }, { "password", Password } };
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("user/login", parameters)).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result.CheckStatus() ? JsonConvert.DeserializeObject<JSON_GetToken>(result, JSONhandler) : throw ShowError(result);
                }
            }
        }
    }

}
