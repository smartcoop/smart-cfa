using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Rename_Table_TrainingIdentity_To_VatExemptionClaim_And_Table_TrainingType_To_TrainingClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingIdentity");

            migrationBuilder.DropTable(
                name: "TrainingType");

            migrationBuilder.CreateTable(
                name: "VatExemptionClaim",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    VatExemptionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatExemptionClaim", x => new { x.TrainingId, x.VatExemptionTypeId });
                    table.ForeignKey(
                        name: "FK_VatExemptionClaim_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VatExemptionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatExemptionType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VatExemptionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "LanguageCourse" });

            migrationBuilder.InsertData(
                table: "VatExemptionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Professional" });

            migrationBuilder.InsertData(
                table: "VatExemptionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "ScholarTraining" });

            migrationBuilder.CreateIndex(
                name: "IX_VatExemptionType_Name",
                table: "VatExemptionType",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VatExemptionClaim");

            migrationBuilder.DropTable(
                name: "VatExemptionType");

            migrationBuilder.CreateTable(
                name: "TrainingIdentity",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrainingTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingIdentity", x => new { x.TrainingId, x.TrainingTypeId });
                    table.ForeignKey(
                        name: "FK_TrainingIdentity_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainingType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "LanguageCourse" });

            migrationBuilder.InsertData(
                table: "TrainingType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Professional" });

            migrationBuilder.InsertData(
                table: "TrainingType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "ScholarTraining" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingType_Name",
                table: "TrainingType",
                column: "Name",
                unique: true);
        }
    }
}
