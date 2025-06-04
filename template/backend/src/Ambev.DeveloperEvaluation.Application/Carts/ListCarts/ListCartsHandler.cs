using System.Linq.Dynamic.Core;

using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public class ListCartsHandler : IRequestHandler<ListCartsCommand, ListCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public ListCartsHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public Task<ListCartsResult> Handle(ListCartsCommand query, CancellationToken cancellationToken)
    {
        var cartsQuery = _cartRepository.Query();

        // Filtering
        if (query.UserId.HasValue)
            cartsQuery = cartsQuery.Where(c => c.UserId == query.UserId.Value);

        if (query.Date.HasValue)
            cartsQuery = cartsQuery.Where(c => c.Date.Date == query.Date.Value.Date);

        if (query.MinDate.HasValue)
            cartsQuery = cartsQuery.Where(c => c.Date >= query.MinDate.Value);

        if (query.MaxDate.HasValue)
            cartsQuery = cartsQuery.Where(c => c.Date <= query.MaxDate.Value);

        // Ordering (dynamic)
        if (!string.IsNullOrWhiteSpace(query.Order))
        {
            cartsQuery = OrderByDynamic(cartsQuery, query.Order);
        }
        else
        {
            cartsQuery = cartsQuery.OrderBy(c => c.Date);
        }

        var totalItems = cartsQuery.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.Size);
        var items = cartsQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToList();

        var result = new ListCartsResult
        {
            Data = _mapper.Map<IEnumerable<ListCartItemResult>>(items),
            TotalItems = totalItems,
            CurrentPage = query.Page,
            TotalPages = totalPages
        };

        return Task.FromResult(result);
    }

    // Helper method for dynamic ordering
    private static IQueryable<Domain.Entities.Cart> OrderByDynamic(IQueryable<Domain.Entities.Cart> query, string order)
    {
        var cleanedOrder = string.Join(",",
            order.Split(',')
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
        );
        return query.OrderBy(cleanedOrder);
    }
}