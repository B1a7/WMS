using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IProductHelper
    {
        Product GetProduct(int id);
    }

    public class ProductHelper : IProductHelper
    {
        private readonly WMSDbContext _dbContext;

        public ProductHelper(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Product GetProduct(int id)
        {
            var product = _dbContext
                .Products
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .Include(p => p.Layout)
                .FirstOrDefault(p => p.Id == id);

            return product;
        }
    }
}
