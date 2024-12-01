namespace OrderManagement.Application.Commands
{
    using MediatR;
    using OrderManagement.Common.Models;
    using System.Text.Json.Serialization;

    public class DecreaseInventoryItemCommand : IRequest<Result<Unit>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
