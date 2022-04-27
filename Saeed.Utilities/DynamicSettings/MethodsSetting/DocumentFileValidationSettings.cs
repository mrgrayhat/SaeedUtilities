using System.Collections.Generic;

using Saeed.Utilities.Types.Enums;

namespace Saeed.Utilities.DynamicSettings.MethodsSetting
{
    public class DocumentFileValidationSettings
    {
        public FileUploadValidationPolicy Policy { get; set; } = FileUploadValidationPolicy.Format;

        public List<string> ValidFormats { get; set; } = null;

        public long MaxLength { get; set; }
        public long MinLength { get; set; } = 1;
    }
}
