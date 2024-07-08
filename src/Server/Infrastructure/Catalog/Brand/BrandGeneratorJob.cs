using Ardalis.Specification;
using Bogus;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using MediatR;
using Microsoft.Extensions.Logging;
using MultiMart.Application.Catalog.Brand;
using MultiMart.Application.Catalog.Brand.Interfaces;
using MultiMart.Application.Catalog.Brand.Requests.Commands;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Common.Persistence;
using MultiMart.Shared.Notifications;

namespace MultiMart.Infrastructure.Catalog.Brand;

public class BrandGeneratorJob : IBrandGeneratorJob
{
    private readonly ILogger<BrandGeneratorJob> _logger;
    private readonly ISender _mediator;
    private readonly IReadRepository<Domain.Catalog.Brand> _repository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public BrandGeneratorJob(
        ILogger<BrandGeneratorJob> logger,
        ISender mediator,
        IReadRepository<Domain.Catalog.Brand> repository,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _mediator = mediator;
        _repository = repository;
        _progressBar = progressBar;
        _performingContext = performingContext;
        _notifications = notifications;
        _currentUser = currentUser;
        _progress = _progressBar.Create();
    }

    private async Task NotifyAsync(string message, int progress, CancellationToken cancellationToken)
    {
        _progress.SetValue(progress);
        await _notifications.SendToUserAsync(
            new JobNotification()
            {
                JobId = _performingContext.BackgroundJob.Id,
                Message = message,
                Progress = progress
            },
            _currentUser.GetUserId().ToString(),
            cancellationToken);
    }

    [Queue("notdefault")]
    public async Task GenerateAsync(int nSeed, CancellationToken cancellationToken)
    {
        await NotifyAsync("Your job processing has started", 0, cancellationToken);

        var createBrandRequests = new Faker<CreateBrandRequest>()
            .RuleFor(b => b.Name, f => "Brand Random - " + f.Company.CompanyName())
            .RuleFor(b => b.Description, f => f.Lorem.Sentence())
            .Generate(nSeed);

        _logger.LogInformation("Generated {BrandsCount} random brands", createBrandRequests.Count.ToString());

        foreach (var brand in createBrandRequests)
        {
            await _mediator.Send(brand, cancellationToken);
            await NotifyAsync("Brand created", (createBrandRequests.IndexOf(brand) + 1) * 100 / createBrandRequests.Count, cancellationToken);
        }

        await NotifyAsync("Job successfully completed", 0, cancellationToken);
    }

    [Queue("notdefault")]
    [AutomaticRetry(Attempts = 5)]
    public async Task CleanAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing Job with Id: {JobId}", _performingContext.BackgroundJob.Id);

        var items = await _repository.ListAsync(new RandomBrandsSpec(), cancellationToken);

        _logger.LogInformation("Brands Random: {BrandsCount} ", items.Count.ToString());

        foreach (var item in items)
        {
            await _mediator.Send(new DeleteBrandRequest(item.Id), cancellationToken);
        }

        _logger.LogInformation("All random brands deleted.");
    }
}

public class RandomBrandsSpec : Specification<Domain.Catalog.Brand>
{
    public RandomBrandsSpec() =>
        Query.Where(b => !string.IsNullOrEmpty(b.Name) && b.Name.Contains("Brand Random"));
}