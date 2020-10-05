using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QiProcureDemo.Migrations
{
    public partial class Added_ProductImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QP_ProductImages",
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
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QP_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QP_ProductImages_QP_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "QP_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QP_ProductImages_ProductId",
                table: "QP_ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QP_ProductImages_TenantId",
                table: "QP_ProductImages",
                column: "TenantId");

			// Additional SQL Scripts

            var sql = @"CREATE VIEW [dbo].[vwSqlColumns]
				--with encryption
				as
				select objects.name           as ObjectName
					 , objects.type           as ObjectType
					 , columns.name           as ColumnName
					 , columns.column_id      as colid
					 , columns.system_type_id as xtype
					 , columns.system_type_id as type
					 , (case when columns.max_length     =  -1 then 1073741823
							 when columns.system_type_id = 231 then columns.max_length/2
							 when columns.system_type_id =  99 then 1073741823
							 else columns.max_length
						end
					   ) as length
					 , columns.max_length
					 , columns.precision      as prec
					 , cast(0 as bit)             as cdefault
					 , cast(0 as bit)             as isoutparam
					 , (case when columns.system_type_id =  36 then N'uniqueidentifier'
							 when columns.system_type_id =  48 then N'tinyint'
							 when columns.system_type_id =  56 then N'int'
							 when columns.system_type_id = 127 then N'bigint'
							 when columns.system_type_id =  59 then N'real'
							 when columns.system_type_id =  62 then N'float('    + cast(columns.precision    as varchar) + N')'
							 when columns.system_type_id =  60 then N'money'
							 when columns.system_type_id = 104 then N'bit'
							 when columns.system_type_id = 175 then N'char('     + cast(columns.max_length   as varchar) + N')'
							 when columns.system_type_id = 167 then N'varchar('  + (case max_length when -1 then 'max' else cast(columns.max_length   as varchar) end) + N')'
							 when columns.system_type_id = 231 then N'nvarchar(' + (case max_length when -1 then 'max' else cast(columns.max_length/2 as varchar) end) + N')'
							 when columns.system_type_id = 239 then N'nchar('    + cast(columns.max_length/2 as varchar) + N')'
							 when columns.system_type_id =  35 then N'text'
							 when columns.system_type_id =  99 then N'ntext'
							 when columns.system_type_id =  61 then N'datetime'
							 when columns.system_type_id =  34 then N'image'
							 when columns.system_type_id = 106 then N'decimal('  + cast(columns.precision  as varchar) + N', ' + cast(columns.scale as varchar) + N')'
							 when columns.system_type_id = 108 then N'decimal('  + cast(columns.precision  as varchar) + N', ' + cast(columns.scale as varchar) + N')'
							 when columns.system_type_id = 165 then N'varbinary('+ (case max_length when -1 then 'max' else cast(columns.max_length as varchar) end) + N')'
							 when columns.system_type_id = 173 then N'binary('   + cast(columns.max_length as varchar) + N')'
							 when columns.system_type_id = 40 then N'date'
						end
					   ) as ColumnType
					 , (case when columns.system_type_id =  36 then N'SqlDbType.UniqueIdentifier'
							 when columns.system_type_id =  48 then N'SqlDbType.TinyInt'
							 when columns.system_type_id =  56 then N'SqlDbType.Int'
							 when columns.system_type_id = 127 then N'SqlDbType.BigInt'
							 when columns.system_type_id =  59 then N'SqlDbType.Real'
							 when columns.system_type_id =  62 then N'SqlDbType.Real'
							 when columns.system_type_id =  60 then N'SqlDbType.Money'
							 when columns.system_type_id = 104 then N'SqlDbType.Bit'
							 when columns.system_type_id = 175 then N'SqlDbType.Char'
							 when columns.system_type_id = 167 then N'SqlDbType.VarChar'
							 when columns.system_type_id = 231 then N'SqlDbType.NVarChar'
							 when columns.system_type_id = 239 then N'SqlDbType.NChar'
							 when columns.system_type_id =  35 then N'SqlDbType.Text'
							 when columns.system_type_id =  99 then N'SqlDbType.NText'
							 when columns.system_type_id =  61 then N'SqlDbType.DateTime'
							 when columns.system_type_id =  34 then N'SqlDbType.VarBinary'
							 when columns.system_type_id = 106 then N'SqlDbType.Real'
							 when columns.system_type_id = 108 then N'SqlDbType.Real'
							 when columns.system_type_id = 165 then N'SqlDbType.VarBinary'
							 when columns.system_type_id = 173 then N'SqlDbType.Binary'
							 when columns.system_type_id = 40 then N'SqlDbType.Date'
						end
					   ) as SqlDbType
					 -- 01/24/2006 Paul.  A severe error occurred on the current command. The results, if any, should be discarded. 
					 -- MS03-031 security patch causes this error because of stricter datatype processing.  
					 -- http://www.microsoft.com/technet/security/bulletin/MS03-031.mspx.
					 -- http://support.microsoft.com/kb/827366/
					 , (case when columns.system_type_id =  36 then N'Guid'
							 when columns.system_type_id =  48 then N'short'
							 when columns.system_type_id =  56 then N'Int32'
							 when columns.system_type_id = 127 then N'Int64'
							 when columns.system_type_id =  59 then N'float'
							 when columns.system_type_id =  62 then N'float'
							 when columns.system_type_id =  60 then N'decimal'
							 when columns.system_type_id = 104 then N'bool'
							 when columns.system_type_id = 175 then N'ansistring'
							 when columns.system_type_id = 167 then N'ansistring'
							 when columns.system_type_id = 231 then N'string'
							 when columns.system_type_id = 239 then N'string'
							 when columns.system_type_id =  35 then N'string'
							 when columns.system_type_id =  99 then N'string'
							 when columns.system_type_id =  61 then N'DateTime'
							 when columns.system_type_id =  34 then N'byte[]'
							 when columns.system_type_id = 106 then N'float'
							 when columns.system_type_id = 108 then N'float'
							 when columns.system_type_id = 165 then N'byte[]'
							 when columns.system_type_id = 173 then N'byte[]'
							 when columns.system_type_id = 40 then N'Date'
						end
					   ) as CsType
					 , (case when columns.system_type_id =  36 then N'g'
							 when columns.system_type_id =  48 then N'n'
							 when columns.system_type_id =  56 then N'n'
							 when columns.system_type_id = 127 then N'l'
							 when columns.system_type_id =  59 then N'fl'
							 when columns.system_type_id =  62 then N'fl'
							 when columns.system_type_id =  60 then N'd'
							 when columns.system_type_id = 104 then N'b'
							 when columns.system_type_id = 175 then N's'
							 when columns.system_type_id = 167 then N's'
							 when columns.system_type_id = 231 then N's'
							 when columns.system_type_id = 239 then N's'
							 when columns.system_type_id =  35 then N's'
							 when columns.system_type_id =  99 then N's'
							 when columns.system_type_id =  61 then N'dt'
							 when columns.system_type_id =  34 then N'by'
							 when columns.system_type_id = 106 then N'fl'
							 when columns.system_type_id = 108 then N'fl'
							 when columns.system_type_id = 165 then N'bin'
							 when columns.system_type_id = 173 then N'bin'
							 when columns.system_type_id = 40 then N'dt'
						end
					   ) as CsPrefix
					 , columns.is_identity as IsIdentity
					 , columns.is_nullable as IsNullable
				  from      sys.objects         objects
				 inner join sys.columns         columns
						 on columns.object_id = objects.object_id
				 where objects.name <> 'dtproperties'
				   and objects.type in ('U', 'V')
				   and columns.system_type_id <> 189 -- timestamp
				union all
				select procedures.name           as ObjectName
					 , procedures.type           as ObjectType
					 , parameters.name           as ColumnName
					 , parameters.parameter_id   as colid
					 , parameters.system_type_id as xtype
					 , parameters.system_type_id as type
					 , (case when parameters.max_length     =  -1 then 100*1024*1024
							 when parameters.system_type_id = 231 then parameters.max_length/2
							 when parameters.system_type_id =  99 then 100*1024*1024  -- Set maximum file upload size to 100M
							 else parameters.max_length
						end
					   ) as length
					 , parameters.max_length
					 , parameters.precision      as prec
					 , has_default_value             as cdefault
					 , is_output                     as isoutparam
					 , (case when parameters.system_type_id =  36 then N'uniqueidentifier'
							 when parameters.system_type_id =  48 then N'tinyint'
							 when parameters.system_type_id =  56 then N'int'
							 when parameters.system_type_id = 127 then N'bigint'
							 when parameters.system_type_id =  59 then N'real'
							 when parameters.system_type_id =  62 then N'float('    + cast(parameters.precision    as varchar) + N')'
							 when parameters.system_type_id =  60 then N'money'
							 when parameters.system_type_id = 104 then N'bit'
							 when parameters.system_type_id = 175 then N'char('     + cast(parameters.max_length   as varchar) + N')'
							 when parameters.system_type_id = 167 then N'varchar('  + (case max_length when -1 then 'max' else cast(parameters.max_length   as varchar) end) + N')'
							 when parameters.system_type_id = 231 then N'nvarchar(' + (case max_length when -1 then 'max' else cast(parameters.max_length/2 as varchar) end) + N')'
							 when parameters.system_type_id = 239 then N'nchar('    + cast(parameters.max_length/2 as varchar) + N')'
							 when parameters.system_type_id =  35 then N'text'
							 when parameters.system_type_id =  99 then N'ntext'
							 when parameters.system_type_id =  61 then N'datetime'
							 when parameters.system_type_id =  34 then N'image'
							 when parameters.system_type_id = 106 then N'decimal('  + cast(parameters.precision  as varchar) + N', ' + cast(parameters.scale as varchar) + N')'
							 when parameters.system_type_id = 108 then N'decimal('  + cast(parameters.precision  as varchar) + N', ' + cast(parameters.scale as varchar) + N')'
							 when parameters.system_type_id = 165 then N'varbinary('+ (case max_length when -1 then 'max' else cast(parameters.max_length as varchar) end) + N')'
							 when parameters.system_type_id = 173 then N'binary('   + cast(parameters.max_length as varchar) + N')'
							 when parameters.system_type_id = 40 then N'date'
						end
					   ) as ColumnType
					 , (case when parameters.system_type_id =  36 then N'SqlDbType.UniqueIdentifier'
							 when parameters.system_type_id =  48 then N'SqlDbType.TinyInt'
							 when parameters.system_type_id =  56 then N'SqlDbType.Int'
							 when parameters.system_type_id = 127 then N'SqlDbType.BigInt'
							 when parameters.system_type_id =  59 then N'SqlDbType.Real'
							 when parameters.system_type_id =  62 then N'SqlDbType.Real'
							 when parameters.system_type_id =  60 then N'SqlDbType.Money'
							 when parameters.system_type_id = 104 then N'SqlDbType.Bit'
							 when parameters.system_type_id = 175 then N'SqlDbType.Char'
							 when parameters.system_type_id = 167 then N'SqlDbType.VarChar'
							 when parameters.system_type_id = 231 then N'SqlDbType.NVarChar'
							 when parameters.system_type_id = 239 then N'SqlDbType.NChar'
							 when parameters.system_type_id =  35 then N'SqlDbType.Text'
							 when parameters.system_type_id =  99 then N'SqlDbType.NText'
							 when parameters.system_type_id =  61 then N'SqlDbType.DateTime'
							 when parameters.system_type_id =  34 then N'SqlDbType.VarBinary'
							 when parameters.system_type_id = 106 then N'SqlDbType.Real'
							 when parameters.system_type_id = 108 then N'SqlDbType.Real'
							 when parameters.system_type_id = 165 then N'SqlDbType.VarBinary'
							 when parameters.system_type_id = 173 then N'SqlDbType.Binary'
							 when parameters.system_type_id = 40 then N'SqlDbType.Date'
						end
					   ) as SqlDbType
					 -- 01/24/2006 Paul.  A severe error occurred on the current command. The results, if any, should be discarded. 
					 -- MS03-031 security patch causes this error because of stricter datatype processing.  
					 -- http://www.microsoft.com/technet/security/bulletin/MS03-031.mspx.
					 -- http://support.microsoft.com/kb/827366/
					 , (case when parameters.system_type_id =  36 then N'Guid'
							 when parameters.system_type_id =  48 then N'short'
							 when parameters.system_type_id =  56 then N'Int32'
							 when parameters.system_type_id = 127 then N'Int64'
							 when parameters.system_type_id =  59 then N'float'
							 when parameters.system_type_id =  62 then N'float'
							 when parameters.system_type_id =  60 then N'decimal'
							 when parameters.system_type_id = 104 then N'bool'
							 when parameters.system_type_id = 175 then N'ansistring'
							 when parameters.system_type_id = 167 then N'ansistring'
							 when parameters.system_type_id = 231 then N'string'
							 when parameters.system_type_id = 239 then N'string'
							 when parameters.system_type_id =  35 then N'string'
							 when parameters.system_type_id =  99 then N'string'
							 when parameters.system_type_id =  61 then N'DateTime'
							 when parameters.system_type_id =  34 then N'byte[]'
							 when parameters.system_type_id = 106 then N'float'
							 when parameters.system_type_id = 108 then N'float'
							 when parameters.system_type_id = 165 then N'byte[]'
							 when parameters.system_type_id = 173 then N'byte[]'
							 when parameters.system_type_id = 40 then N'date'
						end
					   ) as CsType
					 , (case when parameters.system_type_id =  36 then N'g'
							 when parameters.system_type_id =  48 then N'n'
							 when parameters.system_type_id =  56 then N'n'
							 when parameters.system_type_id = 127 then N'l'
							 when parameters.system_type_id =  59 then N'fl'
							 when parameters.system_type_id =  62 then N'fl'
							 when parameters.system_type_id =  60 then N'd'
							 when parameters.system_type_id = 104 then N'b'
							 when parameters.system_type_id = 175 then N's'
							 when parameters.system_type_id = 167 then N's'
							 when parameters.system_type_id = 231 then N's'
							 when parameters.system_type_id = 239 then N's'
							 when parameters.system_type_id =  35 then N's'
							 when parameters.system_type_id =  99 then N's'
							 when parameters.system_type_id =  61 then N'dt'
							 when parameters.system_type_id =  34 then N'by'
							 when parameters.system_type_id = 106 then N'fl'
							 when parameters.system_type_id = 108 then N'fl'
							 when parameters.system_type_id = 165 then N'bin'
							 when parameters.system_type_id = 173 then N'bin'
							 when parameters.system_type_id = 40 then N'date'
						end
					   ) as CsPrefix
					 , cast(null as bit) as IsIdentity
					 , cast(1    as bit) as IsNullable
				  from      sys.procedures         procedures
				 inner join sys.parameters         parameters
						 on parameters.object_id = procedures.object_id
				 where procedures.type = 'P'
				   and parameters.system_type_id <> 189 -- timestamp
				";

			migrationBuilder.Sql(sql);

			sql = @"CREATE VIEW [dbo].[vwSqlTables]
				--with encryption
				as
				select TABLE_NAME
				  from INFORMATION_SCHEMA.TABLES
				 where TABLE_TYPE = N'BASE TABLE'
				   and TABLE_NAME not in (N'dtproperties', N'sysdiagrams')";

			migrationBuilder.Sql(sql);

			sql = @"CREATE PROCEDURE [dbo].[spSqlBuildAuditTable](@TABLE_NAME varchar(80))
					--with encryption
					as
					  begin
						set nocount on

						declare @Command           varchar(max);  -- varchar(max) on SQL 2005
						declare @AUDIT_TABLE       varchar(90);
						declare @AUDIT_PK          varchar(90);
						declare @COLUMN_NAME       varchar(80);
						declare @COLUMN_TYPE       varchar(20);
						declare @CRLF              char(2);
						declare @COLUMN_MAX_LENGTH int;
						declare @TEST              bit;
	
						declare @PrimaryKey_Field varchar(30);
						declare @FieldCount int;
						set @FieldCount = 0;
	
						set @TEST = 0;
						set @CRLF = char(13) + char(10);
						set @AUDIT_TABLE = @TABLE_NAME + '_AUDIT';
						set @AUDIT_PK    = 'PKA_' + @TABLE_NAME;
	
						if not exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @AUDIT_TABLE and TABLE_TYPE = 'BASE TABLE') begin -- then

							declare TABLE_COLUMNS_CURSOR cursor for
							select ColumnName
								 , ColumnType
							  from vwSqlColumns
							 where ObjectName = @TABLE_NAME
							 order by colid;
	
							select @COLUMN_MAX_LENGTH = max(len(ColumnName)) + 1
							  from vwSqlColumns
							 where ObjectName = @TABLE_NAME;
							if @COLUMN_MAX_LENGTH < 20 begin -- then
								set @COLUMN_MAX_LENGTH = 20;
							end -- if;
	
							set @Command = '';
							set @Command = @Command + 'Create Table dbo.' + @AUDIT_TABLE + @CRLF;
							set @Command = @Command + '	( audit_id'      + space(@COLUMN_MAX_LENGTH+1-len('audit_id'       )) + 'uniqueidentifier DEFAULT (newsequentialid())  not null constraint ' + @AUDIT_PK + ' primary key' + @CRLF;
							set @Command = @Command + '	, audit_date'    + space(@COLUMN_MAX_LENGTH+1-len('audit_date'     )) + 'datetime           not null' + @CRLF;
							set @Command = @Command + '	, audit_action'  + space(@COLUMN_MAX_LENGTH+1-len('audit_action'   )) + 'varchar(30)                not null' + @CRLF;
							set @Command = @Command + '	, audit_version' + space(@COLUMN_MAX_LENGTH+1-len('audit_version'  )) + 'timestamp         not null' + @CRLF;
							open TABLE_COLUMNS_CURSOR;
							fetch next from TABLE_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
								If @COLUMN_TYPE <> 'varbinary(max)' 
								begin
									set @Command = @Command + '	, ' + @COLUMN_NAME + space(@COLUMN_MAX_LENGTH+1-len(@COLUMN_NAME)) + @COLUMN_TYPE + space(18-len(@COLUMN_TYPE)) + ' null' + @CRLF;
								end
								fetch next from TABLE_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							end -- while;
							close TABLE_COLUMNS_CURSOR;
							deallocate TABLE_COLUMNS_CURSOR;

							set @Command = @Command + '	)' + @CRLF;
		
							print 'Create Table dbo.' + @AUDIT_TABLE + ';';

							if @TEST= 1 begin
								print (@Command);
							end else begin
								exec (@Command);
							end

							set @Command = 'create index IDX_' + @AUDIT_TABLE + ' on dbo.' + @AUDIT_TABLE + '(' + @PrimaryKey_Field + ', AUDIT_VERSION)';

							if @TEST= 1 begin
								print (@Command);
							end else begin
								exec (@Command);
							end

						end else begin
							print 'Alter Table dbo.' + @AUDIT_TABLE + ';';
							declare AUDIT_TABLE_COLUMNS_CURSOR cursor for
							select vwSqlColumns.ColumnName
								 , vwSqlColumns.ColumnType
							  from            vwSqlColumns
							  left outer join vwSqlColumns                   vwSqlColumnsAudit
										   on vwSqlColumnsAudit.ObjectName = vwSqlColumns.ObjectName + '_AUDIT'
										  and vwSqlColumnsAudit.ColumnName = vwSqlColumns.ColumnName
							 where vwSqlColumnsAudit.ObjectName is null
							   and vwSqlColumns.ObjectName = @TABLE_NAME
							 order by vwSqlColumns.colid;

							select @COLUMN_MAX_LENGTH = max(len(ColumnName)) + 1
							  from vwSqlColumns
							 where ObjectName = @TABLE_NAME;
							if @COLUMN_MAX_LENGTH < 20 begin -- then
								set @COLUMN_MAX_LENGTH = 20;
							end -- if;

							open AUDIT_TABLE_COLUMNS_CURSOR;
							fetch next from AUDIT_TABLE_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
								If @COLUMN_TYPE <> 'varbinary(max)' 
								begin
									set @Command = 'alter table ' + @AUDIT_TABLE + ' add ' + @COLUMN_NAME + space(@COLUMN_MAX_LENGTH+1-len(@COLUMN_NAME)) + @COLUMN_TYPE + space(18-len(@COLUMN_TYPE)) + ' null' + @CRLF;
									print @Command;
									if @TEST = 0 begin -- then
										exec(@Command);
									end -- if;
								end
								fetch next from AUDIT_TABLE_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							end -- while;
							close AUDIT_TABLE_COLUMNS_CURSOR;
							deallocate AUDIT_TABLE_COLUMNS_CURSOR;
						end -- if;

						execute spSqlBuildAuditTrigger @TABLE_NAME;
					  end";

            migrationBuilder.Sql(sql);

			sql = @"CREATE PROCEDURE [dbo].[spSqlBuildAuditTrigger](@TABLE_NAME varchar(80))
				--with encryption
				as
					begin
					set nocount on
	
					declare @Command      varchar(8000);  -- varchar(max) on SQL 2005
					declare @CRLF         char(2);
					declare @AUDIT_TABLE  varchar(90);
					declare @TRIGGER_NAME varchar(90);
					declare @COLUMN_NAME  varchar(80);
					declare @COLUMN_TYPE  varchar(20);
					declare @PRIMARY_KEY  varchar(100);
					declare @TEST         bit;
					declare @SPLENDID_FIELDS   int;
	
					declare @Action_Insert varchar(30);
					declare @Action_Update varchar(30);
					declare @FieldCount int;
	
					set @TEST = 0;
					set @SPLENDID_FIELDS = 0;
					set @PRIMARY_KEY = 'ID';
	
					set @AUDIT_TABLE = @TABLE_NAME + '_AUDIT';
					set @Action_Insert = 'I';
					set @Action_Update = 'U';
					set @FieldCount = 0;
	
					if exists (select * from vwSqlTables where TABLE_NAME = @AUDIT_TABLE) begin -- then
						set @CRLF = char(13) + char(10);
						declare TRIGGER_COLUMNS_CURSOR cursor for
						select vwSqlColumns.ColumnName
								, vwSqlColumns.ColumnType
							from       vwSqlColumns
							inner join vwSqlColumns                   vwSqlColumnsAudit
									on vwSqlColumnsAudit.ObjectName = vwSqlColumns.ObjectName + '_AUDIT'
									and vwSqlColumnsAudit.ColumnName = vwSqlColumns.ColumnName
							where vwSqlColumns.ObjectName = @TABLE_NAME
							order by vwSqlColumns.colid;
						set @TRIGGER_NAME = 'tr' + @TABLE_NAME + '_Ins_AUDIT';
						if exists (select * from sys.objects where name = @TRIGGER_NAME and type = 'TR') begin -- then
							set @Command = 'Drop   Trigger dbo.' + @TRIGGER_NAME;
							if @TEST = 0 begin -- then
								print @Command;
								exec(@Command);
							end -- if;
						end -- if;
		
						if not exists (select * from sys.objects where name = @TRIGGER_NAME and type = 'TR') begin -- then
							set @Command = '';
							set @Command = @Command + 'Create Trigger dbo.' + @TRIGGER_NAME + ' on dbo.' + @TABLE_NAME + @CRLF;
							set @Command = @Command + 'for insert' + @CRLF;
							set @Command = @Command + 'as' + @CRLF;
							set @Command = @Command + '  begin' + @CRLF;
							set @Command = @Command + '	insert into dbo.' + @AUDIT_TABLE + @CRLF;
							set @Command = @Command + '		(' + @CRLF;
							set @Command = @Command + '	     AUDIT_ACTION'  + @CRLF;
							set @Command = @Command + '	     , AUDIT_DATE'    + @CRLF;
							open TRIGGER_COLUMNS_CURSOR;
							fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
									set @Command = @Command + '	     , ' + @COLUMN_NAME + @CRLF;
								fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							end -- while;
							close TRIGGER_COLUMNS_CURSOR
							set @Command = @Command + '	     )' + @CRLF;
							set @Command = @Command + '	select '           + @CRLF;
							set @Command = @Command + '	      ''I'''  -- insert'      + @CRLF;
							set @Command = @Command + '	     , getdate()'         + @CRLF;
			
							SET @FieldCount = 0;
							open TRIGGER_COLUMNS_CURSOR;
							fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
								if @FieldCount = 0
									SET @PRIMARY_KEY = @COLUMN_NAME;					
									set @Command = @Command + '	     , ' + @TABLE_NAME + '.' + @COLUMN_NAME + @CRLF;
								fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
								SET @FieldCount = @FieldCount + 1;
							end -- while;
							close TRIGGER_COLUMNS_CURSOR;
							set @Command = @Command + '	  from       inserted' + @CRLF;
							set @Command = @Command + '	  inner join ' + @TABLE_NAME + @CRLF;
							set @Command = @Command + '	          on ' + @TABLE_NAME + '.' + @PRIMARY_KEY + ' = inserted.' + @PRIMARY_KEY + ';' + @CRLF;
							set @Command = @Command + '  end' + @CRLF;
							if @TEST = 1 begin -- then
								print @Command + @CRLF;
							end else begin
								print substring(@Command, 1, charindex(@CRLF, @Command));
								exec(@Command);
							end -- if;
						end -- if;

						set @TRIGGER_NAME = 'tr' + @TABLE_NAME + '_Upd_AUDIT';
						if exists (select * from sys.objects where name = @TRIGGER_NAME and type = 'TR') begin -- then
							set @Command = 'Drop   Trigger dbo.' + @TRIGGER_NAME;
							if @TEST = 0 begin -- then
								print @Command;
								exec(@Command);
							end -- if;
						end -- if;

						if not exists (select * from sys.objects where name = @TRIGGER_NAME and type = 'TR') begin -- then
							set @Command = '';
							set @Command = @Command + 'Create Trigger dbo.' + @TRIGGER_NAME + ' on dbo.' + @TABLE_NAME + @CRLF;
							set @Command = @Command + 'for update' + @CRLF;
							set @Command = @Command + 'as' + @CRLF;
							set @Command = @Command + '  begin' + @CRLF;
							set @Command = @Command + '	insert into dbo.' + @AUDIT_TABLE + @CRLF;
							set @Command = @Command + '	     ( '      + @CRLF;
							set @Command = @Command + '	      AUDIT_ACTION'  + @CRLF;
							set @Command = @Command + '	     , AUDIT_DATE'    + @CRLF;
			
							SET @FieldCount = 0;
							open TRIGGER_COLUMNS_CURSOR;
							fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
								if @FieldCount = 0
									SET @PRIMARY_KEY = @COLUMN_NAME;
					
									set @Command = @Command + '	     , ' + @COLUMN_NAME + @CRLF;
								fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
								SET @FieldCount = @FieldCount + 1;
							end -- while;
							close TRIGGER_COLUMNS_CURSOR;
							set @Command = @Command + '	     )' + @CRLF;
							set @Command = @Command + '	select '           + @CRLF;
							set @Command = @Command + '	     ''U''' -- updated'      + @CRLF;
							set @Command = @Command + '	     , getdate()'         + @CRLF;
			
							open TRIGGER_COLUMNS_CURSOR;
							fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
									set @Command = @Command + '	     , ' + @TABLE_NAME + '.' + @COLUMN_NAME + @CRLF;
								fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							end -- while;
							close TRIGGER_COLUMNS_CURSOR;
							set @Command = @Command + '	  from       inserted' + @CRLF;
							set @Command = @Command + '	  inner join ' + @TABLE_NAME + @CRLF;
							set @Command = @Command + '	          on ' + @TABLE_NAME + '.' + @PRIMARY_KEY + ' = inserted.' + @PRIMARY_KEY + ';' + @CRLF;
							set @Command = @Command + '  end' + @CRLF;
							if @TEST = 1 begin -- then
								print @Command + @CRLF;
							end else begin
								print substring(@Command, 1, charindex(@CRLF, @Command));
								exec(@Command);
							end -- if;
						end -- if;

						set @TRIGGER_NAME = 'tr' + @TABLE_NAME + '_Del_AUDIT';
						if exists (select * from sys.objects where name = @TRIGGER_NAME and type = 'TR') begin -- then
							set @Command = 'Drop   Trigger dbo.' + @TRIGGER_NAME;
							if @TEST = 0 begin -- then
								print @Command;
								exec(@Command);
							end -- if;
						end -- if;

						if not exists (select * from sys.objects where name = @TRIGGER_NAME and type = 'TR') begin -- then
							set @Command = ''
							set @Command = @Command + 'Create Trigger dbo.' + @TRIGGER_NAME + ' on dbo.' + @TABLE_NAME + @CRLF;
							set @Command = @Command + 'for delete' + @CRLF;
							set @Command = @Command + 'as' + @CRLF;
							set @Command = @Command + '  begin' + @CRLF;
							set @Command = @Command + '	insert into dbo.' + @AUDIT_TABLE + @CRLF;
							set @Command = @Command + '	     ( AUDIT_ID'     + @CRLF;
							set @Command = @Command + '	     , AUDIT_ACTION' + @CRLF;
							set @Command = @Command + '	     , AUDIT_DATE'   + @CRLF;
							open TRIGGER_COLUMNS_CURSOR;
							fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
								--Cannot use text, ntext, or image columns in the 'inserted' and 'deleted' tables.
								if @COLUMN_TYPE <> 'text' and @COLUMN_TYPE <> 'ntext' and @COLUMN_TYPE <> 'image' begin -- then
									set @Command = @Command + '	     , ' + @COLUMN_NAME + @CRLF;
								end -- if;
								fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							end -- while;
							close TRIGGER_COLUMNS_CURSOR;
							set @Command = @Command + '	     )' + @CRLF;
							set @Command = @Command + '	select newid()'       + @CRLF;
							set @Command = @Command + '	     , ''D'''  -- delete' + @CRLF;
							set @Command = @Command + '	     , getdate()'     + @CRLF;
			
							open TRIGGER_COLUMNS_CURSOR;
							fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							while @@FETCH_STATUS = 0 begin -- while
								--Cannot use text, ntext, or image columns in the 'inserted' and 'deleted' tables.
								if @COLUMN_TYPE <> 'text' and @COLUMN_TYPE <> 'ntext' and @COLUMN_TYPE <> 'image' begin -- then
									set @Command = @Command + '	     , ' + @COLUMN_NAME + @CRLF;
								end -- if;
								fetch next from TRIGGER_COLUMNS_CURSOR into @COLUMN_NAME, @COLUMN_TYPE;
							end -- while;
							close TRIGGER_COLUMNS_CURSOR;
							set @Command = @Command + '	  from deleted;' + @CRLF;
							set @Command = @Command + '  end' + @CRLF;
							if @TEST = 1 begin -- then
								print @Command + @CRLF;
							end else begin
								print substring(@Command, 1, charindex(@CRLF, @Command));
								exec(@Command);
							end -- if;
						end -- if;
		
						deallocate TRIGGER_COLUMNS_CURSOR;

					end -- if;
			end";

			migrationBuilder.Sql(sql);

			sql = @"CREATE PROCEDURE [dbo].[SearchAllTables]
				(
					@SearchStr nvarchar(100)
				)
				AS
				BEGIN

				DECLARE @Results TABLE(ColumnName nvarchar(370), ColumnValue nvarchar(3630))

				SET NOCOUNT ON

				DECLARE @TableName nvarchar(256), @ColumnName nvarchar(128), @SearchStr2 nvarchar(110)
				SET  @TableName = ''
				SET @SearchStr2 = QUOTENAME('%' + @SearchStr + '%','''')

				WHILE @TableName IS NOT NULL
				BEGIN
					SET @ColumnName = ''
					SET @TableName = 
					(
						SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME))
						FROM    INFORMATION_SCHEMA.TABLES
						WHERE       TABLE_TYPE = 'BASE TABLE'
							AND QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableName
							AND OBJECTPROPERTY(
									OBJECT_ID(
										QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)
											), 'IsMSShipped'
											) = 0
					)

					WHILE (@TableName IS NOT NULL) AND (@ColumnName IS NOT NULL)
					BEGIN
						SET @ColumnName =
						(
							SELECT MIN(QUOTENAME(COLUMN_NAME))
							FROM    INFORMATION_SCHEMA.COLUMNS
							WHERE       TABLE_SCHEMA    = PARSENAME(@TableName, 2)
								AND TABLE_NAME  = PARSENAME(@TableName, 1)
								AND DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar')
								AND QUOTENAME(COLUMN_NAME) > @ColumnName
						)

						IF @ColumnName IS NOT NULL
						BEGIN
							INSERT INTO @Results
							EXEC
							(
								'SELECT ''' + @TableName + '.' + @ColumnName + ''', LEFT(' + @ColumnName + ', 3630) 
								FROM ' + @TableName + ' (NOLOCK) ' +
								' WHERE ' + @ColumnName + ' LIKE ' + @SearchStr2
							)
						END
					END 
				END

				SELECT ColumnName, ColumnValue FROM @Results
			END";

			migrationBuilder.Sql(sql);

			sql = @"EXEC dbo.spSqlBuildAuditTable 'QP_ProductImages'";
			migrationBuilder.Sql(sql);

			sql = @"CREATE TRIGGER [dbo].[tr_QP_ProductImages_IsMain_Modified]
			   ON [dbo].[QP_ProductImages]
			   AFTER UPDATE
			AS BEGIN
				SET NOCOUNT ON;
				IF UPDATE (IsMain) 
				BEGIN
					DECLARE @beforeIsMain bit
					DECLARE @afterIsMain bit
					Declare @productImageId int
					SELECT  @productImageId=id, @afterIsMain= IsMain FROM Inserted
					SELECT  @beforeIsMain = IsMain FROM deleted		
					IF @afterIsMain = 1 and @beforeIsMain = 0 begin
						UPDATE QP_ProductImages set IsMain = 0 where id <> @productImageId and IsDeleted = 0
					end
				END 
			END";
			migrationBuilder.Sql(sql);
			
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QP_ProductImages");

			var sql = @"DROP PROCEDURE dbo.SearchAllTables";
			migrationBuilder.Sql(sql);

			sql = @"DROP PROCEDURE dbo.spSqlBuildAuditTable";
			migrationBuilder.Sql(sql);

			sql = @"DROP PROCEDURE dbo.spSqlBuildAuditTrigger";
			migrationBuilder.Sql(sql);

			sql = @"DROP VIEW dbo.vwSqlTables";
			migrationBuilder.Sql(sql);

			sql = @"DROP VIEW dbo.vwSqlColumns";
			migrationBuilder.Sql(sql);

			sql = @"DROP TRIGGER dbo.tr_QP_ProductImages_IsMain_Modified";
			migrationBuilder.Sql(sql);

			sql = @"DROP TABLE dbo.QP_ProductImages_AUDIT";
			migrationBuilder.Sql(sql);

		}
	}
}
