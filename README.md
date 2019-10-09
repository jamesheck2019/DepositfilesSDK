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
    Sub SetClient()
        Dim MyClient As DepositfilesSDK.IClient = New DepositfilesSDK.DClient("usern", "passw")
    End Sub
```
```vb
    Sub SetClientWithOptions()
        Dim Optians As New DepositfilesSDK.ConnectionSettings With {.CloseConnection = True, .TimeOut = TimeSpan.FromMinutes(30), .Proxy = New DepositfilesSDK.ProxyConfig With {.ProxyIP = "172.0.0.0", .ProxyPort = 80, .ProxyUsername = "myname", .ProxyPassword = "myPass", .SetProxy = True}}
        Dim MyClient As DepositfilesSDK.IClient = New DepositfilesSDK.DClient("access token", Optians)
    End Sub
```
```vb
    Async Sub ListMyFilesAndFolders()
        Dim result = Await MyClient.List("")
        For Each vid In result.FilesList
            DataGridView1.Rows.Add(vid.Value.filename, vid.Value.DownloadCount, vid.Value.FileUrl, vid.Value.file_id)
        Next
        ''list files
        Dim resultF = Await MyClient.ListFiles()
        ''list folders
        Dim resultD = Await MyClient.ListFolders()
    End Sub
```
```vb
    Async Sub CreateNewFolder()
        Dim result = Await MyClient.CreateNewFolder("folder name")
    End Sub
```
```vb
    Async Sub LockFile()
        Dim result = Await MyClient.LockFile("file id", "pass1234")
    End Sub
```
```vb
    Async Sub MoveFile()
        Dim result = Await MyClient.MoveFile("file id", "to folder id")
    End Sub
```
```vb
    Async Sub DeleteAFileOrFolder()
        Dim result = Await MyClient.DeleteFile("file id")
        Dim resultd = Await MyClient.DeleteFolder("folder id")
    End Sub
```
```vb
    Async Sub RenameFileOrFolder()
        Dim result = Await MyClient.RenameFile("file id", "new name")
        Dim resultD = Await MyClient.RenameFolder("folder id", "new name")
    End Sub
```
```vb
    Async Sub VideoDirectUrl()
        Dim result = Await MyClient.Search("stre", SearchTypeEnum.Contains)
        For Each vid In result
            DataGridView1.Rows.Add(vid.filename, vid.DownloadCount, vid.FileUrl, vid.file_id)
        Next
    End Sub
```
```vb
    Async Sub Upload_Remote()
        Dim result = Await MyClient.RemoteUpload("https://www.tube.com/video.mp4")
    End Sub
```
```vb
    Async Sub Upload_Local_WithProgressTracking()
        Dim UploadCancellationToken As New Threading.CancellationTokenSource()
        Dim _ReportCls As New Progress(Of DepositfilesSDK.ReportStatus)(Sub(ReportClass As DepositfilesSDK.ReportStatus)
                                                                            Label1.Text = String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes))
                                                                            ProgressBar1.Value = CInt(ReportClass.ProgressPercentage)
                                                                            Label2.Text = CStr(ReportClass.TextStatus)
                                                                        End Sub)
        Dim RSLT = Await MyClient.Upload("J:\DB\myvideo.mp4", UploadTypes.FilePath, "myvideo.mp4", "folder id", _ReportCls, UploadCancellationToken.Token)
    End Sub
```
