namespace MultiMart.Infrastructure.Auth;

public class StateData<T>
{
    public string TenantId { get; set; } = null!;
    public T Data { get; set; } = default!;
    public DateTimeOffset ExpireAt { get; set; } = DateTimeOffset.UtcNow.AddMinutes(5);

    public StateData()
    {
    }

    public StateData(string tenantId, T data, DateTimeOffset expireAt)
    {
        TenantId = tenantId;
        Data = data;
        ExpireAt = expireAt;
    }
}