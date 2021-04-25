using MovieManagementDataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieManagementDataAccess.IRepositories
{
    public interface IMovieRepository
    {
        Task<List<Movie>> getMoviesAsync(FilterCriteria filterCriteria);
        Task<List<Movie>> getMoviesByUserAverageRatings();
        Task<List<Movie>> getMoviesBySpecificUserRating(int userId);

        Task<bool> addMovieRatings(UserRatings movieRatings);

    }
}
