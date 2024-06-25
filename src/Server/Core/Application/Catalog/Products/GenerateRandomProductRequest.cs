using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Products;

public class GenerateRandomProductRequest : IRequest<string>
{
    public int NSeed { get; set; }
}

public class GenerateRandomProductRequestHandler : IRequestHandler<GenerateRandomProductRequest, string>
{
    private readonly IJobService _jobService;

    public GenerateRandomProductRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(GenerateRandomProductRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Enqueue<IProductGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}