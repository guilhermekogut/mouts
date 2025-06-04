using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for updating a sale by cancelling a sale item.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSaleCommand> _validator;
        private readonly IMediator _mediator;

        public UpdateSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            IValidator<UpdateSaleCommand> validator,
            IMediator mediator)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _validator = validator;
            _mediator = mediator;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 2. Get sale
            var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
            if (sale == null)
                throw new InvalidOperationException("Sale not found.");

            // 3. Check user ownership
            if (sale.CustomerId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException("Sale does not belong to the user.");

            // 4. Find the item to cancel
            var item = sale.Items.FirstOrDefault(i => i.ProductId == request.ProductId && !i.Cancelled);
            if (item == null)
                throw new InvalidOperationException("Sale item not found or already cancelled.");

            // 5. Cancel the item
            item.Cancelled = true;

            // 6. Recalculate total amount (only sum non-cancelled items)
            sale.TotalAmount = sale.Items.Where(i => !i.Cancelled).Sum(i => i.Total);

            // 7. If all items are cancelled, mark sale as cancelled
            if (sale.Items.All(i => i.Cancelled))
                sale.Cancelled = true;

            // 8. Validate business rules/specifications
            sale.Validate();

            // 9. Persist changes
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // 10. Publish event
            await _mediator.Publish(new SaleUpdatedEvent(sale), cancellationToken);

            // 11. Map to result
            var result = _mapper.Map<UpdateSaleResult>(sale);
            return result;
        }
    }
}