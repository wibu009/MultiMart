using Bogus;
using Microsoft.Extensions.Logging;
using MultiMart.Infrastructure.Persistence.Context;
using MultiMart.Infrastructure.Persistence.Initialization;

namespace MultiMart.Infrastructure.Catalog.Product;

public class ProductSeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ProductSeeder> _logger;

    public ProductSeeder(ILogger<ProductSeeder> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public int Order => 3;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (!_db.Products.Any())
        {
            _logger.LogInformation("Started to Seed Products.");

            var productName = new List<string>()
            {
                "Harry Potter and the Philosopher's Stone",
                "The Lord of the Rings",
                "The Hobbit",
                "The Great Gatsby",
                "To Kill a Mockingbird",
                "1984",
                "Pride and Prejudice",
                "The Catcher in the Rye",
                "The Da Vinci Code",
                "The Alchemist"
            };

            var products = new Faker<Domain.Catalog.Product>()
                .RuleFor(p => p.Name, f => f.PickRandom(productName))
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Rate, f => f.Random.Decimal(1, 5))
                .RuleFor(p => p.ImagePath, f => f.Image.PicsumUrl())
                .RuleFor(p => p.BrandId, f => f.PickRandom(_db.Brands.Select(b => b.Id).ToList()))
                .RuleFor(p => p.ProductTypeId, f => f.PickRandom(_db.ProductTypes.Select(t => t.Id).ToList()))
                .Generate(10);

            await _db.Products.AddRangeAsync(products, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seeded Products.");
        }
    }
}