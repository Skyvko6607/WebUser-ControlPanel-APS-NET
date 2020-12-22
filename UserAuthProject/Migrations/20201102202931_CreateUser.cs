using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserAuthProject.Migrations
{
    public partial class CreateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    MinAmountRange = table.Column<int>(nullable: false),
                    MaxAmountRange = table.Column<int>(nullable: false),
                    RewardRarity = table.Column<int>(nullable: false),
                    Command = table.Column<string>(nullable: true),
                    Server = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordEncrypted = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    LastRewardDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Verified = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_RewardData_UserId",
                table: "RewardData",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardData");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
