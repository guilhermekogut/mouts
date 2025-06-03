using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Query for listing products with pagination, ordering, and filtering.
/// </summary>
public class ListProductsCommand : IRequest<ListProductsResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }

    // Filtering attributes
    public string? Title { get; set; }
    public decimal? Price { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Image { get; set; }
    public decimal? RatingRate { get; set; }
    public decimal? MinRatingRate { get; set; }
    public decimal? MaxRatingRate { get; set; }
    public int? RatingCount { get; set; }
    public int? MinRatingCount { get; set; }
    public int? MaxRatingCount { get; set; }
}