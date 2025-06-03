using Ambev.DeveloperEvaluation.Domain.Repositories;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListCategories;

public class ListCategoriesHandler : IRequestHandler<ListCategoriesCommand, ListCategoriesResult>
{
    private readonly IProductRepository _productRepository;

    public ListCategoriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ListCategoriesResult> Handle(ListCategoriesCommand query, CancellationToken cancellationToken)
    {
        var categories = await _productRepository.GetCategoriesAsync(cancellationToken);

        categories = ApplyFiltering(categories, query.Name);
        categories = ApplyOrdering(categories, query.Order);

        return new ListCategoriesResult { Categories = categories };
    }

    // Helper method for filtering
    private static IEnumerable<string> ApplyFiltering(IEnumerable<string> categories, string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return categories;

        if (filter.StartsWith("*") && filter.EndsWith("*"))
            return categories.Where(c => c.Contains(filter.Trim('*'), StringComparison.OrdinalIgnoreCase));
        if (filter.StartsWith("*"))
            return categories.Where(c => c.EndsWith(filter.TrimStart('*'), StringComparison.OrdinalIgnoreCase));
        if (filter.EndsWith("*"))
            return categories.Where(c => c.StartsWith(filter.TrimEnd('*'), StringComparison.OrdinalIgnoreCase));
        return categories.Where(c => c.Equals(filter, StringComparison.OrdinalIgnoreCase));
    }

    // Helper method for ordering
    private static IEnumerable<string> ApplyOrdering(IEnumerable<string> categories, string? order)
    {
        if (!string.IsNullOrWhiteSpace(order) && order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            return categories.OrderByDescending(c => c);
        return categories.OrderBy(c => c);
    }
}