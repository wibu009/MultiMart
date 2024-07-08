using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Brand.Requests.Commands;

public class CreateBrandRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class CreateBrandRequestHandler : IRequestHandler<CreateBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;

    public CreateBrandRequestHandler(IRepositoryWithEvents<Domain.Catalog.Brand> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = new Domain.Catalog.Brand(request.Name, request.Description);

        await _repository.AddAsync(brand, cancellationToken);

        return brand.Id;
    }
}