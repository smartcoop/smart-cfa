using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_SoftDelete_To_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SoftDeletedAt",
                schema: "Cfa",
                table: "UserChartRevision",
                type: "datetime2(3)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SoftDeletedAt",
                schema: "Cfa",
                table: "Training",
                type: "datetime2(3)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SoftDeletedAt",
                schema: "Cfa",
                table: "Trainer",
                type: "datetime2(3)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChartRevision_SoftDeletedAt",
                schema: "Cfa",
                table: "UserChartRevision",
                column: "SoftDeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Training_SoftDeletedAt",
                schema: "Cfa",
                table: "Training",
                column: "SoftDeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Trainer_SoftDeletedAt",
                schema: "Cfa",
                table: "Trainer",
                column: "SoftDeletedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserChartRevision_SoftDeletedAt",
                schema: "Cfa",
                table: "UserChartRevision");

            migrationBuilder.DropIndex(
                name: "IX_Training_SoftDeletedAt",
                schema: "Cfa",
                table: "Training");

            migrationBuilder.DropIndex(
                name: "IX_Trainer_SoftDeletedAt",
                schema: "Cfa",
                table: "Trainer");

            migrationBuilder.DropColumn(
                name: "SoftDeletedAt",
                schema: "Cfa",
                table: "UserChartRevision");

            migrationBuilder.DropColumn(
                name: "SoftDeletedAt",
                schema: "Cfa",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "SoftDeletedAt",
                schema: "Cfa",
                table: "Trainer");
        }
    }
}
