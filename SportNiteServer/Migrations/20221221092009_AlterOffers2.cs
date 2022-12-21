using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportNiteServer.Migrations
{
    public partial class AlterOffers2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Offers",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Offers",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Offers",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Offers",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }
    }
}
