using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Used_Car_Shop.Migrations
{
    /// <inheritdoc />
    public partial class NewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CleanTitle",
                table: "Cars",
                newName: "Gov");

            migrationBuilder.RenameColumn(
                name: "Accident",
                table: "Cars",
                newName: "Body");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gov",
                table: "Cars",
                newName: "CleanTitle");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Cars",
                newName: "Accident");
        }
    }
}
