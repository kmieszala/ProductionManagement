namespace ProductionManagement.Common.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum LogCodeEnum
    {
        [Display(Name = "AddUser")]
        AddUser = 1,

        [Display(Name = "EditUser")]
        EditUser = 2,

        [Display(Name = "AddRoles")]
        AddRoles = 3,

        [Display(Name = "EditRoles")]
        EditRoles = 4,

        [Display(Name = "AddTank")]
        AddTank = 5,

        [Display(Name = "EditTank")]
        EditTank = 6,

        /// <summary>
        /// Podano zły PIN przy zmianie statusu zlecenia na wykonane
        /// </summary>
        [Display(Name = "MarkOrderAsDone_BadCode")]
        MarkOrderAsDone_BadCode = 7,

        [Display(Name = "MarkOrderAsDone_NoOrder")]
        MarkOrderAsDone_NoOrder = 8,

        [Display(Name = "MarkOrderAsDone_OrderCompleted")]
        MarkOrderAsDone_OrderCompleted = 9,

        [Display(Name = "MarkOrderAsDone_NextOrderInProgress")]
        MarkOrderAsDone_NextOrderInProgress = 10,
    }
}