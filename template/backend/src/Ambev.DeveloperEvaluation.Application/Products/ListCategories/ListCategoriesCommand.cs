using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListCategories;

/// <summary>
/// Command for listing all product categories.
/// </summary>
public class ListCategoriesCommand : IRequest<ListCategoriesResult>
{
    public string? Name { get; set; } // Filtro por nome da categoria (parcial ou exata)
    public string? Order { get; set; } // "asc" ou "desc"
}