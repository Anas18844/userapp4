using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace userapp.Migrations.EventsDb
{
    /// <inheritdoc />
    public partial class AddTicketPricesToEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RegularPrice",
                table: "Events",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VIPPrice",
                table: "Events",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegularPrice",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "VIPPrice",
                table: "Events");
        }
    }
}
