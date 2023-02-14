using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public LanguagesController(CaptioneerDBContext context)
        {
            _context = context;
        }

        //GET: api/Languages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Language>>> GetLanguages()
        {
            var dbLanguages = await _context.Languages.ToListAsync();

            return Ok(dbLanguages);
        }
    }
}
