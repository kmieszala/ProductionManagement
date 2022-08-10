﻿namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ProductionManagement.Common.Enums;

    /// <summary>
    /// This table is for database triggers.
    /// </summary>
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("LogCodeDict")]
        public LogCodeEnum LogCode { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public virtual LogCodeDict LogCodeDict { get; set; }
    }
}
