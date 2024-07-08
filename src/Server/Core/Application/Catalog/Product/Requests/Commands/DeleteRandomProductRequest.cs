using MultiMart.Application.Catalog.Product.Interfaces;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Product.Requests.Commands;

public class DeleteRandomProductRequest : IRequest<string>
{
    public int NSeed { get; set; }
}

public class DeleteRandomProductRequestHandler : IRequestHandler<DeleteRandomProductRequest, string>
{
    private readonly IJobService _jobService;

    public DeleteRandomProductRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(DeleteRandomProductRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Enqueue<IProductGeneratorJob>(x => x.CleanAsync(default));
        return Task.FromResult(jobId);
    }
}