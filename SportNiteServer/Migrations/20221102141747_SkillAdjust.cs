using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportNiteServer.Migrations
{
    public partial class SkillAdjust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Level",
                table: "Skills",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "Skills",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Nrtp",
                table: "Skills",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Skills",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Years",
                table: "Skills",
                type: "double",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Nrtp",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Years",
                table: "Skills");

            migrationBuilder.AlterColumn<double>(
                name: "Level",
                table: "Skills",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }
    }
}
