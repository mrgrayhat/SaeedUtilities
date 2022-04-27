namespace Saeed.Utilities.API.Requests.Parameters
{
    /// <summary>
    /// the request parameter which include paging filter with search term, for search filters.
    /// </summary>
    public class RequestParameterWithSearch : RequestParameterWithSort
    {
        public RequestParameterWithSearch() : base()
        {

        }
        /// <summary>
        /// the search term / string
        /// </summary>
        public string SearchTerm { get; set; }


        public override string Deconstruct(bool appendTypeName = false)
        {
            return string.Concat(base.Deconstruct(), appendTypeName ? "requestParameterWithSearch:" : "", $"searchTerm:{SearchTerm}:");
        }
    }
}