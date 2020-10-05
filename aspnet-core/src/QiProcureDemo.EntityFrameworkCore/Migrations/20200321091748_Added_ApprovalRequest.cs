using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_ApprovalRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_ApprovalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ReferenceId = table.Column<int>(nullable: false),
                    OrderNo = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RankNo = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    SysRefId = table.Column<int>(nullable: true),
                    SysStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_ApprovalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_ApprovalRequests_QP_SysRefs_SysRefId",
                        column: x => x.SysRefId,
                        principalTable: "QP_SysRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_ApprovalRequests_QP_SysStatuses_SysStatusId",
                        column: x => x.SysStatusId,
                        principalTable: "QP_SysStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_ApprovalRequests_SysRefId",
                table: "QP_ApprovalRequests",
                column: "SysRefId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ApprovalRequests_SysStatusId",
                table: "QP_ApprovalRequests",
                column: "SysStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ApprovalRequests_TenantId",
                table: "QP_ApprovalRequests",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_ApprovalRequests");
        }
    }
}
