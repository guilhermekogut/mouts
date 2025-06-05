using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for creating a new sale based on a cart.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSaleCommand> _validator;
        private readonly IMediator _mediator;

        public CreateSaleHandler(
            ICartRepository cartRepository,
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IValidator<CreateSaleCommand> validator,
            IUserRepository userRepository,
             ILogger<CreateSaleHandler> logger,
             IMediator mediator
            )
        {
            _cartRepository = cartRepository;
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 2. Get cart
            var cart = await _cartRepository.GetByIdAsync(request.CartId, cancellationToken);
            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            // 3. Get user and check cart ownership
            var user = await _userRepository.GetByIdAsync(cart.UserId, cancellationToken);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (user.Id != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException("Cart does not belong to the user.");


            // 4. Get products and validate existence
            var productIds = cart.Products.Select(p => p.ProductId).Distinct().ToArray();
            var existenceResult = await _productRepository.CheckExistenceAsync(productIds, cancellationToken);
            if (!existenceResult.AllExist)
            {
                var notFoundIds = existenceResult.Items.Where(x => !x.Exists).Select(x => x.ProductId);
                throw new InvalidOperationException($"The following product IDs do not exist: {string.Join(", ", notFoundIds)}");
            }

            // 5. Get product details for pricing and names
            var products = _productRepository.Query().Where(p => productIds.Contains(p.Id)).ToList();

            // 6. Build Sale and SaleItems
            var saleItems = new List<SaleItem>();
            foreach (var cartProduct in cart.Products)
            {
                var product = products.First(p => p.Id == cartProduct.ProductId);
                var unitPrice = product.Price;
                var discount = Sale.GetDiscountAmount(unitPrice, cartProduct.Quantity);
                var total = (unitPrice * cartProduct.Quantity) - discount;

                saleItems.Add(new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ProductName = product.Title,
                    Quantity = cartProduct.Quantity,
                    UnitPrice = unitPrice,
                    Discount = discount,
                    Total = total,
                    Cancelled = false
                });
            }

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                CustomerId = cart.UserId,
                CustomerName = string.Concat(user.Name.Firstname, " ", user.Name.Lastname),
                TotalAmount = saleItems.Sum(i => i.Total),
                Cancelled = false,
                Items = saleItems
            };

            sale.Validate();

            // 7. Persist Sale
            await _saleRepository.AddAsync(sale, cancellationToken);

            // 8. Remove Cart
            await _cartRepository.DeleteAsync(cart.Id, cancellationToken);

            // 9. Create Event of Sale Created
            await _mediator.Publish(new SaleCreatedEvent(sale), cancellationToken);

            // 10. Map result
            var result = _mapper.Map<CreateSaleResult>(sale);
            return result;
        }
    }
}