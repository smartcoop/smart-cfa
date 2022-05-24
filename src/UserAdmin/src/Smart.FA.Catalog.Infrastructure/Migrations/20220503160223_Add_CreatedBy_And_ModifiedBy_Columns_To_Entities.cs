using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_CreatedBy_And_ModifiedBy_Columns_To_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "Cfa",
                table: "UserChartRevision",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                schema: "Cfa",
                table: "UserChartRevision",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "Cfa",
                table: "Training",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                schema: "Cfa",
                table: "Training",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "Cfa",
                table: "Trainer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                schema: "Cfa",
                table: "Trainer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Cfa",
                table: "UserChartRevision");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Cfa",
                table: "UserChartRevision");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Cfa",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Cfa",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Cfa",
                table: "Trainer");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Cfa",
                table: "Trainer");
        }
    }
}
