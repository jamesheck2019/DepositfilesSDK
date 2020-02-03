Imports Newtonsoft.Json
Imports DepositfilesSDK.JSON

Public Class RootClient
    Implements IRoot




#Region "Search"
    Public Async Function _Search(SearchKeyword As String, SearchType As utilitiez.SearchTypeEnum) As Task(Of List(Of JSON_FileMetadata)) Implements IRoot.SearchFiles
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("file/listing", New AuthDictionary)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Using content As Net.Http.HttpContent = response.Content
                    Dim result As String = Await content.ReadAsStringAsync()

                    If Linq.JObject.Parse(result)("status").ToString = "OK" Then
                        If Linq.JObject.Parse(result)("data").ToString = "[]" Then
                            Return New List(Of JSON_FileMetadata)
                        Else
                            Dim nfo = JsonConvert.DeserializeObject(Of JSON_ListFiles)(result, JSONhandler)
                            Dim lst As New List(Of JSON_FileMetadata)
                            Select Case SearchType
                                Case utilitiez.SearchTypeEnum.Contains
                                    For Each onz In nfo.FilesList
                                        If onz.Value.filename.ToLower.Contains(SearchKeyword.ToLower) Then lst.Add(onz.Value)
                                    Next
                                Case utilitiez.SearchTypeEnum.Exact
                                    For Each onz In nfo.FilesList
                                        If onz.Value.filename.ToLower = (SearchKeyword.ToLower) Then lst.Add(onz.Value)
                                    Next
                                Case utilitiez.SearchTypeEnum.StartWith
                                    For Each onz In nfo.FilesList
                                        If onz.Value.filename.ToLower.StartsWith(SearchKeyword.ToLower) Then lst.Add(onz.Value)
                                    Next
                                Case utilitiez.SearchTypeEnum.EndWith
                                    For Each onz In nfo.FilesList
                                        If onz.Value.filename.ToLower.EndsWith(SearchKeyword.ToLower) Then lst.Add(onz.Value)
                                    Next
                                Case utilitiez.SearchTypeEnum.Ext
                                    For Each onz In nfo.FilesList
                                        If IO.Path.GetExtension(onz.Value.filename).Trim(".") = (SearchKeyword).Trim(".") Then lst.Add(onz.Value)
                                    Next
                            End Select

                            Return lst

                        End If
                    Else
                        ShowError(result)
                    End If
                End Using
            End Using
        End Using
    End Function
#End Region


#Region "CreateNewFolder"
    Public Async Function _CreateNewFolder(FolderName As String) As Task(Of JSON_FolderMetadata) Implements IRoot.CreateNewFolder
        Using localHttpClient As New HttpClient(New HCHandler)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(New pUri("folder/add", New AuthDictionary() From {{"name", FolderName}})).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If result.CheckStatus Then
                    Return JsonConvert.DeserializeObject(Of JSON_FolderMetadata)(result.Jobj.SelectToken("data").ToString, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "List"
    Public Async Function GET_List() As Task(Of JSON_List) Implements IRoot.ListFiles
        Dim parameters = New AuthDictionary() From {{"folder_id", "_root"}}

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("file/listing", parameters)
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

#Region "ListFolders"
    Public Async Function GET_ListFolders() As Task(Of List(Of JSON_FolderMetadata)) Implements IRoot.ListFolders
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("folder/listing", New AuthDictionary)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If result.CheckStatus Then
                    If result.Jobj("data").ToString = "[]" Then
                        Return New List(Of JSON_FolderMetadata)
                    Else
                        Return JsonConvert.DeserializeObject(Of List(Of JSON_FolderMetadata))(result.Jobj.SelectToken("data").ToString, JSONhandler)
                    End If
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

End Class
