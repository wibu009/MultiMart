using Microsoft.Extensions.Logging;
using MultiMart.Infrastructure.Persistence.Context;
using MultiMart.Infrastructure.Persistence.Initialization;

namespace MultiMart.Infrastructure.Catalog.Product;

public class ProductDynamicPropertyValueSeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ProductDynamicPropertyValueSeeder> _logger;

    public ProductDynamicPropertyValueSeeder(ILogger<ProductDynamicPropertyValueSeeder> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public int Order => 4;
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_db.ProductDynamicPropertyValues.Any())
        {
            _logger.LogInformation("Started to Seed Product Dynamic Property Values.");

            var productDynamicPropertyValues = new List<Domain.Catalog.ProductDynamicPropertyValue>
            {
                new()
                {
                    Value = "J.K. Rowling",
                    ProductId = _db.Products.First(p => p.Name == "Harry Potter and the Philosopher's Stone").Id,
                    DynamicPropertyId = _db.ProductDynamicProperties.First(p => p.Name == "Author").Id
                },
                new()
                {
                    Value = "Bloomsbury",
                    ProductId = _db.Products.First(p => p.Name == "Harry Potter and the Philosopher's Stone").Id,
                    DynamicPropertyId = _db.ProductDynamicProperties.First(p => p.Name == "Publisher").Id
                },
                new()
                {
                    Value = "978-0747532743",
                    ProductId = _db.Products.First(p => p.Name == "Harry Potter and the Philosopher's Stone").Id,
                    DynamicPropertyId = _db.ProductDynamicProperties.First(p => p.Name == "ISBN").Id
                },
                new()
                {
                    Value = "223",
                    ProductId = _db.Products.First(p => p.Name == "Harry Potter and the Philosopher's Stone").Id,
                    DynamicPropertyId = _db.ProductDynamicProperties.First(p => p.Name == "Pages").Id
                },
                new()
                {
                    Value = "English",
                    ProductId = _db.Products.First(p => p.Name == "Harry Potter and the Philosopher's Stone").Id,
                    DynamicPropertyId = _db.ProductDynamicProperties.First(p => p.Name == "Language").Id
                },
                new()
                {
                    Value = "J.R.R. Tolkien",
                    ProductId = _db.Products.First(p => p.Name == "The Lord of the Rings").Id,
                    DynamicPropertyId = _db.ProductDynamicProperties.First(p => p.Name == "Author").Id
                },
                new()
                {
                    Value = "George Allen & Unwin",
                    ProductId = _db.Products.First(p => p.Name == "The Lord of the Rings").Id,
                    DynamicPropertyId = _db.ProductDynamicProperties.First(p => p.Name == "Publisher").Id
                },
            };

            await _db.ProductDynamicPropertyValues.AddRangeAsync(productDynamicPropertyValues, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seeded Product Dynamic Property Values.");
        }
    }
}