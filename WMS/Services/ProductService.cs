using WMS.Models;
using WMS.Models.Dtos;

namespace WMS.Services
{
    public interface IProductService
    {
        int AddProduct(AddProductDto dto);
    }

    public class ProductService : IProductService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int AddProduct(AddProductDto dto)
        {
            return 0;
        }
    }
}
 