using System.Collections.Generic;
using System.Drawing;

using Saeed.Utilities.Types.Enums;

namespace Saeed.Utilities.DynamicSettings.MethodsSetting
{
    public class ImageValidationSettings
    {
        public FileUploadValidationPolicy Policy { get; set; } = FileUploadValidationPolicy.Format;

        public List<string> ValidFormats { get; set; } = null;

        public long MaxLength { get; set; }
        public long MinLength { get; set; } = 1;

        public PointF Resolution { get; set; }
    }
}
