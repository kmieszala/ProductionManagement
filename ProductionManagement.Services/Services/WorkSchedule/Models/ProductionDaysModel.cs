namespace ProductionManagement.Services.Services.WorkSchedule.Models
{
    public class ProductionDaysModel
    {
        public DateTime CalendarDay { get; set; }

        public List<ProductionDaysBasicModel> ProductionDay { get; set; }
    }
}