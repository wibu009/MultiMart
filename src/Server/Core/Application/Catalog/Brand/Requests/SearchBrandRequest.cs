using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Common.Models;

namespace MultiMart.Application.Catalog.Brand.Requests;

public class SearchBrandRequest : PaginationFilter, IRequest<PaginationResponse<BrandDto>>
{
}