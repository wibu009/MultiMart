using MultiMart.Application.Catalog.Product.Specifications;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Brand.Requests.Commands;

public class DeleteBrandRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteBrandRequest(Guid id) => Id = id;
}

public class DeleteBrandRequestHandler : IRequestHandler<DeleteBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _brandRepo;
    private readonly IReadRepository<Domain.Catalog.Product> _productRepo;
    private readonly IStringLocalizer _t;

    public DeleteBrandRequestHandler(IRepositoryWithEvents<Domain.Catalog.Brand> brandRepo, IReadRepository<Domain.Catalog.Product> productRepo, IStringLocalizer<DeleteBrandRequestHandler> localizer) =>
        (_brandRepo, _productRepo, _t) = (brandRepo, productRepo, localizer);

    public async Task<Guid> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
    {
        if (await _productRepo.AnyAsync(new ProductsByBrandSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_t["Brand cannot be deleted as it's being used."]);
        }

        var brand = await _brandRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(_t["Brand {0} Not Found."]);

        await _brandRepo.DeleteAsync(brand, cancellationToken);

        return request.Id;
    }
}