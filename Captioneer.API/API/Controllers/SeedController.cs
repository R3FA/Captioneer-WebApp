using API.Data;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityService.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;
        private readonly IConfiguration _configuration;

        public SeedController(IConfiguration configuration, CaptioneerDBContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> SeedMedia(IFormFile dataset)
        {
            var apiKey = _configuration["ApiKeys:OMDBKey"];
            var models = await DatasetParser.ParseDataset(dataset.OpenReadStream(), apiKey);

            await Task.Run(async () =>
            {
                foreach(var model in models)
                {
                    if (model.Type == "movie" && !await _context.Movies.AnyAsync(m => m.IMDBId == model.ImdbId))
                        await OMDbCacher.CacheMovie(model, _context);
                    else if (!await _context.TVShows.AnyAsync(m => m.IMDBId == model.ImdbId))
                        await OMDbCacher.CacheShow(model, _context);

                    LoggerManager.GetInstance().LogInfo($"Seeded {model.Title} to database");
                }
            });

            return Ok();
        }
    }
}
