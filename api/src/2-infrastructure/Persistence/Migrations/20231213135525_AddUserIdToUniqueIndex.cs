using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chores.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                table: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name_CreatedBy",
                table: "Tags",
                columns: new[] { "Name", "CreatedBy" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Name_CreatedBy",
                table: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);
        }
    }
}
