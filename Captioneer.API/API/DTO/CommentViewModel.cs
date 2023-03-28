
namespace API.DTO
{
    public class CommentViewModel
    {
        public int? ID { get; set; }
        public string Username { get; set; }
        public int? SubtitleMovieID { get; set; }
        public int? SubtitleTVShowID { get; set; }
        public string Content { get; set; }
        public int? Page { get; set; }
        public int? TotalPages { get; set; }
    }
}