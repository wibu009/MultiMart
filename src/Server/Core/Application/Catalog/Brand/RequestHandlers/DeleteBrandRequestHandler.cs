using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Brand.RequestHandlers;

public class DeleteBrandRequestHandler : IRequestHandler<DeleteBrandRequest, string>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
    private readonly ICloudinaryFileStorageService _cloudinaryFileStorageService;
    private readonly IStringLocalizer<DeleteBrandRequestHandler> _t;

    public DeleteBrandRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Brand> repository,
        ICloudinaryFileStorageService cloudinaryFileStorageService,
        IStringLocalizer<DeleteBrandRequestHandler> t)
        => (_repository, _cloudinaryFileStorageService, _t) = (repository, cloudinaryFileStorageService, t);

    public async Task<string> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (!string.IsNullOrWhiteSpace(brand.LogoUrl))
        {
            await _cloudinaryFileStorageService.RemoveAsync(brand.LogoUrl, cancellationToken: cancellationToken);
        }

        await _repository.DeleteAsync(brand, cancellationToken);

        return _t["Brand deleted successfully."];
    }
}