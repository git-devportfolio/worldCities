using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCities.Server.Controllers;
using WorldCities.Server.Data;
using WorldCities.Server.Data.Models;

namespace WorldCities.Server.Tests
{
    public class CitiesController_Test
    {
        /// <summary>
        /// Test the GetCity() method
        /// </summary>
        [Fact]
        public async Task GetCity()
        {
            // Arrange
            // todo: define the required assets
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "WorldCities")
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Add(new City()
            {
                Id = 1,
                CountryId = 1,
                Lat = 1,
                Lon = 1,
                Name = "TestCity1"
            });
            context.SaveChanges();

            // Act
            // todo: invoke the test
            var controller = new CitiesController(context);
            City? city_existing = null;
            City? city_notexisting = null;
            city_existing = (await controller.GetCity(1)).Value;
            city_notexisting = (await controller.GetCity(2)).Value;

            // Assert
            // todo: verify that conditions are met.
            Assert.NotNull(city_existing);
            Assert.Null(city_notexisting);
        }
    }
}
