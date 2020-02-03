Imports DepositfilesSDK.JSON
Imports Newtonsoft.Json

Public Class GetToken

#Region "GetToken"
    Shared Async Function Token(Username As String, Password As String) As Task(Of JSON_GetToken)
        Net.ServicePointManager.Expect100Continue = True : Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12 Or Net.SecurityProtocolType.Ssl3

        Dim parameters = New Dictionary(Of String, String) From {{"login", Username}, {"password", Password}}
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("user/login", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
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
