﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Common.Consts;
using ProductionManagement.Services.Services.ProductionLine;
using ProductionManagement.Services.Services.ProductionLine.Models;
using ProductionManagement.WebUI.Areas.ProductionLine.ViewModels.Request;

namespace ProductionManagement.WebUI.Areas.ProductionLine
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionLineController : ControllerBase
    {
        private readonly IProductionLineService _productionLineService;
        private readonly IMapper _mapper;

        public ProductionLineController(
            IProductionLineService productionLineService, IMapper mapper)
        {
            _productionLineService = productionLineService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        [Authorize(Policy = CustomPolicy.ProductionLinesView)]
        public async Task<IActionResult> GetLines()
        {
            var result = await _productionLineService.GetLinesAsync();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductionLines()
        {
            var result = await _productionLineService.GetProductionLinesAsync();
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.ProductionLines)]
        public async Task<IActionResult> AddLine(ProductionLineVM model)
        {
            var result = await _productionLineService.AddLineAsync(_mapper.Map<ProductionLineModel>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Policy = CustomPolicy.ProductionLines)]
        public async Task<IActionResult> EditLine(ProductionLineVM model)
        {
            var result = await _productionLineService.EditLineAsync(_mapper.Map<ProductionLineModel>(model));
            return Ok(result);
        }
    }
}