using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Handler for creating a new cart.
    /// </summary>
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateCartHandler(ICartRepository cartRepository, IMapper mapper, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<CreateCartResult> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            // Rule: A user can only have one active cart
            var existingCart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (existingCart != null)
                throw new InvalidOperationException("User already has an active cart.");

            var productIds = request.Products.Select(p => p.ProductId).Distinct().ToArray();
            var existenceResult = await _productRepository.CheckExistenceAsync(productIds, cancellationToken);
            if (!existenceResult.AllExist)
            {
                var notFoundIds = existenceResult.Items.Where(x => !x.Exists).Select(x => x.ProductId);
                throw new InvalidOperationException($"The following product IDs do not exist: {string.Join(", ", notFoundIds)}");
            }
            // Business rule validation (quantity per item)
            foreach (var item in request.Products)
            {
                if (item.Quantity < 1)
                    throw new InvalidOperationException($"The quantity of product {item.ProductId} must be greater than zero.");
                if (item.Quantity > 20)
                    throw new InvalidOperationException($"It is not allowed to sell more than 20 items of product {item.ProductId}.");
            }

            // Maps the command to the Cart entity
            var cart = _mapper.Map<Cart>(request);

            // Persistence
            await _cartRepository.AddAsync(cart, cancellationToken);

            // Maps the Cart entity to the result
            var result = _mapper.Map<CreateCartResult>(cart);
            return result;
        }
    }
}