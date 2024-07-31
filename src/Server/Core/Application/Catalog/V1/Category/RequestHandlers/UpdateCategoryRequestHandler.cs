using MultiMart.Application.Catalog.Category.Requests;

namespace MultiMart.Application.Catalog.Category.RequestHandlers;

public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, string>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer<UpdateCategoryRequestHandler> _t;

    public UpdateCategoryRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Category> repository,
        IStringLocalizer<UpdateCategoryRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<string> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        category = request.Adapt(category);
        await _repository.UpdateAsync(category!, cancellationToken);
        return _t["Category updated successfully."];
    }
}