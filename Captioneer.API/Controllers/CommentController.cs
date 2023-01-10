using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public CommentController(CaptioneerDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> SetComment(CommentViewModel comment)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == comment.UserName);

            if(dbUser == null)
            {
                return NotFound("User isn't found in our database!");
            }
            if (comment.SubtitleMovieID == null && comment.SubtitleTVShowID == null)
            {
                return BadRequest("The comment must be made for a valid movie or a TV series!");
            }
            if(comment.CommentContent.Length > 100)
            {
                return BadRequest("Comment can't contain more than 100 characters!");
            }

            var newComment = new Comment();

            if (comment.SubtitleMovieID != null)
            {
                var dbSubtitleMovie = await _context.SubtitleMovies.FindAsync(comment.SubtitleMovieID);
                if (dbSubtitleMovie != null)
                {
                    newComment.User = dbUser;
                    newComment.SubtitleMovie = dbSubtitleMovie;
                    newComment.Content = comment.CommentContent;
                }
            }
            else if(comment.SubtitleTVShowID != null)
            {
                var dbSubtitleTVShow = await _context.SubtitleTVShows.FindAsync(comment.SubtitleTVShowID);
                if(dbSubtitleTVShow != null)
                {
                    newComment.User = dbUser;
                    newComment.Content = comment.CommentContent;
                    newComment.SubtitleTVShow = dbSubtitleTVShow;
                }
            }
            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return Ok();
        }
        // GET: api/Comment/SubtitleMovies
        [HttpGet("SubtitleMovies")]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetSubtitleMoviesComments(int subtitleMovieID)
        {
            var dbSubtitleMovie = await _context.SubtitleMovies.FindAsync(subtitleMovieID);

            if(dbSubtitleMovie == null)
            {
                return NotFound("Searched SUBTITLE movie isn't found in our database!");
            }

            var dbComments = await _context.Comments.Where(x => x.SubtitleMovie != null).Include(x=> x.User).ToListAsync();
            dbComments = dbComments.Where(x => x.SubtitleMovie!.ID == dbSubtitleMovie.ID).ToList();
            var commentViewModels = new List<CommentViewModel>();

            foreach (var comment in dbComments)
            {
                commentViewModels.Add(new CommentViewModel()
                {
                    UserName = comment.User.Username,
                    CommentContent = comment.Content,
                    SubtitleMovieID= dbSubtitleMovie.ID,
                });
            }

            return Ok(commentViewModels);
        }
        // GET: api/Comment/SubtitleTVShows
        [HttpGet("SubtitleTVShows")]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetSubtitleTVShowComments(int subtitleTVShowID)
        {
            var dbSubtitleTVShow = await _context.SubtitleTVShows.FindAsync(subtitleTVShowID);

            if (dbSubtitleTVShow == null)
            {
                return NotFound("Searched SUBTITLE movie isn't found in our database!");
            }

            var dbComments = await _context.Comments.Where(x => x.SubtitleTVShow != null).Include(x => x.User).ToListAsync();
            dbComments = dbComments.Where(x => x.SubtitleTVShow!.ID == dbSubtitleTVShow.ID).ToList();
            var commentViewModels = new List<CommentViewModel>();

            foreach (var comment in dbComments)
            {
                commentViewModels.Add(new CommentViewModel()
                {
                    UserName = comment.User.Username,
                    CommentContent = comment.Content,
                    SubtitleTVShowID = dbSubtitleTVShow.ID,
                });
            }
            return Ok(commentViewModels);
        }
    }
}