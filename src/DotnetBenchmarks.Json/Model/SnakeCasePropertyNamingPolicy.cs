using System.Text.Json;

namespace DotnetBenchmarks.Json.Model;

public class SnakeCasePropertyNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        var upperCaseLength = name.Count(t => t is >= 'A' and <= 'Z' && t != name[0]);

        var bufferSize = name.Length + upperCaseLength;

        Span<char> buffer = new char[bufferSize];

        var bufferPosition = 0;

        var namePosition = 0;

        while (bufferPosition < buffer.Length)
        {
            if (namePosition > 0 && name[namePosition] >= 'A' && name[namePosition] <= 'Z')
            {
                buffer[bufferPosition] = '_';
                buffer[bufferPosition + 1] = name[namePosition];
                bufferPosition += 2;
                namePosition++;
                continue;
            }

            buffer[bufferPosition] = name[namePosition];

            bufferPosition++;

            namePosition++;
        }

        return buffer.ToString();
    }
}
