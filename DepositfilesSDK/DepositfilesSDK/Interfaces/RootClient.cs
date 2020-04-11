using DepositfilesSDK.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static DepositfilesSDK.Basic;
using static DepositfilesSDK.Utilitiez;

namespace DepositfilesSDK
{
    public class RootClient : IRoot
    {

        public string RootID
        {
            get
            {
                return "_root";
            }
        }

        public async Task<List<JSON_FileMetadata>> SearchFiles(string SearchKeyword, SearchTypeEnum SearchType)
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("file/listing", new AuthDictionary())).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (JObject.Parse(result).SelectToken("status").ToString() == "OK")
                    {
                        if (JObject.Parse(result).SelectToken("data").ToString() == "[]")
                        {
                            return new List<JSON_FileMetadata>();
                        }
                        else
                        {
                            var nfo = JsonConvert.DeserializeObject<JSON_ListFiles>(result, JSONhandler);
                            List<JSON_FileMetadata> lst = new List<JSON_FileMetadata>();
                            switch (SearchType)
                            {
                                case SearchTypeEnum.Contains:
                                    foreach (var onz in nfo.FilesList)
                                    {
                                        if (onz.Value.filename.ToLower().Contains(SearchKeyword.ToLower()))
                                        {
                                            lst.Add(onz.Value);
                                        }
                                    }

                                    break;
                                case SearchTypeEnum.Exact:
                                    foreach (var onz in nfo.FilesList)
                                    {
                                        if (onz.Value.filename.ToLower() == (SearchKeyword.ToLower()))
                                        {
                                            lst.Add(onz.Value);
                                        }
                                    }

                                    break;
                                case SearchTypeEnum.StartWith:
                                    foreach (var onz in nfo.FilesList)
                                    {
                                        if (onz.Value.filename.ToLower().StartsWith(SearchKeyword.ToLower()))
                                        {
                                            lst.Add(onz.Value);
                                        }
                                    }

                                    break;
                                case SearchTypeEnum.EndWith:
                                    foreach (var onz in nfo.FilesList)
                                    {
                                        if (onz.Value.filename.ToLower().EndsWith(SearchKeyword.ToLower()))
                                        {
                                            lst.Add(onz.Value);
                                        }
                                    }

                                    break;
                                case SearchTypeEnum.Ext:
                                    foreach (var onz in nfo.FilesList)
                                    {
                                        if (System.IO.Path.GetExtension(onz.Value.filename).Trim('.') == (SearchKeyword).Trim('.'))
                                        {
                                            lst.Add(onz.Value);
                                        }
                                    }

                                    break;
                            }

                            return lst;
                        }
                    }
                    else
                    {
                        throw ShowError(result);
                    }
                }
            }
        }

        public async Task<JSON_FolderMetadata> CreateNewFolder(string FolderName)
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("folder/add", new AuthDictionary() { { "name", FolderName } })).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    return result.CheckStatus() ? JsonConvert.DeserializeObject<JSON_FolderMetadata>(result.Jobj().SelectToken("data").ToString(), JSONhandler) : throw ShowError(result);
                }
            }
        }

        public async Task<JSON_List> ListFiles()
        {
            using (HttpClient localHttpClient = new HttpClient(new HCHandler()))
            {
                var RequestUri = new pUri("file/listing", new AuthDictionary() { { "folder_id", "_root" } });
                using (HttpResponseMessage response = await localHttpClient.GetAsync(RequestUri).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (result.CheckStatus())
                    {
                        return result.Jobj().SelectToken("data").ToString() == "[]" ? new JSON_List() { FilesList = new Dictionary<string, JSON_FileMetadata>() } : JsonConvert.DeserializeObject<JSON_List>(result, JSONhandler);
                    }
                    else
                    {
                        throw ShowError(result);
                    }
                }
            }
        }

        public async Task<List<JSON_FolderMetadata>> ListFolders()
        {
            using (HttpClient localHttpClient = new HttpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("folder/listing", new AuthDictionary())).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (result.CheckStatus())
                    {
                        return result.Jobj().SelectToken("data").ToString() == "[]" ? new List<JSON_FolderMetadata>() : JsonConvert.DeserializeObject<List<JSON_FolderMetadata>>(result.Jobj().SelectToken("data").ToString(), JSONhandler);
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

