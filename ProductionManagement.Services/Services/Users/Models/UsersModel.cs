using ProductionManagement.Common.Enums;
using ProductionManagement.Services.Services.Shared.Models;

namespace ProductionManagement.Services.Services.Users.Models
{
    public class UsersModel
    {
        public int Id { get; set; }

        public DateTime RegisteredDate { get; set; }

        public DateTime? ActivationDate { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string? Password { get; set; }

        public UserStatusEnum Status { get; set; }

        public IEnumerable<DictModel> Roles { get; set; }
    }
}