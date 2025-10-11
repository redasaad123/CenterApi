using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_UsersAccount_courseId",
                table: "Enrollment");

            migrationBuilder.AlterColumn<string>(
                name: "studentId",
                table: "Enrollment",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_studentId",
                table: "Enrollment",
                column: "studentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_UsersAccount_studentId",
                table: "Enrollment",
                column: "studentId",
                principalSchema: "security",
                principalTable: "UsersAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_UsersAccount_studentId",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_studentId",
                table: "Enrollment");

            migrationBuilder.AlterColumn<string>(
                name: "studentId",
                table: "Enrollment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_UsersAccount_courseId",
                table: "Enrollment",
                column: "courseId",
                principalSchema: "security",
                principalTable: "UsersAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
