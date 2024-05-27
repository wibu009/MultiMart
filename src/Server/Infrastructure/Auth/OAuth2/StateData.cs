using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BookStack.Infrastructure.Auth.OAuth2;

public class StateData<T>
{
    public string TenantId { get; set; }
    public T Data { get; set; }
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

    public string ToBase64String()
    {
        string json = JsonConvert.SerializeObject(this);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public static StateData<T> FromBase64String(string base64)
    {
        byte[] bytes = Convert.FromBase64String(base64);
        string json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<StateData<T>>(json) ?? throw new InvalidOperationException();
    }
}