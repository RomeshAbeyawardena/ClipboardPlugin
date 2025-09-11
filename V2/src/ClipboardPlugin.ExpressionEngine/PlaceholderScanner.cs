using ClipboardPlugin.Abstractions.Expressions;

namespace ClipboardPlugin.ExpressionEngine;

internal class PlaceholderScanner : IPlaceholderScanner
{
    private static bool Validate(string value, char startChar, char endChar)
    {
        var charSpan = value.AsSpan();
        var startCharOpen = false;
        var currentchar = charSpan[0];

        void GetNextChar(ReadOnlySpan<char> charSpan, int index)
        {
            currentchar = charSpan[index];
        }

        for (var i = 1; i < charSpan.Length; i++)
        {

            if (!startCharOpen && currentchar == startChar)
            {
                startCharOpen = true;
                GetNextChar(charSpan, i);
                continue;
            }
            else if (currentchar == startChar)
            {
                return false;
            }

            if (currentchar == endChar && startCharOpen)
            {
                startCharOpen = false;
                GetNextChar(charSpan, i);
                continue;
            }
            else if (currentchar == endChar)
            {
                return false;
            }

            GetNextChar(charSpan, i);
        }

        return true;
    }

    private static int[] GetRanges(string value, char character)
    {
        List<int> ranges = [];
        int lastIndex = -1;
        do
        {
            lastIndex = value.IndexOf(character, lastIndex + 1);
            if (lastIndex >= 0)
            {
                ranges.Add(lastIndex);
            }
        }
        while (lastIndex != -1);

        return [.. ranges];
    }

    public IEnumerable<Range> ScanRanges(string value, char startChar, char endChar)
    {

        var isValid = Validate(value, startChar, endChar);

        if (!isValid)
        {
            throw new IndexOutOfRangeException("Start and end ranges don't match, ensure one of each start and end character are supplied");
        }

        var startRanges = GetRanges(value, startChar);
        var endRanges = GetRanges(value, endChar);

        if (startRanges.Length != endRanges.Length)
        {
            throw new IndexOutOfRangeException("Start and end ranges don't match, ensure one of each start and end character are supplied");
        }

        var ranges = new List<Range>();
        for (int i = 0; i < startRanges.Length; i++)
        {
            ranges.Add(new Range(new Index(startRanges.ElementAt(i)),
                new Index(endRanges.ElementAt(i))));
        }

        return ranges;
    }

    public IEnumerable<(Range, string)> GetPlaceholderExpressions(string value, char startChar, char endChar)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        var indices = ScanRanges(value, startChar, endChar);
        var span = value.AsSpan();
        var placeholderExpressions = new List<(Range, string)>();
        foreach (var index in indices)
        {
            var (offset, length) = index.GetOffsetAndLength(span.Length);
            //captures everything inside of {}
            placeholderExpressions.Add((index, new string(span.Slice(offset + 1, length - 1))));
        }

        return placeholderExpressions;
    }
}
