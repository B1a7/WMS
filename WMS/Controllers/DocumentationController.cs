using Microsoft.AspNetCore.Mvc;
using WMS.Services;

namespace WMS.Controllers
{

    [Route("api/documentation")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        private readonly IDocumentationService _codeService;

        public DocumentationController(IDocumentationService codeService)
        {
            _codeService = codeService;
        }


        [HttpGet("product/QR/{id}")]
        public ActionResult GenerateProductLabel([FromRoute] int id)
        {
            var productQR = _codeService.GenerateProductLabel(id);

            return File(productQR.Item1, productQR.Item2, productQR.Item3);
        }


        [HttpGet("product/{id}")]
        public ActionResult GenerateProductDocument([FromRoute] int id)
        {
            var productDoc = _codeService.GenerateProductDocument(id);

            return File(productDoc.Item1, productDoc.Item2, productDoc.Item3);
        }

        [HttpGet("supplier/QR/{id}")]
        public ActionResult GenerateSupplierLabel([FromRoute] int id)
        {
            var productQR = _codeService.GenerateSupplierLabel(id);

            return Ok(productQR);
        }


        [HttpGet("supplier/{id}")]
        public ActionResult GenerateSupplierDocument([FromRoute] int id)
        {
            var productDoc = _codeService.GenerateSupplierDocument(id);

            return File(productDoc.Item1, productDoc.Item2, productDoc.Item3);
        }

    }
}
