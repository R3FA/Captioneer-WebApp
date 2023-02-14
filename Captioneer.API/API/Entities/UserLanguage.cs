using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class UserLanguage
    {
        public int UserID { get; set; }

        public int LanguageID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("LanguageID")]
        public virtual Language Language { get; set; }
    }
}
