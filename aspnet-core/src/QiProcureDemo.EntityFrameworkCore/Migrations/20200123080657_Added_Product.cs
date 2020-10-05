using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_Products",
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
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Stock = table.Column<double>(nullable: false),
                    Uom = table.Column<string>(maxLength: 15, nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_Products_QP_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "QP_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_Products_CategoryId",
                table: "QP_Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Products_TenantId",
                table: "QP_Products",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_Products");
        }
    }
}
