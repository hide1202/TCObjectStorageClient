using System.Text;

namespace ToastCloud.ObjectStorage.Internals
{
    internal class UrlBuilder
    {
        private readonly string _endPoint;
        private const string Delimiter = "/";

        internal UrlBuilder(string endPoint)
        {
            _endPoint = endPoint;
        }

        internal string Build(params string[] paths)
        {
            var endPoint = _endPoint.TrimEnd('/');
            var builder = new StringBuilder(endPoint);
            foreach (var path in paths)
            {
                builder.Append($"{Delimiter}{path}");
            }
            return builder.ToString();
        }
    }
}
