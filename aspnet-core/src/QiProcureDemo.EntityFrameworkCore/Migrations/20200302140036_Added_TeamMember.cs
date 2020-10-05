using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_TeamMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_TeamMembers",
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
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    ReportingTeamMemberId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    SysRefId = table.Column<int>(nullable: true),
                    SysStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_TeamMembers_QP_SysRefs_SysRefId",
                        column: x => x.SysRefId,
                        principalTable: "QP_SysRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_TeamMembers_QP_SysStatuses_SysStatusId",
                        column: x => x.SysStatusId,
                        principalTable: "QP_SysStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_TeamMembers_QP_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "QP_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_TeamMembers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_TeamMembers_SysRefId",
                table: "QP_TeamMembers",
                column: "SysRefId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_TeamMembers_SysStatusId",
                table: "QP_TeamMembers",
                column: "SysStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_TeamMembers_TeamId",
                table: "QP_TeamMembers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_TeamMembers_TenantId",
                table: "QP_TeamMembers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_TeamMembers_UserId",
                table: "QP_TeamMembers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_TeamMembers");
        }
    }
}
