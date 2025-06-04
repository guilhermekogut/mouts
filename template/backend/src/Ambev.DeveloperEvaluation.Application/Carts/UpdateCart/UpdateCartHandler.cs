using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Handler for updating an existing cart.
    /// </summary>
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCartCommand> _validator;

        public UpdateCartHandler(
            ICartRepository cartRepository,
            IMapper mapper,
            IValidator<UpdateCartCommand> validator)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UpdateCartResult> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            // Pipeline validation
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            // Rule: UserId cannot be changed
            if (cart.UserId != request.UserId)
                throw new InvalidOperationException("UserId cannot be changed.");

            cart.Validate();

            var incomingProductIds = request.Products.Select(p => p.ProductId).ToHashSet();


            foreach (var item in request.Products)
            {
                cart.AddOrUpdateProduct(item.ProductId, item.Quantity);
            }

            var productsToRemove = cart.Products
                                                .Where(p => !incomingProductIds.Contains(p.ProductId))
                                                .Select(p => p.ProductId)
                                                .ToList();

            foreach (var productId in productsToRemove)
            {
                cart.RemoveProduct(productId);
            }

            cart.Date = request.Date;

            await _cartRepository.UpdateAsync(cart, cancellationToken);

            var result = _mapper.Map<UpdateCartResult>(cart);
            return result;
        }
    }
}