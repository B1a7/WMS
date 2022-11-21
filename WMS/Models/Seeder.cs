using Bogus;
using WMS.Models.Entities;
using WMS.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace WMS.Models
{
    public class Seeder
    {
        private readonly WMSDbContext _dbContext;
        
        public Seeder(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void GenerateData()
        {
            if (_dbContext.Database.CanConnect() && _dbContext.Suppliers.Count() == 0)
            {
                //TrunkateDatabase();

                #region Roles 

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                }

                #endregion

                #region Users

                var admin = new User()
                {
                    Email = "admin@admin.com",
                    FirstName = "admin",
                    LastName = "admin",
                    PasswordHash = "admin",
                    RoleId = 3,                
                };

                _dbContext.Add(admin);
                
                #endregion

                #region Addresses

                var addressGenerator = new Faker<Address>()
                    .RuleFor(a => a.City, f => f.Address.City())
                    .RuleFor(a => a.Street, f => f.Address.StreetName())
                    .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                    .RuleFor(a => a.Country, f => f.Address.Country());

                #endregion

                #region Categories

                var categories = new[] { "Arts", "Automotive", "Computers", "Electronics", "Garden", "Grocery",
                    "Movies", "Toys", "Home", "Kitchen", "Office"};
                var categoryGenerator = new Faker<Category>()
                    .RuleFor(c => c.Name, f => f.Commerce.Color())
                    .RuleFor(c => c.HSCode, f => f.Commerce.Ean8());

                #endregion

                #region Statuses

                var statuses = new[] { "out of warehouse", "delivered", "placed in warehouse", "sent" };

                var statusInactiveGenerator = new Faker<Status>()
                    .RuleFor(s => s.IsActive, f => false)
                    .RuleFor(s => s.PackageStatus, f => f.PickRandom(statuses));

                var statusActiveGenerator = new Faker<Status>()
                    .RuleFor(s => s.IsActive, f => true)
                    .RuleFor(s => s.PackageStatus, f => f.PickRandom(statuses));

                #endregion

                #region Layouts

                var spotSizes = new[] { "small", "medium", "large" };

                List<Layout> layouts = new List<Layout>();

                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 1; j <= 20; j++)
                    {
                        for (int k = 1; k <= 10; k++)
                        {
                            var layoutsGenerator = new Faker<Layout>()
                                .RuleFor(l => l.SpotSize, f => f.PickRandom(spotSizes))
                                .RuleFor(l => l.PositionXYZ, f => $"{k}.{j}.{i}");
                            layouts.Add(layoutsGenerator.Generate());
                        }
                    }
                }

                _dbContext.Layouts.AddRange(layouts);
                #endregion

                #region Products

                var layoutId = 0;

                var productGenerator = new Faker<Product>()
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Quantity, f => f.PickRandom(1, 10))
                    .RuleFor(p => p.IsAvaiable, f => f.Random.Bool())
                    .RuleFor(p => p.ProductionDate, f => DateTime.Now)
                    .RuleFor(p => p.Layout, f => layouts[layoutId])
                    .RuleFor(p => p.Size, f => layouts[layoutId++].SpotSize)
                    .RuleFor(p => p.Categories, f => categoryGenerator.Generate(4))
                    .RuleFor(p => p.Statuses, f => new List<Status>(){
                        statusInactiveGenerator.Generate(),
                        statusInactiveGenerator.Generate(),
                        statusActiveGenerator.Generate()
                    });

                #endregion

                #region Suppliers

                var supplierGenerator = new Faker<Supplier>()
                    .RuleFor(s => s.Name, f => f.Name.FullName())
                    .RuleFor(s => s.Email, f => f.Internet.Email())
                    .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(s => s.Address, f => addressGenerator.Generate())
                    .RuleFor(s => s.Products, f => productGenerator.Generate(5));

                var supplier = supplierGenerator.Generate(100);

                #endregion


                _dbContext.AddRange(supplier);
                _dbContext.SaveChanges();
            } 
        }

        //TODO
        //private void TrunkateDatabase()
        //{
        //}

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                { Name = "none" },
                new Role()
                { Name = "warehouseman" },
                new Role()
                { Name = "manager" },
                new Role() 
                { Name = "admin" }
            };
            return roles;
        }

        private IEnumerable<Status> GetStatuses()
        {
            var statuses = new List<Status>()
            {
                new Status()
                { PackageStatus = "delivered" },
                new Status()
                { PackageStatus = "placed in stock" },
                new Status()
                { PackageStatus = "sent" }
            };
            return statuses;
        }



    }
}
