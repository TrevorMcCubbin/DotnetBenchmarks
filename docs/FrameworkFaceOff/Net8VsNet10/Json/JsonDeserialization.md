| Method     | Runtime   | Categories                  | Count |      Mean | Ratio | Allocated | Alloc Ratio |
|------------|-----------|-----------------------------|-------|----------:|------:|----------:|------------:|
| Newtonsoft | .NET 8.0  | Deserialize Big Data        | 10000 | 20.963 ms |  1.01 |   4.52 MB |        1.00 |
| Newtonsoft | .NET 10.0 | Deserialize Big Data        | 10000 | 16.859 ms |  0.81 |   4.52 MB |        1.00 |
| Microsoft  | .NET 8.0  | Deserialize Big Data        | 10000 | 12.709 ms |  0.61 |   3.42 MB |        0.76 |
| Microsoft  | .NET 10.0 | Deserialize Big Data        | 10000 | 10.633 ms |  0.51 |   3.55 MB |        0.78 |
|            |           |                             |       |           |       |           |             |
| Newtonsoft | .NET 8.0  | Deserialize Individual Data | 10000 | 14.663 ms |  1.00 |  29.49 MB |        1.00 |
| Newtonsoft | .NET 10.0 | Deserialize Individual Data | 10000 | 11.157 ms |  0.76 |  29.52 MB |        1.00 |
| Microsoft  | .NET 8.0  | Deserialize Individual Data | 10000 |  6.410 ms |  0.44 |   3.05 MB |        0.10 |
| Microsoft  | .NET 10.0 | Deserialize Individual Data | 10000 |  5.279 ms |  0.36 |   3.05 MB |        0.10 |
