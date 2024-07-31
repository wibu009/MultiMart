using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Catalog.V1.Brand.Update;

public class UpdateBrandRequestHandler : IRequestHandler<UpdateBrandRequest, UpdateBrandResponse>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
    private readonly ICloudinaryFileStorageService _cloudinaryFileStorageService;
    private readonly IStringLocalizer _t;

    public UpdateBrandRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Brand> repository,
        ICloudinaryFileStorageService cloudinaryFileStorageService,
        IStringLocalizer<UpdateBrandRequestHandler> t)
    {
        _repository = repository;
        _cloudinaryFileStorageService = cloudinaryFileStorageService;
        _t = t;
    }

    public async Task<UpdateBrandResponse> Handle(UpdateBrandRequest request, CancellationToken cancellationToken)
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

        return new UpdateBrandResponse { Message = _t["Brand updated successfully."] };
    }
}