using MultiMart.Application.Catalog.Brand.Dtos;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Brand.Requests.Queries;

public class GetBrandRequest : IRequest<BrandDto>
{
    public Guid Id { get; set; }

    public GetBrandRequest(Guid id) => Id = id;
}

public class GetBrandRequestHandler : IRequestHandler<GetBrandRequest, BrandDto>
{
    private readonly IRepository<Domain.Catalog.Brand> _repository;
    private readonly IStringLocalizer _t;

    public GetBrandRequestHandler(IRepository<Domain.Catalog.Brand> repository, IStringLocalizer<GetBrandRequestHandler> localizer) => (_repository, _t) = (repository, localizer);

    public async Task<BrandDto> Handle(GetBrandRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            new BrandByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Brand {0} Not Found.", request.Id]);
}