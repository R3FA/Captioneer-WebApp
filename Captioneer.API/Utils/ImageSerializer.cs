
using Captioneer.API.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace Captioneer.API.Utils
{
    public static class ImageSerializer
    {
        /// <summary>
        /// Serializes an image after decoding it from a Base64 string
        /// </summary>
        /// <param name="encodedimage">The image encoded as Base64</param>
        /// <param name="webRootPath">Path to the wwwroot directory</param>
        /// <param name="writeName">Name of the file to be written</param>
        /// <returns>Full path of the newly written file or null if writing has failed</returns>
        public static async Task<string?> Serialize(string encodedimage, string webRootPath, string writeName)
        {
            var imagesPath = Path.Combine(webRootPath, "images/users");
            IImageFormat? format = null;
            var decodedImage = DecodeImage(encodedimage, ref format);

            if (decodedImage == null)
            {
                return null;
            }

            var filePath = Path.Combine(imagesPath, writeName + $".{GetFormatString(format!)}");
            await decodedImage.SaveAsync(filePath);

            return filePath;
        }

        /// <summary>
        /// Decodes an image from a Base64 string
        /// </summary>
        /// <param name="encodedImage">The image encoded as a Base64 string</param>
        /// <param name="imageFormat">Reference to a format object so the image's format can be recorded</param>
        /// <returns>The decoded image or null if decoding has failed</returns>
        public static Image? DecodeImage(string encodedImage, ref IImageFormat? imageFormat)
        {
            // Remove header information before conversion
            encodedImage = encodedImage.Substring(encodedImage.LastIndexOf("base64,", StringComparison.InvariantCulture) + 7);

            var buffer = new Span<byte>(new byte[encodedImage.Length]);
            if (!Convert.TryFromBase64String(encodedImage, buffer, out var bytesParsed))
                return null;

            //Must be under 2MB
            if (buffer.Length == 0 || buffer.Length > 2097152)
            {
                return null;
            }

            var decodedImage = Image.Load(buffer, out var format);

            if (format != JpegFormat.Instance && format != PngFormat.Instance)
                return null;

            imageFormat = format;

            return decodedImage;
        }

        /// <summary>
        /// Gets the extension name of an image based on its format
        /// </summary>
        /// <param name="format">Object describing the image's format</param>
        /// <returns>A string with the extension name</returns>
        private static string GetFormatString(IImageFormat format)
        {
            if (format == JpegFormat.Instance)
                return "jpg";
            if (format == PngFormat.Instance)
                return "png";

            return "";
        }
    }
}
