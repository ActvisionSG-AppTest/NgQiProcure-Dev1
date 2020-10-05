using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_ServiceImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_ServiceImages",
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
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Url = table.Column<string>(maxLength: 100, nullable: true),
                    IsMain = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    Bytes = table.Column<byte[]>(nullable: false),
                    ServiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_ServiceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_ServiceImages_QP_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "QP_Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_ServiceImages_ServiceId",
                table: "QP_ServiceImages",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ServiceImages_TenantId",
                table: "QP_ServiceImages",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_ServiceImages");
        }
    }
}
