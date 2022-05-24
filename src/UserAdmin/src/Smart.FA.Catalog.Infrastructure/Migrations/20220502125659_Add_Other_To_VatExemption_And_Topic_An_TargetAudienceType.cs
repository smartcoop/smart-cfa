using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Add_Other_To_VatExemption_And_Topic_An_TargetAudienceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Cfa",
                table: "TargetAudienceType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Other" });

            migrationBuilder.InsertData(
                schema: "Cfa",
                table: "Topic",
                columns: new[] { "Id", "Name" },
                values: new object[] { 10, "Other" });

            migrationBuilder.InsertData(
                schema: "Cfa",
                table: "VatExemptionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Other" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Cfa",
                table: "TargetAudienceType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Cfa",
                table: "Topic",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "Cfa",
                table: "VatExemptionType",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
