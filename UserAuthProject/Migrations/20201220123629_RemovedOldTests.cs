using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserAuthProject.Migrations
{
    public partial class RemovedOldTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardData");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropColumn(
                name: "LastRewardDate",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastRewardDate",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "RewardData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Verified = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Command = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Image = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    MaxAmountRange = table.Column<int>(type: "int", nullable: false),
                    MinAmountRange = table.Column<int>(type: "int", nullable: false),
                    RewardRarity = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RewardData_UserId",
                table: "RewardData",
                column: "UserId");
        }
    }
}
