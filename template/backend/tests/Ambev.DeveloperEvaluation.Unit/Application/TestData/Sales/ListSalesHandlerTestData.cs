using Ambev.DeveloperEvaluation.Application.Sales.ListSales;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

/// <summary>
/// Provides methods for generating test data for ListSalesHandler unit tests.
/// </summary>
public static class ListSalesHandlerTestData
{
    private static readonly Faker<ListSalesCommand> Faker = new Faker<ListSalesCommand>("pt_BR")
        .RuleFor(c => c.Page, 1)
        .RuleFor(c => c.Size, 10);

    public static ListSalesCommand GenerateValidCommand() => Faker.Generate();

    public static ListSalesCommand GenerateInvalidCommand() => new ListSalesCommand
    {
        Page = 0,
        Size = 0
    };
}