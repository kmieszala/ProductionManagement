using ProductionManagement.Common.Enums;

namespace ProductionManagement.Services.Services.Users.Models
{
    public class LoginUserModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int TimeBlockCount { get; set; }

        public UserStatusEnum Status { get; set; }

        public List<RolesEnum> UserRoles { get; set; }
    }
}