using System.Collections.Generic;
using System.Net;

namespace Saeed.Utilities.Contracts
{
    public interface IMessageContractBase
    {
        List<string> Errors { get; set; }
        string Message { get; set; }
        HttpStatusCode StatusCode { get; set; }
        bool Success { get; set; }
    }
}