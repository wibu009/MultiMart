namespace MultiMart.Application.Catalog.V1.Category.Delete;

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, DeleteCategoryResponse>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer<DeleteCategoryRequestHandler> _t;

    public DeleteCategoryRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Category> repository,
        IStringLocalizer<DeleteCategoryRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<DeleteCategoryResponse> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(category!, cancellationToken);
        return new DeleteCategoryResponse { Message = _t["Category deleted successfully."] };
    }
}