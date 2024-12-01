using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderManagement.Infrastructure.Data.DbContext;
using OrderManagement.Infrastructure.Interceptors;

namespace OrderManagement.Application.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Aggiunge il DbContext per l'applicazione, configurando la connessione al database PostgreSQL.
            // La connessione al database viene effettuata tramite la stringa di connessione definita nel file di configurazione,
            // e viene applicata una politica di retry in caso di errori di connessione temporanei.

            // Il metodo `EnableRetryOnFailure` configura il comportamento di retry per le operazioni che falliscono temporaneamente.
            // - Il primo parametro (5) indica il numero massimo di tentativi di retry prima che l'operazione fallisca definitivamente.
            // - Il secondo parametro (TimeSpan.FromSeconds(10)) definisce l'intervallo di attesa tra un tentativo di retry e il successivo.
            // - Il terzo parametro è impostato su null, il che significa che non vengono definiti errori specifici per i quali eseguire il retry. 
            //   Il retry verrà eseguito su qualsiasi eccezione che soddisfi la condizione di errore temporaneo di Npgsql.

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>

                options
                .UseNpgsql(configuration["ConnectionStrings:DefaultConnection"],
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(0, TimeSpan.FromSeconds(10), null)));
        }


    }
}
