using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Core.Entities
{
    public class OrderItem : BaseEntity, ISoftDeletable
    {
        public int Quantity { get;  set; } // Quantità dell'articolo
        public decimal PricePerUnit { get;  set; } // Prezzo per unità
        public decimal TotalPrice => Quantity * PricePerUnit; // Prezzo totale calcolato
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        #region Navigation Properties

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }

        [Required]
        public int InventoryItemId { get; set; }

        [ForeignKey(nameof(InventoryItemId))]
        public InventoryItem? InventoryItem { get; set; }

        #endregion


    }

}
