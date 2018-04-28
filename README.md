# ASP.NET Core 2.1 API entity data-serialization benchmarks
Run `./run.ps1` at the repository root to repeat the experiment

## Question

What is the most performant method of data serialization for resources served by ASP.NET Core 2.1 APIs?

## Variables

Three categories of serialization are tested:

- JSON
- CSV
- byte[]

Jil IActionResult, Jil Formatter, and Newtonsoft (default) JSON performance is compared. byte[] object-result vs. IActionResult is also compared.

## Hypothesis

`Jil` is expected to be more performant than `Newtonsoft.Json` based on the [github.com/aspnet/benchmarks](https://github.com/aspnet/benchmarks) work

`StringBuilder` is expected to perform well given the benchmarking published in [this blog post](https://blogs.msdn.microsoft.com/dotnet/2018/04/18/performance-improvements-in-net-core-2-1/).

CSV should perform much better than JSON since it is schema-less.

Byte-array block copy should perform even better than CSV since it is also schema-less and contains less data

## Results

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

### API Response Time

In-memory ASP.NET Core web server Jil JSON endpoint responds 10.83% faster than default JsonFormatter endpoint

In-memory ASP.NET Core web server CSV endpoint responds 13.12% faster than default JsonFormatter endpoint

In-memory ASP.NET Core web server byte[] endpoint responds 13.77% faster than default JsonFormatter endpoint

## Conclusion

byte[] block-copy serialization outperformed other methods in terms of data-size, serialization runtime, and API request-response runtime.

The resultant Data Table indicates that the in-memory ASP.NET Core server is less performant in handling object results (with or without a Formatter attribute) than when handling IActionResults

## Future Research

Benchmark client-server scenario (w/ result de-serialization) using strongly-typed C# and TypeScript client SDKs
