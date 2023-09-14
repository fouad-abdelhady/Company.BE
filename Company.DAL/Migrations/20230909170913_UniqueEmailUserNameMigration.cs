using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UniqueEmailUserNameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auths_Staffs_StaffMemeberId",
                table: "Auths");

            migrationBuilder.RenameColumn(
                name: "StaffMemeberId",
                table: "Auths",
                newName: "StaffMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Auths_StaffMemeberId",
                table: "Auths",
                newName: "IX_Auths_StaffMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auths_Staffs_StaffMemberId",
                table: "Auths",
                column: "StaffMemberId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auths_Staffs_StaffMemberId",
                table: "Auths");

            migrationBuilder.RenameColumn(
                name: "StaffMemberId",
                table: "Auths",
                newName: "StaffMemeberId");

            migrationBuilder.RenameIndex(
                name: "IX_Auths_StaffMemberId",
                table: "Auths",
                newName: "IX_Auths_StaffMemeberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auths_Staffs_StaffMemeberId",
                table: "Auths",
                column: "StaffMemeberId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
