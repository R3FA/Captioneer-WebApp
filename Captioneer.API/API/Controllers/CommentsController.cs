using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityService.Utils;

namespace API.Controllers
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
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetSubtitleMovieComments(int subtitleID, int page = 1, int pageSize = 10)
        {
            var commentVMs = new List<CommentViewModel>();
            var dbComments = await _context.Comments.Where(c => c.SubtitleMovie != null)
                .Where(c => c.SubtitleMovie!.ID == subtitleID)
                .Include(c => c.User)
                .Include(c => c.SubtitleMovie)
                .ToListAsync();

            var pagedComments = dbComments.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling((double)dbComments.Count / pageSize);

            foreach (var comment in pagedComments)
            {
                commentVMs.Add(new CommentViewModel()
                {
                    Username = comment.User.Username,
                    Content = comment.Content,
                    SubtitleMovieID = comment.SubtitleMovie!.ID,
                    Page = page,
                    TotalPages = totalPages
                });
            }

            return Ok(commentVMs);
        }

        [HttpGet("Shows/{subtitleID}")]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetSubtitleTVShowComments(int subtitleID, int page = 1, int pageSize = 10)
        {
            var commentVMs = new List<CommentViewModel>();
            var dbComments = await _context.Comments.Where(c => c.SubtitleTVShow != null)
                .Where(c => c.SubtitleTVShow!.ID == subtitleID)
                .Include(c => c.User)
                .Include(c => c.SubtitleTVShow)
                .ToListAsync();

            var pagedComments = dbComments.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling((double)dbComments.Count / pageSize);

            foreach (var comment in pagedComments)
            {
                commentVMs.Add(new CommentViewModel()
                {
                    Username = comment.User.Username,
                    Content = comment.Content,
                    SubtitleTVShowID = comment.SubtitleTVShow!.ID,
                    Page = page,
                    TotalPages = totalPages
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
                {
                    LoggerManager.GetInstance().LogError("Status 404: Subtitle with the provided ID was not found");
                    return NotFound("Subtitle with the provided ID was not found");
                }

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
                {
                    LoggerManager.GetInstance().LogError("Status 404: Subtitle with the provided ID was not found");
                    return NotFound("Subtitle with the provided ID was not found");
                }

                var newComment = new Comment()
                {
                    User = dbUser,
                    SubtitleTVShow = dbSubtitleTVShow,
                    Content = model.Content
                };

                await _context.Comments.AddAsync(newComment);
            }
            else
            {
                LoggerManager.GetInstance().LogError("Status 400: A subtitle ID must be provided");
                return BadRequest("A subtitle ID must be provided");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{commentID}")]
        public async Task<IActionResult> DeleteComment(int commentID)
        {
            var dbComment = await _context.Comments.FindAsync(commentID);

            if (dbComment == null)
            {
                LoggerManager.GetInstance().LogError("Status 404: Comment with the provided ID was not found");
                return NotFound("Comment with the provided ID was not found");
            }

            _context.Comments.Remove(dbComment);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
