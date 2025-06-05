using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Profile for mapping between Cart entity, UpdateCartCommand, and UpdateCartResult.
    /// </summary>
    public class UpdateCartProfile : Profile
    {
        public UpdateCartProfile()
        {
            CreateMap<CartProductItemResult, CartProduct>()
                .ConstructUsing(src => new CartProduct(src.ProductId, src.Quantity));

            // Map UpdateCartCommand to Cart entity
            CreateMap<UpdateCartCommand, Cart>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
                    src.Products.Select(p => new CartProduct(p.ProductId, p.Quantity)).ToList()))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Map Cart entity to UpdateCartResult
            CreateMap<Cart, UpdateCartResult>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
                    src.Products.Select(p => new CartProductItemResult
                    {
                        ProductId = p.ProductId,
                        Quantity = p.Quantity
                    }).ToList()));
        }
    }
}