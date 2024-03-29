﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("actors")]
    public class Actor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        public string Surname { get; set; } = string.Empty;
    }
}
