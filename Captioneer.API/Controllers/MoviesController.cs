﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.Data.OMDb;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public MoviesController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        // GET: api/Movies/Guardians+of+The+Galaxy
        // GET: api/Movies/tt8425532
        [HttpGet("{searchQuery}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie(string searchQuery)
        {
            var moviesFiltered = new List<Movie>();

            if (searchQuery.StartsWith("tt"))
                moviesFiltered = await _context.Movies.Where(m => m.IMDBId == searchQuery).ToListAsync();
            else
                moviesFiltered = await _context.Movies.Where(m => m.Title.ToLower().Contains(searchQuery.ToLower())).ToListAsync();

            if (moviesFiltered.Count <= 0)
            {
                var omdbModel = await OMDbFetcher.Fetch(searchQuery, "movie");
                var movie = await OMDbCacher.CacheMovie(omdbModel, _context);

                if (movie == null)
                    return NotFound();

                return Ok(movie);
            }

            return Ok(moviesFiltered);
        }
    }
}