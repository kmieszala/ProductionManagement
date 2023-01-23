using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Common.Consts;
using ProductionManagement.Services.Services.Tanks;
using ProductionManagement.Services.Services.Tanks.Models;
using ProductionManagement.WebUI.Areas.Tanks.ViewModels.Request;

namespace ProductionManagement.WebUI.Areas.Tanks
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TanksController : ControllerBase
    {
        private readonly ITanksService _tanksService;
        private readonly IMapper _mapper;

        public TanksController(
            ITanksService tanksService, IMapper mapper)
        {
            _tanksService = tanksService;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.SettingsView)]
        public async Task<IActionResult> GetTanks([FromBody] bool active)
        {
            var result = await _tanksService.GetTanksAsync(active);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.Settings)]
        public async Task<IActionResult> AddTank(TankRequestVM model)
        {
            var result = await _tanksService.AddTankAsync(_mapper.Map<TankModel>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.Settings)]
        public async Task<IActionResult> EditTank(TankRequestVM model)
        {
            var result = await _tanksService.EditTankAsync(_mapper.Map<TankModel>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.Settings)]
        public async Task<IActionResult> DeactiveTank([FromBody] int model)
        {
            var result = await _tanksService.ChangeTankStatusAsync(model, false);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.Settings)]
        public async Task<IActionResult> ActiveTank([FromBody] int model)
        {
            var result = await _tanksService.ChangeTankStatusAsync(model, true);
            return Ok(result);
        }
    }
}