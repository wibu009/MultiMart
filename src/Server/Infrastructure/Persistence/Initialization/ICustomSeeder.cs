namespace MultiMart.Infrastructure.Persistence.Initialization;

public interface ICustomSeeder
{
    public int Order { get; }
    Task InitializeAsync(CancellationToken cancellationToken);
}