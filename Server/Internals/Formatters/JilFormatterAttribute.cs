using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using static Server.Internals.Formatters.JilOutputFormatterConstant;

namespace Server.Internals.Formatters
{
    internal sealed class JilFormatterAttribute : Attribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult result) result.Formatters.Add(JilFormatter);
        }
    }
}
