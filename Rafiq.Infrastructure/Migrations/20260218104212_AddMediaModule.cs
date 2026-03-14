using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafiq.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PublicId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                """
                INSERT INTO [Media] ([Url], [ThumbnailUrl], [PublicId], [Description], [Category], [UploadedByUserId], [UploadedAt], [IsDeleted], [CreatedAtUtc], [UpdatedAtUtc])
                SELECT
                    [e].[DemoVideoUrl],
                    NULL,
                    CONCAT('legacy/exercise-', CAST([e].[Id] AS nvarchar(20))),
                    'Migrated from DemoVideoUrl',
                    1,
                    0,
                    GETUTCDATE(),
                    0,
                    GETUTCDATE(),
                    NULL
                FROM [Exercises] AS [e];
                """);

            migrationBuilder.Sql(
                """
                UPDATE [e]
                SET [e].[MediaId] = [m].[Id]
                FROM [Exercises] AS [e]
                INNER JOIN [Media] AS [m]
                    ON [m].[PublicId] = CONCAT('legacy/exercise-', CAST([e].[Id] AS nvarchar(20)));
                """);

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "Exercises",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MediaId",
                table: "Exercises",
                column: "MediaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_Category",
                table: "Media",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Media_IsDeleted",
                table: "Media",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Media_MediaId",
                table: "Exercises",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "DemoVideoUrl",
                table: "Exercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DemoVideoUrl",
                table: "Exercises",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE [e]
                SET [e].[DemoVideoUrl] = [m].[Url]
                FROM [Exercises] AS [e]
                LEFT JOIN [Media] AS [m] ON [m].[Id] = [e].[MediaId];
                """);

            migrationBuilder.Sql(
                """
                UPDATE [Exercises]
                SET [DemoVideoUrl] = ''
                WHERE [DemoVideoUrl] IS NULL;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Media_MediaId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_MediaId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "DemoVideoUrl",
                table: "Exercises",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.DropTable(
                name: "Media");
        }
    }
}
