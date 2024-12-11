using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Services;
using Polly;
using Polly.Extensions.Http;

namespace OrderManagement.Application.Extensions
{
    public static class HttpExtensions
    {
        public static void AddHttpExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            /*
             * Usare Retry e Circuit Breaker insieme offre una protezione resiliente completa:
            Retry gestisce i fallimenti temporanei, tentando di ripetere l'operazione senza interruzioni.
            Circuit Breaker interviene quando ci sono troppi fallimenti consecutivi, "aprendo il circuito" per evitare di sovraccaricare il sistema e permettendo il recupero.
            Insieme, garantiscono che il sistema tenti di recuperare dai problemi temporanei, ma non continui a fare richieste quando il sistema è chiaramente in uno stato di errore prolungato.
             * 
             */


            // Configura una politica di retry con Polly per gestire errori HTTP transitori
            // come errori di server (5xx) e timeout (408).
            // - HandleTransientHttpError(): intercetta errori transitori (ad esempio, 5xx o 408).
            // - WaitAndRetryAsync(): riprova la richiesta per un massimo di 3 tentativi con un backoff esponenziale.
            // - Il backoff esponenziale aumenta il tempo di attesa tra un tentativo e l'altro, raddoppiando il tempo di attesa ad ogni tentativo.
            // - onRetry: callback che viene invocato ogni volta che un tentativo fallisce, stampando il numero di tentativo
            //   e il tempo di attesa prima di riprovare.
            Polly.Retry.AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError() // Gestisce errori transitori come 5xx o 408
                .WaitAndRetryAsync(
                    retryCount: 3, // Numero di retry
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Esponenziale
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        Console.WriteLine($"Tentativo {retryAttempt} fallito. Riprovo tra {timespan.TotalSeconds} secondi.");
                    });


            // Definizione della politica di Circuit Breaker per gestire gli errori transitori nelle richieste HTTP
            Polly.CircuitBreaker.AsyncCircuitBreakerPolicy<HttpResponseMessage> circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()  // Gestisce errori transitori, come 5xx (errore del server) o 408 (timeout della richiesta)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,  // Il circuito si apre dopo 3 errori consecutivi (per evitare troppi fallimenti)
                    durationOfBreak: TimeSpan.FromSeconds(30),  // Dopo che il circuito si è aperto, rimarrà aperto per 30 secondi
                    onBreak: (exception, timespan) =>  // Evento che si attiva quando il circuito si apre
                    {
                        Console.WriteLine($"Circuito aperto per {timespan.TotalSeconds} secondi a causa di un errore.");
                    },
                    onReset: () =>  // Evento che si attiva quando il circuito viene ripristinato (chiuso) dopo il periodo di break
                    {
                        Console.WriteLine("Circuito chiuso, sistema operativo.");
                    },
                    onHalfOpen: () =>  // Evento che si attiva quando il circuito è "mezzo aperto", pronto a testare se il sistema si è ripreso
                    {
                        Console.WriteLine("Circuito mezzo aperto, testando il sistema.");
                    });

            // Combinazione delle politiche Retry e Circuit Breaker
            var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

            // Configura il client HTTP per il servizio 'ITestHttpService' con una base URL specificata.
            // - AddHttpClient<ITestHttpService, TestHttpService>: Aggiunge un'istanza di HttpClient per il servizio 'ITestHttpService',
            //   utilizzando 'TestHttpService' come implementazione concreta.
            // - o.BaseAddress: Imposta l'indirizzo base del client HTTP a "HttpTestUri" in configuration
            // - AddPolicyHandler(retryPolicy): Applica la politica di retry definita da Polly per gestire i tentativi di connessione
            //   in caso di errori transitori (ad esempio, timeout o errori del server).
            //   Questa politica verrà applicata ad ogni richiesta HTTP inviata dal client.
            //QUESTA MODALITA' AL MOMENTO NON FUNZIONA SE IL SERVIZIO E' KEYED. IN TAL CASO SI DEVE USARE QUALCOSA COME QUESTO OVVERO REGITRARE IL CLIENT HTTP MANUALMENTE
            /*
             // Registrazione keyed per un servizio con HttpClient
                services.AddKeyedService<string, MyHttpClientService>("ServiceKey1", (provider, key) =>
                {
                    var factory = provider.GetRequiredService<IHttpClientFactory>();
                    var httpClient = factory.CreateClient();
                    httpClient.BaseAddress = new Uri("https://api.example.com/");
                    return new MyHttpClientService(httpClient);
                });
             
             */
            services.AddHttpClient<ITestHttpServiceTest, TestHttpService>(o =>

            {
                o.BaseAddress = new Uri(configuration["HttpTestUri"]!);
            })
            .AddPolicyHandler(policyWrap);

            services.AddHttpClient("TestHttpService3Client", client =>
            {
                client.BaseAddress = new Uri(configuration["HttpTestUri"]!);
            });

        }
    }
}
