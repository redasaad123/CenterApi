using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatatabletask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Materails_materailId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "materailId",
                table: "Tasks",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_materailId",
                table: "Tasks",
                newName: "IX_Tasks_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Courses_CourseId",
                table: "Tasks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Courses_CourseId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Tasks",
                newName: "materailId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_CourseId",
                table: "Tasks",
                newName: "IX_Tasks_materailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Materails_materailId",
                table: "Tasks",
                column: "materailId",
                principalTable: "Materails",
                principalColumn: "materailId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
