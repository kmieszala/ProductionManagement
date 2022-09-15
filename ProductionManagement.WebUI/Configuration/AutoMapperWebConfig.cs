using AutoMapper;
using ProductionManagement.Services.Services.Parts.Models;
using ProductionManagement.Services.Services.Tanks.Models;
using ProductionManagement.WebUI.Areas.Parts.ViewModels.Request;
using ProductionManagement.WebUI.Areas.Tanks.ViewModels.Request;

namespace ProductionManagement.WebUI.Configuration
{
    public class AutoMapperWebConfig : Profile
    {
        public AutoMapperWebConfig()
        {
            CreateMap<PartRequestVM, PartModel>();
            CreateMap<PartModel, PartRequestVM>();

            CreateMap<TankRequestVM, TankModel>();
            CreateMap<TankModel, TankRequestVM>();

            CreateMap<TankPartsVM, TankPartsModel>();
            CreateMap<TankPartsModel, TankPartsVM>();
        }
    }
}
