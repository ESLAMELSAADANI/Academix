using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class seedDepartmentsTableWithData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DeptId", "Capacity", "DeptName" },
                values: new object[,]
                {
                    { 100, 50, "CS" },
                    { 200, 25, "Cyber" },
                    { 300, 30, "Java" },
                    { 400, 45, "Cross" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 400);
        }
    }
}
