﻿using Newtonsoft.Json;

namespace Captioneer.API.Data.OMDb
{
    public class OMDbModel
    {
        [JsonProperty("Title")]
        public string? Title { get; set; }

        [JsonProperty("Year")]
        public string? Year { get; set; }

        [JsonProperty("Rated")]
        public string? Rated { get; set; }

        [JsonProperty("Released")]
        public string? Released { get; set; }

        [JsonProperty("Runtime")]
        public string? Runtime { get; set; }

        [JsonProperty("Genre")]
        public string? Genre { get; set; }

        [JsonProperty("Director")]
        public string? Director { get; set; }

        [JsonProperty("Writer")]
        public string? Writer { get; set; }

        [JsonProperty("Actors")]
        public string? Actors { get; set; }

        [JsonProperty("Plot")]
        public string? Plot { get; set; }

        [JsonProperty("Language")]
        public string? Language { get; set; }

        [JsonProperty("Country")]
        public string? Country { get; set; }

        [JsonProperty("Awards")]
        public string? Awards { get; set; }

        [JsonProperty("Poster")]
        public Uri? Poster { get; set; }

        [JsonProperty("Ratings")]
        public OMDbRating[]? Ratings { get; set; }

        [JsonProperty("Metascore")]
        public string? Metascore { get; set; }

        [JsonProperty("imdbRating")]
        public string? ImdbRating { get; set; }

        [JsonProperty("imdbVotes")]
        public string? ImdbVotes { get; set; }

        [JsonProperty("imdbID")]
        public string? ImdbId { get; set; }

        [JsonProperty("Type")]
        public string? Type { get; set; }

        [JsonProperty("DVD")]
        public string? Dvd { get; set; }

        [JsonProperty("BoxOffice")]
        public string? BoxOffice { get; set; }

        [JsonProperty("Production")]
        public string? Production { get; set; }

        [JsonProperty("Website")]
        public string? Website { get; set; }

        [JsonProperty("Response")]
        public string? Response { get; set; }

        [JsonProperty("totalSeasons")]
        public string? TotalSeasons { get; set; }
    }
}
