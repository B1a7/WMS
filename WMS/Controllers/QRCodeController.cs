using Microsoft.AspNetCore.Mvc;
using WMS.Services;

namespace WMS.Controllers
{

    [Route("api/QRCode")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        private readonly IQRCodeService _codeService;

        public QRCodeController(IQRCodeService codeService)
        {
            _codeService = codeService;
        }


        [HttpGet("product/{id}")]
        public ActionResult GererateProductQRCode([FromRoute] int id)
        {
            var productQR = _codeService.GenerateProductQRCode(id);

            return Ok(productQR);
        }


    }
}
