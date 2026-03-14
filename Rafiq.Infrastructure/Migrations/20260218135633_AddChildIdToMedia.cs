using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafiq.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChildIdToMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_ChildId",
                table: "Media",
                column: "ChildId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Children_ChildId",
                table: "Media",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Children_ChildId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_ChildId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "Media");
        }
    }
}
