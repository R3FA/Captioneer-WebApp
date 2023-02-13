using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.DTO;

namespace Captioneer.API.Controllers
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

            if (dbActorMovies.Count == 0)
                return NotFound();

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
