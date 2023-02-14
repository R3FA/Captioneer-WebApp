﻿using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class UserMovies
    {
        public int UserID { get; set; }

        public int MovieID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
    }
}
