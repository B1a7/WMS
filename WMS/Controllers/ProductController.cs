using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Models;
using WMS.Models.Dtos.Product;
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
            var id = _productService.AddProduct(dto);

            return Created($"/api/product/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateProductDto dto, [FromRoute]int id)
        {
            _productService.Update(id, dto);

            return Ok();
        }

        [HttpPut("{id}/changestatus")]
        public ActionResult ChangeStatus([FromRoute] int id, [FromBody] string newPackageStatus)
        {
            var product = _productService.ChangeStatus(id, newPackageStatus);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _productService.Delete(id);

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
