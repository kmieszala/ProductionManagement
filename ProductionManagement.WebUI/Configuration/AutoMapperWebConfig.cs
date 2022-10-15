using AutoMapper;
using ProductionManagement.Services.Services.Orders.Models;
using ProductionManagement.Services.Services.Parts.Models;
using ProductionManagement.Services.Services.ProductionLine.Models;
using ProductionManagement.Services.Services.Tanks.Models;
using ProductionManagement.Services.Services.WorkSchedule.Models;
using ProductionManagement.WebUI.Areas.Orders.ViewModels.Request;
using ProductionManagement.WebUI.Areas.Parts.ViewModels.Request;
using ProductionManagement.WebUI.Areas.ProductionLine.ViewModels.Request;
using ProductionManagement.WebUI.Areas.Tanks.ViewModels.Request;
using ProductionManagement.WebUI.Areas.WorkSchedule.ViewModels;

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

            CreateMap<ProductionLineVM, ProductionLineModel>();
            CreateMap<ProductionLineModel, ProductionLineVM>();

            CreateMap<LineTankVM, LineTankModel>();
            CreateMap<LineTankModel, LineTankVM>();

            CreateMap<ProductionLineTankVM, ProductionLineTankModel>();
            CreateMap<ProductionLineTankModel, ProductionLineTankVM>();

            CreateMap<OrderVM, OrderModel>();
            CreateMap<OrderModel, OrderVM>();

            CreateMap<PartsStorekeeperVM, PartsStorekeeperModel>();
            CreateMap<PartsStorekeeperModel, PartsStorekeeperVM>();

            CreateMap<SequenceModel, SequenceVM>();
            CreateMap<SequenceVM, SequenceModel>();

            CreateMap<FilterPlannedOrdersModel, FilterPlannedOrdersVM>();
            CreateMap<FilterPlannedOrdersVM, FilterPlannedOrdersModel>();

            CreateMap<GetProdDaysFilterModel, GetProdDaysFilterVM>();
            CreateMap<GetProdDaysFilterVM, GetProdDaysFilterModel>();

            CreateMap<ProductionDaysBasicVM, ProductionDaysBasicModel>();
            CreateMap<ProductionDaysBasicModel, ProductionDaysBasicVM>();
        }
    }
}