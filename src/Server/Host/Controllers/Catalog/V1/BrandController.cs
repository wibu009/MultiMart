using MultiMart.Application.Catalog.V1.Brand.Create;
using MultiMart.Application.Catalog.V1.Brand.Delete;
using MultiMart.Application.Catalog.V1.Brand.Get;
using MultiMart.Application.Catalog.V1.Brand.Search;
using MultiMart.Application.Catalog.V1.Brand.Update;
using MultiMart.Application.Common.Models;
using MultiMart.Infrastructure.Common.Extensions;

namespace MultiMart.Host.Controllers.Catalog.V1;

[ApiVersion(VersionName.V1)]
public class BrandController : VersionedApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.Brands)]
    [SwaggerOperation("Search brands using available filters.", "")]
    public async Task<PaginationResponse<SearchBrandResponseItem>> SearchAsync(SearchBrandRequest request, CancellationToken cancellationToken = default)
        => await Mediator.Send(request, cancellationToken);

    [HttpGet("{id:guid}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Brands)]
    [SwaggerOperation("Get brand details.", "")]
    public async Task<GetBrandResponse> GetAsync(DefaultIdType id, CancellationToken cancellationToken = default)
        => await Mediator.Send(new GetBrandRequest { Id = id }, cancellationToken);

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Brands)]
    [SwaggerOperation("Create a new brand.", "")]
    public async Task<CreateBrandResponse> CreateAsync(CreateBrandRequest request, CancellationToken cancellationToken = default)
        => await Mediator.Send(request, cancellationToken);

    [HttpPut("{id:guid}")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Brands)]
    [SwaggerOperation("Update an existing brand.", "")]
    public async Task<UpdateBrandResponse> UpdateAsync(DefaultIdType id, UpdateBrandRequest request, CancellationToken cancellationToken = default)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.Id), id), cancellationToken);

    [HttpDelete("{id:guid}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Brands)]
    [SwaggerOperation("Delete an existing brand.", "")]
    public async Task<DeleteBrandResponse> DeleteAsync(DefaultIdType id, CancellationToken cancellationToken = default)
        => await Mediator.Send(new DeleteBrandRequest { Id = id }, cancellationToken);
}