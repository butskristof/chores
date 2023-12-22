using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chores.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChoreNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Chores",
                type: "nvarchar(max)",
                maxLength: -1,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Chores");
        }
    }
}
