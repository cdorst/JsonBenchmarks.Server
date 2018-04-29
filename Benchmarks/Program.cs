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
    [SimpleJob(25)]
    [RPlotExporter, RankColumn]
    public class Tests
    {
        [Benchmark]
        public async Task<HttpResponseMessage> ByteArray()
            => await Test(ByteArrayRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> ByteArrayActionResult()
            => await Test(ByteArrayActionResultRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> Csv()
            => await Test(CsvRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> JilJsonActionResult()
            => await Test(JilJsonActionResultRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> JilJsonActionResultNoNulls()
            => await Test(JilJsonActionResultNoNullsRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> JilJsonFormatter()
            => await Test(JilJsonFormatterRoute);

        [Benchmark]
        public async Task<HttpResponseMessage> JsonDefault()
            => await Test(JsonDefaultRoute);

        private async Task<HttpResponseMessage> Test(string route)
            => await Server.GetAsync(route);
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Tests>();
            var dataTable = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "BenchmarkDotNet.Artifacts", "results", "Tests-report-github.md"));
            var resultSummary = (await GetResultSummary(dataTable)).Split(Environment.NewLine);
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

        private static async Task<string> GetResultSummary(string[] dataTable)
        {
            var (apiJsonNetResponseTime, apiJilJsonResponseTime, apiJilNoNullsResponseTime, apiCsvResponseTime, apiBytesResponseTime) = parseApiResponseTimes(dataTable);
            var (apiJsonNetContentLength, apiJilJsonContentLength, apiCsvContentLength, apiBytesContentLength) = await testApiResponseContentLengths();
            return new StringBuilder()
                .AppendLine("### API Response Time")
                .AppendLine()
                .AppendLine(CompareResponseTime(apiJsonNetResponseTime, apiJilJsonResponseTime, "Jil JSON"))
                .AppendLine()
                .AppendLine(CompareResponseTime(apiJsonNetResponseTime, apiJilNoNullsResponseTime, "Jil JSON without null values"))
                .AppendLine()
                .AppendLine(CompareResponseTime(apiJsonNetResponseTime, apiCsvResponseTime, "CSV"))
                .AppendLine()
                .AppendLine(CompareResponseTime(apiJsonNetResponseTime, apiBytesResponseTime, "byte[]"))
                .AppendLine()
                .AppendLine("### API Response Content Length")
                .AppendLine()
                .AppendLine(CompareResponseContentLength(apiJsonNetContentLength, apiJilJsonContentLength, "Jil JSON without null values"))
                .AppendLine()
                .AppendLine(CompareResponseContentLength(apiJsonNetContentLength, apiCsvContentLength, "CSV"))
                .AppendLine()
                .AppendLine(CompareResponseContentLength(apiJsonNetContentLength, apiBytesContentLength, "byte[]"))
                .AppendLine()
                .ToString();
        }

        private static string CompareResponseContentLength(long? largeResponseLength, long? smallResponseLength, string label)
        {
            var large = d(largeResponseLength);
            var small = d(smallResponseLength);
            return $"{label} content length is {(large / small).ToString("N1")}x smaller (contains {(small / large).ToString("p")} as many bytes; {((large - small) / large).ToString("p")} fewer bytes) than default JSON response";
        }

        private static decimal d(long? number) => Decimal.Parse(number?.ToString() ?? "0");

        private static string CompareResponseTime(decimal slowResponseTime, decimal fastResponseTime, string label)
            => $"{label} endpoint responds {(slowResponseTime / fastResponseTime - 1).ToString("p")} faster than default JsonFormatter endpoint";

        private static (decimal apiJsonNetResponseTime, decimal apiJilJsonResponseTime, decimal apiJilNoNullsResponseTime, decimal apiCsvResponseTime, decimal apiBytesResponseTime) parseApiResponseTimes(string[] dataTable)
            => (
            parseResponseTime(dataTable, nameof(Tests.JsonDefault)),
            parseResponseTime(dataTable, nameof(Tests.JilJsonActionResult)),
            parseResponseTime(dataTable, nameof(Tests.JilJsonActionResultNoNulls)),
            parseResponseTime(dataTable, nameof(Tests.Csv)),
            parseResponseTime(dataTable, nameof(Tests.ByteArrayActionResult)));

        private static decimal parseResponseTime(string[] dataTable, string method)
            => decimal.Parse(dataTable.First(line => line.Contains(method)).Split('|').Skip(2).First().Replace(",", "").Replace("ns", "").Replace("us", "").Replace("ms", "").Trim());

        private static async Task<(long? apiJsonNetContentLength, long? apiJilJsonContentLength, long? apiCsvContentLength, long? apiBytesContentLength)> testApiResponseContentLengths()
            => (
            await ContentLengthTests.JsonDefault(),
            await ContentLengthTests.JilJsonActionResultNoNulls(),
            await ContentLengthTests.Csv(),
            await ContentLengthTests.ByteArrayActionResult());
    }
}
