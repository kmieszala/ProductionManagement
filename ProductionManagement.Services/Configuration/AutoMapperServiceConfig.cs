using AutoMapper;
using ProductionManagement.Model.DbSets;
using ProductionManagement.Services.Services.Orders.Models;
using ProductionManagement.Services.Services.Parts.Models;
using ProductionManagement.Services.Services.ProductionLine.Models;
using ProductionManagement.Services.Services.Tanks.Models;

namespace ProductionManagement.Services.Configuration
{
    public class AutoMapperServiceConfig : Profile
    {
        public AutoMapperServiceConfig()
        {
            CreateMap<PartModel, Parts>();
            CreateMap<Parts, PartModel>();

            CreateMap<OrderModel, Orders>();
            CreateMap<Orders, OrderModel>();

            CreateMap<TankModel, Tanks>()
                .ForMember(dest => dest.TankParts, opts => opts.MapFrom(src => src.Parts))
                .ForMember(dest => dest.LineTank, opts => opts.MapFrom(src => src.ProductionLines));
            CreateMap<Tanks, TankModel>()
                .ForMember(dest => dest.ProductionLines, opts => opts.MapFrom(src => src.LineTank))
                .ForMember(dest => dest.Parts, opts => opts.MapFrom(src => src.TankParts));

            CreateMap<TankPartsModel, TankParts>();
            CreateMap<TankParts, TankPartsModel>()
                .ForMember(dest => dest.PartsName, opts => opts.MapFrom(src => src.Parts.Name));

            CreateMap<ProductionLineModel, ProductionLine>()
                .ForMember(dest => dest.LineTank, opts => opts.MapFrom(src => src.Tanks));
            CreateMap<ProductionLine, ProductionLineModel>()
                .ForMember(dest => dest.Tanks, opts => opts.MapFrom(src => src.LineTank));

            CreateMap<LineTank, LineTankModel>();
            CreateMap<LineTankModel, LineTank>();

            CreateMap<LineTank, ProductionLineTankModel>();
            CreateMap<ProductionLineTankModel, LineTank>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.LineTankId))
                .ForMember(dest => dest.ProductionLineId, opts => opts.MapFrom(src => src.ProductionLineId));
        }
    } 
}
