using MultiMart.Application.Catalog.Category.Requests;

namespace MultiMart.Application.Catalog.Category.RequestHandlers;

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, string>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer<DeleteCategoryRequestHandler> _t;

    public DeleteCategoryRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Category> repository,
        IStringLocalizer<DeleteCategoryRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<string> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(category!, cancellationToken);
        return _t["Category deleted successfully."];
    }
}