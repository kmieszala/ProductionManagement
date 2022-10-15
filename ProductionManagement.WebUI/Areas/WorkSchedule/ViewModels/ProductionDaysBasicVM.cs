using System;

namespace ProductionManagement.WebUI.Areas.WorkSchedule.ViewModels
{
    public class ProductionDaysBasicVM
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