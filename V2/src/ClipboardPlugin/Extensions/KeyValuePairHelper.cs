namespace ClipboardPlugin.Extensions;

internal class KeyValuePairHelper
{
    internal static char? DetermineSeparator(string value, params char[] c)
    {
        char? currentSeparator;
        int index = 0;
        do
        {
            currentSeparator = c[index++];
            if (!value.Contains(currentSeparator.Value))
            {
                currentSeparator = null;
            }
        }
        while (index < c.Length && currentSeparator is null);
        return currentSeparator;
    }

    internal static IEnumerable<(string, string?)?> GetKeyValuePairs(IEnumerable<string> values, params char[] c)
    {
        if (!values.Any())
        {
            return [];
        }

        var currentSeparator = DetermineSeparator(values.First(), c);

        if (currentSeparator is null)
        {
            return [];
        }

        return values.Select(x => GetKeyValuePair(x, currentSeparator));
    }

    internal static (string, string?)? GetKeyValuePair(string value, char? currentSeparator)
    {
        if (!currentSeparator.HasValue)
        {
            return null;
        }

        var definition = value.Split(currentSeparator.Value);

        if (definition.Length == 2)
        {
            var key = definition[0];
            var val = definition[1];

            return (key, val);
        }

        return null;
    }
}
