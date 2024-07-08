using Bogus;
using Microsoft.Extensions.Logging;
using MultiMart.Domain.Catalog;
using MultiMart.Infrastructure.Persistence.Context;
using MultiMart.Infrastructure.Persistence.Initialization;

namespace MultiMart.Infrastructure.Catalog.Product;

public class ProductTypeSeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ProductTypeSeeder> _logger;

    public ProductTypeSeeder(ILogger<ProductTypeSeeder> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public int Order => 1;
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_db.ProductTypes.Any())
        {
            _logger.LogInformation("Started to Seed Products.");

            var productTypeNames = new List<string>()
            {
                "Book",
                "Tool",
            };

            var productTypes = new Faker<ProductType>()
                .RuleFor(p => p.Name, f => f.PickRandom(productTypeNames))
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .Generate(2);

            await _db.ProductTypes.AddRangeAsync(productTypes, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seeded Products.");
        }
    }
}