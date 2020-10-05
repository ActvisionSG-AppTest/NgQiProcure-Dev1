using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_Team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_Teams",
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
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    SysStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_Teams_QP_SysStatuses_SysStatusId",
                        column: x => x.SysStatusId,
                        principalTable: "QP_SysStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_Teams_SysStatusId",
                table: "QP_Teams",
                column: "SysStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_Teams_TenantId",
                table: "QP_Teams",
                column: "TenantId");

			// Additional SQL Scripts

			var sql = @"ALTER TABLE [dbo].[QP_Teams] ADD  CONSTRAINT [CS_QP_Teams_IsActive]  DEFAULT ((1)) FOR [IsActive]";
			migrationBuilder.Sql(sql);

            sql = @"ALTER TABLE [dbo].[QP_Teams] ADD  CONSTRAINT [CS_QP_Teams_StatusId]  DEFAULT ((3)) FOR [SysStatusId]";
            migrationBuilder.Sql(sql);

            sql = @"CREATE TRIGGER[dbo].[tr_QP_Teams_ReferenceTypeId_Modified]
                    ON[dbo].QP_Teams
                    AFTER UPDATE
                    AS BEGIN
                        SET NOCOUNT ON;
                        IF UPDATE(ReferenceTypeId)

                                BEGIN
                                    DECLARE @beforeReferenceTypeId int
                                    DECLARE @afterReferenceTypeId int
                                    Declare @TeamId int
                                    SELECT  @TeamId= id, @afterReferenceTypeId = ReferenceTypeId FROM Inserted
                                    SELECT  @beforeReferenceTypeId = ReferenceTypeId FROM deleted
                                    IF @beforeReferenceTypeId<> @afterReferenceTypeId and @beforeReferenceTypeId is not null begin
                                        UPDATE QP_TeamMembers set SysRefId = null where TeamId = @TeamId and IsDeleted = 0;
                                end
                         END

                   END";
             migrationBuilder.Sql(sql);

        }

    protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_Teams");
        }
    }
}
