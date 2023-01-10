using System;
using System.Collections.Generic;
using ProductionManagement.Common.Enums;
using ProductionManagement.WebUI.Areas.Shared.Models;

namespace ProductionManagement.WebUI.Areas.Users.ViewModels
{
    public class UsersVM
    {
        public int Id { get; set; }

        public DateTime RegisteredDate { get; set; }

        public DateTime? ActivationDate { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string? Password { get; set; }

        public UserStatusEnum Status { get; set; }

        public IEnumerable<DictVM> Roles { get; set; }
    }
}