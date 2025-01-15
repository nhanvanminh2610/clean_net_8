using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2025_01_15_1245 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId1",
                table: "AspNetUserLogins");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "AspNetUserLogins",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId1",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_RoleId",
                table: "AspNetUserLogins",
                column: "RoleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_RoleId",
                table: "AspNetUserLogins");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AspNetUserLogins",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_RoleId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId1",
                table: "AspNetUserLogins",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
