using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityService.Utils;
using API.Data;
using API.Entities;
using API.Utils;

namespace API.Controllers
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
        public async Task<IActionResult> GetTVShows(int page = 1, int pageSize = 10)
        {
            var data = await _context.TVShows.ToListAsync();
            var totalRecords = data.Count();
            var totalPages = (int)(totalRecords / pageSize);
            var pagedData = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                totalRecords,
                totalPages,
                data = pagedData
            });
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

                if (model != null && model.TotalSeasons == null)
                    return showsFiltered;
                if (model != null && model.TotalSeasons != null)
                    if (int.Parse(model.TotalSeasons) == 0)
                        return showsFiltered;

                var show = await OMDbCacher.CacheShow(model, _context);

                if (show == null)
                {
                    LoggerManager.GetInstance().LogError($"Could not fetch show {searchQuery} from OMDb");
                    return NotFound($"Could not fetch show {searchQuery} from OMDb");
                }

                showsFiltered.Add(show);
            }

            await EpisoDateCacher.Cache(showsFiltered, _context);

            return Ok(showsFiltered);
        }
    }
}
