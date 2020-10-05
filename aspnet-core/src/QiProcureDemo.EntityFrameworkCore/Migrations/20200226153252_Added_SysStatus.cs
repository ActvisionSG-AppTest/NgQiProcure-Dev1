using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_SysStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_SysStatuses",
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
                    Code = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SysRefId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_SysStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_SysStatuses_QP_SysRefs_SysRefId",
                        column: x => x.SysRefId,
                        principalTable: "QP_SysRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_SysStatuses_SysRefId",
                table: "QP_SysStatuses",
                column: "SysRefId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_SysStatuses_TenantId",
                table: "QP_SysStatuses",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_SysStatuses");
        }
    }
}
