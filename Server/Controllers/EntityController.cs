using Microsoft.AspNetCore.Mvc;
using Server.Internals.ActionResults;
using Server.Internals.Formatters;
using Server.Models;
using static Server.Internals.ActionResults.ContentTypeConstants;

namespace Server.Controllers
{
    [Route("api")]
    public class EntityController : ControllerBase
    {
        [HttpGet("bytes")]
        public byte[] Bytes()
            => Entity.Constant.ToBytes();

        [HttpGet("bytes-actionresult")]
        public IActionResult BytesAction()
            => new BytesActionResult(Entity.Constant.ToBytes());

        [HttpGet("csv")]
        public IActionResult Csv()
            => new CsvActionResult(Entity.Constant.ToStringCsv());

        [HttpGet("json-actionresult")]
        public IActionResult JsonJilActionResult()
            => new JsonActionResult(Entity.Constant);

        [HttpGet("json-default")]
        [Produces(JSON)]
        public Entity JsonDefault() 
            => Entity.Constant;

        [HttpGet("json-formatter")]
        [JilFormatter]
        [Produces(JSON)]
        public Entity JsonJilFormatter() 
            => Entity.Constant;
    }
}
