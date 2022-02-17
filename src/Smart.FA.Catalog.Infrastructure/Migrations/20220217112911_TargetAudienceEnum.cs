using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TargetAudienceEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingTargetAudience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTargetAudience", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainingTargetAudience",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Employee" });

            migrationBuilder.InsertData(
                table: "TrainingTargetAudience",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Student" });

            migrationBuilder.InsertData(
                table: "TrainingTargetAudience",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Unemployed" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTargetAudience_Name",
                table: "TrainingTargetAudience",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingTargetAudience");
        }
    }
}
