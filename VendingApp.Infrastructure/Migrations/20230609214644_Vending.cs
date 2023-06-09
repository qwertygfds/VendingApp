using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VendingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Vending : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendingMachineId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VendingMachines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Shelf = table.Column<int>(type: "integer", nullable: false),
                    PlacesOnShelfCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendingMachines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendingMachineId",
                table: "Products",
                column: "VendingMachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VendingMachines_VendingMachineId",
                table: "Products",
                column: "VendingMachineId",
                principalTable: "VendingMachines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_VendingMachines_VendingMachineId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "VendingMachines");

            migrationBuilder.DropIndex(
                name: "IX_Products_VendingMachineId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VendingMachineId",
                table: "Products");
        }
    }
}
