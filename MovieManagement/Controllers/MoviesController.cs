using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManagementDataAccess.IRepositories;
using MovieManagementDataAccess.Models;

namespace MovieManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly MovieContext _context;
        public MoviesController(IMovieRepository repository,MovieContext movieContext)

        {
            _movieRepository = repository;
            _context = movieContext;
            
        }

        // GET: api/SearchMovies
        [HttpPost("SearchMovies")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<IEnumerable<Movie>>> SearchMovies(FilterCriteria filterCriteria)
        {
            if (Validate(filterCriteria))
            {
                var respose = await _movieRepository.getMoviesAsync(filterCriteria);

                if (respose == null)
                {
                    return NotFound();
                }

                return respose;
            }
            else
            {
                return BadRequest();
            }
        }

        private bool Validate(FilterCriteria filterCriteria)
        {
            if ((filterCriteria.genres == string.Empty) && (filterCriteria.title == string.Empty) && (filterCriteria.yearOfRelease == 0))
            {
                return false;
            }
            else
            { 
                return true;
            }
        }

        [HttpGet("GetMoviesByAverageUserRatings")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<IEnumerable<Movie>>> GetTopMovies()
        {

            var respose = await _movieRepository.getMoviesByUserAverageRatings();

            if (respose == null)
            {
                return NotFound();
            }

            return respose;
        }

        // GET: api/Moviesratings By specific users
        [HttpGet("GetMoviesByUser/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie(int id)
        {
            var movie = await _movieRepository.getMoviesBySpecificUserRating(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        //PUT: api/Movies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("UpdateUserRatings")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> PutMovie(UserRatings movieRating)
        {
            if ((movieRating.UserID == 0) &&( movieRating.Rating ==0) && (movieRating.MovieID == 0) )
            {
                return BadRequest();
            }


            try
            {
                await   _movieRepository.addMovieRatings(movieRating);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

   
      
         
    }
}
