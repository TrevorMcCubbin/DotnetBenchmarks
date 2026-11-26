using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetBenchmarks.Json.Model;

// System.Text.Json source-generation-backed serialization contexts
// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation

[JsonSourceGenerationOptions(JsonSerializerDefaults.General)]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(List<User>))]
internal partial class UserSerializerContext : JsonSerializerContext;

[JsonSourceGenerationOptions(JsonSerializerDefaults.General,
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(List<User>))]
internal partial class UserSerializerContextWithOptions : JsonSerializerContext;
