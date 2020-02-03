Imports System.ComponentModel

Public Class utilitiez

    Public Shared Function AsQueryString(parameters As Dictionary(Of String, String)) As String
        If Not parameters.Any() Then Return String.Empty
        Dim builder = New Text.StringBuilder("?")
        Dim separator = String.Empty
        For Each kvp In parameters.Where(Function(P) Not String.IsNullOrEmpty(P.Value))
            builder.AppendFormat("{0}{1}={2}", separator, Net.WebUtility.UrlEncode(kvp.Key), Net.WebUtility.UrlEncode(kvp.Value.ToString()))
            separator = "&"
        Next
        Return builder.ToString()
    End Function

    Shared Function ExtractLinks(HTMLString As String, HTTPS As Boolean) As List(Of String)
        Dim linkParser = New Text.RegularExpressions.Regex(String.Format("\b(?:{0}?://|www\.)\S+\b", If(HTTPS, "https", "http")), Text.RegularExpressions.RegexOptions.Compiled Or Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim linkz = linkParser.Matches(HTMLString)
        Return (From mch As Text.RegularExpressions.Match In linkz Select mch.Value).ToList
    End Function

    Enum UploadTypes
        FilePath
        Stream
        BytesArry
    End Enum
    Enum SearchTypeEnum
        Contains
        Exact
        StartWith
        EndWith
        Ext
    End Enum
End Class

Public Class ConnectionSettings
    Public Property TimeOut As System.TimeSpan = Nothing
    Public Property CloseConnection As Boolean? = True
    Public Property Proxy As ProxyConfig = Nothing
End Class