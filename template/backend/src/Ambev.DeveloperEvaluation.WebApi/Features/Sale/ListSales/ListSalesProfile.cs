using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.ListSales
{
    public class ListSalesProfile : Profile
    {
        public ListSalesProfile()
        {
            CreateMap<ListSalesRequest, ListSalesCommand>();
            CreateMap<ListSalesResult, ListSalesResponse>();
            CreateMap<ListSaleItemResult, ListSalesItemResponse>();
            CreateMap<Application.Sales.Common.SaleItemResult, SaleItemResponse>();
        }
    }
}