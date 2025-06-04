using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    /// <summary>
    /// Profile for mapping between CreateCartRequest, CreateCartCommand, Cart, and CreateCartResponse.
    /// </summary>
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartRequest, CreateCartResponse>();
        }
    }
}