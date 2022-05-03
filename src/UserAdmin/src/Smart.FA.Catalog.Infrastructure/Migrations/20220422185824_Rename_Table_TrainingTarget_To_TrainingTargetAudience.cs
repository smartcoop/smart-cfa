using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Rename_Table_TrainingTarget_To_TrainingTargetAudience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingTarget");

            migrationBuilder.CreateTable(
                name: "TrainingTargetAudience",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TargetAudienceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTargetAudience", x => new { x.TrainingId, x.TargetAudienceTypeId });
                    table.ForeignKey(
                        name: "FK_TrainingTargetAudience_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingTargetAudience");

            migrationBuilder.CreateTable(
                name: "TrainingTarget",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TargetAudienceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTarget", x => new { x.TrainingId, x.TargetAudienceTypeId });
                    table.ForeignKey(
                        name: "FK_TrainingTarget_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
