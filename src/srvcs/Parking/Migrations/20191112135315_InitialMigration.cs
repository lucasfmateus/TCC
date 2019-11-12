using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Parking");

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                schema: "Parking",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                schema: "Parking",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DistDoor = table.Column<double>(nullable: false),
                    IsBusy = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                schema: "Parking",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                schema: "Parking",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ManufacturerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "Parking",
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlotTypes",
                schema: "Parking",
                columns: table => new
                {
                    SlotId = table.Column<string>(nullable: false),
                    TypeId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotTypes", x => new { x.SlotId, x.TypeId });
                    table.ForeignKey(
                        name: "FK_SlotTypes_Slots_SlotId",
                        column: x => x.SlotId,
                        principalSchema: "Parking",
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SlotTypes_Types_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Parking",
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                schema: "Parking",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    ModelId = table.Column<string>(nullable: false),
                    TypeId = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Folder = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Models_ModelId",
                        column: x => x.ModelId,
                        principalSchema: "Parking",
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_Types_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Parking",
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parked",
                schema: "Parking",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    CarId = table.Column<string>(nullable: true),
                    SlotId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parked", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parked_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "Parking",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parked_Slots_SlotId",
                        column: x => x.SlotId,
                        principalSchema: "Parking",
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ModelId",
                schema: "Parking",
                table: "Cars",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TypeId",
                schema: "Parking",
                table: "Cars",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_ManufacturerId",
                schema: "Parking",
                table: "Models",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Parked_CarId",
                schema: "Parking",
                table: "Parked",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Parked_SlotId",
                schema: "Parking",
                table: "Parked",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_SlotTypes_TypeId",
                schema: "Parking",
                table: "SlotTypes",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parked",
                schema: "Parking");

            migrationBuilder.DropTable(
                name: "SlotTypes",
                schema: "Parking");

            migrationBuilder.DropTable(
                name: "Cars",
                schema: "Parking");

            migrationBuilder.DropTable(
                name: "Slots",
                schema: "Parking");

            migrationBuilder.DropTable(
                name: "Models",
                schema: "Parking");

            migrationBuilder.DropTable(
                name: "Types",
                schema: "Parking");

            migrationBuilder.DropTable(
                name: "Manufacturers",
                schema: "Parking");
        }
    }
}
