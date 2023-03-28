using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace UtilityService.Utils
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
            IImageFormat? format = null;
            var decodedImage = DecodeImage(encodedimage, ref format);

            if (decodedImage == null)
            {
                return null;
            }

            var fileName = $"{writeName}{DateTime.Now}";
            fileName = fileName.Replace("/", "");
            fileName = fileName.Replace(":", "");
            fileName = fileName.Replace(" ", "");
            fileName += $".{GetFormatString(format!)}";
            var filePath = $"images/users/{fileName}";

            if (!Directory.Exists(Path.Combine(webRootPath, "images", "users")))
            {
                Directory.CreateDirectory(Path.Combine(webRootPath, "images", "users"));
            }

            var savePath = Path.Combine(webRootPath, filePath);
            await decodedImage.SaveAsync(savePath);
            LoggerManager.GetInstance().LogInfo($"Serialized new image to {savePath}");

            return filePath;
        }

        /// <summary>
        /// Deserializes and returns the Base64 representation of an image from the given path
        /// </summary>
        /// <param name="imagePath">Path to the image on disk</param>
        /// <returns>Base64 representation of the image or null if conversion failed</returns>
        public static async Task<string?> Deserialize(string webRootPath, string imageFilePath)
        {
            var filePath = Path.Combine(webRootPath, imageFilePath);

            if (!File.Exists(filePath))
            {
                LoggerManager.GetInstance().LogError($"Cannot deserialize {filePath} because it does not exist");
                return null;
            }

            var encodedImage = EncodeImage(filePath);

            if (encodedImage == null)
            {
                return null;
            }

            return encodedImage;
        }

        /// <summary>
        /// Decodes an image from a Base64 string
        /// </summary>
        /// <param name="encodedImage">The image encoded as a Base64 string</param>
        /// <param name="imageFormat">Reference to a format object so the image's format can be recorded</param>
        /// <returns>The decoded image or null if decoding has failed</returns>
        private static Image? DecodeImage(string encodedImage, ref IImageFormat? imageFormat)
        {
            // Remove header information before conversion
            encodedImage = encodedImage.Substring(encodedImage.LastIndexOf("base64,", StringComparison.InvariantCulture) + 7);

            var buffer = new Span<byte>(new byte[encodedImage.Length]);
            if (!Convert.TryFromBase64String(encodedImage, buffer, out var bytesParsed))
            {
                LoggerManager.GetInstance().LogError("Image could not be converted from Base64");
                return null;
            }

            //Must be under 2MB
            if (buffer.Length == 0 || buffer.Length > 2097152)
            {
                LoggerManager.GetInstance().LogWarning("Image passed for decoding was too large");
                return null;
            }

            var decodedImage = Image.Load(buffer, out var format);

            if (format != JpegFormat.Instance && format != PngFormat.Instance)
            {
                LoggerManager.GetInstance().LogWarning("Wrong format passed for image decode");
                return null;
            }

            imageFormat = format;

            return decodedImage;
        }

        /// <summary>
        /// Encodes an image to a Base64 string
        /// </summary>
        /// <param name="imagePath">Path to the image on disk</param>
        /// <returns>Base64 representation of the image or null if the conversion failed</returns>
        private static string? EncodeImage(string imagePath)
        {
            var type = imagePath.Substring(imagePath.LastIndexOf('.'), 3);

            try
            {
                var imageBytes = File.ReadAllBytes(imagePath);
                var base64 = Convert.ToBase64String(imageBytes);

                if (type == ".png")
                    return "data:image/png;charset=utf-8;base64," + base64;
                else
                    return "data:image/jpg;charset=utf-8;base64," + base64;
            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return null;
            }
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

            LoggerManager.GetInstance().LogWarning("Format passed for check was not expected");
            return "";
        }
    }
}
