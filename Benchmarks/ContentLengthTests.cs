using System.Net.Http;
using System.Threading.Tasks;

namespace Benchmarks
{
    public static class ContentLengthTests
    {
        private static readonly Tests _response = new Tests();

        public static async Task<long?> ByteArray()
            => await Test(_response.ByteArray());

        public static async Task<long?> ByteArrayActionResult()
            => await Test(_response.ByteArrayActionResult());

        public static async Task<long?> Csv()
            => await Test(_response.Csv());

        public static async Task<long?> JilJsonActionResult()
            => await Test(_response.JilJsonActionResult());

        public static async Task<long?> JilJsonActionResultNoNulls()
            => await Test(_response.JilJsonActionResultNoNulls());

        public static async Task<long?> JilJsonFormatter()
            => await Test(_response.JilJsonFormatter());

        public static async Task<long?> JsonDefault()
            => await Test(_response.JsonDefault());

        private static async Task<long?> Test(Task<HttpResponseMessage> response)
            => (await response).Content.Headers.ContentLength;
    }
}
