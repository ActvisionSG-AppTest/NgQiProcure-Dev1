using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Regenerated_Service5402 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "QP_Services",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SysRefId",
                table: "QP_Services",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QP_Services_CategoryId",
                table: "QP_Services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Services_SysRefId",
                table: "QP_Services",
                column: "SysRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_QP_Services_QP_Categories_CategoryId",
                table: "QP_Services",
                column: "CategoryId",
                principalTable: "QP_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QP_Services_QP_SysRefs_SysRefId",
                table: "QP_Services",
                column: "SysRefId",
                principalTable: "QP_SysRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QP_Services_QP_Categories_CategoryId",
                table: "QP_Services");

            migrationBuilder.DropForeignKey(
                name: "FK_QP_Services_QP_SysRefs_SysRefId",
                table: "QP_Services");

            migrationBuilder.DropIndex(
                name: "IX_QP_Services_CategoryId",
                table: "QP_Services");

            migrationBuilder.DropIndex(
                name: "IX_QP_Services_SysRefId",
                table: "QP_Services");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "QP_Services");

            migrationBuilder.DropColumn(
                name: "SysRefId",
                table: "QP_Services");
        }
    }
}
