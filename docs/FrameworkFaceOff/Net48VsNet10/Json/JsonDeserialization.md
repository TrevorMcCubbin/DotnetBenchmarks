| Method     | Runtime            | Categories                  | Count |      Mean | Ratio | Allocated | Alloc Ratio |
|------------|--------------------|-----------------------------|-------|----------:|------:|----------:|------------:|
| Newtonsoft | .NET Framework 4.8 | Deserialize Big Data        | 10000 | 29.585 ms |  1.00 |    4.8 MB |        1.00 |
| Microsoft  | .NET Framework 4.8 | Deserialize Big Data        | 10000 | 25.340 ms |  0.86 |   5.21 MB |        1.08 |
| Newtonsoft | .NET 10.0          | Deserialize Big Data        | 10000 | 16.506 ms |  0.56 |   4.52 MB |        0.94 |
| Microsoft  | .NET 10.0          | Deserialize Big Data        | 10000 |  8.069 ms |  0.27 |   3.55 MB |        0.74 |
|            |                    |                             |       |           |       |           |             |
| Newtonsoft | .NET Framework 4.8 | Deserialize Individual Data | 10000 | 25.604 ms |  1.00 |  29.92 MB |        1.00 |
| Microsoft  | .NET Framework 4.8 | Deserialize Individual Data | 10000 | 21.765 ms |  0.85 |   3.25 MB |        0.11 |
| Newtonsoft | .NET 10.0          | Deserialize Individual Data | 10000 | 10.318 ms |  0.40 |  29.49 MB |        0.99 |
| Microsoft  | .NET 10.0          | Deserialize Individual Data | 10000 |  5.454 ms |  0.21 |   3.05 MB |        0.10 |
