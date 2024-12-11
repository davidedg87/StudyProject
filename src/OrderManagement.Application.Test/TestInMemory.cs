using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data.DbContext;

namespace OrderManagement.Application.Test
{
    public class TestInMemory
    {
        private DbContextOptions<AppDbContext> _dbContextOptions;

        // Questo metodo viene chiamato una sola volta prima di ogni test
        public TestInMemory()
        {
            // Configura il contesto per usare il database in memoria
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
        }

        [Fact]
        public void Test_AddEntity_WithInMemoryDb()
        {
            // Arrange
            using var dbContext = new AppDbContext(_dbContextOptions);
            var entity = new Order { CustomerName = "Test Entity" };

            // Act
            dbContext.Orders.Add(entity);
            dbContext.SaveChanges();

            // Assert
            var savedEntity = dbContext.Orders.FirstOrDefault(e => e.CustomerName == "Test Entity");
            Assert.NotNull(savedEntity);

        }

    }
}