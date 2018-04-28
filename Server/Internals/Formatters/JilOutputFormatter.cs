using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System.Threading.Tasks;
using static Jil.JSON;
using static Server.Internals.Formatters.MediaTypeHeaderValues;
using static System.Text.Encoding;

namespace Server.Internals.Formatters
{
    internal class JilOutputFormatter : TextOutputFormatter
    {
        public JilOutputFormatter()
        {
            SupportedEncodings.Add(UTF8);
            SupportedEncodings.Add(Unicode);
            SupportedMediaTypes.Add(ApplicationJson);
            SupportedMediaTypes.Add(TextJson);
            SupportedMediaTypes.Add(ApplicationAnyJsonSyntax);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding encoding)
        {
            using (var writer = context.WriterFactory(context.HttpContext.Response.Body, encoding))
            {
                Serialize(context.Object, writer);
                await writer.FlushAsync();
            }
        }
    }
}
