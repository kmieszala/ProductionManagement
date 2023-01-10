namespace ProductionManagement.Services.Services.Users.Models
{
    public class ChangePasswordModel
    {
        public int UserId { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }
    }
}