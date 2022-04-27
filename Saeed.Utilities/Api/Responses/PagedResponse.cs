using System;

using MessagePack;

namespace Saeed.Utilities.API.Responses
{
    /// <summary>
    /// Paged Model useful for responses with big, array data or paged collections.
    /// give page number, size and total count and make a paged model with total available pages.
    /// </summary>
    [Serializable]
    [MessagePackObject(true)]
    [System.Runtime.Serialization.DataContract]
    public record PagedResponse : IPagedResponse
    {
        /// <summary>
        /// give page number, size and total count and make a paged model with total available pages.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        public PagedResponse(long pageNumber, int pageSize, long totalRecords)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
            this.TotalPages = (int)Math.Ceiling((double)totalRecords / (pageSize == 0 ? 1 : pageSize));
        }

        /// <summary>
        /// the current page number
        /// </summary>
        [Key(0)]
        public long PageNumber { get; private set; }
        /// <summary>
        /// max number of items per page
        /// </summary>
        [Key(1)]
        public int PageSize { get; private set; }
        /// <summary>
        /// total pages calculated from total records / pagesize.
        /// </summary>
        [Key(2)]
        public int TotalPages { get; private set; }
        /// <summary>
        /// total items count
        /// </summary>
        [Key(3)]
        public long TotalRecords { get; private set; }

        public PagedResponse SetPageNumber(long pageNumber)
        {
            this.PageNumber = pageNumber;
            return this;
        }

        public PagedResponse SetPageSize(int pageSize)
        {
            this.PageSize = pageSize;
            return this;
        }

        public PagedResponse SetTotalPages(int totalPages)
        {
            this.TotalPages = totalPages;
            return this;
        }

        public PagedResponse SetTotalRecords(long totalRecords)
        {
            this.TotalRecords = totalRecords;
            return this;
        }
    }
}