using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        Task<int> AddProductAsync(AddProductDto dto, string loggedUserId);
        Task<ProductDto> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id, string loggedUserId);
        Task<bool> UpdateAsync(int id, UpdateProductDto dto, string loggedUserId);
        Task<PagedResult<ProductDto>> GetAllAsync(ProductQuery query);
        Task<ProductDto> ChangeStatusAsync(int id, string newPackageStatus, string loggedUserId);
        Task<ProductDetailDto> GetFullDetailsByIdAsync(int id);
        Task<string> GetPlacementAsync(int id);
        Task<List<ProductStatusDto>> GetProductHistoryAsync(int id);
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


        public async Task<int> AddProductAsync( AddProductDto dto, string loggedUserId)
        {
            var newProduct = _mapper.Map<Product>(dto);
            var categoryName = dto.CategoryName;
            var hsCode = dto.HSCode;

            await _productHelper.AddCategoryAsync(categoryName, hsCode, newProduct);

            await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();
            await _journalHelper.CreateJournalAsync(OperationTypeEnum.Add, newProduct.GetType().Name.ToString(), newProduct.Id, loggedUserId);

            return newProduct.Id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto, string loggedUserId)
        {

            var product = _dbContext.Products
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            product.Name = dto.Name;
            product.Quantity = dto.Quantity;

            _productHelper.AddCategory(dto.CategoryName, dto.HSCode, product);

            await _journalHelper.CreateJournalAsync(OperationTypeEnum.Update, product.GetType().Name.ToString(), product.Id, loggedUserId);

            return true;

        }

        public async Task<ProductDto> ChangeStatusAsync(int id, string newPackageStatus, string loggedUserId)
        {
            var product = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Statuses)
                .Include(p => p.Layout)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException($"Product with id {id} not found");
            
            var newPackageStatusEnum = PackageStatus.PackageStatuses
                .FirstOrDefault(s => s.Equals(newPackageStatus));

            if (newPackageStatusEnum == null)
                throw new BadRequestException($"Status can be only :{PackageStatus.PackageStatuses.ToString()}");
           
            if (product.Statuses != null)
                product.Statuses.ForEach(s => s.IsActive = false);
                      
            var isPlacementChanged = await _productPlacementHelper.ModifyProductPlacementAsync(product, newPackageStatusEnum);

            if (!isPlacementChanged)
                throw new InternalServerErrorException("Cannot change product placement");

            var newStatus = new Status()
            {
                IsActive = true,
                DateStatus = DateTime.Now,
                PackageStatus = newPackageStatus,
                ProductId = id
            };

            await _dbContext.Statuses.AddAsync(newStatus);
            await _dbContext.SaveChangesAsync();
            await _journalHelper.CreateJournalAsync(OperationTypeEnum.ChangeStatus, product.GetType().Name.ToString(), product.Id, loggedUserId);


            
            var result = _mapper.Map<ProductDto>(product);

            return result;
        }

        public async Task<bool> DeleteAsync(int id, string loggedUserId)
        {
            _logger.LogError($"Product with id: {id} DELETE action invoked");

            Product product = new Product()
            {
                Id = id
            };

            _dbContext.Entry(product).State = EntityState.Deleted;

            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
                throw new NotFoundException("Product not found");

            await _journalHelper.CreateJournalAsync(OperationTypeEnum.Delete, product.GetType().Name.ToString(), product.Id, loggedUserId);

            return result > 0 ? true : false;
        }

        public Task<PagedResult<ProductDto>> GetAllAsync(ProductQuery query)
        {
            var baseQuery = _dbContext
                .Products
                .AsNoTracking()
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

            return Task.FromResult(result);
        }

        public Task<ProductDto> GetByIdAsync(int id)
        {

            var product = _dbContext
                .Products
                .AsNoTracking()
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .Include(p => p.Layout)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            return Task.FromResult(product);
        }

        public Task<ProductDetailDto> GetFullDetailsByIdAsync(int id)
        {

            var product = _dbContext
                .Products
                .AsNoTracking()
                .Include(p => p.Supplier)
                .Include(p => p.Categories)
                .Include(p => p.Statuses)
                .Include(p => p.Layout)
                .ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
                throw new NotFoundException("Product not found");

            return Task.FromResult(product);
        }

        public Task<string> GetPlacementAsync(int id)
        {
            var productPlacement = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Layout)
                .FirstOrDefault(p => p.Id == id)
                .Layout.PositionXYZ;

            if (productPlacement is null)
                throw new NotFoundException("product is not in our Warehouse");

            return Task.FromResult(productPlacement);
        }

        public Task<List<ProductStatusDto>> GetProductHistoryAsync(int id)
        {
            var statuses = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Statuses)
                .FirstOrDefault(p => p.Id == id)
                .Statuses;

            if (statuses is null)
                throw new NotFoundException("No product history found");

            var result = _mapper.Map<List<ProductStatusDto>>(statuses);

            return Task.FromResult(result);
        }
    }
}
 