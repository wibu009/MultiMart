using MultiMart.Application.Catalog.Product.Models;
using MultiMart.Application.Catalog.Product.Requests.Queries;
using MultiMart.Application.Common.Specification;

namespace MultiMart.Application.Catalog.Product.Specifications;

public class ExportProductsWithBrandsSpecification : EntitiesByBaseFilterSpec<Domain.Catalog.Product, ProductExportDto>
{
    public ExportProductsWithBrandsSpecification(ExportProductsRequest request)
        : base(request) =>
        Query
            .Include(p => p.Brand)
            .Where(p => p.BrandId.Equals(request.BrandId!.Value), request.BrandId.HasValue)
            .Where(p => p.Rate >= request.MinimumRate!.Value, request.MinimumRate.HasValue)
            .Where(p => p.Rate <= request.MaximumRate!.Value, request.MaximumRate.HasValue);
}