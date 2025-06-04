using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

public static class UpdateCartHandlerTestData
{
    private static readonly Faker<CartProductItemResult> ProductFaker = new Faker<CartProductItemResult>("pt_BR")
        .RuleFor(p => p.ProductId, f => f.Random.Guid())
        .RuleFor(p => p.Quantity, f => f.Random.Int(1, 5));

    private static readonly Faker<UpdateCartCommand> CommandFaker = new Faker<UpdateCartCommand>("pt_BR")
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.UserId, f => f.Random.Guid())
        .RuleFor(c => c.Date, f => f.Date.Recent())
        .RuleFor(c => c.Products, f => ProductFaker.Generate(f.Random.Int(1, 3)));

    public static UpdateCartCommand GenerateValidCommand() => CommandFaker.Generate();

    public static UpdateCartCommand GenerateCommandWithInvalidQuantity()
    {
        var command = CommandFaker.Generate();
        command.Products[0].Quantity = 0; // Invalid: quantity < 1
        return command;
    }

    public static UpdateCartCommand GenerateCommandWithDuplicateProducts()
    {
        var command = GenerateValidCommand();
        if (command.Products.Count == 0)
        {
            command.Products.Add(new CartProductItemResult
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1
            });
        }
        var duplicate = new CartProductItemResult
        {
            ProductId = command.Products[0].ProductId,
            Quantity = 2
        };
        command.Products.Add(duplicate);
        return command;
    }

    public static Cart GenerateCartFromCommand(UpdateCartCommand command)
    {
        return new Cart(
            command.UserId,
            command.Date,
            command.Products.Select(p => new CartProduct(p.ProductId, p.Quantity))
        )
        {
            Id = command.Id
        };
    }

    public static UpdateCartResult GenerateResultFromCart(Cart cart)
    {
        return new UpdateCartResult
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