using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediasAsyncDemo.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medias_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Shield Hero", "Series" },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "Your Name", "Movie" },
                    { new Guid("24810dfc-2d94-4cc7-aab5-cdf98b83f0c9"), "Goblin Slayer", "Series" },
                    { new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), "Fairy Tail", "Series" }
                });

            migrationBuilder.InsertData(
                table: "Medias",
                columns: new[] { "Id", "ChannelId", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "While in the library, college student Naofumi Iwatani finds a fantasy book about \"Four Heroes\"; The Spear, Sword, Bow, and Shield.", "EP 1 The Shield Hero" },
                    { new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "A teenage boy and girl embark on a quest to meet each other for the first time after they magically swap bodies.", "Your Name" },
                    { new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"), new Guid("24810dfc-2d94-4cc7-aab5-cdf98b83f0c9"), "On Priestes first official adventure, she and her party of novices fall victim to murderous goblins.", "EP 1 The Fate of Particular Adventurers" },
                    { new Guid("493c3228-3444-4a49-9cc0-e8532edc59b2"), new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), "When a phony wizard lures Lucy onto his ship with the promise of getting into the guild, her new friends must bail her out.", "EP 1 The Fairy Tail" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_ChannelId",
                table: "Medias",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "Channels");
        }
    }
}
