using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafiq.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeMedicalReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalReports_ParentProfiles_UploadedByParentProfileId",
                table: "MedicalReports");

            migrationBuilder.DropIndex(
                name: "IX_MedicalReports_UploadedByParentProfileId",
                table: "MedicalReports");

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "MedicalReports",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                INSERT INTO [Media] ([Url], [ThumbnailUrl], [PublicId], [Description], [Category], [UploadedByUserId], [ChildId], [UploadedAt], [IsDeleted], [CreatedAtUtc], [UpdatedAtUtc])
                SELECT
                    [mr].[ReportUrl],
                    NULL,
                    CONCAT('legacy/medical-report-', CAST([mr].[Id] AS nvarchar(20))),
                    'Migrated medical report',
                    3,
                    [pp].[UserId],
                    [mr].[ChildId],
                    [mr].[CreatedAtUtc],
                    CAST(0 AS bit),
                    [mr].[CreatedAtUtc],
                    [mr].[UpdatedAtUtc]
                FROM [MedicalReports] AS [mr]
                INNER JOIN [ParentProfiles] AS [pp] ON [pp].[Id] = [mr].[UploadedByParentProfileId];
                ");

            migrationBuilder.Sql(@"
                UPDATE [mr]
                SET
                    [mr].[MediaId] = [m].[Id],
                    [mr].[UploadedByParentProfileId] = [pp].[UserId]
                FROM [MedicalReports] AS [mr]
                INNER JOIN [ParentProfiles] AS [pp] ON [pp].[Id] = [mr].[UploadedByParentProfileId]
                INNER JOIN [Media] AS [m] ON [m].[PublicId] = CONCAT('legacy/medical-report-', CAST([mr].[Id] AS nvarchar(20)));
                ");

            migrationBuilder.RenameColumn(
                name: "UploadedByParentProfileId",
                table: "MedicalReports",
                newName: "UploadedByUserId");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "MedicalReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "ReportUrl",
                table: "MedicalReports");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalReports_MediaId",
                table: "MedicalReports",
                column: "MediaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalReports_Media_MediaId",
                table: "MedicalReports",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalReports_Media_MediaId",
                table: "MedicalReports");

            migrationBuilder.DropIndex(
                name: "IX_MedicalReports_MediaId",
                table: "MedicalReports");

            migrationBuilder.AddColumn<string>(
                name: "ReportUrl",
                table: "MedicalReports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE [mr]
                SET [mr].[ReportUrl] = [m].[Url]
                FROM [MedicalReports] AS [mr]
                INNER JOIN [Media] AS [m] ON [m].[Id] = [mr].[MediaId];
                ");

            migrationBuilder.Sql(@"
                DELETE FROM [MedicalReports]
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM [ParentProfiles] AS [pp]
                    WHERE [pp].[UserId] = [MedicalReports].[UploadedByUserId]
                );
                ");

            migrationBuilder.Sql(@"
                UPDATE [mr]
                SET [mr].[UploadedByUserId] = [pp].[Id]
                FROM [MedicalReports] AS [mr]
                INNER JOIN [ParentProfiles] AS [pp] ON [pp].[UserId] = [mr].[UploadedByUserId];
                ");

            migrationBuilder.RenameColumn(
                name: "UploadedByUserId",
                table: "MedicalReports",
                newName: "UploadedByParentProfileId");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "MedicalReports");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalReports_UploadedByParentProfileId",
                table: "MedicalReports",
                column: "UploadedByParentProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalReports_ParentProfiles_UploadedByParentProfileId",
                table: "MedicalReports",
                column: "UploadedByParentProfileId",
                principalTable: "ParentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
