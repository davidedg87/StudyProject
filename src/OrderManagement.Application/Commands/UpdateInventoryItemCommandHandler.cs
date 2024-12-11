using MediatR;
using OrderManagement.Common.Models;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Application.Commands
{
    public class UpdateInventoryItemCommandHandler : IRequestHandler<UpdateInventoryItemCommand, Result<Unit>>
    {
        private readonly IInventoryRepository _repository;

        public UpdateInventoryItemCommandHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(UpdateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);

            if (item == null)
                throw new KeyNotFoundException($"Item with Id {request.Id} not found");

            item.UpdateQuantity(request.AvailableQuantity);
            await _repository.UpdateAsync(item);

            return Result<Unit>.SuccessResultUnit();

        }

      
    }

}
