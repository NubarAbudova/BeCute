using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnchantElegance.Persistence.Contexts.Migrations
{
    public partial class AddNewColumnCoilorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "No",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No",
                table: "Colors");
        }
    }
}
