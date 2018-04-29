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

        [HttpGet("json-actionresult-no-nulls")]
        public IActionResult JsonJilActionResultWithoutNulls()
            => new JsonWithoutNullsActionResult(Entity.Constant);

        [HttpGet("json-default")]
        [Produces(JSON)]
        public Entity JsonDefault()
            => Entity.Constant;

        [HttpGet("json-default-actionresult")]
        [Produces(JSON)]
        public ActionResult<Entity> JsonDefaultActionResult()
            => Entity.Constant;

        [HttpGet("json-formatter")]
        [JilFormatter]
        [Produces(JSON)]
        public Entity JsonJilFormatter()
            => Entity.Constant;

        [HttpGet("json-formatter-actionresult")]
        [JilFormatter]
        [Produces(JSON)]
        public ActionResult<Entity> JsonJilFormatterActionResult()
            => Entity.Constant;
    }
}
