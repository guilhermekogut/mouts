using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Handler for cancelling an entire sale.
    /// </summary>
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CancelSaleCommand> _validator;
        private readonly IMediator _mediator;

        public CancelSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            IValidator<CancelSaleCommand> validator,
            IMediator mediator)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _validator = validator;
            _mediator = mediator;
        }

        public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
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

            // 4. Cancel all items and the sale
            foreach (var item in sale.Items)
                item.Cancelled = true;

            sale.Cancelled = true;
            sale.TotalAmount = 0m;

            // 5. Validate business rules/specifications
            sale.Validate();

            // 6. Persist changes
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // 7. Publish event
            await _mediator.Publish(new SaleCancelledEvent(sale), cancellationToken);

            // 8. Map to result
            var result = _mapper.Map<CancelSaleResult>(sale);
            return result;
        }
    }
}