using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CancelSale
{
    /// <summary>
    /// Profile for mapping between CancelSaleRequest, CancelSaleCommand, and CancelSaleResponse.
    /// </summary>
    public class CancelSaleProfile : Profile
    {
        public CancelSaleProfile()
        {
            CreateMap<CancelSaleRequest, CancelSaleCommand>();
            CreateMap<CancelSaleResult, CancelSaleResponse>();
            CreateMap<Application.Sales.Common.SaleItemResult, SaleItemResponse>();
        }
    }
}