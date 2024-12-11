using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Extensions;
using OrderManagement.Infrastructure.Interceptors;
using Polly;

namespace OrderManagement.Infrastructure.Data.DbContext
{

    //COMANDI MIGRATION
    //Add-Migration InitialCreate -Context AppDbContext -OutputDir Data/Migrations  [Selezionando come progetto di default OrderManagement.Infrastructure]
    //Update-Database -Context AppDbContext

    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        #region Query Compilata

        public class OrderDtoContext
        {
            public DateTime OrderDate { get; set; }
            public int OrderId { get; set; }

        }

        private static Func<AppDbContext, int, IAsyncEnumerable<OrderDtoContext>> OrderListAsync =
        EF.CompileAsyncQuery((AppDbContext ctx, int orderId) =>
        ctx.Orders
        .Where(o => o.Id == orderId)
        .Include(o => o.OrderItems)
            .ThenInclude(o => o.InventoryItem)
        .Select(o => new OrderDtoContext
        {
            OrderId = o.Id,
            OrderDate = o.OrderDate
        }));

        public async Task<IEnumerable<OrderDtoContext>> GetAllData_Compiled_Async(int id)
        {
            var result = new List<OrderDtoContext>();
            await foreach (var s in OrderListAsync(this, id))
            {
                result.Add(s);
            }
            return result;
        }

        #endregion

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IInterceptor[] interceptors = 
            {
                new CustomCommandInterceptor(),
                new CustomSaveChangesInterceptor(),
                new CustomTransactionInterceptor(),
                new CustomConnectionInterceptor(),
                new SoftDeleteInterceptor()
            };

            optionsBuilder.AddInterceptors(interceptors);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<OrderStatistics> OrderStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione delle entità

            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            // Relazione tra Order e OrderItem: eliminazione in cascata
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Elimina gli OrderItem quando un Order è eliminato.

            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.Id);
            // Relazione tra OrderItem e InventoryItem: nessuna eliminazione in cascata
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.InventoryItem)
                .WithMany()
                .HasForeignKey(oi => oi.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict); // Genera errore se un InventoryItem è referenziato da OrderItem.

            modelBuilder.Entity<InventoryItem>().HasKey(ii => ii.Id);
            modelBuilder.Entity<OrderStatistics>().HasKey(ii => ii.Id);


            #region QueryFilters and Indexes

            modelBuilder.ApplyGlobalFiltersByInterface<ISoftDeletable>(e => !e.IsDeleted, nameof(ISoftDeletable.IsDeleted));

            #endregion



        }
    }
}
