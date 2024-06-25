using MultiMart.Application.Catalog.Products;
using MultiMart.Application.Common.Models;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Catalog;

public class ProductsController : VersionedApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.Products)]
    [SwaggerOperation("Search products using available filters.", "")]
    public Task<PaginationResponse<ProductDto>> SearchAsync(SearchProductsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Products)]
    [SwaggerOperation("Get product details.", "")]
    public Task<ProductDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetProductRequest(id));
    }

    [HttpGet("dapper")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Products)]
    [SwaggerOperation("Get product details via dapper.", "")]
    public Task<ProductDto> GetDapperAsync(Guid id)
    {
        return Mediator.Send(new GetProductViaDapperRequest(id));
    }

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Products)]
    [SwaggerOperation("Create a new product.", "")]
    public Task<Guid> CreateAsync(CreateProductRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Products)]
    [SwaggerOperation("Update a product.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateProductRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Products)]
    [SwaggerOperation("Delete a product.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteProductRequest(id));
    }

    [HttpPost("export")]
    [RequiresPermission(ApplicationAction.Export, ApplicationResource.Products)]
    [SwaggerOperation("Export a products.", "")]
    public async Task<FileResult> ExportAsync(ExportProductsRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "ProductExports");
    }

    [HttpPost("generate-random")]
    [RequiresPermission(ApplicationAction.Generate, ApplicationResource.Products)]
    [SwaggerOperation("Generate a number of random products.", "")]
    public Task<string> GenerateRandomAsync(GenerateRandomProductRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("delete-random")]
    [RequiresPermission(ApplicationAction.Clean, ApplicationResource.Products)]
    [SwaggerOperation("Delete the products generated with the generate-random call.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Search))]
    public Task<string> DeleteRandomAsync()
    {
        return Mediator.Send(new DeleteRandomProductRequest());
    }
}