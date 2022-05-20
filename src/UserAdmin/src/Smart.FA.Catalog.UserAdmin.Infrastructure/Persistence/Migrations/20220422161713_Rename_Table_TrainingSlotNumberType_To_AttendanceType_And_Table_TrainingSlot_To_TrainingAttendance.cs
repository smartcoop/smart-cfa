using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Rename_Table_TrainingSlotNumberType_To_AttendanceType_And_Table_TrainingSlot_To_TrainingAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingSlot");

            migrationBuilder.DropTable(
                name: "TrainingSlotNumberType");

            migrationBuilder.CreateTable(
                name: "AttendanceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingAttendance",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    AttendanceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingAttendance", x => new { x.TrainingId, x.AttendanceTypeId });
                    table.ForeignKey(
                        name: "FK_TrainingAttendance_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AttendanceType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Group" });

            migrationBuilder.InsertData(
                table: "AttendanceType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Single" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceType_Name",
                table: "AttendanceType",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceType");

            migrationBuilder.DropTable(
                name: "TrainingAttendance");

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

            migrationBuilder.InsertData(
                table: "TrainingSlotNumberType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Group" });

            migrationBuilder.InsertData(
                table: "TrainingSlotNumberType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Single" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSlotNumberType_Name",
                table: "TrainingSlotNumberType",
                column: "Name",
                unique: true);
        }
    }
}
