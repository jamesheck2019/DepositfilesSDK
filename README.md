## DepositfilesSDK


`Download`
[https://github.com/jamesheck2019/DepositfilesSDK/releases](https://github.com/jamesheck2019/DepositfilesSDK/releases)<br>
`NuGet:`
[![NuGet](https://img.shields.io/nuget/v/DeQmaTech.DepositfilesSDK.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/DeQmaTech.DepositfilesSDK)<br>
`Help:`
[https://github.com/jamesheck2019/DepositfilesSDK/wiki](https://github.com/jamesheck2019/DepositfilesSDK/wiki)<br>


**Features**

* Assemblies for .NET 4.5.2 and .NET Standard 2.0 and .NET Core 2.1
* Just one external reference (Newtonsoft.Json)
* Easy installation using NuGet
* Upload/Download tracking support
* Proxy Support
* Upload/Download cancellation support

# List of functions:
**Authentication**
> * GetToken
> * UserInfo

**File**
> * GetDownloadToken
> * GetThumb
> * Unlock
> * Lock
> * Rename
> * Move
> * Delete

**Files**
> * Delete
> * Move

**Folder**
> * SearchFolder
> * Rename
> * DeleteParentWithoutSubs
> * Delete
> * Upload
> * RemoteUploadMultiple
> * RemoteUpload
> * ListFiles

**Root**
> * RootID
> * ListFolders
> * ListFiles
> * CreateNewFolder
> * SearchFiles


# CodeMap:
![codemap](https://www.mediafire.com/convkey/67b704c1/wqb4vcjjn1fqr0wzg.jpg)


# Code simple:
```vb
'first get auth token
Dim tokn = Await DepositfilesSDK.Authentication.GetToken("user", "pass")
Dim cLENT As DepositfilesSDK.IClient = New DepositfilesSDK.DClient(tokn.data.token)

''or
'set proxy And connection options
Dim con = New DepositfilesSDK.ConnectionSettings With {.CloseConnection = True, .TimeOut = TimeSpan.FromMinutes(30), .Proxy = New DepositfilesSDK.ProxyConfig With {.SetProxy = True, .ProxyIP = "127.0.0.1", .ProxyPort = 8888, .ProxyUsername = "user", .ProxyPassword = "pass"}}
'Dim cLENT As DepositfilesSDK.IClient = New DepositfilesSDK.DClient("user", "pass", con)

Await cLENT.UserInfo
Await cLENT.Root.CreateNewFolder("new folder")
Await cLENT.Root.ListFiles
Await cLENT.Root.ListFolders
Await cLENT.Root.SearchFiles("emy", DepositfilesSDK.Utilitiez.SearchTypeEnum.Contains)

Await cLENT.Folder("folder_id").Delete
Await cLENT.Folder("folder_id").DeleteParentWithoutSubs
Await cLENT.Folder("folder_id").ListFiles
Await cLENT.Folder("folder_id").Rename("new name")
Await cLENT.Folder("folder_id").SearchFolder("emy", 0)
Dim cts As New Threading.CancellationTokenSource()
Dim _ReportCls As New Progress(Of DepositfilesSDK.ReportStatus)(Sub(ReportClass As DepositfilesSDK.ReportStatus) Console.WriteLine(String.Format("{0} - {1}% - {2}", $"{ReportClass.BytesTransferred}/{ReportClass.TotalBytes}", ReportClass.ProgressPercentage, ReportClass.TextStatus)))
Await cLENT.Folder("folder_id").Upload("c:\\myvid.mp4", DepositfilesSDK.Utilitiez.UploadTypes.FilePath, "myvid.mp4", _ReportCls, cts.Token)
Await cLENT.Folder("folder_id").RemoteUpload(New Uri("https://domain.com/wat.mp4"))
Await cLENT.Folder("folder_id").RemoteUploadMultiple(New String() {"https://domain.com/wat.mp4", "https://domain.com/wat.mp4"})

Await cLENT.File("file_id").Delete
Await cLENT.File("file_id").GetDownloadToken
Await cLENT.File("file_id").GetThumb
Await cLENT.File("file_id").Lock("12345")
Await cLENT.File("file_id").Unlock
Await cLENT.File("file_id").Move("folder_id")
Await cLENT.File("file_id").Rename("new name")

Await cLENT.Files(New String() {"file_id", "file_id"}).Delete()
Await cLENT.Files(New String() {"file_id", "file_id"}).Move("folder_id")

```
