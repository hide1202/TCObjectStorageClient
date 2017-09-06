namespace ToastCloud.ObjectStorage.Internals
{
    internal static class ObjectStorageUrls
    {
        internal static string ContainerUrl(string endPoint, string container)
        {
            return new UrlBuilder(endPoint).Build(container);
        }

        internal static string ObjectUrl(string endPoint, string container, string objectName)
        {
            return new UrlBuilder(endPoint).Build(container, objectName);
        }
    }
}
