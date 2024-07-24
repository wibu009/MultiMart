using MultiMart.Application.Catalog.Category.Requests;

namespace MultiMart.Application.Catalog.Category.RequestHandlers;

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, string>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer<CreateCategoryRequestHandler> _t;

    public CreateCategoryRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Category> repository,
        IStringLocalizer<CreateCategoryRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<string> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = request.Adapt(new Domain.Catalog.Category());
        await _repository.AddAsync(category, cancellationToken);
        return _t["Category created successfully."];
    }
}