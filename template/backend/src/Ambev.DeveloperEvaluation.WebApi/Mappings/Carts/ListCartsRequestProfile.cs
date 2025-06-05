using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Carts;

public class ListCartsRequestProfile : Profile
{
    public ListCartsRequestProfile()
    {
        CreateMap<ListCartsRequest, ListCartsCommand>();
    }
}