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

    public class ProductController : ControllerBase
    {
        private IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost]
        public ActionResult AddProduct([FromBody] AddProductDto dto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var id = _productService.AddProduct(dto, userId);

            return Created($"/api/product/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateProductDto dto, [FromRoute]int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _productService.Update(id, dto, userId);

            return Ok();
        }

        [HttpPut("{id}/changestatus")]
        public ActionResult ChangeStatus([FromRoute] int id, [FromBody] string newPackageStatus)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = _productService.ChangeStatus(id, newPackageStatus, userId);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _productService.Delete(id, userId);

            return NoContent();
        }

        [HttpGet]
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
        public ActionResult GetProductHistory([FromRoute] int id)
        {
            var statusList = _productService.GetProductHistory(id);

            return Ok(statusList);
        }
    }
}
