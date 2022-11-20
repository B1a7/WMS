using Microsoft.AspNetCore.Mvc;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private ISupplierService _supplierService;

        public SupplierController(ISupplierService customerService)
        {
            _supplierService = customerService;
        }


        [HttpPost]
        public ActionResult AddSupplier([FromBody] AddSupplierDto dto)
        {
            var id = _supplierService.AddSupplier(dto);

            return Created($"/api/customer/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateSupplierDto dto, [FromRoute] int id)
        {
            _supplierService.Update(dto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _supplierService.Delete(id);

            return NoContent();
        }
        [HttpGet]
        public ActionResult GetAll([FromQuery] SupplierQuery query)
        {
            var supplierDtos = _supplierService.GetAll(query);

            return Ok(supplierDtos);
        }

        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            var customer = _supplierService.GetById(id);

            return Ok(customer);
        }

        [HttpGet("{id}/products")]
        public ActionResult GetSupplierProducts([FromRoute] int id)
        {
            var supplierProductsDto = _supplierService.GetSupplierProducts(id);

            return Ok(supplierProductsDto);
        }
        
    }
}
