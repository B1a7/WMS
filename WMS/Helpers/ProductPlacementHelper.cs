using WMS.Enums;
using WMS.Exceptions;
using WMS.Models;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IProductPlacementHelper
    {
        void ModifyProductPlacement(Product product, string newPackageStatus);
    }

    public class ProductPlacementHelper : IProductPlacementHelper
    {
        private readonly WMSDbContext _dbContext;

        public ProductPlacementHelper(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        private void AddToWarehouse(Product product)
        {
            var emptySpots = _dbContext.Layouts
                .Where(l => l.Product == null && l.SpotSize == product.Size)
                .FirstOrDefault();

            if (emptySpots == null)
                throw new NoEmptySpotException("Cannot add product to the warehouse (no empty spot)");
            
            product.Layout = emptySpots;
            _dbContext.SaveChanges();
        }

        private void RemoveFromWarehouse(Product product)
        {
            product.LayoutId = null;
            _dbContext.SaveChanges();
        }


        public void ModifyProductPlacement(Product product, string newPackageStatus)
        {
            var currentStatus = product.Statuses
                .FirstOrDefault(s => s.IsActive == true);

            if (currentStatus.PackageStatus == PackageStatus.PlacedInWarehouse 
                    && newPackageStatus == PackageStatus.PlacedInWarehouse)
                return;
            else if (newPackageStatus == PackageStatus.PlacedInWarehouse)
                AddToWarehouse(product);
            else
                RemoveFromWarehouse(product);
        }

    }
}
