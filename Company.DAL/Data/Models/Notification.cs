using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "value";
        [Required]
        public string ArTitle { get; set; } = "value";

        [Required]
        public string Description { get; set; } = "value";
        [Required]
        public string ArDescription { get; set; } = "value";
        [Required]
        public int PosterId { get; set; }
        [Required]
        public int RecieverId { get; set; }
        [Required]
        public int TaskId { get; set; }
        [Required]
        public int Type { get; set; } // 0 task creation, 1 for update, 3 for other
        [Required]
        public int Status { get; set; } = 0; // default recieved 1 for seen
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime StateChangedAt { get; set; } = DateTime.Now;

        // Navigation properties for the foreign keys
        public Staff? Poster { get; set; } = null;
        public Staff? Reciever { get; set; } = null;
        public Task? Task { get; set; } = null;
    }
}
/*
 using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffs_Staffs_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.UniqueConstraint("Email_Unique", staff => staff.EmailAddress);
                });

            migrationBuilder.CreateTable(
                name: "Auths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auths_Staffs_StaffMemberId",
                        column: x => x.StaffMemberId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.UniqueConstraint("UserName_Uniqe", auth => auth.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true),
                    SeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StateChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Staffs_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Staffs_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosterId = table.Column<int>(type: "int", nullable: false),
                    RecieverId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StateChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Staffs_PosterId",
                        column: x => x.PosterId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Staffs_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "EmailAddress", "FullName", "Image", "ManagerId", "Role", "Salary" },
                values: new object[,]
                {
                    { 1, "fouad.abdelhady@egabi.com", "Fouad Abdelhady Fouad", null, null, "Manager", 40000.5m },
                    { 3, "admin@admin.com", "Admin", null, null, "Admin", 20000m }
                });

            migrationBuilder.InsertData(
                table: "Auths",
                columns: new[] { "Id", "Password", "StaffMemberId", "UserName" },
                values: new object[,]
                {
                    { 1, "25f9e794323b453885f5181f1b624d0b", 1, "fouad.abdelhady" },
                    { 3, "e3afed0047b08059d0fada10f400c1e5", 3, "admin.admin" }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "EmailAddress", "FullName", "Image", "ManagerId", "Role", "Salary" },
                values: new object[] { 2, "ahmed.abdelhady@egabi.com", "Ahmed Abdelhady Fouad", null, 1, "Employee", 4000.5m });

            migrationBuilder.InsertData(
                table: "Auths",
                columns: new[] { "Id", "Password", "StaffMemberId", "UserName" },
                values: new object[] { 2, "25f9e794323b453885f5181f1b624d0b", 2, "ahmed.abdelhady" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "ArDescription", "ArTitle", "CreatedAt", "CreatorId", "Description", "EmployeeId", "Grade", "SeenAt", "StateChangedAt", "Status", "Title" },
                values: new object[] { 1, "مهمة الاسبوع الأول", "أول مهمة", new DateTime(2023, 9, 20, 11, 15, 54, 329, DateTimeKind.Local).AddTicks(7525), 1, "First week task", 2, null, null, new DateTime(2023, 9, 20, 11, 15, 54, 329, DateTimeKind.Local).AddTicks(7572), 0, "First Task" });

            migrationBuilder.InsertData(
               table: "Notifications",
               columns: new[] { "Id", "ArDescription", "ArTitle", "CreatedAt", "Description", "PosterId", "RecieverId", "TaskId", "StateChangedAt", "Status", "Title", "Type" },
               values: new object[] { 1, "تمت اضافتها بواسطة فؤاد", "مهمه جديدة اضيفت", new DateTime(2023, 9, 20, 11, 15, 54, 329, DateTimeKind.Local).AddTicks(7611), "added by Fouad", 1, 2, 1, new DateTime(2023, 9, 20, 11, 15, 54, 329, DateTimeKind.Local).AddTicks(7616), 0, "first task added", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Auths_StaffMemberId",
                table: "Auths",
                column: "StaffMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PosterId",
                table: "Notifications",
                column: "PosterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecieverId",
                table: "Notifications",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TaskId",
                table: "Notifications",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ManagerId",
                table: "Staffs",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatorId",
                table: "Tasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EmployeeId",
                table: "Tasks",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auths");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}

 */