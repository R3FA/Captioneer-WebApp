using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using API.DTO;
using UtilityService.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorMoviesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public ActorMoviesController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/ActorMovies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorMovie>>> GetActorMovies()
        {
            return await _context.ActorMovies.ToListAsync();
        }

        // GET: api/ActorMovies/5
        [HttpGet("{movieID}")]
        public async Task<ActionResult<IEnumerable<ActorViewModel>>> GetActorMovie(int movieID)
        {
            var dbActorMovies = await _context.ActorMovies.Where(am => am.MovieID == movieID).ToListAsync();
            var actorViewModels = new List<ActorViewModel>();

            foreach (var actorMovie in dbActorMovies)
            {
                var actor = await _context.Actors.FindAsync(actorMovie.ActorID);
                actorViewModels.Add(new ActorViewModel() { FirstName = actor!.FirstName, LastName = actor!.Surname });
            }

            return Ok(actorViewModels);
        }
    }
}
