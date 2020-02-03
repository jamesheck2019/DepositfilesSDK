Imports DepositfilesSDK.JSON

Public Interface IRoot

    Function ListFolders() As Task(Of List(Of JSON.JSON_FolderMetadata))

    Function ListFiles() As Task(Of JSON.JSON_List)

    Function CreateNewFolder(FolderName As String) As Task(Of JSON.JSON_FolderMetadata)

    Function SearchFiles(SearchKeyword As String, SearchType As utilitiez.SearchTypeEnum) As Task(Of List(Of JSON_FileMetadata))

End Interface
