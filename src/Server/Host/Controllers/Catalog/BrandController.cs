using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Common.Models;
using MultiMart.Infrastructure.Common.Extensions;

namespace MultiMart.Host.Controllers.Catalog;

public class BrandController : VersionedApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.Brands)]
    [SwaggerOperation("Search brands using available filters.", "")]
    public async Task<PaginationResponse<BrandDto>> SearchAsync(SearchBrandRequest request)
        => await Mediator.Send(request);

    [HttpGet("{id:guid}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Brands)]
    [SwaggerOperation("Get brand details.", "")]
    public async Task<BrandDto> GetAsync(DefaultIdType id)
        => await Mediator.Send(new GetBrandByIdRequest(id));

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Brands)]
    [SwaggerOperation("Create a new brand.", "")]
    public async Task<string> CreateAsync(CreateBrandRequest request)
        => await Mediator.Send(request);

    [HttpPut("{id:guid}")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Brands)]
    [SwaggerOperation("Update an existing brand.", "")]
    public async Task<string> UpdateAsync(DefaultIdType id, UpdateBrandRequest request)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.Id), id));

    [HttpDelete("{id:guid}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Brands)]
    [SwaggerOperation("Delete an existing brand.", "")]
    public async Task<string> DeleteAsync(DefaultIdType id)
        => await Mediator.Send(new DeleteBrandRequest(id));
}