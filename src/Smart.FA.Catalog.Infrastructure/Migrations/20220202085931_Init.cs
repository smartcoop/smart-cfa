using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    DefaultLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ApplicationType = table.Column<int>(type: "int", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSlotNumberType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSlotNumberType", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "TrainerEnrollment",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerEnrollment", x => new { x.TrainingId, x.TrainerId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_TrainerEnrollment_Trainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainerEnrollment_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingDetail",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "NCHAR(2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Goal = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    Methodology = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true)
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
                name: "TrainingSlot",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrainingSlotTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSlot", x => new { x.TrainingId, x.TrainingSlotTypeId });
                    table.ForeignKey(
                        name: "FK_TrainingSlot_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTarget",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrainingTargetAudienceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTarget", x => new { x.TrainingId, x.TrainingTargetAudienceId });
                    table.ForeignKey(
                        name: "FK_TrainingTarget_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TrainingSlotNumberType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Group" },
                    { 2, "Single" }
                });

            migrationBuilder.InsertData(
                table: "TrainingStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "WaitingForValidation" },
                    { 3, "Validated" },
                    { 4, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "TrainingType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "LanguageCourse" },
                    { 2, "Professional" },
                    { 3, "SchoolCourse" },
                    { 4, "PermanentSchoolCourse" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerEnrollment_TrainerId",
                table: "TrainerEnrollment",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSlotNumberType_Name",
                table: "TrainingSlotNumberType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingStatus_Name",
                table: "TrainingStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingType_Name",
                table: "TrainingType",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerEnrollment");

            migrationBuilder.DropTable(
                name: "TrainingDetail");

            migrationBuilder.DropTable(
                name: "TrainingIdentity");

            migrationBuilder.DropTable(
                name: "TrainingSlot");

            migrationBuilder.DropTable(
                name: "TrainingSlotNumberType");

            migrationBuilder.DropTable(
                name: "TrainingStatus");

            migrationBuilder.DropTable(
                name: "TrainingTarget");

            migrationBuilder.DropTable(
                name: "TrainingType");

            migrationBuilder.DropTable(
                name: "Trainer");

            migrationBuilder.DropTable(
                name: "Training");
        }
    }
}
