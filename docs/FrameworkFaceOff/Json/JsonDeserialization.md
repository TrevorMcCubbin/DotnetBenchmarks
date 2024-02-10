| Method     | Runtime            | Categories                  | Count |      Mean | Ratio | Allocated | Alloc Ratio |
| ---------- | ------------------ | --------------------------- | ----- | --------: | ----: | --------: | ----------: |
| Newtonsoft | .NET Framework 4.8 | Deserialize Big Data        | 10000 | 33.928 ms |  1.00 |    4.8 MB |        1.00 |
| Microsoft  | .NET Framework 4.8 | Deserialize Big Data        | 10000 | 28.431 ms |  0.84 |   5.21 MB |        1.08 |
| Newtonsoft | .NET 8.0           | Deserialize Big Data        | 10000 | 21.099 ms |  0.63 |   4.52 MB |        0.94 |
| Microsoft  | .NET 8.0           | Deserialize Big Data        | 10000 | 12.996 ms |  0.39 |   5.01 MB |        1.04 |
|            |                    |                             |       |           |       |           |             |
| Newtonsoft | .NET Framework 4.8 | Deserialize Individual Data | 10000 | 29.528 ms |  1.00 |  29.96 MB |        1.00 |
| Microsoft  | .NET Framework 4.8 | Deserialize Individual Data | 10000 | 23.407 ms |  0.79 |   3.25 MB |        0.11 |
| Newtonsoft | .NET 8.0           | Deserialize Individual Data | 10000 | 15.203 ms |  0.51 |  29.55 MB |        0.99 |
| Microsoft  | .NET 8.0           | Deserialize Individual Data | 10000 |  8.211 ms |  0.28 |   3.05 MB |        0.10 |
