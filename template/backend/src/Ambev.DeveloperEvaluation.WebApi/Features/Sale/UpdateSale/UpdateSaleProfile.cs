using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale
{
    /// <summary>
    /// Profile for mapping between UpdateSaleRequest, UpdateSaleCommand, and UpdateSaleResponse.
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
            CreateMap<UpdateSaleResult, UpdateSaleResponse>();
            CreateMap<Application.Sales.Common.SaleItemResult, SaleItemResponse>();
        }
    }
}