using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IProductHelper
    {
        Product GetProduct(int id);
        void AddCategory(string categoryName, string hsCode, Product product);
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

        public void AddCategory(string categoryName, string hsCode, Product product)
        {
            if (!_dbContext.Categories.Any(c => c.Name.ToLower() == categoryName.ToLower()))
            {
                var newCategory = new Category()
                {
                    Name = categoryName,
                    HSCode = hsCode,
                };

                _dbContext.Categories.Add(newCategory);
                product.Categories.Add(newCategory);
            }
            else
            {
                product.Categories.Add(_dbContext.Categories.First(c => c.Name == categoryName));
            }
        }
    }
}
