namespace OrderManagement.Application.Commands
{
    using MediatR;
    using OrderManagement.Common.Models;
    using System.Text.Json.Serialization;

    public class UpdateInventoryItemCommand : IRequest<Result<Unit>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int AvailableQuantity { get; set; }
    }

}
