namespace ToastCloud.ObjectStorage.SDKResponses
{
    public class ContainerInfo
    {
        public int Count { get; }

        public long Bytes { get; }

        public string Name { get; }

        internal ContainerInfo(int count, long bytes, string name)
        {
            Count = count;
            Bytes = bytes;
            Name = name;
        }

        public override string ToString()
        {
            return $"[{nameof(Count)}: {Count}, {nameof(Bytes)}: {Bytes}, {nameof(Name)}: {Name}]";
        }
    }
}
