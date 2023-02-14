using API.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtitleUserController : ControllerBase
    {

        private readonly CaptioneerDBContext _context;

        public SubtitleUserController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/<SubtitleUserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SubtitleUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SubtitleUserController>
        [HttpPost]
        public async Task<ActionResult> Post(string userEmail, int movieId)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email==userEmail);
            //if (user == null)
            //    return BadRequest();
            //var movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == movieId);
            //if (movie == null)
            //    return BadRequest();
            //new SubtitleUser
            //{
            //    RatingCount = 0,
            //    RatingValue = 0,
            //    SubtitleMovie=new SubtitleMovie
            //    {
            //        Movie=movie,
            //        DownloadCount=0,

            //    }
            //}
            return Ok();
        }

        // PUT api/<SubtitleUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SubtitleUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
