using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportNiteServer.Migrations
{
    public partial class AlterOffers5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Offers",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Offers",
                type: "longtext",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Offers");
        }
    }
}
