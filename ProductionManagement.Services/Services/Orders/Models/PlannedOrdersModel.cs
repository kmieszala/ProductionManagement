namespace ProductionManagement.Services.Services.Orders.Models
{
    public class PlannedOrdersModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ProductionDaysModel> ProductionDays { get; set; }

        public DateTime StartDate { get; set; }
    }
}
