using AutoMapper;
using ProductionManagement.Model.DbSets;
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

            CreateMap<TankModel, Tanks>()
                .ForMember(dest => dest.TankParts, opts => opts.MapFrom(src => src.Parts));
            CreateMap<Tanks, TankModel>()
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
        }
    } 
}
