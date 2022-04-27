using Huddeh.LocalizedResource.Keywords;

using System.ComponentModel;

namespace Saeed.Utilities.Types.Enums
{
    public enum GenderTypes
    {
        [Description(nameof(Keywords.Unknown))]
        Unknown = 0,
        [Description(nameof(Keywords.Male))]
        Male,
        [Description(nameof(Keywords.Female))]
        Female,

    }
}
