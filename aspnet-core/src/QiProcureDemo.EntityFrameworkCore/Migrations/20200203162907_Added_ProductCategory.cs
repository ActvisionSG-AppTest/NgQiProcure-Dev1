using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_ProductCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_ProductCategories_QP_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "QP_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QP_ProductCategories_QP_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "QP_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_ProductCategories_CategoryId",
                table: "QP_ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ProductCategories_ProductId",
                table: "QP_ProductCategories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ProductCategories_TenantId",
                table: "QP_ProductCategories",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_ProductCategories");
        }
    }
}
