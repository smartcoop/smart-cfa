using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update_SocialNetwork_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Cfa",
                table: "SocialNetwork",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "PersonalWebsite");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Cfa",
                table: "SocialNetwork",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Personal Website");
        }
    }
}
