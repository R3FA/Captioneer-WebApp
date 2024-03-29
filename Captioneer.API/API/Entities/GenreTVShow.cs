﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("genrestvshows")]
    public class GenreTVShow
    {
        public int GenreID { get; set; }

        public int TVShowID { get; set; }

        [ForeignKey("GenreID")]
        public virtual Genre Genre { get; set; }

        [ForeignKey("TVShowID")]
        public virtual TVShow TVShow { get; set; }
    }
}
