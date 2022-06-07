using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_SoftDeleted_To_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSoftDeleted",
                schema: "Cfa",
                table: "Training",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftDeleted",
                schema: "Cfa",
                table: "Trainer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftDeleted",
                schema: "Cfa",
                table: "UserChartRevision",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserChartRevision_IsSoftDeleted",
                schema: "Cfa",
                table: "UserChartRevision",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Training_IsSoftDeleted",
                schema: "Cfa",
                table: "Training",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Trainer_IsSoftDeleted",
                schema: "Cfa",
                table: "Trainer",
                column: "IsSoftDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserChartRevision_IsSoftDeleted",
                schema: "Cfa",
                table: "UserChartRevision");

            migrationBuilder.DropIndex(
                name: "IX_Training_IsSoftDeleted",
                schema: "Cfa",
                table: "Training");

            migrationBuilder.DropIndex(
                name: "IX_Trainer_IsSoftDeleted",
                schema: "Cfa",
                table: "Trainer");

            migrationBuilder.DropColumn(
                name: "IsSoftDeleted",
                schema: "Cfa",
                table: "UserChartRevision");

            migrationBuilder.DropColumn(
                name: "IsSoftDeleted",
                schema: "Cfa",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "IsSoftDeleted",
                schema: "Cfa",
                table: "Trainer");

        }
    }
}
