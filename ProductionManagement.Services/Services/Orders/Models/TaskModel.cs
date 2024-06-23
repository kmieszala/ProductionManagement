using ProductionManagement.Services.Services.Shared.Models;

namespace ProductionManagement.Services.Services.Orders.Models;

public class TaskModel
{
    public int ProductionLineId { get; set; }

    public List<DictModel> TasksList { get; set; }
}