using Microsoft.EntityFrameworkCore.Migrations;

namespace RTSTicket.Data.Migrations
{
    public partial class RemuveReadOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Read_Only",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Read_Only",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
