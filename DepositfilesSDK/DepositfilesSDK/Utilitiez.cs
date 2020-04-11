using System.Collections.Generic;
using System.Linq;

namespace DepositfilesSDK
{
    public class Utilitiez
    {

        public static string AsQueryString(Dictionary<string, string> parameters)
        {
            if (!parameters.Any())
            {
                return string.Empty;
            }

            var builder = new System.Text.StringBuilder("?");
            var separator = string.Empty;
            foreach (var kvp in parameters.Where(P => !string.IsNullOrEmpty(P.Value)))
            {
                builder.AppendFormat("{0}{1}={2}", separator, System.Net.WebUtility.UrlEncode(kvp.Key), System.Net.WebUtility.UrlEncode(kvp.Value.ToString()));
                separator = "&";
            }
            return builder.ToString();
        }

        public static List<string> ExtractLinks(string HTMLString, bool HTTPS)
        {
            var linkParser = new System.Text.RegularExpressions.Regex(string.Format(@"\b(?:{0}?://|www\.)\S+\b", HTTPS ? "https" : "http"), System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.MatchCollection linkz = linkParser.Matches(HTMLString);
            List<string> tobereturn = new List<string>();
            return (from System.Text.RegularExpressions.Match mch in linkz select mch.Value).ToList();
        }

        public enum UploadTypes
        {
            FilePath,
            Stream,
            BytesArry
        }

        public enum SearchTypeEnum
        {
            Contains,
            Exact,
            StartWith,
            EndWith,
            Ext
        }

    }
}
