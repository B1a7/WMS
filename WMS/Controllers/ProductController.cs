using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Entities;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/product")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            productService = _productService;
        }


        [HttpPost]
        public ActionResult AddProduct([FromBody] AddProductDto dto)
        {
            var id = _productService.AddProduct(dto);

            return Created($"/api/produuct/{id}", null);
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

        [HttpGet("{id}")]
        public ActionResult<Product> GetFullDetailsById([FromRoute] int id)
        {
            var product = _productService.GetFullDetailsById(id);

            return Ok(product);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateProductDto dto, [FromRoute]int id)
        {
            _productService.Update(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _productService.Delete(id);

            return NoContent();
        }
    }
}
