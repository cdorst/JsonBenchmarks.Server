using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Jil.JSON;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Server.Internals.ActionResults.ByteOffsetConstant;
using static Server.Internals.ActionResults.ContentTypeConstants;
using static Server.Internals.ActionResults.JilOptionsConstants;
using static System.Text.Encoding;

namespace Server.Internals.ActionResults
{
    internal class JsonWithoutNullsActionResult : IActionResult
    {
        private readonly byte[] _payload;
        private readonly int _payloadLength;

        public JsonWithoutNullsActionResult(object @object)
        {
            _payload = UTF8.GetBytes(Serialize(@object, JilOptions));
            _payloadLength = _payload.Length;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = Status200OK;
            response.ContentType = JSON;
            response.ContentLength = _payloadLength;
            return response.Body.WriteAsync(_payload, OffsetZero, _payloadLength);
        }
    }
}
