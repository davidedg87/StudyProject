using Microsoft.EntityFrameworkCore.Diagnostics;

namespace OrderManagement.Infrastructure.Interceptors
{
    public class CustomSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Saving changes");
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Saved changes");
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Save changes failed");
            return base.SaveChangesFailedAsync(eventData, cancellationToken);
        }

    }
}
