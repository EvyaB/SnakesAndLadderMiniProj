using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SnakesAndLadderEvyatar.Migrations
{
    public partial class gamesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentCell_Column",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentCell_Row",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GameStartDateTime",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerGameState",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TurnNumber",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(nullable: false),
                    TurnNumber = table.Column<int>(nullable: false),
                    PlayerPosition_Column = table.Column<int>(nullable: true),
                    PlayerPosition_Row = table.Column<int>(nullable: true),
                    CurrentGameState = table.Column<int>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerId",
                table: "Games",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CurrentGameState_TurnNumber",
                table: "Games",
                columns: new[] { "CurrentGameState", "TurnNumber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.AddColumn<int>(
                name: "CurrentCell_Column",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentCell_Row",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GameStartDateTime",
                table: "Players",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PlayerGameState",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TurnNumber",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
