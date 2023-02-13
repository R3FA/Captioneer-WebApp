using Captioneer.API.Data;
using Captioneer.API.DTO;
using Captioneer.API.Entities;
using Captioneer.API.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public CommentsController(CaptioneerDBContext context)
        {
            _context = context;
        }

        [HttpGet("Movies/{subtitleID}")]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetSubtitleMovieComments(int subtitleID)
        {
            var commentVMs = new List<CommentViewModel>();
            var dbComments = await _context.Comments.Where(c => c.SubtitleMovie != null)
                .Where(c => c.SubtitleMovie!.ID == subtitleID)
                .Include(c => c.User).ToListAsync();

            foreach (var dbComment in dbComments)
            {
                commentVMs.Add(new CommentViewModel()
                {
                    Username = dbComment.User.Username,
                    Content = dbComment.Content,
                });
            }

            return Ok(commentVMs);
        }

        [HttpGet("Shows/{subtitleID}")]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetSubtitleTVShowComments(int subtitleID)
        {
            var commentVMs = new List<CommentViewModel>();
            var dbComments = await _context.Comments.Where(c => c.SubtitleTVShow != null)
                .Where(c => c.SubtitleTVShow!.ID == subtitleID)
                .Include(c => c.User).ToListAsync();

            foreach(var dbComment in dbComments)
            {
                commentVMs.Add(new CommentViewModel()
                {
                    Username = dbComment.User.Username,
                    Content = dbComment.Content,
                });
            }

            return Ok(commentVMs);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(CommentViewModel model)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (dbUser == null)
                return NotFound("User with the provided username was not found");

            if (model.SubtitleMovieID != null)
            {
                var dbSubtitleMovie = await _context.SubtitleMovies.FindAsync(model.SubtitleMovieID);

                if (dbSubtitleMovie == null)
                    return NotFound("Subtitle with the provided ID was not found");

                var newComment = new Comment()
                {
                    User = dbUser,
                    SubtitleMovie = dbSubtitleMovie,
                    Content = model.Content
                };

                await _context.Comments.AddAsync(newComment);
            }
            else if (model.SubtitleTVShowID != null)
            {
                var dbSubtitleTVShow = await _context.SubtitleTVShows.FindAsync(model.SubtitleTVShowID);

                if (dbSubtitleTVShow == null)
                    return NotFound("Subtitle with the provided ID was not found");

                var newComment = new Comment()
                {
                    User = dbUser,
                    SubtitleTVShow = dbSubtitleTVShow,
                    Content = model.Content
                };

                await _context.Comments.AddAsync(newComment);
            }
            else
                return BadRequest("A subtitle ID must be provided");

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{commentID}")]
        public async Task<IActionResult> DeleteComment(int commentID)
        {
            var dbComment = await _context.Comments.FindAsync(commentID);

            if (dbComment == null)
                return NotFound("Comment with the provided ID was not found");

            _context.Comments.Remove(dbComment);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
