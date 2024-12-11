using Microsoft.EntityFrameworkCore;
using Npgsql;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data.DbContext;
using System.Data.Common;
using System.Data;
using Testcontainers.PostgreSql;

namespace OrderManagement.Application.Test
{
    public class TestContainerPostgres : IAsyncLifetime
    {
        private AppDbContext? DbContext;

        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

        // Questo metodo viene chiamato prima di ogni test asincrono
        public async Task InitializeAsync()
        {
             await _postgreSqlContainer.StartAsync();

            var _connectionString = _postgreSqlContainer.GetConnectionString();

            var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseNpgsql(_connectionString)
           .Options;

            DbContext = new AppDbContext(options);

            await DbContext.Database.MigrateAsync();

        }

        // Questo metodo viene chiamato dopo ogni test per fermare il container
        public  async Task DisposeAsync()
        {
            await  _postgreSqlContainer.DisposeAsync().AsTask();
        }

        [Fact]
        public void ConnectionStateReturnsOpen()
        {
            // Given
            using DbConnection connection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString());

            // When
            connection.Open();

            // Then
            Assert.Equal(ConnectionState.Open, connection.State);
        }

        [Fact]
        public async Task ExecScriptReturnsSuccessful()
        {
            // Given
            const string scriptContent = "SELECT 1;";

            // When
            var execResult = await _postgreSqlContainer.ExecScriptAsync(scriptContent)
                .ConfigureAwait(true);

            // Then
            Assert.True(0L.Equals(execResult.ExitCode), execResult.Stderr);
            Assert.Empty(execResult.Stderr);
        }


        [Fact]
        public void Test_AddEntity_WithPostgresContainer()
        {

            var entity = new Order { CustomerName = "Test Entity" };
            DbContext!.Orders.Add(entity);
            DbContext!.SaveChanges();

            var savedEntity = DbContext!.Orders.FirstOrDefault(e => e.CustomerName == "Test Entity");
            Assert.NotNull(savedEntity);  // Verifica che l'entità sia stata salvata nel database

        }

    }
}