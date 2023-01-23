using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Common.Consts;
using ProductionManagement.Services.Services.Parts;
using ProductionManagement.Services.Services.Parts.Models;
using ProductionManagement.WebUI.Areas.Parts.ViewModels.Request;

namespace ProductionManagement.WebUI.Areas.Parts
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PartsController : ControllerBase
    {
        private readonly IPartsService _partsService;
        private readonly IMapper _mapper;

        public PartsController(
            IPartsService partsService, IMapper mapper)
        {
            _partsService = partsService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        [Authorize(Policy = CustomPolicy.SettingsView)]
        public async Task<IActionResult> GetParts()
        {
            var result = await _partsService.GetPartsAsync();
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.Settings)]
        public async Task<IActionResult> AddPart(PartRequestVM model)
        {
            var result = await _partsService.AddPartAsync(_mapper.Map<PartModel>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.Settings)]
        public async Task<IActionResult> EditPart(PartRequestVM model)
        {
            var result = await _partsService.EditPartAsync(_mapper.Map<PartModel>(model));
            return Ok(result);
        }
    }
}