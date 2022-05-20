using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Rename_Table_TrainingDetail_To_TrainingLocalizedDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingDetail");

            migrationBuilder.CreateTable(
                name: "TrainingLocalizedDetails",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "NCHAR(2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Methodology = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PracticalModalities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingLocalizedDetails", x => new { x.TrainingId, x.Language });
                    table.ForeignKey(
                        name: "FK_TrainingLocalizedDetails_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingLocalizedDetails");

            migrationBuilder.CreateTable(
                name: "TrainingDetail",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "NCHAR(2)", nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Methodology = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PracticalModalities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingDetail", x => new { x.TrainingId, x.Language });
                    table.ForeignKey(
                        name: "FK_TrainingDetail_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
