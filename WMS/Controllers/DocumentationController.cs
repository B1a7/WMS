using Microsoft.AspNetCore.Mvc;
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


        [HttpPost]
        public ActionResult ScanQrCode(IFormFile file)
        {
            var result = _documentationService.ScanQrCode(file);

            return Ok(result);
        }

        [HttpGet("label/product/{id}")]
        public ActionResult GenerateProductLabel([FromRoute] int id)
        {
            var productQR = _documentationService.GenerateProductLabel(id);

            return File(productQR.Item1, productQR.Item2, productQR.Item3);
        }


        [HttpGet("document/product/{id}")]
        public ActionResult GenerateProductDocument([FromRoute] int id)
        {
            var productDoc = _documentationService.GenerateProductDocument(id);

            return File(productDoc.Item1, productDoc.Item2, productDoc.Item3);
        }

        [HttpGet("label/supplier/QR/{id}")]
        public ActionResult GenerateSupplierLabel([FromRoute] int id)
        {
            var supplierQR = _documentationService.GenerateSupplierLabel(id);

            return File(supplierQR.Item1, supplierQR.Item2, supplierQR.Item3);
        }

        [HttpGet("document/supplier/{id}")]
        public ActionResult GenerateSupplierDocument([FromRoute] int id)
        {
            var supplierDoc = _documentationService.GenerateSupplierDocument(id);

            return File(supplierDoc.Item1, supplierDoc.Item2, supplierDoc.Item3);
        }


    }
}
