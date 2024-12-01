namespace OrderManagement.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime UpdateDate { get; set; } = DateTime.UtcNow; // Imposta la data di aggiornamento a UTC
    }


    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }


}
