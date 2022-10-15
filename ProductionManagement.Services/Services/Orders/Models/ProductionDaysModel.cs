namespace ProductionManagement.Services.Services.Orders.Models
{
    public class ProductionDaysModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool DayOff { get; set; }

        public int? OrdersId { get; set; }

        public string OrderName { get; set; }

        public string Color { get; set; }
    }
}