﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Used_Car_Shop.Migrations
{
    /// <inheritdoc />
    public partial class AddingColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cars");
        }
    }
}
