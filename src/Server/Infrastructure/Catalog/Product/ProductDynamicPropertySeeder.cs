using Microsoft.Extensions.Logging;
using MultiMart.Infrastructure.Persistence.Context;
using MultiMart.Infrastructure.Persistence.Initialization;

namespace MultiMart.Infrastructure.Catalog.Product;

public class ProductDynamicPropertySeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ProductDynamicPropertySeeder> _logger;

    public ProductDynamicPropertySeeder(ILogger<ProductDynamicPropertySeeder> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public int Order => 2;
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_db.ProductDynamicProperties.Any())
        {
            _logger.LogInformation("Started to Seed Product Dynamic Properties.");

            var productDynamicProperties = new List<Domain.Catalog.ProductDynamicProperty>
            {
                new()
                {
                    Name = "Author",
                    Type = nameof(String),
                    Description = "The author of the book",
                    IsRequired = true,
                    ProductTypeId = _db.ProductTypes.First(t => t.Name == "Book").Id
                },
                new()
                {
                    Name = "Publisher",
                    Type = nameof(String),
                    Description = "The publisher of the book",
                    IsRequired = true,
                    ProductTypeId = _db.ProductTypes.First(t => t.Name == "Book").Id
                },
                new()
                {
                    Name = "ISBN",
                    Type = nameof(String),
                    Description = "The ISBN of the book",
                    IsRequired = true,
                    ProductTypeId = _db.ProductTypes.First(t => t.Name == "Book").Id
                },
                new()
                {
                    Name = "Pages",
                    Type = nameof(Int32),
                    Description = "The number of pages in the book",
                    IsRequired = true,
                    ProductTypeId = _db.ProductTypes.First(t => t.Name == "Book").Id
                },
                new()
                {
                    Name = "Language",
                    Type = nameof(String),
                    Description = "The language of the book",
                    IsRequired = true,
                    ProductTypeId = _db.ProductTypes.First(t => t.Name == "Book").Id
                },
                new()
                {
                    Name = "PublicationDate",
                    Type = nameof(DateTime),
                    Description = "The publication date of the book",
                    IsRequired = true,
                    ProductTypeId = _db.ProductTypes.First(t => t.Name == "Book").Id
                }
            };

            await _db.ProductDynamicProperties.AddRangeAsync(productDynamicProperties, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seeded Product Dynamic Properties.");
        }
    }
}