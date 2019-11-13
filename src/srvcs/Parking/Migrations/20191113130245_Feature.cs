using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking.Migrations
{
    public partial class Feature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                schema: "Parking",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                schema: "Parking",
                table: "Models",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Folder",
                schema: "Parking",
                table: "Cars",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                schema: "Parking",
                table: "Models");

            migrationBuilder.AlterColumn<string>(
                name: "Folder",
                schema: "Parking",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                schema: "Parking",
                table: "Cars",
                nullable: false,
                defaultValue: 0);
        }
    }
}
