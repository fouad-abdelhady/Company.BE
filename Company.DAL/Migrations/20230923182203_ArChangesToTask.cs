using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ArChangesToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArChanges",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "StateChangedAt" },
                values: new object[] { new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9194), new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9199) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArChanges", "CreatedAt", "StateChangedAt" },
                values: new object[] { null, new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9095), new DateTime(2023, 9, 23, 21, 22, 3, 439, DateTimeKind.Local).AddTicks(9151) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArChanges",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "StateChangedAt" },
                values: new object[] { new DateTime(2023, 9, 23, 20, 47, 17, 776, DateTimeKind.Local).AddTicks(99), new DateTime(2023, 9, 23, 20, 47, 17, 776, DateTimeKind.Local).AddTicks(103) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "StateChangedAt" },
                values: new object[] { new DateTime(2023, 9, 23, 20, 47, 17, 776, DateTimeKind.Local).AddTicks(13), new DateTime(2023, 9, 23, 20, 47, 17, 776, DateTimeKind.Local).AddTicks(58) });
        }
    }
}
