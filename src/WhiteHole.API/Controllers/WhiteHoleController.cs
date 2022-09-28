using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using WhiteHole.Repository;
using WhiteHole.Repository.Repositories;
using WhiteHole.Services;

namespace WhiteHole.API.Controllers
{
    [ApiController]
    [Route("whitehole")]
    public class WhiteHoleController : ControllerBase
    {
        private readonly ILogger<WhiteHoleController> _logger;
        private readonly IWhiteHoleCRUDServices _crudServices;
        private readonly IWhiteHoleQueryServices _queryServices;

        public WhiteHoleController(ILogger<WhiteHoleController> logger,
            IWhiteHoleCRUDServices crudServices,
            IWhiteHoleQueryServices queryServices)
        {
            _logger = logger;
            _crudServices = crudServices;
            _queryServices = queryServices;
        }

        [HttpPost]
        [Route("{*.}")]
        public async Task<IActionResult> Create([FromBody] JsonElement json)
        {
            var res = await _crudServices.Create(this.Request.Path.Value, json.GetRawText());
            return Ok(res);
        }

        [HttpGet]
        [Route("{*.}")]
        public async Task<IActionResult> Get([FromQuery]string? sort = null)
        {

            var pathRes = Util.PathParser(this.Request.Path.Value);
            if (pathRes[Constants.PATH_LAST_KEY] == Constants.PATH_LAST_ID)
            {
                return Ok(await _queryServices.Get(pathRes));
            }
            else if(pathRes[Constants.PATH_LAST_KEY] == Constants.PATH_LAST_OBJ)
            {
                return Ok(await _queryServices.GetList(pathRes, sort));
            }

            return NotFound();
        }

        [HttpPut]
        [Route("{*.}")]
        public async Task<IActionResult> Put([FromBody] JsonElement json)
        {
            var res = await _crudServices.Put(this.Request.Path.Value, json.GetRawText());
            return Ok(res);
        }

        [HttpPatch]
        [Route("{*.}")]
        public async Task<IActionResult> HttpPatch([FromBody] JsonElement json)
        {
            var res = await _crudServices.Patch(this.Request.Path.Value, json.GetRawText());
            return Ok(res);
        }
    }
}