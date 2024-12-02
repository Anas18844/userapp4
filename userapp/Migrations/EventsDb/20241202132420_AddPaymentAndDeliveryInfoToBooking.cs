using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace userapp.Migrations.EventsDb
{
    /// <inheritdoc />
    public partial class AddPaymentAndDeliveryInfoToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryInfoId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentDetailsId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DeliveryInfoId",
                table: "Bookings",
                column: "DeliveryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentDetailsId",
                table: "Bookings",
                column: "PaymentDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_DeliveryInfos_DeliveryInfoId",
                table: "Bookings",
                column: "DeliveryInfoId",
                principalTable: "DeliveryInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_PaymentDetails_PaymentDetailsId",
                table: "Bookings",
                column: "PaymentDetailsId",
                principalTable: "PaymentDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_DeliveryInfos_DeliveryInfoId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_PaymentDetails_PaymentDetailsId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_DeliveryInfoId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PaymentDetailsId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DeliveryInfoId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentDetailsId",
                table: "Bookings");
        }
    }
}
