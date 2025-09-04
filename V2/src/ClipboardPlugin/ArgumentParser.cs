namespace ClipboardPlugin;

public static class ArgumentParser
{
    public static T AsModel<T>(this IDictionary<string, object> values, T model)
        where T : class
    {
        var errorModel = new Dictionary<string, Exception>();
        
        var properties = typeof(T).GetProperties();

        foreach (var (key, value) in values)
        {
            try
            {
                var property = properties.FirstOrDefault(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase) && x.CanWrite);

                if (property is null)
                {
                    continue;
                }

                property.SetValue(model, Convert.ChangeType(value, property.PropertyType));
            }
            catch (Exception ex)
            {
                errorModel.Add(key, ex);
                continue;
            }
        }

        return model;
    }

    public static T AsModel<T>(this IDictionary<string, object> values)
        where T : class, new()
    {
       return values.AsModel(new T());
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
