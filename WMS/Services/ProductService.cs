using WMS.Models;
using WMS.Models.Dtos;

namespace WMS.Services
{
    public interface IProductService
    {
        int AddProduct(AddProductDto dto);
        ProductDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateProductDto dto);
        PagedResult<ProductDto> GetAll(ProductQuery query);
    }

    public class ProductService : IProductService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;


        public ProductService(WMSDbContext dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public int AddProduct(AddProductDto dto)
        {
            return 0;
        }

        public ProductDto GetById(int id)
        {
            return null;
        }

        public void Delete(int id)
        {

        }

        public void Update(int id, UpdateProductDto dto)
        {

        }

        public PagedResult<ProductDto> GetAll(ProductQuery query)
        {
            return null;
        }



    }
}
 