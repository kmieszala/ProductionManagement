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
            Tank = new HashSet<Tanks>();
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

        public int TimeBlockCount { get; set; }

        [MaxLength(64)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Secret code to change order status
        /// </summary>
        [MaxLength(4)]
        [Required]
        public string? Code { get; set; }

        /// <summary>
        /// User registration by the administrator.
        /// </summary>
        public DateTime RegisteredDate { get; set; }

        /// <summary>
        /// First login and change password by user.
        /// </summary>
        public DateTime? ActivationDate { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public virtual ICollection<Tanks> Tank { get; set; }

        public virtual UserStatusDict UserStatusDict { get; set; }
    }
}