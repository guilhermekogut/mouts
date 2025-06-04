using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Carts;

public class ListCartsResultProfile : Profile
{
    public ListCartsResultProfile()
    {
        CreateMap<ListCartsResult, ListCartsResponse>();
        CreateMap<ListCartItemResult, ListCartsItemResponse>();
        CreateMap<Ambev.DeveloperEvaluation.Application.Carts.Common.CartProductItemResult, CartProductItemResponse>();
    }
}