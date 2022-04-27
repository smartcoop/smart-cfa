using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Change_Default_Schema_To_Cfa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Cfa");

            migrationBuilder.RenameTable(
                name: "VatExemptionType",
                newName: "VatExemptionType",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "VatExemptionClaim",
                newName: "VatExemptionClaim",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "TrainingTopic",
                newName: "TrainingTopic",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "TrainingTargetAudience",
                newName: "TrainingTargetAudience",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "TrainingLocalizedDetails",
                newName: "TrainingLocalizedDetails",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "TrainingAttendance",
                newName: "TrainingAttendance",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "Training",
                newName: "Training",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "TrainerSocialNetwork",
                newName: "TrainerSocialNetwork",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "TrainerAssignment",
                newName: "TrainerAssignment",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "Trainer",
                newName: "Trainer",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Topic",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "TargetAudienceType",
                newName: "TargetAudienceType",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "SocialNetwork",
                newName: "SocialNetwork",
                newSchema: "Cfa");

            migrationBuilder.RenameTable(
                name: "AttendanceType",
                newName: "AttendanceType",
                newSchema: "Cfa");

            migrationBuilder.CreateTable(
                name: "TrainingStatusType",
                schema: "Cfa",
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
                schema: "Cfa",
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
                schema: "Cfa",
                table: "TrainingStatusType",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingStatusType",
                schema: "Cfa");

            migrationBuilder.RenameTable(
                name: "VatExemptionType",
                schema: "Cfa",
                newName: "VatExemptionType");

            migrationBuilder.RenameTable(
                name: "VatExemptionClaim",
                schema: "Cfa",
                newName: "VatExemptionClaim");

            migrationBuilder.RenameTable(
                name: "TrainingTopic",
                schema: "Cfa",
                newName: "TrainingTopic");

            migrationBuilder.RenameTable(
                name: "TrainingTargetAudience",
                schema: "Cfa",
                newName: "TrainingTargetAudience");

            migrationBuilder.RenameTable(
                name: "TrainingLocalizedDetails",
                schema: "Cfa",
                newName: "TrainingLocalizedDetails");

            migrationBuilder.RenameTable(
                name: "TrainingAttendance",
                schema: "Cfa",
                newName: "TrainingAttendance");

            migrationBuilder.RenameTable(
                name: "Training",
                schema: "Cfa",
                newName: "Training");

            migrationBuilder.RenameTable(
                name: "TrainerSocialNetwork",
                schema: "Cfa",
                newName: "TrainerSocialNetwork");

            migrationBuilder.RenameTable(
                name: "TrainerAssignment",
                schema: "Cfa",
                newName: "TrainerAssignment");

            migrationBuilder.RenameTable(
                name: "Trainer",
                schema: "Cfa",
                newName: "Trainer");

            migrationBuilder.RenameTable(
                name: "Topic",
                schema: "Cfa",
                newName: "Topic");

            migrationBuilder.RenameTable(
                name: "TargetAudienceType",
                schema: "Cfa",
                newName: "TargetAudienceType");

            migrationBuilder.RenameTable(
                name: "SocialNetwork",
                schema: "Cfa",
                newName: "SocialNetwork");

            migrationBuilder.RenameTable(
                name: "AttendanceType",
                schema: "Cfa",
                newName: "AttendanceType");
        }
    }
}
