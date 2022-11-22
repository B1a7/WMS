using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IProductHelper
    {
        Task AddCategoryAsync(string categoryName, string hsCode, Product product);
    }

    public class ProductHelper : IProductHelper
    {
        private readonly WMSDbContext _dbContext;

        public ProductHelper(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task AddCategoryAsync(string categoryName, string hsCode, Product product)
        {
            product.Categories = new List<Category>();
         
            if (!_dbContext.Categories.Any(c => c.Name.ToLower() == categoryName.ToLower()))
            {
                var newCategory = new Category()
                {
                    Name = categoryName,
                    HSCode = hsCode,
                };

                await _dbContext.Categories.AddAsync(newCategory);
                product.Categories.Add(newCategory);
            }
            else
            {
                product.Categories.Add(_dbContext.Categories.First(c => c.Name == categoryName));
            }
        }
    }
}
