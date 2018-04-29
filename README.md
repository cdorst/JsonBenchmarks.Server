# ASP.NET Core 2.1 API entity data-serialization benchmarks
Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment

## Question

What is the most performant method of data serialization for resources served by ASP.NET Core 2.1 APIs?

## Variables

Three categories of serialization are tested:

- JSON
- CSV
- byte[]

Jil IActionResult, Jil Formatter, and Newtonsoft (default) JSON performance is compared. byte[] object-result vs. IActionResult is also compared.

## Hypothesis

Based on the [github.com/cdorst/JsonBenchmarks](https://github.com/cdorst/JsonBenchmarks) work, performance is expected to rank in descending order: byte[], CSV, JSON; IActionResult, object-result

## Results

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

### API Response Time

Jil JSON endpoint responds 10.87% faster than default JsonFormatter endpoint

Jil JSON without null values endpoint responds 10.93% faster than default JsonFormatter endpoint

CSV endpoint responds 12.74% faster than default JsonFormatter endpoint

byte[] endpoint responds 13.96% faster than default JsonFormatter endpoint

### API Response Content Length

Jil JSON without null values content length is 1.6x smaller (contains 63.16% as many bytes; 36.84% fewer bytes) than default JSON response

CSV content length is 7.6x smaller (contains 13.16% as many bytes; 86.84% fewer bytes) than default JSON response

byte[] content length is 14.3x smaller (contains 7.02% as many bytes; 92.98% fewer bytes) than default JSON response


## Conclusion

byte[]-serialized IActionResult outperformed other methods in terms of data-size, serialization runtime, and API server request-response runtime.

Results indicate that ASP.NET Core is less performant when handling object results than when handling IActionResults

## Future Research

Benchmark client-server scenario (w/ result de-serialization) using strongly-typed C# and TypeScript client SDKs

