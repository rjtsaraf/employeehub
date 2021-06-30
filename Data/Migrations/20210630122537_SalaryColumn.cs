using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpDepAPI.Data.Migrations
{
    public partial class SalaryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Salary",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employees");
        }
    }
}
