using DepositfilesSDK.JSON;
using System;
using System.Threading.Tasks;
using static DepositfilesSDK.Utilitiez;

namespace DepositfilesSDK
{
    public interface IFolder
    {
        Task<JSON_SearchFolder> SearchFolder(string SearchKeyword, int OffSet);

        Task<bool> Rename(string NewName);

        Task<bool> DeleteParentWithoutSubs();

        Task<bool> Delete();

        Task<string> Upload(object FileToUpload, UploadTypes UploadType, string FileName, IProgress<ReportStatus> ReportCls = null, System.Threading.CancellationToken token = default);
        Task<JSON_RemoteUpload> RemoteUploadMultiple(string[] Urls);
        Task<JSON_RemoteUpload> RemoteUpload(Uri FileUrl);
        Task<JSON_List> ListFiles();
    }
}
