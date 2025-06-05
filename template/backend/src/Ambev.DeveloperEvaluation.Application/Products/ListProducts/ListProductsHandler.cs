using System.Linq.Dynamic.Core;

using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : IRequestHandler<ListProductsCommand, ListProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ListProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public Task<ListProductsResult> Handle(ListProductsCommand query, CancellationToken cancellationToken)
    {
        var productsQuery = _productRepository.Query();

        // Filtering for all attributes
        if (!string.IsNullOrWhiteSpace(query.Title))
            productsQuery = ApplyStringFilter(productsQuery, "Title", query.Title);

        if (query.Price.HasValue)
            productsQuery = productsQuery.Where(p => p.Price == query.Price.Value);

        if (query.MinPrice.HasValue)
            productsQuery = productsQuery.Where(p => p.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            productsQuery = productsQuery.Where(p => p.Price <= query.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(query.Description))
            productsQuery = ApplyStringFilter(productsQuery, "Description", query.Description);

        if (!string.IsNullOrWhiteSpace(query.Category))
            productsQuery = ApplyStringFilter(productsQuery, "Category", query.Category);

        if (!string.IsNullOrWhiteSpace(query.Image))
            productsQuery = ApplyStringFilter(productsQuery, "Image", query.Image);

        if (query.RatingRate.HasValue)
            productsQuery = productsQuery.Where(p => p.Rating.Rate == query.RatingRate.Value);

        if (query.MinRatingRate.HasValue)
            productsQuery = productsQuery.Where(p => p.Rating.Rate >= query.MinRatingRate.Value);

        if (query.MaxRatingRate.HasValue)
            productsQuery = productsQuery.Where(p => p.Rating.Rate <= query.MaxRatingRate.Value);

        if (query.RatingCount.HasValue)
            productsQuery = productsQuery.Where(p => p.Rating.Count == query.RatingCount.Value);

        if (query.MinRatingCount.HasValue)
            productsQuery = productsQuery.Where(p => p.Rating.Count >= query.MinRatingCount.Value);

        if (query.MaxRatingCount.HasValue)
            productsQuery = productsQuery.Where(p => p.Rating.Count <= query.MaxRatingCount.Value);

        // Ordering (dynamic)
        if (!string.IsNullOrWhiteSpace(query.Order))
        {
            productsQuery = OrderByDynamic(productsQuery, query.Order);
        }
        else
        {
            productsQuery = productsQuery.OrderBy(p => p.Title);
        }

        var totalItems = productsQuery.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.Size);
        var items = productsQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToList();

        var result = new ListProductsResult
        {
            Data = _mapper.Map<IEnumerable<ProductItemResult>>(items),
            TotalItems = totalItems,
            CurrentPage = query.Page,
            TotalPages = totalPages
        };

        return Task.FromResult(result);
    }

    // Helper method for dynamic ordering
    private static IQueryable<Domain.Entities.Product> OrderByDynamic(IQueryable<Domain.Entities.Product> query, string order)
    {
        var cleanedOrder = string.Join(",",
            order.Split(',')
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
        );
        return query.OrderBy(cleanedOrder);
    }

    // Helper method for string filtering with wildcards
    private static IQueryable<Domain.Entities.Product> ApplyStringFilter(IQueryable<Domain.Entities.Product> query, string property, string value)
    {
        if (value.StartsWith("*") && value.EndsWith("*"))
            return query.Where($"{property}.Contains(@0)", value.Trim('*'));
        if (value.StartsWith("*"))
            return query.Where($"{property}.EndsWith(@0)", value.TrimStart('*'));
        if (value.EndsWith("*"))
            return query.Where($"{property}.StartsWith(@0)", value.TrimEnd('*'));
        return query.Where($"{property} == @0", value);
    }
}