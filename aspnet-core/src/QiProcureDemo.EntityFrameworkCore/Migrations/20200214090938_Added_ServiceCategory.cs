using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_ServiceCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_ServiceCategories",
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
                    ServiceId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_ServiceCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_ServiceCategories_QP_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "QP_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_ServiceCategories_QP_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "QP_Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_ServiceCategories_CategoryId",
                table: "QP_ServiceCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ServiceCategories_ServiceId",
                table: "QP_ServiceCategories",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ServiceCategories_TenantId",
                table: "QP_ServiceCategories",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_ServiceCategories");
        }
    }
}
