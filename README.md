# DepositfilesSDK
DepositfilesSDK is a .Net library for Depositfiles.com file hosting service.
.net framework 4.5.2
.net standard 2.0

`Download`
[https://github.com/jamesheck2019/DepositfilesSDK/releases](https://github.com/jamesheck2019/DepositfilesSDK/releases)<br>

[![NuGet version (BlackBeltCoder.Silk)](https://img.shields.io/nuget/v/DeQmaTech.DepositfilesSDK.svg?style=plastic)](https://www.nuget.org/packages/DeQmaTech.DepositfilesSDK/)

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
**set client**
```vb
Dim cLENT as DepositfilesSDK.IClient = New DepositfilesSDK.DClient("USER", "PASS", Nothing)
```
**list root files/folders**
```vb
Dim RSLT = Await cLENT.ListFiles()
For Each fold In RSLT.FilesList
    DataGridView1.Rows.Add(fold.Value.filename, fold.Value.file_id, (fold.Value.size))
Next

Dim RSLT = Await cLENT.ListFolders()
For Each fold In RSLT.data
    DataGridView1.Rows.Add(fold.name, fold.folder_id, fold.FilesCount)
Next
```
**upload local file (without progress tracking)**
```vb.net
Dim UploadCancellationToken As New Threading.CancellationTokenSource()
Dim RSLT = Await cLENT.Upload("C:\ureWiz.png", UploadTypes.FilePath, "ureWiz.png", Nothing, nothing, UploadCancellationToken.Token)
```
**upload local file with progress tracking**
```vb.net
Dim UploadCancellationToken As New Threading.CancellationTokenSource()
Dim prog_ReportCls As New Progress(Of MediafireSDK.ReportStatus)(Sub(ReportClass As MediafireSDK.ReportStatus)
                   Label1.Text = String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes))
                   ProgressBar1.Value = CInt(ReportClass.ProgressPercentage)
                   Label2.Text = CStr(ReportClass.TextStatus)
                   End Sub)
Dim RSLT = Clnt.UploadLocalFile("C:\ureWiz.png", UploadTypes.FilePath, "ureWiz.png",Nothing, prog_ReportCls , UploadCancellationToken.Token)
```
