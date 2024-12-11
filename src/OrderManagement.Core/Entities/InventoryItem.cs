namespace OrderManagement.Core.Entities
{
    public class InventoryItem : BaseEntity, ISoftDeletable
    {
        public required string Name { get; set; }
        public int AvailableQuantity { get; set; } // Quantità disponibile
        public decimal PricePerUnit { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        // Metodo per aggiornare la quantità disponibile
        public void UpdateQuantity(int quantity)
        {
            AvailableQuantity = quantity;
        }

        // Metodo per diminuire la quantità (es. quando un ordine viene completato)
        public void DecreaseQuantity(int quantity)
        {
            if (AvailableQuantity >= quantity)
            {
                AvailableQuantity -= quantity;
            }
            else
            {
                throw new InvalidOperationException("Non c'è abbastanza quantità disponibile.");
            }
        }
    }

}
