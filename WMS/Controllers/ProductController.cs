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
        public ActionResult AddProduct([FromBody] AddProductDto dto)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var id = _productService.AddProduct(dto, loggedUserId);

            return Created($"/api/product/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin, manager")]
        public ActionResult Update([FromBody] UpdateProductDto dto, [FromRoute]int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _productService.Update(id, dto, loggedUserId);

            return Ok();
        }

        [HttpPut("{id}/changestatus")]
        public ActionResult ChangeStatus([FromRoute] int id, [FromBody] string newPackageStatus)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = _productService.ChangeStatus(id, newPackageStatus, loggedUserId);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin, manager")]
        public ActionResult Delete([FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _productService.Delete(id, loggedUserId);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "admin, manager")]
        public ActionResult<IEnumerable<ProductDto>> GetAll([FromQuery] ProductQuery query)
        {
            var productDtos = _productService.GetAll(query);

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetById([FromRoute] int id)
        {
            var product = _productService.GetById(id);

            return Ok(product);
        }

        [HttpGet("{id}/detail")]
        [Authorize(Roles = "admin, manager")]
        public ActionResult<ProductDetailDto> GetFullDetailsById([FromRoute] int id)
        {
            var product = _productService.GetFullDetailsById(id);

            return Ok(product);
        }

        [HttpGet("{id}/placement")]
        public ActionResult GetPlacement([FromRoute] int id)
        {
            var placement = _productService.GetPlacement(id);

            return Ok(placement);
        }

        [HttpGet("{id}/history")]
        [Authorize(Roles = "admin, manager")]
        public ActionResult GetProductHistory([FromRoute] int id)
        {
            var statusList = _productService.GetProductHistory(id);

            return Ok(statusList);
        }
    }
}
