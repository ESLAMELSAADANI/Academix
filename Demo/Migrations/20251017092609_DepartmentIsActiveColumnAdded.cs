using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentIsActiveColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 100,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 200,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 300,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DeptId",
                keyValue: 400,
                column: "IsActive",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Department");
        }
    }
}
