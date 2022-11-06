using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Enums;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Dtos.Customer;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface ICustomerService
    {
        PagedResult<SupplierDto> GetAll(SupplierQuery query);
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

                baseQuery = query.SortDirection == SortDirection.ASC
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

    }
}
