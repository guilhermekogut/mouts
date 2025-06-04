using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Carts;

public class GetCartResultProfile : Profile
{
    public GetCartResultProfile()
    {
        CreateMap<GetCartResult, GetCartResponse>();
        CreateMap<Ambev.DeveloperEvaluation.Application.Carts.Common.CartProductItemResult, CartProductItemResponse>();
    }
}