using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.Common.Enums;

public enum OrderStatusEnum
{
    /// <summary>
    /// Zlecenie dodane
    /// </summary>
    [Display(Name = "Dodane")]
    Add = 1,

    /// <summary>
    /// Zaplanowane w kalendarzu
    /// </summary>
    [Display(Name = "Zaplanowane")]
    Planned = 2,

    [Display(Name = "W realizacji")]
    InProgress = 3,

    [Display(Name = "Zakończone")]
    Completed = 4,
}