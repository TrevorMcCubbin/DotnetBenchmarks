| Method     | Runtime            | Categories                | Count |      Mean | Ratio | Allocated | Alloc Ratio |
|------------|--------------------|---------------------------|-------|----------:|------:|----------:|------------:|
| Microsoft  | .NET Framework 4.8 | Serialize Big Data        | 10000 | 13.554 ms |  1.05 |   5.43 MB |        0.66 |
| Newtonsoft | .NET Framework 4.8 | Serialize Big Data        | 10000 | 12.976 ms |  1.00 |   8.18 MB |        1.00 |
| Newtonsoft | .NET 10.0          | Serialize Big Data        | 10000 |  7.585 ms |  0.58 |   8.07 MB |        0.99 |
| Microsoft  | .NET 10.0          | Serialize Big Data        | 10000 |  3.090 ms |  0.24 |   4.73 MB |        0.58 |
|            |                    |                           |       |           |       |           |             |
| Microsoft  | .NET Framework 4.8 | Serialize Individual Data | 10000 | 15.834 ms |  1.26 |   3.69 MB |        0.21 |
| Newtonsoft | .NET Framework 4.8 | Serialize Individual Data | 10000 | 12.583 ms |  1.00 |  17.69 MB |        1.00 |
| Newtonsoft | .NET 10.0          | Serialize Individual Data | 10000 |  5.487 ms |  0.44 |  17.22 MB |        0.97 |
| Microsoft  | .NET 10.0          | Serialize Individual Data | 10000 |  3.533 ms |  0.28 |   3.64 MB |        0.21 |
