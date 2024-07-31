namespace MultiMart.Application.Catalog.V1.Category.Create;

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer _t;

    public CreateCategoryRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Category> repository,
        IStringLocalizer<CreateCategoryRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = request.Adapt(new Domain.Catalog.Category());
        await _repository.AddAsync(category, cancellationToken);
        return new CreateCategoryResponse { Message = _t["Category created successfully."] };
    }
}