using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/supplier")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private ISupplierService _supplierService;

        public SupplierController(ISupplierService customerService)
        {
            _supplierService = customerService;
        }


        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> AddSupplierAsync([FromBody] AddSupplierDto dto)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var id = await _supplierService.AddSupplierAsync(dto, loggedUserId);

            return Created($"/api/customer/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> UpdateAsync([FromBody] UpdateSupplierDto dto, [FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _supplierService.UpdateAsync(dto, id, loggedUserId);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _supplierService.DeleteAsync(id, loggedUserId);

            return Ok(result);
        }
        [HttpGet]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> GetAllAsync([FromQuery] SupplierQuery query)
        {
            var supplierDtos = await _supplierService.GetAllAsync(query);

            return Ok(supplierDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] int id)
        {
            var customer = await _supplierService.GetByIdAsync(id);

            return Ok(customer);
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult> GetSupplierProductsAsync([FromRoute] int id)
        {
            var supplierProductsDto = await _supplierService.GetSupplierProductsAsync(id);

            return Ok(supplierProductsDto);
        }
        
    }
}
