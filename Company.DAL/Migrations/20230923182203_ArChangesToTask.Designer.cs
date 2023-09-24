﻿// <auto-generated />
using System;
using Company.DAL.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Company.DAL.Migrations
{
    [DbContext(typeof(CompanyContext))]
    [Migration("20230923182203_ArChangesToTask")]
    partial class ArChangesToTask
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Company.DAL.Data.Models.Auth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StaffMemberId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("StaffMemberId");

                    b.ToTable("Auths");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "25f9e794323b453885f5181f1b624d0b",
                            StaffMemberId = 1,
                            UserName = "fouad.abdelhady"
                        },
                        new
                        {
                            Id = 2,
                            Password = "25f9e794323b453885f5181f1b624d0b",
                            StaffMemberId = 2,
                            UserName = "ahmed.abdelhady"
                        },
                        new
                        {
                            Id = 3,
                            Password = "e3afed0047b08059d0fada10f400c1e5",
                            StaffMemberId = 3,
                            UserName = "admin.admin"
                        });
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PosterId")
                        .HasColumnType("int");

                    b.Property<int>("RecieverId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StateChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PosterId");

                    b.HasIndex("RecieverId");

                    b.HasIndex("TaskId");

                    b.ToTable("Notifications");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArDescription = "تمت اضافتها بواسطة فؤاد",
                            ArTitle = "مهمه جديدة اضيفت",
                            CreatedAt = new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9194),
                            Description = "added by Fouad",
                            PosterId = 1,
                            RecieverId = 2,
                            StateChangedAt = new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9199),
                            Status = 0,
                            TaskId = 1,
                            Title = "first task added",
                            Type = 0
                        });
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(15,2)");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Staffs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EmailAddress = "fouad.abdelhady@egabi.com",
                            FullName = "Fouad Abdelhady Fouad",
                            Role = "Manager",
                            Salary = 40000.5m
                        },
                        new
                        {
                            Id = 2,
                            EmailAddress = "ahmed.abdelhady@egabi.com",
                            FullName = "Ahmed Abdelhady Fouad",
                            ManagerId = 1,
                            Role = "Employee",
                            Salary = 4000.5m
                        },
                        new
                        {
                            Id = 3,
                            EmailAddress = "admin@admin.com",
                            FullName = "Admin",
                            Role = "Admin",
                            Salary = 20000m
                        });
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArChanges")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Changes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int?>("Grade")
                        .HasColumnType("int");

                    b.Property<DateTime>("StateChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArDescription = "مهمة الاسبوع الأول",
                            ArTitle = "أول مهمة",
                            CreatedAt = new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9095),
                            CreatorId = 1,
                            Description = "First week task",
                            EmployeeId = 2,
                            StateChangedAt = new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9151),
                            Status = 0,
                            Title = "First Task"
                        });
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Auth", b =>
                {
                    b.HasOne("Company.DAL.Data.Models.Staff", "StaffMember")
                        .WithMany()
                        .HasForeignKey("StaffMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StaffMember");
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Notification", b =>
                {
                    b.HasOne("Company.DAL.Data.Models.Staff", "Poster")
                        .WithMany()
                        .HasForeignKey("PosterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Company.DAL.Data.Models.Staff", "Reciever")
                        .WithMany()
                        .HasForeignKey("RecieverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Company.DAL.Data.Models.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Poster");

                    b.Navigation("Reciever");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Staff", b =>
                {
                    b.HasOne("Company.DAL.Data.Models.Staff", "Manager")
                        .WithMany("TeamMembers")
                        .HasForeignKey("ManagerId");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Task", b =>
                {
                    b.HasOne("Company.DAL.Data.Models.Staff", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Company.DAL.Data.Models.Staff", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Company.DAL.Data.Models.Staff", b =>
                {
                    b.Navigation("TeamMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
