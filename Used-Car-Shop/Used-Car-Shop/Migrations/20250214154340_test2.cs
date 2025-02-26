using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Used_Car_Shop.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalColor",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "InternalColor",
                table: "Cars",
                newName: "FilePath");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Cars",
                newName: "InternalColor");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "ExternalColor",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
