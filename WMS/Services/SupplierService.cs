using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Enums;
using WMS.Exceptions;
using WMS.Helpers;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface ISupplierService
    {
        Task<int> AddSupplierAsync(AddSupplierDto dto, string loggedUserId);
        Task<bool> UpdateAsync(UpdateSupplierDto dto, int id, string loggedUserId);
        Task<bool> DeleteAsync(int id, string loggedUserId);
        Task<PagedResult<SupplierDto>> GetAllAsync(SupplierQuery query);
        Task<SupplierDetailDto> GetByIdAsync(int id);
        Task<List<SupplierProductDto>> GetSupplierProductsAsync(int id);
    }

    public class SupplierService : ISupplierService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly IJournalHelper _journalHepler;

        public SupplierService(WMSDbContext dbContext, ILogger<ProductService> logger, IMapper mapper, IJournalHelper journalHelper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _journalHepler = journalHelper;
        }


        public async Task<int> AddSupplierAsync(AddSupplierDto dto, string loggedUserId)
        {
            var supplier = _mapper.Map<Supplier>(dto);

            await _dbContext.Suppliers.AddAsync(supplier);
            await _dbContext.SaveChangesAsync();
            await _journalHepler.CreateJournalAsync(OperationTypeEnum.Add, supplier.GetType().Name.ToString(), supplier.Id, loggedUserId);

            return supplier.Id;
        }

        public async Task<bool> UpdateAsync(UpdateSupplierDto dto, int id, string loggedUserId)
        {
            _logger.LogError($"Supplier with id: {id} UPDATE action invoked");

            var supplier = _dbContext.Suppliers
                .Include(s => s.Address)
                .FirstOrDefault(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            var updatedSupllier = _mapper.Map<Supplier>(dto);

            _dbContext.Update(updatedSupllier);

            bool journal = await _journalHepler.CreateJournalAsync(OperationTypeEnum.Update, supplier.GetType().Name.ToString(), supplier.Id, loggedUserId);

            return journal;
        }

        public async Task<bool> DeleteAsync(int id, string loggedUserId)
        {
            _logger.LogError($"Supplier with id: {id} DELETE action invoked");


            Supplier supplier = new Supplier()
            {
                Id = id,
            };

            _dbContext.Entry(supplier).State = EntityState.Deleted;

            var result = await _journalHepler.CreateJournalAsync(OperationTypeEnum.Delete, supplier.GetType().Name.ToString(), supplier.Id, loggedUserId);

            return result;
        }

        public Task<PagedResult<SupplierDto>> GetAllAsync(SupplierQuery query)
        {
            var baseQuery = _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .Include(s => s.Address)
                .Where(s => query.SearchPhrase == null || (s.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                        || s.Email.ToLower().Contains(query.SearchPhrase.ToLower())
                        || s.PhoneNumber.Contains(query.SearchPhrase.ToLower())));

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Supplier, object>>>
                {
                    {nameof(Supplier.Name), p => p.Name },
                    {nameof(Supplier.Email), p => p.Email },
                    {nameof(Supplier.PhoneNumber), p => p.PhoneNumber }
                };

                var selectedColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirectionEnum.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var suppliers = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var suppliersDtos = _mapper.Map<List<SupplierDto>>(suppliers);

            var result = new PagedResult<SupplierDto>(suppliersDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return Task.FromResult(result);
        }

        public Task<SupplierDetailDto> GetByIdAsync(int id)
        {
            var supplier = _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Address)
                .FirstOrDefault(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            var result  = _mapper.Map<SupplierDetailDto>(supplier);

            return Task.FromResult(result);
        }

        public Task<List<SupplierProductDto>> GetSupplierProductsAsync(int id)
        {
            var supplier = _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products) 
                .Select(s => s.Id)
                .FirstOrDefault(id);

            if (supplier == null)
                throw new NotFoundException("Supplier not found");

            var ProductList = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Categories)
                .Include(p => p.Layout)
                .Include(p => p.Statuses)
                .Where(p => p.Supplier.Id == id)
                .ProjectTo<SupplierProductDto>(_mapper.ConfigurationProvider)
                .ToList();

            var result = _mapper.Map<List<SupplierProductDto>>(ProductList);
            
            return Task.FromResult(result);

        }
    }
}
