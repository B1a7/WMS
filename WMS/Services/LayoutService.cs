using AutoMapper;
using WMS.Enums;
using WMS.Models;
using Microsoft.EntityFrameworkCore;
using WMS.Exceptions;
using WMS.Converters;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Dtos.LayoutDtos;

namespace WMS.Services
{
    public interface ILayoutService
    {
        Task<int> GetCapacityAsync();
        Task<int> GetDetailCapacityAsync(string size);
        Task<int> GetWarehouseFillingAsync();
        Task<int> GetWarehouseDetailFillingAsync(string size);
        Task<ProductDto> GetPlacementProductAsync(int layoutId);
        Task<Dictionary<int, Coordinates>> GetMapAsync();

    }


    public class LayoutService : ILayoutService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;


        public LayoutService(WMSDbContext dbContext, ILogger<ProductService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }


        public Task<int> GetCapacityAsync()
        {
            var capacity = _dbContext.Layouts
                .AsNoTracking()
                .Count();

            return Task.FromResult(capacity);
        }

        public Task<int> GetDetailCapacityAsync(string size)
        {
            if (!SpotSize.SpotSizes.Contains(size.ToLower()))
                throw new BadRequestException($"Wrong size name pick one of {SpotSize.SpotSizes.ToList()}");

            var capacity = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.SpotSize == size)
                .Count();
            
            return Task.FromResult(capacity);
        }

        public Task<Dictionary<int, Coordinates>> GetMapAsync()
        {
            var map = _dbContext.Layouts
                .AsNoTracking()
                .ToList();

            var result = new Dictionary<int, Coordinates>();

            foreach (var layout in map)
            {
                result.Add(layout.Id, layout.PositionXYZ.ConvertToStruct());
            }

            return Task.FromResult(result);
        }

        public Task<ProductDto> GetPlacementProductAsync(int layoutId)
        {
            var product = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.Id == layoutId)
                .Include(l => l.Product)
                .Select(l => l.Product)
                .FirstOrDefault();

            var result = _mapper.Map<ProductDto>(product);

            return Task.FromResult(result);
        }

        public Task<int> GetWarehouseDetailFillingAsync(string size)
        {

            if (!SpotSize.SpotSizes.Contains(size.ToLower()))
                throw new BadRequestException("Wrong size name");

            var detailFilling = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.SpotSize == size && l.Product != null)
                .Count();

            return Task.FromResult(detailFilling);
        }
        
        public Task<int> GetWarehouseFillingAsync()
        {
            var filling = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.Product != null)
                .Count();

            return Task.FromResult(filling);
        }

    }
}
