using DepositfilesSDK.JSON;
using System.Threading.Tasks;

namespace DepositfilesSDK
{
    public interface IFile
    {
        Task<JSON_GetDownloadToken> GetDownloadToken();
        Task<string> GetThumb();
        Task<bool> Unlock();
        Task<bool> Lock(string Password);
        Task<string> Rename(string NewName);
        Task<bool> Move(string DestinationFolderID);
        Task<bool> Delete();
    }
}
