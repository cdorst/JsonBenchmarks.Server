using Microsoft.AspNetCore.Mvc.Formatters;

namespace Server.Internals.Formatters
{
    internal static class JilOutputFormatterConstant
    {
        public static readonly IOutputFormatter JilFormatter = new JilOutputFormatter();
    }
}
