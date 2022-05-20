using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Replace_Validated_By_Published_Training_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Cfa",
                table: "TrainingStatusType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Published");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Cfa",
                table: "TrainingStatusType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Validated");
        }
    }
}
