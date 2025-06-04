using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<GetCartResult> Handle(GetCartCommand query, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(query.Id, cancellationToken);
            if (cart == null)
                throw new InvalidOperationException($"Cart with Id {query.Id} not found.");

            return _mapper.Map<GetCartResult>(cart);
        }
    }
}