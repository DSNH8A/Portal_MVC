using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Migrations
{
    /// <inheritdoc />
    public partial class Free : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_Skills_MaterialId",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "FK_courses_Skills_SkillId",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_MaterialId",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_SkillId",
                table: "courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_courses_MaterialId",
                table: "courses",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_courses_SkillId",
                table: "courses",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_Skills_MaterialId",
                table: "courses",
                column: "MaterialId",
                principalTable: "Skills",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_Skills_SkillId",
                table: "courses",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id");
        }
    }
}
