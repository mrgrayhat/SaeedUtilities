using Microsoft.Data.SqlClient;

namespace Saeed.Utilities.API.Requests.Parameters
{
    /// <summary>
    /// the base of all requests input parameter. contain paging params.
    /// </summary>
    public abstract class RequestParameterBase
    {
        /// <summary>
        /// Maximum items that can be requested per page
        /// </summary>
        private const ushort MaxPageSize = 20;
        /// <summary>
        /// the page number to skip queried data / page
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// maximum items per page / response
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// initialize default paging filters by page 1 and page size 10
        /// </summary>
        public RequestParameterBase()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        /// <summary>
        /// initialize an request parameter with provided page and page size
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        protected RequestParameterBase(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > MaxPageSize ? 10 : pageSize;
        }


        public virtual string Deconstruct(bool appendTypeName = false)
        {
            return string.Concat(appendTypeName ? "requestparameters:" : "", $"pageNumber:{PageNumber}:pageSize:{PageSize}:");
        }
    }
}
