using MultiMart.Application.Catalog.Brand;
using MultiMart.Application.Catalog.Brand.Dtos;
using MultiMart.Application.Catalog.Brand.Requests.Commands;
using MultiMart.Application.Catalog.Brand.Requests.Queries;
using MultiMart.Application.Common.Models;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Catalog;

public class BrandsController : VersionedApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.Brands)]
    [SwaggerOperation("Search brands using available filters.", "")]
    public Task<PaginationResponse<BrandDto>> SearchAsync(SearchBrandsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Brands)]
    [SwaggerOperation("Get brand details.", "")]
    public Task<BrandDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetBrandRequest(id));
    }

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Brands)]
    [SwaggerOperation("Create a new brand.", "")]
    public Task<Guid> CreateAsync(CreateBrandRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Brands)]
    [SwaggerOperation("Update a brand.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateBrandRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Brands)]
    [SwaggerOperation("Delete a brand.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteBrandRequest(id));
    }

    [HttpPost("generate-random")]
    [RequiresPermission(ApplicationAction.Generate, ApplicationResource.Brands)]
    [SwaggerOperation("Generate a number of random brands.", "")]
    public Task<string> GenerateRandomAsync(GenerateRandomBrandRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("delete-random")]
    [RequiresPermission(ApplicationAction.Clean, ApplicationResource.Brands)]
    [SwaggerOperation("Delete the brands generated with the generate-random call.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Search))]
    public Task<string> DeleteRandomAsync()
    {
        return Mediator.Send(new DeleteRandomBrandRequest());
    }
}