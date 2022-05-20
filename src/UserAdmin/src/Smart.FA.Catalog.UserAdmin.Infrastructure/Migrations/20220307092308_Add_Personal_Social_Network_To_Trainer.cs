using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_Personal_Social_Network_To_Trainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainerPersonalNetwork",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    SocialNetwork = table.Column<int>(type: "int", nullable: false),
                    UrlToProfile = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerPersonalNetwork", x => new { x.TrainerId, x.SocialNetwork });
                    table.ForeignKey(
                        name: "FK_TrainerPersonalNetwork_Trainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerPersonalNetwork_SocialNetwork",
                table: "TrainerPersonalNetwork",
                column: "SocialNetwork");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerPersonalNetwork");
        }
    }
}
