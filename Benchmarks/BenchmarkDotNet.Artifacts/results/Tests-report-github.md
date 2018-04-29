``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.16299.371 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742185 Hz, Resolution=364.6727 ns, Timer=TSC
.NET Core SDK=2.1.300-preview2-008533
  [Host]     : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT
  Job-HVAAJX : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT

LaunchCount=25  

```
|                     Method |     Mean |    Error |   StdDev |   Median | Rank |
|--------------------------- |---------:|---------:|---------:|---------:|-----:|
|                  ByteArray | 924.1 us | 4.344 us | 35.79 us | 919.8 us |    4 |
|      ByteArrayActionResult | 817.8 us | 3.402 us | 28.05 us | 815.7 us |    1 |
|                        Csv | 826.7 us | 3.611 us | 29.77 us | 822.6 us |    2 |
|        JilJsonActionResult | 840.6 us | 3.579 us | 30.92 us | 837.2 us |    3 |
| JilJsonActionResultNoNulls | 840.2 us | 3.517 us | 28.78 us | 835.9 us |    3 |
|           JilJsonFormatter | 924.1 us | 3.894 us | 31.49 us | 920.8 us |    4 |
|                JsonDefault | 932.0 us | 4.035 us | 34.40 us | 928.3 us |    5 |
