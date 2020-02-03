Imports DepositfilesSDK.utilitiez
Imports DepositfilesSDK.JSON

Public Interface IFolders

    Function SearchFolder(SearchKeyword As String, OffSet As Integer) As Task(Of JSON_SearchFolder)



    Function Rename(NewName As String) As Task(Of Boolean)

    Function DeleteParentWithoutSubs() As Task(Of Boolean)

    Function Delete() As Task(Of Boolean)


    Function Upload(FileToUpload As Object, UploadType As UploadTypes, FileName As String, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of String)
    Function RemoteUploadMultiple(Urls() As String) As Task(Of JSON_RemoteUpload)
    Function RemoteUpload(FileUrl As Uri) As Task(Of JSON_RemoteUpload)
    Function ListFiles() As Task(Of JSON_List)

End Interface
