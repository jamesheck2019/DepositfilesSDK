using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DepositfilesSDK.JSON;

namespace DepositfilesSDK
{
	public interface IClient
	{
		Task<JSON_GetToken> GetToken(string Username, string Password);

		Task<JSON_GetToken> UserInfo();

		Task<bool> LockFile(string DestinationFileID, string Password);

		Task<bool> UnlockMultipleFiles(List<string> DestinationFilesIDList);

		Task<bool> UnlockFile(string DestinationFileID);

		bool LockMultipleFiles(List<string> DestinationFilesIDList, string Password);

		Task<JSON_RemoteUpload> RemoteUploadMultiple(List<string> Urls);

		Task<JSON_SearchFolder> SearchFolder(string DestinationFolderID, string SearchKeyword, int OffSet);

		Task<JSON_DeleteMultipleFiles> MoveMultipleFiles(List<string> SourceFilesIDList, string DestinationFolderID);

		bool DeleteMultipleFolders(List<string> DestinationFoldersIDList);

		Task<JSON_DeleteMultipleFiles> DeleteMultipleFiles(List<string> DestinationFilesIDList);

		Task<List<JSON_FileMetadata>> Search(string SearchKeyword, DClient.SearchTypeEnum SearchType);

		Task<JSON_GetDownloadToken> GetDownloadToken(string DestinationFileID);

		Task<JSON_DeleteFolder> DeleteFile(string DestinationFileID);

		Task<JSON_RenameFile> RenameFile(string DestinationFileID, string NewName);

		Task<JSON_DeleteFile> MoveFile(string SourceFileID, string DestinationFolderID);

		Task<JSON_DeleteFolder> DeleteFolderWithoutContains(string DestinationFolderID);

		Task<JSON_DeleteFolder> RenameFolder(string DestinationFolderID, string NewName);

		Task<JSON_DeleteFolder> DeleteFolder(string DestinationFolderID);

		Task<JSON_CreateNewFolder> CreateNewFolder(string FolderName);

		Task<JSON_List> List(string DestinationFolderID);

		Task<JSON_ListFiles> ListFiles();

		Task<JSON_ListFolders> ListFolders();

		Task<string> Upload(object FileToUpload, DClient.UploadTypes UploadType, string FileName, string DestinationFolderID, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default(CancellationToken));

		Task<JSON_RemoteUpload> RemoteUpload(string FileUrl);
	}
}
