namespace Saeed.Utilities.Types.Enums
{
    public enum FileUploadValidationPolicy
    {
        /// <summary>
        /// format and size and resolution
        /// </summary>
        All = 0,
        Format,
        /// <summary>
        /// total size of image
        /// </summary>
        Size,
        /// <summary>
        /// mime type format and size length
        /// </summary>
        FormatAndSize,
        /// <summary>
        /// resolution of image
        /// </summary>
        Resolution,
    }
}
