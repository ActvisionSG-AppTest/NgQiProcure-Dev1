using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_ProjectInstruction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_ProjectInstructions",
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
                    InstructionNo = table.Column<int>(nullable: false),
                    Instructions = table.Column<string>(maxLength: 10000, nullable: true),
                    Remarks = table.Column<string>(maxLength: 5000, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_ProjectInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_ProjectInstructions_QP_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "QP_Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_ProjectInstructions_ProjectId",
                table: "QP_ProjectInstructions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ProjectInstructions_TenantId",
                table: "QP_ProjectInstructions",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_ProjectInstructions");
        }
    }
}
