using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_UserChartRevision_To_Trainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserChartRevision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChartRevision", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainerApproval",
                columns: table => new
                {
                    UserChartId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerApproval", x => new { x.TrainerId, x.UserChartId });
                    table.ForeignKey(
                        name: "FK_TrainerApproval_Trainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainerApproval_UserChartRevision_UserChartId",
                        column: x => x.UserChartId,
                        principalTable: "UserChartRevision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerApproval_UserChartId",
                table: "TrainerApproval",
                column: "UserChartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerApproval");

            migrationBuilder.DropTable(
                name: "UserChartRevision");
        }
    }
}
