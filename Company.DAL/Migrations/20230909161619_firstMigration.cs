using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.DAL.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
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
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "Auths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    StaffMemeberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auths_Staffs_StaffMemeberId",
                        column: x => x.StaffMemeberId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "EmailAddress", "FullName", "Image", "ManagerId", "Role", "Salary" },
                values: new object[] { 1, "fouad.abdelhady@egabi.com", "Fouad Abdelhady Fouad", null, null, "Manager", 40000.5m });

            migrationBuilder.InsertData(
                table: "Auths",
                columns: new[] { "Id", "Password", "StaffMemeberId", "UserName" },
                values: new object[] { 1, "123456789", 1, "fouad.abdelhady" });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "EmailAddress", "FullName", "Image", "ManagerId", "Role", "Salary" },
                values: new object[] { 2, "ahmed.abdelhady@egabi.com", "Ahmed Abdelhady Fouad", null, 1, "Employee", 4000.5m });
            migrationBuilder.InsertData(
                table: "Auths",
                columns: new[] { "Id", "Password", "StaffMemeberId", "UserName" },
                values: new object[] { 2, "123456789", 2, "ahmed.abdelhady" });
            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "EmailAddress", "FullName", "Role", "Salary" },
                values: new object[] { 3, "admin@admin.com", "Admin", "Admin", 20000.5m });
            migrationBuilder.InsertData(
                table: "Auths",
                columns: new[] { "Id", "Password", "StaffMemeberId", "UserName" },
                values: new object[] { 3, "Admin", 3, "admin.admin" });
            migrationBuilder.CreateIndex(
                name: "IX_Auths_StaffMemeberId",
                table: "Auths",
                column: "StaffMemeberId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ManagerId",
                table: "Staffs",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auths");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
