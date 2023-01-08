namespace Captioneer.API.ViewModels
{
    public class MovieViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string? IMDBId { get; set; } = string.Empty;

        public string? Synopsis { get; set; } = string.Empty;

        public string Year { get; set; } = string.Empty;

        public int? Runtime { get; set; }

        public double? IMDBRatingValue { get; set; }

        public int? IMDBRatingCount { get; set; }

        public string? RottenTomatoesValue { get; set; }

        public string? MetacriticValue { get; set; }

        public string? CoverArt { get; set; }
    }
}
