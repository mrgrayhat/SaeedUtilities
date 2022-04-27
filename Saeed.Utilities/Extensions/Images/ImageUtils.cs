//using LazZiya.ImageResize;
//using LazZiya.ImageResize.ColorFormats;
//using LazZiya.ImageResize.ResizeMethods;
//using LazZiya.ImageResize.Tools;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

using Huddeh.LocalizedResource.ErrorMessages;
using Saeed.Utilities.Contracts;
using Saeed.Utilities.Extensions.Images.ResizeMethods;
using Saeed.Utilities.Types.Enums;

using Microsoft.AspNetCore.Http;

using Saeed.Utilities.DynamicSettings.MethodsSetting;
using Saeed.Utilities.Extensions.Files;
using Saeed.Utilities.Extensions.Images.Options;
using Saeed.Utilities.Extensions.Units;

namespace Saeed.Utilities.Extensions.Images
{
    public static class ImageUtils
    {
        private const int exifOrientationID = 0x112; //274
        /// <summary>
        /// for storage, album and portfolios
        /// </summary>
        private static readonly IDictionary<string, string> ImageMimeDictionary = new Dictionary<string, string>
        {
            { ".bmp", "image/bmp" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" }
        };

        /// <summary>
        /// for ticket or support uploads. allowed multiple file / safe types
        /// </summary>
        private static readonly IDictionary<string, string> DocumentsMimeDictionary = new Dictionary<string, string>
        {
            { "xls", "application/vnd.ms-excel" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".pdf", "application/pdf" },
            { ".rar", "application/x-rar-compressed" },
            { ".zip", "application/zip" },
            { ".tar", "application/x-tar" },
        };

        public static MessageContract IsValidAttachmentDocument(IFormFile file, DocumentFileValidationSettings fileValidationSettings = null)
        {
            //todo: safe file type detection (no extension only)
            fileValidationSettings ??= new DocumentFileValidationSettings();

            try
            {
                if (fileValidationSettings.ValidFormats != null && fileValidationSettings.ValidFormats.Any()) // custom formats
                {
                    if (!fileValidationSettings.ValidFormats.Any(x => x == file.GetFileExtension().ToLower()))
                    {
                        return MessageContract.Bad(message: ErrorMessages.FileFormatIsNowAllowed,
                           errors: new List<string>
                            {
                                ErrorMessages.SupportedFileFormats.Replace("{0}",string.Join(",",fileValidationSettings.ValidFormats))
                            });
                    }
                }
                else // defaults
                {
                    var fileExt = file.GetFileExtension().ToLower();
                    if (!ImageMimeDictionary.ContainsKey(fileExt) && !DocumentsMimeDictionary.ContainsKey(fileExt))
                    {
                        return MessageContract.Bad(message: ErrorMessages.FileFormatIsNowAllowed,
                           errors: new List<string>
                           {
                                ErrorMessages.SupportedFileFormats.Replace("{0}", string.Join(", ", DocumentsMimeDictionary.Keys.ToList(), ImageMimeDictionary.Keys.ToList()))
                           });
                    }
                }

                switch (fileValidationSettings.Policy)
                {
                    case FileUploadValidationPolicy.Format:
                        // DocumentsMimeDictionary is checked before. so extension and file bytes are valid image.
                        break;
                    default:
                    case FileUploadValidationPolicy.FormatAndSize:
                    case FileUploadValidationPolicy.Size:
                        if (file.Length > fileValidationSettings.MaxLength)
                            return MessageContract.Bad(message: ErrorMessages.FileSizeIsMoreThanMb.Replace("{0}", fileValidationSettings.MaxLength.ConvertBytesToShortMegabytes().ToString()));

                        if (file.Length < fileValidationSettings.MinLength)
                            return MessageContract.Bad(message: ErrorMessages.FileSizeIsLessThanMb.Replace("{0}", fileValidationSettings.MinLength.ConvertBytesToShortMegabytes().ToString()));
                        break;
                }

                return MessageContract.Ok();

            }
            catch // invalid image file
            {
                return MessageContract.Bad(message: ErrorMessages.InvalidImageFormat);
            }

        }
        public static MessageContract IsValidImage(IFormFile image, ImageValidationSettings imageValidtionSettings = null)
        {
            imageValidtionSettings ??= new ImageValidationSettings();
            try
            {
                if (imageValidtionSettings.ValidFormats != null && imageValidtionSettings.ValidFormats.Any()) // custom formats
                {
                    if (!imageValidtionSettings.ValidFormats.Any(x => x == image.GetFileExtension().ToLower()))
                    {
                        return MessageContract.Bad(message: ErrorMessages.InvalidImageFormat,
                           errors: new List<string>
                            {
                                ErrorMessages.SupportedFileFormats.Replace("{0}",string.Join(",",imageValidtionSettings.ValidFormats))
                            });
                    }
                }
                else // defaults
                {
                    if (!ImageMimeDictionary.ContainsKey(image.GetFileExtension().ToLower()))
                    {
                        return MessageContract.Bad(message: ErrorMessages.InvalidImageFormat,
                           errors: new List<string>
                           {
                                ErrorMessages.SupportedFileFormats.Replace("{0}", string.Join(", ",ImageMimeDictionary.Keys.ToList()))
                           });
                    }
                }
                // real image bytes check and other policies
                using var imageStream = Image.FromStream(image.OpenReadStream());

                switch (imageValidtionSettings.Policy)
                {
                    case FileUploadValidationPolicy.Format:
                        // ImageMimeDictionary is checked before. so extension and file bytes are valid image.
                        break;
                    case FileUploadValidationPolicy.Resolution:
                        if (imageStream.CheckMinResolution(imageValidtionSettings.Resolution))
                            return MessageContract.Bad(message: ErrorMessages.ImageResolutionIsLow);

                        if (imageStream.CheckMaxResolution(imageValidtionSettings.Resolution))
                            return MessageContract.Bad(message: ErrorMessages.ImageResolutionIsLow);

                        break;
                    default:
                    case FileUploadValidationPolicy.FormatAndSize:
                    case FileUploadValidationPolicy.Size:
                        if (image.Length > imageValidtionSettings.MaxLength)
                            return MessageContract.Bad(message: ErrorMessages.ImageSizeIsLargerThanMb.Replace("{0}", imageValidtionSettings.MaxLength.ConvertBytesToShortMegabytes().ToString()));

                        if (image.Length < imageValidtionSettings.MinLength)
                            return MessageContract.Bad(message: ErrorMessages.ImageSizeIsLessThanMb.Replace("{0}", imageValidtionSettings.MinLength.ConvertBytesToShortMegabytes().ToString()));
                        break;
                }

                return MessageContract.Ok();

            }
            catch // invalid image file
            {
                return MessageContract.Bad(message: ErrorMessages.InvalidImageFormat);
            }

        }

        public static bool CheckMaxResolution(this Image image, PointF resolution)
        {
            if (image.HorizontalResolution > resolution.X || image.VerticalResolution > resolution.Y)
                return false;
            return true;
        }
        public static bool CheckMinResolution(this Image image, PointF resolution)
        {
            if (image.HorizontalResolution < resolution.X || image.VerticalResolution < resolution.Y)
                return false;
            return true;
        }

        public static Image ClearGPSAndPrivateInfo(this Image image)
        {
            image.RemovePropertyItem(1);

            return image;
        }
        public static ImageFormat GetRawImageFormat(byte[] fileBytes)
        {
            using (var ms = new MemoryStream(fileBytes))
            {
                var fileImage = Image.FromStream(ms);
                return fileImage.RawFormat;
            }
        }


        public static void Crop(int width, int height, Stream imageStream, string saveFilePath)
        {
            Bitmap sourceImage = new Bitmap(imageStream);
            using (Bitmap objBitmap = new Bitmap(width, height))
            {
                objBitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
                using (Graphics objGraphics = Graphics.FromImage(objBitmap))
                {
                    // Set the graphic format for better result cropping   
                    objGraphics.SmoothingMode = SmoothingMode.AntiAlias;

                    objGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    objGraphics.DrawImage(sourceImage, 0, 0, width, height);

                    // Save the file path, note we use png format to support png file   
                    objBitmap.Save(saveFilePath);
                }
            }
        }

        /// <summary>
        /// add an single text watermark
        /// </summary>
        /// <param name="img"></param>
        /// <param name="text"></param>
        /// <param name="ops"></param>
        /// <param name="rotateAngle"></param>
        /// <returns></returns>
        public static Image AddTextWatermark(this Image img, string text, TextWatermarkOptions ops, float rotateAngle = 0)
        {
            using (Graphics graphics = Graphics.FromImage(img))
            {
                Rectangle rectangle = TextWatermarkPosition.SetBGPos(img.Width, img.Height, ops.FontSize, ops.Location, ops.Margin);

                StringFormat stringFormat = new StringFormat()
                {
                    FormatFlags = StringFormatFlags.NoWrap
                };

                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                if (ops.BGColor.A > 0)
                {
                    SolidBrush solidBrush = new SolidBrush(ops.BGColor);
                    graphics.FillRectangle(solidBrush, rectangle);
                }

                using FontFamily family = new FontFamily(ops.FontName);
                using Font font = new Font(family, ops.FontSize, ops.FontStyle, GraphicsUnit.Pixel);

                PointF pointF = new PointF(TextWatermarkPosition.SetTextAlign(graphics.MeasureString(text, font, img.Width, stringFormat), img.Width, ops.Location), rectangle.Y + rectangle.Height / 4);
                graphics.RotateTransform(rotateAngle);
                using (Pen pen = new Pen(new SolidBrush(ops.OutlineColor), ops.OutlineWidth))
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddString(text, family, (int)ops.FontStyle, ops.FontSize, pointF, stringFormat);
                        Color color = ops.OutlineColor;
                        if (color.A > 0)
                            graphics.DrawPath(pen, path);
                        color = ops.TextColor;
                        if (color.A > 0)
                        {
                            SolidBrush solidBrush = new SolidBrush(ops.TextColor);
                            graphics.FillPath(solidBrush, path);
                        }
                    }
                }
            }
            return img;
        }


        public static Image AddRepeatedTextWatermark(this Image img, string text, TextWatermarkOptions ops, float rotateAngle = 0)
        {
            using (Graphics graphics = Graphics.FromImage(img))
            {
                int y = -50;
                Rectangle rectangle = TextWatermarkPosition.SetBGPos(img.Width, img.Height, ops.FontSize, ops.Location, ops.Margin);

                StringFormat stringFormat = new StringFormat()
                {
                    FormatFlags = StringFormatFlags.NoWrap
                };

                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                //graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                //var state = graphics.Save();
                //graphics.ResetTransform();
                //graphics.TranslateTransform(img.Width / 2, img.Height / 2);
                //graphics.RotateTransform(rotateAngle);
                if (ops.BGColor.A > 0)
                {
                    using SolidBrush solidBrush = new SolidBrush(ops.BGColor);
                    graphics.FillRectangle(solidBrush, rectangle);
                }
                Color color = ops.OutlineColor;
                using FontFamily family = new FontFamily(ops.FontName);
                using Font font = new Font(family, ops.FontSize, ops.FontStyle, GraphicsUnit.Pixel);
                using (Pen pen = new Pen(new SolidBrush(ops.OutlineColor), ops.OutlineWidth))
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        PointF pointF = new PointF(TextWatermarkPosition.SetTextAlign(graphics.MeasureString(text, font, img.Width, stringFormat), img.Width, ops.Location), rectangle.Y + rectangle.Height / 4);

                        path.AddString(text, family, (int)ops.FontStyle, ops.FontSize, pointF, stringFormat);
                        if (color.A > 0)
                            graphics.DrawPath(pen, path);
                        color = ops.TextColor;
                        if (color.A > 0)
                        {
                            using var solidBrush = new SolidBrush(ops.TextColor);
                            graphics.FillPath(solidBrush, path);
                        }
                    }
                }
            }
            return img;
        }

        public static Image AddImageWatermark(
            this Image img,
            Image watermarkImage,
            ImageWatermarkOptions ops)
        {
            if (ops.Opacity > 0)
            {
                using (Graphics graphics = Graphics.FromImage(img))
                {
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.CompositingMode = CompositingMode.SourceOver;
                    if (ops.Opacity < 100)
                        watermarkImage = ChangeImageOpacity(watermarkImage, ops.Opacity);
                    int width = watermarkImage.Width;
                    int height = watermarkImage.Height;
                    PointF pointF = ImageWatermarkPosition.ImageWatermarkPos(img.Width, img.Height, width, height, ops.Location, ops.Margin);
                    graphics.DrawImage(watermarkImage, pointF.X, pointF.Y, width, height);
                }
            }
            watermarkImage.Dispose();
            return img;
        }
        public static Image ChangeImageOpacity(Image image, int opacity)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (ImageAttributes imageAttributes = new ImageAttributes())
                {
                    ColorMatrix newColorMatrix = new ColorMatrix();
                    //if (opacity < 100)
                    //    graphics.Clear(Color.White);
                    newColorMatrix.Matrix33 = opacity / 100f;
                    imageAttributes.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    graphics.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }
            return bitmap;
        }


        //public static Image Scale(this Image img, int newWidth, int newHeight)
        //{
        //    LazZiya.ImageResize.ResizeMethods.Scale scale = new LazZiya.ImageResize.ResizeMethods.Scale(img.get_Size(), new Size(newWidth, newHeight));
        //    return Resize(img, scale.SourceRect, scale.TargetRect);
        //}
        public static Image Resize(this Image img, int width, int height, GraphicOptions ops)
        {
            PixelFormat format = ((Bitmap)img).GetColorFormat() == ImageColorFormatTypes.Cmyk ? PixelFormat.Format32bppArgb : img.PixelFormat;

            using (Bitmap bitmap = new Bitmap(width, height, format))
            {
                //bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = ops.SmoothingMode;
                    graphics.InterpolationMode = ops.InterpolationMode;
                    graphics.PixelOffsetMode = ops.PixelOffsetMode;
                    graphics.CompositingQuality = ops.CompositingQuality;
                    graphics.CompositingMode = ops.CompositingMode;
                    graphics.PageUnit = ops.PageUnit;
                    //graphics.DrawImage(img, target, source, ops.PageUnit);
                }
                if (img.PixelFormat != PixelFormat.Format32bppArgb)
                    return Image.FromHbitmap(bitmap.GetHbitmap());
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, img.RawFormat);
                    return Image.FromStream(memoryStream);
                }
            }
        }
        public static void ExifRotateFlipIfPossible(this Image image)
        {
            try
            {
                foreach (var prop in image.PropertyItems)
                {
                    image.SetPropertyItem(prop);
                }
                image.ExifRotate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// get image rotation and flip if it's orientation was rotated
        /// </summary>
        /// <param name="img"></param>
        public static void ExifRotate(this Image img)
        {
            try
            {
                if (!img.PropertyIdList.Contains(exifOrientationID))
                    return;

                var prop = img.GetPropertyItem(exifOrientationID);
                int val = prop.Value[1];
                var rot = RotateFlipType.RotateNoneFlipNone;

                if (val == 3 || val == 4)
                    rot = RotateFlipType.Rotate180FlipNone;
                else if (val == 5 || val == 6)
                    rot = RotateFlipType.Rotate90FlipNone;
                else if (val == 7 || val == 8)
                    rot = RotateFlipType.Rotate270FlipNone;

                if (val == 2 || val == 4 || val == 5 || val == 7)
                    rot |= RotateFlipType.RotateNoneFlipX;

                if (rot != RotateFlipType.RotateNoneFlipNone)
                    img.RotateFlip(rot);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Image Resize(this Image img, int width, int height)
        {
            try
            {
                var size = new Scale(new Size(img.Width, img.Height), new Size(width, height));
                return img.GetThumbnailImage(size.TargetRect.Width, size.TargetRect.Height, () => false, IntPtr.Zero);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }

    internal abstract class ImageWatermarkPosition
    {
        internal static PointF ImageWatermarkPos(
            int imgWidth,
            int imgHeight,
            int wmWidth,
            int wmHeight,
            TargetSpot spot,
            int margin)
        {
            PointF pointF;
            switch (spot)
            {
                case TargetSpot.TopLeft:
                    pointF = new PointF(margin, margin);
                    break;
                case TargetSpot.TopMiddle:
                    pointF = new PointF(imgWidth / 2 - wmWidth / 2, margin);
                    break;
                case TargetSpot.MiddleLeft:
                    pointF = new PointF(margin, imgHeight / 2 - wmHeight / 2);
                    break;
                case TargetSpot.Center:
                    pointF = new PointF(imgWidth / 2 - wmWidth / 2, imgHeight / 2 - wmHeight / 2);
                    break;
                case TargetSpot.MiddleRight:
                    pointF = new PointF(imgWidth - (wmWidth + margin), imgHeight / 2 - wmHeight / 2);
                    break;
                case TargetSpot.BottomLeft:
                    pointF = new PointF(margin, imgHeight - (wmHeight + margin));
                    break;
                case TargetSpot.BottomMiddle:
                    pointF = new PointF(imgWidth / 2 - wmWidth / 2, imgHeight - (wmHeight + margin));
                    break;
                case TargetSpot.BottomRight:
                    pointF = new PointF(imgWidth - (wmWidth + margin), imgHeight - (wmHeight + margin));
                    break;
                default:
                    pointF = new PointF(imgWidth - (margin + wmWidth), margin);
                    break;
            }
            return pointF;
        }
    }
    internal abstract class TextWatermarkPosition
    {
        /// <summary>
        /// Calculate the watermark text background size and position according to the taret spot,
        /// main image size and font size.
        /// </summary>
        /// <param name="imgWidth">Main image width</param>
        /// <param name="imgHeight">Main image height</param>
        /// <param name="fontSize">Font size</param>
        /// <param name="spot">target spot</param>
        /// <param name="margin">Distance from the nearest border</param>
        /// <returns></returns>
        internal static Rectangle SetBGPos(
          int imgWidth,
          int imgHeight,
          int fontSize,
          TargetSpot spot,
          int margin)
        {
            int height = fontSize * 2;
            Rectangle rectangle;
            switch (spot)
            {
                case TargetSpot.TopLeft:
                case TargetSpot.TopMiddle:
                case TargetSpot.TopRight:
                    rectangle = new Rectangle(0, margin, imgWidth, height);
                    break;
                case TargetSpot.MiddleLeft:
                case TargetSpot.Center:
                case TargetSpot.MiddleRight:
                    rectangle = new Rectangle(0, imgHeight / 2 - height / 2, imgWidth, height);
                    break;
                default:
                    rectangle = new Rectangle(0, imgHeight - height - margin, imgWidth, height);
                    break;
            }
            return rectangle;
        }

        internal static int SetTextAlign(SizeF textMetrics, int imgWidth, TargetSpot spot)
        {
            int num;
            switch (spot)
            {
                case TargetSpot.TopMiddle:
                case TargetSpot.Center:
                case TargetSpot.BottomMiddle:
                    num = (int)(imgWidth - (double)textMetrics.Width) / 2;
                    break;
                case TargetSpot.TopRight:
                case TargetSpot.MiddleRight:
                case TargetSpot.BottomRight:
                    num = (int)(imgWidth - (double)textMetrics.Width) - 5;
                    break;
                default:
                    num = 5;
                    break;
            }
            return num;
        }
    }

}
