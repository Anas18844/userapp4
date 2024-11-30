using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace userapp.Migrations.EventsDb
{
    /// <inheritdoc />
    public partial class RemoveCoverImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "CoverImagePath",
            table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "CoverImagePath",
            table: "Events",
            type: "nvarchar(max)",
            nullable: true);
        }
    }
}
