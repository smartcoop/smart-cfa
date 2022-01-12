using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TrainingDetailLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingDetails_Training_TrainingId",
                table: "TrainingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingDetails",
                table: "TrainingDetails");

            migrationBuilder.RenameTable(
                name: "TrainingDetails",
                newName: "TrainingDetail");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TrainingDetail",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingDetail",
                table: "TrainingDetail",
                columns: new[] { "TrainingId", "Language" });

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingDetail_Training_TrainingId",
                table: "TrainingDetail",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingDetail_Training_TrainingId",
                table: "TrainingDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingDetail",
                table: "TrainingDetail");

            migrationBuilder.RenameTable(
                name: "TrainingDetail",
                newName: "TrainingDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TrainingDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingDetails",
                table: "TrainingDetails",
                columns: new[] { "TrainingId", "Language" });

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingDetails_Training_TrainingId",
                table: "TrainingDetails",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
