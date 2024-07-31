using MultiMart.Application.Catalog.Category.Models;
using MultiMart.Application.Catalog.Category.Requests;
using MultiMart.Application.Common.Models;
using MultiMart.Infrastructure.Common.Extensions;

namespace MultiMart.Host.Controllers.Catalog.V1;

public class CategoryController : VersionedApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.Categories)]
    [SwaggerOperation("Search categories using available filters.", "")]
    public async Task<PaginationResponse<CategoryDto>> SearchAsync(SearchCategoryRequest request)
        => await Mediator.Send(request);

    [HttpGet("{id:guid}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Categories)]
    [SwaggerOperation("Get category details.", "")]
    public async Task<CategoryDto> GetAsync(DefaultIdType id)
        => await Mediator.Send(new GetCategoryByIdRequest(id));

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Categories)]
    [SwaggerOperation("Create a new category.", "")]
    public async Task<string> CreateAsync(CreateCategoryRequest request)
        => await Mediator.Send(request);

    [HttpPut("{id:guid}")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Categories)]
    [SwaggerOperation("Update an existing category.", "")]
    public async Task<string> UpdateAsync(DefaultIdType id, UpdateCategoryRequest request)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.Id), id));

    [HttpDelete("{id:guid}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Categories)]
    [SwaggerOperation("Delete an existing category.", "")]
    public async Task<string> DeleteAsync(DefaultIdType id)
        => await Mediator.Send(new DeleteCategoryRequest(id));
}