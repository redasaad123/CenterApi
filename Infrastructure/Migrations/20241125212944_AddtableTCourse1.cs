using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddtableTCourse1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherCourses",
                columns: table => new
                {
                    TCoursesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    courseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCourses", x => x.TCoursesId);
                    table.ForeignKey(
                        name: "FK_TeacherCourses_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherCourses_UsersAccount_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "security",
                        principalTable: "UsersAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourses_courseId",
                table: "TeacherCourses",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourses_TeacherId",
                table: "TeacherCourses",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherCourses");
        }
    }
}
