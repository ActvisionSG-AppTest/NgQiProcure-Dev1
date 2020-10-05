using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Regenerated_Account8333 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "QP_Accounts",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "QP_Accounts",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "QP_Accounts",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "QP_Accounts");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "QP_Accounts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "QP_Accounts");
        }
    }
}
