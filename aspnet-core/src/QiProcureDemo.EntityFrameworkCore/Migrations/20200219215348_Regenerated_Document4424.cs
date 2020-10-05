using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Regenerated_Document4424 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "QP_Documents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "QP_Documents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QP_Documents_ProductId",
                table: "QP_Documents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Documents_ServiceId",
                table: "QP_Documents",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_QP_Documents_QP_Products_ProductId",
                table: "QP_Documents",
                column: "ProductId",
                principalTable: "QP_Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QP_Documents_QP_Services_ServiceId",
                table: "QP_Documents",
                column: "ServiceId",
                principalTable: "QP_Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QP_Documents_QP_Products_ProductId",
                table: "QP_Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_QP_Documents_QP_Services_ServiceId",
                table: "QP_Documents");

            migrationBuilder.DropIndex(
                name: "IX_QP_Documents_ProductId",
                table: "QP_Documents");

            migrationBuilder.DropIndex(
                name: "IX_QP_Documents_ServiceId",
                table: "QP_Documents");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "QP_Documents");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "QP_Documents");
        }
    }
}
