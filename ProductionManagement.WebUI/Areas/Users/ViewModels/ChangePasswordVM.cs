namespace ProductionManagement.WebUI.Areas.Users.ViewModels
{
    public class ChangePasswordVM
    {
        public int UserId { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }
    }
}