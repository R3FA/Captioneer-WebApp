using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityService.Utils;
using API.Entities;
using API.Data;
using API.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;
        private readonly IConfiguration _configuration;

        public MoviesController(CaptioneerDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<IActionResult> GetMovies(int page = 1, int pageSize = 10)
        {
            var data = await _context.Movies.ToListAsync();
            var totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling((double)(totalRecords / pageSize));
            var pagedData = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                totalRecords,
                totalPages,
                data = pagedData
            });
        }

        // GET: api/Movies/Guardians+of+The+Galaxy
        // GET: api/Movies/tt8425532
        [HttpGet("{searchQuery}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie(string searchQuery)
        {
            var moviesFiltered = new List<Movie>();
            searchQuery = searchQuery.Trim();

            if (searchQuery.StartsWith("tt"))
                moviesFiltered = await _context.Movies.Where(m => m.IMDBId == searchQuery).ToListAsync();
            else
            {
                var filter = searchQuery.Replace(" ", string.Empty);
                filter = filter.ToLower();
                moviesFiltered = await _context.Movies.Where(m => m.Title.ToLower().Replace(" ", string.Empty).Contains(filter)).ToListAsync();
            }

            if (moviesFiltered.Count <= 0)
            {
                var apiKey = _configuration["ApiKeys:OMDBKey"];
                var omdbModel = await OMDbFetcher.Fetch(searchQuery, "movie", apiKey);

                if (omdbModel != null && omdbModel.TotalSeasons != null)
                    if (int.Parse(omdbModel.TotalSeasons) > 0)
                        return moviesFiltered;

                var movie = await OMDbCacher.CacheMovie(omdbModel, _context);

                if (movie == null)
                {
                    LoggerManager.GetInstance().LogError($"Status 404: Unable to find movie {searchQuery} on OMDb");
                    return NotFound($"Unable to find movie {searchQuery} on OMDb");
                }

                return Ok(movie);
            }

            return Ok(moviesFiltered);
        }
    }
}
