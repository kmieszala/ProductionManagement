namespace ProductionManagement.Common.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum RolesEnum
    {
        [Display(Name = "Administrator")]
        Administrator = 1,

        [Display(Name = "Editor")]
        Editor = 2,

        [Display(Name = "Reader")]
        Reader = 3,
    }
}
