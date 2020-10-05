using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Regenerated_Team9076 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceTypeId",
                table: "QP_Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QP_Teams_ReferenceTypeId",
                table: "QP_Teams",
                column: "ReferenceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_QP_Teams_QP_ReferenceTypes_ReferenceTypeId",
                table: "QP_Teams",
                column: "ReferenceTypeId",
                principalTable: "QP_ReferenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QP_Teams_QP_ReferenceTypes_ReferenceTypeId",
                table: "QP_Teams");

            migrationBuilder.DropIndex(
                name: "IX_QP_Teams_ReferenceTypeId",
                table: "QP_Teams");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeId",
                table: "QP_Teams");
        }
    }
}
