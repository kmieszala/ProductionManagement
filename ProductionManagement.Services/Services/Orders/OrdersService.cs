using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Model;
using ProductionManagement.Services.Services.Orders.Models;

namespace ProductionManagement.Services.Services.Orders
{
    public interface IOrdersService
    {
        Task<List<OrderModel>> GetOrdersAsync();

        Task<int> AddOrderAsync(OrderModel model);

        Task<bool> EditOrderAsync(OrderModel model);

        Task<IXLWorkbook> PrepareStorekeeperDocumentAsync(List<int>? ordersIds, List<PartsStorekeeperModel>? parts);

        Task<bool> UpdateSequenceOrdersAsync(List<SequenceModel> sequenceList);

        Task<bool> GenerateCalendarAsync(List<OrderModel> orderModels);

        //Task<List<PlannedOrdersModel>> GetPlannedOrdersAsync(FilterPlannedOrdersModel filterPlannedOrdersModel);
    }

    public class OrdersService : IOrdersService
    {
        private readonly ProductionManagementContext _context;
        private readonly IMapper _mapper;

        public OrdersService(ProductionManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddOrderAsync(OrderModel model)
        {
            var maxSequence = _context.Orders
                         .Max(x => x.Sequence as int?) ?? 0;
            var dbModel = _mapper.Map<Model.DbSets.Orders>(model);
            dbModel.Sequence = ++maxSequence;
            await _context.Orders.AddAsync(dbModel);
            await _context.SaveChangesAsync();

            return dbModel.Id;
        }

        public async Task<bool> EditOrderAsync(OrderModel model)
        {
            var dbModel = await _context.Orders.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (dbModel == null)
            {
                return false;
            }

            dbModel.OrderName = model.OrderName;
            dbModel.Color = model.Color;
            dbModel.Description = model.Description;
            dbModel.TankId = model.TankId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<OrderModel>> GetOrdersAsync()
        {
            var result = await _context.Orders
                .OrderBy(x => x.Sequence)
                .Select(x => new OrderModel()
                {
                    Id = x.Id,
                    Color = x.Color,
                    Description = x.Description,
                    OrderName = x.OrderName,
                    ProductionDays = (int) x.Tank.ProductionDays,
                    TankId = x.TankId,
                    TankName = x.Tank.Name,
                    Sequence = x.Sequence,
                    StartDate = x.StartDate,
                    StopDate = x.StopDate,
                    ProductionLinesNames = String.Join(", ", x.Tank.LineTank.Select(y => y.ProductionLine.Name))
                }).ToListAsync();

            return result;
        }

        public async Task<bool> UpdateSequenceOrdersAsync(List<SequenceModel> sequenceList)
        {
            var ordersIds = sequenceList.Select(x => x.Id).ToList();

            var orders = await _context.Orders
                .Where(x => ordersIds.Contains(x.Id))
                .Where(x => !x.StartDate.HasValue)
                .ToListAsync();

            if (orders.Count != sequenceList.Count)
            {
                return false;
            }

            var maxSequence = await _context.Orders.Where(x => x.StartDate.HasValue).MaxAsync(x => x.Sequence as int?) ?? 0;
            foreach (var x in orders)
            {
                var tmp = sequenceList.First(y => y.Id == x.Id);
                x.Sequence = tmp.Sequence + maxSequence;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IXLWorkbook> PrepareStorekeeperDocumentAsync(List<int>? ordersIds, List<PartsStorekeeperModel>? parts)
        {
            var workBook = new XLWorkbook();

            var partsOfOrders = await GetPartsForOrdersAsync(ordersIds);
            var allParts = parts != null ? partsOfOrders.Concat(parts) : partsOfOrders;

            allParts = allParts.GroupBy(x => x.PartsId).Select(x => new PartsStorekeeperModel()
            {
                PartsId = x.Key,
                PartsName = x.First().PartsName,
                PartsNumber = x.Sum(y => y.PartsNumber)
            }).ToList();

            workBook = CreateSStorekeeperSheet(workBook, allParts).Result;

            return workBook;
        }

        //public async Task<List<PlannedOrdersModel>> GetPlannedOrdersAsync(FilterPlannedOrdersModel filterPlannedOrdersModel)
        //{
        //    var result = await _context.ProductionLine
        //        .Where(x => x.Active)
        //        .Select(x => new PlannedOrdersModel()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            StartDate = x.StartDate,
        //            ProductionDays = x.ProductionDays
        //                .Select(y => new ProductionDaysModel()
        //                {
        //                    Id = y.Id,
        //                    Date = y.Date,
        //                    DayOff = y.DayOff,
        //                    //OrdersId = y.OrdersId,
        //                    //OrderName = y.Orders.OrderName,
        //                    //Color = y.Orders.Color,
        //                })
        //                .OrderBy(y => y.Date)
        //                .ToList(),
        //        }).ToListAsync();

        //    result = result.OrderBy(x => x.Id).ToList();

        //    return result;
        //}

        public async Task<bool> GenerateCalendarAsync(List<OrderModel> orderModels)
        {
            orderModels = orderModels.OrderBy(x => x.Sequence).ToList();
            var ordersIds = orderModels.Select(x => x.Id);
            var maxSequence = await _context.Orders.Where(x => x.StartDate.HasValue).MaxAsync(x => x.Sequence as int?) ?? 0;
            maxSequence += 1;

            var ordersDb = await _context.Orders
                .Include(x => x.Tank)
                    .ThenInclude(x => x.LineTank)
                        .ThenInclude(x => x.ProductionLine)
                .Where(x => !x.StartDate.HasValue)
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            var productionLines = await _context.ProductionLine
                .Where(x => x.Active)
                .Select(x => new
                {
                    ProductionLineId = x.Id,
                    Date = x.StartDate,
                }).ToListAsync();

            var productionLinesIds = productionLines.Select(y => y.ProductionLineId);
            var maxDateNoFreeDayForProductionLine = await _context.Orders
                .Where(x => x.ProductionLineId.HasValue && productionLinesIds.Contains(x.ProductionLineId.Value))
                .GroupBy(x => x.ProductionLineId)
                .Select(x => new
                {
                    ProductionLineId = x.Key,
                    Date = x.Max(y => y.StopDate.Value),
                })
                .ToListAsync();

            //var minDate = maxDateNoFreeDayForProductionLine.Min(x => x.Date);

            var freeDaysForProductionLines = (from x in productionLinesIds
                                              join y in _context.ProductionDays
                                                 on x equals y.ProductionLineId
                                              select
                                                 new { y.ProductionLineId, y.Date, y.DayOff }).ToList();

            var maxDateDict = new Dictionary<int, DateTime>();
            if (maxDateNoFreeDayForProductionLine.Count == 0)
            {
                maxDateDict = productionLines.ToDictionary(x => x.ProductionLineId, x => x.Date);
            }
            else
            {
                foreach (var productionLine in productionLines)
                {
                    var tmp = maxDateNoFreeDayForProductionLine.FirstOrDefault(x => x.ProductionLineId == productionLine.ProductionLineId);
                    if (tmp != null)
                    {
                        maxDateDict.Add(tmp.ProductionLineId.Value, tmp.Date.Date.AddDays(1));
                    }
                    else
                    {
                        maxDateDict.Add(productionLine.ProductionLineId, productionLine.Date);
                    }
                }
            }

            foreach (var order in ordersDb)
            {
                if (!ordersIds.Any(x => x == order.Id))
                {
                    continue;
                }

                var linesIds = new List<int>();
                if (order.Tank.LineTank.Count > 0)
                {
                    linesIds = order.Tank.LineTank.Select(x => x.ProductionLineId).ToList();
                    linesIds = linesIds.Where(x => productionLines.Select(y => y.ProductionLineId).Contains(x)).ToList();
                }
                else
                {
                    linesIds = productionLines.Select(y => y.ProductionLineId).ToList();
                }

                var maxDateNoFreeDayForProductionLineTMP = maxDateDict.Where(x => linesIds.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

                var productionLineId = 0;
                while (productionLineId == 0)
                {
                    var workDate = maxDateDict.Where(x => linesIds.Contains(x.Key)).OrderBy(x => x.Value).First();
                    var addedFreeDay = freeDaysForProductionLines.Where(x =>
                        x.ProductionLineId == workDate.Key
                        && !x.DayOff
                        && x.Date.Date == maxDateDict[workDate.Key].Date)
                        .FirstOrDefault();
                    if (addedFreeDay == null && (workDate.Value.DayOfWeek == DayOfWeek.Saturday || workDate.Value.DayOfWeek == DayOfWeek.Sunday))
                    {
                        maxDateDict[workDate.Key] = workDate.Value.AddDays(1);
                    }
                    else if (freeDaysForProductionLines.Count > 0
                      && freeDaysForProductionLines.FirstOrDefault(x => x.ProductionLineId == workDate.Key && x.Date.Date == workDate.Value.Date && x.DayOff) != null)
                    {
                        maxDateDict[workDate.Key] = workDate.Value.AddDays(1);
                    }
                    else
                    {
                        productionLineId = workDate.Key;
                    }
                }

                for (int i = 0; i < order.Tank.ProductionDays; i++)
                {
                    var addedFreeDay = freeDaysForProductionLines.Where(x =>
                        x.ProductionLineId == productionLineId
                        && !x.DayOff
                        && x.Date.Date == maxDateDict[productionLineId].Date)
                        .FirstOrDefault();
                    if (addedFreeDay == null && (maxDateDict[productionLineId].Date.DayOfWeek == DayOfWeek.Saturday || maxDateDict[productionLineId].Date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        //_context.ProductionDays.Add(new ProductionDays()
                        //{
                        //    Date = maxDateDict[productionLineId].Date,
                        //    ProductionLineId = productionLineId,
                        //    DayOff = true,
                        //});
                        maxDateDict[productionLineId] = maxDateDict[productionLineId].Date.AddDays(1);
                        i--;
                    }
                    else if (freeDaysForProductionLines.Count > 0
                      && freeDaysForProductionLines.FirstOrDefault(x => x.ProductionLineId == productionLineId && x.Date.Date == maxDateDict[productionLineId].Date && x.DayOff) != null)
                    {
                        //_context.ProductionDays.Add(new ProductionDays()
                        //{
                        //    Date = maxDateDict[productionLineId].Date,
                        //    ProductionLineId = productionLineId,
                        //    DayOff = true,
                        //});
                        maxDateDict[productionLineId] = maxDateDict[productionLineId].Date.AddDays(1);
                        i--;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            order.StartDate = maxDateDict[productionLineId].Date;
                            order.StopDate = maxDateDict[productionLineId].Date;
                            order.Sequence = maxSequence++;
                            order.ProductionLineId = productionLineId;
                        }
                        else
                        {
                            order.StopDate = maxDateDict[productionLineId].Date;
                        }

                        //_context.ProductionDays.Add(new ProductionDays()
                        //{
                        //    Date = maxDateDict[productionLineId].Date,
                        //    //OrdersId = order.Id,
                        //    ProductionLineId = productionLineId,
                        //});

                        maxDateDict[productionLineId] = maxDateDict[productionLineId].Date.AddDays(1);
                    }
                }
            }

            foreach (var order in ordersDb)
            {
                if (!order.StartDate.HasValue)
                {
                    order.Sequence = maxSequence++;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<List<PartsStorekeeperModel>> GetPartsForOrdersAsync(List<int>? ordersIds)
        {
            var result = await _context.Orders
                .Where(x => ordersIds.Contains(x.Id))
                .SelectMany(x => x.Tank.TankParts)
                .Select(x => new PartsStorekeeperModel()
                {
                    PartsId = x.PartsId,
                    PartsName = x.Parts.Name,
                    PartsNumber = x.PartsNumber,
                })
                .ToListAsync();

            return result;
        }

        private async Task<XLWorkbook> CreateSStorekeeperSheet(XLWorkbook workBook, IEnumerable<PartsStorekeeperModel> reportModel)
        {
            string worksheetName = $"Lista części";

            var result = workBook;
            var workSheet = result.Worksheets.Add(worksheetName);

            workSheet.Cell(1, 1).Value = $"Lista części";
            workSheet.Cell(2, 1).Value = $"Data generowania:";
            workSheet.Cell(2, 2).Value = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}";

            workSheet.Range(1, 1, 1, 3).Merge().AddToNamed("Title");

            DataTable headers = new DataTable();
            DataColumn ordinalNumberHeader = headers.Columns.Add("Liczba pojedyncza", typeof(string));
            DataColumn dateHeader = headers.Columns.Add("Nazwa części", typeof(string));
            DataColumn accountNumberHeader = headers.Columns.Add("Ilość", typeof(string));

            headers.Rows.Add("Lp.", "Nazwa części", "Ilość");
            workSheet.Cell(4, 1).Value = headers.AsEnumerable();

            DataTable values = new DataTable();
            DataColumn ordinalNumberColumn = values.Columns.Add("Lp.", typeof(int));
            DataColumn dateColumn = values.Columns.Add("Nazwa części", typeof(string));
            DataColumn accountNumberColumn = values.Columns.Add("Ilość:", typeof(int));

            foreach (var deposit in reportModel.Select((value, index) => new { index, value }))
            {
                values.Rows.Add(
                    deposit.index + 1,
                    deposit.value.PartsName,
                    deposit.value.PartsNumber);
            }

            workSheet.Cell(5, 1).Value = values.AsEnumerable();

            DataTable summary = new DataTable();
            DataColumn paymentsCountHeader = summary.Columns.Add("Łączna ilośc", typeof(string));
            DataColumn paymentsCountValue = summary.Columns.Add("Łączna ilość", typeof(int));

            int lastRow = values.Rows.Count;
            var partsSum = reportModel.Select(x => x.PartsNumber).ToList().Sum();
            summary.Rows.Add("Łączna ilość: ", partsSum);
            workSheet.Cell(lastRow + 5, 2).Value = summary.AsEnumerable();

            //stylowanie komórek
            workSheet.Cell(1, 1).Style.Font.Bold = true;
            workSheet.Cell(1, 1).Style.Font.FontSize = 18;
            workSheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            workSheet.Cell(2, 1).Style.Font.Bold = true;
            workSheet.Cell(2, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            workSheet.Cell(2, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            workSheet.Cell(2, 2).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
            workSheet.Range(4, 1, 4, 7).Style.Font.Bold = true;
            workSheet.Range(lastRow + 5, 3, lastRow + 5, 6).Style.Font.Bold = true;

            //formatowanie szerokości kolumn do zawartości
            workSheet.Columns(1, 7).AdjustToContents();

            return result;
        }
    }
}