using Microsoft.EntityFrameworkCore;
using MovieManagementDataAccess.IRepositories;
using MovieManagementDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagementDataAccess.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private   MovieContext _context;
        
        public MovieRepository(MovieContext movieContext) 
        {
            _context = movieContext;
            genarateMovieContext();
            genarateUserContext();
            genarateRatingsContext();
        }
        public async Task<bool> addMovieRatings(UserRatings movieRating)
        { 

            _context.Entry(movieRating).State = EntityState.Modified;

            try
            {
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movieRating.MovieID))
                {
                    return false;
                }
                else
                {

                    throw;
                }
                 
            }

            return true;
        }

        public async Task<List<Movie>> getMoviesAsync(FilterCriteria filterCriteria)
        {
            var response = await (from movies in _context.Movies
                                  let rating = getrating(movies.id)
                                 // join ur in _context.UserRatings.Where(u=>u.MovieID == movies.id)
                                  where (movies.title.ToUpper().Contains(filterCriteria.title.ToUpper())
                                  || movies.yearOfRelease == filterCriteria.yearOfRelease
                                  || movies.genres.ToUpper() ==(filterCriteria.genres.ToUpper()))
                                  select new Movie
                                  {
                                      id=movies.id,
                                      title=movies.title,
                                      yearOfRelease=movies.yearOfRelease,//
                                      genres=movies.genres,
                                      runningTime = movies.runningTime,
                                      UsereRatings = rating
                                  }).ToListAsync();
            


            return response;
             
        }

        private double getrating(int id)
        {
            var ratings = (from rl in _context.UserRatings
                           where rl.MovieID == id
                           select rl.Rating).ToList();
            double average = ratings.Count > 0 ? ratings.Average() : 0.0;

            return Math.Round(average);


        }
        private async void updateContextRatingsByuser(int UserID)
        {
            foreach (var movie in _context.Movies)
            {
                movie.UsereRatings = getUserRatings(UserID,movie.id);
                _context.Entry(movie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            
   
        }
        private double getUserRatings(int userId,int movieId)
        {
            return (from ur in _context.UserRatings
                             where ur.MovieID==movieId && ur.UserID==userId
                             select ur.Rating).Single();

        }
        public async Task<List<Movie>> getMoviesBySpecificUserRating(int UserId)
        {
            updateContextRatingsByuser(UserId);
            return await _context.Movies.OrderByDescending(x => x.UsereRatings).ThenBy(x => x.title).ToListAsync();

        }

        public async Task<List<Movie>> getMoviesByUserAverageRatings()
        {
            await UpdateAverageUserRatings();
            return await _context.Movies.OrderByDescending(x => x.UsereRatings).ThenBy(x => x.title).ToListAsync();

        }

        private async Task UpdateAverageUserRatings()
        {
                foreach (var movie in _context.Movies)
                {
                    movie.UsereRatings = getrating(movie.id);
                    _context.Entry(movie).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
        }

        private void genarateRatingsContext()
        {
            if(!_context.UserRatings.Any())
            {
                List<UserRatings> userRatings = new List<UserRatings> 
                {   new UserRatings { id=1,Rating =2,MovieID=1,UserID=1 } ,
                    new UserRatings { id=2, Rating =2,MovieID=2,UserID=1 },
                    new UserRatings { id=3, Rating =2,MovieID=3,UserID=1 },
                    new UserRatings { id=4, Rating =2,MovieID=4,UserID=1 },
                    new UserRatings { id=5,Rating =2,MovieID=5,UserID=1 },

                    new UserRatings { id=6, Rating =3,MovieID=1,UserID=2 },
                    new UserRatings { id=7,Rating =3,MovieID=2,UserID=2 },
                    new UserRatings { id=8,Rating =2,MovieID=3,UserID=2 },
                    new UserRatings { id=9,Rating =4,MovieID=4,UserID=2 },
                    new UserRatings {id=16, Rating =2,MovieID=5,UserID=2 },

                    new UserRatings { id=61,Rating =4,MovieID=1,UserID=3 },
                    new UserRatings { id=62,Rating =2,MovieID=2,UserID=3 },
                    new UserRatings { id=63,Rating =5,MovieID=3,UserID=3 },
                    new UserRatings { id=64,Rating =2,MovieID=4,UserID=3 },
                    new UserRatings { id=65,Rating =3,MovieID=5,UserID=3 }};
                _context.AddRange(userRatings);
                _context.SaveChanges();

            }

        }
        private void genarateMovieContext()
        {
            if (!_context.Movies.Any())
            {
                List<Movie> movieList = new List<Movie> {
                 new Movie {genres="comedy",id=1,runningTime=2.45,title="Comedy 1",yearOfRelease=2021 } ,
                 new Movie {genres="horrer",id=2,runningTime=2.45,title="HC 1",yearOfRelease=2020 },
                 new Movie {genres="comedy",id=3,runningTime=2.45,title="Comedy 2",yearOfRelease=2001 } ,
                 new Movie {genres="horrer",id=4,runningTime=2.45,title="HDC 1",yearOfRelease=2020 },
                 new Movie {genres="thriller",id=5,runningTime=2.45,title="TC 1",yearOfRelease=2000 } ,
                };
            
                _context.AddRange(movieList);
                _context.SaveChanges();
            }
            

        }
 
        private void genarateUserContext()
        {
            if (!_context.Users.Any())
            {
                List<Users> users = new List<Users> { new Users {Adddress="London",Email="xyz2abc.com",Name="Ram" ,id=4},
            new Users {Adddress="London2",Email="xyz2abc.com",Name="Syam" ,id=1},
            new Users {Adddress="London3",Email="xyz2abc.com",Name="Raghu", id=2},
            new Users {Adddress="London4",Email="xyz2abc.com",Name="Ramu",id=3 }};
                _context.AddRange(users);

                _context.SaveChanges();
            }
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.id == id);
        }
    }
}
