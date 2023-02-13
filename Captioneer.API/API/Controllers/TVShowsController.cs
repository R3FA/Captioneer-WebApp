using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.Utils;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        private readonly IConfiguration _configuration;

        public TVShowsController(CaptioneerDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            {
                var filter = searchQuery.Replace(" ", string.Empty);
                filter = filter.ToLower();
                showsFiltered = await _context.TVShows.Where(tv => tv.Title.ToLower().Replace(" ", string.Empty).Contains(filter)).ToListAsync();
            }

            if (showsFiltered.Count == 0)
            {
                var apiKey = _configuration["ApiKeys:OMDBKey"];
                var model = await OMDbFetcher.Fetch(searchQuery, "series", apiKey);
                var show = await OMDbCacher.CacheShow(model, _context);

                if (show == null)
                    return NotFound();

                showsFiltered.Add(show);
            }

            await EpisoDateCacher.Cache(showsFiltered, _context);

            return Ok(showsFiltered);
        }
    }
}
