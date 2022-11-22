using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    [Authorize]
    public class LayoutController : ControllerBase
    {
        private readonly ILayoutService _layoutService;


        public LayoutController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }


        [HttpGet("capacity")]
        public async Task<ActionResult> GetCapacityAsync()
        {
            var capacity = await _layoutService.GetCapacityAsync();

            return Ok(capacity);
        }

        [HttpGet("detailCapacity")]
        public async Task<ActionResult> GetWarehouseDetailCapacityAsync([FromQuery] string size)
        {
            var detailCapacity = await _layoutService.GetDetailCapacityAsync(size);

            return Ok(detailCapacity);
        }

        [HttpGet("filling")]
        public async Task<ActionResult> GetWarehouseFillingAsync()
        {
            var filling = await _layoutService.GetWarehouseFillingAsync();

            return Ok(filling);
        }

        [HttpGet("detailFilling")]
        public async Task<ActionResult> GetWarehouseDetailFillingAsync([FromQuery] string size)
        {
            var detailfilling = await _layoutService.GetWarehouseDetailFillingAsync(size);

            return Ok(detailfilling);
        }

        [HttpGet("placement/{id}/product")]
        public async Task<ActionResult> GetPlacementProductAsync([FromRoute] int id)
        {
            var product = await _layoutService.GetPlacementProductAsync(id);

            return Ok(product);
        }

        [HttpGet("map")]
        public async Task<ActionResult> GetMap()
        {
            var product = await _layoutService.GetMapAsync();

            return Ok(product);
        }
    }
}
