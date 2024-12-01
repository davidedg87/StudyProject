using Dapper;
using MediatR;
using OrderManagement.Application.DTOs;
using System.Data;

namespace OrderManagement.Application.Queries
{
    public class GetInventoryItemQueryHandler : IRequestHandler<GetInventoryItemQuery, InventoryItemDto>
    {
        private readonly IDbConnection _dbConnection;

        public GetInventoryItemQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<InventoryItemDto> Handle(GetInventoryItemQuery request, CancellationToken cancellationToken)
        {
            string sql = "SELECT \"Id\", \"Name\", \"AvailableQuantity\", \"PricePerUnit\" FROM \"InventoryItems\" WHERE \"Id\" = @Id";

            var result = await _dbConnection.QueryFirstOrDefaultAsync<InventoryItemDto>(sql, new { request.Id });

            if (result == null)
                throw new KeyNotFoundException($"Item with Id {request.Id} not found");

            return result;

        }
    }
}
