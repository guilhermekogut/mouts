using System.Linq.Dynamic.Core;

using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public Task<ListSalesResult> Handle(ListSalesCommand query, CancellationToken cancellationToken)
        {
            var salesQuery = _saleRepository.Query();

            // Filtering
            if (query.CustomerId.HasValue)
                salesQuery = salesQuery.Where(s => s.CustomerId == query.CustomerId.Value);

            if (query.Date.HasValue)
                salesQuery = salesQuery.Where(s => s.Date.Date == query.Date.Value.Date);

            if (query.MinDate.HasValue)
                salesQuery = salesQuery.Where(s => s.Date >= query.MinDate.Value);

            if (query.MaxDate.HasValue)
                salesQuery = salesQuery.Where(s => s.Date <= query.MaxDate.Value);

            if (query.Cancelled.HasValue)
                salesQuery = salesQuery.Where(s => s.Cancelled == query.Cancelled.Value);

            // Ordering (dynamic)
            if (!string.IsNullOrWhiteSpace(query.Order))
            {
                salesQuery = OrderByDynamic(salesQuery, query.Order);
            }
            else
            {
                salesQuery = salesQuery.OrderByDescending(s => s.Date);
            }

            var totalItems = salesQuery.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)query.Size);
            var items = salesQuery
                .Skip((query.Page - 1) * query.Size)
                .Take(query.Size)
                .ToList();

            var result = new ListSalesResult
            {
                Data = _mapper.Map<IEnumerable<ListSaleItemResult>>(items),
                TotalItems = totalItems,
                CurrentPage = query.Page,
                TotalPages = totalPages
            };

            return Task.FromResult(result);
        }

        // Helper method for dynamic ordering
        private static IQueryable<Sale> OrderByDynamic(IQueryable<Sale> query, string order)
        {
            var cleanedOrder = string.Join(",",
                order.Split(',')
                    .Select(part => part.Trim())
                    .Where(part => !string.IsNullOrWhiteSpace(part))
            );
            return query.OrderBy(cleanedOrder);
        }
    }
}