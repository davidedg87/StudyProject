using OrderManagement.Core.Enums;

namespace OrderManagement.Core.Entities
{
    public class Order : BaseEntity, ISoftDeletable
    {
        public DateTime OrderDate { get;  set; } // Data di creazione dell'ordine.
        public string? CustomerName { get;  set; } // Nome del cliente.
        public OrderStatus Status { get;  set; } // Stato dell'ordine.
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

    }

}
