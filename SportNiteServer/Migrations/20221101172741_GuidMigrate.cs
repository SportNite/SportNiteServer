using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SportNiteServer.Migrations
{
    public partial class GuidMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Users",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Responses",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfferId",
                table: "Responses",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResponseId",
                table: "Responses",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Offers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfferId",
                table: "Offers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Responses",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "Responses",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseId",
                table: "Responses",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Offers",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "Offers",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }
    }
}
