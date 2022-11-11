using AutoMapper;
using WMS.Models;

namespace WMS.Services
{
    public interface IWarehouseService
    {
        int GetWarehouseCapacity();
    }

    public class WarehouseService : IWarehouseService
    {
        private readonly WMSDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public WarehouseService(WMSDbContext dbContext, ILogger<ProductService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }


        public int GetWarehouseCapacity()
        {
            throw new NotImplementedException();
        }
    }
}
