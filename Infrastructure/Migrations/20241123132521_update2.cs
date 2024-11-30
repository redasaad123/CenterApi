using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeJop_UsersAccount_TypeId",
                table: "TypeJop");

            migrationBuilder.DropIndex(
                name: "IX_TypeJop_TypeId",
                table: "TypeJop");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "TypeJop");

            migrationBuilder.AlterColumn<string>(
                name: "TypeJopId",
                schema: "security",
                table: "UsersAccount",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAccount_TypeJopId",
                schema: "security",
                table: "UsersAccount",
                column: "TypeJopId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAccount_TypeJop_TypeJopId",
                schema: "security",
                table: "UsersAccount",
                column: "TypeJopId",
                principalTable: "TypeJop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAccount_TypeJop_TypeJopId",
                schema: "security",
                table: "UsersAccount");

            migrationBuilder.DropIndex(
                name: "IX_UsersAccount_TypeJopId",
                schema: "security",
                table: "UsersAccount");

            migrationBuilder.AlterColumn<string>(
                name: "TypeJopId",
                schema: "security",
                table: "UsersAccount",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                table: "TypeJop",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeJop_TypeId",
                table: "TypeJop",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeJop_UsersAccount_TypeId",
                table: "TypeJop",
                column: "TypeId",
                principalSchema: "security",
                principalTable: "UsersAccount",
                principalColumn: "Id");
        }
    }
}
