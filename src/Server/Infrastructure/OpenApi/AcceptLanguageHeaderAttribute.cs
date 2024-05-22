using NJsonSchema;

namespace BookStack.Infrastructure.OpenApi;

public class AcceptLanguageHeaderAttribute : SwaggerHeaderAttribute
{
    public AcceptLanguageHeaderAttribute()
        : base(
            "Accept-Language",
            "Language support (English and Vietnamese)",
            "en-US",
            JsonObjectType.String,
            false,
            new[] { "en-US", "en", "fr", "fr-FR", "de", "de-DE", "it", "it-IT", "pt", "nl", "nl-NL", "vi", "vi-VN"})
    {
    }
}