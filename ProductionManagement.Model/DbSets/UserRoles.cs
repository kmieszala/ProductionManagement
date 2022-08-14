namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations.Schema;
    using ProductionManagement.Common.Enums;

    public class UserRoles
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Role")]
        public RolesEnum RoleId { get; set; }

        public virtual Users User { get; set; }

        public virtual Role Role { get; set; }
    }
}
