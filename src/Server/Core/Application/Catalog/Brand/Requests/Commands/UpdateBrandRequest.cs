using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Brand.Requests.Commands;

public class UpdateBrandRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class UpdateBrandRequestHandler : IRequestHandler<UpdateBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
    private readonly IStringLocalizer _t;

    public UpdateBrandRequestHandler(IRepositoryWithEvents<Domain.Catalog.Brand> repository, IStringLocalizer<UpdateBrandRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = brand
        ?? throw new NotFoundException(_t["Brand {0} Not Found.", request.Id]);

        brand.Update(request.Name, request.Description);

        await _repository.UpdateAsync(brand, cancellationToken);

        return request.Id;
    }
}