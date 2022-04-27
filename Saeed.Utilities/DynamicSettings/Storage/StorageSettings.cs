namespace Saeed.Utilities.DynamicSettings.Storage
{
    public class StorageSettings
    {
        public bool SecureStorageCachingEnabled { get; set; } = true;
        public bool StaticFilesCachingEnalbed { get; set; } = true;

        public bool ApplyWatermark { get; set; } = true;
        public string BaseRootPath { get; set; } = "wwwroot";
        public string ProfileFilesPath { get; set; } = "public\\profiles";
        public string UserUploadFilesPath { get; set; } = "public\\uploads";
        public string WatermarkText { get; set; } = "Axeto";
        public string DefaultProfileImage { get; set; } = "assets//user_profile_default.png";
        public string DefaultCoverImage { get; set; } = "assets//user_profile_default_cover.jpg";
        public string GuardImage { get; set; } = "assets\\protected_icon.png";
        public string NotFoundImage { get; set; } = "assets\\axeto-logo-02.png";
        public string WatermarkImage { get; set; } = "assets\\axeto-logo-04.png";
        public string AssetFilesPath { get; set; } = "assets";
        public string PublicFilesPath { get; set; } = "public";
        public string PreviewFilesPath { get; set; } = "public\\previews";
        public string ThumbnailFilesPath { get; set; } = "public\\thumbnails";
        public string PrivateProjectFilesPath { get; set; } = "private\\projects";
        public string PrivateFilesPath { get; set; } = "data";

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
