using Ambev.DeveloperEvaluation.Application.Sales.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and CancelSaleResult.
    /// </summary>
    public class CancelSaleProfile : Profile
    {
        public CancelSaleProfile()
        {
            CreateMap<Sale, CancelSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
                    src.Items.Select(i => new SaleItemResult
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Discount = i.Discount,
                        Total = i.Total,
                        Cancelled = i.Cancelled
                    }).ToList()));
        }
    }
}