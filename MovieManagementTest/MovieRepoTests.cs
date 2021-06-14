using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManagement.Controllers;
using MovieManagementDataAccess.IRepositories;
using MovieManagementDataAccess.Models;
using MovieManagementDataAccess.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace MovieManagementTest
{
    public class MovieRepoTests
    {

        private MovieContext _context;
        private MoviesController _movieController;
        private IMovieRepository _movieRepository;
        public   MovieRepoTests()
        {
            var option = new DbContextOptionsBuilder<MovieContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;
            _context = new MovieContext(option);

 
        }
        [Fact]
        public async void Getall_Movies_list_Test()
        {
            //Arrange
            _movieRepository = new MovieRepository(_context);
        
            //Act
            var data = await _movieRepository.getMoviesByUserAverageRatings();

            //Assert
            Assert.Equal(5, data.Count);
            Assert.NotNull(data);

        }

    }
}
