using WMS.Enums;
using WMS.Exceptions;
using WMS.Models;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IProductPlacementHelper
    {
        Task<bool> ModifyProductPlacementAsync(Product product, string newPackageStatus);
    }

    public class ProductPlacementHelper : IProductPlacementHelper
    {
        private readonly WMSDbContext _dbContext;

        public ProductPlacementHelper(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        private async Task<bool> AddToWarehouseAsync(Product product)
        {
            var emptySpots = _dbContext.Layouts
                .Where(l => l.Product == null && l.SpotSize == product.Size)
                .FirstOrDefault();

            if (emptySpots == null)
                throw new NoEmptySpotException("Cannot add product to the warehouse (no empty spot)");
            
            product.Layout = emptySpots;
            _dbContext.Update(product);
            int result = await _dbContext.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        private async Task<bool> RemoveFromWarehouseAsync(Product product)
        {
            product.LayoutId = null;

            _dbContext.Update(product);
            int result = await _dbContext.SaveChangesAsync();

            return result > 0 ? true : false;
        }


        public async Task<bool> ModifyProductPlacementAsync(Product product, string newPackageStatus)
        {
            var currentStatus = product.Statuses
                .FirstOrDefault(s => s.IsActive == true);

            bool result = false;

            if (currentStatus != null)
            {
                if (currentStatus.PackageStatus == PackageStatus.PlacedInWarehouse
                        && newPackageStatus == PackageStatus.PlacedInWarehouse)
                    result =  true;

                else if (newPackageStatus == PackageStatus.PlacedInWarehouse)
                    result =  await AddToWarehouseAsync(product);
                else
                    result =  await RemoveFromWarehouseAsync(product);
            }

            if (!result)
                throw new InternalServerErrorException("Cannot change product placement");

            return result;
        }

    }
}
