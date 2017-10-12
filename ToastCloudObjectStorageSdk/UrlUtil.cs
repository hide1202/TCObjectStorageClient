using System.Text;

namespace ToastCloud.ObjectStorage
{
    internal static class UrlUtil
    {
        internal static string UrlWithQueryString(string url, params (string key, string value)[] querys)
        {
            if (querys.Length == 0)
                return url.TrimEnd('/');

            var result = new StringBuilder($"{url.TrimEnd('/')}?");
            for (int i = 0; i < querys.Length; i++)
            {
                if (i > 0) result.Append("&");
                result.AppendFormat("{0}={1}", querys[i].key, querys[i].value);
            }
            return result.ToString();
        }
    }
}
