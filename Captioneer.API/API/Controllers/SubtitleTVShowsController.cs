using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtitleTVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public SubtitleTVShowsController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/SubtitleTVShows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubtitleTVShow>>> GetSubtitleTVShows()
        {
            return await _context.SubtitleTVShows.ToListAsync();
        }

        // GET: api/SubtitleTVShows/tt0944947/1/1
        [HttpGet("{imdbID}/{seasonNumber}/{episodeNumber}")]
        public async Task<ActionResult<SubtitleTVShow?>> GetSubtitleTVShow(string imdbID, int seasonNumber, int episodeNumber)
        {
            return default(SubtitleTVShow);
        }
    }
}
