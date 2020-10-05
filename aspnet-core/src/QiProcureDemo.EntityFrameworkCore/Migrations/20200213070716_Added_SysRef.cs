using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_SysRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_SysRefs",
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
                    RefCode = table.Column<string>(maxLength: 30, nullable: false),
                    ReferenceTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_SysRefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_SysRefs_QP_ReferenceTypes_ReferenceTypeId",
                        column: x => x.ReferenceTypeId,
                        principalTable: "QP_ReferenceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_SysRefs_ReferenceTypeId",
                table: "QP_SysRefs",
                column: "ReferenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_SysRefs_TenantId",
                table: "QP_SysRefs",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_SysRefs");
        }
    }
}
