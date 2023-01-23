namespace ProductionManagement.Common.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum RolesEnum
    {
        [Display(Name = "Administrator")]
        Administrator = 1,

        [Display(Name = "Kalendarz")]
        Calendar = 2,

        [Display(Name = "KalendarzPodglad")]
        CalendarView = 3,

        [Display(Name = "Zlecenia")]
        Orders = 4,

        [Display(Name = "ZleceniaPodglad")]
        OrdersView = 5,

        [Display(Name = "LinieProd")]
        ProductionLines = 6,

        [Display(Name = "Ustawienia")]
        Settings = 7,

        [Display(Name = "UstawieniaPodglad")]
        SettingsView = 8,
    }
}