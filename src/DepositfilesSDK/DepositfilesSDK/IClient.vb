Imports DepositfilesSDK.JSON

Public Interface IClient

    ReadOnly Property Files(FileID As String) As IFiles
    ReadOnly Property Folders(FolderID As String) As IFolders
    ReadOnly Property RootID As String
    ReadOnly Property Root As IRoot


    Function UserInfo() As Task(Of JSON_GetToken)

End Interface
