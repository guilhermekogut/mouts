using Ambev.DeveloperEvaluation.Application.Sales.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public class ListSalesProfile : Profile
    {
        public ListSalesProfile()
        {
            CreateMap<Sale, ListSaleItemResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<SaleItem, SaleItemResult>();
        }
    }
}