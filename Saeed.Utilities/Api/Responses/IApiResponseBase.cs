namespace Saeed.Utilities.API.Responses
{
    public interface IApiResponseBase
    {
        string ApiVersion { get; set; }
        string TraceIdentifier { get; set; }
    }
}