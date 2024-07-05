using Ardalis.Specification;
using Bogus;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using MediatR;
using Microsoft.Extensions.Logging;
using MultiMart.Application.Catalog.Product;
using MultiMart.Application.Catalog.Product.Interfaces;
using MultiMart.Application.Catalog.Product.Requests.Commands;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Common.Persistence;
using MultiMart.Infrastructure.Catalog.Brand;
using MultiMart.Shared.Notifications;

namespace MultiMart.Infrastructure.Catalog.Product;

public class ProductGeneratorJob : IProductGeneratorJob
{
    private readonly ILogger<ProductGeneratorJob> _logger;
    private readonly ISender _mediator;
    private readonly IReadRepository<Domain.Catalog.Product> _productRepository;
    private readonly IReadRepository<Domain.Catalog.Brand> _brandRepository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public ProductGeneratorJob(
        ILogger<ProductGeneratorJob> logger,
        ISender mediator,
        IReadRepository<Domain.Catalog.Product> productRepository,
        IReadRepository<Domain.Catalog.Brand> brandRepository,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _mediator = mediator;
        _productRepository = productRepository;
        _progressBar = progressBar;
        _performingContext = performingContext;
        _notifications = notifications;
        _currentUser = currentUser;
        _brandRepository = brandRepository;
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
        //get all brands
        var brands = await _brandRepository.ListAsync(cancellationToken);

        //generate products using bogus
        var createProductRequests = new Faker<CreateProductRequest>()
            .RuleFor(p => p.Name, f => "Product Random - " + f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Rate, f => f.Random.Decimal(1, 5))
            .RuleFor(p => p.BrandId, f => f.PickRandom(brands).Id)
            .Generate(nSeed);

        _logger.LogInformation("Generated {ProductsCount} random products", createProductRequests.Count.ToString());

        await NotifyAsync("Your job processing has started", 0, cancellationToken);

        foreach (var product in createProductRequests)
        {
            await _mediator.Send(product, cancellationToken);
            await NotifyAsync("Product created", (createProductRequests.IndexOf(product) + 1) * 100 / createProductRequests.Count, cancellationToken);
        }
    }

    public async Task CleanAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing Job with Id: {JobId}", _performingContext.BackgroundJob.Id);

        var items = await _productRepository.ListAsync(new RandomProductsSpec(), cancellationToken);

        _logger.LogInformation("Product Random: {ProductsCount} ", items.Count.ToString());

        foreach (var item in items)
        {
            await _mediator.Send(new DeleteProductRequest(item.Id), cancellationToken);
        }

        _logger.LogInformation("All random products deleted.");
    }
}

public sealed class RandomProductsSpec : Specification<Domain.Catalog.Product>
{
    public RandomProductsSpec() =>
        Query.Where(b => !string.IsNullOrEmpty(b.Name) && b.Name.Contains("Product Random"));
}