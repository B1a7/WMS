using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var id = _supplierService.AddSupplier(dto, loggedUserId);

            return Created($"/api/customer/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateSupplierDto dto, [FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _supplierService.Update(dto, id, loggedUserId);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _supplierService.Delete(id, loggedUserId);

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
