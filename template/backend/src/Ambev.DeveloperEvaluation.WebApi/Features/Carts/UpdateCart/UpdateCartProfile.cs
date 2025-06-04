using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    /// <summary>
    /// Profile for mapping between UpdateCartRequest, UpdateCartCommand, and UpdateCartResponse.
    /// </summary>
    public class UpdateCartProfile : Profile
    {
        public UpdateCartProfile()
        {
            CreateMap<UpdateCartRequest, UpdateCartResponse>();
        }
    }
}