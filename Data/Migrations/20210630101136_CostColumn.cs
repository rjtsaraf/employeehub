using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpDepAPI.Data.Migrations
{
    public partial class CostColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Cost",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Projects");
        }
    }
}
