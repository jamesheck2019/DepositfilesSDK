Module Base


    Public Property APIbase As String = "https://depositfiles.com/api/"
    Public Property m_TimeOut As System.TimeSpan = Threading.Timeout.InfiniteTimeSpan
    Public Property m_CloseConnection As Boolean = True
    Public Property JSONhandler As New Newtonsoft.Json.JsonSerializerSettings() With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore, .NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore}
    Public Property AccToken() As String


    Public Class AuthDictionary
        Inherits Dictionary(Of String, String)
        Sub New()
            MyBase.Add("token", AccToken)
        End Sub
    End Class
    Public Class pUri
        Inherits Uri
        Sub New(Action As String, Optional Parameters As Dictionary(Of String, String) = Nothing)
            MyBase.New(APIbase + Action + If(Parameters Is Nothing, Nothing, utilitiez.AsQueryString(Parameters)))
        End Sub
    End Class

    Private _proxy As ProxyConfig
    Public Property m_proxy As ProxyConfig
        Get
            Return If(_proxy, New ProxyConfig)
        End Get
        Set(value As ProxyConfig)
            _proxy = value
        End Set
    End Property

    Public Class HCHandler
        Inherits Net.Http.HttpClientHandler
        Sub New()
            MyBase.New()
            If m_proxy.SetProxy Then
                MaxRequestContentBufferSize = 1 * 1024 * 1024
                Proxy = New Net.WebProxy(String.Format("http://{0}:{1}", m_proxy.ProxyIP, m_proxy.ProxyPort), True, Nothing, New Net.NetworkCredential(m_proxy.ProxyUsername, m_proxy.ProxyPassword))
                UseProxy = m_proxy.SetProxy
            End If
        End Sub
    End Class

    Public Class HttpClient
        Inherits Net.Http.HttpClient
        Sub New(HCHandler As HCHandler)
            MyBase.New(HCHandler)
            MyBase.DefaultRequestHeaders.UserAgent.ParseAdd("DepositfilesSDK")
            DefaultRequestHeaders.ConnectionClose = m_CloseConnection
            Timeout = m_TimeOut
        End Sub
        Sub New(progressHandler As Net.Http.Handlers.ProgressMessageHandler)
            MyBase.New(progressHandler)
            MyBase.DefaultRequestHeaders.UserAgent.ParseAdd("DepositfilesSDK")
            DefaultRequestHeaders.ConnectionClose = m_CloseConnection
            Timeout = m_TimeOut
        End Sub
    End Class

    Function ShowError(result As String)
        Dim errorInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(Of JSON.JSON_Error)(result, JSONhandler)
        Throw ExceptionCls.CreateException(errorInfo._ErrorMessage, errorInfo._ErrorCode)
    End Function

    <Runtime.CompilerServices.Extension()>
    Function CheckStatus(result As String) As Boolean
        Return Newtonsoft.Json.Linq.JObject.Parse(result).Value(Of Integer)("status_code").Equals(1)
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Function Jobj(response As String) As Newtonsoft.Json.Linq.JObject
        Return Newtonsoft.Json.Linq.JObject.Parse(response)
    End Function

End Module
