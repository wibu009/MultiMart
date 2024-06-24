namespace MultiMart.Infrastructure.OpenApi;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SwaggerHeaderAttribute : Attribute
{
    public string HeaderName { get; }

    public string? Description { get; }

    //supported types: string, number, integer, boolean, array
    public string Type { get; set; }
    public string? DefaultValue { get; set; }
    public IEnumerable<string>? Enum { get; set; }
    public bool IsRequired { get; set; }

    protected SwaggerHeaderAttribute(string headerName,
        string? description = null,
        string? defaultValue = null,
        string type = "string",
        bool isRequired = false,
        IEnumerable<string>? enumerable = null)
    {
        HeaderName = headerName;
        Description = description;
        Type = type;
        DefaultValue = defaultValue;
        IsRequired = isRequired;
        Enum = enumerable;
    }
}