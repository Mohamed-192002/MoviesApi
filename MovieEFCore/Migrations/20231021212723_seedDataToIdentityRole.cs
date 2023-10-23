using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoviesApi.Migrations
{
    /// <inheritdoc />
    public partial class seedDataToIdentityRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1277236b-0803-46d6-af80-677a725a353c", "932975b5-2cdc-44dc-ad7c-4b538908cd11", "User", "USER" },
                    { "8bcda48b-c43b-4307-93b7-d49655c5bc09", "5783f0c1-3848-44fb-85c3-71398a3a04b2", "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1277236b-0803-46d6-af80-677a725a353c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8bcda48b-c43b-4307-93b7-d49655c5bc09");
        }
    }
}
