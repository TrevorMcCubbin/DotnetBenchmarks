| Method     | Runtime            | Categories                | Count |      Mean | Ratio | Allocated | Alloc Ratio |
| ---------- | ------------------ | ------------------------- | ----- | --------: | ----: | --------: | ----------: |
| Newtonsoft | .NET Framework 4.8 | Serialize Big Data        | 10000 | 17.736 ms |  1.00 |   8.16 MB |        1.00 |
| Microsoft  | .NET Framework 4.8 | Serialize Big Data        | 10000 | 17.481 ms |  0.99 |   5.42 MB |        0.66 |
| Newtonsoft | .NET 8.0           | Serialize Big Data        | 10000 |  9.821 ms |  0.55 |   8.07 MB |        0.99 |
| Microsoft  | .NET 8.0           | Serialize Big Data        | 10000 |  5.909 ms |  0.33 |   3.42 MB |        0.42 |
|            |                    |                           |       |           |       |           |             |
| Microsoft  | .NET Framework 4.8 | Serialize Individual Data | 10000 | 17.734 ms |  1.02 |   3.69 MB |        0.21 |
| Newtonsoft | .NET Framework 4.8 | Serialize Individual Data | 10000 | 17.293 ms |  1.00 |  17.69 MB |        1.00 |
| Newtonsoft | .NET 8.0           | Serialize Individual Data | 10000 |  7.670 ms |  0.44 |  17.14 MB |        0.97 |
| Microsoft  | .NET 8.0           | Serialize Individual Data | 10000 |  5.636 ms |  0.33 |   3.64 MB |        0.21 |
