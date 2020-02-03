Imports Newtonsoft.Json
Imports DepositfilesSDK.JSON

Public Class FoldersClient
    Implements IFolders


    Private Property FolderID As String

    Sub New(FolderID As String)
        Me.FolderID = FolderID
    End Sub





#Region "DeleteFolder"
    Public Async Function GET_DeleteFolder() As Task(Of Boolean) Implements IFolders.Delete
        Dim parameters = New AuthDictionary() From {{"folder_id", FolderID}}
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("folder/delete", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return result.Jobj.SelectToken("data").Value(Of Boolean)("result")
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "DeleteParentFolderWithoutSubs"
    Public Async Function GET_DeleteFolderWithoutContains() As Task(Of Boolean) Implements IFolders.DeleteParentWithoutSubs
        Dim parameters = New AuthDictionary()
        parameters.Add("folder_id", FolderID)
        parameters.Add("files_action", 2)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("folder/delete", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If result.CheckStatus Then
                    Return result.Jobj.SelectToken("data").Value(Of Boolean)("result")
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "RenameFolder"
    Public Async Function GET_RenameFolder(NewName As String) As Task(Of Boolean) Implements IFolders.Rename
        Dim parameters = New AuthDictionary()
        parameters.Add("folder_id", FolderID)
        parameters.Add("name", NewName)

        Using localHttpClient As New HttpClient(New HCHandler)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(New pUri("folder/rename", parameters)).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return result.Jobj.SelectToken("data").Value(Of Boolean)("result")
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "SearchFolder"
    Public Async Function GET_SearchFolder(SearchKeyword As String, OffSet As Integer) As Task(Of JSON_SearchFolder) Implements IFolders.SearchFolder
        Dim parameters = New AuthDictionary()
        parameters.Add("folder_id", If(String.IsNullOrEmpty(FolderID), "_root", FolderID))
        parameters.Add("pattern", SearchKeyword)
        parameters.Add("page", OffSet)

        Using localHttpClient As New HttpClient(New HCHandler)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(New pUri("file/search", parameters)).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If result.CheckStatus AndAlso result.Jobj("status").ToString = "OK" Then
                    If result.Jobj("data").ToString = "[]" Then
                        Return New JSON_SearchFolder With {.FilesList = New Dictionary(Of String, JSON_FileMetadata)}
                    Else
                        Return JsonConvert.DeserializeObject(Of JSON_SearchFolder)(result, JSONhandler)
                    End If
                Else
                    ShowError(result)
                End If
            End Using
            End Using
    End Function
#End Region

#Region "UploadFile"

#Region "GetUploadUrl"
    Private Async Function GET_GetUploadUrl() As Task(Of JSON_GetUploadUrl)
        Using localHttpClient As New HttpClient(New HCHandler)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(New pUri("upload/regular", New AuthDictionary)).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Dim userInfo = JsonConvert.DeserializeObject(Of JSON_GetUploadUrl)(result, JSONhandler)
                    userInfo.Success = True
                    Return userInfo
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region


    Public Async Function Get_UploadLocal(FileToUpload As Object, UploadType As utilitiez.UploadTypes, FileName As String, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of String) Implements IFolders.Upload
        Dim uploadUrl = Await GET_GetUploadUrl()

        If ReportCls Is Nothing Then ReportCls = New Progress(Of ReportStatus)
        ReportCls.Report(New ReportStatus With {.Finished = False, .TextStatus = "Initializing..."})
        Try
            Dim progressHandler As New Net.Http.Handlers.ProgressMessageHandler(New HCHandler)
            AddHandler progressHandler.HttpSendProgress, (Function(sender, e)
                                                              ReportCls.Report(New ReportStatus With {.ProgressPercentage = e.ProgressPercentage, .BytesTransferred = e.BytesTransferred, .TotalBytes = If(e.TotalBytes Is Nothing, 0, e.TotalBytes), .TextStatus = "Uploading..."})
                                                          End Function)
            Dim localHttpClient As New HttpClient(progressHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            Dim MultipartsformData = New Net.Http.MultipartFormDataContent("--")
            ''remove the quotes from the boundary
            Dim boundary = MultipartsformData.Headers.ContentType.Parameters.First(Function(o) o.Name = "boundary")
            boundary.Value = boundary.Value.Replace("""", String.Empty)
            ''
            Dim streamContent As Net.Http.HttpContent
            Select Case UploadType
                Case utilitiez.UploadTypes.FilePath
                    streamContent = New Net.Http.StreamContent(New IO.FileStream(FileToUpload, IO.FileMode.Open, IO.FileAccess.Read))
                Case utilitiez.UploadTypes.Stream
                    streamContent = New Net.Http.StreamContent(CType(FileToUpload, IO.Stream))
                Case utilitiez.UploadTypes.BytesArry
                    streamContent = New Net.Http.StreamContent(New IO.MemoryStream(CType(FileToUpload, Byte())))
            End Select
            streamContent.Headers.ContentDisposition = New System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") With {.Name = """files""", .FileName = """" + FileName + """"}
            streamContent.Headers.ContentType = New Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream")
            MultipartsformData.Add(streamContent)
            ''''''''''''''''''''''''''
            MultipartsformData.Add(New Net.Http.StringContent(If(String.IsNullOrEmpty(FolderID), "_root", FolderID)), """fm""")
            MultipartsformData.Add(New Net.Http.StringContent(AccToken), """autologin""")

            HtpReqMessage.Content = MultipartsformData
            HtpReqMessage.RequestUri = New Uri(uploadUrl.data.upload_url)
            '''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
            Using ResPonse As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(False)
                Dim result As String = Await ResPonse.Content.ReadAsStringAsync()

                token.ThrowIfCancellationRequested()
                If ResPonse.StatusCode = Net.HttpStatusCode.OK Then
                    If result.Contains("ud_download_url") Then
                        Dim parsUrls = (utilitiez.ExtractLinks(result, False).Where(Function(x As String) x.Contains("http://depositfiles.com/files/")).Select(Function(x) x).FirstOrDefault) '.Replace("http://picimage.net/][img]", "").Replace("[/img][/url", "")
                        ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = String.Format("[{0}] Uploaded successfully", FileName)})
                        Return parsUrls
                    Else
                        ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = String.Format("The request returned with HTTP status code {0}", result)})
                    End If
                End If
            End Using
        Catch ex As Exception
            ReportCls.Report(New ReportStatus With {.Finished = True})
            If ex.Message.ToString.ToLower.Contains("a task was canceled") Then
                ReportCls.Report(New ReportStatus With {.TextStatus = ex.Message})
            Else
                Throw ExceptionCls.CreateException(ex.Message, ex.Message)
            End If
        End Try
    End Function
#End Region

#Region "RemoteUpload"
    Public Async Function GET_RemoteUpload(FileUrl As Uri) As Task(Of JSON_RemoteUpload) Implements IFolders.RemoteUpload
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("upload/remote/add", New AuthDictionary() From {{"url", FileUrl.ToString}})
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return JsonConvert.DeserializeObject(Of JSON_RemoteUpload)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "RemoteUploadMultiple"
    Public Async Function GET_RemoteUploadMultiple(Urls() As String) As Task(Of JSON_RemoteUpload) Implements IFolders.RemoteUploadMultiple
        Dim encodedContent = New Net.Http.FormUrlEncodedContent(New AuthDictionary() From {{"url", String.Join(Environment.NewLine, Urls)}})
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Post}
            HtpReqMessage.RequestUri = New pUri("upload/remote/add")
            HtpReqMessage.Content = encodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If CheckStatus(result) Then
                    Return JsonConvert.DeserializeObject(Of JSON_RemoteUpload)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "List"
    Public Async Function GET_List() As Task(Of JSON_List) Implements IFolders.ListFiles
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("file/listing", New AuthDictionary() From {{"folder_id", FolderID}})
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If result.CheckStatus Then
                    If result.Jobj("data").ToString = "[]" Then
                        Return New JSON_List With {.FilesList = New Dictionary(Of String, JSON_FileMetadata)}
                    Else
                        Return JsonConvert.DeserializeObject(Of JSON_List)(result, JSONhandler)
                    End If
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region


End Class
