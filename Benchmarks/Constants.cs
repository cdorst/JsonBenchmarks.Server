using System;
using System.Net.Http;

namespace Benchmarks
{
    internal static class Constants
    {
        public const string ByteArrayRoute = "/api/bytes";
        public const string ByteArrayActionResultRoute = "/api/bytes-actionresult";
        public const string CsvRoute = "/api/csv";
        public const string JilJsonActionResultNoNullsRoute = "/api/json-actionresult-no-nulls";
        public const string JilJsonActionResultRoute = "/api/json-actionresult";
        public const string JilJsonFormatterActionResultRoute = "/api/json-formatter-actionresult";
        public const string JilJsonFormatterRoute = "/api/json-formatter";
        public const string JsonDefaultActionResultRoute = "/api/json-default-actionresult";
        public const string JsonDefaultRoute = "/api/json-default";

        public static readonly HttpClient Server = new HttpClient() { BaseAddress = new Uri("https://localhost:44301") };
    }
}
