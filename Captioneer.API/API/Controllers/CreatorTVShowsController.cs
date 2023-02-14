using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTO;
using API.Data;
using API.Entities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorTVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public CreatorTVShowsController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/CreatorTVShows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreatorTVShow>>> GetCreatorsTVShows()
        {
            return await _context.CreatorsTVShows.ToListAsync();
        }

        // GET: api/CreatorTVShows/5
        [HttpGet("{showID}")]
        public async Task<ActionResult<IEnumerable<CreatorViewModel>>> GetCreatorTVShow(int showID)
        {
            var dbCreatorTVShows = await _context.CreatorsTVShows.Where(cm => cm.TVShowID == showID).ToListAsync();
            var creatorViewModels = new List<CreatorViewModel>();

            foreach (var dbCreatorTVShow in dbCreatorTVShows)
            {
                var creator = await _context.Creators.FindAsync(dbCreatorTVShow.CreatorID);
                creatorViewModels.Add(new CreatorViewModel()
                {
                    FirstName = creator!.FirstName,
                    LastName = creator!.Surname,
                    Position = dbCreatorTVShow!.Position,
                });
            }

            return Ok(creatorViewModels);
        }
    }
}
