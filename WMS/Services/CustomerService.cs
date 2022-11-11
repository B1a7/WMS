using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Enums;
using WMS.Exceptions;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Dtos.Customer;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface ICustomerService
    {
        int AddSupplier(AddSupplierDto dto);
        void Update(UpdateSupplierDto dto, int id);
        void Delete(int id);
        PagedResult<SupplierDto> GetAll(SupplierQuery query);
        SupplierDetailDto GetById(int id);
        List<SupplierProductDto> GetSupplierProducts(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;


        public CustomerService(WMSDbContext dbContext, ILogger<ProductService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }


        public int AddSupplier(AddSupplierDto dto)
        {
            var supplier = _mapper.Map<Supplier>(dto);

            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();

            return supplier.Id;
        }

        public void Update(UpdateSupplierDto dto, int id)
        {
            _logger.LogError($"Supplier with id: {id} UPDATE action invoked");

            var supplier = _dbContext.Suppliers
                .Include(s => s.Address)
                .FirstOrDefault(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            supplier.Name = dto.Name;
            supplier.Email = dto.Email;
            supplier.PhoneNumber = dto.PhoneNumber;
            supplier.Address.City = dto.City;
            supplier.Address.Street = dto.Street;
            supplier.Address.Country = dto.Country;
            supplier.Address.PostalCode = dto.PostalCode;

            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogError($"Supplier with id: {id} DELETE action invoked");

            var supplier = _dbContext.Suppliers
                .FirstOrDefault(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            _dbContext.Suppliers.Remove(supplier);
            _dbContext.SaveChanges();
        }

        public PagedResult<SupplierDto> GetAll(SupplierQuery query)
        {
            var baseQuery = _dbContext.Suppliers
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
                .Include(s => s.Products)      
                .FirstOrDefault(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            var ProductList = _dbContext.Products
                .Include(p => p.Categories)
                .Include(p => p.Layout)
                .Include(p => p.Statuses)
                .Where(p => p.Supplier.Id == id)
                .ToList();

            var result = _mapper.Map<List<SupplierProductDto>>(ProductList);
            
            return result;

        }
    }
}
