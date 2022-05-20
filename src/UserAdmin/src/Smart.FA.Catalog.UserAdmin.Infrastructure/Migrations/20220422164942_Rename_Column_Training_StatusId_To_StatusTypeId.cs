using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Rename_Column_Training_StatusId_To_StatusTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingStatus");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Training");

            migrationBuilder.AddColumn<int>(
                name: "StatusTypeId",
                table: "Training",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrainingStatusType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingStatusType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainingStatusType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "PendingValidation" },
                    { 3, "Validated" },
                    { 4, "Cancelled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingStatusType_Name",
                table: "TrainingStatusType",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingStatusType");

            migrationBuilder.DropColumn(
                name: "StatusTypeId",
                table: "Training");

            migrationBuilder.AddColumn<short>(
                name: "StatusId",
                table: "Training",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "TrainingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainingStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "PendingValidation" },
                    { 3, "Validated" },
                    { 4, "Cancelled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingStatus_Name",
                table: "TrainingStatus",
                column: "Name",
                unique: true);
        }
    }
}
