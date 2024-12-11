using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Infrastructure.Data.DbContext;

namespace OrderManagement.Application.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            // Configura i contesti utilizzando il metodo generico
            AddDbContext<AppDbContext>(services, configuration);
        }

        private static void AddDbContext<TContext>(IServiceCollection services, IConfiguration configuration)
       where TContext : DbContext
        {

            // Aggiunge il DbContext per l'applicazione, configurando la connessione al database PostgreSQL.
            // La connessione al database viene effettuata tramite la stringa di connessione definita nel file di configurazione,
            // e viene applicata una politica di retry in caso di errori di connessione temporanei.

            // Il metodo `EnableRetryOnFailure` configura il comportamento di retry per le operazioni che falliscono temporaneamente.
            // - Il primo parametro (5) indica il numero massimo di tentativi di retry prima che l'operazione fallisca definitivamente.
            // - Il secondo parametro (TimeSpan.FromSeconds(10)) definisce l'intervallo di attesa tra un tentativo di retry e il successivo.
            // - Il terzo parametro è impostato su null, il che significa che non vengono definiti errori specifici per i quali eseguire il retry. 
            //   Il retry verrà eseguito su qualsiasi eccezione che soddisfi la condizione di errore temporaneo di Npgsql.

            services.AddDbContext<TContext>((serviceProvider, options) =>
                options.UseNpgsql(
                    configuration["ConnectionStrings:DefaultConnection"],
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null
                    )
                )
            );
        }

        public static void ApplyMigrations(this WebApplication app)
        {
            // Configura i contesti utilizzando il metodo generico
            app.ApplyMigrations<AppDbContext>();
        }


        public static void ApplyMigrations<TDbContext>(this WebApplication app) where TDbContext : DbContext
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                dbContext.Database.Migrate(); // Applica tutte le migration pendenti
            }
        }


    }

}
