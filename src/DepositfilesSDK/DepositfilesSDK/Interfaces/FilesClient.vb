Imports DepositfilesSDK.JSON
Imports Newtonsoft.Json

Public Class FilesClient
    Implements IFiles


    Private Property FileID As String

    Sub New(FileID As String)
        Me.FileID = FileID
    End Sub



#Region "DeleteFile"
    Public Async Function GET_DeleteFile() As Task(Of Boolean) Implements IFiles.Delete
        Dim parameters = New AuthDictionary() From {{"file_id", FileID}}
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("file/delete", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return result.Jobj("data").Value(Of Boolean)("result")
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "DeleteMultipleFiles"
    Public Async Function GET_DeleteMultiFiles(FileIDs() As String) As Task(Of Boolean) Implements IFiles.DeleteMultiple
        Dim parameters = New List(Of KeyValuePair(Of String, String))()
        FileIDs.ToList().ForEach(Sub(x) parameters.Add(New KeyValuePair(Of String, String)("file_id[]", x)))
        parameters.Add(New KeyValuePair(Of String, String)("token", AccToken))
        Dim encodedContent = New Net.Http.FormUrlEncodedContent(parameters)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Post}
            HtpReqMessage.RequestUri = New pUri("file/delete")
            HtpReqMessage.Content = encodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return result.Jobj.SelectToken("data[0]").Value(Of Boolean)("result")
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "MoveFile"
    Public Async Function GET_MoveFile(DestinationFolderID As String) As Task(Of Boolean) Implements IFiles.Move
        Dim parameters = New Dictionary(Of String, String)
        parameters.Add("destination_folder_id", If(String.IsNullOrEmpty(DestinationFolderID), "_root", DestinationFolderID))
        parameters.Add("file_id", FileID)
        parameters.Add("token", AccToken)
        Dim encodedContent = New Net.Http.FormUrlEncodedContent(parameters)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Post}
            HtpReqMessage.RequestUri = New pUri("file/move")
            HtpReqMessage.Content = encodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return result.Jobj("data").Value(Of Boolean)("result")
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "MoveMultipleFiles"
    Public Async Function GET_MoveMultipleFiles(FileIDs() As String, DestinationFolderID As String) As Task(Of Boolean) Implements IFiles.MoveMultiple
        Dim parameters = New List(Of KeyValuePair(Of String, String))()
        FileIDs.ToList().ForEach(Sub(x) parameters.Add(New KeyValuePair(Of String, String)("file_id[]", x)))
        parameters.Add(New KeyValuePair(Of String, String)("destination_folder_id", DestinationFolderID))
        parameters.Add(New KeyValuePair(Of String, String)("token", AccToken))
        Dim encodedContent = New Net.Http.FormUrlEncodedContent(parameters)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Post}
            HtpReqMessage.RequestUri = New pUri("file/move")
            HtpReqMessage.Content = encodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return result.Jobj("data").FirstOrDefault.Value(Of Boolean)("result")
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "RenameFile"
    Public Async Function GET_RenameFile(NewName As String) As Task(Of String) Implements IFiles.Rename
        Dim parameters = New AuthDictionary()
        parameters.Add("file_id", FileID)
        parameters.Add("name", NewName)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("file/rename", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return result.Jobj.SelectToken("data.name").ToString
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "LockFile"
    Public Async Function GET_LockFile(Password As String) As Task(Of Boolean) Implements IFiles.Lock
        Dim parameters = New AuthDictionary()
        parameters.Add("file_id", FileID)
        parameters.Add("password", Password)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("file/edit", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return If(result.Jobj.SelectToken("data.file_id").ToString = FileID, True, False)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "UnlockFile"
    Public Async Function GET_UnlockFile() As Task(Of Boolean) Implements IFiles.Unlock
        Dim parameters = New AuthDictionary()
        parameters.Add("file_id", FileID)
        parameters.Add("password", String.Empty)
        Dim encodedContent = New Net.Http.FormUrlEncodedContent(parameters)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Post}
            HtpReqMessage.RequestUri = New pUri("file/edit")
            HtpReqMessage.Content = encodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return If(result.Jobj.SelectToken("data.file_id").ToString = FileID, True, False)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "GetThumb"
    Public Async Function GET_GetThumb() As Task(Of String) Implements IFiles.GetThumb
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New Uri(String.Format("https://depositfiles.com/thumb/{0}", FileID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Dim fin = utilitiez.ExtractLinks(result, True)
                    If fin.Count.Equals(0) Then Return Nothing Else Return fin(0)
                Else
                    Throw ExceptionCls.CreateException(response.ReasonPhrase, response.StatusCode)
                End If
            End Using
        End Using
    End Function
#End Region



#Region "GetDownloadToken"
    Public Async Function GET_DownloadToken() As Task(Of JSON_GetDownloadToken) Implements IFiles.GetDownloadToken
        Dim parameters = New Dictionary(Of String, String)
        parameters.Add("file_id", FileID)
        parameters.Add("token", AccToken)
        Dim encodedContent = New Net.Http.FormUrlEncodedContent(parameters)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Post}
            HtpReqMessage.RequestUri = New pUri("download/file")
            HtpReqMessage.Content = encodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetDownloadToken)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region


End Class
