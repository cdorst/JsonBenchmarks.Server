# ASP.NET Core 2.1 API entity data-serialization benchmarks
Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment

## Question

What is the most performant method of data serialization for resources served by ASP.NET Core 2.1 APIs?

## Variables

Three categories of serialization are tested:

- JSON
- CSV
- byte[]

Jil IActionResult, Jil Formatter, and Newtonsoft (default) JSON performance is compared. byte[] object-result vs. IActionResult performance is also compared.

## Hypothesis

Based on the [github.com/cdorst/JsonBenchmarks](https://github.com/cdorst/JsonBenchmarks) work, performance is expected to rank in following order: byte[], CSV, JSON; IActionResult, object-result

## Results

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

### API Response Time

Jil `JsonActionResult` endpoint responds 18.32% faster than default JsonFormatter endpoint

Jil `JsonWithoutNullsactionResult` endpoint responds 13.15% faster than default JsonFormatter endpoint

`CsvActionResult` endpoint responds 18.67% faster than default JsonFormatter endpoint

`ByteArrayActionResult` endpoint responds 19.39% faster than default JsonFormatter endpoint

### API Response Content Length

Jil JSON without null values content length is 1.6x smaller (contains 63.16% as many bytes; 36.84% fewer bytes) than default JSON response

CSV content length is 7.6x smaller (contains 13.16% as many bytes; 86.84% fewer bytes) than default JSON response

byte[] content length is 14.3x smaller (contains 7.02% as many bytes; 92.98% fewer bytes) than default JSON response


## Conclusion

Entity byte[]-serialized IActionResult API endpoint outperforms other methods in terms of data-size, serialization runtime (per [github.com/cdorst/JsonBenchmarks](https://github.com/cdorst/JsonBenchmarks) results), and API server request-response runtime.

Results indicate that ASP.NET Core is less performant when handling object & ActionResult<T> results than when handling custom ActionResult types

## Future Research

Benchmark client-server scenario (w/ result de-serialization) using strongly-typed C# and TypeScript client SDKs

Benchmark TechEmpower `message = 'Hello, world!` scenario with Jil vs. JSON.NET and anonymous object vs. strongly-typed object vs. ActionResult<T>

