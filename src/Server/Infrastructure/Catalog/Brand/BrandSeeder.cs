using System.Reflection;
using Bogus;
using Microsoft.Extensions.Logging;
using MultiMart.Infrastructure.Persistence.Context;
using MultiMart.Infrastructure.Persistence.Initialization;

namespace MultiMart.Infrastructure.Catalog.Brand;

public class BrandSeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<BrandSeeder> _logger;

    public BrandSeeder(ILogger<BrandSeeder> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public int Order => 1;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (!_db.Brands.Any())
        {
            _logger.LogInformation("Started to Seed Brands.");

            var brands = new Faker<Domain.Catalog.Brand>()
                .RuleFor(b => b.Name, f => f.Company.CompanyName())
                .RuleFor(b => b.Description, f => f.Company.CompanySuffix())
                .Generate(10);

            await _db.Brands.AddRangeAsync(brands, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Brands.");
        }
    }
}