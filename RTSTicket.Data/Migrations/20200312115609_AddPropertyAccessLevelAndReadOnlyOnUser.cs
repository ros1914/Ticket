using Microsoft.EntityFrameworkCore.Migrations;

namespace RTSTicket.Data.Migrations
{
    public partial class AddPropertyAccessLevelAndReadOnlyOnUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Access_Level",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Read_Only",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access_Level",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Read_Only",
                table: "Users");
        }
    }
}
