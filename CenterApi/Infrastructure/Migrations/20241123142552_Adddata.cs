using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

                migrationBuilder.InsertData(
                table: "TypeJop",
                columns: new[] { "Id", "JopType" },
                values: new object[] { Guid.NewGuid().ToString(), "student"});

            migrationBuilder.InsertData(
                table: "TypeJop",
                columns: new[] { "Id", "JopType" },
                values: new object[] { Guid.NewGuid().ToString(), "Teacher" });


            migrationBuilder.InsertData(
                table: "TypeJop",
                columns: new[] { "Id", "JopType" },
                values: new object[] { Guid.NewGuid().ToString(), "Admin" });


            migrationBuilder.InsertData(
                table: "TypeJop",
                columns: new[] { "Id", "JopType" },
                values: new object[] { Guid.NewGuid().ToString(), "Employee" });







        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
