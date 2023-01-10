using System.Collections.Generic;
using ProductionManagement.Common.Enums;

namespace ProductionManagement.WebUI.Areas.Authorization.ViewModels
{
    public class LoginOutVM
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public int TimeBlockCount { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public UserStatusEnum Status { get; set; }
    }
}