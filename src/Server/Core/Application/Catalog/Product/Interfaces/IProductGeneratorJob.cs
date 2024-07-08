using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Product.Interfaces;

public interface IProductGeneratorJob : IScopedService
{
    [DisplayName("Generate Random Product example job on Queue notDefault")]
    Task GenerateAsync(int nSeed, CancellationToken cancellationToken);

    [DisplayName("removes all random products created example job on Queue notDefault")]
    Task CleanAsync(CancellationToken cancellationToken);
}