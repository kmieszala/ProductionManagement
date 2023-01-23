using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Common.Enums;
using ProductionManagement.Model;
using ProductionManagement.Model.DbSets;
using ProductionManagement.Services.Services.WorkSchedule.Models;

namespace ProductionManagement.Services.Services.WorkSchedule
{
    public interface IWorkScheduleService
    {
        Task<List<ProductionDaysModel>> GetProductionDaysAsync(GetProdDaysFilterModel parameters);

        Task<List<string>> GetCalendarHeadersAsync();

        Task<bool> ChangeWorkDayAsync(ChangeWorkDayOptionEnum changeOption, ProductionDaysBasicModel productionDay);
    }

    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly ProductionManagementContext _context;
        private readonly IMapper _mapper;

        public WorkScheduleService(ProductionManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ChangeWorkDayAsync(ChangeWorkDayOptionEnum changeOption, ProductionDaysBasicModel productionDay)
        {
            switch (changeOption)
            {
                case ChangeWorkDayOptionEnum.SetWorkDay:
                    await ChangeProductionDayOrRemoveAsync(productionDay, false);
                    break;

                case ChangeWorkDayOptionEnum.SetDayFree:
                    await ChangeProductionDayOrRemoveAsync(productionDay, true);
                    break;

                case ChangeWorkDayOptionEnum.RemoveOrder:
                    await RemoveOrderFromCalendarAsync(productionDay, false);
                    break;

                case ChangeWorkDayOptionEnum.RemoveOrderAndNext:
                    await RemoveOrderFromCalendarAsync(productionDay, true);
                    break;

                case ChangeWorkDayOptionEnum.SetDayFreeWitchMove:
                    await SetDayFreeWitchMoveAsync(productionDay);
                    break;
            }

            return true;
        }

        public async Task<List<string>> GetCalendarHeadersAsync()
        {
            var result = await _context.ProductionLine
                .Where(x => x.Active)
                .OrderBy(x => x.Id)
                .Select(x => x.Name)
                .ToListAsync();

            result.Insert(0, "Data");

            return result;
        }

        public async Task<List<ProductionDaysModel>> GetProductionDaysAsync(GetProdDaysFilterModel parameters)
        {
            List<ProductionDaysModel> result = new List<ProductionDaysModel>();

            var orders = await _context.Orders
                .Where(x => x.ProductionLine != null && x.ProductionLine.Active)
                .Where(x => x.StartDate >= parameters.DateFrom.Date || x.StopDate <= parameters.DateTo.Date)
                .OrderBy(x => x.ProductionLineId)
                .AsNoTracking()
                .ToListAsync();

            var productionLinesIds = await _context.ProductionLine
                .Where(x => x.Active)
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .ToListAsync();

            var productionDays = await _context.ProductionDays
                .Where(x => x.Date.Date >= parameters.DateFrom.Date || x.Date.Date <= parameters.DateTo.Date)
                .Where(x => productionLinesIds.Contains(x.ProductionLineId))
                .OrderByDescending(x => x.ProductionLineId)
                .AsNoTracking()
                .ToListAsync();

            var dateTMP = parameters.DateFrom;
            while (dateTMP <= parameters.DateTo.Date)
            {
                List<ProductionDaysBasicModel> list = new List<ProductionDaysBasicModel>();
                foreach (var prodLId in productionLinesIds)
                {
                    var freeDay = productionDays
                        .Where(x => x.Date.Date == dateTMP.Date)
                        .Where(x => x.ProductionLineId == prodLId)
                        .FirstOrDefault();
                    if (freeDay != null && freeDay.DayOff)
                    {
                        list.Add(new ProductionDaysBasicModel()
                        {
                            DayOff = freeDay.DayOff,
                            ProductionLineId = prodLId,
                            Id = freeDay.Id,
                            Date = dateTMP.Date,
                        });
                    }
                    else
                    {
                        var tmp = orders
                            .Where(x => x.StartDate.HasValue && x.StartDate.Value.Date <= dateTMP.Date)
                            .Where(x => x.StopDate.HasValue && x.StopDate.Value.Date >= dateTMP.Date)
                            .Where(x => x.ProductionLineId == prodLId)
                            .FirstOrDefault();

                        if (tmp != null && ((freeDay != null && !freeDay.DayOff) || (dateTMP.Date.DayOfWeek != DayOfWeek.Saturday && dateTMP.Date.DayOfWeek != DayOfWeek.Sunday)))
                        {
                            list.Add(new ProductionDaysBasicModel()
                            {
                                DayOff = false,
                                ProductionLineId = prodLId,
                                Color = tmp.Color,
                                OrderName = tmp.OrderName,
                                OrdersId = tmp.Id,
                                Date = dateTMP.Date,
                            });
                        }
                        else
                        {
                            list.Add(new ProductionDaysBasicModel()
                            {
                                DayOff = freeDay != null ? freeDay.DayOff : dateTMP.Date.DayOfWeek == DayOfWeek.Saturday || dateTMP.Date.DayOfWeek == DayOfWeek.Sunday,
                                ProductionLineId = prodLId,
                                Date = dateTMP.Date,
                            });
                        }
                    }
                }

                result.Add(new ProductionDaysModel()
                {
                    CalendarDay = dateTMP.Date,
                    ProductionDay = list,
                });

                dateTMP = dateTMP.AddDays(1);
            }

            return result;
        }

        private async Task RemoveOrderFromCalendarAsync(ProductionDaysBasicModel productionDay, bool removeNextOrders)
        {
            var maxSequence = await _context.Orders.MaxAsync(x => x.Sequence as int?) ?? 0;

            var orderQuery = _context.Orders
                                    .Where(x => x.ProductionLineId.HasValue && x.ProductionLineId.Value == productionDay.ProductionLineId)
                                    .Where(x => x.Id == productionDay.OrdersId
                                        || (x.StartDate.HasValue && x.StartDate.Value.Date > productionDay.Date.Date))
                                    .AsQueryable();

            orderQuery = removeNextOrders == false
                ? orderQuery.Where(x => x.Id == productionDay.OrdersId)
                : orderQuery.Where(x => x.Id == productionDay.OrdersId || (x.StartDate.HasValue && x.StartDate.Value.Date > productionDay.Date.Date));

            var orders = await orderQuery.ToListAsync();

            foreach (var order in orders)
            {
                order.StartDate = null;
                order.StopDate = null;
                order.ProductionLineId = null;
                order.Sequence = ++maxSequence;
            }

            await _context.SaveChangesAsync();
        }

        private async Task ChangeProductionDayOrRemoveAsync(ProductionDaysBasicModel productionDay, bool dayOff)
        {
            if (productionDay.Id != 0)
            {
                // remove added free/work day
                var tmp = await _context.ProductionDays.Where(x => x.Id == productionDay.Id).FirstAsync();
                _context.ProductionDays.Remove(tmp);
            }
            else
            {
                // change day to work/free day
                _context.ProductionDays.Add(new ProductionDays()
                {
                    Date = productionDay.Date,
                    DayOff = dayOff,
                    ProductionLineId = productionDay.ProductionLineId,
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task SetDayFreeWitchMoveAsync(ProductionDaysBasicModel productionDay)
        {
            var orders = await _context.Orders
                                    .Where(x => x.ProductionLineId.HasValue && x.ProductionLineId.Value == productionDay.ProductionLineId)
                                    .Where(x => x.Id == productionDay.OrdersId
                                        || (x.StartDate.HasValue && x.StartDate.Value.Date > productionDay.Date.Date))
                                    .OrderBy(x => x.StartDate)
                                    .ToListAsync();

            var freeDays = await _context.ProductionDays
                .Where(x => x.ProductionLineId == productionDay.ProductionLineId)
                .Where(x => x.Date.Date > productionDay.Date.Date)
                .Where(x => x.DayOff)
                .AsNoTracking()
                .ToListAsync();

            int i = 1;
            foreach (var item in orders)
            {
                if (i != 1)
                {
                    item.StartDate = GetNextWorkDayDate(item.StartDate!.Value, freeDays);
                }

                item.StopDate = GetNextWorkDayDate(item.StopDate!.Value, freeDays);
                i++;
            }

            _context.ProductionDays.Add(new ProductionDays()
            {
                Date = productionDay.Date,
                DayOff = true,
                ProductionLineId = productionDay.ProductionLineId,
            });

            await _context.SaveChangesAsync();
        }

        private DateTime GetNextWorkDayDate(DateTime date, List<ProductionDays> freeDays)
        {
            while (true)
            {
                var tmp = freeDays.Where(x => x.Date.Date == date.AddDays(1).Date).FirstOrDefault();
                if (tmp != null
                    || date.AddDays(1).Date.DayOfWeek == DayOfWeek.Saturday
                    || date.AddDays(1).Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(1);
                }
                else
                {
                    return date.AddDays(1);
                }
            }
        }
    }
}