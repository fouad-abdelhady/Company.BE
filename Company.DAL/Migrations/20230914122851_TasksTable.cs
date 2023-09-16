using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TasksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Grade = table.Column<int>(type: "int", nullable: true ,defaultValue: null),
                    SeenAt = table.Column<DateTime>(type: "datetime", nullable: true,defaultValue: null),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", defaultValue: DateTime.Now),
                    StateChangedAt = table.Column<DateTime>(type: "datetime2", defaultValue: DateTime.Now)
                }
,
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

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: null);

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: null);

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "EmailAddress", "FullName", "Image", "ManagerId", "Role", "Salary" },
                values: new object[] { 3, "admin@admin.com", "Admin", null, null, "Admin", 20000m });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "CreatorId", "Description", "EmployeeId", "Grade", "SeenAt", "StateChangedAt", "Status", "Title" },
                values: new object[] { 1, new DateTime(2023, 9, 14, 15, 28, 51, 119, DateTimeKind.Local).AddTicks(840), 1, "First week task", 2, null, null, new DateTime(2023, 9, 14, 15, 28, 51, 119, DateTimeKind.Local).AddTicks(885), 0, "First Task" });

            migrationBuilder.InsertData(
                table: "Auths",
                columns: new[] { "Id", "Password", "StaffMemberId", "UserName" },
                values: new object[] { 3, "Admin", 3, "admin.admin" });

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
                name: "Tasks");

            migrationBuilder.DeleteData(
                table: "Auths",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Staffs",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: new byte[0]);

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: new byte[0]);
        }
    }
}
