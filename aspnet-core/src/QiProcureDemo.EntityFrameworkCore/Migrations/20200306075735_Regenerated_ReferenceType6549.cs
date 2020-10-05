using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Regenerated_ReferenceType6549 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeCode",
                table: "QP_ReferenceTypes",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeGroup",
                table: "QP_ReferenceTypes",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceTypeCode",
                table: "QP_ReferenceTypes");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeGroup",
                table: "QP_ReferenceTypes");
        }
    }
}
