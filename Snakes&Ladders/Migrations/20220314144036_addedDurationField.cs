using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SnakesAndLadderEvyatar.Migrations
{
    public partial class addedDurationField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "GameDuration",
                table: "Games",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameDuration",
                table: "Games");
        }
    }
}
