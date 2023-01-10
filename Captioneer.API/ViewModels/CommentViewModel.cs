using Captioneer.API.Entities;

namespace Captioneer.API.ViewModels
{
    public class CommentViewModel
    {
        public string UserName { get; set; }
        public string CommentContent { get; set; }
        public int? SubtitleMovieID { get; set; }
        public int? SubtitleTVShowID { get; set; }
    }
}