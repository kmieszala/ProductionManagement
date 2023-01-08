using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Services.Services.Orders;
using ProductionManagement.Services.Services.Orders.Models;
using ProductionManagement.WebUI.Areas.Orders.ViewModels.Request;

namespace ProductionManagement.WebUI.Areas.Orders
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IMapper _mapper;

        public OrdersController(
            IOrdersService ordersService, IMapper mapper)
        {
            _ordersService = ordersService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _ordersService.GetOrdersAsync();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddOrder(OrderVM model)
        {
            var result = await _ordersService.AddOrderAsync(_mapper.Map<OrderModel>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> EditOrder(OrderVM model)
        {
            var result = await _ordersService.EditOrderAsync(_mapper.Map<OrderModel>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PrepareStorekeeperDocument(StorekeeperDocumentParametersVM model)
        {
            var result = await _ordersService.PrepareStorekeeperDocumentAsync(model.OrdersIds, _mapper.Map<List<PartsStorekeeperModel>>(model.Parts));

            var bytes = new byte[0];
            using (var ms = new MemoryStream())
            {
                result.SaveAs(ms);
                bytes = ms.ToArray();
            }

            return File(bytes, "application/octet-stream");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateSequenceOrders(List<SequenceVM> model)
        {
            var result = await _ordersService.UpdateSequenceOrdersAsync(_mapper.Map<List<SequenceModel>>(model));
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GenerateCalendar(List<OrderVM> model)
        {
            var result = await _ordersService.GenerateCalendarAsync(_mapper.Map<List<OrderModel>>(model));
            return Ok(result);
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> GetPlannedOrders(FilterPlannedOrdersVM model)
        //{
        //    var result = await _ordersService.GetPlannedOrdersAsync(_mapper.Map<FilterPlannedOrdersModel>(model));
        //    return Ok(result);
        //}
    }
}