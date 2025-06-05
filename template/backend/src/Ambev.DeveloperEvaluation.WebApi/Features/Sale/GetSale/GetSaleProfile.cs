using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale
{
    public class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            CreateMap<Guid, GetSaleCommand>()
                .ConstructUsing(id => new GetSaleCommand(id));

            CreateMap<GetSaleResult, GetSaleResponse>();
            CreateMap<Application.Sales.Common.SaleItemResult, SaleItemResponse>();
        }
    }
}