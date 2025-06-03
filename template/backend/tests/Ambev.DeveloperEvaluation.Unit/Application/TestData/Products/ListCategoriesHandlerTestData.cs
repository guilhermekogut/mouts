using Ambev.DeveloperEvaluation.Application.Products.ListCategories;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

/// <summary>
/// Provides methods for generating test data for ListCategoriesHandler unit tests.
/// </summary>
public static class ListCategoriesHandlerTestData
{
    public static ListCategoriesCommand GenerateValidCommand() => new ListCategoriesCommand { Name = null, Order = null };
    public static ListCategoriesCommand GenerateFilteredCommand(string filter) => new ListCategoriesCommand { Name = filter, Order = null };
    public static ListCategoriesCommand GenerateOrderedCommand(string order) => new ListCategoriesCommand { Name = null, Order = order };
}