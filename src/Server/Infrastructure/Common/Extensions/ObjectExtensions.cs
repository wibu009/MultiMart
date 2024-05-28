using System.Reflection;

namespace MultiMart.Infrastructure.Common.Extensions;

public static class ObjectExtensions
{
    public static T SetPropertyValue<T>(this T obj, string propertyName, object value)
        where T : class
    {
        var propertyInfo = obj!.GetType().GetProperty(propertyName);
        if (propertyInfo == null) return obj;
        if (propertyInfo.PropertyType != value.GetType())
        {
            var targetType = propertyInfo.PropertyType.IsGenericType &&
                             propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                : propertyInfo.PropertyType;

            value = Convert.ChangeType(value, targetType!);
        }

        propertyInfo.SetValue(obj, value);
        return obj;
    }

    public static T? TryGetPropertyValue<T>(this object? obj, string propertyName, T? defaultValue = default)
        => obj?.GetType().GetRuntimeProperty(propertyName) is { } propertyInfo
            ? (T?)propertyInfo.GetValue(obj)
            : defaultValue;
}