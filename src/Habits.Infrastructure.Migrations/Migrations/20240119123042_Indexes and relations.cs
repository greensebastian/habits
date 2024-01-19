using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habits.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Indexesandrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HabitId",
                table: "LogEntries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PerformedAt",
                table: "LogEntries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "DirectionId",
                table: "Habits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "End",
                table: "Habits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Start",
                table: "Habits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                table: "Directions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_HabitId_PerformedAt",
                table: "LogEntries",
                columns: new[] { "HabitId", "PerformedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Habits_DirectionId_Start_End",
                table: "Habits",
                columns: new[] { "DirectionId", "Start", "End" });

            migrationBuilder.CreateIndex(
                name: "IX_Directions_UserProfileId_Start_End",
                table: "Directions",
                columns: new[] { "UserProfileId", "Start", "End" });

            migrationBuilder.AddForeignKey(
                name: "FK_Directions_UserProfiles_UserProfileId",
                table: "Directions",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_Directions_DirectionId",
                table: "Habits",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntries_Habits_HabitId",
                table: "LogEntries",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directions_UserProfiles_UserProfileId",
                table: "Directions");

            migrationBuilder.DropForeignKey(
                name: "FK_Habits_Directions_DirectionId",
                table: "Habits");

            migrationBuilder.DropForeignKey(
                name: "FK_LogEntries_Habits_HabitId",
                table: "LogEntries");

            migrationBuilder.DropIndex(
                name: "IX_LogEntries_HabitId_PerformedAt",
                table: "LogEntries");

            migrationBuilder.DropIndex(
                name: "IX_Habits_DirectionId_Start_End",
                table: "Habits");

            migrationBuilder.DropIndex(
                name: "IX_Directions_UserProfileId_Start_End",
                table: "Directions");

            migrationBuilder.DropColumn(
                name: "HabitId",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "PerformedAt",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "DirectionId",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Directions");
        }
    }
}
