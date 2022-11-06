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

        [HttpGet]
        public ActionResult GetAll([FromQuery] SupplierQuery query)
        {
            var supplierDtos = _customerService.GetAll(query);

            return Ok(supplierDtos);
        }
    }
}
