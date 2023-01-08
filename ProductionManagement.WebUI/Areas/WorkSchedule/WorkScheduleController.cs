using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Common.Enums;
using ProductionManagement.Services.Services.WorkSchedule;
using ProductionManagement.Services.Services.WorkSchedule.Models;
using ProductionManagement.WebUI.Areas.WorkSchedule.ViewModels;

namespace ProductionManagement.WebUI.Areas.WorkSchedule
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkScheduleController : ControllerBase
    {
        private readonly IWorkScheduleService _workScheduleService;
        private readonly IMapper _mapper;

        public WorkScheduleController(
            IWorkScheduleService workScheduleService, IMapper mapper)
        {
            _workScheduleService = workScheduleService;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetProductionDays(GetProdDaysFilterVM model)
        {
            var result = await _workScheduleService.GetProductionDaysAsync(_mapper.Map<GetProdDaysFilterModel>(model));
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCalendarHeaders()
        {
            var result = await _workScheduleService.GetCalendarHeadersAsync();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeWorkDay(ProductionDaysBasicVM model)
        {
            var result = await _workScheduleService.ChangeWorkDayAsync((ChangeWorkDayOptionEnum) int.Parse(model.ProductionLineName), _mapper.Map<ProductionDaysBasicModel>(model));
            return Ok(result);
        }
    }
}