﻿using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

/// <summary>
/// Query for listing carts with pagination, ordering, and filtering.
/// </summary>
public class ListCartsCommand : IRequest<ListCartsResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }

    // Filtering attributes
    public Guid? UserId { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}