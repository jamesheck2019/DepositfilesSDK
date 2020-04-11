using System.Threading.Tasks;

namespace DepositfilesSDK
{
    public interface IFiles
    {
        Task<bool> Delete();
        Task<bool> Move( string DestinationFolderID);
    }
}