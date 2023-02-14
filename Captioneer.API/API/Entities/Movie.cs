﻿using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string IMDBId { get; set; } = string.Empty;

        [Required]
        [StringLength(600)]
        public string Synopsis { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Year { get; set; } = string.Empty;

        // In minutes
        [Required]
        [Range(1, int.MaxValue)]
        public int Runtime { get; set; }

        [Required]
        [Range(0.0, 10.0)]
        public double IMDBRatingValue { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int IMDBRatingCount { get; set; }

        [StringLength(3)]
        public string? RottenTomatoesValue { get; set; }

        [StringLength(7)]
        public string? MetacriticValue { get; set; }

        public string? CoverArt { get; set; }
    }
}
