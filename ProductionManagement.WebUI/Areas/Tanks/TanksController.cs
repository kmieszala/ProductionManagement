﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Services.Services.Tanks;
using ProductionManagement.Services.Services.Tanks.Models;
using ProductionManagement.WebUI.Areas.Tanks.ViewModels.Request;

namespace ProductionManagement.WebUI.Areas.Tanks
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("[action]")]
        public async Task<IActionResult> Echo()
        {
            return Ok("Elo");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTanks()
        {
            var result = await _tanksService.GetTanksAsync();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddTank(TankRequestVM model)
        {
            var result = await _tanksService.AddTankAsync(_mapper.Map<TankModel>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> EditTank(TankRequestVM model)
        {
            var result = await _tanksService.EditTankAsync(_mapper.Map<TankModel>(model));
            return Ok(result);
        }
    }
}