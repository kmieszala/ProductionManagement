namespace ProductionManagement.Services.Services.Orders.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string OrderName { get; set; }

        public string Description { get; set; }

        public int TankId { get; set; }

        public string TankName { get; set; }

        public int ProductionDays { get; set; }

        public string Color { get; set; }

        public string ProductionLinesNames { get; set; }
    }
}
