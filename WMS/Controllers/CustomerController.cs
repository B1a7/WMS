using Microsoft.AspNetCore.Mvc;
using WMS.Models.Dtos.Customer;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpPost]
        public ActionResult AddSupplier([FromBody] AddSupplierDto dto)
        {
            var id = _customerService.AddSupplier(dto);

            return Created($"/api/customer/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateSupplierDto dto, [FromRoute] int id)
        {
            _customerService.Update(dto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _customerService.Delete(id);

            return NoContent();
        }
        [HttpGet]
        public ActionResult GetAll([FromQuery] SupplierQuery query)
        {
            var supplierDtos = _customerService.GetAll(query);

            return Ok(supplierDtos);
        }

        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            var customer = _customerService.GetById(id);

            return Ok(customer);
        }

        [HttpGet("products/{id}")]
        public ActionResult GetSupplierProducts([FromRoute] int id)
        {
            var supplierProductsDto = _customerService.GetSupplierProducts(id);

            return Ok(supplierProductsDto);
        }
        
    }
}
