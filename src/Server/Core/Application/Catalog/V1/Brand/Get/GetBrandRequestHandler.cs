using MultiMart.Application.Common.Exceptions;

namespace MultiMart.Application.Catalog.V1.Brand.Get;

public class GetBrandRequestHandler : IRequestHandler<GetBrandRequest, GetBrandResponse>
{
    private readonly IReadRepository<Domain.Catalog.Brand> _repository;
    private readonly IStringLocalizer _t;

    public GetBrandRequestHandler(
        IReadRepository<Domain.Catalog.Brand> repository,
        IStringLocalizer<GetBrandRequestHandler> t)
    {
        _repository = repository;
        _t = t;
    }

    public async Task<GetBrandResponse> Handle(GetBrandRequest request, CancellationToken cancellationToken)
    => await _repository.FirstOrDefaultAsync(
        LambdaSingleResultSpecification<Domain.Catalog.Brand, GetBrandResponse>
            .Create(spec => spec.Query.Where(b => b.Id == request.Id)),
        cancellationToken)
       ?? throw new NotFoundException(_t["Brand not found"]);
}