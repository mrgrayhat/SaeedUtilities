using Saeed.Utilities.Types.Enums;

namespace Saeed.Utilities.API.Requests.Parameters
{
    /// <summary>
    /// the request filter parameter with paging and sorting filters param. Descending sort order by default. and null sort field (can be any target data field like Id,..)
    /// </summary>
    public class RequestParameterWithSort : RequestParameterBase
    {
        public RequestParameterWithSort() : base()
        {

        }
        /// <summary>
        /// sort the data by this order. Desc for Descending (Newer are top, Default), or Asc for Ascending (older are top)
        /// </summary>
        public SortOrderTypes SortOrder { get; set; } = SortOrderTypes.Desc;
        /// <summary>
        /// sort by this field (such as Id, Username, CreatedOn,.. for ex)
        /// </summary>
        public string SortOrderBy { get; set; } = null;


        public override string Deconstruct(bool appendTypeName = false)
        {
            return string.Concat(base.Deconstruct(), appendTypeName ? "requestparameterswithsort:" : "",
                $"sortOrder:{SortOrder}", string.IsNullOrEmpty(SortOrderBy) ? "" : $":sortOrderBy:{SortOrderBy}", ":");
        }

    }
}