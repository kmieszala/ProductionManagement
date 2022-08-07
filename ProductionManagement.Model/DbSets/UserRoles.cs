namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserRoles
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
