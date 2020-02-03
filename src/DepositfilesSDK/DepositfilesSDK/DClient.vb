Imports DepositfilesSDK.JSON
Imports Newtonsoft.Json

Public Class DClient
    Implements IClient



    Public Sub New(Username As String, Password As String, Optional Settings As ConnectionSettings = Nothing)
        Net.ServicePointManager.Expect100Continue = True : Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12 Or Net.SecurityProtocolType.Ssl3
        If Settings Is Nothing Then
            m_proxy = Nothing
        Else
            m_proxy = Settings.Proxy
            m_CloseConnection = If(Settings.CloseConnection, True)
            m_TimeOut = If(Settings.TimeOut = Nothing, TimeSpan.FromMinutes(60), Settings.TimeOut)
        End If
        AccToken = GetToken.Token(Username, Password).Result.data.token
    End Sub
    Public Sub New(accessToken As String, Optional Settings As ConnectionSettings = Nothing)
        AccToken = accessToken
        If Settings Is Nothing Then
            m_proxy = Nothing
        Else
            m_proxy = Settings.Proxy
            m_CloseConnection = If(Settings.CloseConnection, True)
            m_TimeOut = If(Settings.TimeOut = Nothing, TimeSpan.FromMinutes(60), Settings.TimeOut)
        End If
        Net.ServicePointManager.Expect100Continue = True : Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12 Or Net.SecurityProtocolType.Ssl3
    End Sub


    Public ReadOnly Property Files(FileID As String) As IFiles Implements IClient.Files
        Get
            Return New FilesClient(FileID)
        End Get
    End Property
    Public ReadOnly Property Folders(FolderID As String) As IFolders Implements IClient.Folders
        Get
            Return New FoldersClient(FolderID)
        End Get
    End Property
    Public ReadOnly Property Root() As IRoot Implements IClient.Root
        Get
            Return New RootClient()
        End Get
    End Property

    Public ReadOnly Property RootID() As String Implements IClient.RootID
        Get
            Return "_root"
        End Get
    End Property

#Region "UserInfo"
    Public Async Function GET_UserInfo() As Task(Of JSON_GetToken) Implements IClient.UserInfo
        Using localHttpClient As New HttpClient(New HCHandler)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(New pUri("user/profile", New AuthDictionary)).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetToken)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region




End Class
