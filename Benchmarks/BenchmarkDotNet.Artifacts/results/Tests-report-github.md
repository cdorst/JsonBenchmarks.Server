``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.16299.371 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742186 Hz, Resolution=364.6726 ns, Timer=TSC
.NET Core SDK=2.1.300-preview2-008533
  [Host]     : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT
  Job-LGAMQP : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT

LaunchCount=25  

```
|                       Method |     Mean |     Error |    StdDev |   Median | Rank |
|----------------------------- |---------:|----------:|----------:|---------:|-----:|
|                    ByteArray | 806.4 us |  3.452 us |  30.01 us | 802.2 us |    4 |
|        ByteArrayActionResult | 737.4 us |  4.212 us |  42.36 us | 728.8 us |    1 |
|                          Csv | 741.9 us |  3.979 us |  39.01 us | 735.1 us |    2 |
|          JilJsonActionResult | 744.1 us |  3.340 us |  29.08 us | 741.6 us |    2 |
|   JilJsonActionResultNoNulls | 778.1 us |  3.307 us |  35.13 us | 774.6 us |    3 |
|             JilJsonFormatter | 883.2 us |  4.305 us |  46.66 us | 874.8 us |    5 |
| JilJsonFormatterActionResult | 878.6 us | 10.594 us | 106.59 us | 847.2 us |    5 |
|                  JsonDefault | 880.4 us |  4.977 us |  47.25 us | 876.8 us |    5 |
|      JsonDefaultActionResult | 890.5 us |  4.200 us |  43.85 us | 885.0 us |    6 |
