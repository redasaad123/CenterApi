using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "TypeJop");

            migrationBuilder.DropColumn(
                name: "Employee",
                table: "TypeJop");

            migrationBuilder.DropColumn(
                name: "student",
                table: "TypeJop");

            migrationBuilder.RenameColumn(
                name: "teacher",
                table: "TypeJop",
                newName: "JopType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JopType",
                table: "TypeJop",
                newName: "teacher");

            migrationBuilder.AddColumn<string>(
                name: "Admin",
                table: "TypeJop",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Employee",
                table: "TypeJop",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "student",
                table: "TypeJop",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
