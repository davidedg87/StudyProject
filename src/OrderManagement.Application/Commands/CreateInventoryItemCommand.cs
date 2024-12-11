namespace OrderManagement.Application.Commands
{
    using MediatR;
    using OrderManagement.Common.Models;

    public class CreateInventoryItemCommand : IRequest<Result<int>>
    {
        public string? Name { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }

}
