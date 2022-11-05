using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Exceptions;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface IProductService
    {
        int AddProduct(AddProductDto dto);
        ProductDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateProductDto dto);
        PagedResult<ProductDto> GetAll(ProductQuery query);
        Product GetFullDetailsById(int id);
    }

    public class ProductService : IProductService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(WMSDbContext dbContext, ILogger<ProductService> logger, IMapper mapper )
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }


        public int AddProduct(AddProductDto dto)
        {
            return 0;
        }

        public ProductDto GetById(int id)
        {
            var product = _dbContext
                .Products
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            var result = _mapper.Map<ProductDto>(product);
            return result;
        }

        public void Delete(int id)
        {

        }

        public void Update(int id, UpdateProductDto dto)
        {

        }

        public PagedResult<ProductDto> GetAll(ProductQuery query)
        {
            var baseQuery = _dbContext
                .Products
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .Where(p => query.SearchPhrase == null || (p.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                            || p.Statuses.Where(s => s.IsActive).FirstOrDefault().PackageStatus.ToLower()
                                .Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Product, object>>>
                {
                    {nameof(Product.Name), p => p.Name }
                };

                var selectedColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var products = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var productsDtos = _mapper.Map<List<ProductDto>>(products);

            var result = new PagedResult<ProductDto>(productsDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public Product GetFullDetailsById(int id)
        {
            var product = _dbContext
                .Products
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

           
            return product;
        }

    }
}
 