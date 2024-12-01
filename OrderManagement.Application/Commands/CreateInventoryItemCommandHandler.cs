using MediatR;
using OrderManagement.Common.Models;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Commands
{
    public class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, Result<int>>
    {
        private readonly IInventoryRepository _repository;

        public CreateInventoryItemCommandHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var item = new InventoryItem
            {
                Name = request.Name ?? string.Empty,
                AvailableQuantity = request.AvailableQuantity,
                PricePerUnit = request.PricePerUnit
            };

            await _repository.AddAsync(item);
            return  Result<int>.SuccessResult(item.Id);

        }
    }

}
