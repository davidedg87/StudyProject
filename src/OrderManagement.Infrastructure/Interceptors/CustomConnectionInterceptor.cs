using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace OrderManagement.Infrastructure.Interceptors
{
    public  class CustomConnectionInterceptor : DbConnectionInterceptor
    {
        public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Connection opened");
            return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }

        public override ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            Console.WriteLine("Connection closing");
            return base.ConnectionClosingAsync(connection, eventData, result);
        }

        public override ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Connection opening");
            return base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }

        public override Task ConnectionFailedAsync(DbConnection connection, ConnectionErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Connection failed");
            return base.ConnectionFailedAsync(connection, eventData, cancellationToken);
        }

        public override Task ConnectionClosedAsync(DbConnection connection, ConnectionEndEventData eventData)
        {
            Console.WriteLine("Connection closed");
            return base.ConnectionClosedAsync(connection, eventData);
        }

    }
}
