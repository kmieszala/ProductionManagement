namespace ProductionManagement.Services.Services.WorkSchedule.Models
{
    public class ProductionDaysBasicModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string ProductionLineName { get; set; }

        public int ProductionLineId { get; set; }

        public int? OrdersId { get; set; }

        public string? OrderName { get; set; }

        public bool DayOff { get; set; }

        public string? Color { get; set; }
    }
}