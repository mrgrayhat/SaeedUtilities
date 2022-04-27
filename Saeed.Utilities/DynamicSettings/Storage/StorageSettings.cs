namespace Saeed.Utilities.DynamicSettings.Storage
{
    public class StorageSettings
    {
        public bool SecureStorageCachingEnabled { get; set; } = true;
        public bool StaticFilesCachingEnalbed { get; set; } = true;

        public bool ApplyWatermark { get; set; } = true;
        public string BaseRootPath { get; set; }
        public string ProfileFilesPath { get; set; }
        public string UserUploadFilesPath { get; set; }
        public string WatermarkText { get; set; }
        public string DefaultProfileImage { get; set; }
        public string DefaultCoverImage { get; set; }
        public string GuardImage { get; set; }
        public string NotFoundImage { get; set; }
        public string WatermarkImage { get; set; }
        public string AssetFilesPath { get; set; }
        public string PublicFilesPath { get; set; }
        public string PreviewFilesPath { get; set; }
        public string ThumbnailFilesPath { get; set; }
        public string PrivateProjectFilesPath { get; set; }
        public string PrivateFilesPath { get; set; }

        public long MinUploadSize { get; set; } = 0;
        public long MaxUploadSize { get; set; } = 40 * 1024 * 1024; // 40mb == ~ 41943040 byte
        public long MaxPublicUploadSize { get; set; } = 2 * 1024 * 1024; // 2mb == ~ 2097152 byte
        public long MinUploadImageQuality { get; set; } = 0;

        /// <summary>
        /// max width as pixel
        /// </summary>
        public int MaxPreviewImageWidth { get; set; } = 484;
        public int MaxPreviewImageHeight { get; set; } = 484;

        public int MaxImageWidth { get; set; } = 1280;
        /// <summary>
        /// max height as pixel
        /// </summary>
        public int MaxImageHeight { get; set; } = 720;

        public int MaxThumbnailImageWidth { get; set; } = 154;
        public int MaxThumbnailImageHeight { get; set; } = 154;


        public string[] AllowedExtensions { get; set; } = new[] { "jpg", "jpeg", "png", "bmp" };
        public string[] AllowedDocumentsExtensions { get; set; } = new[] { "pdf", "doc", "docx", "xls", "xlsx", "rar", "zip", "tar" };

        public bool IsCleanupEnabled { get; set; } = false;
        public bool GenerateStandardSizes { get; set; } = false;
    }
}
