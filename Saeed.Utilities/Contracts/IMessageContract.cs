using Saeed.Utilities.API.Responses;

namespace Saeed.Utilities.Contracts
{
    public interface IMessageContract<T> : IMessageContractBase
    {
        T Data { get; set; }
        PagedResponse Paging { get; set; }
    }

    public interface IMessageContract : IMessageContractBase
    {

    }
}