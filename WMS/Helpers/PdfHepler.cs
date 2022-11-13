using AutoMapper;
using IronPdf;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WMS.Exceptions;
using WMS.Models;

namespace WMS.Helpers
{
    public class PdfHepler
    {
        private readonly IMapper _mapper;
        private readonly WMSDbContext _dbContext;
        private readonly string rootPath;

        public PdfHepler(WMSDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            rootPath = Directory.GetCurrentDirectory();
        }

        private (byte[], string, string) GeneratePdf(string fileName, string filePath, string content)
        {
            var htmlLines = new ChromePdfRenderer();

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);

            var pdf = htmlLines.RenderHtmlAsPdf(content)
                .SaveAs(filePath);

            var fileContent = File.ReadAllBytes(filePath);

            return (fileContent, contentType, fileName);
        }

        private string GetHtmlSchema(string fileName)
        {
            var filePath = $"{rootPath}/ProductDocumentation/{fileName}";

            string html = File.ReadAllText(filePath);
            return html;
        }

        public (byte[], string, string) GenerateProductLabel(int id)
        {
            var product = _dbContext.Products
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new NotFoundException("Cannot find product");

            var fileName = "ProductLabel.pdf";
            var filePath = $"{rootPath}/ProductDocumentation/{fileName}";

            var htmlFile = "ProductLabel.html";

            var content = GetHtmlSchema(htmlFile);
          
            return GeneratePdf(fileName, filePath, content);
        }

    }
}
