using AutoMapper;
using ProductionManagement.Model.DbSets;
using ProductionManagement.Services.Services.Parts.Models;

namespace ProductionManagement.Services.Configuration
{
    public class AutoMapperServiceConfig : Profile
    {
        public AutoMapperServiceConfig()
        {
            CreateMap<PartModel, Parts>();
        //    CreateMap<UserModel, Users>()
        //    .ForMember(dest => dest.UserProfile, opts => opts.MapFrom(src => src.UserProfile));
        }
    } 
}
