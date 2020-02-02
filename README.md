## DepositfilesSDK


`Download`
[https://github.com/jamesheck2019/DepositfilesSDK/releases](https://github.com/jamesheck2019/DepositfilesSDK/releases)<br>
`NuGet:`
[![NuGet](https://img.shields.io/nuget/v/DeQmaTech.DepositfilesSDK.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/DeQmaTech.DepositfilesSDK)<br>

**Features**

* Assemblies for .NET 4.5.2 and .NET Standard 2.0 and .NET Core 2.1
* Just one external reference (Newtonsoft.Json)
* Easy installation using NuGet
* Upload/Download tracking support
* Proxy Support
* Upload/Download cancellation support

# List of functions:
* GetToken
* UserInfo
* LockFile
* UnlockMultipleFiles
* UnlockFile
* LockMultipleFiles
* RemoteUploadMultiple
* SearchFolder
* MoveMultipleFiles
* DeleteMultipleFolders
* DeleteMultipleFiles
* Search
* GetDownloadToken
* DeleteFile
* RenameFile
* MoveFile
* DeleteFolderWithoutContains
* RenameFolder
* DeleteFolder
* CreateNewFolder
* List
* ListFiles
* ListFolders
* Upload
* RemoteUpload



# Code simple:
```vb
Dim cLENT As DepositfilesSDK.IClient = New DepositfilesSDK.DClient("username", "password", Nothing)

Await cLENT.UserInfo
Await cLENT.Root.CreateNewFolder("new folder")
Await cLENT.Root.ListFiles
Await cLENT.Root.ListFolders
Await cLENT.Root.SearchFiles("emy", SearchTypeEnum.Contains)

Await cLENT.Folders("folder_id").Delete
Await cLENT.Folders("folder_id").DeleteParentWithoutSubs
Await cLENT.Folders("folder_id").ListFiles
Await cLENT.Folders("folder_id").Rename("new name")
Await cLENT.Folders("folder_id").SearchFolder("emy", 0)
Dim cts As New Threading.CancellationTokenSource()
Dim _ReportCls As New Progress(Of DepositfilesSDK.ReportStatus)(Sub(ReportClass As DepositfilesSDK.ReportStatus) Console.WriteLine(String.Format("{0} - {1}% - {2}", String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes)), CInt(ReportClass.ProgressPercentage), ReportClass.TextStatus)))
Await cLENT.Folders("folder_id").Upload("c:\\myvid.mp4", UploadTypes.FilePath, "myvid.mp4", _ReportCls, cts.Token)
Await cLENT.Folders("folder_id").RemoteUpload(New Uri("https://domain.com/wat.mp4"))
Await cLENT.Folders("folder_id").RemoteUploadMultiple(New String() {"https://domain.com/wat.mp4", "https://domain.com/wat.mp4"})

Await cLENT.Files("file_id").Delete
Await cLENT.Files(Nothing).DeleteMultiple(New String() {"file_id", "file_id"})
Await cLENT.Files("file_id").GetDownloadToken
Await cLENT.Files("file_id").GetThumb
Await cLENT.Files("file_id").Lock("12345")
Await cLENT.Files("file_id").Unlock
Await cLENT.Files("file_id").Move("folder_id")
Await cLENT.Files(Nothing).MoveMultiple(New String() {"file_id", "file_id"}, "folder_id")
Await cLENT.Files("file_id").Rename("new name")
```
