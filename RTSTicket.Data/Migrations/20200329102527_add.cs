using Microsoft.EntityFrameworkCore.Migrations;

namespace RTSTicket.Data.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRolse_Roles_RoleId",
                table: "UserRolse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRolse_Users_UserId",
                table: "UserRolse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRolse",
                table: "UserRolse");

            migrationBuilder.RenameTable(
                name: "UserRolse",
                newName: "UserRolses");

            migrationBuilder.RenameIndex(
                name: "IX_UserRolse_RoleId",
                table: "UserRolses",
                newName: "IX_UserRolses_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRolses",
                table: "UserRolses",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRolses_Roles_RoleId",
                table: "UserRolses",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRolses_Users_UserId",
                table: "UserRolses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRolses_Roles_RoleId",
                table: "UserRolses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRolses_Users_UserId",
                table: "UserRolses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRolses",
                table: "UserRolses");

            migrationBuilder.RenameTable(
                name: "UserRolses",
                newName: "UserRolse");

            migrationBuilder.RenameIndex(
                name: "IX_UserRolses_RoleId",
                table: "UserRolse",
                newName: "IX_UserRolse_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRolse",
                table: "UserRolse",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRolse_Roles_RoleId",
                table: "UserRolse",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRolse_Users_UserId",
                table: "UserRolse",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
