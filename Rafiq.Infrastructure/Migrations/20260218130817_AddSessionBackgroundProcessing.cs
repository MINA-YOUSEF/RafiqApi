using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafiq.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionBackgroundProcessing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE [Sessions] SET [Status] = 5 WHERE [Status] = 4;
                UPDATE [Sessions] SET [Status] = 4 WHERE [Status] = 3;
                """);

            migrationBuilder.AddColumn<int>(
                name: "AnalysisAttempts",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "Sessions",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                """
                INSERT INTO [Media] ([Url], [ThumbnailUrl], [PublicId], [Description], [Category], [UploadedByUserId], [UploadedAt], [IsDeleted], [CreatedAtUtc], [UpdatedAtUtc])
                SELECT
                    [s].[SessionVideoUrl],
                    NULL,
                    CONCAT('legacy/session-', CAST([s].[Id] AS nvarchar(20))),
                    'Migrated from SessionVideoUrl',
                    4,
                    0,
                    GETUTCDATE(),
                    0,
                    GETUTCDATE(),
                    NULL
                FROM [Sessions] AS [s]
                WHERE [s].[SessionVideoUrl] IS NOT NULL;
                """);

            migrationBuilder.Sql(
                """
                UPDATE [s]
                SET [s].[MediaId] = [m].[Id]
                FROM [Sessions] AS [s]
                INNER JOIN [Media] AS [m]
                    ON [m].[PublicId] = CONCAT('legacy/session-', CAST([s].[Id] AS nvarchar(20)));
                """);

            migrationBuilder.AddColumn<int>(
                name: "AnalyzedSessionsCount",
                table: "Children",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageAccuracyScore",
                table: "Children",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_MediaId",
                table: "Sessions",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Media_MediaId",
                table: "Sessions",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "SessionVideoUrl",
                table: "Sessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE [Sessions] SET [Status] = 2 WHERE [Status] = 3;
                UPDATE [Sessions] SET [Status] = 3 WHERE [Status] = 4;
                UPDATE [Sessions] SET [Status] = 4 WHERE [Status] = 5;
                """);

            migrationBuilder.AddColumn<string>(
                name: "SessionVideoUrl",
                table: "Sessions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE [s]
                SET [s].[SessionVideoUrl] = [m].[Url]
                FROM [Sessions] AS [s]
                LEFT JOIN [Media] AS [m] ON [m].[Id] = [s].[MediaId];
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Media_MediaId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_MediaId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "AnalysisAttempts",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "AnalyzedSessionsCount",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "AverageAccuracyScore",
                table: "Children");
        }
    }
}
