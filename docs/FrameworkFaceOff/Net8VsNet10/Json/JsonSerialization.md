| Method     | Runtime   | Categories                | Count |     Mean | Ratio | Allocated | Alloc Ratio |
|------------|-----------|---------------------------|-------|---------:|------:|----------:|------------:|
| Newtonsoft | .NET 8.0  | Serialize Big Data        | 10000 | 8.354 ms |  1.00 |   8.07 MB |        1.00 |
| Newtonsoft | .NET 10.0 | Serialize Big Data        | 10000 | 8.097 ms |  0.97 |   8.09 MB |        1.00 |
| Microsoft  | .NET 8.0  | Serialize Big Data        | 10000 | 4.505 ms |  0.54 |   4.59 MB |        0.57 |
| Microsoft  | .NET 10.0 | Serialize Big Data        | 10000 | 3.245 ms |  0.39 |   4.78 MB |        0.59 |
|            |           |                           |       |          |       |           |             |
| Newtonsoft | .NET 8.0  | Serialize Individual Data | 10000 | 7.322 ms |  1.00 |  17.14 MB |        1.00 |
| Newtonsoft | .NET 10.0 | Serialize Individual Data | 10000 | 6.531 ms |  0.89 |  17.22 MB |        1.00 |
| Microsoft  | .NET 8.0  | Serialize Individual Data | 10000 | 4.382 ms |  0.60 |   3.64 MB |        0.21 |
| Microsoft  | .NET 10.0 | Serialize Individual Data | 10000 | 4.034 ms |  0.55 |   3.64 MB |        0.21 |
