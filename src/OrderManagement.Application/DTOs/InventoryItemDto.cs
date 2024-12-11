namespace OrderManagement.Application.DTOs
{
    public class InventoryItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
