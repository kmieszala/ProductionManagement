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
    }
}
