``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.16299.371 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742185 Hz, Resolution=364.6727 ns, Timer=TSC
.NET Core SDK=2.1.300-preview2-008533
  [Host]     : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT
  Job-EWGMZT : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT

LaunchCount=100  

```
|                Method |     Mean |    Error |   StdDev |   Median | Rank |
|---------------------- |---------:|---------:|---------:|---------:|-----:|
|             ByteArray | 842.9 us | 2.148 us | 36.32 us | 838.8 us |    4 |
| ByteArrayActionResult | 753.1 us | 1.531 us | 25.01 us | 751.2 us |    1 |
|                   Csv | 757.4 us | 1.646 us | 28.11 us | 753.6 us |    2 |
|   JilJsonActionResult | 773.1 us | 1.636 us | 28.47 us | 769.4 us |    3 |
|      JilJsonFormatter | 845.7 us | 1.815 us | 29.86 us | 843.0 us |    5 |
|           JsonDefault | 856.8 us | 1.882 us | 30.53 us | 854.1 us |    6 |
