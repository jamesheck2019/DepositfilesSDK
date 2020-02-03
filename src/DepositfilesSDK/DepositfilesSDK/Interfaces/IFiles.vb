Public Interface IFiles

    Function GetDownloadToken() As Task(Of JSON.JSON_GetDownloadToken)
    Function MoveMultiple(FileIDs() As String, DestinationFolderID As String) As Task(Of Boolean)
    Function GetThumb() As Task(Of String)
    Function Unlock() As Task(Of Boolean)
    Function Lock(Password As String) As Task(Of Boolean)
    Function Rename(NewName As String) As Task(Of String)
    Function Move(DestinationFolderID As String) As Task(Of Boolean)
    Function DeleteMultiple(FileIDs() As String) As Task(Of Boolean)
    Function Delete() As Task(Of Boolean)


End Interface
