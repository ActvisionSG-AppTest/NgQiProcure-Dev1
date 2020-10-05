using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_Emails",
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
                    EmailFrom = table.Column<string>(maxLength: 50, nullable: false),
                    EmailTo = table.Column<string>(maxLength: 5000, nullable: false),
                    EmailCC = table.Column<string>(maxLength: 5000, nullable: true),
                    EmailBCC = table.Column<string>(maxLength: 5000, nullable: true),
                    Subject = table.Column<string>(maxLength: 500, nullable: false),
                    Body = table.Column<string>(maxLength: 999999, nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    SentDate = table.Column<DateTime>(nullable: false),
                    SysRefId = table.Column<int>(nullable: true),
                    SysStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_Emails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_Emails_QP_SysRefs_SysRefId",
                        column: x => x.SysRefId,
                        principalTable: "QP_SysRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_Emails_QP_SysStatuses_SysStatusId",
                        column: x => x.SysStatusId,
                        principalTable: "QP_SysStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_Emails_SysRefId",
                table: "QP_Emails",
                column: "SysRefId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Emails_SysStatusId",
                table: "QP_Emails",
                column: "SysStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Emails_TenantId",
                table: "QP_Emails",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_Emails");
        }
    }
}
