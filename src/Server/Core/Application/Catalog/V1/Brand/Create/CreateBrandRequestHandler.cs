using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Catalog.V1.Brand.Create;

public sealed class CreateBrandRequestHandler : IRequestHandler<CreateBrandRequest, CreateBrandResponse>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
    private readonly ICloudinaryFileStorageService _cloudinaryFileStorageService;
    private readonly IStringLocalizer _t;

    public CreateBrandRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Brand> repository,
        ICloudinaryFileStorageService cloudinaryFileStorageService,
        IStringLocalizer<CreateBrandRequestHandler> t)
    {
        _repository = repository;
        _cloudinaryFileStorageService = cloudinaryFileStorageService;
        _t = t;
    }

    public async Task<CreateBrandResponse> Handle(CreateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = request.Adapt(new Domain.Catalog.Brand());

        var logoUploadResult = request.Logo is not null
            ? await _cloudinaryFileStorageService.UploadAsync(request.Logo, FileType.Image, cancellationToken: cancellationToken)
            : null;
        brand.LogoUrl = logoUploadResult?.Url;

        await _repository.AddAsync(brand, cancellationToken);
        return new CreateBrandResponse { Message = _t["Brand created successfully."] };
    }
}