using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Profile for mapping between Cart entity, CreateCartCommand, and CreateCartResult.
    /// </summary>
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CartProductItemResult, CartProduct>()
               .ConstructUsing(src => new CartProduct(src.ProductId, src.Quantity));

            // Map CreateCartCommand to Cart entity
            CreateMap<CreateCartCommand, Cart>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
                    src.Products.Select(p => new CartProduct(p.ProductId, p.Quantity)).ToList()))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Map Cart entity to CreateCartResult
            CreateMap<Cart, CreateCartResult>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
                    src.Products.Select(p => new CartProductItemResult
                    {
                        ProductId = p.ProductId,
                        Quantity = p.Quantity
                    }).ToList()));


        }
    }
}