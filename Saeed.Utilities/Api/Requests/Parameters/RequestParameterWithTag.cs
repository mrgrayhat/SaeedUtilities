namespace Saeed.Utilities.API.Requests.Parameters
{
    /// <summary>
    /// the request parameter which include paging filter with search term, for search filters.
    /// </summary>
    public class RequestParameterWithTag : RequestParameterBase
    {
        public RequestParameterWithTag() : base()
        {

        }
        /// <summary>
        /// the search term / string
        /// </summary>
        public string Tag { get; set; }
    }
}