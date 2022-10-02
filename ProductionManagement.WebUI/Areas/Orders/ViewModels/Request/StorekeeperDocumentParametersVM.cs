using System.Collections.Generic;

namespace ProductionManagement.WebUI.Areas.Orders.ViewModels.Request
{
    public class StorekeeperDocumentParametersVM
    {
        public List<int>? OrdersIds { get; set; }

        public List<PartsStorekeeperVM>? Parts { get; set; }
    }
}
