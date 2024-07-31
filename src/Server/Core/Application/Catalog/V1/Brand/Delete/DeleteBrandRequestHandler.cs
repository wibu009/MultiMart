using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.FileStorage.Cloudinary;

namespace MultiMart.Application.Catalog.V1.Brand.Delete;

public class DeleteBrandRequestHandler : IRequestHandler<DeleteBrandRequest, DeleteBrandResponse>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
    private readonly ICloudinaryFileStorageService _cloudinaryFileStorageService;
    private readonly IStringLocalizer _t;

    public DeleteBrandRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Brand> repository,
        ICloudinaryFileStorageService cloudinaryFileStorageService,
        IStringLocalizer<DeleteBrandRequestHandler> t)
    {
        _repository = repository;
        _cloudinaryFileStorageService = cloudinaryFileStorageService;
        _t = t;
    }

    public async Task<DeleteBrandResponse> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(_t["Brand not found."]);

        if (!string.IsNullOrWhiteSpace(brand.LogoUrl))
        {
            await _cloudinaryFileStorageService.RemoveAsync(brand.LogoUrl, cancellationToken: cancellationToken);
        }

        await _repository.DeleteAsync(brand, cancellationToken);

        return new DeleteBrandResponse { Message = _t["Brand deleted successfully."] };
    }
}