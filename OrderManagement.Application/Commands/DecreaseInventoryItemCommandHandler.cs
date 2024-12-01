namespace OrderManagement.Application.Commands
{
    using MediatR;
    using OrderManagement.Common.Models;
    using OrderManagement.Core.Interfaces;

    public class DecreaseInventoryItemCommandHandler : IRequestHandler<DecreaseInventoryItemCommand, Result<Unit>>
    {
        private readonly IInventoryRepository _repository;

        public DecreaseInventoryItemCommandHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(DecreaseInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);

            if (item == null)
                throw new KeyNotFoundException($"Item with Id {request.Id} not found");

            item.DecreaseQuantity(request.Quantity);
            await _repository.UpdateAsync(item);


            return  Result<Unit>.SuccessResultUnit();

        }


    }
}
