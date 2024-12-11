namespace OrderManagement.Core.Entities
{
    public class OrderStatistics : BaseEntity, ISoftDeletable
    {
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

    }
}