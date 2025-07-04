﻿namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common
{
    public class ListProductsItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public ProductRatingResponse Rating { get; set; } = new();
    }
}
