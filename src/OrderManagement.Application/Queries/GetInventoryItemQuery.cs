using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Queries
{

    public class GetInventoryItemQuery : IRequest<InventoryItemDto>
    {
        public int Id { get; set; }
    }
}
