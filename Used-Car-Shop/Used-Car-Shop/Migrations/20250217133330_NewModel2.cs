using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Used_Car_Shop.Migrations
{
    /// <inheritdoc />
    public partial class NewModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "FuelType",
                table: "Cars",
                newName: "RemoteControl");

            migrationBuilder.RenameColumn(
                name: "EngineType",
                table: "Cars",
                newName: "PowerSteering");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "Cars",
                newName: "AirConditioner");

            migrationBuilder.AlterColumn<int>(
                name: "price",
                table: "Cars",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemoteControl",
                table: "Cars",
                newName: "FuelType");

            migrationBuilder.RenameColumn(
                name: "PowerSteering",
                table: "Cars",
                newName: "EngineType");

            migrationBuilder.RenameColumn(
                name: "AirConditioner",
                table: "Cars",
                newName: "Color");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
