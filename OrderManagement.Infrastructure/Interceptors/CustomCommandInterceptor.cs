using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace OrderManagement.Infrastructure.Interceptors
{
    public  class CustomCommandInterceptor : DbCommandInterceptor
    {


        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Reader executing");
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("NonQuery executing");
            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Scalar executing");
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
