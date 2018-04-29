using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Benchmarks.Constants;

namespace Benchmarks
{
    [SimpleJob(100)]
    [RPlotExporter, RankColumn]
    public class Tests
    {
        [Benchmark]
        public async Task<HttpResponseMessage> ByteArray()
            => await Server.GetAsync(ByteArrayRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> ByteArrayActionResult()
            => await Server.GetAsync(ByteArrayActionResultRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> Csv()
            => await Server.GetAsync(CsvRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> JilJsonActionResult()
            => await Server.GetAsync(JilJsonActionResultRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> JilJsonFormatter()
            => await Server.GetAsync(JilJsonFormatterRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> JsonDefault()
            => await Server.GetAsync(JsonDefaultRoute);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Tests>();
            var dataTable = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "BenchmarkDotNet.Artifacts", "results", "Tests-report-github.md"));
            var resultSummary = GetResultSummary(dataTable).Split(Environment.NewLine);
            foreach (var line in resultSummary) Console.WriteLine(line);
            var readme = new StringBuilder()
                .Append("# ASP.NET Core 2.1 API entity data-serialization benchmarks")
                .AppendLine()
                .AppendLine("Run `./run.ps1` at the repository root to repeat the experiment")
                .AppendLine()
                .AppendLine("## Question")
                .AppendLine()
                .AppendLine("What is the most performant method of data serialization for resources served by ASP.NET Core 2.1 APIs?")
                .AppendLine()
                .AppendLine("## Variables")
                .AppendLine()
                .AppendLine("Three categories of serialization are tested:")
                .AppendLine()
                .AppendLine("- JSON")
                .AppendLine("- CSV")
                .AppendLine("- byte[]")
                .AppendLine()
                .AppendLine("Jil IActionResult, Jil Formatter, and Newtonsoft (default) JSON performance is compared. byte[] object-result vs. IActionResult is also compared.")
                .AppendLine()
                .AppendLine("## Hypothesis")
                .AppendLine()
                .AppendLine("Based on the [github.com/cdorst/JsonBenchmarks](https://github.com/cdorst/JsonBenchmarks) work, performance is expected to rank in descending order: byte[], CSV, JSON; IActionResult, object-result")
                .AppendLine()
                .AppendLine("## Results")
                .AppendLine();
            foreach (var line in dataTable) readme.AppendLine(line);
            readme.AppendLine();
            foreach (var line in resultSummary) readme.AppendLine(line);
            readme
                .AppendLine("## Conclusion")
                .AppendLine()
                .AppendLine("byte[]-serialized IActionResult outperformed other methods in terms of data-size, serialization runtime, and API server request-response runtime.")
                .AppendLine()
                .AppendLine("The resultant Data Table indicates that the ASP.NET Core server is less performant in handling object results (with or without a Formatter attribute) than when handling IActionResults")
                .AppendLine()
                .AppendLine("## Future Research")
                .AppendLine()
                .AppendLine("Benchmark client-server scenario (w/ result de-serialization) using strongly-typed C# and TypeScript client SDKs")
                .AppendLine();
            File.WriteAllText("../README.md", readme.ToString());
        }

        private static string GetResultSummary(string[] dataTable)
        {
            var (apiJsonNetResponseTime, apiJilJsonResponseTime, apiCsvResponseTime, apiBytesResponseTime) = parseApiResponseTimes(dataTable);
            return new StringBuilder()
                .AppendLine("### API Response Time")
                .AppendLine()
                .AppendLine(CompareResponseTime(apiJsonNetResponseTime, apiJilJsonResponseTime, "Jil JSON"))
                .AppendLine()
                .AppendLine(CompareResponseTime(apiJsonNetResponseTime, apiCsvResponseTime, "CSV"))
                .AppendLine()
                .AppendLine(CompareResponseTime(apiJsonNetResponseTime, apiBytesResponseTime, "byte[]"))
                .ToString();
        }

        private static string CompareResponseTime(decimal slowResponseTime, decimal fastResponseTime, string label)
            => $"{label} endpoint responds {(slowResponseTime / fastResponseTime - 1).ToString("p")} faster than default JsonFormatter endpoint";

        private static (decimal apiJsonNetResponseTime, decimal apiJilJsonResponseTime, decimal apiCsvResponseTime, decimal apiBytesResponseTime) parseApiResponseTimes(string[] dataTable)
            => (
            parseResponseTime(dataTable, nameof(Tests.JsonDefault)),
            parseResponseTime(dataTable, nameof(Tests.JilJsonActionResult)),
            parseResponseTime(dataTable, nameof(Tests.Csv)),
            parseResponseTime(dataTable, nameof(Tests.ByteArrayActionResult)));

        private static decimal parseResponseTime(string[] dataTable, string method)
            => decimal.Parse(dataTable.First(line => line.Contains(method)).Split('|').Skip(2).First().Replace(",", "").Replace("ns", "").Replace("us", "").Replace("ms", "").Trim());
    }
}
