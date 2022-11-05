﻿using AutoMapper;
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
                .ForMember(m => m.DateStatus, c => c.MapFrom(e => e.Statuses.Where(x => x.IsActive).FirstOrDefault().DateStatus));


    }
    }
}