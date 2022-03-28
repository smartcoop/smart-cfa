using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_Email_To_Trainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TrainingType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Trainer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TrainingStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "PendingValidation");

            migrationBuilder.UpdateData(
                table: "TrainingTopic",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "HealthCare");

            migrationBuilder.InsertData(
                table: "TrainingTopic",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 8, "EconomyMarketing" },
                    { 9, "Sport" }
                });

            migrationBuilder.UpdateData(
                table: "TrainingType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "ScholarTraining");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TrainingTopic",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TrainingTopic",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Trainer");

            migrationBuilder.UpdateData(
                table: "TrainingStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "WaitingForValidation");

            migrationBuilder.UpdateData(
                table: "TrainingTopic",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Health");

            migrationBuilder.UpdateData(
                table: "TrainingType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "SchoolCourse");

            migrationBuilder.InsertData(
                table: "TrainingType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "PermanentSchoolCourse" });
        }
    }
}
