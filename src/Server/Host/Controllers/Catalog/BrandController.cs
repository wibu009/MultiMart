using MultiMart.Application.Catalog.Brand.V1;
using MultiMart.Application.Common.Models;
using MultiMart.Infrastructure.Common.Extensions;

namespace MultiMart.Host.Controllers.Catalog;

public class BrandController : VersionedApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.Brands)]
    [SwaggerOperation("Search brands using available filters.", "")]
    public async Task<PaginationResponse<SearchBrandQuery.ResponseItem>> SearchAsync(SearchBrandQuery.Request request, CancellationToken cancellationToken = default)
        => await Mediator.Send(request, cancellationToken);

    [HttpGet("{id:guid}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Brands)]
    [SwaggerOperation("Get brand details.", "")]
    public async Task<GetBrandByIdQuery.Response> GetAsync(DefaultIdType id, CancellationToken cancellationToken = default)
        => await Mediator.Send(new GetBrandByIdQuery.Request { Id = id }, cancellationToken);

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Brands)]
    [SwaggerOperation("Create a new brand.", "")]
    public async Task<string> CreateAsync(CreateBrandCommand.Request request, CancellationToken cancellationToken = default)
        => await Mediator.Send(request, cancellationToken);

    [HttpPut("{id:guid}")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Brands)]
    [SwaggerOperation("Update an existing brand.", "")]
    public async Task<string> UpdateAsync(DefaultIdType id, UpdateBrandCommand.Request request, CancellationToken cancellationToken = default)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.Id), id), cancellationToken);

    [HttpDelete("{id:guid}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Brands)]
    [SwaggerOperation("Delete an existing brand.", "")]
    public async Task<string> DeleteAsync(DefaultIdType id, CancellationToken cancellationToken = default)
        => await Mediator.Send(new DeleteBrandCommand.Request { Id = id }, cancellationToken);
}