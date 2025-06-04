using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart
{
    public class GetCartProfile : Profile
    {
        public GetCartProfile()
        {
            CreateMap<Guid, GetCartCommand>()
                .ConstructUsing(id => new GetCartCommand(id));

            CreateMap<GetCartResult, GetCartResponse>();
            CreateMap<Ambev.DeveloperEvaluation.Application.Carts.Common.CartProductItemResult, CartProductItemResponse>();
        }
    }
}