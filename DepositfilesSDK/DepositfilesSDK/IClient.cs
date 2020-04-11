using System.Threading.Tasks;
using DepositfilesSDK.JSON;

namespace DepositfilesSDK
{
    public interface IClient
    {
        IFile File(string FileID);
        IFiles Files(string[] FilesID);
        IFolder Folder(string FolderID);
        IRoot Root();
        Task<JSON_GetToken> UserInfo();
    }
}