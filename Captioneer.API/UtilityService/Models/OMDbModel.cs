using System.Text.Json.Serialization;

namespace UtilityService.Models
{
    /// <summary>
    /// Model class for the JSON result from the OMDb API
    /// </summary>
    public class OMDbModel
    {
        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("Year")]
        public string? Year { get; set; }

        [JsonPropertyName("Rated")]
        public string? Rated { get; set; }

        [JsonPropertyName("Released")]
        public string? Released { get; set; }

        [JsonPropertyName("Runtime")]
        public string? Runtime { get; set; }

        [JsonPropertyName("Genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("Director")]
        public string? Director { get; set; }

        [JsonPropertyName("Writer")]
        public string? Writer { get; set; }

        [JsonPropertyName("Actors")]
        public string? Actors { get; set; }

        [JsonPropertyName("Plot")]
        public string? Plot { get; set; }

        [JsonPropertyName("Language")]
        public string? Language { get; set; }

        [JsonPropertyName("Country")]
        public string? Country { get; set; }

        [JsonPropertyName("Awards")]
        public string? Awards { get; set; }

        [JsonPropertyName("Poster")]
        public Uri? Poster { get; set; }

        [JsonPropertyName("Ratings")]
        public OMDbRating[]? Ratings { get; set; }

        [JsonPropertyName("Metascore")]
        public string? Metascore { get; set; }

        [JsonPropertyName("imdbRating")]
        public string? ImdbRating { get; set; }

        [JsonPropertyName("imdbVotes")]
        public string? ImdbVotes { get; set; }

        [JsonPropertyName("imdbID")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("DVD")]
        public string? Dvd { get; set; }

        [JsonPropertyName("BoxOffice")]
        public string? BoxOffice { get; set; }

        [JsonPropertyName("Production")]
        public string? Production { get; set; }

        [JsonPropertyName("Website")]
        public string? Website { get; set; }

        [JsonPropertyName("Response")]
        public string? Response { get; set; }

        [JsonPropertyName("totalSeasons")]
        public string? TotalSeasons { get; set; }
    }

    /// <summary>
    /// Model class for the JSON property relating to ratings
    /// </summary>
    public class OMDbRating
    {
        [JsonPropertyName("Source")]
        public string? Source { get; set; }

        [JsonPropertyName("Value")]
        public string? Value { get; set; }
    }
}
