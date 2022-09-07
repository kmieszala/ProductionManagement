using AutoMapper;
using ProductionManagement.Services.Services.Parts.Models;
using ProductionManagement.WebUI.Areas.Parts.ViewModels.Request;

namespace ProductionManagement.WebUI.Configuration
{
    public class AutoMapperWebConfig : Profile
    {
        public AutoMapperWebConfig()
        {
            CreateMap<PartRequestVM, PartModel>();
            CreateMap<PartModel, PartRequestVM>();
        }
    }
}
