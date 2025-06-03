﻿namespace Ambev.DeveloperEvaluation.Application.Products.Common
{
    public class ProductItemResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public ProductRatingResult Rating { get; set; } = new();
    }
}
