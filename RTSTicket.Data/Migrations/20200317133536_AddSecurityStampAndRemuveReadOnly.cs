using Microsoft.EntityFrameworkCore.Migrations;

namespace RTSTicket.Data.Migrations
{
    public partial class AddSecurityStampAndRemuveReadOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access_Level",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Access_Level",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
