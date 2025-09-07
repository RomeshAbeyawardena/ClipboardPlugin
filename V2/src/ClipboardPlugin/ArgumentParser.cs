using System.Reflection;

namespace ClipboardPlugin;

public record PropertyAliases(IEnumerable<string> Aliases, PropertyInfo Property);

public static class ArgumentParser
{
    private static object ChangeType(object value, Type propertyType)
    {
        if ((propertyType == typeof(int) || propertyType == typeof(int?))
            && int.TryParse(value.ToString(), out var result))
        {
            return result;
        }

        if ((propertyType == typeof(decimal) || propertyType == typeof(decimal?))
            && decimal.TryParse(value.ToString(), out var decResult))
        {
            return decResult;
        }

        if ((propertyType == typeof(bool) || propertyType == typeof(bool?))
            && bool.TryParse(value.ToString(), out var boolResult))
        {
            return boolResult;
        }

        return value;
    }

    public static T AsModel<T>(this IDictionary<string, object> values, T model, out IReadOnlyDictionary<string, Exception> errors)
        where T : class
    {
        var errorModel = new Dictionary<string, Exception>();
        
        var properties = typeof(T).GetProperties();
        var propertyAlises = new List<PropertyAliases>();
        foreach (var property in properties)
        {
            var argumentAttribute = property.GetCustomAttribute<ArgumentAttribute>();

            if (argumentAttribute is null)
            {
                continue;
            }

            propertyAlises.Add(new PropertyAliases(argumentAttribute.Aliases, property));
        }

        foreach (var (key, value) in values)
        {
            try
            {
                var property = properties.FirstOrDefault(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase) && x.CanWrite);
                PropertyAliases? propertyAlias = propertyAlises.FirstOrDefault(x => x.Aliases.Any(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
                if (property is null && propertyAlias is null)
                {
                    continue;
                }
                var prop = (property ?? propertyAlias?.Property ?? throw new NullReferenceException());
                prop.SetValue(model, ChangeType(value, prop.PropertyType));
            }
            catch (Exception ex)
            {
                errorModel.Add(key, ex);
                continue;
            }
        }

        errors = errorModel;
        return model;
    }

    public static T AsModel<T>(this IDictionary<string, object> values, out IReadOnlyDictionary<string, Exception> errors)
        where T : class, new()
    {
       return values.AsModel(new T(), out errors);
    }

    public static IDictionary<string, object> ToDictionary(this string [] args)
    {
        var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        string? currentKey = null;
        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                if (currentKey != null)
                {
                    dict[currentKey] = true; // Flag without value
                }
                currentKey = arg[2..].Trim('-');
            }
            else if (arg.StartsWith('-'))
            {
                if (currentKey != null)
                {
                    dict[currentKey] = true; // Flag without value
                }
                currentKey = arg[1..].Trim('-');
            }
            else
            {
                if (currentKey != null)
                {
                    dict[currentKey] = arg; // Key-value pair
                    currentKey = null;
                }
                else
                {
                    // Positional argument without a key
                    dict[arg] = true; // Treat as a flag
                }
            }
        }
        if (currentKey != null)
        {
            dict[currentKey] = true; // Last flag without value
        }
        return dict;
    }

}
