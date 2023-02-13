namespace Captioneer.API.DTO
{
    public class TVShowViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string? IMDBId { get; set; } = string.Empty;

        public string? Synopsis { get; set; } = string.Empty;

        public string Year { get; set; } = string.Empty;

        public int? SeasonCount { get; set; }

        public int? EpisodeCount { get; set; }

        public double? IMDBRatingValue { get; set; }

        public int? IMDBRatingCount { get; set; }

        public string? RottenTomatoesValue { get; set; }

        public string? MetacriticValue { get; set; }

        public string? CoverArt { get; set; }
    }
}
