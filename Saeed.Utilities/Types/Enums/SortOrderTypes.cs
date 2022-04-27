using System.ComponentModel;

namespace Saeed.Utilities.Types.Enums
{
    public enum SortOrderTypes
    {
        [Description("نزولی")]
        //[Display(ResourceType = typeof(Keywords), Name = nameof(Keywords.DescendingSortOrder))]
        Desc,
        //[Display(ResourceType = typeof(Keywords), Name = nameof(Keywords.AscendingSortOrder))]
        [Description("صعودی")]
        Asc,

    }
}