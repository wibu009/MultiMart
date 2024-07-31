using MultiMart.Application.Catalog.Category.Models;
using MultiMart.Application.Catalog.Category.Requests;
using MultiMart.Application.Catalog.Category.Specifications;
using MultiMart.Application.Common.Exceptions;

namespace MultiMart.Application.Catalog.Category.RequestHandlers;

public class GetCategoryByIdRequestHandler : IRequestHandler<GetCategoryByIdRequest, CategoryDto>
{
    private readonly IReadRepository<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer _t;

    public GetCategoryByIdRequestHandler(IReadRepository<Domain.Catalog.Category> repository, IStringLocalizer<GetCategoryByIdRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<CategoryDto> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    => await _repository.FirstOrDefaultAsync(new GetCategoryByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Category not found"]);
}