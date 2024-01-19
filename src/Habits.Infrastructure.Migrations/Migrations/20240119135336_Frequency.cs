using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habits.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Frequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "Habits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Habits",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Habits");
        }
    }
}
