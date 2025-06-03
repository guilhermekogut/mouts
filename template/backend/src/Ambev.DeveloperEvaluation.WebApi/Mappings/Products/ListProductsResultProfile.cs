using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class ListProductsResultProfile : Profile
{
    public ListProductsResultProfile()
    {
        CreateMap<ListProductsResult, ListProductsResponse>();
    }
}