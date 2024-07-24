using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Exceptions;

namespace MultiMart.Application.Catalog.Brand.RequestHandlers;

public class GetBrandByIdRequestHandler : IRequestHandler<GetBrandByIdRequest, BrandDto>
{
    private readonly IReadRepository<Domain.Catalog.Brand> _repository;
    private readonly IStringLocalizer _t;

    public GetBrandByIdRequestHandler(IReadRepository<Domain.Catalog.Brand> repository, IStringLocalizer<GetBrandByIdRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<BrandDto> Handle(GetBrandByIdRequest request, CancellationToken cancellationToken)
        => await _repository.FirstOrDefaultAsync(new GetBrandByIdSpec(request.Id), cancellationToken)
           ?? throw new NotFoundException(_t["Brand not found"]);
}