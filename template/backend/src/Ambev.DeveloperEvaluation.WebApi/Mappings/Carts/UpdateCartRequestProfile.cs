using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Carts
{
    public class UpdateCartRequestProfile : Profile
    {
        public UpdateCartRequestProfile()
        {
            CreateMap<UpdateCartRequest, UpdateCartCommand>();
            CreateMap<CartProductItemRequest, CartProductItemResult>();
            CreateMap<CartProductItemRequest, CartProductItemResponse>();
        }
    }
}