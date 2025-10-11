using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetableAttend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendences_Materails_MaterailId",
                table: "attendences");

            migrationBuilder.RenameColumn(
                name: "MaterailId",
                table: "attendences",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_attendences_MaterailId",
                table: "attendences",
                newName: "IX_attendences_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_attendences_Courses_CourseId",
                table: "attendences",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendences_Courses_CourseId",
                table: "attendences");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "attendences",
                newName: "MaterailId");

            migrationBuilder.RenameIndex(
                name: "IX_attendences_CourseId",
                table: "attendences",
                newName: "IX_attendences_MaterailId");

            migrationBuilder.AddForeignKey(
                name: "FK_attendences_Materails_MaterailId",
                table: "attendences",
                column: "MaterailId",
                principalTable: "Materails",
                principalColumn: "materailId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
