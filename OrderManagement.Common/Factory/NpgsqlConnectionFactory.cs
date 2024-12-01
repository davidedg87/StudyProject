using Microsoft.Extensions.Configuration;
using Npgsql;
using Polly;
using System.Data;

namespace OrderManagement.Common.Factory
{
    public class OpenNpgsqlConnectionFactory
    {
        private readonly string _connectionString;

        public OpenNpgsqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
        }

        public IDbConnection CreateOpenConnection()
        {
            // Definisci la politica di retry

            /*Handle<NpgsqlException>(ex => ex.IsTransient): Specifica che la politica si applica solo alle eccezioni di tipo NpgsqlException che sono considerate transitorie, ossia errori temporanei che potrebbero essere risolti semplicemente riprovando (ad esempio, problemi di rete o timeout). L'uso di ex.IsTransient permette di distinguere tra errori transitori e quelli definitivi, come errori di configurazione o database non raggiungibile.
             * WaitAndRetry(5, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))): Indica che la connessione verrà riprovata fino a 5 volte. L'attesa tra i tentativi è calcolata con un backoff esponenziale: il primo tentativo avviene dopo 2^1 (2 secondi), il secondo dopo 2^2 (4 secondi), e così via. Questo tipo di strategia è utile per ridurre il carico sui sistemi nel caso in cui il problema sia temporaneo e per evitare di sovraccaricare il server con richieste ripetute troppo ravvicinate.
             */
            var retryPolicy = Policy
                .Handle<NpgsqlException>(ex => ex.IsTransient)
                .WaitAndRetry(5, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));



            // Definisci la politica di circuit breaker

            /*Handle<NpgsqlException>(): La politica gestisce tutte le eccezioni di tipo NpgsqlException, che sono quelle sollevate dal client PostgreSQL (ad esempio, quando si verifica un errore durante l'interazione con il database).

            CircuitBreaker(3, TimeSpan.FromMinutes(1)): Il "circuit breaker" è progettato per proteggere il sistema da ulteriori fallimenti se il database continua a restituire errori. Se vengono rilevati 3 errori consecutivi, il "circuito" si apre, il che significa che il sistema smette di tentare di connettersi per un periodo di tempo specificato (in questo caso, 1 minuto). Dopo il periodo di "raffreddamento", il sistema torna a provare a connettersi e a eseguire operazioni.
               
             */
            var circuitBreakerPolicy = Policy
                .Handle<NpgsqlException>()
                .CircuitBreaker(3, TimeSpan.FromMinutes(1)); // Dopo 3 errori, entra in modalità aperta per 1 minuto

            // Combina le politiche: retry prima, circuit breaker dopo
            var policyWrap = Policy.Wrap(retryPolicy, circuitBreakerPolicy);


            var connection = new NpgsqlConnection(_connectionString);

            // Applica la politica di resilienza
            policyWrap.Execute(() =>
            {
                connection.Open();
            });

            return connection;
        }
    }

   
}
