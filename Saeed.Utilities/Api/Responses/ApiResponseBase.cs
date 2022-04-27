
using System;
using System.Runtime.Serialization;

using Saeed.Utilities.Contracts;

using MessagePack;

namespace Saeed.Utilities.API.Responses
{
    [DataContract]
    [MessagePackObject(true)]
    [Serializable]
    public record ApiResponseBase : MessageContractBase, IApiResponseBase
    {

        public ApiResponseBase(bool success = false) : base()
        {
            Success = success;
        }
        public ApiResponseBase(MessageContractBase messageContract) : base(messageContract)
        {
            Success = messageContract.Success;
            Errors = messageContract.Errors;
            StatusCode = messageContract.StatusCode;
        }
        [Key(6)]
        public string TraceIdentifier { get; set; }
        [Key(7)]
        public string ApiVersion { get; set; } = "1.0";
    }

}
