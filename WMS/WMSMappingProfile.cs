using AutoMapper;
using WMS.Models.Dtos.Customer;
using WMS.Models.Dtos.Documentation;
using WMS.Models.Dtos.Product;
using WMS.Models.Entities;

namespace WMS
{
    public class WMSMappingProfile : Profile
    {
        public WMSMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(m => m.SupplierName, c => c.MapFrom(e => e.Supplier.Name))
                .ForMember(m => m.PackageStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().PackageStatus))
                .ForMember(m => m.DateStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().DateStatus))
                .ForMember(m => m.CategoryName, c => c.MapFrom(e => e.Categories.Select(x => x.Name).ToList()));

            CreateMap<Product, ProductDetailDto>()
                .ForMember(m => m.SupplierName, c => c.MapFrom(e => e.Supplier.Name))
                .ForMember(m => m.SupplierEmail, c => c.MapFrom(e => e.Supplier.Email))
                .ForMember(m => m.SupplierPhoneNumber, c => c.MapFrom(e => e.Supplier.PhoneNumber))
                .ForMember(m => m.PackageStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().PackageStatus))
                .ForMember(m => m.DateStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().DateStatus))
                .ForMember(m => m.CategoryName, c => c.MapFrom(e => e.Categories.Select(x => x.Name).ToList()))
                .ForMember(m => m.CategoryHSCode, c => c.MapFrom(e => e.Categories.Select(x => x.HSCode).ToList()))
                .ForMember(m => m.Position, c => c.MapFrom(e => e.Layout.PositionXYZ));

            CreateMap<AddProductDto, Product>()
                .ForMember(m => m.Categories, c => c.MapFrom(dto => new List<Category>(){
                    new Category() { Name = dto.CategoryName, HSCode = dto.HSCode} }))
                .ForMember(m => m.Statuses, c => c.MapFrom(dto => new List<Status>(){ 
                    new Status() { PackageStatus = dto.Status, IsActive = dto.IsActive, DateStatus = dto.StatusRegistrationDate }}));
           
            CreateMap<Product, ProductScanQrDto>()
                .ForMember(m => m.SupplierName, c => c.MapFrom(e => e.Supplier.Name))
                .ForMember(m => m.PackageStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().PackageStatus))
                .ForMember(m => m.DateStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().DateStatus))
                .ForMember(m => m.CategoryName, c => c.MapFrom(e => e.Categories.Select(x => x.Name).ToList()));


            CreateMap<Supplier, SupplierDto>();

            CreateMap<Supplier, SupplierScanQrDto>();

            CreateMap<Supplier, SupplierDetailDto>()
                .ForMember(m => m.City, c => c.MapFrom(e => e.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(e => e.Address.Street))
                .ForMember(m => m.Country, c => c.MapFrom(e => e.Address.Country))
                .ForMember(m => m.PostalCode, c => c.MapFrom(e => e.Address.PostalCode));

            CreateMap<Supplier, UpdateSupplierDto>()
                .ForMember(m => m.City, c => c.MapFrom(e => e.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(e => e.Address.Street))
                .ForMember(m => m.Country, c => c.MapFrom(e => e.Address.Country))
                .ForMember(m => m.PostalCode, c => c.MapFrom(e => e.Address.PostalCode));

            CreateMap<AddSupplierDto, Supplier>()
                .ForMember(s => s.Address, c => c.MapFrom(dto => new Address()
                { City = dto.City, Street = dto.Street, Country = dto.Country, PostalCode = dto.PostalCode }));

            CreateMap<Product, SupplierProductDto>()
                .ForMember(m => m.DateStatus, c => c.MapFrom(e => e.Statuses.FirstOrDefault(x => x.IsActive).DateStatus))
                .ForMember(m => m.PackageStatus, c => c.MapFrom(e => e.Statuses.FirstOrDefault(x => x.IsActive).PackageStatus))
                .ForMember(m => m.Position, c => c.MapFrom(e => e.Layout.PositionXYZ));

        }
    }
}
