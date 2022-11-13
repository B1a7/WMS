using AutoMapper;
using IronPdf;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WMS.Exceptions;
using WMS.Models;

namespace WMS.Helpers
{
    public interface IPdfHepler
    {
        (byte[], string, string) GetDocumentation (int id, string pdfName);
    }

    public class PdfHepler : IPdfHepler
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

        public (byte[], string, string) GetDocumentation(int id, string pdfName)
        {
            var fileName = $"{pdfName}.pdf";
            var filePath = $"{rootPath}/ProductDocumentation/{fileName}";

            var htmlFile = $"{pdfName}.html";

            var content = GetHtmlSchema(htmlFile);

            var result = GeneratePdf(fileName, filePath, content);
            return result;
        }
    }
}
