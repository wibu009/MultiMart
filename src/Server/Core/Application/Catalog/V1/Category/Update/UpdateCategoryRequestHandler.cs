namespace MultiMart.Application.Catalog.V1.Category.Update;

public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, UpdateCategoryResponse>
{
    private readonly IRepositoryWithEvents<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer<UpdateCategoryRequestHandler> _t;

    public UpdateCategoryRequestHandler(
        IRepositoryWithEvents<Domain.Catalog.Category> repository,
        IStringLocalizer<UpdateCategoryRequestHandler> t)
        => (_repository, _t) = (repository, t);

    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        category = request.Adapt(category);
        await _repository.UpdateAsync(category!, cancellationToken);
        return new UpdateCategoryResponse { Message = _t["Category updated successfully."] };
    }
}