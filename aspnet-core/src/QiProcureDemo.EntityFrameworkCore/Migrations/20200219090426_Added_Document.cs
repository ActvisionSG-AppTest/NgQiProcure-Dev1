using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_Document : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
       
            migrationBuilder.CreateTable(
                name: "QP_Documents",
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
                    Url = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Bytes = table.Column<byte[]>(nullable: true),
                    SysRefId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_Documents_QP_SysRefs_SysRefId",
                        column: x => x.SysRefId,
                        principalTable: "QP_SysRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_ServicePrices_ServiceId",
                table: "QP_ServicePrices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Documents_SysRefId",
                table: "QP_Documents",
                column: "SysRefId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Documents_TenantId",
                table: "QP_Documents",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_QP_ServicePrices_QP_Services_ServiceId",
                table: "QP_ServicePrices",
                column: "ServiceId",
                principalTable: "QP_Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QP_ServicePrices_QP_Services_ServiceId",
                table: "QP_ServicePrices");

            migrationBuilder.DropTable(
                name: "QP_Documents");

            migrationBuilder.DropIndex(
                name: "IX_QP_ServicePrices_ServiceId",
                table: "QP_ServicePrices");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "QP_ServicePrices");
         
        }
    }
}
