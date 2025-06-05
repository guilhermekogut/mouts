using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<GetSaleResult> Handle(GetSaleCommand query, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(query.Id, cancellationToken);
            if (sale == null)
                throw new InvalidOperationException($"Sale with Id {query.Id} not found.");

            return _mapper.Map<GetSaleResult>(sale);
        }
    }
}