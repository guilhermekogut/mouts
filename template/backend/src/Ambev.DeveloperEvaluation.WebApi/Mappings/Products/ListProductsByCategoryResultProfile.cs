using Ambev.DeveloperEvaluation.Application.Products.ListProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class ListProductsByCategoryResultProfile : Profile
{
    public ListProductsByCategoryResultProfile()
    {
        CreateMap<ListProductsByCategoryResult, ListProductsResponse>();
    }
}