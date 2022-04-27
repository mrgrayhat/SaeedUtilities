namespace Saeed.Utilities.API.Requests.Parameters
{
    /// <summary>
    /// the request parameter which include paging filter with search term, for search filters.
    /// </summary>
    public class RequestParameterWithCityId : RequestParameterBase
    {
        public RequestParameterWithCityId() : base()
        {

        }
        /// <summary>
        /// the search term / string
        /// </summary>
        public int CityId { get; set; }
    }
}