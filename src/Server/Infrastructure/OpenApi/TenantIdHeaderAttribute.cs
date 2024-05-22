using BookStack.Shared.Multitenancy;
using NJsonSchema;

namespace BookStack.Infrastructure.OpenApi;

public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
{
    public TenantIdHeaderAttribute()
        : base(
            MultitenancyConstants.TenantIdName,
            "Input your tenant Id to access this API",
            string.Empty,
            JsonObjectType.String,
            true)
    {
    }
}
