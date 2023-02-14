using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using API.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorTVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public ActorTVShowsController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/ActorTVShows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorTVShow>>> GetActorTVShows()
        {
            return await _context.ActorTVShows.ToListAsync();
        }

        // GET: api/ActorTVShows/5
        [HttpGet("{showID}")]
        public async Task<ActionResult<IEnumerable<ActorViewModel>>> GetActorTVShow(int showID)
        {
            var dbActorTVShows = await _context.ActorTVShows.Where(am => am.TVShowID == showID).ToListAsync();
            var actorViewModels = new List<ActorViewModel>();

            foreach (var actorTVShow in dbActorTVShows)
            {
                var actor = await _context.Actors.FindAsync(actorTVShow.ActorID);
                actorViewModels.Add(new ActorViewModel() { FirstName = actor!.FirstName, LastName = actor!.Surname });
            }

            return Ok(actorViewModels);
        }
    }
}
