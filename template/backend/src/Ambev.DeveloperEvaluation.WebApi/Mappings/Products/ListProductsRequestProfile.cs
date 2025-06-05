using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class ListProductsRequestProfile : Profile
{
    public ListProductsRequestProfile()
    {
        CreateMap<ListProductsRequest, ListProductsCommand>();
    }
}