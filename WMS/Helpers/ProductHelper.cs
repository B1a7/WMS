using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IProductHelper
    {
        void AddCategory(string categoryName, string hsCode, Product product);
    }

    public class ProductHelper : IProductHelper
    {
        private readonly WMSDbContext _dbContext;

        public ProductHelper(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
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
