using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Data.OMDb;
using Captioneer.API.Entities;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public TVShowsController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/TVShows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TVShow>>> GetTVShows()
        {
            return await _context.TVShows.ToListAsync();
        }

        // GET: api/TVShows/5
        [HttpGet("{searchQuery}")]
        public async Task<ActionResult<IEnumerable<TVShow>>> GetTVShow(string searchQuery)
        {
            var showsFiltered = new List<TVShow>();

            if (searchQuery.StartsWith("tt"))
                showsFiltered = await _context.TVShows.Where(tv => tv.IMDBId == searchQuery).ToListAsync();
            else
                showsFiltered = await _context.TVShows.Where(tv => tv.Title.ToLower().Contains(searchQuery)).ToListAsync();

            if (showsFiltered.Count == 0)
            {
                var model = await OMDbFetcher.Fetch(searchQuery, "series");
                var show = await OMDbCacher.CacheShow(model, _context);

                if (show == null)
                    return NotFound();

                return Ok(show);
            }

            return Ok(showsFiltered);
        }
    }
}
