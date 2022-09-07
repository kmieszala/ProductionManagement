﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductionManagement.Model;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    [DbContext(typeof(ProductionManagementContext))]
    partial class ProductionManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("LogCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LogCode");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.LogCodeDict", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("LogCodeDict");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            Name = "AddUser"
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            Name = "EditUser"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            Name = "AddRoles"
                        },
                        new
                        {
                            Id = 4,
                            Active = true,
                            Name = "EditRoles"
                        },
                        new
                        {
                            Id = 5,
                            Active = true,
                            Name = "AddTank"
                        },
                        new
                        {
                            Id = 6,
                            Active = true,
                            Name = "EditTank"
                        });
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Parts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            Name = "Editor"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            Name = "Reader"
                        });
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Tank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ModificationUserId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("ProductionDays")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ModificationUserId");

                    b.ToTable("Tank");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.UserRoles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleId = 1,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ActivationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("RegisteredDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("Email"), new[] { "Password" });

                    b.HasIndex("Status");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActivationDate = new DateTime(2022, 9, 7, 21, 28, 18, 924, DateTimeKind.Local).AddTicks(489),
                            Email = "",
                            FirstName = "Admin",
                            LastName = "Admin",
                            Password = "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9",
                            RegisteredDate = new DateTime(2022, 9, 7, 21, 28, 18, 924, DateTimeKind.Local).AddTicks(527),
                            Status = 2
                        });
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.UserStatusDict", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("UserStatusDict");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            Name = "New"
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            Name = "Active"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            Name = "TimeBlocked"
                        },
                        new
                        {
                            Id = 4,
                            Active = true,
                            Name = "Deleted"
                        });
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Log", b =>
                {
                    b.HasOne("ProductionManagement.Model.DbSets.LogCodeDict", "LogCodeDict")
                        .WithMany("Log")
                        .HasForeignKey("LogCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogCodeDict");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Tank", b =>
                {
                    b.HasOne("ProductionManagement.Model.DbSets.Users", "User")
                        .WithMany("Tank")
                        .HasForeignKey("ModificationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.UserRoles", b =>
                {
                    b.HasOne("ProductionManagement.Model.DbSets.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductionManagement.Model.DbSets.Users", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Users", b =>
                {
                    b.HasOne("ProductionManagement.Model.DbSets.UserStatusDict", "UserStatusDict")
                        .WithMany("User")
                        .HasForeignKey("Status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserStatusDict");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.LogCodeDict", b =>
                {
                    b.Navigation("Log");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.Users", b =>
                {
                    b.Navigation("Tank");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ProductionManagement.Model.DbSets.UserStatusDict", b =>
                {
                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
