using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeJop_UsersAccount_TypeJopId",
                table: "TypeJop");

            migrationBuilder.DropIndex(
                name: "IX_TypeJop_TypeJopId",
                table: "TypeJop");

            migrationBuilder.DropColumn(
                name: "TypeJopId",
                table: "TypeJop");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "TypeJopId",
                table: "TypeJop",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TypeJop_TypeJopId",
                table: "TypeJop",
                column: "TypeJopId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeJop_UsersAccount_TypeJopId",
                table: "TypeJop",
                column: "TypeJopId",
                principalSchema: "security",
                principalTable: "UsersAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
