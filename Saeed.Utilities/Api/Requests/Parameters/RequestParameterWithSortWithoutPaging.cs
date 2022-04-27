using System;

using Saeed.Utilities.Types.Enums;

using Microsoft.OpenApi.Extensions;

namespace Saeed.Utilities.API.Requests.Parameters
{
    public class RequestParameterWithSortWithoutPaging
    {
        public RequestParameterWithSortWithoutPaging() : base()
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


        public virtual string Deconstruct(bool appendTypeName = false)
        {
            return $"sortOrder:{SortOrder}:sortOrderBy:{SortOrderBy}:";
        }
    }
}