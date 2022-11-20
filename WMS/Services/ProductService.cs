using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Enums;
using WMS.Exceptions;
using WMS.Helpers;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface IProductService
    {
        int AddProduct(AddProductDto dto, string loggedUserId);
        ProductDto GetById(int id);
        void Delete(int id, string loggedUserId);
        void Update(int id, UpdateProductDto dto, string loggedUserId);
        PagedResult<ProductDto> GetAll(ProductQuery query);
        ProductDetailDto GetFullDetailsById(int id);
        string GetPlacement(int id);
        ProductDto ChangeStatus(int id, string newPackageStatus, string loggedUserId);
        List<ProductStatusDto> GetProductHistory(int id);
    }

    public class ProductService : IProductService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly IProductHelper _productHelper;
        private readonly IProductPlacementHelper _productPlacementHelper;
        private readonly IJournalHelper _journalHelper;

        public ProductService(WMSDbContext dbContext, ILogger<ProductService> logger, IMapper mapper, IProductHelper productHelper,
            IProductPlacementHelper productPlacementHelper, IJournalHelper journalHelper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _productHelper = productHelper;
            _productPlacementHelper = productPlacementHelper;
            _journalHelper = journalHelper;
        }


        public int AddProduct( AddProductDto dto, string loggedUserId)
        {
            var newProduct = _mapper.Map<Product>(dto);


            _dbContext.Products.Add(newProduct);
            _dbContext.SaveChanges();
            _journalHelper.CreateJournal(OperationTypeEnum.Add, newProduct.GetType().Name.ToString(), newProduct.Id, loggedUserId);

            return newProduct.Id;
        }

        public void Update(int id, UpdateProductDto dto, string loggedUserId)
        {

            var product = _productHelper.GetProduct(id);

            if (product is null)
                throw new NotFoundException("Product not found");

            product.Name = dto.Name;
            product.Quantity = dto.Quantity;
            product.Categories.Add(new Category() { Name = dto.CategoryName, HSCode = dto.HSCode });

            _dbContext.SaveChanges();
            _journalHelper.CreateJournal(OperationTypeEnum.Add, product.GetType().Name.ToString(), product.Id, loggedUserId);

        }

        public ProductDto ChangeStatus(int id, string newPackageStatus, string loggedUserId)
        {
            var product = _productHelper.GetProduct(id);

            if (product is null)
                throw new NotFoundException($"Product with id {id} not found");
            
            var newPackageStatusEnum = PackageStatus.PackageStatuses
                .FirstOrDefault(s => s.Equals(newPackageStatus));

            if (newPackageStatusEnum == null)
                throw new BadRequestException($"Status can be only :{PackageStatus.PackageStatuses.ToString()}");
           
            if (product.Statuses != null)
                product.Statuses.ForEach(s => s.IsActive = false);
                      
            _productPlacementHelper.ModifyProductPlacement(product, newPackageStatusEnum);      

            var newStatus = new Status()
            {
                IsActive = true,
                DateStatus = DateTime.Now,
                PackageStatus = newPackageStatus,
                ProductId = id
            };

            _dbContext.Statuses.Add(newStatus);
            _dbContext.SaveChanges();
            _journalHelper.CreateJournal(OperationTypeEnum.Add, product.GetType().Name.ToString(), product.Id, loggedUserId);

            var result = _mapper.Map<ProductDto>(product);

            return result;
        }

        public void Delete(int id, string loggedUserId)
        {
            _logger.LogError($"Product with id: {id} DELETE action invoked");

            var product = _dbContext.Products
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            _journalHelper.CreateJournal(OperationTypeEnum.Add, product.GetType().Name.ToString(), product.Id, loggedUserId);

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
                       || p.Statuses.FirstOrDefault(s => s.IsActive).PackageStatus.ToLower()
                                .Contains(query.SearchPhrase.ToLower())
                       || p.Quantity.ToString().Contains(query.SearchPhrase)
                       || p.Size.ToString().Contains(query.SearchPhrase)));


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
            var product = _productHelper.GetProduct(id);

            if (product is null)
                throw new NotFoundException("Product not found");

            var result = _mapper.Map<ProductDto>(product);
            return result;
        }

        public ProductDetailDto GetFullDetailsById(int id)
        {
            var product = _productHelper.GetProduct(id);

            if (product is null)
                throw new NotFoundException("Product not found");

            var result = _mapper.Map<ProductDetailDto>(product);

            return result;
        }

        public string GetPlacement(int id)
        {
            var productPlacement = _productHelper.GetProduct(id)
                .Layout.PositionXYZ;

            if (productPlacement is null)
                throw new NotFoundException("product is not in our Warehouse");

            return productPlacement;
        }

        public List<ProductStatusDto> GetProductHistory(int id)
        {
            var statuses = _productHelper.GetProduct(id)
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
    }
}
 