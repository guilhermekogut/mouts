using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

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
        private readonly IValidator<CreateCartCommand> _validator;

        public CreateCartHandler(ICartRepository cartRepository, IMapper mapper, IProductRepository productRepository, IValidator<CreateCartCommand> validator)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _validator = validator;
        }

        public async Task<CreateCartResult> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            // Pipeline validation
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

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
            // Maps the command to the Cart entity
            var cart = _mapper.Map<Cart>(request);

            cart.Validate();

            // Persistence
            await _cartRepository.AddAsync(cart, cancellationToken);

            // Maps the Cart entity to the result
            var result = _mapper.Map<CreateCartResult>(cart);
            return result;
        }
    }
}