using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_Topics_To_Training : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingCategory",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrainingTopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCategory", x => new { x.TrainingId, x.TrainingTopicId });
                    table.ForeignKey(
                        name: "FK_TrainingCategory_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTopic", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainingTopic",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "LanguageCourse" },
                    { 2, "InformationTechnology" },
                    { 3, "SocialScience" },
                    { 4, "School" },
                    { 5, "Health" },
                    { 6, "Communication" },
                    { 7, "Culture" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTopic_Name",
                table: "TrainingTopic",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingCategory");

            migrationBuilder.DropTable(
                name: "TrainingTopic");
        }
    }
}
