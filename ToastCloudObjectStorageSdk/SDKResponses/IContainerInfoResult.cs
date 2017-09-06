using System.Collections.Generic;

namespace ToastCloud.ObjectStorage.SDKResponses
{
    public interface IContainerInfoResult
    {
        List<ContainerInfo> Containers { get; }
    }
}
