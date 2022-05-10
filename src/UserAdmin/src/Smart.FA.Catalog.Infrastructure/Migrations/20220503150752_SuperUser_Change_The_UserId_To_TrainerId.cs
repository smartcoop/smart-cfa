using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SuperUser_Change_The_UserId_To_TrainerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Cfa");

            migrationBuilder.DropTable("SuperUser", "Cfa");

            migrationBuilder.CreateTable(
                name: "SuperUser",
                schema: "Cfa",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperUser", x => x.TrainerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SuperUser", "Cfa");

            migrationBuilder.CreateTable(
                name: "SuperUser",
                schema: "Cfa",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperUser", x => x.UserId);
                });
        }
    }
}
