using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Catalog.Brand.RequestHandlers;

public class UpdateBrandRequestHandler : IRequestHandler<UpdateBrandRequest, string>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
    private readonly ICloudinaryFileStorageService _cloudinaryFileStorageService;
    private readonly IStringLocalizer<UpdateBrandRequestHandler> _t;

    public UpdateBrandRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Brand> repository,
        ICloudinaryFileStorageService cloudinaryFileStorageService,
        IStringLocalizer<UpdateBrandRequestHandler> t)
        => (_repository, _cloudinaryFileStorageService, _t) = (repository, cloudinaryFileStorageService, t);

    public async Task<string> Handle(UpdateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);
        brand = request.Adapt(brand);

        if (request.DeleteCurrentLogo)
        {
            if (!string.IsNullOrWhiteSpace(brand.LogoUrl))
            {
                await _cloudinaryFileStorageService.RemoveAsync(brand.LogoUrl, cancellationToken: cancellationToken);
                brand.LogoUrl = null;
            }
        }
        else if (request.Logo != null)
        {
            if (!string.IsNullOrWhiteSpace(brand.LogoUrl))
            {
                await _cloudinaryFileStorageService.RemoveAsync(brand.LogoUrl, cancellationToken: cancellationToken);
            }

            var uploadResult = await _cloudinaryFileStorageService.UploadAsync(request.Logo, FileType.Image, cancellationToken: cancellationToken);
            brand.LogoUrl = uploadResult.Url;
        }

        await _repository.UpdateAsync(brand!, cancellationToken);

        return _t["Brand updated successfully."];
    }
}