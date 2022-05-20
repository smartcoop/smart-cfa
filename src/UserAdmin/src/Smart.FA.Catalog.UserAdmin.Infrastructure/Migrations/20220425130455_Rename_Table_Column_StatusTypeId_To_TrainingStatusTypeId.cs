using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Rename_Table_Column_StatusTypeId_To_TrainingStatusTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingStatusType");

            migrationBuilder.RenameColumn(
                name: "StatusTypeId",
                table: "Training",
                newName: "TrainingStatusTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainingStatusTypeId",
                table: "Training",
                newName: "StatusTypeId");

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
    }
}
