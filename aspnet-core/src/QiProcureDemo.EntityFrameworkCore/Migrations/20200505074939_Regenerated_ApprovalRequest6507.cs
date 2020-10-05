using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Regenerated_ApprovalRequest6507 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "QP_ApprovalRequests",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ApprovalRequests_UserId",
                table: "QP_ApprovalRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QP_ApprovalRequests_AbpUsers_UserId",
                table: "QP_ApprovalRequests",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QP_ApprovalRequests_AbpUsers_UserId",
                table: "QP_ApprovalRequests");

            migrationBuilder.DropIndex(
                name: "IX_QP_ApprovalRequests_UserId",
                table: "QP_ApprovalRequests");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "QP_ApprovalRequests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
