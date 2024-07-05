using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Application.Common.FileStorage.LocalStorage;
using MultiMart.Application.Common.Persistence;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Product.Requests.Commands;

public class UpdateProductRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public Guid BrandId { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
    public FileUploadRequest? Image { get; set; }
}

public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest, Guid>
{
    private readonly IRepository<Domain.Catalog.Product> _repository;
    private readonly IStringLocalizer _t;
    private readonly ILocalFileStorageService _localFile;

    public UpdateProductRequestHandler(IRepository<Domain.Catalog.Product> repository, IStringLocalizer<UpdateProductRequestHandler> localizer, ILocalFileStorageService localFile) =>
        (_repository, _t, _localFile) = (repository, localizer, localFile);

    public async Task<Guid> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = product ?? throw new NotFoundException(_t["Product {0} Not Found.", request.Id]);

        // Remove old image if flag is set
        if (request.DeleteCurrentImage)
        {
            string? currentProductImagePath = product.ImagePath;
            if (!string.IsNullOrEmpty(currentProductImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                await _localFile.RemoveAsync(Path.Combine(root, currentProductImagePath), cancellationToken);
            }

            product = product.ClearImagePath();
        }

        string? productImagePath = request.Image is not null
            ? await _localFile.UploadAsync<Domain.Catalog.Product>(request.Image, FileType.Image, cancellationToken)
            : null;

        var updatedProduct = product.Update(request.Name, request.Description, request.Rate, request.BrandId, productImagePath);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityUpdatedEvent.WithEntity(product));

        await _repository.UpdateAsync(updatedProduct, cancellationToken);

        return request.Id;
    }
}