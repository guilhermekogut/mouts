using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts
{
    public class ListCartsProfile : Profile
    {
        public ListCartsProfile()
        {
            CreateMap<ListCartsRequest, ListCartsCommand>();
            CreateMap<ListCartsResult, ListCartsResponse>();
            CreateMap<ListCartItemResult, ListCartsItemResponse>();
            CreateMap<Ambev.DeveloperEvaluation.Application.Carts.Common.CartProductItemResult, CartProductItemResponse>();
        }
    }
}