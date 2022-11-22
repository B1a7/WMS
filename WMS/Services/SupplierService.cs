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
        int AddSupplier(AddSupplierDto dto, string loggedUserId);
        void Update(UpdateSupplierDto dto, int id, string loggedUserId);
        void Delete(int id, string loggedUserId);
        PagedResult<SupplierDto> GetAll(SupplierQuery query);
        SupplierDetailDto GetById(int id);
        List<SupplierProductDto> GetSupplierProducts(int id);
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


        public int AddSupplier(AddSupplierDto dto, string loggedUserId)
        {
            var supplier = _mapper.Map<Supplier>(dto);

            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();
            _journalHepler.CreateJournal(OperationTypeEnum.Add, supplier.GetType().Name.ToString(), supplier.Id, loggedUserId);

            return supplier.Id;
        }

        public void Update(UpdateSupplierDto dto, int id, string loggedUserId)
        {
            _logger.LogError($"Supplier with id: {id} UPDATE action invoked");

            var supplier = _dbContext.Suppliers
                .Include(s => s.Address)
                .FirstOrDefault(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            var updatedSupllier = _mapper.Map<Supplier>(dto);

            _dbContext.Update(updatedSupllier);

            _journalHepler.CreateJournal(OperationTypeEnum.Update, supplier.GetType().Name.ToString(), supplier.Id, loggedUserId);

        }

        public void Delete(int id, string loggedUserId)
        {
            _logger.LogError($"Supplier with id: {id} DELETE action invoked");


            Supplier supplier = new Supplier()
            {
                Id = id,
            };

            _dbContext.Entry(supplier).State = EntityState.Deleted;

            var result = _dbContext.SaveChanges();


            if (result == 0)
                throw new NotFoundException("Supplier not found");

            _journalHepler.CreateJournal(OperationTypeEnum.Delete, supplier.GetType().Name.ToString(), supplier.Id, loggedUserId);

        }

        public PagedResult<SupplierDto> GetAll(SupplierQuery query)
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

            return result;
        }

        public SupplierDetailDto GetById(int id)
        {
            var supplier = _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Address)
                .FirstOrDefault(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            var result  = _mapper.Map<SupplierDetailDto>(supplier);

            return result;
        }

        public List<SupplierProductDto> GetSupplierProducts(int id)
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
            
            return result;

        }
    }
}
