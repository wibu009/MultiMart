using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Catalog.Brand.RequestHandlers;

public class CreateBrandRequestHandler : IRequestHandler<CreateBrandRequest, string>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
    private readonly ICloudinaryFileStorageService _cloudinaryFileStorageService;
    private readonly IStringLocalizer<CreateBrandRequestHandler> _t;

    public CreateBrandRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Brand> repository,
        ICloudinaryFileStorageService cloudinaryFileStorageService,
        IStringLocalizer<CreateBrandRequestHandler> t)
        => (_repository, _cloudinaryFileStorageService, _t) = (repository, cloudinaryFileStorageService, t);

    public async Task<string> Handle(CreateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = request.Adapt(new Domain.Catalog.Brand());

        var logoUploadResult = request.Logo is not null
            ? await _cloudinaryFileStorageService.UploadAsync(request.Logo, FileType.Image, cancellationToken: cancellationToken)
            : null;
        brand.LogoUrl = logoUploadResult?.Url;

        await _repository.AddAsync(brand, cancellationToken);
        return _t["Brand created successfully."];
    }
}