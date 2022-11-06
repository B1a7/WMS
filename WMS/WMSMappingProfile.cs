using AutoMapper;
using WMS.Models.Dtos;
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
                .ForMember(m => m.SupplierName, c => c.MapFrom(e => e.Supplier.Email))
                .ForMember(m => m.SupplierName, c => c.MapFrom(e => e.Supplier.PhoneNumber))
                .ForMember(m => m.PackageStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().PackageStatus))
                .ForMember(m => m.DateStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().DateStatus))
                .ForMember(m => m.CategoryName, c => c.MapFrom(e => e.Categories.Select(x => x.Name).ToList()))
                .ForMember(m => m.CategoryHSCode, c => c.MapFrom(e => e.Categories.Select(x => x.HSCode).ToList()));

            CreateMap<AddProductDto, Product>()
                .ForMember(m => m.Categories, c => c.MapFrom(dto => new List<Category>(){
                    new Category() { Name = dto.CategoryName, HSCode = dto.HSCode} }))
                .ForMember(m => m.Statuses, c => c.MapFrom(dto => new List<Status>(){ 
                    new Status() { PackageStatus = dto.Status, IsActive = dto.IsActive, DateStatus = dto.StatusRegistrationDate }}));



        }
    }
}
