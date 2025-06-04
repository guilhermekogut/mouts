using Ambev.DeveloperEvaluation.Application.Sales.Common;

using AutoMapper;
namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and CreateSaleResult.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<Sale, CreateSaleResult>();
            CreateMap<SaleItem, SaleItemResult>();
        }
    }
}