using MultiMart.Application.Common.Exceptions;

namespace MultiMart.Application.Catalog.V1.Category.Get;

public class GetCategoryRequestHandler : IRequestHandler<GetCategoryRequest, GetCategoryResponse>
{
    private readonly IReadRepository<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer _t;

    public GetCategoryRequestHandler(IReadRepository<Domain.Catalog.Category> repository, IStringLocalizer<GetCategoryRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<GetCategoryResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    => await _repository
           .FirstOrDefaultAsync(
               LambdaSpecification<Domain.Catalog.Category, GetCategoryResponse>
                   .Create(spec => spec.Query.Where(c => c.Id == request.Id)),
               cancellationToken)
        ?? throw new NotFoundException(_t["Category not found"]);
}