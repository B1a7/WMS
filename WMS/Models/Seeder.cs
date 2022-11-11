using Bogus;
using WMS.Models.Entities;
using WMS.ExtensionMethods;

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
            if (_dbContext.Database.CanConnect())
            {
                Randomizer.Seed = new Random(911);

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                var addressGenerator = new Faker<Address>()
                    .RuleFor(a => a.City, f => f.Address.City())
                    .RuleFor(a => a.Street, f => f.Address.StreetName())
                    .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                    .RuleFor(a => a.Country, f => f.Address.Country());


                var categoryGenerator = new Faker<Category>()
                    .RuleFor(c => c.Name, f => f.Commerce.Color())
                    .RuleFor(c => c.HSCode, f => f.Commerce.Ean8());


                var statuses = new[] { "out of warehouse", "delivered", "placed in warehouse", "sent" };
                var statusGenerator = new Faker<Status>()
                    .RuleFor(s => s.IsActive, f => f.Random.Bool())
                    .RuleFor(s => s.PackageStatus, f => f.PickRandom(statuses));

                var random = new Random();
                var spotSizes = new[] { "small", "medium", "large" };
                var layoutsGenerator = new Faker<Layout>()
                    .RuleFor(l => l.SpotSize, f => f.PickRandom(spotSizes))
                    .RuleFor(l => l.PositionXYZ, f => $"{random.Next(1,10)}.{random.Next(1, 10)}.1");


                var sizes = new[] { "small", "medium", "large" };
                var productGenerator = new Faker<Product>()
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Quantity, f => f.PickRandom(1, 10))
                    .RuleFor(p => p.IsAvaiable, f => f.Random.Bool())
                    .RuleFor(p => p.ProductionDate, f => DateTime.Now)
                    .RuleFor(p => p.Size, f => f.PickRandom(sizes))
                    .RuleFor(p => p.Layout, f => layoutsGenerator.Generate())
                    .RuleFor(p => p.Categories, f => categoryGenerator.Generate(5))
                    .RuleFor(p => p.Statuses, f => statusGenerator.Generate(2));

                var supplierGenerator = new Faker<Supplier>()
                    .RuleFor(s => s.Name, f => f.Name.FullName())
                    .RuleFor(s => s.Email, f => f.Internet.Email())
                    .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(s => s.Address, f => addressGenerator.Generate())
                    .RuleFor(s => s.Products, f => productGenerator.Generate(20));

                var supplier = supplierGenerator.Generate(20);

                _dbContext.AddRange(supplier);
                _dbContext.SaveChanges();
            } 
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                { Name = "User" },
                new Role()
                { Name = "Manager" },
                new Role() 
                { Name = "Admin" }
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
