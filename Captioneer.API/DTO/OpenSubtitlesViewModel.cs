using System.Text.Json.Serialization;

namespace Captioneer.API.DTO
{
    public class OpenSubtitlesViewModel
    {
        public int? FileId { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public double? Fps { get; set; }

        public DateTime? UploadDate { get; set; }

        public string Release { get; set; }

        public string Uploader { get; set; }
    }
}
