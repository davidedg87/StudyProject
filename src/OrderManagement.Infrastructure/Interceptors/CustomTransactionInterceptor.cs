using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace OrderManagement.Infrastructure.Interceptors
{
    public  class CustomTransactionInterceptor : DbTransactionInterceptor
    {

        public override ValueTask<DbTransaction> TransactionStartedAsync(DbConnection connection, TransactionEndEventData eventData, DbTransaction result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Transaction started");
            return base.TransactionStartedAsync(connection, eventData, result, cancellationToken);
        }

        public override Task TransactionCommittedAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Transaction committed");
            return base.TransactionCommittedAsync(transaction, eventData, cancellationToken);
        }

        public override Task TransactionFailedAsync(DbTransaction transaction, TransactionErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Transaction failed");
            return base.TransactionFailedAsync(transaction, eventData, cancellationToken);
        }

        public override Task TransactionRolledBackAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Transaction rolled back");
            return base.TransactionRolledBackAsync(transaction, eventData, cancellationToken);
        }
    }
}
