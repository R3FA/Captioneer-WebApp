using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.DTO;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorMoviesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public CreatorMoviesController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/CreatorMovies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreatorMovie>>> GetCreatorsMovie()
        {
            return await _context.CreatorsMovie.ToListAsync();
        }

        // GET: api/CreatorMovies/5
        [HttpGet("{movieID}")]
        public async Task<ActionResult<IEnumerable<CreatorViewModel>>> GetCreatorMovie(int movieID)
        {
            var dbCreatorMovies = await _context.CreatorsMovie.Where(cm => cm.MovieID == movieID).ToListAsync();
            var creatorViewModels = new List<CreatorViewModel>();

            foreach (var dbCreatorMovie in dbCreatorMovies)
            {
                var creator = await _context.Creators.FindAsync(dbCreatorMovie.CreatorID);
                creatorViewModels.Add(new CreatorViewModel()
                {
                    FirstName = creator!.FirstName,
                    LastName = creator!.Surname,
                    Position = dbCreatorMovie!.Position,
                });
            }

            return Ok(creatorViewModels);
        }
    }
}
