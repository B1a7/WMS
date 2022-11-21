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
        public ActionResult GetCapacity()
        {
            var capacity = _layoutService.GetCapacity();

            return Ok(capacity);
        }

        [HttpGet("detailCapacity")]
        public ActionResult GetWarehouseDetailCapacity([FromBody] string size)
        {
            var detailCapacity = _layoutService.GetDetailCapacity(size);

            return Ok(detailCapacity);
        }

        [HttpGet("filling")]
        public ActionResult GetWarehouseFilling()
        {
            var filling = _layoutService.GetWarehouseFilling();

            return Ok(filling);
        }

        [HttpGet("detailFilling")]
        public ActionResult GetWarehouseDetailFilling([FromBody] string size)
        {
            var detailfilling = _layoutService.GetWarehouseDetailFilling(size);

            return Ok(detailfilling);
        }

        [HttpGet("product/{id}/placement")]
        public ActionResult GetPlacementProduct([FromRoute] int layoutId)
        {
            var product = _layoutService.GetPlacementProduct(layoutId);

            return Ok(product);
        }

        [HttpGet("map")]
        public ActionResult GetMap()
        {
            var product = _layoutService.GetMap();

            return Ok(product);
        }
    }
}
