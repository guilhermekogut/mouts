using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

using AutoMapper;
namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Carts;

public class CreateCartResultProfile : Profile
{
    public CreateCartResultProfile()
    {
        CreateMap<CreateCartResult, CreateCartResponse>();
        CreateMap<CartProductItemResult, CartProductItemResponse>();
    }
}
