using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_Accounts",
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
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    IsPersonal = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    Code = table.Column<string>(maxLength: 30, nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    SysStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_Accounts_QP_SysStatuses_SysStatusId",
                        column: x => x.SysStatusId,
                        principalTable: "QP_SysStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_Accounts_QP_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "QP_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_Accounts_SysStatusId",
                table: "QP_Accounts",
                column: "SysStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Accounts_TeamId",
                table: "QP_Accounts",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Accounts_TenantId",
                table: "QP_Accounts",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_Accounts");
        }
    }
}
