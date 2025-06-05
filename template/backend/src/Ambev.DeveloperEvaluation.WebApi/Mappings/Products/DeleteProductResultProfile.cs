using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class DeleteProductResultProfile : Profile
{
    public DeleteProductResultProfile()
    {
        CreateMap<DeleteProductResult, DeleteProductResponse>();
    }
}