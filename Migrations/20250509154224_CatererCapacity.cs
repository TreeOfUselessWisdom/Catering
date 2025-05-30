﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Migrations
{
    /// <inheritdoc />
    public partial class CatererCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "AspNetUsers");
        }
    }
}
