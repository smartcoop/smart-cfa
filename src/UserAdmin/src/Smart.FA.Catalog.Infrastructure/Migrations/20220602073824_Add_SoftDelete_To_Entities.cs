using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_SoftDelete_To_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Cfa",
                table: "UserChartRevision",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Cfa",
                table: "Training",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Cfa",
                table: "Trainer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Cfa",
                table: "UserChartRevision");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Cfa",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Cfa",
                table: "Trainer");
        }
    }
}
