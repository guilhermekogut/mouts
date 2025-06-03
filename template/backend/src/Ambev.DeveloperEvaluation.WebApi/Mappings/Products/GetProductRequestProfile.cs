using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class GetProductRequestProfile : Profile
{
    public GetProductRequestProfile()
    {
        CreateMap<GetProductRequest, GetProductCommand>();
    }
}