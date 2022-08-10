﻿namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ProductionManagement.Common.Enums;

    public class Users
    {
        public Users()
        {
            UserRoles = new HashSet<UserRoles>();
            Tank = new HashSet<Tank>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }

        [ForeignKey("UserStatusDict")]
        public UserStatusEnum Status { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public virtual ICollection<Tank> Tank { get; set; }

        public virtual UserStatusDict UserStatusDict { get; set; }
    }
}