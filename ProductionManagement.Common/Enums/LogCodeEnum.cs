namespace ProductionManagement.Common.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum LogCodeEnum
    {
        [Display(Name = "AddUser")]
        AddUser = 0,

        [Display(Name = "EditUser")]
        EditUser = 1,

        [Display(Name = "AddRoles")]
        AddRoles = 2,

        [Display(Name = "EditRoles")]
        EditRoles = 3,

        [Display(Name = "AddTank")]
        AddTank = 4,

        [Display(Name = "EditTank")]
        EditTank = 5,
    }
}
