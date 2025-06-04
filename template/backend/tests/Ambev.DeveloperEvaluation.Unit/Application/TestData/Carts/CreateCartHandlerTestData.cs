using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for CreateCartHandler unit tests.
/// </summary>
public static class CreateCartHandlerTestData
{
    private static readonly Faker<CartProductItemResult> ProductFaker = new Faker<CartProductItemResult>("pt_BR")
        .RuleFor(p => p.ProductId, f => f.Random.Guid())
        .RuleFor(p => p.Quantity, f => f.Random.Int(1, 5));

    private static readonly Faker<CreateCartCommand> CommandFaker = new Faker<CreateCartCommand>("pt_BR")
        .RuleFor(c => c.UserId, f => f.Random.Guid())
        .RuleFor(c => c.Products, f => ProductFaker.Generate(f.Random.Int(1, 3)));

    /// <summary>
    /// Generates a command with at least one invalid product ID (not existing in the system).
    /// </summary>
    public static CreateCartCommand GenerateCommandWithInvalidProductId(Guid? invalidProductId = null)
    {
        var command = GenerateValidCommand();
        command.Products[0].ProductId = invalidProductId ?? Guid.NewGuid();
        return command;
    }

    public static CreateCartCommand GenerateValidCommand() => CommandFaker.Generate();

    public static CreateCartCommand GenerateCommandWithInvalidQuantity()
    {
        var command = CommandFaker.Generate();
        command.Products[0].Quantity = 0; // Invalid: quantity < 1
        return command;
    }

    public static Cart GenerateCartFromCommand(CreateCartCommand command)
    {
        return new Cart(
            command.UserId,
            DateTime.UtcNow,
            command.Products.Select(p => new CartProduct(p.ProductId, p.Quantity))
        );
    }

    public static CreateCartResult GenerateResultFromCart(Cart cart)
    {
        return new CreateCartResult
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Date = cart.Date,
            Products = cart.Products.Select(p => new CartProductItemResult
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        };
    }
}