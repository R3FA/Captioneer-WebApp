using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public LanguagesController(CaptioneerDBContext context)
        {
            this._context = context;
        }
        //GET: api/Langauges
        [HttpGet]
        public async Task<IEnumerable<Language>> getLanguages()
        {
            return await _context.Languages.ToListAsync();
        }
    }
}
