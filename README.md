# .NET Benchmarks

## Overview

Welcome to my .NET Benchmarks repository! This repository is dedicated to benchmarking performance in .NET applications. Whether you're optimizing code, comparing different implementations, or evaluating the impact of changes, this collection of benchmarks will help you make informed decisions about the performance of your .NET projects.

## Table of Contents

- [Getting Started](#getting-started)
  - [Running Benchmarks](#running-benchmarks)
- [Benchmark Environments](#benchmark-environments)
- [Benchmarking Guidelines](#benchmarking-guidelines)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

To get started with .NET benchmarks, follow these steps:

1. **Clone the Repository:**

```bash
git clone https://github.com/TrevorMcCubbin/DotnetBenchmarks.git
cd DotnetBenchmarks
```

Install .NET 10 SDK:
Make sure you have the .NET Core SDK installed.

### Running Benchmarks

To run Json Benchmarks in .Net 10:

```bash
dotnet run -c release --project .\src\DotnetBenchmarks.Json\DotnetBenchmarks.Json.csproj
```

To run a comparison of various benchmarks between .Net Framework 4.8 and .Net 10:

```bash
dotnet run -c release --project .\src\DotnetBenchmarks.FrameworkFaceOff\DotnetBenchmarks.FrameworkFaceOff.csproj --framework net10.0 -- --filter "*DotnetBenchmarks.FrameworkFaceOff.Json*"
```

To run a comparison of various benchmarks between .Net 8 and .Net 10:

```bash
dotnet run -c release --project .\src\DotnetBenchmarks.FrameworkFaceOff\DotnetBenchmarks.FrameworkFaceOff.csproj --framework net10.0 -- --filter "*Net8VsNet10*"
```

### Formatting C# code

Restore the local tools and run CSharpier from the repository root:

```bash
dotnet tool restore
dotnet csharpier format .
```

To check formatting without changing files:

```bash
dotnet csharpier check .
```

Explore Results:
After running the benchmarks, explore the generated reports and analyze the performance metrics.

## Benchmark Environments

Benchmark results in this repository were captured on the following machines:

| .NET Version | Machine          | CPU                  | RAM                   |
|--------------|------------------|----------------------|-----------------------|
| .NET 8 & 9   | Dell XPS 15 7590 | Intel Core i7-9750H  | 16 GB DDR4 @ 2666 MHz |
| .NET 10      | Dell XPS 15 9510 | Intel Core i7-11800H | 16 GB DDR4 @ 3200 MHz |

The .NET 8 and .NET 9 results (including the .NET Framework 4.8 vs .NET 8 face-off) were run on the older machine, while
all .NET 10 results (including the .NET Framework 4.8 vs .NET 10 and .NET 8 vs .NET 10 face-offs) were run on the newer
machine. Keep this in mind when comparing results across .NET versions, as they are not directly comparable across
machines.

## Benchmarking Guidelines

When contributing benchmarks or analyzing results, please follow these guidelines:

    Isolate Benchmarks:
    Ensure that each benchmark is isolated and representative of a specific scenario or functionality.

    Repeatable Results:
    Benchmarks should be repeatable, providing consistent results across multiple runs.

    Documentation:
    Clearly document the purpose and methodology of each benchmark. Include relevant details about the test environment.

    Avoid Premature Optimization:
    Focus on meaningful optimizations rather than premature optimizations. Consider the trade-offs between readability and performance.

    Report Issues:
    If you encounter any issues or unexpected behavior in benchmarks, please report them using the GitHub Issues page.

## Contributing

We welcome contributions! To contribute to this repository:

- Fork the repository and create a new branch for your feature or bug fix.
- Make your changes and ensure all tests and benchmarks pass.
- Submit a pull request with a clear description of your changes.

## License

This project is licensed under the GNU 3 License - see the [LICENSE](LICENSE.txt) file for details.

Happy benchmarking! 🚀
