using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Rename_Table_TrainingTopic_To_Topic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingTopic");

            migrationBuilder.RenameColumn(
                name: "TrainingTopicId",
                table: "TrainingCategory",
                newName: "TopicId");

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Topic",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "LanguageCourse" },
                    { 2, "InformationTechnology" },
                    { 3, "SocialScience" },
                    { 4, "School" },
                    { 5, "HealthCare" },
                    { 6, "Communication" },
                    { 7, "Culture" },
                    { 8, "EconomyMarketing" },
                    { 9, "Sport" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topic_Name",
                table: "Topic",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "TrainingCategory",
                newName: "TrainingTopicId");

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
                    { 5, "HealthCare" },
                    { 6, "Communication" },
                    { 7, "Culture" },
                    { 8, "EconomyMarketing" },
                    { 9, "Sport" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTopic_Name",
                table: "TrainingTopic",
                column: "Name",
                unique: true);
        }
    }
}
