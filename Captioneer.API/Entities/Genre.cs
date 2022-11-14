﻿using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
    public class Genre
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;
    }
}