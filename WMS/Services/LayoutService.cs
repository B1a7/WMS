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
        int GetCapacity();
        int GetDetailCapacity(string size);
        int GetWarehouseFilling();
        int GetWarehouseDetailFilling(string size);
        ProductDto GetPlacementProduct(int layoutId);
        Dictionary<int, Coordinates> GetMap();

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


        public int GetCapacity()
        {
            var capacity = _dbContext.Layouts
                .AsNoTracking()
                .Count();

            return capacity;
        }

        public int GetDetailCapacity(string size)
        {
            if (!SpotSize.SpotSizes.Contains(size.ToLower()))
                throw new BadRequestException($"Wrong size name pick one of {SpotSize.SpotSizes.ToList()}");

            var capacity = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.SpotSize == size)
                .Count();

            return capacity;
        }

        public Dictionary<int, Coordinates> GetMap()
        {
            var map = _dbContext.Layouts
                .AsNoTracking()
                .ToList();

            var result = new Dictionary<int, Coordinates>();

            foreach (var layout in map)
            {
                result.Add(layout.Id, layout.PositionXYZ.ConvertToStruct());
            }

            return result;
        }

        public ProductDto GetPlacementProduct(int layoutId)
        {
            var product = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.Id == layoutId)
                .Include(l => l.Product)
                .Select(l => l.Product)
                .FirstOrDefault();

            var result = _mapper.Map<ProductDto>(product);

            return result;
        }

        public int GetWarehouseDetailFilling(string size)
        {

            if (!SpotSize.SpotSizes.Contains(size.ToLower()))
                throw new BadRequestException("Wrong size name");

            var filling = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.SpotSize == size && l.Product != null)
                .Count();

            return filling;
        }
        
        public int GetWarehouseFilling()
        {
            var occupied = _dbContext.Layouts
                .AsNoTracking()
                .Where(l => l.Product != null)
                .Count();

            return occupied;
        }

    }
}
