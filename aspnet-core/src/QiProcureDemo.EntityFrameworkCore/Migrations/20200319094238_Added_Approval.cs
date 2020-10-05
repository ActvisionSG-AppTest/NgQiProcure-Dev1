using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_Approval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_Approvals",
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
                    RankNo = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    SysRefId = table.Column<int>(nullable: true),
                    TeamId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    SysStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_Approvals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_Approvals_QP_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "QP_Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_Approvals_QP_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "QP_Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_Approvals_QP_SysRefs_SysRefId",
                        column: x => x.SysRefId,
                        principalTable: "QP_SysRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_Approvals_QP_SysStatuses_SysStatusId",
                        column: x => x.SysStatusId,
                        principalTable: "QP_SysStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_Approvals_QP_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "QP_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_Approvals_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_Approvals_AccountId",
                table: "QP_Approvals",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Approvals_ProjectId",
                table: "QP_Approvals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Approvals_SysRefId",
                table: "QP_Approvals",
                column: "SysRefId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Approvals_SysStatusId",
                table: "QP_Approvals",
                column: "SysStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Approvals_TeamId",
                table: "QP_Approvals",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Approvals_TenantId",
                table: "QP_Approvals",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Approvals_UserId",
                table: "QP_Approvals",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_Approvals");
        }
    }
}
