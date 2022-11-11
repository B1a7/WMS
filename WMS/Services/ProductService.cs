using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Enums;
using WMS.Exceptions;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Dtos.Product;
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
        ProductDetailDto GetFullDetailsById(int id);
        string GetPlacement(int id);
        ProductDto ChangeStatus(int id, string newPackageStatus);
        List<ProductStatusDto> GetProductHistory(int id);
    }

    public class ProductService : IProductService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;


        public ProductService(WMSDbContext dbContext, ILogger<ProductService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }


        public int AddProduct( AddProductDto dto)
        {
            var newProduct = _mapper.Map<Product>(dto);

            _dbContext.Products.Add(newProduct);
            _dbContext.SaveChanges();

            return newProduct.Id;
        }

        public void Update(int id, UpdateProductDto dto)
        {
            _logger.LogError($"Product with id: {id} UPDATE action invoked");

            var product = GetProduct(id);

            if (product is null)
                throw new NotFoundException("Product not found");

            product.Name = dto.Name;
            product.Quantity = dto.Quantity;
            product.Categories.Add(new Category() { Name = dto.CategoryName, HSCode = dto.HSCode });

            _dbContext.SaveChanges();
        }
        
        public ProductDto ChangeStatus(int id, string newPackageStatus)
        {
            var product = _dbContext.Products
                    .Include(p => p.Statuses)
                    .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");
            if (!PackageStatus.PackageStatuses.Contains(newPackageStatus.ToLower()))
                throw new BadRequestException($"Status can be only :{PackageStatus.PackageStatuses.ToString()}");
            if (product.Statuses != null)
                product.Statuses.ForEach(s => s.IsActive = false);

            var newStatus = new Status()
            {
                IsActive = true,
                PackageStatus = newPackageStatus,
                ProductId = id
            };

            _dbContext.Statuses.Add(newStatus);
            _dbContext.SaveChanges();

            var result = _mapper.Map<ProductDto>(product);

            return result;
        }

        public void Delete(int id)
        {
            _logger.LogError($"Product with id: {id} DELETE action invoked");

            var product = _dbContext.Products
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }
        
        public PagedResult<ProductDto> GetAll(ProductQuery query)
        {
            var baseQuery = _dbContext
                .Products
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .Include(p => p.Layout)
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

                baseQuery = query.SortDirection == SortDirectionEnum.ASC
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

        public ProductDto GetById(int id)
        {
            var product = _dbContext
                .Products
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .Include(p => p.Layout)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            var result = _mapper.Map<ProductDto>(product);
            return result;
        }

        public ProductDetailDto GetFullDetailsById(int id)
        {
            var product = GetProduct(id);

            if (product is null)
                throw new NotFoundException("Product not found");

            var result = _mapper.Map<ProductDetailDto>(product);

            return result;
        }

        public string GetPlacement(int id)
        {
            var productPlacement = _dbContext.Products
                .Include(p => p.Layout)
                .FirstOrDefault(p => p.Id == id).Layout.PositionXYZ;

            if (productPlacement is null)
                throw new NotFoundException("product is not in our Warehouse");

            return productPlacement;
        }

        public List<ProductStatusDto> GetProductHistory(int id)
        {
            var statuses = _dbContext.Products
                           .Include(p => p.Statuses)
                           .FirstOrDefault(p => p.Id == id)
                           .Statuses;

            List<ProductStatusDto> result = new List<ProductStatusDto>(); 
            
            foreach (var status in statuses)
            {
                result.Add(new ProductStatusDto()
                {
                    DateStatus = status.DateStatus,
                    PackageStatus = status.PackageStatus,
                    IsActive = status.IsActive
                });
            }

            return result;
        }



        //TODO: move it to extension helper class
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
 