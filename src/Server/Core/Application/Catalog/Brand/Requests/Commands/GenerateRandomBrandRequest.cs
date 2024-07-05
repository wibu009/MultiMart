using MultiMart.Application.Catalog.Brand.Interfaces;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Brand.Requests.Commands;

public class GenerateRandomBrandRequest : IRequest<string>
{
    public int NSeed { get; set; }
}

public class GenerateRandomBrandRequestHandler : IRequestHandler<GenerateRandomBrandRequest, string>
{
    private readonly IJobService _jobService;

    public GenerateRandomBrandRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(GenerateRandomBrandRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Enqueue<IBrandGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}