namespace API.DTO
{
    public class SubtitleViewModel
    {
        public string uploader { get; set; } = string.Empty;
        public int? fps { get; set; }
        public string? release { get; set; }
        public double ratingValue { get; set; } 
        public int ratingCount { get; set; } 
        public int downloadCount { get; set; }
        public int subMovieID { get; set; }
    }
}
