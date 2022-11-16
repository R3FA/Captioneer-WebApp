using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.ViewModels;

namespace Captioneer.API.Controllers
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

            if (dbActorTVShows.Count == 0)
                return NotFound();

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
