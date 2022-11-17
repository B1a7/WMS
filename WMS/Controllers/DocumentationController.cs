using Microsoft.AspNetCore.Mvc;
using WMS.Models.Entities;
using WMS.Services;

namespace WMS.Controllers
{

    [Route("api/documentation")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        private readonly IDocumentationService _documentationService;

        public DocumentationController(IDocumentationService documentationService)
        {
            _documentationService = documentationService;
        }


        [HttpPost("scanqr")]
        public ActionResult ScanQrCode(IFormFile file)
        {
            var result = _documentationService.ScanQrCode(file);

            return Ok(result);
        }

        [HttpGet("supplier/{id}/{docType}")]
        public ActionResult GenerateSupplierDocumentation([FromRoute] int id, [FromRoute] string docType)
        {
            var productQR = _documentationService.GenerateDocumentation<Supplier>(id, docType);

            return File(productQR.Item1, productQR.Item2, productQR.Item3);
        }

        [HttpGet("product/{id}/{docType}")]
        public ActionResult GenerateDocumentation([FromRoute] int id, [FromRoute] string docType)
        {
            var productQR = _documentationService.GenerateDocumentation<Product>(id, docType);

            return File(productQR.Item1, productQR.Item2, productQR.Item3);
        }

    }
}
