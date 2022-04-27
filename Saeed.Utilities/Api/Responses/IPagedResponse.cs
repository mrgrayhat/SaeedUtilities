namespace Saeed.Utilities.API.Responses
{
    public interface IPagedResponse
    {
        long PageNumber { get; }
        int PageSize { get; }
        int TotalPages { get; }
        long TotalRecords { get; }
    }
}