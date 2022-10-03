using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Services.Services.Orders.Models;
using ProductionManagement.Model;
using ClosedXML.Excel;
using System.Data;

namespace ProductionManagement.Services.Services.Orders
{
    public interface IOrdersService
    {
        Task<List<OrderModel>> GetOrdersAsync();

        Task<int> AddOrderAsync(OrderModel model);

        Task<bool> EditOrderAsync(OrderModel model);

        Task<IXLWorkbook> PrepareStorekeeperDocumentAsync(List<int>? ordersIds, List<PartsStorekeeperModel>? parts);
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
            var dbModel = _mapper.Map<Model.DbSets.Orders>(model);
            await _context.Orders.AddAsync(dbModel);
            await _context.SaveChangesAsync();

            return dbModel.Id;
        }

        public async Task<bool> EditOrderAsync(OrderModel model)
        {
            var dbModel = await _context.Orders.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if(dbModel == null)
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
                .Select(x => new OrderModel()
                {
                    Id = x.Id,
                    Color = x.Color,
                    Description = x.Description,
                    OrderName = x.OrderName,
                    ProductionDays = (int) x.Tank.ProductionDays,
                    TankId = x.TankId,
                    TankName = x.Tank.Name,
                    ProductionLinesNames = String.Join(", ", x.Tank.LineTank.Select(y => y.ProductionLine.Name))
                }).ToListAsync();

            return result;
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

        public async Task<XLWorkbook> CreateSStorekeeperSheet(XLWorkbook workBook, IEnumerable<PartsStorekeeperModel> reportModel)
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
