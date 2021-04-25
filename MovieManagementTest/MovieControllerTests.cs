using Microsoft.EntityFrameworkCore;
using MovieManagement.Controllers;
using MovieManagementDataAccess.Models;
using System;
using Xunit;

namespace MovieManagementTest
{
    public class MovieControllerTests
    {

        private MovieContext _context;
        private MoviesController _movieController;
        public   MovieControllerTests()
        {
            var option = new DbContextOptionsBuilder<MovieContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;
            _context = new MovieContext(option);
        }

    }
}
