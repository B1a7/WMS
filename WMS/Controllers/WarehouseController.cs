using Microsoft.AspNetCore.Mvc;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }


        [HttpGet]
        public ActionResult GetWarehouseCapacity()
        {
            var capacity = _warehouseService.GetWarehouseCapacity();

            return Ok(capacity);
        }

        //[HttpGet("detailcapacity/")]
        //public ActionResult GetWarehouseDetailCapacity()
        //{
        //    var id = _productService.AddProduct(dto);

        //    return Created($"/api/product/{id}", null);
        //}
    }
}
