using Ambev.DeveloperEvaluation.Application.Sales.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Profile for mapping between Sale entity, UpdateSaleCommand, and UpdateSaleResult.
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            // Map Sale entity to UpdateSaleResult
            CreateMap<Sale, UpdateSaleResult>()
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