﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Captioneer.API.Entities
{
    public class CreatorMovie
    {
        public int CreatorID { get; set; }

        public int MovieID { get; set; }

        [StringLength(30)]
        public string Position { get; set; } = string.Empty;

        [ForeignKey("CreatorID")]
        public virtual Creator Creator { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
    }
}