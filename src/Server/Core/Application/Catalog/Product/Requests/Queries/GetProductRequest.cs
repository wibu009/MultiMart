using MultiMart.Application.Catalog.Product.Models;
using MultiMart.Application.Catalog.Product.Specifications;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Product.Requests.Queries;

public class GetProductRequest : IRequest<ProductDetailsDto>
{
    public Guid Id { get; set; }

    public GetProductRequest(Guid id) => Id = id;
}

public class GetProductRequestHandler : IRequestHandler<GetProductRequest, ProductDetailsDto>
{
    private readonly IRepository<Domain.Catalog.Product> _repository;
    private readonly IStringLocalizer _t;

    public GetProductRequestHandler(IRepository<Domain.Catalog.Product> repository, IStringLocalizer<GetProductRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<ProductDetailsDto> Handle(GetProductRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Domain.Catalog.Product, ProductDetailsDto>)new ProductByIdWithBrandSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Product {0} Not Found.", request.Id]);
}