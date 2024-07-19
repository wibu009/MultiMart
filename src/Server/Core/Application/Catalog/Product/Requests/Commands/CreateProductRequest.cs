using MultiMart.Application.Common.FileStorage;
using MultiMart.Application.Common.FileStorage.LocalStorage;
using MultiMart.Application.Common.Persistence;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Product.Requests.Commands;

public class CreateProductRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public Guid BrandId { get; set; }
    public FileUploadRequest? Image { get; set; }
}

public class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, Guid>
{
    private readonly IRepository<Domain.Catalog.Product> _repository;
    private readonly ILocalFileStorageService _localFile;

    public CreateProductRequestHandler(IRepository<Domain.Catalog.Product> repository, ILocalFileStorageService localFile) =>
        (_repository, _localFile) = (repository, localFile);

    public async Task<Guid> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        string productImagePath = await _localFile.UploadAsync<Domain.Catalog.Product>(request.Image, FileType.Image, cancellationToken);

        var product = new Domain.Catalog.Product(request.Name, request.Description, request.Rate, request.BrandId, productImagePath);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityCreatedEvent.WithEntity(product));

        await _repository.AddAsync(product, cancellationToken);

        return product.Id;
    }
}