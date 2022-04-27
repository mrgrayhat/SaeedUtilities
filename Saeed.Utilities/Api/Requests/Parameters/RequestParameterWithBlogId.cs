namespace Saeed.Utilities.API.Requests.Parameters
{
    /// <summary>
    /// the request parameter which include paging filter with search term, for search filters.
    /// </summary>
    public class RequestParameterWithBlogId : RequestParameterBase
    {
        public RequestParameterWithBlogId() : base()
        {

        }
        /// <summary>
        /// the search term / string
        /// </summary>
        public int BlogId { get; set; }
    }
}