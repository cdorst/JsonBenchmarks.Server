using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Server.Internals.ActionResults.ByteOffsetConstant;

namespace Server.Internals.ActionResults
{
    internal class BytesActionResult : IActionResult
    {
        private readonly byte[] _payload;
        private readonly int _payloadLength;

        public BytesActionResult(byte[] bytes)
        {
            _payload = bytes;
            _payloadLength = _payload.Length;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = Status200OK;
            response.ContentLength = _payloadLength;
            return response.Body.WriteAsync(_payload, OffsetZero, _payloadLength);
        }
    }
}
