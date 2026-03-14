using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafiq.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileImageToProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileImageMediaId",
                table: "SpecialistProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileImageMediaId",
                table: "ParentProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileImageMediaId",
                table: "Children",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialistProfiles_ProfileImageMediaId",
                table: "SpecialistProfiles",
                column: "ProfileImageMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentProfiles_ProfileImageMediaId",
                table: "ParentProfiles",
                column: "ProfileImageMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Children_ProfileImageMediaId",
                table: "Children",
                column: "ProfileImageMediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Children_Media_ProfileImageMediaId",
                table: "Children",
                column: "ProfileImageMediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentProfiles_Media_ProfileImageMediaId",
                table: "ParentProfiles",
                column: "ProfileImageMediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialistProfiles_Media_ProfileImageMediaId",
                table: "SpecialistProfiles",
                column: "ProfileImageMediaId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Children_Media_ProfileImageMediaId",
                table: "Children");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentProfiles_Media_ProfileImageMediaId",
                table: "ParentProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialistProfiles_Media_ProfileImageMediaId",
                table: "SpecialistProfiles");

            migrationBuilder.DropIndex(
                name: "IX_SpecialistProfiles_ProfileImageMediaId",
                table: "SpecialistProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ParentProfiles_ProfileImageMediaId",
                table: "ParentProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Children_ProfileImageMediaId",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "ProfileImageMediaId",
                table: "SpecialistProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImageMediaId",
                table: "ParentProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImageMediaId",
                table: "Children");
        }
    }
}
