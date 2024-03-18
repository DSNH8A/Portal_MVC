using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Migrations
{
    /// <inheritdoc />
    public partial class newID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Users_UserId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Skills",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_UserId",
                table: "Skills",
                newName: "IX_Skills_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Users_UserID",
                table: "Skills",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Users_UserID",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Skills",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_UserID",
                table: "Skills",
                newName: "IX_Skills_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Users_UserId",
                table: "Skills",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
