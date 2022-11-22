using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Models;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> AddProduct([FromBody] AddProductDto dto)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var id = await _productService.AddProductAsync(dto, loggedUserId);

            return Created($"/api/product/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> UpdateAsync([FromBody] UpdateProductDto dto, [FromRoute]int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _productService.UpdateAsync(id, dto, loggedUserId);

            return Ok(result);
        }

        [HttpPut("{id}/changestatus")]
        public async Task<ActionResult> ChangeStatusAsync([FromRoute] int id, [FromBody] string newPackageStatus)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _productService.ChangeStatusAsync(id, newPackageStatus, loggedUserId);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _productService.DeleteAsync(id, loggedUserId);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllAsync([FromQuery] ProductQuery query)
        {
            var productDtos = await _productService.GetAllAsync(query);

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetByIdAsync([FromRoute] int id)
        {
            var product = await _productService.GetByIdAsync(id);

            return Ok(product);
        }

        [HttpGet("{id}/detail")]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult<ProductDetailDto>> GetFullDetailsByIdAsync([FromRoute] int id)
        {
            var product = await _productService.GetFullDetailsByIdAsync(id);

            return Ok(product);
        }

        [HttpGet("{id}/placement")]
        public async Task<ActionResult> GetPlacementAsync([FromRoute] int id)
        {
            var placement = await _productService.GetPlacementAsync(id);

            return Ok(placement);
        }

        [HttpGet("{id}/history")]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> GetProductHistoryAsync([FromRoute] int id)
        {
            var statusList = await _productService.GetProductHistoryAsync(id);

            return Ok(statusList);
        }
    }
}
