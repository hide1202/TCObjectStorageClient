using System.Collections.Generic;
using System.Linq;
using ToastCloud.ObjectStorage.Responses;

namespace ToastCloud.ObjectStorage.SDKResponses
{
    public class ContainerInfoResult : IContainerInfoResult
    {
        public List<ContainerInfo> Containers { get; private set; }

        internal static ContainerInfoResult FromResponse(List<ContainerInfoResponse> responses)
        {
            var result = new ContainerInfoResult
            {
                Containers = responses.Select(r => new ContainerInfo(r.Count, r.Bytes, r.Name)).ToList()
            };
            return result;
        }
    }
}
