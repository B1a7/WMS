using WMS.Models;

namespace WMS.Services
{
    public interface IProductService
    {
        void someMethod();
    }

    public class ProductService : IProductService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void someMethod()
        {

        }
    }
}
 