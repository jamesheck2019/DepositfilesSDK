using DepositfilesSDK.JSON;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositfilesSDK
{
    public interface IRoot
    {

        string RootID { get; }
        Task<List<JSON_FolderMetadata>> ListFolders();
        Task<JSON_List> ListFiles();
        Task<JSON_FolderMetadata> CreateNewFolder(string FolderName);
        Task<List<JSON_FileMetadata>> SearchFiles(string SearchKeyword, Utilitiez.SearchTypeEnum SearchType);

    }
}
